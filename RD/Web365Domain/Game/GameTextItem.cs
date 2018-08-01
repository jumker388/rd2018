using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class GameTextItem
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public DateTime DataStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string DataStartstring { get; set; }
        public string DateEndstring { get; set; }

        public bool IsDelete { get; set; }
        public int Order { get; set; }
    }
}
