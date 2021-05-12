using FlightSE.Data;
using FlightSE.Models;
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

        public JsonResult GetLocationFrom(string From)
        {
            var json = JsonConvert.SerializeObject(_context.Location.Single(p => p.AirportLocation == From));
            return Json(json);
        }
        public JsonResult GetLocations(string Name)
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
            string Start = "";
            string Finish = "";
            var LocationFrom = _context.Location.Single(p => p.AirportLocation == From);
            if (LocationFrom != null) {
                Start = LocationFrom.Code;
           
            } else
            {
                Start = From;
          
            }
            var LocationTo = _context.Location.Single(p => p.AirportLocation == To);
            if (LocationTo != null)
            {
                Finish = LocationTo.Code;
            } else
            {
                Finish = To;
            }
            if (Class!="X")
            {
                Cabin = $"&selected_cabins={Class}";
            }
            if (Stopovers != 6)
            {
                TotalStops = $"&max_stopovers={Stopovers}";
            }

            if (Way=="return")
            {
                DateReturn = $"&return_from={DateTo}";
                DateReturnTo = $"&return_to={DateTo}";
            }
            var url = $"https://tequila-api.kiwi.com/v2/search?fly_from={Start}&fly_to={Finish}&date_from={DateFrom}&date_to={DateFrom}{DateReturn}{DateReturnTo}&flight_type={Way}&adults={Adults}&children={Children}&infants={Infant}{Cabin}&only_working_days=false&only_weekends=false&partner_market=gb&curr={Currency}&locale=en{TotalStops}&vehicle_type=aircraft&limit=20";

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
       
        
        public IEnumerable<SearchQuery> SearchQueriesFromData { get; set; }

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
        public async Task<IActionResult> Index()
        {

            SearchQueriesFromData = await _context.SearchQuery.OrderByDescending(a => a.QueryDate).ToListAsync();
            ViewBag.SearchHistory = SearchQueriesFromData;
         
            // GetData();
          //  ViewBag.SearchData = new IndexModel {
                //  MyData = NewData 
              //  MyFlight = NewFlight
        //};
            /* @ViewBag.SearchData.MyData */
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
