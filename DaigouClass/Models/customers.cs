using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseHelper.Models
{
    public class CustomerModel
    {
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string IM { get; set; }
        public string IdentificationNumber { get; set; }
    }
}
