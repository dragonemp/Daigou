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
            List<CustomerModel> customers = customerBo.GetList("");
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.ValueMember = "CustomerId";
            comboBoxCustomer.DataSource = customers;
            cbCustomerList.DisplayMember = "Name";
            cbCustomerList.ValueMember = "CustomerId";
            cbCustomerList.DataSource = customers;
        }

        private void LoadStatusList()
        {
            cbStatus.DataSource = Enum.GetValues(typeof(OrderStatus));
        }

        private void LoadMerchandiseList()
        {
            MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
            List<MerchandiseModel> merchandise = merchandiseBo.GetList("");
            cbMerchandise.DisplayMember = "MerchandiseName";
            cbMerchandise.ValueMember = "MerchandiseID";
            cbMerchandise.DataSource = merchandise;
            cbMerchandiseList.DisplayMember = "MerchandiseName";
            cbMerchandiseList.ValueMember = "MerchandiseID";
            cbMerchandiseList.DataSource = merchandise;
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
                    Bitmap img = ByteToImage(customer.CustomerPicture);
                    pbCustomer.Image = img;
                    pbCustomer.SizeMode = PictureBoxSizeMode.StretchImage;
                    pbCustomerPic.Image = img;
                    pbCustomerPic.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pbCustomer.Image = null;
                    pbCustomerPic.Image = null;
                }
                txtFirstName.Text = customer.FirstName;
                txtLastName.Text = customer.LastName;
                txtAddress1.Text = customer.Address1;
                txtAddress2.Text = customer.Address2;
                txtCity.Text = customer.City;
                txtState.Text = customer.State;
                txtCountry.Text = customer.Country;
                txtPhone.Text = customer.Phone;
                txtID.Text = customer.IdentificationNumber;
                txtIM.Text = customer.IM;
                txtZip.Text = customer.Zip;
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
                lblProfit.Text = order.Profit.ToString();
                if (order.Profit.Value >= 0)
                    lblProfit.ForeColor = Color.Green;
                else
                    lblProfit.ForeColor = Color.Red;
                lblExchangeRate.Text = order.ExchangeRate.ToString();
                txtShippingNumber.Text = order.ShipmentNumber;
                LoadOrderItemList(order.OrderItems);
            }
        }

        private void LoadMerchandiseDetail()
        {
            if (cbMerchandise.SelectedIndex >= 0)
            {
                MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
                MerchandiseModel merchandise = merchandiseBo.GetByID((int)cbMerchandise.SelectedValue);
                txtOriginalPrice.Text = merchandise.USDPrice.ToString();
                if (merchandise.Picture != null)
                {
                    Bitmap img = ByteToImage(merchandise.Picture);
                    pbMerchandise.Image = img;
                    pbMerchandise.SizeMode = PictureBoxSizeMode.StretchImage;
                    pbMerchandisePic.Image = img;
                    pbMerchandisePic.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pbMerchandise.Image = null;
                    pbMerchandisePic.Image = null;
                }
                txtMerchandiseName.Text = merchandise.Name;
                txtStore.Text = merchandise.Store;
                nudMerchandisePrice.Value = merchandise.USDPrice;
                nudWeight.Value = (decimal)merchandise.weight;
                txtDesc.Text = merchandise.Description;
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
            lblProfit.Visible = enable;
            lblExchangeRate.Visible = enable;
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
        private CustomerModel GetCustomerContract()
        {
            CustomerModel customer = new CustomerModel();
            if (cbCustomerList.SelectedValue != null)
                customer.CustomerId = (int)cbCustomerList.SelectedValue;
            customer.Address1 = txtAddress1.Text;
            customer.Address2 = txtAddress2.Text;
            customer.City = txtCity.Text;
            customer.Country = txtCountry.Text;
            string fileName = txtCustomerPic.Text;
            if (!string.IsNullOrEmpty(fileName))
            {
                byte[] bytes = File.ReadAllBytes(fileName);
                customer.CustomerPicture = bytes;
            }
            customer.FirstName = txtFirstName.Text;
            customer.IdentificationNumber = txtID.Text;
            customer.IM = txtIM.Text;
            customer.LastName = txtLastName.Text;
            customer.Phone = txtPhone.Text;
            customer.State = txtState.Text;
            customer.Zip = txtZip.Text;

            return customer;
        }

        private MerchandiseModel GetMerchandiseContract()
        {
            MerchandiseModel merchandise = new MerchandiseModel();
            if (cbMerchandiseList.SelectedValue != null)
                merchandise.MerchandiseID = (int)cbMerchandiseList.SelectedValue;
            merchandise.Name = txtMerchandiseName.Text;
            merchandise.Store = txtStore.Text;
            merchandise.USDPrice = nudMerchandisePrice.Value;
            merchandise.weight = (double)nudWeight.Value;
            merchandise.Description = txtDesc.Text;
            string fileName = txtMerchandisePic.Text;
            if (!string.IsNullOrEmpty(fileName))
            {
                byte[] bytes = File.ReadAllBytes(fileName);
                merchandise.Picture = bytes;
            }

            return merchandise;
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Saving item...";
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
            toolStripStatusLabel1.Text = "";
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (cbOrderItem.SelectedValue != null && (int)cbOrderItem.SelectedValue > 0)
            {
                toolStripStatusLabel1.Text = "Deleting item...";
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
                toolStripStatusLabel1.Text = "";
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
                toolStripStatusLabel1.Text = "Creating...";
                OrderModel order = new OrderModel();
                order.CustomerID = (int)comboBoxCustomer.SelectedValue;
                order.OrderDate = DateTime.Now;
                order.OrderStatus = (int)OrderStatus.Pending;
                OrderBO orderBo = new OrderBO(_connString);
                int pk = orderBo.Save(order);
                LoadOrderList();
                comboBoxOrder.SelectedValue = pk;
                toolStripStatusLabel1.Text = "";
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
                toolStripStatusLabel1.Text = "Deleting...";
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
                toolStripStatusLabel1.Text = "";
            }
            else
            {
                MessageBox.Show("Please choose an order first");
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Saving...";
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
            toolStripStatusLabel1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                txtCustomerPic.Text = file;
            }
        }

        private void cbCustomerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCustomer.SelectedIndex = cbCustomerList.SelectedIndex;
        }

        private void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Saving...";
            CustomerBO customerBo = new CustomerBO(_connString);
            int pk = customerBo.Save(GetCustomerContract());
            if (pk <= 0)
            {
                if (customerBo.ValidationErrors.Count > 0)
                {
                    string errs = "";
                    foreach (string error in customerBo.ValidationErrors)
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
                LoadCustomerList();
                cbCustomerList.SelectedValue = pk;
                MessageBox.Show("Save Complete");
            }
            toolStripStatusLabel2.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Creating...";
            CustomerModel customer = new CustomerModel();
            customer.FirstName = " ";
            customer.LastName = " ";
            customer.City = " ";
            customer.State = " ";
            customer.Country = " ";
            customer.Phone = " ";
            customer.Zip = " ";
            customer.Address1 = " ";
            CustomerBO customerBo = new CustomerBO(_connString);
            int pk = customerBo.Save(customer);
            if (pk <= 0)
            {
                if (customerBo.ValidationErrors.Count > 0)
                {
                    string errs = "";
                    foreach (string error in customerBo.ValidationErrors)
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
                LoadCustomerList();
                cbCustomerList.SelectedValue = pk;
                MessageBox.Show("Save Complete");
            }
            toolStripStatusLabel2.Text = "";
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if(cbCustomerList.SelectedValue != null && (int)cbCustomerList.SelectedValue > 0)
            {
                toolStripStatusLabel2.Text = "Deleting...";
                CustomerBO customerBo = new CustomerBO(_connString);
                bool deleted = customerBo.Delete((int)cbCustomerList.SelectedValue);
                if (!deleted)
                {
                    if (customerBo.ValidationErrors.Count > 0)
                    {
                        string errs = "";
                        foreach (string error in customerBo.ValidationErrors)
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
                    LoadCustomerList();
                    MessageBox.Show("Delete Complete");
                }
                toolStripStatusLabel2.Text = "";
            }
            else
            {
                MessageBox.Show("Please choose a custoemr first");
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog2.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog2.FileName;
                txtMerchandisePic.Text = file;
            }
        }

        private void btnSaveMerchandise_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = "Saving...";
            MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
            int pk = merchandiseBo.Save(GetMerchandiseContract());
            if (pk <= 0)
            {
                if (merchandiseBo.ValidationErrors.Count > 0)
                {
                    string errs = "";
                    foreach (string error in merchandiseBo.ValidationErrors)
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
                LoadMerchandiseList();
                cbMerchandiseList.SelectedValue = pk;
                MessageBox.Show("Save Complete");
            }
            toolStripStatusLabel3.Text = "";
        }

        private void btnDeleteMerchandise_Click(object sender, EventArgs e)
        {
            if(cbMerchandiseList.SelectedValue != null && (int)cbCustomerList.SelectedValue > 0)
            {
                toolStripStatusLabel3.Text = "Deleting...";
                MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
                bool deleted = merchandiseBo.Delete((int)cbMerchandiseList.SelectedValue);
                if (!deleted)
                {
                    if (merchandiseBo.ValidationErrors.Count > 0)
                    {
                        string errs = "";
                        foreach (string error in merchandiseBo.ValidationErrors)
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
                    LoadMerchandiseList();
                    MessageBox.Show("Delete Complete");
                }
                toolStripStatusLabel3.Text = "";
            }
            else
            {
                MessageBox.Show("Please choose a merchandise first");
            }
        }

        private void btnNewMerchandise_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = "Creating...";
            MerchandiseModel contract = new MerchandiseModel();
            contract.Name = " ";
            MerchandiseBO bo = new MerchandiseBO(_connString);
            int pk = bo.Save(contract);
            if (pk <= 0)
            {
                if (bo.ValidationErrors.Count > 0)
                {
                    string errs = "";
                    foreach (string error in bo.ValidationErrors)
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
                LoadMerchandiseList();
                cbMerchandise.SelectedValue = pk;
                MessageBox.Show("Save Complete");
            }
            toolStripStatusLabel3.Text = "";
        }
    }
}
