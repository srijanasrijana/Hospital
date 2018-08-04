using SComponents;
namespace Inventory
{
    partial class frmStockAging
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockAging));
            this.chkProduct = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sTextBox3 = new STextBox();
            this.sTextBox4 = new STextBox();
            this.sTextBox2 = new STextBox();
            this.sTextBox1 = new STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnshow = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkProduct
            // 
            this.chkProduct.AutoSize = true;
            this.chkProduct.Location = new System.Drawing.Point(6, 33);
            this.chkProduct.Name = "chkProduct";
            this.chkProduct.Size = new System.Drawing.Size(107, 17);
            this.chkProduct.TabIndex = 5;
            this.chkProduct.Text = "Show All Product";
            this.chkProduct.UseVisualStyleBackColor = true;
            this.chkProduct.CheckedChanged += new System.EventHandler(this.chkshowalldebtors_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Product";
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(217, 31);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(166, 21);
            this.cmbProduct.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.sTextBox3);
            this.groupBox3.Controls.Add(this.sTextBox4);
            this.groupBox3.Controls.Add(this.sTextBox2);
            this.groupBox3.Controls.Add(this.sTextBox1);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(3, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 102);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Choose Ageing Period";
            // 
            // sTextBox3
            // 
            this.sTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sTextBox3.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.sTextBox3.FocusLostColor = System.Drawing.Color.White;
            this.sTextBox3.Location = new System.Drawing.Point(266, 64);
            this.sTextBox3.Name = "sTextBox3";
            this.sTextBox3.Size = new System.Drawing.Size(96, 20);
            this.sTextBox3.TabIndex = 9;
            this.sTextBox3.Text = "60";
            // 
            // sTextBox4
            // 
            this.sTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sTextBox4.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.sTextBox4.FocusLostColor = System.Drawing.Color.White;
            this.sTextBox4.Location = new System.Drawing.Point(73, 63);
            this.sTextBox4.Name = "sTextBox4";
            this.sTextBox4.Size = new System.Drawing.Size(96, 20);
            this.sTextBox4.TabIndex = 8;
            this.sTextBox4.Text = "45";
            // 
            // sTextBox2
            // 
            this.sTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sTextBox2.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.sTextBox2.FocusLostColor = System.Drawing.Color.White;
            this.sTextBox2.Location = new System.Drawing.Point(267, 20);
            this.sTextBox2.Name = "sTextBox2";
            this.sTextBox2.Size = new System.Drawing.Size(96, 20);
            this.sTextBox2.TabIndex = 7;
            this.sTextBox2.Text = "30";
            this.sTextBox2.TextChanged += new System.EventHandler(this.sTextBox2_TextChanged);
            // 
            // sTextBox1
            // 
            this.sTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sTextBox1.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.sTextBox1.FocusLostColor = System.Drawing.Color.White;
            this.sTextBox1.Location = new System.Drawing.Point(74, 19);
            this.sTextBox1.Name = "sTextBox1";
            this.sTextBox1.Size = new System.Drawing.Size(96, 20);
            this.sTextBox1.TabIndex = 6;
            this.sTextBox1.Text = "15";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Fourth Period:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Third Period:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(181, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Second Period:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "First Period:";
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(105, 164);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 28);
            this.btncancel.TabIndex = 8;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnshow
            // 
            this.btnshow.Location = new System.Drawing.Point(12, 164);
            this.btnshow.Name = "btnshow";
            this.btnshow.Size = new System.Drawing.Size(75, 28);
            this.btnshow.TabIndex = 7;
            this.btnshow.Text = "Show";
            this.btnshow.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Group";
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(217, 4);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(166, 21);
            this.cmbGroup.TabIndex = 10;
            // 
            // frmStockAging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 196);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnshow);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkProduct);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbProduct);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStockAging";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Aeging Report";
            this.Load += new System.EventHandler(this.frmStockAging_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.GroupBox groupBox3;
        private STextBox sTextBox3;
        private STextBox sTextBox4;
        private STextBox sTextBox2;
        private STextBox sTextBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnshow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbGroup;
    }
}