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
        if ($(this).val() == "oneway") {
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
                    let airports = JSON.parse(data);
                    if(airports.length != 0) {
                        let suggestions = `<datalist id="locationsFrom">`;
                        for (let a = 0; a < 10; a++) {
                            suggestions += `<option value="${airports[a]["AirportLocation"]}">`;
                        }
                        suggestions += `</datalist>`;
                        $("#FromSuggestions").html(suggestions);
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        
    });



    $("#To").on("keyup", function () {
        
            $.ajax({
                url: "/Home/GetLocations",
                method: "GET",
                data: { Name: $(this).val() },
                dataType: "json",
                success: function (data2) {
                    let airports2 = JSON.parse(data2);
                    if(airports2.length != 0) {
                        let suggestions2 = `<datalist id="locationsTo">`;
                        for (let b = 0; b < 10; b++) {
                            suggestions2 += `<option value="${airports2[b]["AirportLocation"]}">`;
                        }
                        suggestions2 += `</datalist>`;
                        $("#ToSuggestions").html(suggestions2);
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        
      
    });
    $("#submitSearch").submit(function () {
        if ($(this).valid()) {
            $("#runSearch").html(`<div class="card col-8 searchResults">
    <div class="card-body" id="results">
<div class="sk-chase">
    <div class="sk-chase-dot"></div>
    <div class="sk-chase-dot"></div>
    <div class="sk-chase-dot"></div>
    <div class="sk-chase-dot"></div>
    <div class="sk-chase-dot"></div>
    <div class="sk-chase-dot"></div>
</div>
    </div>
</div>`);
            function Reformat(myDate) {
                let newDateFormat = myDate.substr(myDate.length - 2, 2) + "/" + myDate.substr(5, 2) + "/" + myDate.substr(0, 4);
                return newDateFormat;
            }
            let passDateFrom = Reformat($("#DateFrom").val());
            let passDateTo = Reformat($("#DateTo").val());
        $.ajax({
            url: "/Home/GetFlights",
            method: "GET",
            data: {
                Way: $("#Way").val(),
                Adults: $("#Adults").val(),
                Children: $("#Children").val(),
                Infant: $("#Infant").val(),
                Class: $("#Class").val(),
                Stopovers: $("#Stopovers").val(),
                Currency: $("#Currency").val(),
                From: $("#From").val(),
                To: $("#To").val(),
                DateFrom: passDateFrom,
                DateTo: passDateTo
            },
            dataType: "json",
            success: function (foundFlights) {
                let userFlights = JSON.parse(foundFlights);
                console.log(userFlights);
                if (userFlights["data"].length > 0) {
                    $("#runSearch").html(`<div class="card col-8 searchResults">
    <div class="card-body" id="results">
${userFlights}
    </div>
</div>`);
                } else {
                    $("#runSearch").html(`<div class="card col-8 searchResults">
    <div class="card-body" id="results">
No flights have been found based on the information provided.
    </div>
</div>`);
                }
            },
            error: function (err) {
                console.log(err);
                $("#runSearch").html(`<div class="card col-8 searchResults">
    <div class="card-body" id="results">
No flights have been found based on the information provided.
    </div>
</div>`);
            }});
        }
    });

});