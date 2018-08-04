using SComponents;
namespace Accounts
{
    partial class frmbulkvoucherposting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmbulkvoucherposting));
            this.label3 = new System.Windows.Forms.Label();
            this.btnDate = new System.Windows.Forms.Button();
            this.grdpayroll = new SourceGrid.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnbulkposting = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboCategoryName = new SComboBox();
            this.txtDate = new SMaskedTextBox();
            this.txtVchNo = new STextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(400, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Date:";
            // 
            // btnDate
            // 
            this.btnDate.Image = global::Accounts.Properties.Resources.dateIcon;
            this.btnDate.Location = new System.Drawing.Point(520, 21);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(26, 23);
            this.btnDate.TabIndex = 9;
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // grdpayroll
            // 
            this.grdpayroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdpayroll.Location = new System.Drawing.Point(13, 63);
            this.grdpayroll.Name = "grdpayroll";
            this.grdpayroll.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdpayroll.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdpayroll.Size = new System.Drawing.Size(610, 187);
            this.grdpayroll.TabIndex = 13;
            this.grdpayroll.TabStop = true;
            this.grdpayroll.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Choose Category:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnbulkposting);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(13, 261);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(610, 32);
            this.panel1.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(507, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnbulkposting
            // 
            this.btnbulkposting.Image = global::Accounts.Properties.Resources.document_edit;
            this.btnbulkposting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnbulkposting.Location = new System.Drawing.Point(369, 4);
            this.btnbulkposting.Name = "btnbulkposting";
            this.btnbulkposting.Size = new System.Drawing.Size(132, 29);
            this.btnbulkposting.TabIndex = 8;
            this.btnbulkposting.Text = "&Make Bulk Posting";
            this.btnbulkposting.UseVisualStyleBackColor = true;
            this.btnbulkposting.Click += new System.EventHandler(this.btnbulkposting_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(288, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboCategoryName
            // 
            this.cboCategoryName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCategoryName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategoryName.BackColor = System.Drawing.Color.White;
            this.cboCategoryName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboCategoryName.FocusLostColor = System.Drawing.Color.White;
            this.cboCategoryName.FormattingEnabled = true;
            this.cboCategoryName.Location = new System.Drawing.Point(127, 21);
            this.cboCategoryName.Name = "cboCategoryName";
            this.cboCategoryName.Size = new System.Drawing.Size(159, 21);
            this.cboCategoryName.TabIndex = 14;
            this.cboCategoryName.SelectedIndexChanged += new System.EventHandler(this.cboCategoryName_SelectedIndexChanged);
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(439, 21);
            this.txtDate.Mask = "0000/00/00";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(75, 20);
            this.txtDate.TabIndex = 8;
            // 
            // txtVchNo
            // 
            this.txtVchNo.BackColor = System.Drawing.Color.White;
            this.txtVchNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVchNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtVchNo.FocusLostColor = System.Drawing.Color.White;
            this.txtVchNo.Location = new System.Drawing.Point(12, 39);
            this.txtVchNo.Name = "txtVchNo";
            this.txtVchNo.Size = new System.Drawing.Size(135, 20);
            this.txtVchNo.TabIndex = 17;
            this.txtVchNo.Visible = false;
            // 
            // frmbulkvoucherposting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 300);
            this.Controls.Add(this.txtVchNo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboCategoryName);
            this.Controls.Add(this.grdpayroll);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmbulkvoucherposting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bulk Voucher Posting";
            this.Load += new System.EventHandler(this.frmPayroll_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Label label3;
        private SMaskedTextBox txtDate;
        private SourceGrid.Grid grdpayroll;
        private SComboBox cboCategoryName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnbulkposting;
        private System.Windows.Forms.Button btnSave;
        private STextBox txtVchNo;
    }
}