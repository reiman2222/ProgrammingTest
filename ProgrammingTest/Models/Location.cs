using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingTest.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        [Display(Name = "Location Name")]
        public string LocationName { get; set; }
    }
}
