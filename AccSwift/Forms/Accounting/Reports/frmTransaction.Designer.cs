namespace Inventory
{
    partial class frmTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransaction));
            this.grdTransaction = new SourceGrid.Grid();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTotalCreditAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalDebitAmount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.Print = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdTransaction
            // 
            this.grdTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTransaction.DefaultHeight = 23;
            this.grdTransaction.Location = new System.Drawing.Point(3, 57);
            this.grdTransaction.Name = "grdTransaction";
            this.grdTransaction.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdTransaction.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdTransaction.Size = new System.Drawing.Size(985, 501);
            this.grdTransaction.TabIndex = 0;
            this.grdTransaction.TabStop = true;
            this.grdTransaction.ToolTipText = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 580);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(991, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblProjectName);
            this.panel2.Controls.Add(this.lblAllSettings);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.grdTransaction);
            this.panel2.Location = new System.Drawing.Point(0, 41);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(991, 602);
            this.panel2.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(650, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 15);
            this.label1.TabIndex = 37;
            this.label1.Text = "As On Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjectName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(300, 35);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(396, 15);
            this.lblProjectName.TabIndex = 35;
            this.lblProjectName.Text = "Project Name:";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(8, 35);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(315, 15);
            this.lblAllSettings.TabIndex = 36;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(946, 22);
            this.label6.TabIndex = 30;
            this.label6.Text = "Transaction";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.lblTotalCreditAmount);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.lblTotalDebitAmount);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(3, 559);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(988, 29);
            this.panel3.TabIndex = 7;
            // 
            // lblTotalCreditAmount
            // 
            this.lblTotalCreditAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCreditAmount.Location = new System.Drawing.Point(855, 2);
            this.lblTotalCreditAmount.Name = "lblTotalCreditAmount";
            this.lblTotalCreditAmount.Size = new System.Drawing.Size(130, 13);
            this.lblTotalCreditAmount.TabIndex = 11;
            this.lblTotalCreditAmount.Text = "0";
            this.lblTotalCreditAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(466, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Total Debit Amount:";
            // 
            // lblTotalDebitAmount
            // 
            this.lblTotalDebitAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDebitAmount.Location = new System.Drawing.Point(590, 2);
            this.lblTotalDebitAmount.Name = "lblTotalDebitAmount";
            this.lblTotalDebitAmount.Size = new System.Drawing.Size(130, 13);
            this.lblTotalDebitAmount.TabIndex = 9;
            this.lblTotalDebitAmount.Text = "0";
            this.lblTotalDebitAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(726, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Total Credit Amount:";
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
            this.panel1.Controls.Add(this.Print);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(991, 35);
            this.panel1.TabIndex = 24;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Inventory.Properties.Resources.export1;
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
            this.btnPrintPreview.Image = global::Inventory.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(135, 3);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(94, 29);
            this.btnPrintPreview.TabIndex = 24;
            this.btnPrintPreview.Text = "Pr&int Preview";
            this.btnPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click_1);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::Inventory.Properties.Resources.refresh;
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
            this.button4.Image = global::Inventory.Properties.Resources.ExitButton;
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
            // Print
            // 
            this.Print.Image = global::Inventory.Properties.Resources.print;
            this.Print.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Print.Location = new System.Drawing.Point(230, 3);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(54, 29);
            this.Print.TabIndex = 2;
            this.Print.Text = "&Print";
            this.Print.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Print.UseVisualStyleBackColor = true;
            this.Print.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 643);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction";
            this.Load += new System.EventHandler(this.frmTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTransaction_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdTransaction;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button Print;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAllSettings;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTotalCreditAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalDebitAmount;
        private System.Windows.Forms.Label label4;


    }
}