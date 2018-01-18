using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using PurchaseHelper.BusinessObjects;
using PurchaseHelper.Models;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;

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
            List<CustomerModel> customers = customerBo.GetList("", true);
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.ValueMember = "CustomerId";
            comboBoxCustomer.DataSource = customers;
            cbCustomerList.DisplayMember = "Name";
            cbCustomerList.ValueMember = "CustomerId";
            cbCustomerList.DataSource = customers;
            cbClientSearch.DisplayMember = "Name";
            cbClientSearch.ValueMember = "CustomerId";
            cbClientSearch.DataSource = customers;
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

        private void ClearMerchandise()
        {
            cbMerchandise.SelectedIndex = -1;
            nudNumer.Value = 0;
            nudDiscountPercent.Value = 0;
            nudDiscountValue.Value = 0;
            nudPurchasePrice.Value = 0;
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
                    pbCustomer.SizeMode = PictureBoxSizeMode.Zoom;
                    pbCustomerPic.Image = img;
                    pbCustomerPic.SizeMode = PictureBoxSizeMode.Zoom;
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
                lblProfit.Text = order.Profit.HasValue ? order.Profit.Value.ToString("f2") : "0";
                nudPurchasePrice.Value = order.PurchasePrice.HasValue ? order.PurchasePrice.Value : order.Merchandise != null ? order.Merchandise.USDPrice * (1 + order.Merchandise.Tax / 100) : 0;
                cbMerchandise.SelectedValue = order.MerchandiseID;
                nudNumer.Value = order.Number;
                nudDiscountPercent.Value = order.DiscountPercent.HasValue ? order.DiscountPercent.Value : 0;
                nudDiscountValue.Value = order.DiscountValue.HasValue ? order.DiscountValue.Value : 0;
                if (order.Profit.HasValue)
                {
                    if (order.Profit.Value >= 0)
                        lblProfit.ForeColor = Color.Green;
                    else
                        lblProfit.ForeColor = Color.Red;
                }
                nudExchangeRate.Value = order.ExchangeRate.HasValue ? order.ExchangeRate.Value : order.RealTimeExchangeRate;
                txtShippingNumber.Text = order.ShipmentNumber;
            }
        }

        private void LoadMerchandiseDetail()
        {
            if (cbMerchandise.SelectedIndex >= 0)
            {
                MerchandiseBO merchandiseBo = new MerchandiseBO(_connString);
                MerchandiseModel merchandise = merchandiseBo.GetByID((int)cbMerchandise.SelectedValue);
                nudPurchasePrice.Value = merchandise.USDPrice * (1 + merchandise.Tax / 100);
                if (merchandise.Picture != null)
                {
                    Bitmap img = ByteToImage(merchandise.Picture);
                    pbMerchandise.Image = img;
                    pbMerchandise.SizeMode = PictureBoxSizeMode.Zoom;
                    pbMerchandisePic.Image = img;
                    pbMerchandisePic.SizeMode = PictureBoxSizeMode.Zoom;
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
                nudTax.Value = merchandise.Tax;
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
            lblProfit.Visible = enable;
            nudPurchasePrice.Visible = enable;
            nudDiscountValue.Visible = enable;
            nudDiscountPercent.Visible = enable;
            cbMerchandise.Visible = enable;
            nudNumer.Visible = enable;
            nudExchangeRate.Visible = enable;
            pbMerchandise.Visible = enable;
            if (!enable)
                ClearMerchandise();
        }

        private void cbMerchandise_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMerchandiseDetail();
            txtMerchandisePic.Text = "";
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
            return orderItem;
        }

        private OrderModel GetOrderContract()
        {
            OrderModel order = new OrderModel();
            if (comboBoxOrder.SelectedValue != null)
                order.OrderID = (int)comboBoxOrder.SelectedValue;
            if (comboBoxCustomer.SelectedValue != null)
                order.CustomerID = (int)comboBoxCustomer.SelectedValue;
            if (nudSalePrice.Value >= 0)
                order.ChargedPrice = nudSalePrice.Value;
            order.OrderStatus = (int)cbStatus.SelectedValue;
            order.OrderDate = dtOrderDate.Value;
            if (dtShipDate.Checked)
                order.ShipmentDate = dtShipDate.Value;
            if (nudShipCost.Value >= 0)
                order.ShippingCost = nudShipCost.Value;
            order.ShipmentNumber = txtShippingNumber.Text;
            order.ExchangeRate = nudExchangeRate.Value;
            order.DiscountPercent = nudDiscountPercent.Value;
            order.DiscountValue = nudDiscountValue.Value;
            order.PurchasePrice = nudPurchasePrice.Value;
            order.MerchandiseID = (int)cbMerchandise.SelectedValue;
            order.Number = (int)nudNumer.Value;
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
            merchandise.Tax = nudTax.Value;
            string fileName = txtMerchandisePic.Text;
            if (!string.IsNullOrEmpty(fileName))
            {
                byte[] bytes = File.ReadAllBytes(fileName);
                merchandise.Picture = bytes;
            }

            return merchandise;
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
                order.MerchandiseID = 1;
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
            var confirmDelete = MessageBox.Show("Are you sure to delete this order?", "Confirm Delete!", MessageBoxButtons.YesNo);

            if (confirmDelete == DialogResult.Yes)
            {
                if (comboBoxOrder.SelectedValue != null && (int)comboBoxOrder.SelectedValue > 0)
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
            cbCustomerList.SelectedIndex = 0;
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtCountry.Text = "";
            txtPhone.Text = "";
            txtZip.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtIM.Text = "";
            txtID.Text = "";
            txtCustomerPic.Text = "";
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            var confirmDelete = MessageBox.Show("Are you sure to delete this customer?", "Confirm Delete!", MessageBoxButtons.YesNo);

            if (confirmDelete == DialogResult.Yes)
            {
                if (cbCustomerList.SelectedValue != null && (int)cbCustomerList.SelectedValue > 0)
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
            txtMerchandisePic.Text = "";
            toolStripStatusLabel3.Text = "";
        }

        private void btnDeleteMerchandise_Click(object sender, EventArgs e)
        {
            var confirmDelete = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete!", MessageBoxButtons.YesNo);

            if (confirmDelete == DialogResult.Yes)
            {
                if (cbMerchandiseList.SelectedValue != null && (int)cbCustomerList.SelectedValue > 0)
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
        }

        private void btnNewMerchandise_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = "Creating...";
            MerchandiseModel contract = new MerchandiseModel();
            contract.Name = "";
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //pictureBox2.Hide();
        }

        private void pbMerchandisePic_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime start = dtStartSearch.Value;
            DateTime end = dtEndSearch.Value;
            int customerId = -1;
            if (cbClientSearch.SelectedIndex > 0)
                customerId = (int)cbClientSearch.SelectedValue;
            string filter = "OrderDate>='" + start.ToString() + "' AND OrderDate<='" + end.ToString() + "'";
            if (customerId > 0)
                filter += " AND CustomerID=" + customerId;
            OrderBO bo = new OrderBO(_connString);
            List<OrderSearchModel> result = bo.GetList(filter).Select(x=> new OrderSearchModel() { 编号 =x.OrderID.Value,  状态 = ((OrderStatus)x.OrderStatus).ToString(),  客户 = x.CustomerName,  商品 = x.MerchandiseName,  数量 = x.Number,  订货日期 = x.OrderDate,  利润 = x.Profit,  发货日期 = x.ShipmentDate, 买价 = x.PurchasePrice, 卖价 = x.ChargedPrice, 运费 = x.ShippingCost }).ToList();
            SortableBindingList<OrderSearchModel> resultSortable = new SortableBindingList<OrderSearchModel>(result);
            int totalCount = result.Count();
            decimal totalProfit = 0, totalIncome = 0, totalCost = 0, totalShipping = 0;
            foreach(OrderSearchModel order in result)
            {
                totalProfit += order.利润.HasValue ? order.利润.Value : 0;
                totalCost += order.买价.HasValue ? order.买价.Value : 0;
                totalIncome += order.卖价.HasValue ? order.卖价.Value : 0;
                totalShipping += order.运费.HasValue ? order.运费.Value : 0;
            }
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.HeaderText = "打印";
            chk.Name = "chkPrint";
            dgSearch.Columns.Add(chk);
            dgSearch.DataSource = resultSortable;
            dgSearch.Refresh();
            lblTotalCount.Text = totalCount.ToString();
            lblTotalProfit.Text = totalProfit.ToString("f2");
            lblTotalIncome.Text = totalIncome.ToString("f2");
            lblTotalCost.Text = totalCost.ToString("f2");
            lblTotalShipping.Text = totalShipping.ToString("f2");
            if (totalProfit > 0)
                lblTotalProfit.ForeColor = Color.Green;
            else
                lblTotalProfit.ForeColor = Color.Red;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<int> orders = new List<int>();
            foreach (DataGridViewRow row in dgSearch.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chkPrint"].Value) == true)
                {
                    orders.Add((int)row.Cells["编号"].Value);
                }
            }
            string location = System.Reflection.Assembly.GetExecutingAssembly().EscapedCodeBase;
            string path = System.IO.Path.GetDirectoryName(location).Replace("file:\\", "").Replace("\\", "/");
            if (path.Substring(path.Length - 3, 3).ToLowerInvariant() == "bin")
            {
                path = path.Substring(0, path.Length - 3);
            }
            if (path.Substring(path.Length - 1, 1) != "/")
            {
                path += "/";
            }
            ReportDocument report = new ReportDocument();
            string rptFile = path + "order.rpt";
            string pdfFile = path + Path.GetRandomFileName() + ".pdf";
            report.Load(rptFile);
            report.SetDataSource(FillReportData(orders));
            report.Refresh();
            report.ExportToDisk(ExportFormatType.PortableDocFormat, pdfFile);
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            process.StartInfo = startInfo;

            startInfo.FileName = pdfFile;
            process.Start();
        }

        private orderDataSet FillReportData(List<int> orderIds)
        {
            OrderBO bo = new OrderBO(_connString);
            orderDataSet orderData = new orderDataSet();
            string filter = "OrderID IN (";
            foreach (int orderId in orderIds)
            {
                filter += orderId + ",";
            }
            filter = filter.Substring(0, filter.Length - 1) + ")";
            List<OrderModel> orders = bo.GetList(filter);
            List<CustomerModel> customers = new List<CustomerModel>();
            List<MerchandiseModel> merchandises = new List<MerchandiseModel>();
            foreach(OrderModel order in orders)
            {
                if(!customers.Any(x=>x.CustomerId == order.CustomerID))
                {
                    customers.Add(order.Customer);
                }
                if(!merchandises.Any(x=>x.MerchandiseID == order.MerchandiseID))
                {
                    merchandises.Add(order.Merchandise);
                }
                DataRow drOrder = orderData.Tables["Orders"].NewRow();
                drOrder["OrderID"] = order.OrderID;
                drOrder["CustomerID"] = order.CustomerID;
                drOrder["MerchandiseID"] = order.MerchandiseID;
                drOrder["Number"] = order.Number;
                orderData.Tables["Orders"].Rows.Add(drOrder);
            }
            foreach(CustomerModel customer in customers)
            {
                DataRow drCustomer = orderData.Tables["Customer"].NewRow();
                drCustomer["CustomerId"] = customer.CustomerId;
                drCustomer["FirstName"] = customer.FirstName;
                drCustomer["LastName"] = customer.LastName;
                drCustomer["Address1"] = customer.Address1;
                drCustomer["Address2"] = customer.Address2;
                drCustomer["City"] = customer.City;
                drCustomer["State"] = customer.State;
                drCustomer["Country"] = customer.Country;
                drCustomer["Zip"] = customer.Zip;
                drCustomer["IdentificationNumber"] = customer.IdentificationNumber;
                drCustomer["Phone"] = customer.Phone;
                drCustomer["CustomerPicture"] = customer.CustomerPicture;
                orderData.Tables["Customer"].Rows.Add(drCustomer);
            }
            foreach (MerchandiseModel merchandise in merchandises)
            {
                DataRow drMerchandise = orderData.Tables["Merchandise"].NewRow();
                drMerchandise["MerchandiseID"] = merchandise.MerchandiseID;
                drMerchandise["Name"] = merchandise.Name;
                drMerchandise["Picture"] = merchandise.Picture;
                orderData.Tables["Merchandise"].Rows.Add(drMerchandise);
            }
            return orderData;
        }
    }

    /// <summary>
    /// Provides a generic collection that supports data binding and additionally supports sorting.
    /// See http://msdn.microsoft.com/en-us/library/ms993236.aspx
    /// If the elements are IComparable it uses that; otherwise compares the ToString()
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SortableBindingList<T> : BindingList<T> where T : class
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor _sortProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        public SortableBindingList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        /// <param name="list">An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the <see cref="T:System.ComponentModel.BindingList`1" />.</param>
        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return _isSorted; }
        }

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        /// <summary>
        /// Removes any sort applied with ApplySortCore if sorting is implemented
        /// </summary>
        protected override void RemoveSortCore()
        {
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
            _isSorted = false; //thanks Luca
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            _sortProperty = prop;
            _sortDirection = direction;

            List<T> list = Items as List<T>;
            if (list == null) return;

            list.Sort(Compare);

            _isSorted = true;
            //fire an event that the list has been changed.
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }


        private int Compare(T lhs, T rhs)
        {
            var result = OnComparison(lhs, rhs);
            //invert if descending
            if (_sortDirection == ListSortDirection.Descending)
                result = -result;
            return result;
        }

        private int OnComparison(T lhs, T rhs)
        {
            object lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
            object rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);
            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1; //nulls are equal
            }
            if (rhsValue == null)
            {
                return 1; //first has value, second doesn't
            }
            if (lhsValue is IComparable)
            {
                return ((IComparable)lhsValue).CompareTo(rhsValue);
            }
            if (lhsValue.Equals(rhsValue))
            {
                return 0; //both are the same
            }
            //not comparable, compare ToString
            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }
    }
}
