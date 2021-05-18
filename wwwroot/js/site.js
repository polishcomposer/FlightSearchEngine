// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let passangers = 1;
let dateDeparture;
let dateReturn;
let oldReturn;
let defaultPhoto = "";
let newHistoryRecords = "";
let toPictureData = "";
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
    let locationFromQuery = "";
    let locationToQuery = "";
    $("#From").on("keyup", function () {
          let locationFrom = $(this).val();
      
                    $.ajax({
                          url: "/Home/GetLocations",
                          method: "GET",
                        data: { Name: locationFrom },
                          dataType: "json",
                          success: function (data) {
                              let airports = JSON.parse(data);
                              if (Object.keys(airports).length > 0) {
                                  let suggestions = `<datalist id="locationsFrom">`;
                                  for (let a = 0; a < Object.keys(airports).length; a++) {
                                      suggestions += `<option value="${airports[a]["AirportLocation"]}">`;
                                  }
                                  suggestions += `</datalist>`;
                                  $("#FromSuggestions").html(suggestions);
                              }

                              $.ajax({
                                  url: "/Home/GetLocationFrom",
                                  method: "GET",
                                  data: { From: locationFrom },
                                  dataType: "json",
                                  success: function (respondFromCode) {
                                      let respondFQ = JSON.parse(respondFromCode);
                                      locationFromQuery = respondFQ["Code"];

                          },
                          error: function (err) {
                              console.log(err);
                          }
                      }); 
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $("#To").on("keyup", function () {
        let locationTo = $(this).val();

                $.ajax({
                    url: "/Home/GetLocationsDestination",
                    method: "GET",
                    data: { Name: locationTo },
                    dataType: "json",
                    success: function (data2) {
                        let airports2 = JSON.parse(data2);
                        if (Object.keys(airports2).length > 0) {
                            let suggestions2 = `<datalist id="locationsTo">`;
                            for (let b = 0; b < Object.keys(airports2).length; b++) {
                                suggestions2 += `<option value="${airports2[b].AirportLocation}">`;
                            }
                            suggestions2 += `</datalist>`;
                            $("#ToSuggestions").html(suggestions2);
                        }

                        $.ajax({
                            url: "/Home/GetLocationTo",
                            method: "GET",
                            data: { To: locationTo },
                            dataType: "json",
                            success: function (respondToCode) {
                                let respondTQ = JSON.parse(respondToCode);
                                locationToQuery = respondTQ["Code"];

                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
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
                let newDateFormat = myDate.substr(myDate.length-2, 2) + "/" + myDate.substr(5, 2) + "/" + myDate.substr(0, 4);
                return newDateFormat;
            }
            let passDateFrom = Reformat($("#DateFrom").val());
            let passDateTo = Reformat($("#DateTo").val());


            function convertSeconds(seconds) {
                var day, hour, minute
                minute = Math.floor(seconds / 60);
                hour = Math.floor(minute / 60);
                day = Math.floor(hour / 24);
                seconds = seconds % 60;
                minute = minute % 60;
                hour = hour % 24;
                if (day == 0) {
                    return hour + "h " + minute + "m";
                } else if (day == 0 && hour == 0) {
                    return minute + "m";
                } else {
                    return day + "d " + hour + "h " + minute + "m";
                }
            
            }
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
                From: locationFromQuery,
                To: locationToQuery,
                DateFrom: passDateFrom,
                DateTo: passDateTo
            },
            dataType: "json",
            success: function (foundFlights) {
                let userFlights = JSON.parse(foundFlights);
                let allFlights = userFlights["data"];
                if (allFlights.length > 0) {

                    $.ajax({
                        url: "/Home/AddQuery",
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
                            DateFrom: $("#DateFrom").val(),
                            DateTo: $("#DateTo").val()
                        },
                        success: function (ok) {
                          
                            $.ajax({
                                url: "/Home/HistoryQ",
                                method: "GET",
                                success: function (hQueries) {
                                    newHistoryRecords = "";
                                    for (let hQ = 0; hQ < 4; hQ++) {
                                      
                                        toPictureData = hQueries[hQ]["to"];
                                        $.ajax({
                                            url: `https://pixabay.com/api/?key=3853087-8c2a07a3d1d9a8e2ac4f750a3&q=${toPictureData}&image_type=photo&per_page=3`,
                                            method: "GET",
                                            dataType: "json",
                                            success: function (newImg) {
                                      
                                              if (newImg["hits"].length>0) {
                                                  defaultPhoto = newImg["hits"][0]["webformatURL"];
                                                } else {
                                                  defaultPhoto = "../img/journey.jpg";
                                                } 
                                            
                                                newHistoryRecords += `<div class="col">
                                                <div class="card h-100 historyQuery">
                                                    <img src="${defaultPhoto}" class="card-img-top" alt="toPictureData">
                                                     <div class="card-body">
                                                        <h5 class="card-title">
                                                            From ${hQueries[hQ]["from"]}<br />To ${hQueries[hQ]["to"]}
                                                        </h5>
                                                        <p class="card-text">
                                                            <!--  <a href="#" class="btn btn-primary">View</a> -->
                                                        </p>
                                                    </div></div></div>`;
                                            },
                                            async: false,
                                            error: function (err) {
                                                console.log(err);
                                            }
                                        });
                                    }
                              
                                    $("#historyQueries").html(newHistoryRecords);

                                },
                                error: function (err) {
                                    console.log(err);
                                }
                            });



                        },
                        error: function (err) {
                            console.log(err);
                        }
                    });


                    let stringWithResults = `<div class="card col-8 searchResults">
    <div class="card-body" id="results">`;
                    let departureTime = ""; 
                    let arrivalTime = ""; 
                    let timeFrom = "";
                    let timeTo = ""; 
                    let dateFromA = "";
                    let dateFromB = "";
                    let dateToA = ""; 
                    let dateToB = "";
                    let firstDay = "";
                    let secondDay = "";
                    let newDay = "";
                    let stops = "";
                    let currencySign = "";
                    let milliseconds = 0;
                    let back = "";
                    let saveButton = "";

                  
                    let userFlightResults = {};
                
                    console.log(allFlights);

                    for (let c = 0; c < allFlights.length; c++) {
                        departureTime = allFlights[c].local_departure;
                        arrivalTime = allFlights[c].local_arrival;
                        timeFrom = departureTime.substring(11, 16);
                        timeTo = arrivalTime.substring(11, 16);
                        dateFromA = departureTime.substring(8, 10);
                        dateFromB = departureTime.substring(5, 7);
                        dateToA = arrivalTime.substring(8, 10);
                        dateToB = arrivalTime.substring(5, 7);

                        firstDay = departureTime.substring(0, 10);
                        secondDay = arrivalTime.substring(0, 10);
                        milliseconds = allFlights[c].duration.total;

                        const date1 = new Date(firstDay);
                        const date2 = new Date(secondDay);
                        const diffTime = Math.abs(date2 - date1);
                        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

                        if (diffTime != 0) {
                            newDay = "+" + diffDays;
                        }

                        if (allFlights[c].route.length > 1) {
                            stops = "Changes: " + (allFlights[c].route.length - 1);
                        } else {
                            stops = "Direct";
                        }
                        if ($('#Currency').val() == "GBP") {
                            currencySign = "£";
                        } else {
                            currencySign = $('#Currency').val();
                        }

                        if ($('#DateTo').val()) {
                            back = ` - ${allFlights[c].cityFrom} ${allFlights[c].cityCodeFrom}`;
                        }
                        if ($('#UserName').val() != "") {

                            $.ajax({
                                url: "/Home/GetSingleFlight",
                                method: "GET",
                                data: {
                                    UserID: $('#UserName').val(),
                                    FlightID: allFlights[c].id
                                },
                                dataType: "json",
                                success: function (ok) {
                                     saveButton = `<span class="saved">Saved <img src="../img/ok.svg" alt="ok"></span>`;
                                },
                                async: false,
                                error: function (err) {
                                    saveButton = `<span class="user-flight${c}"><button type="button" class="btn btn-primary btn-sm save-button user-flight-button${c}" id="user-save-button">Save</button></span>`;
                                }
                            }); 
                        } else {
                            saveButton = `<span class="user-flight${c}"><button type="button" class="btn btn-primary btn-sm save-button" data-toggle="modal" data-target="#goLogin">Save</button></span>`;
                        }


                        stringWithResults += `<div class="card mb-3 flightResult">
                                <div class="col-12">
                                    <div class="card-body">
                                      <div class="cardsHead"><h5 class="card-title">Total price: ${currencySign} ${allFlights[c].price}</h5>

         <div class="totalTime"><span class="flightTime">${timeFrom} - ${timeTo}</span> <span class="totalDates">${dateFromA}/${dateFromB} - ${dateToA}/${dateToB}<span class="topTime text-muted">${newDay}</span></div>
                  <div class="top-right-buttons">    <a type="button" class="btn btn-primary flight-button btn-sm" href="${allFlights[c].deep_link}" target="_blank">Book your flight (kiwi.com)</a>  ${saveButton}</div>                                                                       
</div> 
                                       <div class="cardsHead2">
                                                    <span> ${allFlights[c].cityFrom} ${allFlights[c].cityCodeFrom} - ${allFlights[c].cityTo} ${allFlights[c].cityCodeTo} ${back} (${stops})</span>
                                                    <span>Total time: ${convertSeconds(milliseconds)}</span>
                                      </div>`;


                        userFlightResults[c] = {};
                        userFlightResults[c]["flightID"] = allFlights[c].id;
                        userFlightResults[c]["Price"] = `Total price: ${currencySign} ${allFlights[c].price}`;
                        userFlightResults[c]["TotalFlightTimes"] = `${timeFrom} - ${timeTo}`;
                        userFlightResults[c]["TotalDates"] = `${dateFromA}/${dateFromB} - ${dateToA}/${dateToB}`;
                        userFlightResults[c]["BookingLink"] = allFlights[c].deep_link;
                        userFlightResults[c]["TotalTime"] = convertSeconds(milliseconds);
                        userFlightResults[c]["FlightPlaces"] = ` ${allFlights[c].cityFrom} ${allFlights[c].cityCodeFrom} - ${allFlights[c].cityTo} ${allFlights[c].cityCodeTo} ${back} (${stops})`;
                        userFlightResults[c]["Details"] = "";
                        for (let part = 0; part < allFlights[c]["route"].length; part++) {

                            var dateFrom = new Date(allFlights[c]["route"][part]["local_departure"]);
                            var dateTo = new Date(allFlights[c]["route"][part]["local_arrival"]);
                            var differenceFromTo = (dateTo - dateFrom) / 1000;
                            const diffTimePart = Math.abs(allFlights[c]["route"][part]["local_arrival"].substring(0, 10) - allFlights[c]["route"][part]["local_departure"].substring(0, 10));
                            const diffDaysPart = Math.ceil(diffTimePart / (1000 * 60 * 60 * 24));

                            if (diffTimePart != 0) {
                                newDaysPart = "+" + diffDaysPart;
                            }
                            airlineImage = `https://daisycon.io/images/airline/?iata=${allFlights[c]["route"][part]["airline"]}`;
                            stringWithResults += `
                                        <hr>
                                      <div class="card-text card-flight">
                                            <div class="leftResult">
                                                    <img src="${airlineImage}" alt="${allFlights[c].airlines[0]}" class="airlineImg">
                                            </div>
                                            <div class="rightResult">
                                                    <div class="resultInside"><small class="text-muted">${allFlights[c]["route"][part].cityFrom} <span> ${" _" + convertSeconds(differenceFromTo) + "_"}</span>&nbsp${allFlights[c]["route"][part].cityTo}</small>
                                                    <span class="flightTime">${allFlights[c]["route"][part]["local_departure"].substring(11, 16)} - ${allFlights[c]["route"][part]["local_arrival"].substring(11, 16)}</span></div>
                                                    <span>${allFlights[c]["route"][part]["local_departure"].substring(8, 10)}/${allFlights[c]["route"][part]["local_departure"].substring(5, 7)} - ${allFlights[c]["route"][part]["local_arrival"].substring(8, 10)}/${allFlights[c]["route"][part]["local_arrival"].substring(5, 7)}</span>
                                            </div>
                                      </div>`;

                            userFlightResults[c]["Details"] += `._.${part}_${airlineImage}_${allFlights[c]["route"][part].cityFrom}_${convertSeconds(differenceFromTo)}_${allFlights[c]["route"][part].cityTo}_${allFlights[c]["route"][part]["local_departure"].substring(11, 16)}_${allFlights[c]["route"][part]["local_arrival"].substring(11, 16)}_${allFlights[c]["route"][part]["local_departure"].substring(8, 10)}_${allFlights[c]["route"][part]["local_departure"].substring(5, 7)}_${allFlights[c]["route"][part]["local_arrival"].substring(8, 10)}_${allFlights[c]["route"][part]["local_arrival"].substring(5, 7)}`;
                   
                       
                        }
                        stringWithResults += `</div></div></div>`;

                    }
                    stringWithResults += `</div></div>`;
                    $("#runSearch").html(stringWithResults);

                    console.log(userFlightResults);
                                


                    for (let fN = 0; fN < allFlights.length; fN++) {
                        $(`.user-flight-button${fN}`).on('click', function () {
                            $(`.user-flight${fN}`).html(`<span class="saved">Saved <img src="../img/ok.svg" alt="ok"></span>`);

                            
                            $.ajax({
                                url: "/Home/AddFlight",
                                method: "GET",
                                data: {
                                    UserID: $('#UserName').val(),
                                    Price: userFlightResults[fN]["Price"],
                                    TotalFlightTimes: userFlightResults[fN]["TotalFlightTimes"],
                                    TotalDates: userFlightResults[fN]["TotalDates"],
                                    BookingLink: userFlightResults[fN]["BookingLink"],
                                    TotalTime: userFlightResults[fN]["TotalTime"],
                                    FlightPlaces: userFlightResults[fN]["FlightPlaces"],
                                    Details: userFlightResults[fN]["Details"],
                                    FlightID: userFlightResults[fN]["flightID"]

                                },
                                success: function (addedFlight) {

                                    console.log("Added Flight: " + addedFlight);
                                },
                                error: function (err) {
                                    console.log(err);
                                }
                            }); 

                        });
                    }
                    
                   
                   
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

    $('#testUser').on('click', function () {
        alert("Test account management is unavailable. Please create your own account.");
    });



    /*
    let details = "._.0_https://daisycon.io/images/airline/?iata=FR_London_2h 20m_Porto_09:30_11:50_19_05_19_05._.1_https://daisycon.io/images/airline/?iata=FR_Porto_3h 15m_Paris_14:10_17:25_19_05_19_05";
    let parts = details.split("._.");
    let insertLines = "";
    for (let fI = 1; fI < parts.length; fI++) {
        let partsInfo = parts[fI].split("_");
        for (let pI = 0; pI < partsInfo.length; pI++) {
            insertLines += `${partsInfo[pI]} <br/>`;
        }
        insertLines += `<hr>`;
    }
    $('#flights').html(insertLines);
    */










});