using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PurchaseHelper.Models
{
    public class MerchandiseModel
    {
        public int? MerchandiseID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Store { get; set; }
        public string Picture { get; set; }
        public decimal USDPrice { get; set; }
        public double weight { get; set; }        
    }
}
