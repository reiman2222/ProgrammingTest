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
    public class TicketFullsController : Controller
    {
        private readonly ProgrammingTestContext _context;

        public TicketFullsController(ProgrammingTestContext context)
        {
            _context = context;
        }

        // GET: TicketFulls
        public async Task<IActionResult> Index()
        {
            var tickets = from t in _context.TicketsDaily
                          join g in _context.Game on t.GameId equals g.GameId
                          join l in _context.Location on t.LocationId equals l.LocationId
                          orderby t.BlockDate
                          select new TicketFull(){ GameName = g.GameName, LocationName = l.LocationName, TicketsPlayed = t.TicketsPlayed, BlockDate = t.BlockDate };

            return View(await tickets.ToListAsync());
        }


        private bool TicketFullExists(int id)
        {
            return _context.TicketFull.Any(e => e.TicketFullId == id);
        }
    }
}
