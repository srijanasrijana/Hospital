namespace POS.POSInventory
{
    partial class frmprintingtemplate
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbprintstyle = new System.Windows.Forms.ComboBox();
            this.btnprint = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose Print Style:";
            // 
            // cmbprintstyle
            // 
            this.cmbprintstyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbprintstyle.FormattingEnabled = true;
            this.cmbprintstyle.Items.AddRange(new object[] {
            "POS Style",
            "Dot Matrix"});
            this.cmbprintstyle.Location = new System.Drawing.Point(144, 45);
            this.cmbprintstyle.Name = "cmbprintstyle";
            this.cmbprintstyle.Size = new System.Drawing.Size(145, 21);
            this.cmbprintstyle.TabIndex = 1;
            // 
            // btnprint
            // 
            this.btnprint.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnprint.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnprint.Location = new System.Drawing.Point(12, 94);
            this.btnprint.Name = "btnprint";
            this.btnprint.Size = new System.Drawing.Size(75, 57);
            this.btnprint.TabIndex = 2;
            this.btnprint.Text = "Print";
            this.btnprint.UseVisualStyleBackColor = true;
            this.btnprint.Click += new System.EventHandler(this.btnprint_Click);
            // 
            // btncancel
            // 
            this.btncancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btncancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncancel.Location = new System.Drawing.Point(108, 94);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 57);
            this.btncancel.TabIndex = 3;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            // 
            // frmprintingtemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 223);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnprint);
            this.Controls.Add(this.cmbprintstyle);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmprintingtemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Template";
            this.Load += new System.EventHandler(this.frmprintingtemplate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbprintstyle;
        private System.Windows.Forms.Button btnprint;
        private System.Windows.Forms.Button btncancel;
    }
}