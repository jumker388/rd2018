using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class GamePheInGameModel
    {
        public List<List<GameTaxItem>> ListTax { get; set; }
        public List<GameItem> ListGame { get; set; }
        
    }
}
