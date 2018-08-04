namespace Attendance_And_Leave
{
    partial class frmleaveapproval
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmleaveapproval));
            this.grdleaveapproval = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // grdleaveapproval
            // 
            this.grdleaveapproval.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdleaveapproval.Location = new System.Drawing.Point(2, 5);
            this.grdleaveapproval.Name = "grdleaveapproval";
            this.grdleaveapproval.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdleaveapproval.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdleaveapproval.Size = new System.Drawing.Size(724, 302);
            this.grdleaveapproval.TabIndex = 8;
            this.grdleaveapproval.TabStop = true;
            this.grdleaveapproval.ToolTipText = "";
            // 
            // frmleaveapproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 309);
            this.Controls.Add(this.grdleaveapproval);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmleaveapproval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Leave Approval";
            this.Load += new System.EventHandler(this.frmleaveapproval_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdleaveapproval;
    }
}