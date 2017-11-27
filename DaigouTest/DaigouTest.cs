using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurchaseHelper.BusinessObjects;
using PurchaseHelper.Models;

namespace DaigouTest
{
    [TestClass]
    public class DaigouTest
    {
        private string _conn = "Data Source=(local);Initial Catalog=Daigou;Integrated Security=False;User Id=test;Password=test;MultipleActiveResultSets=True";

        [TestMethod]
        public void TestCurrency()
        {
            CurrencyHelper obj = new CurrencyHelper();
            decimal price = obj.GetExchangeRates("CNY");
            decimal price2 = obj.GetExchangeRates("KRW");
        }

        [TestMethod]
        public void TestCustomerBOSave()
        {
            CustomerBO bo = new CustomerBO(_conn);
            CustomerModel contract = new CustomerModel();
            contract.CustomerId = 5;
            contract.FirstName = "莉";
            contract.LastName = "王";
            contract.Address1 = "二环路东四段426号4单元5号";
            contract.City = "成都市";
            contract.State = "四川省";
            contract.Phone = "18980078133";
            contract.Zip = "610000";
            contract.Country = "中国";
            string fileName = @"E:/Downloads/wenphoto.PNG";
            byte[] bytes = File.ReadAllBytes(fileName);
            contract.CustomerPicture = bytes;

            bo.Save(contract);
        }

        [TestMethod]
        public void TestMerchandiseBOSave()
        {
            MerchandiseBO bo = new MerchandiseBO(_conn);
            MerchandiseModel contract = new MerchandiseModel();
            contract.MerchandiseID = 1;
            contract.Description = "a";
            contract.Name = "b";
            string fileName = @"E:/Downloads/wenphoto.PNG";
            byte[] bytes = File.ReadAllBytes(fileName);
            contract.Picture = bytes;
            contract.Store = "d";
            contract.USDPrice = (decimal)1.345;
            contract.weight = 1.25;

            bo.Save(contract);
        }

        [TestMethod]
        public void TestOrderBOSave()
        {
            OrderBO bo = new OrderBO(_conn);
            OrderModel contract = new OrderModel();
            contract.OrderID = -1;
            contract.CustomerID = 5;
            contract.OrderDate = DateTime.Parse("1/5/2015");
            contract.ShippingCost = (decimal)12.45;
            contract.ChargedPrice = (decimal)52.12;
            contract.OrderItems = new System.Collections.Generic.List<OrderItemModel>();
            OrderItemModel item = new OrderItemModel();
            item.MerchandiseID = 1;
            item.Number = 3;
            item.DiscountPercent = (decimal)1.5;
            contract.OrderItems.Add(item);
            bo.Save(contract);
        }

        [TestMethod]
        public void TestOrderBODelete()
        {
            OrderBO bo = new OrderBO(_conn);
            bo.Delete(6);
        }

        [TestMethod]
        public void TestOrderBOGetById()
        {
            OrderBO bo = new OrderBO(_conn);
            OrderModel order = bo.GetByID(8);
        }

        [TestMethod]
        public void TestOrderBOGetList()
        {
            OrderBO bo = new OrderBO(_conn);
            List<OrderModel> order = bo.GetList("");
        }
    }
}
