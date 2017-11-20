using PurchaseHelper.Models;

namespace PurchaseHelper.BusinessObjects
{
    public class MerchandiseBO : BOBase<MerchandiseModel>
    {
        public MerchandiseBO(string connString)
		{
            _connString = connString;
            TableName = "Merchandise";
            PrimaryKey = "MerchandiseID";
		}
    }
}
