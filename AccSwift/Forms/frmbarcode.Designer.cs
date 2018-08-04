using SComponents;
namespace AccSwift.Forms
{
    partial class frmBarCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBarCode));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.rdAllProducts = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rdSpecificProduct = new System.Windows.Forms.RadioButton();
            this.rdSpecificValue = new System.Windows.Forms.RadioButton();
            this.cboProductSingle = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.cboBarCodeType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkIncludeLabel = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxAspectRatio = new System.Windows.Forms.TextBox();
            this.textBoxBarWidth = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpBarCode = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbRotateFlip = new System.Windows.Forms.ComboBox();
            this.cbLabelLocation = new System.Windows.Forms.ComboBox();
            this.lblLabelLocation = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbBarcodeAlign = new System.Windows.Forms.ComboBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNoOfDigits = new System.Windows.Forms.TextBox();
            this.chkAddPadding = new System.Windows.Forms.CheckBox();
            this.txtPadChar = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtData);
            this.groupBox1.Controls.Add(this.rdAllProducts);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rdSpecificProduct);
            this.groupBox1.Controls.Add(this.rdSpecificValue);
            this.groupBox1.Controls.Add(this.cboProductSingle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(30, 322);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(499, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(153, 88);
            this.txtData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(289, 26);
            this.txtData.TabIndex = 87;
            this.txtData.Text = "038000356216";
            // 
            // rdAllProducts
            // 
            this.rdAllProducts.AutoSize = true;
            this.rdAllProducts.Location = new System.Drawing.Point(342, 0);
            this.rdAllProducts.Name = "rdAllProducts";
            this.rdAllProducts.Size = new System.Drawing.Size(118, 24);
            this.rdAllProducts.TabIndex = 10;
            this.rdAllProducts.Text = "All Products";
            this.rdAllProducts.UseVisualStyleBackColor = true;
            this.rdAllProducts.CheckedChanged += new System.EventHandler(this.rdAllProducts_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 20);
            this.label3.TabIndex = 88;
            this.label3.Text = "Value to Encode";
            // 
            // rdSpecificProduct
            // 
            this.rdSpecificProduct.AutoSize = true;
            this.rdSpecificProduct.Checked = true;
            this.rdSpecificProduct.Location = new System.Drawing.Point(21, 2);
            this.rdSpecificProduct.Name = "rdSpecificProduct";
            this.rdSpecificProduct.Size = new System.Drawing.Size(149, 24);
            this.rdSpecificProduct.TabIndex = 9;
            this.rdSpecificProduct.TabStop = true;
            this.rdSpecificProduct.Text = "Specific Product";
            this.rdSpecificProduct.UseVisualStyleBackColor = true;
            this.rdSpecificProduct.CheckedChanged += new System.EventHandler(this.rdSpecificProduct_CheckedChanged);
            // 
            // rdSpecificValue
            // 
            this.rdSpecificValue.AutoSize = true;
            this.rdSpecificValue.Location = new System.Drawing.Point(183, 0);
            this.rdSpecificValue.Name = "rdSpecificValue";
            this.rdSpecificValue.Size = new System.Drawing.Size(135, 24);
            this.rdSpecificValue.TabIndex = 8;
            this.rdSpecificValue.Text = "Specific Value";
            this.rdSpecificValue.UseVisualStyleBackColor = true;
            this.rdSpecificValue.CheckedChanged += new System.EventHandler(this.rdSpecificValue_CheckedChanged);
            // 
            // cboProductSingle
            // 
            this.cboProductSingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProductSingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductSingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductSingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductSingle.FocusLostColor = System.Drawing.Color.White;
            this.cboProductSingle.FormattingEnabled = true;
            this.cboProductSingle.Location = new System.Drawing.Point(153, 50);
            this.cboProductSingle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboProductSingle.Name = "cboProductSingle";
            this.cboProductSingle.Size = new System.Drawing.Size(289, 28);
            this.cboProductSingle.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Product Name:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(283, 573);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 35);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnprint_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(403, 573);
            this.btncancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(112, 35);
            this.btncancel.TabIndex = 3;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // cboBarCodeType
            // 
            this.cboBarCodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBarCodeType.FormattingEnabled = true;
            this.cboBarCodeType.Items.AddRange(new object[] {
            "UPC-A",
            "UPC-E",
            "UPC 2 Digit Ext.",
            "UPC 5 Digit Ext.",
            "EAN-13",
            "JAN-13",
            "EAN-8",
            "ITF-14",
            "Interleaved 2 of 5",
            "Standard 2 of 5",
            "Codabar",
            "PostNet",
            "Bookland/ISBN",
            "Code 11",
            "Code 39",
            "Code 39 Extended",
            "Code 39 Mod 43",
            "Code 93",
            "Code 128",
            "Code 128-A",
            "Code 128-B",
            "Code 128-C",
            "LOGMARS",
            "MSI",
            "Telepen",
            "FIM",
            "Pharmacode"});
            this.cboBarCodeType.Location = new System.Drawing.Point(149, 52);
            this.cboBarCodeType.Name = "cboBarCodeType";
            this.cboBarCodeType.Size = new System.Drawing.Size(380, 28);
            this.cboBarCodeType.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Bar Code Type:";
            // 
            // chkIncludeLabel
            // 
            this.chkIncludeLabel.AutoSize = true;
            this.chkIncludeLabel.Checked = true;
            this.chkIncludeLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeLabel.Location = new System.Drawing.Point(32, 275);
            this.chkIncludeLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkIncludeLabel.Name = "chkIncludeLabel";
            this.chkIncludeLabel.Size = new System.Drawing.Size(130, 24);
            this.chkIncludeLabel.TabIndex = 8;
            this.chkIncludeLabel.Text = "Include Label";
            this.chkIncludeLabel.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxAspectRatio);
            this.groupBox4.Controls.Add(this.textBoxBarWidth);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.txtWidth);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.txtHeight);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(213, 110);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(316, 140);
            this.groupBox4.TabIndex = 79;
            this.groupBox4.TabStop = false;
            // 
            // textBoxAspectRatio
            // 
            this.textBoxAspectRatio.Location = new System.Drawing.Point(161, 106);
            this.textBoxAspectRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxAspectRatio.Name = "textBoxAspectRatio";
            this.textBoxAspectRatio.Size = new System.Drawing.Size(50, 26);
            this.textBoxAspectRatio.TabIndex = 55;
            // 
            // textBoxBarWidth
            // 
            this.textBoxBarWidth.Location = new System.Drawing.Point(97, 106);
            this.textBoxBarWidth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxBarWidth.Name = "textBoxBarWidth";
            this.textBoxBarWidth.Size = new System.Drawing.Size(50, 26);
            this.textBoxBarWidth.TabIndex = 54;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(141, 88);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 20);
            this.label13.TabIndex = 53;
            this.label13.Text = "AspectRatio";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(69, 88);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 20);
            this.label12.TabIndex = 52;
            this.label12.Text = "BarWidth";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(95, 39);
            this.txtWidth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(50, 26);
            this.txtWidth.TabIndex = 43;
            this.txtWidth.Text = "300";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(91, 19);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 20);
            this.label7.TabIndex = 41;
            this.label7.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(157, 19);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 20);
            this.label6.TabIndex = 42;
            this.label6.Text = "Height";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(159, 39);
            this.txtHeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(50, 26);
            this.txtHeight.TabIndex = 44;
            this.txtHeight.Text = "150";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(146, 43);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 20);
            this.label9.TabIndex = 51;
            this.label9.Text = "x";
            // 
            // grpBarCode
            // 
            this.grpBarCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.grpBarCode.Location = new System.Drawing.Point(549, 12);
            this.grpBarCode.Name = "grpBarCode";
            this.grpBarCode.Size = new System.Drawing.Size(675, 596);
            this.grpBarCode.TabIndex = 80;
            this.grpBarCode.TabStop = false;
            this.grpBarCode.Text = "Barcode Image";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 169);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 20);
            this.label10.TabIndex = 82;
            this.label10.Text = "Rotate";
            // 
            // cbRotateFlip
            // 
            this.cbRotateFlip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotateFlip.FormattingEnabled = true;
            this.cbRotateFlip.Items.AddRange(new object[] {
            "Center",
            "Left",
            "Right"});
            this.cbRotateFlip.Location = new System.Drawing.Point(28, 194);
            this.cbRotateFlip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbRotateFlip.Name = "cbRotateFlip";
            this.cbRotateFlip.Size = new System.Drawing.Size(160, 28);
            this.cbRotateFlip.TabIndex = 81;
            // 
            // cbLabelLocation
            // 
            this.cbLabelLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLabelLocation.FormattingEnabled = true;
            this.cbLabelLocation.Items.AddRange(new object[] {
            "BottomCenter",
            "BottomLeft",
            "BottomRight",
            "TopCenter",
            "TopLeft",
            "TopRight"});
            this.cbLabelLocation.Location = new System.Drawing.Point(330, 274);
            this.cbLabelLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbLabelLocation.Name = "cbLabelLocation";
            this.cbLabelLocation.Size = new System.Drawing.Size(142, 28);
            this.cbLabelLocation.TabIndex = 83;
            // 
            // lblLabelLocation
            // 
            this.lblLabelLocation.AutoSize = true;
            this.lblLabelLocation.Location = new System.Drawing.Point(209, 277);
            this.lblLabelLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLabelLocation.Name = "lblLabelLocation";
            this.lblLabelLocation.Size = new System.Drawing.Size(113, 20);
            this.lblLabelLocation.TabIndex = 84;
            this.lblLabelLocation.Text = "Label Location";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 110);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 20);
            this.label8.TabIndex = 86;
            this.label8.Text = "Alignment";
            // 
            // cbBarcodeAlign
            // 
            this.cbBarcodeAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBarcodeAlign.FormattingEnabled = true;
            this.cbBarcodeAlign.Items.AddRange(new object[] {
            "Center",
            "Left",
            "Right"});
            this.cbBarcodeAlign.Location = new System.Drawing.Point(30, 135);
            this.cbBarcodeAlign.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbBarcodeAlign.Name = "cbBarcodeAlign";
            this.cbBarcodeAlign.Size = new System.Drawing.Size(158, 28);
            this.cbBarcodeAlign.TabIndex = 85;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(163, 573);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(112, 35);
            this.btnPreview.TabIndex = 87;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 88;
            this.label4.Text = "No. of digits:";
            // 
            // txtNoOfDigits
            // 
            this.txtNoOfDigits.Location = new System.Drawing.Point(114, 45);
            this.txtNoOfDigits.MaxLength = 2;
            this.txtNoOfDigits.Name = "txtNoOfDigits";
            this.txtNoOfDigits.Size = new System.Drawing.Size(54, 26);
            this.txtNoOfDigits.TabIndex = 89;
            this.txtNoOfDigits.Text = "11";
            // 
            // chkAddPadding
            // 
            this.chkAddPadding.AutoSize = true;
            this.chkAddPadding.Checked = true;
            this.chkAddPadding.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddPadding.Location = new System.Drawing.Point(21, 0);
            this.chkAddPadding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAddPadding.Name = "chkAddPadding";
            this.chkAddPadding.Size = new System.Drawing.Size(126, 24);
            this.chkAddPadding.TabIndex = 90;
            this.chkAddPadding.Text = "Add Padding";
            this.chkAddPadding.UseVisualStyleBackColor = true;
            this.chkAddPadding.CheckedChanged += new System.EventHandler(this.chkAddPadding_CheckedChanged);
            // 
            // txtPadChar
            // 
            this.txtPadChar.Location = new System.Drawing.Point(371, 45);
            this.txtPadChar.MaxLength = 1;
            this.txtPadChar.Name = "txtPadChar";
            this.txtPadChar.Size = new System.Drawing.Size(73, 26);
            this.txtPadChar.TabIndex = 92;
            this.txtPadChar.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 20);
            this.label5.TabIndex = 91;
            this.label5.Text = "Padding Character:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtPadChar);
            this.groupBox2.Controls.Add(this.chkAddPadding);
            this.groupBox2.Controls.Add(this.txtNoOfDigits);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(28, 473);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(497, 92);
            this.groupBox2.TabIndex = 93;
            this.groupBox2.TabStop = false;
            // 
            // frmBarCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 622);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbBarcodeAlign);
            this.Controls.Add(this.cbLabelLocation);
            this.Controls.Add(this.lblLabelLocation);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbRotateFlip);
            this.Controls.Add(this.grpBarCode);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.chkIncludeLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboBarCodeType);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmBarCode";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bar Code Generator";
            this.Load += new System.EventHandler(this.frmbarcode_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private SComboBox cboProductSingle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.ComboBox cboBarCodeType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkIncludeLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxAspectRatio;
        private System.Windows.Forms.TextBox textBoxBarWidth;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox grpBarCode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbRotateFlip;
        private System.Windows.Forms.ComboBox cbLabelLocation;
        private System.Windows.Forms.Label lblLabelLocation;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbBarcodeAlign;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdAllProducts;
        private System.Windows.Forms.RadioButton rdSpecificProduct;
        private System.Windows.Forms.RadioButton rdSpecificValue;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNoOfDigits;
        private System.Windows.Forms.CheckBox chkAddPadding;
        private System.Windows.Forms.TextBox txtPadChar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}