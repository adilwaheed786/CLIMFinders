var stripe = Stripe("pk_test_5104HR54ZriKXbh6h0S1vNDz3fKVbQIaI1ZmD5C5eVVvfp0LYdV5Y9tg2WMmPZrYfcPgHEOa0d9uHgmmWDFiaWcKX00K39qkg6J"); // Replace with your Stripe Publishable Key

function ProcessPayment() {
    var request = JSON.stringify({ plan: $('#hdnRole').val(), name: $('#fullName').val(), email: $('#email').val(), subRoleId: $("input[name='subRole']:checked").val() });
    console.log(request);

    $.ajax({
        url: '/api/SubscriptionPlan/PostSubscription',
        type: 'POST',
        contentType: 'application/json',
        data: request,
        success: function (data) {
            if (!data) {
                console.error("Empty response from server");
                return;
            }

            console.log("Subscription request successful:", data);

            // Assuming the server returns the Stripe Checkout session URL
            if (data.sessionUrl) {
                // Redirect to Stripe Checkout page
                data.sessionUrl == "N" ? $(".text-danger").html("Email already exists") : window.location.href = data.sessionUrl;
            } else {
                console.error("Stripe session URL not found in response.");
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX error:", error);
        }
    });
}


$("#validationForm").on("submit", function (e) {
    e.preventDefault();
    ProcessPayment();
});


$(() => {
    $('#exampleModal').on('show.bs.modal', function (event) {
        var label = $(event.relatedTarget); // Button that triggered the modal
        var modal = $(this); // The modal itself
        var plan = JSON.parse(label.attr("data-whatever") || "{}"); // Ensure valid JSON
        console.log(plan);

        // Reset input fields
        $("#fullName").val("");
        $("#email").val("");
        $(".text-danger").html("");
        var modalTitle = modal.find('.modal-title'); // Fix jQuery selection

        if (plan.id === 1) {
            $("#hdnRole").val("user");
            modal.find('.modal-body #dvSubRole').hide();
            modalTitle.text("User Subscription"); // Fix textContent usage
            $(".lblName").text("Full Name");
            $("#fullName").attr("placeholder", "Enter Full Name"); // Use attr() for placeholder
        } else {
            modal.find('.modal-body #hdnRole').val("business");
            modal.find('.modal-body #dvSubRole').show();
            modalTitle.text("Business Subscription"); // Fix textContent usage
            $(".lblName").text("Company Name");
            $("#fullName").attr("placeholder", "Enter Company Name");
        }
    });
});

