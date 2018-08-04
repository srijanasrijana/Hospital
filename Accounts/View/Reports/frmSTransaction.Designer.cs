namespace Accounts
{
    partial class frmSTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSTransaction));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblAsOnDate = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.dgSTransaction = new SourceGrid.DataGrid();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalClosingBalance = new System.Windows.Forms.Label();
            this.lblTotalbalance = new System.Windows.Forms.Label();
            this.lblTotalCreditAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalDebitAmount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grdTransaction = new SourceGrid.Grid();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 35);
            this.panel1.TabIndex = 26;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Accounts.Properties.Resources.export1;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(71, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(62, 29);
            this.btnExport.TabIndex = 25;
            this.btnExport.Text = "&Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click_1);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Image = global::Accounts.Properties.Resources.print_preview;
            this.btnPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPreview.Location = new System.Drawing.Point(134, 4);
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
            this.btnRefresh.Location = new System.Drawing.Point(2, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(68, 29);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click_1);
            // 
            // button4
            // 
            this.button4.Image = global::Accounts.Properties.Resources.ExitButton;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(284, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 29);
            this.button4.TabIndex = 22;
            this.button4.Text = "E&xit";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button3
            // 
            this.button3.Image = global::Accounts.Properties.Resources.print;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(229, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(54, 29);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Print";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.lblAsOnDate);
            this.panel2.Controls.Add(this.lblProjectName);
            this.panel2.Controls.Add(this.dgSTransaction);
            this.panel2.Controls.Add(this.lblAllSettings);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.grdTransaction);
            this.panel2.Location = new System.Drawing.Point(2, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1012, 486);
            this.panel2.TabIndex = 25;
            // 
            // lblAsOnDate
            // 
            this.lblAsOnDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsOnDate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsOnDate.Location = new System.Drawing.Point(765, 36);
            this.lblAsOnDate.Name = "lblAsOnDate";
            this.lblAsOnDate.Size = new System.Drawing.Size(221, 15);
            this.lblAsOnDate.TabIndex = 37;
            this.lblAsOnDate.Text = "As On Date:";
            this.lblAsOnDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjectName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(279, 37);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(460, 15);
            this.lblProjectName.TabIndex = 35;
            this.lblProjectName.Text = "Project Name:";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgSTransaction
            // 
            this.dgSTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSTransaction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgSTransaction.DefaultHeight = 10;
            this.dgSTransaction.DefaultWidth = 100;
            this.dgSTransaction.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgSTransaction.DeleteRowsWithDeleteKey = false;
            this.dgSTransaction.EnableSort = false;
            this.dgSTransaction.EndEditingRowOnValidate = false;
            this.dgSTransaction.FixedRows = 1;
            this.dgSTransaction.Location = new System.Drawing.Point(3, 66);
            this.dgSTransaction.Name = "dgSTransaction";
            this.dgSTransaction.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgSTransaction.Size = new System.Drawing.Size(1004, 369);
            this.dgSTransaction.TabIndex = 5;
            this.dgSTransaction.TabStop = true;
            this.dgSTransaction.ToolTipText = "";
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(7, 36);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(274, 15);
            this.lblAllSettings.TabIndex = 36;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(29, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(967, 22);
            this.label6.TabIndex = 30;
            this.label6.Text = "Transaction";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 464);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1012, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lblTotalClosingBalance);
            this.panel3.Controls.Add(this.lblTotalbalance);
            this.panel3.Controls.Add(this.lblTotalCreditAmount);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.lblTotalDebitAmount);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(2, 436);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1010, 47);
            this.panel3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(527, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Total Credit Amount:";
            // 
            // lblTotalClosingBalance
            // 
            this.lblTotalClosingBalance.AutoSize = true;
            this.lblTotalClosingBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalClosingBalance.Location = new System.Drawing.Point(888, 9);
            this.lblTotalClosingBalance.Name = "lblTotalClosingBalance";
            this.lblTotalClosingBalance.Size = new System.Drawing.Size(14, 13);
            this.lblTotalClosingBalance.TabIndex = 15;
            this.lblTotalClosingBalance.Text = "0";
            // 
            // lblTotalbalance
            // 
            this.lblTotalbalance.AutoSize = true;
            this.lblTotalbalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalbalance.Location = new System.Drawing.Point(797, 8);
            this.lblTotalbalance.Name = "lblTotalbalance";
            this.lblTotalbalance.Size = new System.Drawing.Size(90, 13);
            this.lblTotalbalance.TabIndex = 14;
            this.lblTotalbalance.Text = "Total Balance:";
            // 
            // lblTotalCreditAmount
            // 
            this.lblTotalCreditAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCreditAmount.Location = new System.Drawing.Point(652, 3);
            this.lblTotalCreditAmount.Name = "lblTotalCreditAmount";
            this.lblTotalCreditAmount.Size = new System.Drawing.Size(130, 25);
            this.lblTotalCreditAmount.TabIndex = 11;
            this.lblTotalCreditAmount.Text = "0";
            this.lblTotalCreditAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(263, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Total Debit Amount:";
            // 
            // lblTotalDebitAmount
            // 
            this.lblTotalDebitAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDebitAmount.Location = new System.Drawing.Point(387, 3);
            this.lblTotalDebitAmount.Name = "lblTotalDebitAmount";
            this.lblTotalDebitAmount.Size = new System.Drawing.Size(130, 25);
            this.lblTotalDebitAmount.TabIndex = 9;
            this.lblTotalDebitAmount.Text = "0";
            this.lblTotalDebitAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(647, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 10;
            // 
            // grdTransaction
            // 
            this.grdTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTransaction.DefaultHeight = 23;
            this.grdTransaction.EnableSort = true;
            this.grdTransaction.Location = new System.Drawing.Point(2, 71);
            this.grdTransaction.Name = "grdTransaction";
            this.grdTransaction.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdTransaction.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdTransaction.Size = new System.Drawing.Size(1007, 367);
            this.grdTransaction.TabIndex = 0;
            this.grdTransaction.TabStop = true;
            this.grdTransaction.ToolTipText = "";
            // 
            // frmSTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 527);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STransaction";
            this.Load += new System.EventHandler(this.frmSTransaction_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblAsOnDate;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAllSettings;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblTotalCreditAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalDebitAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotalClosingBalance;
        private System.Windows.Forms.Label lblTotalbalance;
        private SourceGrid.Grid grdTransaction;
        private SourceGrid.DataGrid dgSTransaction;
        private System.Windows.Forms.Label label3;
    }
}