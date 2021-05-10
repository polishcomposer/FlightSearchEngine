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

        public JsonResult GetLocations(string Name)
        {
            var json = JsonConvert.SerializeObject(_context.Location.Where(p => p.AirportLocation.Contains(Name)).ToList().Take(10));
            return Json(json);
        }
        
        public JsonResult GetFlights()
        {
            var url = "https://tequila-api.kiwi.com/v2/search?fly_from=LON&fly_to=PAR&date_from=20%2F05%2F2021&date_to=20%2F05%2F2021&return_from=01%2F06%2F2021&return_to=01%2F06%2F2021&flight_type=round&adults=1&children=1&infants=1&selected_cabins=C&only_working_days=false&only_weekends=false&partner_market=gb&max_stopovers=2&vehicle_type=aircraft&sort=price&limit=100";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["accept"] = "application/json";
            httpRequest.Headers["apikey"] = "DrPUBQjqS8nnOs7MJedRIL5uuY_SXXvr";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var stringResponse = streamReader.ReadToEnd();
                return Json(JObject.Parse(stringResponse));
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
