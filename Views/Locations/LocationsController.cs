using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightSE.Data;
using FlightSE.Models;
using Newtonsoft.Json.Linq;
using Nancy.Json;
using Newtonsoft.Json;

namespace FlightSE.Views.Locations
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        private JObject locations { get; set; }
        public List<Airport> GetAirports()
        {
            string jsonString = System.IO.File.ReadAllText(@"../FlightSE/wwwroot/js/airports.json");
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var records = ser.Deserialize<List<Airport>>(jsonString);
            return records;
        }
        public List<Country> GetCountries()
        {
            string jsonString = System.IO.File.ReadAllText(@"../FlightSE/wwwroot/js/countries.json");
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var records = ser.Deserialize<List<Country>>(jsonString);
            return records;
        }
        public List<Root> GetCities()
        {
            string jsonString = System.IO.File.ReadAllText(@"../FlightSE/wwwroot/js/cities.json");
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var records = ser.Deserialize<List<Root>>(jsonString);
            return records;
        }
        public IActionResult Test()
        {
            var Cities = GetCities();

            // string AirportAll = "";
            
            foreach (Root element in Cities)
            {


                //  AirportAll = element.name + " (" + element.code + ") " + element.city;

                /*    if (!_context.Location.Any(p => p.AirportLocation == element.code))
                {
                    _context.Add(new Location { 
                        AirportLocation = element.code,
                        Code = element.code
                    });
                    _context.SaveChanges();
                }

          if (!_context.Location.Any(p => p.AirportLocation == element.name))
              {
                  _context.Add(new Location
                  {
                      AirportLocation = element.name,
                      Code = element.code
                  });
                  _context.SaveChanges();
              }

              */
                if (!_context.Location.Any(p => p.AirportLocation == element.name))
                {
                    _context.Add(new Location
                    {
                        AirportLocation = element.name,
                        Code = element.code
                    });
                    _context.SaveChanges();
                }


            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Locations
        public async Task<IActionResult> Index()
        {

            return View(await _context.Location.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

   

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AirportLocation")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AirportLocation")] Location location)
        {
            if (id != location.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);
            _context.Location.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.ID == id);
        }
    }
}
