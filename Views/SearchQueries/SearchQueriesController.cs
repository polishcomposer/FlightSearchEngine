using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightSE.Data;
using FlightSE.Models;

namespace FlightSE.Views.SearchQueries
{
    public class SearchQueriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchQueriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SearchQueries
        public async Task<IActionResult> Index()
        {
            return View(await _context.SearchQuery.ToListAsync());
        }

        // GET: SearchQueries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchQuery = await _context.SearchQuery
                .FirstOrDefaultAsync(m => m.ID == id);
            if (searchQuery == null)
            {
                return NotFound();
            }

            return View(searchQuery);
        }

        // GET: SearchQueries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SearchQueries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: SearchQueries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchQuery = await _context.SearchQuery.FindAsync(id);
            if (searchQuery == null)
            {
                return NotFound();
            }
            return View(searchQuery);
        }

        // POST: SearchQueries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,Way,Adults,Children,Infant,Class,Stopovers,Currency,From,To,DateFrom,DateTo,QueryDate")] SearchQuery searchQuery)
        {
            if (id != searchQuery.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(searchQuery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SearchQueryExists(searchQuery.ID))
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
            return View(searchQuery);
        }

        // GET: SearchQueries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchQuery = await _context.SearchQuery
                .FirstOrDefaultAsync(m => m.ID == id);
            if (searchQuery == null)
            {
                return NotFound();
            }

            return View(searchQuery);
        }

        // POST: SearchQueries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var searchQuery = await _context.SearchQuery.FindAsync(id);
            _context.SearchQuery.Remove(searchQuery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SearchQueryExists(int id)
        {
            return _context.SearchQuery.Any(e => e.ID == id);
        }
    }
}
