using SComponents;
namespace Inventory
{
    partial class frmSlabs
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGrpCancel = new System.Windows.Forms.Button();
            this.txtDescription = new STextBox();
            this.txtRate = new STextBox();
            this.txtName = new STextBox();
            this.cachedrptTrialBalance1 = new Inventory.CrystalReports.CachedrptTrialBalance();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rate:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description:";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(160, 243);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 35;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGrpCancel
            // 
            this.btnGrpCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnGrpCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGrpCancel.Location = new System.Drawing.Point(241, 243);
            this.btnGrpCancel.Name = "btnGrpCancel";
            this.btnGrpCancel.Size = new System.Drawing.Size(75, 23);
            this.btnGrpCancel.TabIndex = 36;
            this.btnGrpCancel.Text = "&Cancel";
            this.btnGrpCancel.UseVisualStyleBackColor = true;
            this.btnGrpCancel.Click += new System.EventHandler(this.btnGrpCancel_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.CausesValidation = false;
            this.txtDescription.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDescription.FocusLostColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(125, 115);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(191, 99);
            this.txtDescription.TabIndex = 5;
            // 
            // txtRate
            // 
            this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRate.FocusLostColor = System.Drawing.Color.White;
            this.txtRate.Location = new System.Drawing.Point(125, 66);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(191, 20);
            this.txtRate.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtName.FocusLostColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(125, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(191, 20);
            this.txtName.TabIndex = 2;
            // 
            // frmSlabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 341);
            this.Controls.Add(this.btnGrpCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRate);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSlabs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Slabs";
            this.Load += new System.EventHandler(this.frmSlabs_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSlabs_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private STextBox txtName;
        private STextBox txtRate;
        private System.Windows.Forms.Label label3;
        private STextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGrpCancel;
        private CrystalReports.CachedrptTrialBalance cachedrptTrialBalance1;
    }
}