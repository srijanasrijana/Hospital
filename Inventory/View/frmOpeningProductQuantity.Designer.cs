using SComponents;
namespace Inventory
{
    partial class frmOpeningProductQuantity
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
            this.grdOpeningQuantity = new SourceGrid.Grid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtValidateOpeningBalance = new SComponents.STextBox();
            this.cmbDepotLocation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // grdOpeningQuantity
            // 
            this.grdOpeningQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdOpeningQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdOpeningQuantity.EnableSort = true;
            this.grdOpeningQuantity.Location = new System.Drawing.Point(2, 37);
            this.grdOpeningQuantity.Name = "grdOpeningQuantity";
            this.grdOpeningQuantity.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdOpeningQuantity.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdOpeningQuantity.Size = new System.Drawing.Size(484, 216);
            this.grdOpeningQuantity.TabIndex = 4;
            this.grdOpeningQuantity.TabStop = true;
            this.grdOpeningQuantity.ToolTipText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(411, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(330, 266);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtValidateOpeningBalance
            // 
            this.txtValidateOpeningBalance.BackColor = System.Drawing.Color.White;
            this.txtValidateOpeningBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValidateOpeningBalance.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtValidateOpeningBalance.FocusLostColor = System.Drawing.Color.White;
            this.txtValidateOpeningBalance.Location = new System.Drawing.Point(11, 271);
            this.txtValidateOpeningBalance.Name = "txtValidateOpeningBalance";
            this.txtValidateOpeningBalance.Size = new System.Drawing.Size(88, 20);
            this.txtValidateOpeningBalance.TabIndex = 26;
            this.txtValidateOpeningBalance.Visible = false;
            // 
            // cmbDepotLocation
            // 
            this.cmbDepotLocation.FormattingEnabled = true;
            this.cmbDepotLocation.Location = new System.Drawing.Point(91, 10);
            this.cmbDepotLocation.Name = "cmbDepotLocation";
            this.cmbDepotLocation.Size = new System.Drawing.Size(121, 21);
            this.cmbDepotLocation.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Depot Location";
            // 
            // frmOpeningProductQuantity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 297);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDepotLocation);
            this.Controls.Add(this.txtValidateOpeningBalance);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdOpeningQuantity);
            this.Name = "frmOpeningProductQuantity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opening Quantity";
            this.Load += new System.EventHandler(this.frmOpeningProductQuantity_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grdOpeningQuantity;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private STextBox txtValidateOpeningBalance;
        private System.Windows.Forms.ComboBox cmbDepotLocation;
        private System.Windows.Forms.Label label1;
    }
}