using SComponents;
namespace HRM
{
    partial class frmAddition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddition));
            this.radbtnBothInGrid = new System.Windows.Forms.RadioButton();
            this.radbtnAdditionInGrid = new System.Windows.Forms.RadioButton();
            this.radbtnDeductionInGrid = new System.Windows.Forms.RadioButton();
            this.txtadditionSearch = new SComponents.STextBox();
            this.grdAddition = new SourceGrid.Grid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.radbtnDeduction = new System.Windows.Forms.RadioButton();
            this.radbtnAddition = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtname = new SComponents.STextBox();
            this.txtCode = new SComponents.STextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtid = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radbtnBothInGrid
            // 
            this.radbtnBothInGrid.AutoSize = true;
            this.radbtnBothInGrid.Location = new System.Drawing.Point(11, 9);
            this.radbtnBothInGrid.Name = "radbtnBothInGrid";
            this.radbtnBothInGrid.Size = new System.Drawing.Size(47, 17);
            this.radbtnBothInGrid.TabIndex = 2;
            this.radbtnBothInGrid.TabStop = true;
            this.radbtnBothInGrid.Text = "Both";
            this.radbtnBothInGrid.UseVisualStyleBackColor = true;
            this.radbtnBothInGrid.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radbtnAdditionInGrid
            // 
            this.radbtnAdditionInGrid.AutoSize = true;
            this.radbtnAdditionInGrid.Location = new System.Drawing.Point(76, 9);
            this.radbtnAdditionInGrid.Name = "radbtnAdditionInGrid";
            this.radbtnAdditionInGrid.Size = new System.Drawing.Size(63, 17);
            this.radbtnAdditionInGrid.TabIndex = 3;
            this.radbtnAdditionInGrid.TabStop = true;
            this.radbtnAdditionInGrid.Text = "Addition";
            this.radbtnAdditionInGrid.UseVisualStyleBackColor = true;
            this.radbtnAdditionInGrid.CheckedChanged += new System.EventHandler(this.radbtnAdditionInGrid_CheckedChanged);
            // 
            // radbtnDeductionInGrid
            // 
            this.radbtnDeductionInGrid.AutoSize = true;
            this.radbtnDeductionInGrid.Location = new System.Drawing.Point(160, 9);
            this.radbtnDeductionInGrid.Name = "radbtnDeductionInGrid";
            this.radbtnDeductionInGrid.Size = new System.Drawing.Size(74, 17);
            this.radbtnDeductionInGrid.TabIndex = 3;
            this.radbtnDeductionInGrid.TabStop = true;
            this.radbtnDeductionInGrid.Text = "Deduction";
            this.radbtnDeductionInGrid.UseVisualStyleBackColor = true;
            this.radbtnDeductionInGrid.CheckedChanged += new System.EventHandler(this.radbtnDeductionInGrid_CheckedChanged);
            // 
            // txtadditionSearch
            // 
            this.txtadditionSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtadditionSearch.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtadditionSearch.FocusLostColor = System.Drawing.Color.White;
            this.txtadditionSearch.Location = new System.Drawing.Point(95, 49);
            this.txtadditionSearch.Name = "txtadditionSearch";
            this.txtadditionSearch.Size = new System.Drawing.Size(151, 20);
            this.txtadditionSearch.TabIndex = 0;
            this.txtadditionSearch.TextChanged += new System.EventHandler(this.sTextBox1_TextChanged);
            // 
            // grdAddition
            // 
            this.grdAddition.EnableSort = true;
            this.grdAddition.Location = new System.Drawing.Point(5, 75);
            this.grdAddition.Name = "grdAddition";
            this.grdAddition.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdAddition.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdAddition.Size = new System.Drawing.Size(241, 236);
            this.grdAddition.TabIndex = 1;
            this.grdAddition.TabStop = true;
            this.grdAddition.ToolTipText = "";
            this.grdAddition.Click += new System.EventHandler(this.grdAddition_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(333, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(171, 258);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 24);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(6, 258);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 24);
            this.btnNew.TabIndex = 14;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // radbtnDeduction
            // 
            this.radbtnDeduction.AutoSize = true;
            this.radbtnDeduction.Location = new System.Drawing.Point(6, 56);
            this.radbtnDeduction.Name = "radbtnDeduction";
            this.radbtnDeduction.Size = new System.Drawing.Size(74, 17);
            this.radbtnDeduction.TabIndex = 17;
            this.radbtnDeduction.TabStop = true;
            this.radbtnDeduction.Text = "Deduction";
            this.radbtnDeduction.UseVisualStyleBackColor = true;
            this.radbtnDeduction.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radbtnAddition
            // 
            this.radbtnAddition.AutoSize = true;
            this.radbtnAddition.Location = new System.Drawing.Point(6, 19);
            this.radbtnAddition.Name = "radbtnAddition";
            this.radbtnAddition.Size = new System.Drawing.Size(63, 17);
            this.radbtnAddition.TabIndex = 16;
            this.radbtnAddition.TabStop = true;
            this.radbtnAddition.Text = "Addition";
            this.radbtnAddition.UseVisualStyleBackColor = true;
            this.radbtnAddition.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radbtnAddition);
            this.groupBox1.Controls.Add(this.radbtnDeduction);
            this.groupBox1.Location = new System.Drawing.Point(252, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 89);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Code";
            // 
            // txtname
            // 
            this.txtname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtname.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtname.FocusLostColor = System.Drawing.Color.White;
            this.txtname.Location = new System.Drawing.Point(72, 26);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(151, 20);
            this.txtname.TabIndex = 2;
            this.txtname.TextChanged += new System.EventHandler(this.sTextBox1_TextChanged_1);
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCode.FocusLostColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(72, 77);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(151, 20);
            this.txtCode.TabIndex = 21;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtid);
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.txtCode);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.txtname);
            this.groupBox2.Controls.Add(this.btnNew);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Location = new System.Drawing.Point(271, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 322);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // txtid
            // 
            this.txtid.Location = new System.Drawing.Point(356, 134);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(39, 20);
            this.txtid.TabIndex = 23;
            this.txtid.Visible = false;
            // 
            // btnEdit
            // 
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(87, 259);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 22;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(252, 257);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.grdAddition);
            this.groupBox3.Controls.Add(this.txtadditionSearch);
            this.groupBox3.Location = new System.Drawing.Point(12, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 322);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.radbtnAdditionInGrid);
            this.panel1.Controls.Add(this.radbtnDeductionInGrid);
            this.panel1.Controls.Add(this.radbtnBothInGrid);
            this.panel1.Location = new System.Drawing.Point(6, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 36);
            this.panel1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search :";
            // 
            // frmAddition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 329);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Addition/Deduction";
            this.Load += new System.EventHandler(this.frmAddition_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radbtnBothInGrid;
        private System.Windows.Forms.RadioButton radbtnAdditionInGrid;
        private System.Windows.Forms.RadioButton radbtnDeductionInGrid;
        private STextBox txtadditionSearch;
        private SourceGrid.Grid grdAddition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radbtnDeduction;
        private System.Windows.Forms.RadioButton radbtnAddition;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private STextBox txtname;
        private STextBox txtCode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
    }
}