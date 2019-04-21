using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualBasic;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;
namespace ProgrammingTest.Models
{
    public class AddData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProgrammingTestContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProgrammingTestContext>>()))
            {
                //load games
                if (context.Game.Any())
                {
                    //games loaded
                }
                else
                {
                    List<Game> games = new List<Game>();

                    //load csv
                    char[] delimiters = new char[] { ',' };

                    var p = Path.Combine(Environment.CurrentDirectory, "Games.csv");

                    using (StreamReader reader = new StreamReader(p))
                    {
                        string header = reader.ReadLine();
                        while (true)
                        {
                            string line = reader.ReadLine();

                            if (line == null)
                            {
                                break;
                            }

                            string[] parts = line.Split(delimiters);

                            Game g = new Game();
                            g.GameId = Convert.ToInt32(parts[0]);
                            g.GameName = parts[1];

                            games.Add(g);
                        }
                    }

                    foreach (Game g in games)
                    {
                        context.Game.Add(g);
                        context.SaveChanges();
                    }
                }

                //add locations
                if (context.Location.Any())
                {
                    //locations loaded
                }
                else
                {
                    List<Location> locations = new List<Location>();

                    //load csv
                    char[] delimiters = new char[] { ',' };

                    var p = Path.Combine(Environment.CurrentDirectory, "Locations.csv");

                    using (StreamReader reader = new StreamReader(p))
                    {
                        string header = reader.ReadLine();
                        while (true)
                        {
                            string line = reader.ReadLine();

                            if (line == null)
                            {
                                break;
                            }

                            string[] parts = line.Split(delimiters);

                            Location l = new Location();
                            l.LocationId = Convert.ToInt32(parts[0]);
                            l.LocationName = parts[1];

                            locations.Add(l);
                        }
                    }

                    foreach (Location l in locations)
                    {
                        context.Location.Add(l);
                        context.SaveChanges();
                    }
                }

                //add tickets
                if(context.TicketsDaily.Any())
                {
                    //tickets loaded
                }
                else
                {
                    List<TicketsDaily> tickets = new List<TicketsDaily>();

                    //load csv
                    char[] delimiters = new char[] { ',' };

                    var p = Path.Combine(Environment.CurrentDirectory, "TicketsDaily.csv");

                    using (StreamReader reader = new StreamReader(p))
                    {
                        string header = reader.ReadLine();
                        while (true)
                        {
                            string line = reader.ReadLine();

                            if (line == null)
                            {
                                break;
                            }

                            string[] parts = line.Split(delimiters);

                            TicketsDaily t = new TicketsDaily();
                            t.GameId = Convert.ToInt32(parts[2]);
                            t.LocationId = Convert.ToInt32(parts[1]);
                            t.TicketsPlayed = Convert.ToInt32(parts[3]);
                            t.BlockDate = DateTime.Parse(parts[0]);

                            tickets.Add(t);
                        }
                    }

                    foreach (TicketsDaily t in tickets)
                    {
                        context.TicketsDaily.Add(t);
                        context.SaveChanges();
                    }
                }

            }
        }

    }
}
