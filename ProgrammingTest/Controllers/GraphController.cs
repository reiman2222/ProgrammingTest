using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ProgrammingTest.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace ProgrammingTest.Controllers
{
    public class GraphController : Controller
    {
        private readonly ProgrammingTestContext _context;

        public GraphController(ProgrammingTestContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //populate locations drop down
            var locations = from l in _context.Location
                            select new Location
                            {
                                LocationId = l.LocationId,
                                LocationName = l.LocationName
                            };

            List<SelectListItem> locListItems = new List<SelectListItem>();
            locListItems.Add(new SelectListItem() { Value = "-1", Text = "All Locations"});
            foreach (Location l in locations)
            {
                locListItems.Add(new SelectListItem() { Value = l.LocationId.ToString(), Text = l.LocationName });
            }

            ViewBag.LocationDropDown = new SelectList(locListItems, "Value", "Text");

            //populate games dropdown
            var games = from g in _context.Game
                            select new Game
                            {
                                GameId = g.GameId,
                                GameName = g.GameName
                            };

            List<SelectListItem> gameListItems = new List<SelectListItem>();
            gameListItems.Add(new SelectListItem() { Value = "-1", Text = "All Games" });

            foreach (Game g in games)
            {
                gameListItems.Add(new SelectListItem() { Value = g.GameId.ToString(), Text = g.GameName });
            }

            ViewBag.GameDropDown = new SelectList(gameListItems, "Value", "Text");


            return View();
        }

        [HttpPost]
        public IActionResult RenderGraph(DateTime StartDate, DateTime EndDate, int LocId, int GID)
        {
            if(EndDate < StartDate)
            {
                ViewBag.ErrorMessage = "Invalid Date Range Selected";
            }
            else
            {
                List<DataPoint> dataPoints = new List<DataPoint>();
                //user selected all locations and all games
                if (LocId == -1 && GID == -1)
                {
                    //get total tickets played for each day
                    var data = 
                        from t in _context.TicketsDaily
                        group t by new { t.BlockDate } into date
                        orderby date.Key.BlockDate
                        select new 
                        { 
                            BlockDate = date.Key.BlockDate,
                            TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                        };

                    //select from date range if provided
                    DateTime defaultDate = new DateTime(0001, 1, 1, 0, 0, 0);
                    if(EndDate != defaultDate && StartDate != defaultDate)
                    {
                        data = data.Where(x => x.BlockDate >= StartDate && x.BlockDate <= EndDate);
                    }
                    
                    foreach(var i in data)
                    {
                        dataPoints.Add(new DataPoint(i.BlockDate.ToString(), i.TicketsPlayed));
                    }
                    ViewBag.GraphName = JsonConvert.SerializeObject("All Locations & All Games");
                }
                else if(LocId == -1) //user select a game and all locations
                {
                    //get game name
                    var gameN = from g in _context.Game
                                where g.GameId == GID
                                select new Game
                                {
                                    GameName = g.GameName,
                                    GameId = g.GameId

                                };

                    //get total tickets played
                    var data =
                        from t in _context.TicketsDaily
                        group t by new { t.BlockDate, t.GameId } into date
                        where date.Key.GameId == GID
                        orderby date.Key.BlockDate
                        select new
                        {
                            BlockDate = date.Key.BlockDate,
                            TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                        };

                    //select from date range if provided
                    DateTime defaultDate = new DateTime(0001, 1, 1, 0, 0, 0);
                    if (EndDate != defaultDate && StartDate != defaultDate)
                    {
                        data = data.Where(x => x.BlockDate >= StartDate && x.BlockDate <= EndDate);
                    }
                    
                    foreach (var i in data)
                    {
                        dataPoints.Add(new DataPoint(i.BlockDate.ToString(), i.TicketsPlayed));
                    }

                    ViewBag.GraphName = JsonConvert.SerializeObject(gameN.Cast<Game>().First().GameName + " at All Locations");
                }
                else if(GID == -1) //user data for a location and all games
                {
                    //get location name
                    var locationN = from l in _context.Location
                                where l.LocationId == LocId
                                select new Location
                                {
                                    LocationName = l.LocationName,
                                    LocationId = l.LocationId

                                };

                    //get total tickets
                    var data =
                        from t in _context.TicketsDaily
                        group t by new { t.BlockDate, t.LocationId } into date
                        where date.Key.LocationId == LocId
                        orderby date.Key.BlockDate
                        select new
                        {
                            BlockDate = date.Key.BlockDate,
                            TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                        };

                    //select from date range if provided
                    DateTime defaultDate = new DateTime(0001, 1, 1, 0, 0, 0);
                    if (EndDate != defaultDate && StartDate != defaultDate)
                    {
                        data = data.Where(x => x.BlockDate >= StartDate && x.BlockDate <= EndDate);
                    }

                    foreach (var i in data)
                    {
                        dataPoints.Add(new DataPoint(i.BlockDate.ToString(), i.TicketsPlayed));
                    }

                    ViewBag.GraphName = JsonConvert.SerializeObject("All Games at " + locationN.Cast<Location>().First().LocationName);
                }
                else //a specific location and game was selected
                {
                    //get location name
                    var locationN = from l in _context.Location
                                    where l.LocationId == LocId
                                    select new Location
                                    {
                                        LocationName = l.LocationName,
                                        LocationId = l.LocationId

                                    };

                    //get game name
                    var gameN = from g in _context.Game
                                where g.GameId == GID
                                select new Game
                                {
                                    GameName = g.GameName,
                                    GameId = g.GameId

                                };

                    //get ticket totals
                    var data =
                        from t in _context.TicketsDaily
                        group t by new { t.BlockDate, t.LocationId, t.GameId } into date
                        where date.Key.LocationId == LocId && date.Key.GameId == GID
                        orderby date.Key.BlockDate
                        select new
                        {
                            BlockDate = date.Key.BlockDate,
                            TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                        };

                    //select from date range if provided
                    DateTime defaultDate = new DateTime(0001, 1, 1, 0, 0, 0);
                    if (EndDate != defaultDate && StartDate != defaultDate)
                    {
                        data = data.Where(x => x.BlockDate >= StartDate && x.BlockDate <= EndDate);
                    }

                    foreach (var i in data)
                    {
                        dataPoints.Add(new DataPoint(i.BlockDate.ToString(), i.TicketsPlayed));
                    }

                    ViewBag.GraphName = JsonConvert.SerializeObject(gameN.Cast<Game>().First().GameName + " at " + locationN.Cast<Location>().First().LocationName);
                }

                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }
            return View();
        }

        public IActionResult Totals()
        {
            //tickets sold by location
            var location =
                from t in _context.TicketsDaily
                join l in _context.Location on t.LocationId equals l.LocationId
                group t by new { l.LocationName } into date
                orderby date.Key.LocationName
                select new
                {
                    LocationName = date.Key.LocationName,
                    TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                };

            List<DataPoint> locationTotals = new List<DataPoint>();
            foreach (var i in location)
            {
                locationTotals.Add(new DataPoint(i.LocationName, i.TicketsPlayed));
            }

            ViewBag.LocationTotals = JsonConvert.SerializeObject(locationTotals);

            //tickets sold by game
            var game =
                from t in _context.TicketsDaily
                join g in _context.Game on t.GameId equals g.GameId
                group t by new { g.GameName } into date
                orderby date.Key.GameName
                select new
                {
                    GameName = date.Key.GameName,
                    TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                };

            List<DataPoint> gameTotals = new List<DataPoint>();
            foreach (var i in game)
            {
                gameTotals.Add(new DataPoint(i.GameName, i.TicketsPlayed));
            }

            ViewBag.GameTotals = JsonConvert.SerializeObject(gameTotals);

            return View();
        }

        public IActionResult Popularity(int? locId)
        {
            var locationDropDown = from l in _context.Location
                            select new Location
                            {
                                LocationId = l.LocationId,
                                LocationName = l.LocationName
                            };

            List<SelectListItem> locListItems = new List<SelectListItem>();
            
            foreach (Location l in locationDropDown)
            {
                locListItems.Add(new SelectListItem() { Value = l.LocationId.ToString(), Text = l.LocationName });
            }

            ViewBag.LocationDropDown = new SelectList(locListItems, "Value", "Text");

            //get query string
            int loc;
            if (locId == null)
            {
                loc = 5;
            }
            else
            {
                loc = locId.Value;
            }

            //get total tickes played in location with id loc
            var location =
               from t in _context.TicketsDaily
               join l in _context.Location on t.LocationId equals l.LocationId
               where t.LocationId == loc
               group t by new { l.LocationName } into date
               select new
               {
                   LocationName = date.Key.LocationName,
                   TicketsPlayed = date.Sum(x => x.TicketsPlayed),
               };

            int ticketsPlayed = location.First().TicketsPlayed;

            //get total ticked played by game 
            var game =
                from t in _context.TicketsDaily
                join g in _context.Game on t.GameId equals g.GameId
                join l in _context.Location on t.LocationId equals l.LocationId
                where l.LocationId == loc
                group t by new { g.GameName } into date
                orderby date.Key.GameName
                select new
                {
                    GameName = date.Key.GameName,
                    TicketsPlayed = date.Sum(x => x.TicketsPlayed),
                };

            List<DataPoint> gameTotals = new List<DataPoint>();
            foreach (var i in game)
            {
                //                                                      convert totals to percent
                gameTotals.Add(new DataPoint(i.GameName, Math.Round( ((i.TicketsPlayed / (double) ticketsPlayed) * 100.0), 2)));
            }

            ViewBag.LocationName = JsonConvert.SerializeObject(location.First().LocationName);
            ViewBag.GameTicketsSold = JsonConvert.SerializeObject(gameTotals);

            return View();
        }
    }
}