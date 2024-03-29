﻿using FlightSE.Data;
using FlightSE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightSE.Controllers
{

    public class HomeController : Controller
    {
        private JObject NewData { get; set; }
        private JObject NewFlight { get; set; }
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public IEnumerable<SearchQuery> SearchQueriesFromData { get; set; }
        public IEnumerable<SearchQuery> HistoryQueriesFromData { get; set; }

        public JsonResult GetLocationFrom(string From)
        {
            var json = JsonConvert.SerializeObject(_context.Location.Single(p => p.AirportLocation == From));
            return Json(json);
        }
        public JsonResult GetLocationTo(string To)
        {
            var json = JsonConvert.SerializeObject(_context.Location.Single(p => p.AirportLocation == To));
            return Json(json);
        }
        public JsonResult GetSingleFlight(string UserID, string FlightID)
        {
            var json = JsonConvert.SerializeObject(_context.UserFlight.Single(p => p.UserID == UserID && p.FlightID == FlightID));
            return Json(json);
        }
        public JsonResult GetLocations(string Name)
        {
            var json = JsonConvert.SerializeObject(_context.Location.Where(p => p.AirportLocation.Contains(Name)).ToList().Take(10));
            return Json(json);
        }
        public JsonResult GetLocationsDestination(string Name)
        {
            var json = JsonConvert.SerializeObject(_context.Location.Where(p => p.AirportLocation.Contains(Name)).ToList().Take(10));
            return Json(json);
        }

        public JsonResult GetFlights(string Way, int Adults, int Children, int Infant, string Class, int? Stopovers, string Currency, string From, string To, string DateFrom, string DateTo)
        {
            string DateReturn = "";
            string DateReturnTo = "";
            string Cabin = "";
            string TotalStops = "";
            if (Class!="X")
            {
                Cabin = $"&selected_cabins={Class}";
            }
            if (Stopovers != 6)
            {
                TotalStops = $"&max_stopovers={Stopovers}";
            }

            if (Way=="round")
            {
                DateReturn = $"&return_from={DateTo}";
                DateReturnTo = $"&return_to={DateTo}";
            }
            var url = $"https://tequila-api.kiwi.com/v2/search?fly_from={From}&fly_to={To}&date_from={DateFrom}&date_to={DateFrom}{DateReturn}{DateReturnTo}&flight_type={Way}&adults={Adults}&children={Children}&infants={Infant}{Cabin}&only_working_days=false&only_weekends=false&partner_market=gb&curr={Currency}&locale=en{TotalStops}&vehicle_type=aircraft&limit=20";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["accept"] = "application/json";
            httpRequest.Headers["apikey"] = "DrPUBQjqS8nnOs7MJedRIL5uuY_SXXvr";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
           
            
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var stringResponse = streamReader.ReadToEnd();
                return Json(stringResponse);
            }

        }


        public JsonResult CheckCovid(string Country)
        {

            var url = $"https://covid-19-data.p.rapidapi.com/country/code?code={Country}";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["accept"] = "application/json";
            httpRequest.Headers["x-rapidapi-key"] = "a2b8543599msh61f31ce8dd35036p1daae4jsn21bf23979fc2";
            httpRequest.Headers["x-rapidapi-host"] = "covid-19-data.p.rapidapi.com";
           
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();



            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var stringResponse = streamReader.ReadToEnd();
                return Json(stringResponse);
            }

        }

        public async Task<IActionResult> AddQuery(string Way, int Adults, int Children, int Infant, string Class, int? Stopovers, string Currency, string From, string To, DateTime DateFrom, DateTime DateTo)
        {

            SearchQuery NewSearch = new SearchQuery
            {
                Way = Way,
                Adults = Adults,
                Children = Children,
                Infant = Infant,
                Class = Class,
                Stopovers = Stopovers,
                Currency = Currency,
                From = From,
                To = To,
                DateFrom = DateFrom,
                DateTo = DateTo
            };
            _context.SearchQuery.Add(NewSearch);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFlight = await _context.UserFlight
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userFlight == null)
            {
                return NotFound();
            }

            return View(userFlight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFlight = await _context.UserFlight.FindAsync(id);
            _context.UserFlight.Remove(userFlight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyFlights));
        }
        public async Task<IActionResult> AddFlight(string UserID, string Price, string TotalFlightTimes, string TotalDates, string BookingLink, string TotalTime, string FlightPlaces, string Details, string FlightID)
        {

            UserFlight NewSavedFlight = new UserFlight
            {
                UserID = UserID,
                Price = Price,
                TotalFlightTimes = TotalFlightTimes,
                TotalDates = TotalDates,
                BookingLink = BookingLink,
                TotalTime = TotalTime,
                FlightPlaces = FlightPlaces,
                Details = Details,
                FlightID = FlightID

    };
            _context.UserFlight.Add(NewSavedFlight);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,Way,Adults,Children,Infant,Class,Stopovers,Currency,From,To,DateFrom,DateTo,QueryDate")] SearchQuery searchQuery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(searchQuery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(searchQuery);
        }
        public async Task<IActionResult> HistoryQ()
        {

            HistoryQueriesFromData = await _context.SearchQuery.OrderByDescending(a => a.QueryDate).ToListAsync();
            return Json(HistoryQueriesFromData);
        }
        public async Task<IActionResult> Index()
        {

            SearchQueriesFromData = await _context.SearchQuery.OrderByDescending(a => a.QueryDate).ToListAsync();
            ViewBag.SearchHistory = SearchQueriesFromData;
         
            return View();
        }
        [Authorize]
        public async Task<IActionResult> MyFlights()
        {
            return View(await _context.UserFlight.Where(p => p.UserID == User.Identity.Name).OrderByDescending(a => a.ID).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
