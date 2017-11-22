using System.Collections.Generic;
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

        public override OrderModel GetByID(int id)
        {
            OrderModel order = base.GetByID(id);
            if(order.CustomerID > 0)
            {
                CustomerBO customer = new CustomerBO(_connString);
                order.Customer = customer.GetByID(order.CustomerID);
            }
            OrderItemBO orderitem = new OrderItemBO(_connString);
            order.OrderItems = orderitem.GetList("OrderID=" + order.OrderID.ToString());
            return order;
        }

        public override List<OrderModel> GetList(string filter)
        {
            List<OrderModel> orders = base.GetList(filter);
            OrderItemBO orderitem = new OrderItemBO(_connString);
            foreach (OrderModel order in orders)
            {
                if (order.CustomerID > 0)
                {
                    CustomerBO customer = new CustomerBO(_connString);
                    order.Customer = customer.GetByID(order.CustomerID);
                }
                order.OrderItems = orderitem.GetList("OrderID=" + order.OrderID.ToString());
            }
            return orders;
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

        public override OrderItemModel GetByID(int id)
        {
            OrderItemModel orderitem = base.GetByID(id);
            if(orderitem.MerchandiseID > 0)
            {
                MerchandiseBO merchandise = new MerchandiseBO(_connString);
                orderitem.Merchandise = merchandise.GetByID(orderitem.MerchandiseID);
            }
            return orderitem;
        }

        public override List<OrderItemModel> GetList(string filter)
        {
            List<OrderItemModel> orderItems = base.GetList(filter);
            foreach (OrderItemModel orderItem in orderItems)
            {
                if (orderItem.MerchandiseID > 0)
                {
                    MerchandiseBO merchandise = new MerchandiseBO(_connString);
                    orderItem.Merchandise = merchandise.GetByID(orderItem.MerchandiseID);
                }
            }

            return orderItems;
        }
    }
}
