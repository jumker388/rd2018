using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class GiftCodeResult
    {
        public int totalRecord { get; set; }
        public List<GiftCodeItem> data { get; set; }
    }
}
