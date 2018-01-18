using System;
using System.Collections.Generic;
using PurchaseHelper.BusinessObjects;

namespace PurchaseHelper.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Shipped = 1,
        Delivered = 2,
        Returned = 3,
        Cancelled = 4
    }
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
        [DataMapper("OrderStatus")]
        public int OrderStatus { get; set; }
        [DataMapper("ShippingCost")]
        public decimal? ShippingCost { get; set; }
        [DataMapper("ChargedPrice")]
        public decimal? ChargedPrice { get; set; }
        [DataMapper("MerchandiseID")]
        public int MerchandiseID { get; set; }
        public MerchandiseModel Merchandise { get; set; }
        [DataMapper("Number")]
        public int Number { get; set; }
        [DataMapper("DiscountPercent")]
        public decimal? DiscountPercent { get; set; }
        [DataMapper("DiscountValue")]
        public decimal? DiscountValue { get; set; }
        [DataMapper("PurchasePrice")]
        public decimal? PurchasePrice { get; set; }
        [DataMapper("ExchangeRate")]
        public decimal? ExchangeRate { get; set; }
        public decimal? Profit { get; set; }
        public decimal RealTimeExchangeRate
        {
            get
            {
                CurrencyHelper obj = new CurrencyHelper();
                decimal exchangeRate = obj.GetExchangeRates("CNY");
                return exchangeRate;
            }
        }
        //public List<OrderItemModel> OrderItems { get; set; }
        public string OrderName
        {
            get
            {
                return OrderID.HasValue ? OrderID.Value + " : " + (OrderStatus)OrderStatus : "";
            }
        }
        public string CustomerName
        {
            get
            {
                return Customer != null ? Customer.FullName : "";
            }
        }

        public string MerchandiseName
        {
            get
            {
                return Merchandise != null ? Merchandise.Name : "";
            }
        }
    }

    public class OrderSearchModel
    {
        public int 编号 { get; set; }
        public string 客户 { get; set; }
        public string 商品 { get; set; }
        public int 数量 { get; set; }
        public string 状态 { get; set; }
        public DateTime 订货日期 { get; set; }
        public DateTime? 发货日期 { get; set; }
        public decimal? 卖价 { get; set; }
        public decimal? 买价 { get; set; }
        public decimal? 运费 { get; set; }
        public decimal? 利润 { get; set; }
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
        public decimal? DiscountPercent { get; set; }
        [DataMapper("DiscountValue")]
        public decimal? DiscountValue { get; set; }
        public string OrderItemName
        {
            get
            {
                return OrderItemID.HasValue ? OrderItemID.Value + " : " + Merchandise.Name : "";
            }
        }
    }
}
