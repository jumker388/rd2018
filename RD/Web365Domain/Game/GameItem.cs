using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class GameItem
    {
        public int zoneID { get; set; }
        public string name { get; set; }
        public int displayStatus { get; set; }
        public int gameOrder { get; set; }

        public bool IsShow { get; set; }
    }
}
