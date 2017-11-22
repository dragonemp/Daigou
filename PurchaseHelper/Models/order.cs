using System;
using System.Collections.Generic;
using PurchaseHelper.BusinessObjects;

namespace PurchaseHelper.Models
{
    public class OrderModel
    {
        [DataMapper("OrderID")]
        public int? OrderID { get; set; }
        [DataMapper("CustomerID")]
        public int CustomerID { get; set; }
        public CustomerModel Customer { get; set; }
        [DataMapper("OrderDate")]
        public DateTime OrderDate { get; set; }
        [DataMapper("ShipmentDate")]
        public DateTime? ShipmentDate { get; set; }
        [DataMapper("ShipmentNumber")]
        public string ShipmentNumber { get; set; }
        [DataMapper("ShipmentCarrier")]
        public string ShipmentCarrier { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }

    public class OrderItemModel
    {
        [DataMapper("OrderItemID")]
        public int? OrderItemID { get; set; }
        [DataMapper("OrderID")]
        public int? OrderID { get; set; }
        [DataMapper("MerchandiseID")]
        public int MerchandiseID { get; set; }
        public MerchandiseModel Merchandise { get; set; }
        [DataMapper("Number")]
        public int Number { get; set; }
        [DataMapper("DiscountPercent")]
        public double? DiscountPercent { get; set; }
        [DataMapper("DiscountValue")]
        public double? DiscountValue { get; set; }
    }
}
