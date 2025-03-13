using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Enums;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.StripeProcess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Web;

namespace CLIMFinders.StripeProcess
{
    public class SubscriptionPlanServices(IConfiguration configuration, IStripeClient _stripeClient, IEmailService emailService,
        IRegisterService registerService) : ISubscriptionPlanServices
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IStripeClient stripeClient = _stripeClient;
        private readonly IRegisterService _registerService = registerService;
        private readonly IEmailService _emailService = emailService;


        public string SubscripePlan(SubscriptionRequest plan)
        {
            if (_registerService.IsUserExists(plan.Email, plan.Id))
            {
                return "N";
            }
            StripeConfiguration.ApiKey = stripeClient.ApiKey;

            var customerService = new CustomerService();

            // Check if customer already exists (to prevent duplicates)
            var customers = customerService.List(new CustomerListOptions { Email = plan.Email });
            string? customerId = customers.Data.Count > 0 ? customers.Data[0].Id : null;

            if (customerId == null)
            {
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = plan.Email,
                    Name = plan.Name,
                });
                customerId = customer.Id;
            }
            else
            {
                customerService.Update(customerId, new CustomerUpdateOptions
                {
                    Email = plan.Email,
                    Name = plan.Name,
                });
            }

            // Get the corresponding Price ID from appsettings.json
            string? priceId = plan.Plan.ToLower() switch
            {
                "user" => _configuration["Stripe:UserPriceId"],
                "business" => _configuration["Stripe:BusinessPriceId"],
                _ => null
            };

            var domain = _configuration["JwtSettings:Issuer"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                new() {Price = priceId,Quantity = 1}
                },
                Mode = "subscription",
                Customer = customerId,
                CustomerUpdate = new SessionCustomerUpdateOptions
                {
                    Name = "auto"
                },
                SuccessUrl = $"{domain}/SubscriptionSuccess?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/SubscriptionCancel",
                Metadata = new Dictionary<string, string>
                {
                { "RoleId", plan.Plan.Equals("user", StringComparison.CurrentCultureIgnoreCase) ? "1": "2"  },
                { "SubRoleId", plan.SubRoleId }
                }
            };

            var sessionService = new SessionService(stripeClient);
            var session = sessionService.Create(options);
            return session.Url;
        }
        public void SendInvoiceOnSubscriptionSuccess(string sessionId, int UserId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);
            var invoiceService = new InvoiceService();

            // Get the latest invoice for this subscription
            var invoices = invoiceService.List(new InvoiceListOptions
            {
                Subscription = session.SubscriptionId,
                Limit = 1
            });

            if (invoices.Data.Count > 0)
            {
                var invoice = invoices.Data[0]; // Get most recent invoice

                if (invoice.Status == "paid") // Ensure it's already paid
                {
                    Console.WriteLine($"Invoice URL: {invoice.HostedInvoiceUrl}");
                    PersonInfoDto personInfo = new()
                    {
                        Email = invoice.CustomerEmail,
                        Name = invoice.CustomerName
                    };
                    if (UserId == 0)
                    {
                        var RoleId = Convert.ToInt32(session.Metadata["RoleId"]);
                        var SubRoleId = Convert.ToInt32(session.Metadata["SubRoleId"]);
                        SubscriptionDto subscription = new()
                        {
                            SessionId = sessionId,
                            SubscriptionId = session.SubscriptionId,
                            TierId = RoleId,
                        };
                        _registerService.SaveUser(personInfo, RoleId, SubRoleId, subscription);
                    }
                    else
                    {
                        _registerService.UpdateSubscription(sessionId, UserId);
                    }
                    //return invoice.HostedInvoiceUrl;
                    _emailService.SendEmail(personInfo.Email, "Your Invoice - Payment Successful", $"<p>Thank you for your payment!</p><p>You can download your invoice here: <a href='{invoice.HostedInvoiceUrl}'>View Invoice</a></p>", true);
                }
                else
                {
                    throw new Exception("Invoice is not yet paid.");
                }
            }
            else
            {
                throw new Exception("No invoice found for this subscription.");
            }
        }
        public bool IsSubscriptionActive(string subscriptionId)
        {
            StripeConfiguration.ApiKey = stripeClient.ApiKey;
            var service = new SubscriptionService();
            var subscription = service.Get(subscriptionId);
            return (subscription.Status == "active");
            //return (subscription.Status == "canceled" || 
            //    subscription.Status == "incomplete_expired" || 
            //    subscription.Status == "canceled" ||
            //    subscription.Status == "unpaid" ||
            //    subscription.Status == "past_due" ||
            //    subscription.Status == "incomplete");  
        }
        public Subscription GetSubscriptionBySessionId(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            // Now fetch the subscription associated with the customer ID from the session
            var subscriptionService = new SubscriptionService();
            var subscriptions = subscriptionService.List(new SubscriptionListOptions
            {
                Customer = session.CustomerId,  // Get the customer ID from the session
                Limit = 1 // Fetch the most recent subscription
            });

            return subscriptions.FirstOrDefault(); // Return the most recent subscription
        }
        public SubscriptionDetail GetSubscriptionById(string subscriptionId)
        {
            StripeConfiguration.ApiKey = stripeClient.ApiKey;
            var service = new SubscriptionService();
            var subscription = service.Get(subscriptionId);

            SubscriptionDetail detail = new()
            {
                Amount = subscription.Items.Data[0].Plan.Amount / 100,
                NextPaymentDate = subscription.CurrentPeriodStart.ToString("MMMM dd, yyyy") +" - "+ subscription.CurrentPeriodEnd.ToString("MMMM dd, yyyy"),
                SubscriptionId = subscriptionId,
                PriceModel = subscription.Items.Data[0].Plan.Id,
                Status = subscription.Status
            };

            if (subscription != null && subscription.Items.Data.Count != 0)
            {
                string productId = subscription.Items.Data[0].Plan.ProductId;
                string productName = GetProductName(productId);
                detail.Plan = productName;
                subscription.Metadata["ProductName"] = productName;
            }
            return detail;
        }
        // Generate a checkout session link for renewing a subscription
        public string CreateRenewalCheckoutSession(string sessionId, string plan, int UserId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var domain = _configuration["JwtSettings:Issuer"];

            Dictionary<string, string> metadata = session.Metadata;

            //var queryParams = new Dictionary<string, string?>
            //{
            // { "session_id", "{{CHECKOUT_SESSION_ID}}" },
            //  { "for_id", UserId.ToString() }
            //}; 
            //string successUrl = QueryHelpers.AddQueryString($"{domain}/SubscriptionSuccess", queryParams);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card"],
                Mode = "subscription",
                Customer = session.CustomerId, // Attach the existing customer
                LineItems =
            [
                new() {
                    Price = plan, // The price ID for the subscription plan
                    Quantity = 1,
                },
            ],
                SuccessUrl = $"{domain}/SubscriptionSuccess?session_id={{CHECKOUT_SESSION_ID}}&for_id="+ UserId,
                CancelUrl = $"{domain}/SubscriptionCancel",
                Metadata = metadata
            };

            var sessionrenew = sessionService.Create(options);

            return sessionrenew.Url; // Return the checkout session link
        }

        public bool CancelSubscription(string subscriptionId)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeClient.ApiKey;

                var service = new SubscriptionService();
                var subscription = service.Cancel(subscriptionId);
                _emailService.SendEmail(subscription.Customer.Email, "Subscription Canceled", "<p>We’re sorry to see you go! Your subscription has been canceled, effective <strong>"+DateTime.Now.ToString("MMMM dd, yyyy") + "</strong>.</p>", true);

                return subscription.Status == "canceled";
            }
            catch
            {
                return false;
            }
        }
        string GetProductName(string productId)
        {
            var productService = new ProductService();
            var product = productService.Get(productId);
            return product?.Name ?? "Unknown Product";
        }
        public Subscription GetSubscriptionByCustomerId(string customerId)
        {
            var options = new SubscriptionListOptions
            {
                Customer = customerId,
                Limit = 1 // Fetch the most recent subscription
            };

            var service = new SubscriptionService();
            var subscriptions = service.List(options);
            return subscriptions.FirstOrDefault();
        }
    }
}
