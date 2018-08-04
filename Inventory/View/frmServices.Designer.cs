using SComponents;
namespace Inventory
{
    partial class frmServices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServices));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSrchParam = new SComponents.STextBox();
            this.cboSrchOP = new SComponents.SComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmboSearchin = new SComponents.SComboBox();
            this.btnServicesSave = new System.Windows.Forms.Button();
            this.btnServicesEdit = new System.Windows.Forms.Button();
            this.btnServicesDelete = new System.Windows.Forms.Button();
            this.btnServicesCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnServicesNew = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPurchaseRate = new SComponents.STextBox();
            this.txtServicesID = new SComponents.STextBox();
            this.cmboParentServiceName = new SComponents.SComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescription = new SComponents.STextBox();
            this.txtServiceName = new SComponents.STextBox();
            this.txtServiceCode = new SComponents.STextBox();
            this.txtSalesRate = new SComponents.STextBox();
            this.tabDisplay = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tvServices = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grid1 = new SourceGrid.Grid();
            this.grdListViesServices = new SourceGrid.Grid();
            this.tbTree = new System.Windows.Forms.TabPage();
            this.tvAccount = new System.Windows.Forms.TreeView();
            this.tbList = new System.Windows.Forms.TabPage();
            this.grdListView = new SourceGrid.Grid();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grid1.SuspendLayout();
            this.tbTree.SuspendLayout();
            this.tbList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search in";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "*Service Code:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "*Under:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Sales Rate:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "*Description:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSrchParam);
            this.groupBox1.Controls.Add(this.cboSrchOP);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmboSearchin);
            this.groupBox1.Location = new System.Drawing.Point(6, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 60);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Box";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(349, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Parameter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Operator";
            // 
            // txtSrchParam
            // 
            this.txtSrchParam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSrchParam.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSrchParam.FocusLostColor = System.Drawing.Color.White;
            this.txtSrchParam.Location = new System.Drawing.Point(329, 32);
            this.txtSrchParam.Name = "txtSrchParam";
            this.txtSrchParam.Size = new System.Drawing.Size(100, 20);
            this.txtSrchParam.TabIndex = 2;
            // 
            // cboSrchOP
            // 
            this.cboSrchOP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSrchOP.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboSrchOP.FocusLostColor = System.Drawing.Color.White;
            this.cboSrchOP.FormattingEnabled = true;
            this.cboSrchOP.Items.AddRange(new object[] {
            "Begins With",
            "Contains",
            "Greater or Equals",
            "Smaller or Equals"});
            this.cboSrchOP.Location = new System.Drawing.Point(221, 31);
            this.cboSrchOP.Name = "cboSrchOP";
            this.cboSrchOP.Size = new System.Drawing.Size(84, 21);
            this.cboSrchOP.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(455, 31);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmboSearchin
            // 
            this.cmboSearchin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboSearchin.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboSearchin.FocusLostColor = System.Drawing.Color.White;
            this.cmboSearchin.FormattingEnabled = true;
            this.cmboSearchin.Location = new System.Drawing.Point(66, 32);
            this.cmboSearchin.Name = "cmboSearchin";
            this.cmboSearchin.Size = new System.Drawing.Size(128, 21);
            this.cmboSearchin.TabIndex = 0;
            // 
            // btnServicesSave
            // 
            this.btnServicesSave.Image = global::Inventory.Properties.Resources.save;
            this.btnServicesSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServicesSave.Location = new System.Drawing.Point(248, 19);
            this.btnServicesSave.Name = "btnServicesSave";
            this.btnServicesSave.Size = new System.Drawing.Size(75, 23);
            this.btnServicesSave.TabIndex = 1;
            this.btnServicesSave.Text = "&Save";
            this.btnServicesSave.UseVisualStyleBackColor = true;
            this.btnServicesSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnServicesEdit
            // 
            this.btnServicesEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnServicesEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServicesEdit.Location = new System.Drawing.Point(167, 19);
            this.btnServicesEdit.Name = "btnServicesEdit";
            this.btnServicesEdit.Size = new System.Drawing.Size(75, 23);
            this.btnServicesEdit.TabIndex = 2;
            this.btnServicesEdit.Text = "&Edit";
            this.btnServicesEdit.UseVisualStyleBackColor = true;
            this.btnServicesEdit.Click += new System.EventHandler(this.btnServicesEdit_Click_1);
            // 
            // btnServicesDelete
            // 
            this.btnServicesDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnServicesDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServicesDelete.Location = new System.Drawing.Point(329, 19);
            this.btnServicesDelete.Name = "btnServicesDelete";
            this.btnServicesDelete.Size = new System.Drawing.Size(75, 23);
            this.btnServicesDelete.TabIndex = 3;
            this.btnServicesDelete.Text = "&Delete";
            this.btnServicesDelete.UseVisualStyleBackColor = true;
            this.btnServicesDelete.Click += new System.EventHandler(this.btnServicesDelete_Click);
            // 
            // btnServicesCancel
            // 
            this.btnServicesCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnServicesCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServicesCancel.Location = new System.Drawing.Point(410, 19);
            this.btnServicesCancel.Name = "btnServicesCancel";
            this.btnServicesCancel.Size = new System.Drawing.Size(77, 23);
            this.btnServicesCancel.TabIndex = 4;
            this.btnServicesCancel.Text = "&Cancel";
            this.btnServicesCancel.UseVisualStyleBackColor = true;
            this.btnServicesCancel.Click += new System.EventHandler(this.btnServicesCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnServicesNew);
            this.groupBox2.Controls.Add(this.btnServicesCancel);
            this.groupBox2.Controls.Add(this.btnServicesSave);
            this.groupBox2.Controls.Add(this.btnServicesDelete);
            this.groupBox2.Controls.Add(this.btnServicesEdit);
            this.groupBox2.Location = new System.Drawing.Point(6, 258);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(589, 62);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // btnServicesNew
            // 
            this.btnServicesNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnServicesNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServicesNew.Location = new System.Drawing.Point(86, 19);
            this.btnServicesNew.Name = "btnServicesNew";
            this.btnServicesNew.Size = new System.Drawing.Size(75, 23);
            this.btnServicesNew.TabIndex = 0;
            this.btnServicesNew.Text = "&New";
            this.btnServicesNew.UseVisualStyleBackColor = true;
            this.btnServicesNew.Click += new System.EventHandler(this.btnGrpNew_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtPurchaseRate);
            this.groupBox3.Controls.Add(this.txtServicesID);
            this.groupBox3.Controls.Add(this.cmboParentServiceName);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtDescription);
            this.groupBox3.Controls.Add(this.txtServiceName);
            this.groupBox3.Controls.Add(this.txtServiceCode);
            this.groupBox3.Controls.Add(this.txtSalesRate);
            this.groupBox3.Location = new System.Drawing.Point(6, 71);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(272, 181);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Details";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Purchase Rate";
            // 
            // txtPurchaseRate
            // 
            this.txtPurchaseRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseRate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPurchaseRate.FocusLostColor = System.Drawing.Color.White;
            this.txtPurchaseRate.Location = new System.Drawing.Point(112, 125);
            this.txtPurchaseRate.Name = "txtPurchaseRate";
            this.txtPurchaseRate.Size = new System.Drawing.Size(121, 20);
            this.txtPurchaseRate.TabIndex = 4;
            // 
            // txtServicesID
            // 
            this.txtServicesID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServicesID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtServicesID.FocusLostColor = System.Drawing.Color.White;
            this.txtServicesID.Location = new System.Drawing.Point(240, 16);
            this.txtServicesID.Name = "txtServicesID";
            this.txtServicesID.Size = new System.Drawing.Size(26, 20);
            this.txtServicesID.TabIndex = 8;
            // 
            // cmboParentServiceName
            // 
            this.cmboParentServiceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboParentServiceName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cmboParentServiceName.FocusLostColor = System.Drawing.Color.White;
            this.cmboParentServiceName.FormattingEnabled = true;
            this.cmboParentServiceName.Location = new System.Drawing.Point(112, 71);
            this.cmboParentServiceName.Name = "cmboParentServiceName";
            this.cmboParentServiceName.Size = new System.Drawing.Size(121, 21);
            this.cmboParentServiceName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "*Service Name:";
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDescription.FocusLostColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(112, 151);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(121, 20);
            this.txtDescription.TabIndex = 5;
            // 
            // txtServiceName
            // 
            this.txtServiceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtServiceName.FocusLostColor = System.Drawing.Color.White;
            this.txtServiceName.Location = new System.Drawing.Point(112, 44);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(121, 20);
            this.txtServiceName.TabIndex = 1;
            // 
            // txtServiceCode
            // 
            this.txtServiceCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtServiceCode.FocusLostColor = System.Drawing.Color.White;
            this.txtServiceCode.Location = new System.Drawing.Point(112, 16);
            this.txtServiceCode.Name = "txtServiceCode";
            this.txtServiceCode.Size = new System.Drawing.Size(121, 20);
            this.txtServiceCode.TabIndex = 0;
            // 
            // txtSalesRate
            // 
            this.txtSalesRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSalesRate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSalesRate.FocusLostColor = System.Drawing.Color.White;
            this.txtSalesRate.Location = new System.Drawing.Point(112, 99);
            this.txtSalesRate.Name = "txtSalesRate";
            this.txtSalesRate.Size = new System.Drawing.Size(121, 20);
            this.txtSalesRate.TabIndex = 3;
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.tabPage1);
            this.tabDisplay.Controls.Add(this.tabPage2);
            this.tabDisplay.Location = new System.Drawing.Point(284, 71);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.SelectedIndex = 0;
            this.tabDisplay.Size = new System.Drawing.Size(315, 181);
            this.tabDisplay.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tvServices);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(307, 155);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TreeView";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tvServices
            // 
            this.tvServices.Location = new System.Drawing.Point(6, 0);
            this.tvServices.Name = "tvServices";
            this.tvServices.Size = new System.Drawing.Size(295, 132);
            this.tvServices.TabIndex = 0;
            this.tvServices.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPage2.Size = new System.Drawing.Size(307, 155);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ListView";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grid1
            // 
            this.grid1.Controls.Add(this.grdListViesServices);
            this.grid1.EnableSort = true;
            this.grid1.Location = new System.Drawing.Point(-4, 3);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(305, 129);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // grdListViesServices
            // 
            this.grdListViesServices.EnableSort = true;
            this.grdListViesServices.Location = new System.Drawing.Point(3, 3);
            this.grdListViesServices.Name = "grdListViesServices";
            this.grdListViesServices.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListViesServices.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListViesServices.Size = new System.Drawing.Size(299, 116);
            this.grdListViesServices.TabIndex = 4;
            this.grdListViesServices.TabStop = true;
            this.grdListViesServices.ToolTipText = "";
            // 
            // tbTree
            // 
            this.tbTree.Controls.Add(this.tvAccount);
            this.tbTree.ImageIndex = 0;
            this.tbTree.Location = new System.Drawing.Point(4, 23);
            this.tbTree.Name = "tbTree";
            this.tbTree.Padding = new System.Windows.Forms.Padding(3);
            this.tbTree.Size = new System.Drawing.Size(192, 306);
            this.tbTree.TabIndex = 0;
            this.tbTree.Text = "TreeView";
            this.tbTree.UseVisualStyleBackColor = true;
            // 
            // tvAccount
            // 
            this.tvAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAccount.LineColor = System.Drawing.Color.Empty;
            this.tvAccount.Location = new System.Drawing.Point(3, 3);
            this.tvAccount.Name = "tvAccount";
            this.tvAccount.Size = new System.Drawing.Size(186, 300);
            this.tvAccount.TabIndex = 3;
            // 
            // tbList
            // 
            this.tbList.Controls.Add(this.grdListView);
            this.tbList.ImageIndex = 1;
            this.tbList.Location = new System.Drawing.Point(4, 23);
            this.tbList.Name = "tbList";
            this.tbList.Padding = new System.Windows.Forms.Padding(3);
            this.tbList.Size = new System.Drawing.Size(192, 306);
            this.tbList.TabIndex = 1;
            this.tbList.Text = "ListView";
            this.tbList.UseVisualStyleBackColor = true;
            // 
            // grdListView
            // 
            this.grdListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdListView.EnableSort = true;
            this.grdListView.Location = new System.Drawing.Point(3, 3);
            this.grdListView.Name = "grdListView";
            this.grdListView.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListView.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListView.Size = new System.Drawing.Size(186, 300);
            this.grdListView.TabIndex = 0;
            this.grdListView.TabStop = true;
            this.grdListView.ToolTipText = "";
            // 
            // frmServices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 335);
            this.Controls.Add(this.tabDisplay);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmServices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Services";
            this.Load += new System.EventHandler(this.frmServices_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmServices_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabDisplay.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.grid1.ResumeLayout(false);
            this.tbTree.ResumeLayout(false);
            this.tbList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private SComboBox cmboSearchin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private STextBox txtServiceCode;
        private STextBox txtSalesRate;
        private STextBox txtDescription;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnServicesSave;
        private System.Windows.Forms.Button btnServicesEdit;
        private System.Windows.Forms.Button btnServicesDelete;
        private System.Windows.Forms.Button btnServicesCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tabDisplay;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tbTree;
        private System.Windows.Forms.TreeView tvAccount;
        private System.Windows.Forms.TabPage tbList;
        private SourceGrid.Grid grdListView;
        private System.Windows.Forms.TreeView tvServices;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnServicesNew;
        private System.Windows.Forms.Label label3;
        private STextBox txtServiceName;
        private SComboBox cmboParentServiceName;
        private STextBox txtServicesID;
        private SourceGrid.Grid grid1;
        private SComboBox cboSrchOP;
        private STextBox txtSrchParam;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private SourceGrid.Grid grdListViesServices;
        private System.Windows.Forms.Label label9;
        private STextBox txtPurchaseRate;
    }
}