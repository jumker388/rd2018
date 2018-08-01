using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class GameGuideItem
    {
        public int id { get; set; }
        public int game_id { get; set; }
        public string game { get; set; }
        public string description { get; set; }
    }
}
