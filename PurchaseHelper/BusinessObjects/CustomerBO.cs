using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using PurchaseHelper.Models;

namespace PurchaseHelper.BusinessObjects
{
    public class CustomerBO : BOBase<CustomerModel>
    {
        public CustomerBO(string connString)
		{
            _connString = connString;
            TableName = "Customer";
            PrimaryKey = "CustomerID";
		}

        public override int Save(CustomerModel Contract)
        {
            return base.Save(Contract);
        }

        protected override bool Validate(CustomerModel Contract)
        {
            bool isValid = base.Validate(Contract);
            if (string.IsNullOrEmpty(Contract.Address1))
            {
                isValid = false;
                ValidationErrors.Add("Address 1 is required");
            }
            else if (Contract.Address1.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Address 1 cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.FirstName))
            {
                isValid = false;
                ValidationErrors.Add("First Name is required");
            }
            else if (Contract.FirstName.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("First Name cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.LastName))
            {
                isValid = false;
                ValidationErrors.Add("Last Name is required");
            }
            else if (Contract.LastName.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Last Name cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.Phone))
            {
                isValid = false;
                ValidationErrors.Add("Phone is required");
            }
            else if (Contract.Phone.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Phone cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.State))
            {
                isValid = false;
                ValidationErrors.Add("Stateis required");
            }
            else if (Contract.State.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("State cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.Zip))
            {
                isValid = false;
                ValidationErrors.Add("Zip is required");
            }
            else if (Contract.Zip.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Zip cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.City))
            {
                isValid = false;
                ValidationErrors.Add("City is required");
            }
            else if (Contract.City.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("City cannot be more than 50 characters");
            }

            if (string.IsNullOrEmpty(Contract.Country))
            {
                isValid = false;
                ValidationErrors.Add("Country is required");
            }
            else if (Contract.Country.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Country cannot be more than 50 characters");
            }

            if (Contract.Address2.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Address 2 cannot be more than 50 characters");
            }

            if (Contract.IdentificationNumber.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("Identification Number cannot be more than 50 characters");
            }

            if (Contract.IM.Length > 50)
            {
                isValid = false;
                ValidationErrors.Add("IM cannot be more than 50 characters");
            }

            return isValid;
        }
    }
}
