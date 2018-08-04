namespace Accounts.Reports
{
    partial class frmTrial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrial));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.grdTrial = new SourceGrid.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblAllSettings = new System.Windows.Forms.Label();
            this.lblAsonDate = new System.Windows.Forms.Label();
            this.dgTrial = new SourceGrid.DataGrid();
            this.panel1.SuspendLayout();
            this.dgTrial.SuspendLayout();
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
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(994, 35);
            this.panel1.TabIndex = 20;
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
            // btnPrint
            // 
            this.btnPrint.Image = global::Accounts.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(230, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 29);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // grdTrial
            // 
            this.grdTrial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTrial.DefaultHeight = 23;
            this.grdTrial.EnableSort = true;
            this.grdTrial.Location = new System.Drawing.Point(52, 17);
            this.grdTrial.Name = "grdTrial";
            this.grdTrial.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdTrial.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdTrial.Size = new System.Drawing.Size(34, 541);
            this.grdTrial.TabIndex = 25;
            this.grdTrial.TabStop = true;
            this.grdTrial.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(995, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Trial Balance";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProjectName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(435, 70);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(275, 21);
            this.lblProjectName.TabIndex = 27;
            this.lblProjectName.Text = "Project Name:";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAllSettings
            // 
            this.lblAllSettings.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllSettings.Location = new System.Drawing.Point(7, 70);
            this.lblAllSettings.Name = "lblAllSettings";
            this.lblAllSettings.Size = new System.Drawing.Size(166, 21);
            this.lblAllSettings.TabIndex = 28;
            this.lblAllSettings.Text = "Settings";
            this.lblAllSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAsonDate
            // 
            this.lblAsonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsonDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsonDate.Location = new System.Drawing.Point(813, 70);
            this.lblAsonDate.Name = "lblAsonDate";
            this.lblAsonDate.Size = new System.Drawing.Size(167, 21);
            this.lblAsonDate.TabIndex = 29;
            this.lblAsonDate.Text = "As On Date:";
            this.lblAsonDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgTrial
            // 
            this.dgTrial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgTrial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgTrial.Controls.Add(this.grdTrial);
            this.dgTrial.DefaultHeight = 10;
            this.dgTrial.DefaultWidth = 100;
            this.dgTrial.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dgTrial.DeleteRowsWithDeleteKey = false;
            this.dgTrial.EnableSort = false;
            this.dgTrial.EndEditingRowOnValidate = false;
            this.dgTrial.FixedRows = 1;
            this.dgTrial.Location = new System.Drawing.Point(3, 99);
            this.dgTrial.Name = "dgTrial";
            this.dgTrial.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dgTrial.Size = new System.Drawing.Size(985, 541);
            this.dgTrial.TabIndex = 5;
            this.dgTrial.TabStop = true;
            this.dgTrial.ToolTipText = "";
            this.dgTrial.Visible = false;
            this.dgTrial.Paint += new System.Windows.Forms.PaintEventHandler(this.dgTrial_Paint);
            // 
            // frmTrial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 643);
            this.Controls.Add(this.lblAsonDate);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.dgTrial);
            this.Controls.Add(this.lblAllSettings);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmTrial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trial Balance";
            this.Load += new System.EventHandler(this.frmTrial_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTrial_KeyDown);
            this.panel1.ResumeLayout(false);
            this.dgTrial.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPrintPreview;
        private SourceGrid.Grid grdTrial;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAllSettings;
        private System.Windows.Forms.Label lblAsonDate;
        private SourceGrid.DataGrid dgTrial;
    }
}