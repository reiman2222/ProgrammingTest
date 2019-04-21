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
    public class GameTicketsController : Controller
    {
        private readonly ProgrammingTestContext _context;

        public GameTicketsController(ProgrammingTestContext context)
        {
            _context = context;
        }

        // GET: GameTickets
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Game(int? gameId)
        {
            //papulate game dropdown
            var games = from g in _context.Game
                        select new Game
                        {
                            GameId = g.GameId,
                            GameName = g.GameName
                        };

            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (Game g in games)
            {
                listItems.Add(new SelectListItem() { Value = g.GameId.ToString(), Text = g.GameName });
            }

            ViewBag.DropDownValues = new SelectList(listItems, "Value", "Text");
            
            //get query sting
            int gId;
            if (gameId == null)
            {
                gId = 1;
            }
            else
            {
                gId = gameId.Value;
            }

            //get table data
            var gameTickets =
                from t in _context.TicketsDaily
                join g in _context.Game on t.GameId equals g.GameId
                where g.GameId == gId
                group t by new { t.BlockDate, g.GameName } into date
                orderby date.Key.BlockDate
                select new GameTicket
                {
                    BlockDate = date.Key.BlockDate,
                    TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                    GameName = date.Key.GameName
                };

            return View(await gameTickets.ToListAsync());
        }




        private bool GameTicketExists(int id)
        {
            return _context.GameTicket.Any(e => e.GameTicketId == id);
        }
    }
}
