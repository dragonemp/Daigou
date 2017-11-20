using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseHelper.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }
        public CustomerModel Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentCarrier { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }

    public class OrderItemModel
    {
        public int? OrderItemID { get; set; }
        public MerchandiseModel Merchandise { get; set; }
        public int Number { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountValue { get; set; }
    }
}
