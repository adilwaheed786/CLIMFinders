$(document).ready(function () {
    $("#Input_MakeId").change(function () {
        var makeId = $(this).val();
        BindDropDown(makeId);
    });

    function BindDropDown(makeId) {
        if (makeId) {
            $.ajax({
                url: "../api/ajax/GetVehicleModel/" + makeId,
                type: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest" // Prevents 302 redirect
                },
                success: function (data) {
                    $("#Input_ModelId").empty().append('<option value="">-- Choose One --</option>');
                    $.each(data, function (index, item) {
                        $("#Input_ModelId").append('<option value="' + item.value + '">' + item.text + '</option>');
                    });
                }
            });
        } else {
            $("#Input_ModelId").empty().append('<option value="">-- Choose One --</option>');
        }
    }
});