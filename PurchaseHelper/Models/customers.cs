using PurchaseHelper.BusinessObjects;

namespace PurchaseHelper.Models
{
    public class CustomerModel
    {
        [DataMapper("CustomerID")]
        public int? CustomerId { get; set; }
        [DataMapper("FirstName")]
        public string FirstName { get; set; }
        [DataMapper("LastName")]
        public string LastName { get; set; }
        [DataMapper("Address1")]
        public string Address1 { get; set; }
        [DataMapper("Address2")]
        public string Address2 { get; set; }
        [DataMapper("City")]
        public string City { get; set; }
        [DataMapper("State")]
        public string State { get; set; }
        [DataMapper("Country")]
        public string Country { get; set; }
        [DataMapper("Zip")]
        public string Zip { get; set; }
        [DataMapper("Phone")]
        public string Phone { get; set; }
        [DataMapper("IM")]
        public string IM { get; set; }
        [DataMapper("IdentificationNumber")]
        public string IdentificationNumber { get; set; }
    }
}
