// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let passangers = 1;
let dateDeparture;
let dateReturn;
let oldReturn;
$(document).ready(function () {
    
    let today = new Date().toISOString().slice(0, 10);
    let newDate = new Date();
    newDate.setDate(newDate.getDate() + 1);
    let tomorrow = newDate.toISOString().slice(0, 10);
    $("#DateFrom").attr({ "min": today });
    $("#DateTo").attr({ "min": tomorrow });

    $("#DateFrom").change(function () {
        oldReturn = new Date($("#DateTo").val());
        dateReturn = new Date($(this).val());
        dateReturn.setDate(dateReturn.getDate() + 1);
          
            dateDeparture = new Date($("#DateFrom").val());
        if (dateDeparture > oldReturn) {
                $("#DateTo").val("");
            }
            $("#DateTo").attr({ "min": dateReturn.toISOString().slice(0, 10) });
        
    });
    
    $("#Way").on("change", function () {
        if ($(this).val() == 0) {
            $("#DateTo").attr("disabled", true);
            $("#DateTo").val("");
            $("input").prop('required', false);
        } else {
            $("#DateTo").attr("disabled", false);
            $("input").prop('required', true);
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

    $("#From").on("keyup", function () {
        $.ajax({
            url: "/Home/GetLocations",
            method: "GET",
            data: { Name: $(this).val() },
            dataType: "json",
            success: function (data) {
                let suggestions = `<datalist id="locationsFrom">`;
                let airports = JSON.parse(data);

                for (let a = 0; a < 10; a++) {
                    suggestions += `<option value="${airports[a]["AirportLocation"]}">`;
                }
                suggestions += `</datalist>`;
                $("#FromSuggestions").html(suggestions);
            
            },
            error: function (err) {
                conssole.log(err);
            }
        });
       
    });



    $("#To").on("keyup", function () {
        $.ajax({
            url: "/Home/GetLocations",
            method: "GET",
            data: { Name: $(this).val() },
            dataType: "json",
            success: function (data) {
                let suggestions = `<datalist id="locationsTo">`;
                let airports = JSON.parse(data);

                for (let a = 0; a < 10; a++) {
                    suggestions += `<option value="${airports[a]["AirportLocation"]}">`;
                }
                suggestions += `</datalist>`;
                $("#ToSuggestions").html(suggestions);
             
            },
            error: function (err) {
                conssole.log(err);
            }
        });
    });
    

});