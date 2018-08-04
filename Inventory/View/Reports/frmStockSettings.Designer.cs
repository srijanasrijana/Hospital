using SComponents;
namespace Inventory
{
    partial class frmStockSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockSettings));
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.cboProductGroup = new SComponents.SComboBox();
            this.cboProductSingle = new SComponents.SComboBox();
            this.rdProductGroup = new System.Windows.Forms.RadioButton();
            this.rdProductSingle = new System.Windows.Forms.RadioButton();
            this.rdProductAll = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.DateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkShowZeroQty = new System.Windows.Forms.CheckBox();
            this.rdClosingStock = new System.Windows.Forms.RadioButton();
            this.rdOpeningStock = new System.Windows.Forms.RadioButton();
            this.grpStockTypeSelection = new System.Windows.Forms.GroupBox();
            this.btnDate = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComponents.SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsavestate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMonths = new SComponents.SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmboDepot = new SComponents.SComboBox();
            this.grpProduct.SuspendLayout();
            this.grpStockTypeSelection.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectAccClass
            // 
            this.btnSelectAccClass.Location = new System.Drawing.Point(115, 18);
            this.btnSelectAccClass.Name = "btnSelectAccClass";
            this.btnSelectAccClass.Size = new System.Drawing.Size(148, 23);
            this.btnSelectAccClass.TabIndex = 102;
            this.btnSelectAccClass.Text = "Select Account Class";
            this.btnSelectAccClass.UseVisualStyleBackColor = true;
            this.btnSelectAccClass.Click += new System.EventHandler(this.btnSelectAccClass_Click);
            // 
            // grpProduct
            // 
            this.grpProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpProduct.Controls.Add(this.cboProductGroup);
            this.grpProduct.Controls.Add(this.cboProductSingle);
            this.grpProduct.Controls.Add(this.rdProductGroup);
            this.grpProduct.Controls.Add(this.rdProductSingle);
            this.grpProduct.Controls.Add(this.rdProductAll);
            this.grpProduct.Location = new System.Drawing.Point(3, 6);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(292, 78);
            this.grpProduct.TabIndex = 101;
            this.grpProduct.TabStop = false;
            // 
            // cboProductGroup
            // 
            this.cboProductGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboProductGroup.FormattingEnabled = true;
            this.cboProductGroup.Location = new System.Drawing.Point(110, 50);
            this.cboProductGroup.Name = "cboProductGroup";
            this.cboProductGroup.Size = new System.Drawing.Size(156, 21);
            this.cboProductGroup.TabIndex = 64;
            // 
            // cboProductSingle
            // 
            this.cboProductSingle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProductSingle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductSingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductSingle.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProductSingle.FocusLostColor = System.Drawing.Color.White;
            this.cboProductSingle.FormattingEnabled = true;
            this.cboProductSingle.Location = new System.Drawing.Point(110, 23);
            this.cboProductSingle.Name = "cboProductSingle";
            this.cboProductSingle.Size = new System.Drawing.Size(156, 21);
            this.cboProductSingle.TabIndex = 6;
            // 
            // rdProductGroup
            // 
            this.rdProductGroup.AutoSize = true;
            this.rdProductGroup.Location = new System.Drawing.Point(6, 51);
            this.rdProductGroup.Name = "rdProductGroup";
            this.rdProductGroup.Size = new System.Drawing.Size(94, 17);
            this.rdProductGroup.TabIndex = 0;
            this.rdProductGroup.Text = "Product Group";
            this.rdProductGroup.UseVisualStyleBackColor = true;
            this.rdProductGroup.CheckedChanged += new System.EventHandler(this.rdProductGroup_CheckedChanged);
            // 
            // rdProductSingle
            // 
            this.rdProductSingle.AutoSize = true;
            this.rdProductSingle.Location = new System.Drawing.Point(3, 27);
            this.rdProductSingle.Name = "rdProductSingle";
            this.rdProductSingle.Size = new System.Drawing.Size(94, 17);
            this.rdProductSingle.TabIndex = 0;
            this.rdProductSingle.Text = "Single Product";
            this.rdProductSingle.UseVisualStyleBackColor = true;
            this.rdProductSingle.CheckedChanged += new System.EventHandler(this.rdProductSingle_CheckedChanged);
            // 
            // rdProductAll
            // 
            this.rdProductAll.AutoSize = true;
            this.rdProductAll.Location = new System.Drawing.Point(3, 4);
            this.rdProductAll.Name = "rdProductAll";
            this.rdProductAll.Size = new System.Drawing.Size(81, 17);
            this.rdProductAll.TabIndex = 0;
            this.rdProductAll.Text = "All Products";
            this.rdProductAll.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(10, 50);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 23);
            this.button2.TabIndex = 98;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(10, 20);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(77, 23);
            this.btnShow.TabIndex = 97;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // chkDepot
            // 
            this.chkDepot.AutoSize = true;
            this.chkDepot.Location = new System.Drawing.Point(6, 54);
            this.chkDepot.Name = "chkDepot";
            this.chkDepot.Size = new System.Drawing.Size(91, 17);
            this.chkDepot.TabIndex = 103;
            this.chkDepot.Text = "Select Depot:";
            this.chkDepot.UseVisualStyleBackColor = true;
            this.chkDepot.Visible = false;
            this.chkDepot.CheckedChanged += new System.EventHandler(this.chkDepot_CheckedChanged);
            // 
            // DateTextBox
            // 
            this.DateTextBox.Location = new System.Drawing.Point(107, 16);
            this.DateTextBox.Mask = "##/##/####";
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(113, 20);
            this.DateTextBox.TabIndex = 106;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 105;
            this.label2.Text = "At the End of:";
            // 
            // chkShowZeroQty
            // 
            this.chkShowZeroQty.AutoSize = true;
            this.chkShowZeroQty.Checked = true;
            this.chkShowZeroQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowZeroQty.Location = new System.Drawing.Point(3, 283);
            this.chkShowZeroQty.Name = "chkShowZeroQty";
            this.chkShowZeroQty.Size = new System.Drawing.Size(120, 17);
            this.chkShowZeroQty.TabIndex = 107;
            this.chkShowZeroQty.Text = "Show Zero Quantity";
            this.chkShowZeroQty.UseVisualStyleBackColor = true;
            // 
            // rdClosingStock
            // 
            this.rdClosingStock.AutoSize = true;
            this.rdClosingStock.Location = new System.Drawing.Point(17, 18);
            this.rdClosingStock.Name = "rdClosingStock";
            this.rdClosingStock.Size = new System.Drawing.Size(130, 17);
            this.rdClosingStock.TabIndex = 38;
            this.rdClosingStock.TabStop = true;
            this.rdClosingStock.Text = "Closing Stock(Default)";
            this.rdClosingStock.UseVisualStyleBackColor = true;
            // 
            // rdOpeningStock
            // 
            this.rdOpeningStock.AutoSize = true;
            this.rdOpeningStock.Location = new System.Drawing.Point(165, 18);
            this.rdOpeningStock.Name = "rdOpeningStock";
            this.rdOpeningStock.Size = new System.Drawing.Size(96, 17);
            this.rdOpeningStock.TabIndex = 37;
            this.rdOpeningStock.TabStop = true;
            this.rdOpeningStock.Text = "Opening Stock";
            this.rdOpeningStock.UseVisualStyleBackColor = true;
            // 
            // grpStockTypeSelection
            // 
            this.grpStockTypeSelection.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpStockTypeSelection.Controls.Add(this.rdClosingStock);
            this.grpStockTypeSelection.Controls.Add(this.rdOpeningStock);
            this.grpStockTypeSelection.Location = new System.Drawing.Point(6, 302);
            this.grpStockTypeSelection.Name = "grpStockTypeSelection";
            this.grpStockTypeSelection.Size = new System.Drawing.Size(287, 58);
            this.grpStockTypeSelection.TabIndex = 108;
            this.grpStockTypeSelection.TabStop = false;
            this.grpStockTypeSelection.Text = "Stock Type Selection";
            this.grpStockTypeSelection.Visible = false;
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(226, 13);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 109;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(3, 226);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(292, 55);
            this.groupBox3.TabIndex = 110;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project";
            // 
            // cboProjectName
            // 
            this.cboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboProjectName.FocusLostColor = System.Drawing.Color.White;
            this.cboProjectName.FormattingEnabled = true;
            this.cboProjectName.Location = new System.Drawing.Point(118, 21);
            this.cboProjectName.Name = "cboProjectName";
            this.cboProjectName.Size = new System.Drawing.Size(148, 21);
            this.cboProjectName.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(66, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Project: ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnsavestate);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(297, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 342);
            this.panel1.TabIndex = 111;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnsavestate
            // 
            this.btnsavestate.Location = new System.Drawing.Point(9, 79);
            this.btnsavestate.Name = "btnsavestate";
            this.btnsavestate.Size = new System.Drawing.Size(80, 26);
            this.btnsavestate.TabIndex = 148;
            this.btnsavestate.Text = "Save State";
            this.btnsavestate.UseVisualStyleBackColor = true;
            this.btnsavestate.Click += new System.EventHandler(this.btnsavestate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnSelectAccClass);
            this.groupBox1.Location = new System.Drawing.Point(6, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 53);
            this.groupBox1.TabIndex = 112;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounting Class";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.cboMonths);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkDepot);
            this.groupBox2.Controls.Add(this.cmboDepot);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.DateTextBox);
            this.groupBox2.Controls.Add(this.btnDate);
            this.groupBox2.Location = new System.Drawing.Point(6, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 78);
            this.groupBox2.TabIndex = 113;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(107, 42);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(145, 21);
            this.cboMonths.TabIndex = 111;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "End of Month";
            // 
            // cmboDepot
            // 
            this.cmboDepot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboDepot.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboDepot.FocusLostColor = System.Drawing.Color.White;
            this.cmboDepot.FormattingEnabled = true;
            this.cmboDepot.Location = new System.Drawing.Point(107, 52);
            this.cmboDepot.Name = "cmboDepot";
            this.cmboDepot.Size = new System.Drawing.Size(156, 21);
            this.cmboDepot.TabIndex = 104;
            this.cmboDepot.Visible = false;
            // 
            // frmStockSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 339);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grpStockTypeSelection);
            this.Controls.Add(this.chkShowZeroQty);
            this.Controls.Add(this.grpProduct);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStockSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Stock Status";
            this.Load += new System.EventHandler(this.frmOpeningStockSettings_Load);
            this.grpProduct.ResumeLayout(false);
            this.grpProduct.PerformLayout();
            this.grpStockTypeSelection.ResumeLayout(false);
            this.grpStockTypeSelection.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SComboBox cboProductGroup;
        private System.Windows.Forms.Button btnSelectAccClass;
        private SComboBox cboProductSingle;
        private System.Windows.Forms.GroupBox grpProduct;
        private System.Windows.Forms.RadioButton rdProductGroup;
        private System.Windows.Forms.RadioButton rdProductAll;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnShow;
        private SComboBox cmboDepot;
        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.MaskedTextBox DateTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkShowZeroQty;
        private System.Windows.Forms.RadioButton rdClosingStock;
        private System.Windows.Forms.RadioButton rdOpeningStock;
        private System.Windows.Forms.GroupBox grpStockTypeSelection;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnsavestate;
        private System.Windows.Forms.RadioButton rdProductSingle;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
    }
}