using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using PurchaseHelper.BusinessObjects;
using PurchaseHelper.Models;
using System.Windows.Forms;

namespace Daigou
{
    public partial class Main : Form
    {
        protected string _connString = "";
        public Main()
        {
            InitializeComponent();
            _connString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            LoadStatusList();
            LoadCustomerList();
            LoadMerchandiseList();
            dtShipDate.Checked = false;
        }

        protected void LoadCustomerList()
        {
            CustomerBO customerBo = new CustomerBO(_connString);
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.ValueMember = "CustomerId";
            comboBoxCustomer.DataSource = customerBo.GetList("");
        }

        private void LoadStatusList()
        {
            cbStatus.DataSource = Enum.GetValues(typeof(OrderStatus));
        }

        private void LoadMerchandiseList()
        {
            MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
            cbMerchandise.DisplayMember = "MerchandiseName";
            cbMerchandise.ValueMember = "MerchandiseID";
            cbMerchandise.DataSource = merchandiseBo.GetList("");
        }

        private void LoadOrderList()
        {
            comboBoxOrder.DataSource = null;
            OrderBO orderBo = new OrderBO(_connString);
            string filter = "";
            filter = "CustomerID=" + comboBoxCustomer.SelectedValue;
            comboBoxOrder.DisplayMember = "OrderName";
            comboBoxOrder.ValueMember = "OrderID";
            List<OrderModel> orders = orderBo.GetList(filter);
            EnableItemDetails(false);
            if (orders.Count > 0)
            {
                comboBoxOrder.DataSource = orders;
                EnableDetails(true);
            }
            else
                EnableDetails(false);
        }

        private void LoadOrderItemList(List<OrderItemModel> orderItems)
        {
            cbOrderItem.DataSource = null;
            cbOrderItem.DisplayMember = "OrderItemName";
            cbOrderItem.ValueMember = "OrderItemID";
            cbOrderItem.DataSource = orderItems;
            if (orderItems.Count > 0)
                EnableItemDetails(true);
        }

        private void LoadOrderItemDetail()
        {
            if (cbOrderItem.SelectedValue != null)
            {
                OrderItemBO orderItemBo = new OrderItemBO(_connString);
                OrderItemModel orderItem = orderItemBo.GetByID((int)cbOrderItem.SelectedValue);
                cbMerchandise.SelectedValue = orderItem.MerchandiseID;
                nudNumer.Value = orderItem.Number;
                nudDiscountPercent.Value = orderItem.DiscountPercent.HasValue ? (decimal)orderItem.DiscountPercent.Value : 0;
                nudDiscountValue.Value = orderItem.DiscountValue.HasValue ? (decimal)orderItem.DiscountValue.Value : 0;
                txtOriginalPrice.Text = orderItem.Merchandise.USDPrice.ToString();
            }
        }

        private void LoadCustomer()
        {
            if(comboBoxCustomer.SelectedValue != null)
            {
                CustomerBO customerBo = new CustomerBO(_connString);
                CustomerModel customer = customerBo.GetByID((int)comboBoxCustomer.SelectedValue);
                if(customer.CustomerPicture != null)
                {
                    pbCustomer.Image = ByteToImage(customer.CustomerPicture);
                    pbCustomer.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pbCustomer.Image = null;
                }
            }
        }

        private void comboBoxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomer();
            LoadOrderList();
        }

        private void comboBoxOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            if (comboBoxOrder.SelectedValue != null)
            {
                OrderBO orderBo = new OrderBO(_connString);
                OrderModel order = orderBo.GetByID((int)comboBoxOrder.SelectedValue);
                dtOrderDate.Value = order.OrderDate;
                dtShipDate.Checked = false;
                if (order.ShipmentDate.HasValue)
                {
                    dtShipDate.Checked = true;
                    dtShipDate.Value = order.ShipmentDate.Value;
                }
                cbStatus.SelectedIndex = order.OrderStatus;
                nudSalePrice.Value = order.ChargedPrice.HasValue ? (decimal)order.ChargedPrice.Value : 0;
                nudShipCost.Value = order.ShippingCost.HasValue ? (decimal)order.ShippingCost.Value : 0;
                txtProfit.Text = order.Profit.ToString();
                txtShippingNumber.Text = order.ShipmentNumber;
                LoadOrderItemList(order.OrderItems);
            }
        }

        private void EnableDetails(bool enable)
        {
            comboBoxOrder.Visible = enable;
            dtOrderDate.Visible = enable;
            dtShipDate.Visible = enable;
            cbStatus.Visible = enable;
            nudSalePrice.Visible = enable;
            nudShipCost.Visible = enable;
            txtShippingNumber.Visible = enable;
            cbOrderItem.Visible = enable;
            txtProfit.Visible = enable;
        }

        private void EnableItemDetails(bool enable)
        {
            cbMerchandise.Visible = enable;
            nudNumer.Visible = enable;
            nudDiscountPercent.Visible = enable;
            nudDiscountValue.Visible = enable;
            txtOriginalPrice.Visible = enable;
            pbMerchandise.Visible = enable;
        }

        private void cbOrderItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrderItemDetail();
        }

        private void cbMerchandise_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMerchandise.SelectedValue != null)
            {
                MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
                MerchandiseModel merchandise = merchandiseBo.GetByID((int)cbMerchandise.SelectedValue);
                txtOriginalPrice.Text = merchandise.USDPrice.ToString();
                if (merchandise.Picture != null)
                {
                    pbMerchandise.Image = ByteToImage(merchandise.Picture);
                    pbMerchandise.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pbMerchandise.Image = null;
                }
            }
        }

        private Bitmap ByteToImage(byte[] blob)
        {
            byte[] pData = blob;
            MemoryStream mStream = new MemoryStream();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
    }
}
