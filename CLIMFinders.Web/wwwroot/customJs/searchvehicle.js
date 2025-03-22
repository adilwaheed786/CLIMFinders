$(() => {
    $(".table-responsive").hide();
    var table = $('#vehiclesTable').DataTable({
        "processing": true,
        "serverSide": false,
        "columns": [
            { "data": "id", "orderable": false, "visible": false },
            { "data": "vin" },
            { "data": "make" },
            { "data": "model" },
            { "data": "color" },
            { "data": "year" },
            { "data": "contactInformation" },
            { "data": "impoundFees" },
            { "data": "locationDetails" },
            { "data": "boundStatus" },
            { "data": "companyName" },
            {
                "data": "pickedOn",
                "render": function (data, type, row) {
                    if (!data) return ""; // Handle empty dates
                    let date = new Date(data);
                    return date.toLocaleDateString('en-US', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit',
                        hourCycle: 'h23'
                    });
                }
            }
        ],
        "order": [[1, "asc"]], // Default sorting by VIN
        "lengthMenu": [10, 25, 50, 100] // Number of records per page
    });

    //$("#searchButton").on("click", function (e) {
    //    let vin = $("#vinInput").val();
    //    $("#errorMessage").hide();

    //    if (vin === "") {
    //        $("#errorMessage").text("Please enter a VIN").show();
    //        return;
    //    }
    //    else {
    //        $.ajax({
    //            url: '/api/search/searchbyvin?vin=' + vin,
    //            type: "GET",
    //            success: function (response) {
    //                table.clear().rows.add(response.data).draw(); // Clear and reload data
    //            },
    //            error: function (xhr) {
    //                alert(xhr.responseJSON?.message || "No vehicle found.");
    //                table.clear().draw(); // Clear table if no data found
    //            }
    //        });
    //    }
    //});

    $("#searchButton").on("click", function (e) {
        e.preventDefault(); // Prevent form submission if inside a form

        let vin = $("#vinInput").val().trim();
        $("#errorMessage").hide();


        if (!vin) {
            $("#errorMessage").text("Please enter a VIN").show();
            return;
        }
        $.ajax({
            url: '/api/search/searchbyvin?vin=' + vin,
            type: "GET",
            contentType: 'application/json',
            success: function (response) {
                table.clear().draw();

                if (response && response.data) {
                    var status = response.data.status;
                    if (status == "403") {
                        window.location.href = "/SubscriptionRenew";
                    }
                    else {
                        $(".table-responsive").show();
                        var data = response.data.result;
                        table.clear().rows.add(data).draw();
                    }
                }
            },
            error: function (xhr) {
                console.log(JSON.stringify(xhr));
                let errorMessage = "No vehicle found.";

                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                } else if (xhr.status === 403) {
                    errorMessage = "Access denied. Please renew your subscription.";
                } else if (xhr.status === 500) {
                    errorMessage = "Server error. Please try again later.";
                }

                table.clear().draw(); // Ensure table is cleared on error
            }
        });
    });
    $("#uploadButton").on("click", function (e) {
        var form = document.getElementById("uploadForm");
        if (!form.checkValidity()) {
            form.classList.add("was-validated");
            return;
        } 
        var formData = new FormData();
        formData.append("Name", $("#nameInput").val());
        formData.append("Email", $("#emailInput").val());
        formData.append("Details", $("#detailsInput").val());
        formData.append("VIN", $("#vinInputValue").val());

        var fileInput = document.getElementById("fileInput");
        if (fileInput.files.length > 0) {
            formData.append("Attachment", fileInput.files[0]);
        } else {
            alert("Please select a file.");
            return;
        }

        $.ajax({
            url: "/api/search/uploadfile",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                alert("Email sent successfully with the document!");
                $("#uploadModal").modal("hide"); // Hide modal after successful upload
                $("#uploadForm")[0].reset(); // Clear form fields
            },
            error: function (xhr) {
                console.log(xhr);
                alert("Failed to send email.");
            }
        });
    });
});
