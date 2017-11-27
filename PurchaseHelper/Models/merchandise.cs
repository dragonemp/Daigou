using PurchaseHelper.BusinessObjects;
using System.Drawing;

namespace PurchaseHelper.Models
{
    public class MerchandiseModel
    {
        [DataMapper("MerchandiseID")]
        public int? MerchandiseID { get; set; }
        [DataMapper("Name")]
        public string Name { get; set; }
        [DataMapper("Description")]
        public string Description { get; set; }
        [DataMapper("Store")]
        public string Store { get; set; }
        [DataMapper("Picture")]
        public byte[] Picture { get; set; }
        [DataMapper("USDPrice")]
        public decimal USDPrice { get; set; }
        [DataMapper("weight")]
        public double weight { get; set; }     
        public string MerchandiseName
        {
            get
            {
                return MerchandiseID + " : " + Name;
            }
        }
    }
}
