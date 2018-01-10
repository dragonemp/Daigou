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
            if (id > 0 && Contract.OrderItems != null)
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
            order.OrderItems = orderitem.GetList("OrderID=" + order.OrderID.ToString(), true);
            order.Profit = CalculateProfit(order);
            return order;
        }

        public override List<OrderModel> GetList(string filter, bool addBlankItem = false)
        {
            List<OrderModel> orders = base.GetList(filter, addBlankItem);
            OrderItemBO orderitem = new OrderItemBO(_connString);
            foreach (OrderModel order in orders)
            {
                if (order.OrderID > 0)
                {
                    if (order.CustomerID > 0)
                    {
                        CustomerBO customer = new CustomerBO(_connString);
                        order.Customer = customer.GetByID(order.CustomerID);
                    }
                    order.OrderItems = orderitem.GetList("OrderID=" + order.OrderID.ToString(), true);
                    order.Profit = CalculateProfit(order);
                }
            }
            return orders;
        }

        public decimal CalculateProfit(OrderModel order)
        {
            decimal profit = 0;
            decimal cost = 0;
            decimal exchangeRate = order.ExchangeRate;
            foreach(OrderItemModel orderItem in order.OrderItems)
            {
                if (orderItem.Merchandise != null)
                {
                    if (orderItem.DiscountValue.HasValue)
                    {
                        cost += (orderItem.Merchandise.USDPrice - orderItem.DiscountValue.Value) * exchangeRate;
                    }
                    else if (orderItem.DiscountPercent.HasValue)
                    {
                        cost += orderItem.Merchandise.USDPrice * (1 + orderItem.Merchandise.Tax / 100) * (1 - orderItem.DiscountPercent.Value / 100) * exchangeRate;
                    }
                    else
                    {
                        cost += orderItem.Merchandise.USDPrice * (1 + orderItem.Merchandise.Tax / 100) * exchangeRate;
                    }
                }
            }
            if (order.ShippingCost.HasValue)
                cost += order.ShippingCost.Value;
            if (order.ChargedPrice.HasValue)
                profit = order.ChargedPrice.Value - cost;
            return profit;
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

        public override List<OrderItemModel> GetList(string filter, bool addBlankItem = false)
        {
            List<OrderItemModel> orderItems = base.GetList(filter, addBlankItem);
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

        protected override bool Validate(OrderItemModel Contract)
        {
            bool isValid = base.Validate(Contract);
            if(!Contract.OrderID.HasValue || Contract.OrderID.Value <= 0)
            {
                isValid = false;
                ValidationErrors.Add("Order ID is required");
            }
            if (Contract.MerchandiseID <= 0)
            {
                isValid = false;
                ValidationErrors.Add("Merchandise is required");
            }
            if(Contract.Number <= 0)
            {
                isValid = false;
                ValidationErrors.Add("Number must be greater than 0");
            }
            return isValid;
        }
    }
}
