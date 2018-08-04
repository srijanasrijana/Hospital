using SComponents;
namespace Inventory
{
    partial class frmStockAgingSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockAgingSetting));
            this.chkDepot = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectAccClass = new System.Windows.Forms.Button();
            this.cmboDepot = new SComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboProjectName = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.cboProductGroup = new SComboBox();
            this.cboProductSingle = new SComboBox();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.rdProductGroup = new System.Windows.Forms.RadioButton();
            this.rdProductSingle = new System.Windows.Forms.RadioButton();
            this.rdProductAll = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDate = new System.Windows.Forms.Button();
            this.cboMonths = new SComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpProduct.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDepot
            // 
            this.chkDepot.AutoSize = true;
            this.chkDepot.Location = new System.Drawing.Point(6, 22);
            this.chkDepot.Name = "chkDepot";
            this.chkDepot.Size = new System.Drawing.Size(91, 17);
            this.chkDepot.TabIndex = 103;
            this.chkDepot.Text = "Select Depot:";
            this.chkDepot.UseVisualStyleBackColor = true;
            this.chkDepot.CheckedChanged += new System.EventHandler(this.chkDepot_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(299, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 323);
            this.panel1.TabIndex = 118;
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
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnSelectAccClass);
            this.groupBox1.Location = new System.Drawing.Point(7, 211);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 53);
            this.groupBox1.TabIndex = 119;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounting Class";
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
            // cmboDepot
            // 
            this.cmboDepot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboDepot.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboDepot.FocusLostColor = System.Drawing.Color.White;
            this.cmboDepot.FormattingEnabled = true;
            this.cmboDepot.Location = new System.Drawing.Point(107, 20);
            this.cmboDepot.Name = "cmboDepot";
            this.cmboDepot.Size = new System.Drawing.Size(156, 21);
            this.cmboDepot.TabIndex = 104;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.cboProjectName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(4, 266);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(292, 55);
            this.groupBox3.TabIndex = 117;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 105;
            this.label2.Text = "At the End of:";
            // 
            // DateTextBox
            // 
            this.DateTextBox.Location = new System.Drawing.Point(107, 56);
            this.DateTextBox.Mask = "##/##/####";
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(113, 20);
            this.DateTextBox.TabIndex = 106;
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
            // grpProduct
            // 
            this.grpProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpProduct.Controls.Add(this.cboProductGroup);
            this.grpProduct.Controls.Add(this.cboProductSingle);
            this.grpProduct.Controls.Add(this.rdProductGroup);
            this.grpProduct.Controls.Add(this.rdProductSingle);
            this.grpProduct.Controls.Add(this.rdProductAll);
            this.grpProduct.Location = new System.Drawing.Point(5, 5);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(292, 78);
            this.grpProduct.TabIndex = 114;
            this.grpProduct.TabStop = false;
            // 
            // rdProductGroup
            // 
            this.rdProductGroup.AutoSize = true;
            this.rdProductGroup.Location = new System.Drawing.Point(6, 51);
            this.rdProductGroup.Name = "rdProductGroup";
            this.rdProductGroup.Size = new System.Drawing.Size(94, 17);
            this.rdProductGroup.TabIndex = 0;
            this.rdProductGroup.TabStop = true;
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
            this.rdProductSingle.TabStop = true;
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
            this.rdProductAll.TabStop = true;
            this.rdProductAll.Text = "All Products";
            this.rdProductAll.UseVisualStyleBackColor = true;
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
            this.groupBox2.Location = new System.Drawing.Point(8, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 120);
            this.groupBox2.TabIndex = 120;
            this.groupBox2.TabStop = false;
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(226, 53);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 25);
            this.btnDate.TabIndex = 109;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // cboMonths
            // 
            this.cboMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonths.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboMonths.FocusLostColor = System.Drawing.Color.White;
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Location = new System.Drawing.Point(107, 84);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(145, 21);
            this.cboMonths.TabIndex = 111;
            this.cboMonths.SelectedIndexChanged += new System.EventHandler(this.cboMonths_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "End of Month";
            // 
            // frmStockAgingSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 324);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grpProduct);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStockAgingSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StockAgingSetting";
            this.Load += new System.EventHandler(this.frmStockAgingSetting_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpProduct.ResumeLayout(false);
            this.grpProduct.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDepot;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectAccClass;
        private SComboBox cmboDepot;
        private System.Windows.Forms.GroupBox groupBox3;
        private SComboBox cboProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox DateTextBox;
        private SComboBox cboProductGroup;
        private SComboBox cboProductSingle;
        private System.Windows.Forms.GroupBox grpProduct;
        private System.Windows.Forms.RadioButton rdProductGroup;
        private System.Windows.Forms.RadioButton rdProductSingle;
        private System.Windows.Forms.RadioButton rdProductAll;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private SComboBox cboMonths;
        private System.Windows.Forms.Label label1;
    }
}