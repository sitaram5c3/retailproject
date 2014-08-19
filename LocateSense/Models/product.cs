using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocateSense.Models
{
    public class product
    {
        public int ID { set; get; }
        public string manufacturer { set; get; }
        public string productName { set; get; }
        public string imageURL { set; get; }
        public string imageInstallationURL { set; get; }
        public int availableStock { set; get; }
        public int numberOfVisits { set; get; }
        public decimal price { set; get; }
        public string UUID { set; get; }
        public string category { set; get; }
        public int productOwner { set; get; }
    }
}