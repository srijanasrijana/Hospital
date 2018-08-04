namespace Accounts
{
    partial class frmChequeReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChequeReport));
            this.lblContact = new System.Windows.Forms.Label();
            this.grdCheque = new SourceGrid.Grid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lblCompanyAddress = new System.Windows.Forms.Label();
            this.lblPanNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblContact
            // 
            this.lblContact.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.Location = new System.Drawing.Point(-3, 82);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(831, 18);
            this.lblContact.TabIndex = 33;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblContact.Visible = false;
            // 
            // grdCheque
            // 
            this.grdCheque.Location = new System.Drawing.Point(3, 67);
            this.grdCheque.Name = "grdCheque";
            this.grdCheque.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdCheque.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdCheque.Size = new System.Drawing.Size(822, 322);
            this.grdCheque.TabIndex = 36;
            this.grdCheque.TabStop = true;
            this.grdCheque.ToolTipText = "";
            this.grdCheque.DoubleClick += new System.EventHandler(this.grdCheque_DoubleClick_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnPrintPreview);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(825, 35);
            this.panel1.TabIndex = 34;
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
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
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
            // lblCompanyAddress
            // 
            this.lblCompanyAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyAddress.Location = new System.Drawing.Point(-5, 64);
            this.lblCompanyAddress.Name = "lblCompanyAddress";
            this.lblCompanyAddress.Size = new System.Drawing.Size(833, 18);
            this.lblCompanyAddress.TabIndex = 32;
            this.lblCompanyAddress.Text = "Address";
            this.lblCompanyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCompanyAddress.Visible = false;
            // 
            // lblPanNo
            // 
            this.lblPanNo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPanNo.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Bold);
            this.lblPanNo.Location = new System.Drawing.Point(562, 16);
            this.lblPanNo.Name = "lblPanNo";
            this.lblPanNo.Size = new System.Drawing.Size(68, 21);
            this.lblPanNo.TabIndex = 35;
            this.lblPanNo.Text = "PAN No.";
            this.lblPanNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPanNo.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(834, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "Cheque Report";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(4, 35);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(826, 29);
            this.lblCompanyName.TabIndex = 31;
            this.lblCompanyName.Text = "Company Name";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCompanyName.Visible = false;
            // 
            // frmChequeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 391);
            this.Controls.Add(this.grdCheque);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblCompanyAddress);
            this.Controls.Add(this.lblPanNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.lblContact);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmChequeReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cheque Report";
            this.Load += new System.EventHandler(this.frmChequeReport_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblContact;
        private SourceGrid.Grid grdCheque;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lblCompanyAddress;
        private System.Windows.Forms.Label lblPanNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCompanyName;
    }
}