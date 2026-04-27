using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cyberbezpieczny.Models
{
    // Ta klasa odpowiada strukturze całego pliku JSON
    public class Module
    {
        public string ModuleName { get; set; }
        public List<Challenge> Challenges { get; set; }
    }
}
