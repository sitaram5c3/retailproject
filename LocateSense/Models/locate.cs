using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocateSense.Models
{
    public class locate
    {
        public int ID { set; get; }
        public DateTime vistDateTime { set; get; }
        public int userId { set; get; }
        public int beaconId { set; get; } 
    }
}