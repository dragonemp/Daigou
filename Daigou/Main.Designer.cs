namespace Daigou
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxCustomer = new System.Windows.Forms.ComboBox();
            this.comboBoxOrder = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtOrderDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtShipDate = new System.Windows.Forms.DateTimePicker();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudSalePrice = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudShipCost = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProfit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtShippingNumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbOrderItem = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbMerchandise = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.nudNumer = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.nudDiscountPercent = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.nudDiscountValue = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOriginalPrice = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.pbMerchandise = new System.Windows.Forms.PictureBox();
            this.pbCustomer = new System.Windows.Forms.PictureBox();
            this.btnSaveItem = new System.Windows.Forms.Button();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnSaveOrder = new System.Windows.Forms.Button();
            this.btnNewOrder = new System.Windows.Forms.Button();
            this.btnDeleteOrder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSalePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShipCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscountPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscountValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMerchandise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxCustomer
            // 
            this.comboBoxCustomer.FormattingEnabled = true;
            this.comboBoxCustomer.Location = new System.Drawing.Point(75, 13);
            this.comboBoxCustomer.Name = "comboBoxCustomer";
            this.comboBoxCustomer.Size = new System.Drawing.Size(200, 21);
            this.comboBoxCustomer.TabIndex = 0;
            this.comboBoxCustomer.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomer_SelectedIndexChanged);
            // 
            // comboBoxOrder
            // 
            this.comboBoxOrder.FormattingEnabled = true;
            this.comboBoxOrder.Location = new System.Drawing.Point(75, 41);
            this.comboBoxOrder.Name = "comboBoxOrder";
            this.comboBoxOrder.Size = new System.Drawing.Size(200, 21);
            this.comboBoxOrder.TabIndex = 1;
            this.comboBoxOrder.SelectedIndexChanged += new System.EventHandler(this.comboBoxOrder_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "订货时间";
            // 
            // dtOrderDate
            // 
            this.dtOrderDate.Location = new System.Drawing.Point(75, 69);
            this.dtOrderDate.Name = "dtOrderDate";
            this.dtOrderDate.Size = new System.Drawing.Size(200, 20);
            this.dtOrderDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "发货时间";
            // 
            // dtShipDate
            // 
            this.dtShipDate.Location = new System.Drawing.Point(75, 96);
            this.dtShipDate.Name = "dtShipDate";
            this.dtShipDate.ShowCheckBox = true;
            this.dtShipDate.Size = new System.Drawing.Size(200, 20);
            this.dtShipDate.TabIndex = 5;
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(75, 123);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(121, 21);
            this.cbStatus.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "状态";
            // 
            // nudSalePrice
            // 
            this.nudSalePrice.DecimalPlaces = 2;
            this.nudSalePrice.Location = new System.Drawing.Point(75, 151);
            this.nudSalePrice.Name = "nudSalePrice";
            this.nudSalePrice.Size = new System.Drawing.Size(120, 20);
            this.nudSalePrice.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "卖价";
            // 
            // nudShipCost
            // 
            this.nudShipCost.DecimalPlaces = 2;
            this.nudShipCost.Location = new System.Drawing.Point(75, 178);
            this.nudShipCost.Name = "nudShipCost";
            this.nudShipCost.Size = new System.Drawing.Size(120, 20);
            this.nudShipCost.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "运费";
            // 
            // txtProfit
            // 
            this.txtProfit.Enabled = false;
            this.txtProfit.Location = new System.Drawing.Point(75, 205);
            this.txtProfit.Name = "txtProfit";
            this.txtProfit.ReadOnly = true;
            this.txtProfit.Size = new System.Drawing.Size(121, 20);
            this.txtProfit.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "利润";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(37, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "客户";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "订单";
            // 
            // txtShippingNumber
            // 
            this.txtShippingNumber.Location = new System.Drawing.Point(75, 232);
            this.txtShippingNumber.Name = "txtShippingNumber";
            this.txtShippingNumber.Size = new System.Drawing.Size(200, 20);
            this.txtShippingNumber.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 239);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "运单号";
            // 
            // cbOrderItem
            // 
            this.cbOrderItem.FormattingEnabled = true;
            this.cbOrderItem.Location = new System.Drawing.Point(75, 259);
            this.cbOrderItem.Name = "cbOrderItem";
            this.cbOrderItem.Size = new System.Drawing.Size(200, 21);
            this.cbOrderItem.TabIndex = 19;
            this.cbOrderItem.SelectedIndexChanged += new System.EventHandler(this.cbOrderItem_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 267);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "订单项目";
            // 
            // cbMerchandise
            // 
            this.cbMerchandise.FormattingEnabled = true;
            this.cbMerchandise.Location = new System.Drawing.Point(74, 286);
            this.cbMerchandise.Name = "cbMerchandise";
            this.cbMerchandise.Size = new System.Drawing.Size(200, 21);
            this.cbMerchandise.TabIndex = 21;
            this.cbMerchandise.SelectedIndexChanged += new System.EventHandler(this.cbMerchandise_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 294);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "商品";
            // 
            // nudNumer
            // 
            this.nudNumer.Location = new System.Drawing.Point(74, 314);
            this.nudNumer.Name = "nudNumer";
            this.nudNumer.Size = new System.Drawing.Size(120, 20);
            this.nudNumer.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(37, 321);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "数量";
            // 
            // nudDiscountPercent
            // 
            this.nudDiscountPercent.Location = new System.Drawing.Point(74, 341);
            this.nudDiscountPercent.Name = "nudDiscountPercent";
            this.nudDiscountPercent.Size = new System.Drawing.Size(120, 20);
            this.nudDiscountPercent.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1, 348);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "折扣百分比";
            // 
            // nudDiscountValue
            // 
            this.nudDiscountValue.DecimalPlaces = 2;
            this.nudDiscountValue.Location = new System.Drawing.Point(74, 369);
            this.nudDiscountValue.Name = "nudDiscountValue";
            this.nudDiscountValue.Size = new System.Drawing.Size(120, 20);
            this.nudDiscountValue.TabIndex = 27;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(26, 376);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "折扣价";
            // 
            // txtOriginalPrice
            // 
            this.txtOriginalPrice.Enabled = false;
            this.txtOriginalPrice.Location = new System.Drawing.Point(74, 396);
            this.txtOriginalPrice.Name = "txtOriginalPrice";
            this.txtOriginalPrice.ReadOnly = true;
            this.txtOriginalPrice.Size = new System.Drawing.Size(120, 20);
            this.txtOriginalPrice.TabIndex = 29;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(37, 403);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 30;
            this.label15.Text = "原价";
            // 
            // pbMerchandise
            // 
            this.pbMerchandise.Location = new System.Drawing.Point(282, 285);
            this.pbMerchandise.Name = "pbMerchandise";
            this.pbMerchandise.Size = new System.Drawing.Size(307, 149);
            this.pbMerchandise.TabIndex = 31;
            this.pbMerchandise.TabStop = false;
            // 
            // pbCustomer
            // 
            this.pbCustomer.Location = new System.Drawing.Point(438, 13);
            this.pbCustomer.Name = "pbCustomer";
            this.pbCustomer.Size = new System.Drawing.Size(134, 131);
            this.pbCustomer.TabIndex = 32;
            this.pbCustomer.TabStop = false;
            // 
            // btnSaveItem
            // 
            this.btnSaveItem.Location = new System.Drawing.Point(282, 256);
            this.btnSaveItem.Name = "btnSaveItem";
            this.btnSaveItem.Size = new System.Drawing.Size(40, 23);
            this.btnSaveItem.TabIndex = 33;
            this.btnSaveItem.Text = "保存";
            this.btnSaveItem.UseVisualStyleBackColor = true;
            this.btnSaveItem.Click += new System.EventHandler(this.btnSaveItem_Click);
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Location = new System.Drawing.Point(328, 256);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(40, 23);
            this.btnDeleteItem.TabIndex = 35;
            this.btnDeleteItem.Text = "删除";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnSaveOrder
            // 
            this.btnSaveOrder.Location = new System.Drawing.Point(282, 39);
            this.btnSaveOrder.Name = "btnSaveOrder";
            this.btnSaveOrder.Size = new System.Drawing.Size(40, 23);
            this.btnSaveOrder.TabIndex = 36;
            this.btnSaveOrder.Text = "保存";
            this.btnSaveOrder.UseVisualStyleBackColor = true;
            this.btnSaveOrder.Click += new System.EventHandler(this.btnSaveOrder_Click);
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.Location = new System.Drawing.Point(281, 13);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Size = new System.Drawing.Size(40, 23);
            this.btnNewOrder.TabIndex = 37;
            this.btnNewOrder.Text = "新单";
            this.btnNewOrder.UseVisualStyleBackColor = true;
            this.btnNewOrder.Click += new System.EventHandler(this.btnNewOrder_Click);
            // 
            // btnDeleteOrder
            // 
            this.btnDeleteOrder.Location = new System.Drawing.Point(328, 39);
            this.btnDeleteOrder.Name = "btnDeleteOrder";
            this.btnDeleteOrder.Size = new System.Drawing.Size(40, 23);
            this.btnDeleteOrder.TabIndex = 38;
            this.btnDeleteOrder.Text = "删除";
            this.btnDeleteOrder.UseVisualStyleBackColor = true;
            this.btnDeleteOrder.Click += new System.EventHandler(this.btnDeleteOrder_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 446);
            this.Controls.Add(this.btnDeleteOrder);
            this.Controls.Add(this.btnNewOrder);
            this.Controls.Add(this.btnSaveOrder);
            this.Controls.Add(this.btnDeleteItem);
            this.Controls.Add(this.btnSaveItem);
            this.Controls.Add(this.pbCustomer);
            this.Controls.Add(this.pbMerchandise);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtOriginalPrice);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.nudDiscountValue);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.nudDiscountPercent);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.nudNumer);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbMerchandise);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbOrderItem);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtShippingNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtProfit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudShipCost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudSalePrice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.dtShipDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtOrderDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxOrder);
            this.Controls.Add(this.comboBoxCustomer);
            this.Name = "Main";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nudSalePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShipCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscountPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscountValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMerchandise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCustomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCustomer;
        private System.Windows.Forms.ComboBox comboBoxOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtOrderDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtShipDate;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudSalePrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudShipCost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProfit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtShippingNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbOrderItem;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbMerchandise;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudNumer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudDiscountPercent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudDiscountValue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtOriginalPrice;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pbMerchandise;
        private System.Windows.Forms.PictureBox pbCustomer;
        private System.Windows.Forms.Button btnSaveItem;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnSaveOrder;
        private System.Windows.Forms.Button btnNewOrder;
        private System.Windows.Forms.Button btnDeleteOrder;
    }
}

