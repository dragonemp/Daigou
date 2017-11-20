using PurchaseHelper.Models;

namespace PurchaseHelper.BusinessObjects
{
    public class OrderBO : BOBase<OrderModel>
    {
        public OrderBO(string connString)
        {
            _connString = connString;
            TableName = "Orders";
            PrimaryKey = "OrderID";
        }

        public override int Save(OrderModel Contract)
        {
            int id = base.Save(Contract);
            if (id > 0)
            {
                OrderItemBO childBo = new OrderItemBO(_connString);
                foreach (OrderItemModel orderItem in Contract.OrderItems)
                {
                    if (!orderItem.OrderID.HasValue)
                        orderItem.OrderID = id;
                    else if (orderItem.OrderID.Value != id)
                        continue;
                    childBo.Save(orderItem);
                }
            }

            return id;
        }
    }

    public class OrderItemBO : BOBase<OrderItemModel>
    {
        public OrderItemBO(string connString)
        {
            _connString = connString;
            TableName = "OrderItem";
            PrimaryKey = "OrderItemID";
        }
    }
}
