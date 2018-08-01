using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain.Game
{
    public class RoomItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int game_id { get; set; }
        public int state { get; set; }
        public int min_bet { get; set; }
        public string game_name { get; set; }
    }
}
