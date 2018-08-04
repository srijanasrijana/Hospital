namespace Accounts
{
    partial class frmGroupBalance
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblCompanyAddress = new System.Windows.Forms.Label();
            this.lblPanNo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.lblClosingBalance = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grdGroupBalance = new SourceGrid.Grid();
            this.lblwebsite = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(796, 35);
            this.panel1.TabIndex = 24;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(72, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 29);
            this.btnExport.TabIndex = 25;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(135, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 29);
            this.btnPrintPreview.TabIndex = 24;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::Accounts.Properties.Resources.refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(68, 29);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Image = global::Accounts.Properties.Resources.ExitButton;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(285, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 29);
            this.button4.TabIndex = 22;
            this.button4.Text = "E&xit";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Image = global::Accounts.Properties.Resources.print;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(230, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(54, 29);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(718, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 21);
            this.label6.TabIndex = 37;
            this.label6.Text = "As On Date:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblContact
            // 
            this.lblContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContact.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.Location = new System.Drawing.Point(8, 94);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(774, 18);
            this.lblContact.TabIndex = 33;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompanyAddress
            // 
            this.lblCompanyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyAddress.Location = new System.Drawing.Point(6, 76);
            this.lblCompanyAddress.Name = "lblCompanyAddress";
            this.lblCompanyAddress.Size = new System.Drawing.Size(774, 18);
            this.lblCompanyAddress.TabIndex = 32;
            this.lblCompanyAddress.Text = "Address";
            this.lblCompanyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPanNo
            // 
            this.lblPanNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPanNo.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Bold);
            this.lblPanNo.Location = new System.Drawing.Point(735, 55);
            this.lblPanNo.Name = "lblPanNo";
            this.lblPanNo.Size = new System.Drawing.Size(0, 21);
            this.lblPanNo.TabIndex = 34;
            this.lblPanNo.Text = "PAN No.";
            this.lblPanNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPanNo.Visible = false;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(774, 28);
            this.label7.TabIndex = 30;
            this.label7.Text = "Group Balance";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompanyName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(8, 47);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(774, 29);
            this.lblCompanyName.TabIndex = 31;
            this.lblCompanyName.Text = "Company Name";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(12, 159);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(250, 21);
            this.lblProjectName.TabIndex = 38;
            this.lblProjectName.Text = "Project Name:";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(12, 145);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(7, 21);
            this.lblAllSettings.TabIndex = 39;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClosingBalance
            // 
            this.lblClosingBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClosingBalance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClosingBalance.Location = new System.Drawing.Point(508, 555);
            this.lblClosingBalance.Name = "lblClosingBalance";
            this.lblClosingBalance.Size = new System.Drawing.Size(259, 20);
            this.lblClosingBalance.TabIndex = 75;
            this.lblClosingBalance.Text = "Closing Balance: 0";
            this.lblClosingBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(523, 568);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(259, 17);
            this.label3.TabIndex = 77;
            this.label3.Text = "__________________________________";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(523, 537);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(259, 17);
            this.label5.TabIndex = 76;
            this.label5.Text = "__________________________________";
            // 
            // grdGroupBalance
            // 
            this.grdGroupBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdGroupBalance.Location = new System.Drawing.Point(15, 186);
            this.grdGroupBalance.Name = "grdGroupBalance";
            this.grdGroupBalance.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdGroupBalance.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdGroupBalance.Size = new System.Drawing.Size(764, 351);
            this.grdGroupBalance.TabIndex = 74;
            this.grdGroupBalance.TabStop = true;
            this.grdGroupBalance.ToolTipText = "";
            // 
            // lblwebsite
            // 
            this.lblwebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblwebsite.AutoSize = true;
            this.lblwebsite.Location = new System.Drawing.Point(326, 114);
            this.lblwebsite.Name = "lblwebsite";
            this.lblwebsite.Size = new System.Drawing.Size(46, 13);
            this.lblwebsite.TabIndex = 78;
            this.lblwebsite.Text = "Website";
            // 
            // frmGroupBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 592);
            this.Controls.Add(this.lblwebsite);
            this.Controls.Add(this.lblClosingBalance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grdGroupBalance);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblAllSettings);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.lblCompanyAddress);
            this.Controls.Add(this.lblPanNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmGroupBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Group Balance";
            this.Load += new System.EventHandler(this.frmGroupBalance_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGroupBalance_KeyDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblCompanyAddress;
        private System.Windows.Forms.Label lblPanNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAllSettings;
        private System.Windows.Forms.Label lblClosingBalance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private SourceGrid.Grid grdGroupBalance;
        private System.Windows.Forms.Label lblwebsite;
    }
}