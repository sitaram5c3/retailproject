using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocateSense.Models
{
    public class offer
    {
        public int ID { set; get; }
        public DateTime? startDateTime { set; get; }
        public DateTime? endDateTime { set; get; }
        public string title { set; get; }
        public string description { set; get; }
        public string strapLine {set; get;}
        public decimal? price {set; get;}
        public int productId {set; get;}

        public enum dayFilterenum
        {
            Sun,
            Mon,
            Tue,
            Wed,
            Thu,
            Fri,
            Sat,
            All,
            Weekend,
            Weekday,
            BankHol
        }
        
 
        
        
    }
}