namespace Inventory
{
    partial class frmAccountLedger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountLedger));
            this.grdAccountLedger = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // grdAccountLedger
            // 
            this.grdAccountLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAccountLedger.Location = new System.Drawing.Point(6, 51);
            this.grdAccountLedger.Name = "grdAccountLedger";
            this.grdAccountLedger.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdAccountLedger.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdAccountLedger.Size = new System.Drawing.Size(818, 310);
            this.grdAccountLedger.TabIndex = 3;
            this.grdAccountLedger.TabStop = true;
            this.grdAccountLedger.ToolTipText = "";
            this.grdAccountLedger.DoubleClick += new System.EventHandler(this.grdAccountLedger_DoubleClick);
            // 
            // frmAccountLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 412);
            this.Controls.Add(this.grdAccountLedger);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmAccountLedger";
            this.Text = "Account Ledger";
            this.Load += new System.EventHandler(this.frmAccountLedger_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccountLedger_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdAccountLedger;
    }
}