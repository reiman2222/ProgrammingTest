using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingTest.Models
{
    public class TicketsDaily
    {
        public int TicketsDailyId { get; set; }
        public int GameId { get; set; }

        public int LocationId { get; set; }

        [Display(Name = "Block Date")]
        [DataType(DataType.Date)]
        public DateTime BlockDate { get; set; }

        [Display(Name = "Tickets Played")]
        public int TicketsPlayed { get; set; }
    }
}
