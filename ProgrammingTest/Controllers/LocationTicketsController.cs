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
    public class LocationTicketsController : Controller
    {
        private readonly ProgrammingTestContext _context;

        public LocationTicketsController(ProgrammingTestContext context)
        {
            _context = context;
        }

        // GET: LocationTickets
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: LocationTickets/Location
        public async Task<IActionResult> Location(int? locId)
        {
            //populate locations drop down
            var locations = from l in _context.Location
                            select new Location
                            {
                                LocationId = l.LocationId,
                                LocationName = l.LocationName
                            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (Location l in locations)
            {
                listItems.Add(new SelectListItem() { Value = l.LocationId.ToString(), Text = l.LocationName });
            }

            ViewBag.DropDownValues = new SelectList(listItems, "Value", "Text");

            //get query sting
            int loc;
            if(locId == null)
            {
                loc = 5;
            }
            else
            {
                loc = locId.Value;
            }
            
            //populate table
            var locationTickets =
                from t in _context.TicketsDaily
                join l in _context.Location on t.LocationId equals l.LocationId
                where l.LocationId == loc
                group t by new { t.BlockDate, l.LocationName } into date
                orderby date.Key.BlockDate
                select new LocationTicket
                {
                    BlockDate = date.Key.BlockDate,
                    TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                    LocationName = date.Key.LocationName
                };

            return View(await locationTickets.ToListAsync());
        }
        

        private bool LocationTicketExists(int id)
        {
            return _context.LocationTicket.Any(e => e.LocationTicketId == id);
        }
    }
}
