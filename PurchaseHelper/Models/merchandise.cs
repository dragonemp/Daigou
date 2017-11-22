using PurchaseHelper.BusinessObjects;

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
        public string Picture { get; set; }
        [DataMapper("USDPrice")]
        public double USDPrice { get; set; }
        [DataMapper("weight")]
        public double weight { get; set; }        
    }
}
