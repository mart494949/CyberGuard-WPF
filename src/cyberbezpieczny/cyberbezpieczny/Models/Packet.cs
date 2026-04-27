using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cyberbezpieczny.Models
{
    public class Packet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsMalicious { get; set; } 
        public string ImagePath { get; set; }
        
    }
}
      