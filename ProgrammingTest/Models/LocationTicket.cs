using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ProgrammingTest.Models
{
    public class LocationTicket
    {
        public int LocationTicketId { get; set; }

        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Display(Name = "Block Date")]
        [DataType(DataType.Date)]
        public DateTime BlockDate { get; set; }

        [Display(Name = "Tickets Played")]
        public int TicketsPlayed { get; set; }
    }
}
