using System.Threading.Tasks;
using PurchaseHelper.Models;

namespace PurchaseHelper.BusinessObjects
{
    public class CustomerBO
    {
        string _connString;
        public CustomerBO(string connString)
		{
            _connString = connString;
		}

        public CustomerModel Save(CustomerModel value)
		{
            if (value.CustomerId.HasValue)
            {
                Update(value, "");
            }
            else
            {
                Insert(value, "");
            }
            
			return value;
		}

        protected void Update(CustomerModel customer, string TableName)
        {

        }

        protected void Insert(CustomerModel customer, string TableName)
        {

        }
    }
}
