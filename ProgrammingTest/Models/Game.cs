using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingTest.Models
{
    public class Game
    {
        public int GameId { get; set; }

        [Display(Name = "Game Name")]
        public string GameName { get; set; }

        public Game(int gid, string gName)
        {
            this.GameId = gid;
            this.GameName = gName;
        }
        public Game()
        {

        }
    }
}
