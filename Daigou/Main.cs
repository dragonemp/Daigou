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
            cbMerchandise.SelectedIndex = -1;
        }

        private void LoadOrderList()
        {
            EnableDetails(false);
            comboBoxOrder.DataSource = null;
            if (comboBoxCustomer.SelectedValue != null && ((int)comboBoxCustomer.SelectedValue) > 0)
            {
                OrderBO orderBo = new OrderBO(_connString);
                string filter = "";
                filter = "CustomerID=" + comboBoxCustomer.SelectedValue;
                comboBoxOrder.DisplayMember = "OrderName";
                comboBoxOrder.ValueMember = "OrderID";
                List<OrderModel> orders = orderBo.GetList(filter);
                ClearMerchandise();
                if (orders.Count > 0)
                {
                    comboBoxOrder.DataSource = orders;
                    EnableDetails(true);
                }                    
            }
        }

        private void LoadOrderItemList(List<OrderItemModel> orderItems)
        {
            cbOrderItem.DataSource = null;
            cbOrderItem.DisplayMember = "OrderItemName";
            cbOrderItem.ValueMember = "OrderItemID";
            cbOrderItem.DataSource = orderItems;
            if (orderItems.Count <= 1)
                ClearMerchandise();
        }

        private void LoadOrderItemList()
        {
            if (comboBoxOrder.SelectedValue != null && (int)comboBoxOrder.SelectedValue > 0)
            {
                cbOrderItem.DataSource = null;
                cbOrderItem.DisplayMember = "OrderItemName";
                cbOrderItem.ValueMember = "OrderItemID";
                OrderItemBO orderItemBo = new OrderItemBO(_connString);
                List<OrderItemModel> orderItems = orderItemBo.GetList("OrderID=" + comboBoxOrder.SelectedValue.ToString(), true);
                cbOrderItem.DataSource = orderItems;
                if (orderItems.Count <= 1)
                    ClearMerchandise();
            }
        }

        private void LoadOrderItemDetail()
        {
            if (cbOrderItem.SelectedValue != null && ((int)cbOrderItem.SelectedValue) > 0)
            {
                OrderItemBO orderItemBo = new OrderItemBO(_connString);
                OrderItemModel orderItem = orderItemBo.GetByID((int)cbOrderItem.SelectedValue);
                cbMerchandise.SelectedValue = orderItem.MerchandiseID;
                nudNumer.Value = orderItem.Number;
                nudDiscountPercent.Value = orderItem.DiscountPercent.HasValue ? (decimal)orderItem.DiscountPercent.Value : 0;
                nudDiscountValue.Value = orderItem.DiscountValue.HasValue ? (decimal)orderItem.DiscountValue.Value : 0;
                txtOriginalPrice.Text = orderItem.Merchandise.USDPrice.ToString();
            }
            else
            {
                ClearMerchandise();
            }            
        }

        private void ClearMerchandise()
        {
            cbMerchandise.SelectedIndex = -1;
            nudNumer.Value = 0;
            nudDiscountPercent.Value = 0;
            nudDiscountValue.Value = 0;
            txtOriginalPrice.Text = "";
            pbMerchandise.Image = null;
        }

        private void LoadCustomer()
        {
            if(comboBoxCustomer.SelectedValue != null && ((int)comboBoxCustomer.SelectedValue)>0)
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

        private void LoadOrderDetails()
        {
            if (comboBoxOrder.SelectedValue != null && ((int)comboBoxOrder.SelectedValue) > 0)
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

        private void LoadMerchandiseDetail()
        {
            if (cbMerchandise.SelectedValue != null && ((int)cbMerchandise.SelectedValue) > 0)
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
            else
            {
                ClearMerchandise();
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
            EnableOrderDetails(comboBoxOrder.SelectedValue != null && ((int)comboBoxOrder.SelectedValue) > 0);
        }

        private void EnableDetails(bool enable)
        {
            comboBoxOrder.Visible = enable;
            EnableOrderDetails(comboBoxOrder.SelectedValue != null && ((int)comboBoxOrder.SelectedValue) > 0);
        }

        private void EnableOrderDetails(bool enable)
        {
            dtOrderDate.Visible = enable;
            dtShipDate.Visible = enable;
            cbStatus.Visible = enable;
            nudSalePrice.Visible = enable;
            nudShipCost.Visible = enable;
            txtShippingNumber.Visible = enable;
            cbOrderItem.Visible = enable;
            txtProfit.Visible = enable;
            btnSaveItem.Visible = enable;
            btnDeleteItem.Visible = enable;
            if (!enable)
                ClearMerchandise();
        }

        private void cbOrderItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrderItemDetail();
        }

        private void cbMerchandise_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMerchandiseDetail();
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

        private OrderItemModel GetOrderItemContract()
        {
            OrderItemModel orderItem = new OrderItemModel();
            if (cbMerchandise.SelectedValue != null)
                orderItem.MerchandiseID = (int)cbMerchandise.SelectedValue;
            orderItem.Number = (int)nudNumer.Value;
            if (nudDiscountValue.Value > 0)
            {
                orderItem.DiscountValue = nudDiscountValue.Value;
                orderItem.DiscountPercent = 0;
            }
            else
            {
                orderItem.DiscountPercent = nudDiscountPercent.Value;
                orderItem.DiscountValue = 0;
            }
            if (comboBoxOrder.SelectedValue != null)
                orderItem.OrderID = (int)comboBoxOrder.SelectedValue;
            if (cbOrderItem.SelectedValue != null)
                orderItem.OrderItemID = (int)cbOrderItem.SelectedValue;
            return orderItem;
        }

        private OrderModel GetOrderContract()
        {
            OrderModel order = new OrderModel();
            if (comboBoxOrder.SelectedValue != null)
                order.OrderID = (int)comboBoxOrder.SelectedValue;
            if (comboBoxCustomer.SelectedValue != null)
                order.CustomerID = (int)comboBoxCustomer.SelectedValue;
            if (nudSalePrice.Value > 0)
                order.ChargedPrice = nudSalePrice.Value;
            order.OrderStatus = (int)cbStatus.SelectedValue;
            order.OrderDate = dtOrderDate.Value;
            if (dtShipDate.Checked)
                order.ShipmentDate = dtShipDate.Value;
            if (nudShipCost.Value > 0)
                order.ShippingCost = nudShipCost.Value;
            order.ShipmentNumber = txtShippingNumber.Text;
            return order;
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            OrderItemBO orderItemBo = new OrderItemBO(_connString);
            int pk = orderItemBo.Save(GetOrderItemContract());
            if(orderItemBo.ValidationErrors.Count > 0)
            {
                string errs = "";
                foreach (string error in orderItemBo.ValidationErrors)
                {
                    errs += error + Environment.NewLine;
                }
                MessageBox.Show(errs);
            }
            else
            {
                LoadOrderItemList();
                cbOrderItem.SelectedValue = pk;
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (cbOrderItem.SelectedValue != null && (int)cbOrderItem.SelectedValue > 0)
            {
                OrderItemBO orderItemBo = new OrderItemBO(_connString);
                bool deleted = orderItemBo.Delete((int)cbOrderItem.SelectedValue);
                if (!deleted)
                {
                    if (orderItemBo.ValidationErrors.Count > 0)
                    {
                        string errs = "";
                        foreach (string error in orderItemBo.ValidationErrors)
                        {
                            errs += error + Environment.NewLine;
                        }
                        MessageBox.Show(errs);
                    }
                    else
                    {
                        MessageBox.Show("Error Occurred");
                    }
                }
                else
                {
                    LoadOrderItemList();
                    ClearMerchandise();
                    MessageBox.Show("Delete Complete");
                }
            }
            else
            {
                MessageBox.Show("Please choose an order item first");
            }
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            if(comboBoxCustomer.SelectedValue != null && (int)comboBoxCustomer.SelectedValue > 0)
            {
                OrderModel order = new OrderModel();
                order.CustomerID = (int)comboBoxCustomer.SelectedValue;
                order.OrderDate = DateTime.Now;
                order.OrderStatus = (int)OrderStatus.Pending;
                OrderBO orderBo = new OrderBO(_connString);
                int pk = orderBo.Save(order);
                LoadOrderList();
                comboBoxOrder.SelectedValue = pk;
            }
            else
            {
                MessageBox.Show("Please select a customer first");
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if(comboBoxOrder.SelectedValue != null && (int)comboBoxOrder.SelectedValue > 0)
            {
                OrderBO orderBo = new OrderBO(_connString);
                bool deleted = orderBo.Delete((int)comboBoxOrder.SelectedValue);
                if (!deleted)
                {
                    if (orderBo.ValidationErrors.Count > 0)
                    {
                        string errs = "";
                        foreach (string error in orderBo.ValidationErrors)
                        {
                            errs += error + Environment.NewLine;
                        }
                        MessageBox.Show(errs);
                    }
                    else
                    {
                        MessageBox.Show("Error Occurred");
                    }
                }
                else
                {
                    LoadOrderList();
                    MessageBox.Show("Save Complete");
                }
            }
            else
            {
                MessageBox.Show("Please choose an order first");
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            OrderBO orderBo = new OrderBO(_connString);
            int pk = orderBo.Save(GetOrderContract());
            if (pk <= 0)
            {
                if (orderBo.ValidationErrors.Count > 0)
                {
                    string errs = "";
                    foreach (string error in orderBo.ValidationErrors)
                    {
                        errs += error + Environment.NewLine;
                    }
                    MessageBox.Show(errs);
                }
                else
                {
                    MessageBox.Show("Error Occurred");
                }
            }
            else
            {
                LoadOrderList();
                comboBoxOrder.SelectedValue = pk;
                MessageBox.Show("Save Complete");
            }
        }
    }
}
