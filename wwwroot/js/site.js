// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let passangers = 1;
$(document).ready(function () {
    $("#Way").on("change", function () {
        if ($(this).val() == 0) {
            $("#DateTo").attr("disabled", true);
            $("#DateTo").val("");
        } else {
            $("#DateTo").attr("disabled", false);
        } 
    });
    calculatePassangers();
  
    function calculatePassangers(adults = 1, children = 0, infant = 0) {
        let totalAdultsOptions = "";
        let totalChildrenOptions = "";
        let totalInfantOptions = "";

        for (let ad = 0; ad <= 9 - children - infant; ad++) {
            if (ad > 0)
                totalAdultsOptions += `<option value="${ad}">${ad}</option>`;
        }
        $("#Adults").html(totalAdultsOptions);

        for (let ch = 0; ch <= 9 - adults - infant; ch++) {
                totalChildrenOptions += `<option value="${ch}">${ch}</option>`;
        }
        $("#Children").html(totalChildrenOptions);

        for (let inf = 0; inf <= 9 - adults - children && inf <= adults; inf++) {
            totalInfantOptions += `<option value="${inf}">${inf}</option>`;
        }
        $("#Infant").html(totalInfantOptions);

        $("#Adults").find(`option[value=${adults}]`).attr('selected', 'selected');
        $("#Children").find(`option[value=${children}]`).attr('selected', 'selected');
        $("#Infant").find(`option[value=${infant}]`).attr('selected', 'selected');
        }
    $("#Adults").on("change", function () {
        calculatePassangers($("#Adults").val(), $("#Children").val(), $("#Infant").val());
    });
    $("#Children").on("change", function () {
        calculatePassangers($("#Adults").val(), $("#Children").val(), $("#Infant").val());
    });
    $("#Infant").on("change", function () {
        calculatePassangers($("#Adults").val(), $("#Children").val(), $("#Infant").val());
    });

  /*  $("#From").on("keyup change", function () {
        let suggestionsFrom = "";
        for(let sugFrom = 0; sugFrom< 10; sugFrom++) {
        suggestionsFrom += `<option value="${}">`;
    }
        $("#locationsFrom").html(suggestionsFrom);
    });
    */

});