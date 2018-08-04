using SComponents;
namespace Inventory.Forms.Accounting
{
    partial class frmGroupVoucherList
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPagelist = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.grdbulkvoucher = new SourceGrid.Grid();
            this.btnNew = new System.Windows.Forms.Button();
            this.tabPagenew = new System.Windows.Forms.TabPage();
            this.txtid = new STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtcategory = new STextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPagelist.SuspendLayout();
            this.tabPagenew.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPagelist);
            this.tabControl1.Controls.Add(this.tabPagenew);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(293, 251);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            this.tabControl1.TabStopChanged += new System.EventHandler(this.tabControl1_TabStopChanged);
            // 
            // tabPagelist
            // 
            this.tabPagelist.Controls.Add(this.button1);
            this.tabPagelist.Controls.Add(this.grdbulkvoucher);
            this.tabPagelist.Controls.Add(this.btnNew);
            this.tabPagelist.Location = new System.Drawing.Point(4, 22);
            this.tabPagelist.Name = "tabPagelist";
            this.tabPagelist.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagelist.Size = new System.Drawing.Size(285, 225);
            this.tabPagelist.TabIndex = 0;
            this.tabPagelist.Text = "List";
            this.tabPagelist.UseVisualStyleBackColor = true;
            this.tabPagelist.Click += new System.EventHandler(this.tabPagelist_Click);
            this.tabPagelist.Enter += new System.EventHandler(this.tabPagelist_Enter);
            this.tabPagelist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabPagelist_MouseClick);
            // 
            // button1
            // 
            this.button1.Image = global::Inventory.Properties.Resources.save;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(115, 177);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 29);
            this.button1.TabIndex = 15;
            this.button1.Text = "&SaveToBulkPosting";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grdbulkvoucher
            // 
            this.grdbulkvoucher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdbulkvoucher.Location = new System.Drawing.Point(6, 6);
            this.grdbulkvoucher.Name = "grdbulkvoucher";
            this.grdbulkvoucher.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdbulkvoucher.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdbulkvoucher.Size = new System.Drawing.Size(273, 165);
            this.grdbulkvoucher.TabIndex = 14;
            this.grdbulkvoucher.TabStop = true;
            this.grdbulkvoucher.ToolTipText = "";
            this.grdbulkvoucher.DoubleClick += new System.EventHandler(this.grdbulkvoucher_DoubleClick);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(6, 177);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(103, 29);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "&Create New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // tabPagenew
            // 
            this.tabPagenew.Controls.Add(this.txtid);
            this.tabPagenew.Controls.Add(this.label1);
            this.tabPagenew.Controls.Add(this.txtcategory);
            this.tabPagenew.Controls.Add(this.btnSave);
            this.tabPagenew.Location = new System.Drawing.Point(4, 22);
            this.tabPagenew.Name = "tabPagenew";
            this.tabPagenew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagenew.Size = new System.Drawing.Size(285, 225);
            this.tabPagenew.TabIndex = 1;
            this.tabPagenew.Text = "New";
            this.tabPagenew.UseVisualStyleBackColor = true;
            this.tabPagenew.Enter += new System.EventHandler(this.tabPagenew_Enter);
            // 
            // txtid
            // 
            this.txtid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtid.Enabled = false;
            this.txtid.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtid.FocusLostColor = System.Drawing.Color.White;
            this.txtid.Location = new System.Drawing.Point(24, 202);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(100, 20);
            this.txtid.TabIndex = 10;
            this.txtid.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Category:";
            // 
            // txtcategory
            // 
            this.txtcategory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtcategory.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtcategory.FocusLostColor = System.Drawing.Color.White;
            this.txtcategory.Location = new System.Drawing.Point(125, 55);
            this.txtcategory.Name = "txtcategory";
            this.txtcategory.Size = new System.Drawing.Size(100, 20);
            this.txtcategory.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(39, 145);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmGroupVoucherList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 268);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmGroupVoucherList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Group Voucher List";
            this.Load += new System.EventHandler(this.frmGroupVoucherList_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPagelist.ResumeLayout(false);
            this.tabPagenew.ResumeLayout(false);
            this.tabPagenew.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPagelist;
        private System.Windows.Forms.TabPage tabPagenew;
        private System.Windows.Forms.Button btnNew;
        private SourceGrid.Grid grdbulkvoucher;
        private System.Windows.Forms.Button btnSave;
        private STextBox txtcategory;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private STextBox txtid;
    }
}