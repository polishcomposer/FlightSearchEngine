using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightSE.Data;
using FlightSE.Models;

namespace FlightSE.Views.UserFlights
{
    public class UserFlightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserFlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserFlights
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserFlight.ToListAsync());
        }

        // GET: UserFlights/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: UserFlights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserFlights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,Price,TotalFlightTimes,TotalDates,BookingLink,TotalTime,FlightPlaces,Details")] UserFlight userFlight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userFlight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userFlight);
        }

        // GET: UserFlights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFlight = await _context.UserFlight.FindAsync(id);
            if (userFlight == null)
            {
                return NotFound();
            }
            return View(userFlight);
        }

        // POST: UserFlights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,Price,TotalFlightTimes,TotalDates,BookingLink,TotalTime,FlightPlaces,Details")] UserFlight userFlight)
        {
            if (id != userFlight.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userFlight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserFlightExists(userFlight.ID))
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
            return View(userFlight);
        }

        // GET: UserFlights/Delete/5
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

        // POST: UserFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFlight = await _context.UserFlight.FindAsync(id);
            _context.UserFlight.Remove(userFlight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserFlightExists(int id)
        {
            return _context.UserFlight.Any(e => e.ID == id);
        }
    }
}
