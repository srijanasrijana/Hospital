using SComponents;
namespace Inventory
{
    partial class frmOpeningBalance
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
            this.grdOpeningBalnce = new SourceGrid.Grid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtValidateOpeningBalance = new STextBox();
            this.SuspendLayout();
            // 
            // grdOpeningBalnce
            // 
            this.grdOpeningBalnce.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdOpeningBalnce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdOpeningBalnce.Location = new System.Drawing.Point(2, 2);
            this.grdOpeningBalnce.Name = "grdOpeningBalnce";
            this.grdOpeningBalnce.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdOpeningBalnce.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdOpeningBalnce.Size = new System.Drawing.Size(504, 270);
            this.grdOpeningBalnce.TabIndex = 3;
            this.grdOpeningBalnce.TabStop = true;
            this.grdOpeningBalnce.ToolTipText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(430, 278);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(349, 278);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnGrpSave_Click);
            // 
            // txtValidateOpeningBalance
            // 
            this.txtValidateOpeningBalance.BackColor = System.Drawing.Color.White;
            this.txtValidateOpeningBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValidateOpeningBalance.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtValidateOpeningBalance.FocusLostColor = System.Drawing.Color.White;
            this.txtValidateOpeningBalance.Location = new System.Drawing.Point(12, 284);
            this.txtValidateOpeningBalance.Name = "txtValidateOpeningBalance";
            this.txtValidateOpeningBalance.Size = new System.Drawing.Size(88, 20);
            this.txtValidateOpeningBalance.TabIndex = 25;
            this.txtValidateOpeningBalance.Visible = false;
            // 
            // frmOpeningBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 308);
            this.Controls.Add(this.txtValidateOpeningBalance);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdOpeningBalnce);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "frmOpeningBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Opening Balance";
            this.Load += new System.EventHandler(this.frmOpeningBalance_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmOpeningBalance_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grdOpeningBalnce;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private STextBox txtValidateOpeningBalance;
    }
}