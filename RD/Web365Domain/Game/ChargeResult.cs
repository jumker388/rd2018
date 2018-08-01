using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class ChargeResult
    {
        public int totalRecord { get; set; }
        public List<ChargeItem> data { get; set; }
    }
}
