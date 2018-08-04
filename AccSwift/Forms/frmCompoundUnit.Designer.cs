namespace Common
{
    partial class frmCompoundUnit
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
            this.grdCompoundUnit = new SourceGrid.Grid();
            this.txtValueOfFirstUnit = new System.Windows.Forms.TextBox();
            this.txtValueOfSecondUnit = new System.Windows.Forms.TextBox();
            this.cmbFirstUnit = new System.Windows.Forms.ComboBox();
            this.cmbSecondUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAddNewUnit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdCompoundUnit
            // 
            this.grdCompoundUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCompoundUnit.EnableSort = true;
            this.grdCompoundUnit.Location = new System.Drawing.Point(3, 312);
            this.grdCompoundUnit.Name = "grdCompoundUnit";
            this.grdCompoundUnit.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdCompoundUnit.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdCompoundUnit.Size = new System.Drawing.Size(562, 199);
            this.grdCompoundUnit.TabIndex = 7;
            this.grdCompoundUnit.TabStop = true;
            this.grdCompoundUnit.ToolTipText = "";
            // 
            // txtValueOfFirstUnit
            // 
            this.txtValueOfFirstUnit.Location = new System.Drawing.Point(53, 87);
            this.txtValueOfFirstUnit.Name = "txtValueOfFirstUnit";
            this.txtValueOfFirstUnit.ReadOnly = true;
            this.txtValueOfFirstUnit.Size = new System.Drawing.Size(68, 20);
            this.txtValueOfFirstUnit.TabIndex = 0;
            this.txtValueOfFirstUnit.Text = "1";
            this.txtValueOfFirstUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtValueOfSecondUnit
            // 
            this.txtValueOfSecondUnit.Location = new System.Drawing.Point(310, 87);
            this.txtValueOfSecondUnit.Name = "txtValueOfSecondUnit";
            this.txtValueOfSecondUnit.Size = new System.Drawing.Size(68, 20);
            this.txtValueOfSecondUnit.TabIndex = 2;
            this.txtValueOfSecondUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbFirstUnit
            // 
            this.cmbFirstUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFirstUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFirstUnit.FormattingEnabled = true;
            this.cmbFirstUnit.Location = new System.Drawing.Point(135, 86);
            this.cmbFirstUnit.Name = "cmbFirstUnit";
            this.cmbFirstUnit.Size = new System.Drawing.Size(121, 21);
            this.cmbFirstUnit.TabIndex = 1;
            // 
            // cmbSecondUnit
            // 
            this.cmbSecondUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSecondUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSecondUnit.FormattingEnabled = true;
            this.cmbSecondUnit.Location = new System.Drawing.Point(392, 86);
            this.cmbSecondUnit.Name = "cmbSecondUnit";
            this.cmbSecondUnit.Size = new System.Drawing.Size(121, 21);
            this.cmbSecondUnit.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(271, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "=";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(45, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(354, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Example:         1    kilogram            =            1000   gram";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Value of first unit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Name of first unit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(293, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Value of second unit";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(406, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Name of second unit";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(435, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 29);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(354, 271);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(61, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 41);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(106, 146);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(405, 109);
            this.txtRemarks.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Remarks";
            // 
            // btnAddNewUnit
            // 
            this.btnAddNewUnit.Location = new System.Drawing.Point(107, 117);
            this.btnAddNewUnit.Name = "btnAddNewUnit";
            this.btnAddNewUnit.Size = new System.Drawing.Size(406, 23);
            this.btnAddNewUnit.TabIndex = 12;
            this.btnAddNewUnit.Text = "Add New Unit";
            this.btnAddNewUnit.UseVisualStyleBackColor = true;
            this.btnAddNewUnit.Click += new System.EventHandler(this.btnAddNewUnit_Click);
            // 
            // frmCompoundUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 512);
            this.Controls.Add(this.btnAddNewUnit);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSecondUnit);
            this.Controls.Add(this.cmbFirstUnit);
            this.Controls.Add(this.txtValueOfSecondUnit);
            this.Controls.Add(this.txtValueOfFirstUnit);
            this.Controls.Add(this.grdCompoundUnit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmCompoundUnit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compound Unit";
            this.Load += new System.EventHandler(this.frmCompoundUnit_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grdCompoundUnit;
        private System.Windows.Forms.TextBox txtValueOfFirstUnit;
        private System.Windows.Forms.TextBox txtValueOfSecondUnit;
        private System.Windows.Forms.ComboBox cmbFirstUnit;
        private System.Windows.Forms.ComboBox cmbSecondUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAddNewUnit;
    }
}