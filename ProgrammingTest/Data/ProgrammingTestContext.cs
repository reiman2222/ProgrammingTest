using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgrammingTest.Models;

namespace ProgrammingTest.Models
{
    public class ProgrammingTestContext : DbContext
    {
        public ProgrammingTestContext(DbContextOptions<ProgrammingTestContext> options)
            : base(options)
        {
        }

        public DbSet<ProgrammingTest.Models.Game> Game { get; set; }

        public DbSet<ProgrammingTest.Models.Location> Location { get; set; }

        public DbSet<ProgrammingTest.Models.TicketsDaily> TicketsDaily { get; set; }

        public DbSet<ProgrammingTest.Models.TicketFull> TicketFull { get; set; }

        public DbSet<ProgrammingTest.Models.LocationTicket> LocationTicket { get; set; }

        public DbSet<ProgrammingTest.Models.GameTicket> GameTicket { get; set; }

    }
}
