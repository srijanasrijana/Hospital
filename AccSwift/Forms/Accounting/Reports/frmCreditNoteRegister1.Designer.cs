namespace AccSwift
{
    partial class frmCreditNoteRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreditNoteRegister));
            this.grdCreditNoteRegister = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // grdCreditNoteRegister
            // 
            this.grdCreditNoteRegister.Location = new System.Drawing.Point(12, 30);
            this.grdCreditNoteRegister.Name = "grdCreditNoteRegister";
            this.grdCreditNoteRegister.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdCreditNoteRegister.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdCreditNoteRegister.Size = new System.Drawing.Size(981, 434);
            this.grdCreditNoteRegister.TabIndex = 12;
            this.grdCreditNoteRegister.TabStop = true;
            this.grdCreditNoteRegister.ToolTipText = "";
            // 
            // frmCreditNoteRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 488);
            this.Controls.Add(this.grdCreditNoteRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCreditNoteRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Credit Note Register";
            this.Load += new System.EventHandler(this.frmCreditNoteRegister_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCreditNoteRegister_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdCreditNoteRegister;
    }
}