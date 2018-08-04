using SComponents;
namespace Inventory
{
    partial class frmPreviousYearBalance
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
            this.txtValidateOpeningBalance = new STextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grdPreYearBalnce = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // txtValidateOpeningBalance
            // 
            this.txtValidateOpeningBalance.BackColor = System.Drawing.Color.White;
            this.txtValidateOpeningBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValidateOpeningBalance.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtValidateOpeningBalance.FocusLostColor = System.Drawing.Color.White;
            this.txtValidateOpeningBalance.Location = new System.Drawing.Point(30, 333);
            this.txtValidateOpeningBalance.Name = "txtValidateOpeningBalance";
            this.txtValidateOpeningBalance.Size = new System.Drawing.Size(88, 20);
            this.txtValidateOpeningBalance.TabIndex = 29;
            this.txtValidateOpeningBalance.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(448, 327);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(367, 327);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnGrpSave_Click);
            // 
            // grdPreYearBalnce
            // 
            this.grdPreYearBalnce.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPreYearBalnce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdPreYearBalnce.Location = new System.Drawing.Point(20, 51);
            this.grdPreYearBalnce.Name = "grdPreYearBalnce";
            this.grdPreYearBalnce.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdPreYearBalnce.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdPreYearBalnce.Size = new System.Drawing.Size(501, 270);
            this.grdPreYearBalnce.TabIndex = 26;
            this.grdPreYearBalnce.TabStop = true;
            this.grdPreYearBalnce.ToolTipText = "";
            // 
            // frmPreviousYearBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 404);
            this.Controls.Add(this.txtValidateOpeningBalance);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdPreYearBalnce);
            this.Name = "frmPreviousYearBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Previous Year Balance";
            this.Load += new System.EventHandler(this.frmPreviousYearBalance_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPreYearBalance_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private STextBox txtValidateOpeningBalance;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private SourceGrid.Grid grdPreYearBalnce;
    }
}