using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocateSense.Models
{
    public class user
    {
        public int ID { set; get; }
        public string name { set; get; }
        public string email { set; get; }
        public string telephone { set; get; }
        public string guid { set; get; }
        public string password { set; get; }
        public bool isLive { set; get; }
        public bool isLoggedOn { set; get; }
        public DateTime lastLoggedOn { set; get; }
        public int level {get; set;}

    }
}