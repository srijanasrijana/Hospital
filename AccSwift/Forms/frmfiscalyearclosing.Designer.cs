using SComponents;
namespace AccSwift.Forms
{
    partial class frmfiscalyearclosing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmfiscalyearclosing));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grdfiscalyear = new SourceGrid.Grid();
            this.btnAdjustDepretiation = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDate = new System.Windows.Forms.Button();
            this.btnEndDate = new System.Windows.Forms.Button();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblProfit = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgLedger = new SourceGrid.DataGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgInventory = new SourceGrid.DataGrid();
            this.grdinventoryfiscalyear = new SourceGrid.Grid();
            this.btnFiscalYearClose = new System.Windows.Forms.Button();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboAccountClass = new SComponents.SComboBox();
            this.txtDate = new SComponents.SMaskedTextBox();
            this.txtEndDate = new SComponents.SMaskedTextBox();
            this.txtStartDate = new SComponents.SMaskedTextBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Date :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "New FY Start Date :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(267, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Books Begining From :";
            // 
            // grdfiscalyear
            // 
            this.grdfiscalyear.EnableSort = true;
            this.grdfiscalyear.Location = new System.Drawing.Point(504, 284);
            this.grdfiscalyear.Name = "grdfiscalyear";
            this.grdfiscalyear.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdfiscalyear.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdfiscalyear.Size = new System.Drawing.Size(32, 27);
            this.grdfiscalyear.TabIndex = 153;
            this.grdfiscalyear.TabStop = true;
            this.grdfiscalyear.ToolTipText = "";
            // 
            // btnAdjustDepretiation
            // 
            this.btnAdjustDepretiation.Location = new System.Drawing.Point(233, 443);
            this.btnAdjustDepretiation.Name = "btnAdjustDepretiation";
            this.btnAdjustDepretiation.Size = new System.Drawing.Size(136, 23);
            this.btnAdjustDepretiation.TabIndex = 155;
            this.btnAdjustDepretiation.Text = "Adjust Depretiation";
            this.btnAdjustDepretiation.UseVisualStyleBackColor = true;
            this.btnAdjustDepretiation.Click += new System.EventHandler(this.btnadjustdepretiation_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 156;
            this.label4.Text = "Account Class :";
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(220, 14);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 152;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnEndDate
            // 
            this.btnEndDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnEndDate.Location = new System.Drawing.Point(496, 46);
            this.btnEndDate.Name = "btnEndDate";
            this.btnEndDate.Size = new System.Drawing.Size(26, 23);
            this.btnEndDate.TabIndex = 151;
            this.btnEndDate.UseVisualStyleBackColor = true;
            this.btnEndDate.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStartDate
            // 
            this.btnStartDate.Image = global::Inventory.Properties.Resources.dateIcon;
            this.btnStartDate.Location = new System.Drawing.Point(220, 46);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(26, 23);
            this.btnStartDate.TabIndex = 150;
            this.btnStartDate.UseVisualStyleBackColor = true;
            this.btnStartDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 71);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(543, 337);
            this.tabControl1.TabIndex = 158;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblProfit);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.dgLedger);
            this.tabPage1.Controls.Add(this.grdfiscalyear);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(535, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Account";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblProfit
            // 
            this.lblProfit.AutoSize = true;
            this.lblProfit.Location = new System.Drawing.Point(176, 288);
            this.lblProfit.Name = "lblProfit";
            this.lblProfit.Size = new System.Drawing.Size(13, 13);
            this.lblProfit.TabIndex = 154;
            this.lblProfit.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(87, 288);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 154;
            this.label6.Text = "Profit/ Loss :";
            // 
            // dgLedger
            // 
            this.dgLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgLedger.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgLedger.EnableSort = false;
            this.dgLedger.FixedRows = 1;
            this.dgLedger.Location = new System.Drawing.Point(6, 6);
            this.dgLedger.Name = "dgLedger";
            this.dgLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgLedger.Size = new System.Drawing.Size(523, 272);
            this.dgLedger.TabIndex = 4;
            this.dgLedger.TabStop = true;
            this.dgLedger.ToolTipText = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgInventory);
            this.tabPage2.Controls.Add(this.grdinventoryfiscalyear);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(535, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Inventory";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgInventory
            // 
            this.dgInventory.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgInventory.EnableSort = false;
            this.dgInventory.FixedRows = 1;
            this.dgInventory.Location = new System.Drawing.Point(12, 6);
            this.dgInventory.Name = "dgInventory";
            this.dgInventory.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgInventory.Size = new System.Drawing.Size(511, 279);
            this.dgInventory.TabIndex = 155;
            this.dgInventory.TabStop = true;
            this.dgInventory.ToolTipText = "";
            // 
            // grdinventoryfiscalyear
            // 
            this.grdinventoryfiscalyear.EnableSort = true;
            this.grdinventoryfiscalyear.Location = new System.Drawing.Point(475, 291);
            this.grdinventoryfiscalyear.Name = "grdinventoryfiscalyear";
            this.grdinventoryfiscalyear.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdinventoryfiscalyear.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdinventoryfiscalyear.Size = new System.Drawing.Size(60, 16);
            this.grdinventoryfiscalyear.TabIndex = 154;
            this.grdinventoryfiscalyear.TabStop = true;
            this.grdinventoryfiscalyear.ToolTipText = "";
            // 
            // btnFiscalYearClose
            // 
            this.btnFiscalYearClose.Location = new System.Drawing.Point(380, 443);
            this.btnFiscalYearClose.Name = "btnFiscalYearClose";
            this.btnFiscalYearClose.Size = new System.Drawing.Size(136, 23);
            this.btnFiscalYearClose.TabIndex = 156;
            this.btnFiscalYearClose.Text = "Fiscal Year Close";
            this.btnFiscalYearClose.UseVisualStyleBackColor = true;
            this.btnFiscalYearClose.Click += new System.EventHandler(this.btnfiscalyearclose_Click);
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(495, 415);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(37, 20);
            this.txtDBName.TabIndex = 159;
            this.txtDBName.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label5.Location = new System.Drawing.Point(136, 416);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(283, 15);
            this.label5.TabIndex = 160;
            this.label5.Text = "*** Please Check Properly Before You Close Fiscal Year***";
            // 
            // cboAccountClass
            // 
            this.cboAccountClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAccountClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAccountClass.BackColor = System.Drawing.Color.White;
            this.cboAccountClass.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboAccountClass.FocusLostColor = System.Drawing.Color.White;
            this.cboAccountClass.FormattingEnabled = true;
            this.cboAccountClass.Location = new System.Drawing.Point(390, 14);
            this.cboAccountClass.Name = "cboAccountClass";
            this.cboAccountClass.Size = new System.Drawing.Size(132, 21);
            this.cboAccountClass.TabIndex = 157;
            this.cboAccountClass.SelectedIndexChanged += new System.EventHandler(this.cboAccountClass_SelectedIndexChanged);
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(114, 15);
            this.txtDate.Mask = "##/##/####";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(100, 20);
            this.txtDate.TabIndex = 154;
            // 
            // txtEndDate
            // 
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtEndDate.FocusLostColor = System.Drawing.Color.White;
            this.txtEndDate.Location = new System.Drawing.Point(390, 46);
            this.txtEndDate.Mask = "##/##/####";
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(100, 20);
            this.txtEndDate.TabIndex = 2;
            // 
            // txtStartDate
            // 
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtStartDate.FocusLostColor = System.Drawing.Color.White;
            this.txtStartDate.Location = new System.Drawing.Point(114, 47);
            this.txtStartDate.Mask = "##/##/####";
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(100, 20);
            this.txtStartDate.TabIndex = 1;
            this.txtStartDate.Leave += new System.EventHandler(this.txtstartdate_Leave);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(132, 443);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(82, 23);
            this.btnReload.TabIndex = 156;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmfiscalyearclosing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 472);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnFiscalYearClose);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cboAccountClass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdjustDepretiation);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.btnEndDate);
            this.Controls.Add(this.btnStartDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.txtStartDate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmfiscalyearclosing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fiscal Year Closing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmfiscalyearclosing_FormClosing);
            this.Load += new System.EventHandler(this.frmfiscalyearclosing_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SMaskedTextBox txtStartDate;
        private SMaskedTextBox txtEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEndDate;
        private System.Windows.Forms.Button btnStartDate;
        private System.Windows.Forms.Button btnDate;
        private SourceGrid.Grid grdfiscalyear;
        private SMaskedTextBox txtDate;
        private System.Windows.Forms.Button btnAdjustDepretiation;
        private System.Windows.Forms.Label label4;
        private SComboBox cboAccountClass;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnFiscalYearClose;
        private System.Windows.Forms.TextBox txtDBName;
        private SourceGrid.Grid grdinventoryfiscalyear;
        private System.Windows.Forms.Label label5;
        private SourceGrid.DataGrid dgLedger;
        private SourceGrid.DataGrid dgInventory;
        private System.Windows.Forms.Label lblProfit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnReload;
    }
}