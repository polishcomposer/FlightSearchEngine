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
        public void GetData()
        {
            var url = "https://tequila-api.kiwi.com/v2/search?fly_from=FRA&fly_to=PRG&date_from=04%2F05%2F2021&date_to=12%2F05%2F2021&nights_in_dst_from=2&nights_in_dst_to=3&max_fly_duration=20&flight_type=round&one_for_city=0&one_per_date=0&adults=2&children=2&selected_cabins=C&mix_with_cabins=M&adult_hold_bag=1%2C0&adult_hand_bag=1%2C1&child_hold_bag=2%2C1&child_hand_bag=1%2C1&only_working_days=false&only_weekends=false&partner_market=us&max_stopovers=2&max_sector_stopovers=2&vehicle_type=aircraft&limit=500";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["accept"] = "application/json";
            httpRequest.Headers["apikey"] = "DrPUBQjqS8nnOs7MJedRIL5uuY_SXXvr";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var stringResponse = streamReader.ReadToEnd();
                NewData = JObject.Parse(stringResponse);
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
