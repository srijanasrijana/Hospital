namespace AccSwift.Forms
{
    partial class frmChequeCashReminder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChequeCashReminder));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btndeposit = new System.Windows.Forms.Button();
            this.grdchequecashreminder = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(679, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 72;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btndeposit
            // 
            this.btndeposit.Image = global::Inventory.Properties.Resources.save;
            this.btndeposit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndeposit.Location = new System.Drawing.Point(603, 322);
            this.btndeposit.Name = "btndeposit";
            this.btndeposit.Size = new System.Drawing.Size(70, 29);
            this.btndeposit.TabIndex = 71;
            this.btndeposit.Text = "&Deposit";
            this.btndeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btndeposit.UseVisualStyleBackColor = true;
            this.btndeposit.Click += new System.EventHandler(this.btndeposit_Click);
            // 
            // grdchequecashreminder
            // 
            this.grdchequecashreminder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdchequecashreminder.Location = new System.Drawing.Point(2, 2);
            this.grdchequecashreminder.Name = "grdchequecashreminder";
            this.grdchequecashreminder.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdchequecashreminder.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdchequecashreminder.Size = new System.Drawing.Size(784, 314);
            this.grdchequecashreminder.TabIndex = 73;
            this.grdchequecashreminder.TabStop = true;
            this.grdchequecashreminder.ToolTipText = "";
            // 
            // frmChequeCashReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 357);
            this.Controls.Add(this.grdchequecashreminder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btndeposit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChequeCashReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cheque-Cash Reminder";
            this.Load += new System.EventHandler(this.frmChequeCashReminder_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btndeposit;
        private SourceGrid.Grid grdchequecashreminder;
    }
}