using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProgrammingTest.Models;

namespace ProgrammingTest.Controllers
{
    public class TicketsDailyController : Controller
    {
        private readonly ProgrammingTestContext _context;

        public TicketsDailyController(ProgrammingTestContext context)
        {
            _context = context;
        }

        // GET: TicketsDaily
        public async Task<IActionResult> Index()
        {
            return View(await _context.TicketsDaily.ToListAsync());
        }

        // GET: TicketsDaily/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketsDaily = await _context.TicketsDaily
                .FirstOrDefaultAsync(m => m.TicketsDailyId == id);
            if (ticketsDaily == null)
            {
                return NotFound();
            }

            return View(ticketsDaily);
        }

        // GET: TicketsDaily/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketsDaily/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketsDailyId,GameId,LocationId,BlockDate,TicketsPlayed")] TicketsDaily ticketsDaily)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketsDaily);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketsDaily);
        }

        // GET: TicketsDaily/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketsDaily = await _context.TicketsDaily.FindAsync(id);
            if (ticketsDaily == null)
            {
                return NotFound();
            }
            return View(ticketsDaily);
        }

        // POST: TicketsDaily/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketsDailyId,GameId,LocationId,BlockDate,TicketsPlayed")] TicketsDaily ticketsDaily)
        {
            if (id != ticketsDaily.TicketsDailyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketsDaily);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketsDailyExists(ticketsDaily.TicketsDailyId))
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
            return View(ticketsDaily);
        }

        // GET: TicketsDaily/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketsDaily = await _context.TicketsDaily
                .FirstOrDefaultAsync(m => m.TicketsDailyId == id);
            if (ticketsDaily == null)
            {
                return NotFound();
            }

            return View(ticketsDaily);
        }

        // POST: TicketsDaily/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketsDaily = await _context.TicketsDaily.FindAsync(id);
            _context.TicketsDaily.Remove(ticketsDaily);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketsDailyExists(int id)
        {
            return _context.TicketsDaily.Any(e => e.TicketsDailyId == id);
        }
    }
}
