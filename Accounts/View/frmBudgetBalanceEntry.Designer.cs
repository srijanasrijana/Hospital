namespace Accounts.View
{
    partial class frmBudgetBalanceEntry
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
            this.grdBudgetBalnce = new SourceGrid.Grid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtValidateAmount = new SComponents.STextBox();
            this.SuspendLayout();
            // 
            // grdBudgetBalnce
            // 
            this.grdBudgetBalnce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdBudgetBalnce.EnableSort = true;
            this.grdBudgetBalnce.Location = new System.Drawing.Point(2, 0);
            this.grdBudgetBalnce.Name = "grdBudgetBalnce";
            this.grdBudgetBalnce.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdBudgetBalnce.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdBudgetBalnce.Size = new System.Drawing.Size(386, 261);
            this.grdBudgetBalnce.TabIndex = 16;
            this.grdBudgetBalnce.TabStop = true;
            this.grdBudgetBalnce.ToolTipText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Accounts.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(313, 267);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Accounts.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(232, 267);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtValidateAmount
            // 
            this.txtValidateAmount.BackColor = System.Drawing.Color.White;
            this.txtValidateAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValidateAmount.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtValidateAmount.FocusLostColor = System.Drawing.Color.White;
            this.txtValidateAmount.Location = new System.Drawing.Point(12, 267);
            this.txtValidateAmount.Name = "txtValidateAmount";
            this.txtValidateAmount.Size = new System.Drawing.Size(88, 20);
            this.txtValidateAmount.TabIndex = 26;
            this.txtValidateAmount.Visible = false;
            // 
            // frmBudgetBalanceEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 296);
            this.Controls.Add(this.txtValidateAmount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdBudgetBalnce);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBudgetBalanceEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Budget Balance";
            this.Load += new System.EventHandler(this.frmBudgetBalanceEntry_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private SourceGrid.Grid grdBudgetBalnce;
        private SComponents.STextBox txtValidateAmount;
    }
}