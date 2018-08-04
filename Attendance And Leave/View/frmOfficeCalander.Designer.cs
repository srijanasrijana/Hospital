namespace Attendance_And_Leave
{
    partial class frmOfficeCalander
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOfficeCalander));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnclose = new System.Windows.Forms.RadioButton();
            this.rbtnopen = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnexcel = new System.Windows.Forms.Button();
            this.btngenerate = new System.Windows.Forms.Button();
            this.txtyear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdofficecalender = new SourceGrid.Grid();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.btncancel);
            this.panel1.Controls.Add(this.btnsave);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(579, 35);
            this.panel1.TabIndex = 0;
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(492, 3);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 27);
            this.btncancel.TabIndex = 1;
            this.btncancel.Text = "&Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnsave
            // 
            this.btnsave.Enabled = false;
            this.btnsave.Location = new System.Drawing.Point(402, 3);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 27);
            this.btnsave.TabIndex = 0;
            this.btnsave.Text = "&Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnclose);
            this.groupBox1.Controls.Add(this.rbtnopen);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 41);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sunday";
            // 
            // rbtnclose
            // 
            this.rbtnclose.AutoSize = true;
            this.rbtnclose.Checked = true;
            this.rbtnclose.Location = new System.Drawing.Point(116, 14);
            this.rbtnclose.Name = "rbtnclose";
            this.rbtnclose.Size = new System.Drawing.Size(51, 17);
            this.rbtnclose.TabIndex = 1;
            this.rbtnclose.TabStop = true;
            this.rbtnclose.Text = "Close";
            this.rbtnclose.UseVisualStyleBackColor = true;
            // 
            // rbtnopen
            // 
            this.rbtnopen.AutoSize = true;
            this.rbtnopen.Location = new System.Drawing.Point(59, 14);
            this.rbtnopen.Name = "rbtnopen";
            this.rbtnopen.Size = new System.Drawing.Size(51, 17);
            this.rbtnopen.TabIndex = 0;
            this.rbtnopen.Text = "Open";
            this.rbtnopen.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnexcel);
            this.panel2.Controls.Add(this.btngenerate);
            this.panel2.Controls.Add(this.txtyear);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(2, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(579, 50);
            this.panel2.TabIndex = 2;
            // 
            // btnexcel
            // 
            this.btnexcel.Enabled = false;
            this.btnexcel.Location = new System.Drawing.Point(481, 13);
            this.btnexcel.Name = "btnexcel";
            this.btnexcel.Size = new System.Drawing.Size(89, 27);
            this.btnexcel.TabIndex = 4;
            this.btnexcel.Text = "Sent To Excel";
            this.btnexcel.UseVisualStyleBackColor = true;
            this.btnexcel.Click += new System.EventHandler(this.btnexcel_Click);
            // 
            // btngenerate
            // 
            this.btngenerate.Location = new System.Drawing.Point(392, 13);
            this.btngenerate.Name = "btngenerate";
            this.btngenerate.Size = new System.Drawing.Size(75, 27);
            this.btngenerate.TabIndex = 2;
            this.btngenerate.Text = "&Generate";
            this.btngenerate.UseVisualStyleBackColor = true;
            this.btngenerate.Click += new System.EventHandler(this.btngenerate_Click);
            // 
            // txtyear
            // 
            this.txtyear.BackColor = System.Drawing.Color.CornflowerBlue;
            this.txtyear.Location = new System.Drawing.Point(339, 17);
            this.txtyear.MaxLength = 4;
            this.txtyear.Name = "txtyear";
            this.txtyear.Size = new System.Drawing.Size(41, 20);
            this.txtyear.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(228, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Calender of the Year";
            // 
            // grdofficecalender
            // 
            this.grdofficecalender.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdofficecalender.Location = new System.Drawing.Point(0, 92);
            this.grdofficecalender.Name = "grdofficecalender";
            this.grdofficecalender.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdofficecalender.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdofficecalender.Size = new System.Drawing.Size(580, 239);
            this.grdofficecalender.TabIndex = 6;
            this.grdofficecalender.TabStop = true;
            this.grdofficecalender.ToolTipText = "";
            // 
            // frmOfficeCalander
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 332);
            this.Controls.Add(this.grdofficecalender);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOfficeCalander";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OfficeCalender";
            this.Load += new System.EventHandler(this.frmOfficeCalander_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnclose;
        private System.Windows.Forms.RadioButton rbtnopen;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btngenerate;
        private System.Windows.Forms.TextBox txtyear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnexcel;
        private SourceGrid.Grid grdofficecalender;
    }
}