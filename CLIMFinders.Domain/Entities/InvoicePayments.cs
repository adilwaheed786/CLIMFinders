using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Domain.Entities
{
    public class InvoicePayments : BaseEntity
    {
        public int Id { get; set; }  
        public int UserId { get; set; }  
        public string InvoiceId { get; set; }  
        public string PaymentIntentId { get; set; }  
        public string SessionId { get; set; } 
        public string HostedInvoiceUrl { get; set; }  
    }

}
