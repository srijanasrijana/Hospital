namespace HRM
{
    partial class frmhrmmastersetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmhrmmastersetup));
            this.btnDepartment = new System.Windows.Forms.Button();
            this.btndesignation = new System.Windows.Forms.Button();
            this.btncountry = new System.Windows.Forms.Button();
            this.btnLevel = new System.Windows.Forms.Button();
            this.btnLoan = new System.Windows.Forms.Button();
            this.btnEmpFaculty = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDepartment
            // 
            this.btnDepartment.Location = new System.Drawing.Point(12, 12);
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Size = new System.Drawing.Size(250, 23);
            this.btnDepartment.TabIndex = 0;
            this.btnDepartment.Text = "Department";
            this.btnDepartment.UseVisualStyleBackColor = true;
            this.btnDepartment.Click += new System.EventHandler(this.btnDepartment_Click);
            // 
            // btndesignation
            // 
            this.btndesignation.Location = new System.Drawing.Point(12, 48);
            this.btndesignation.Name = "btndesignation";
            this.btndesignation.Size = new System.Drawing.Size(250, 23);
            this.btndesignation.TabIndex = 1;
            this.btndesignation.Text = "Designation";
            this.btndesignation.UseVisualStyleBackColor = true;
            this.btndesignation.Click += new System.EventHandler(this.btndesignation_Click);
            // 
            // btncountry
            // 
            this.btncountry.Location = new System.Drawing.Point(12, 196);
            this.btncountry.Name = "btncountry";
            this.btncountry.Size = new System.Drawing.Size(250, 23);
            this.btncountry.TabIndex = 5;
            this.btncountry.Text = "Nationality";
            this.btncountry.UseVisualStyleBackColor = true;
            this.btncountry.Click += new System.EventHandler(this.btncountry_Click);
            // 
            // btnLevel
            // 
            this.btnLevel.Location = new System.Drawing.Point(12, 122);
            this.btnLevel.Name = "btnLevel";
            this.btnLevel.Size = new System.Drawing.Size(250, 23);
            this.btnLevel.TabIndex = 3;
            this.btnLevel.Text = "Level";
            this.btnLevel.UseVisualStyleBackColor = true;
            this.btnLevel.Click += new System.EventHandler(this.btnLevel_Click);
            // 
            // btnLoan
            // 
            this.btnLoan.Location = new System.Drawing.Point(12, 160);
            this.btnLoan.Name = "btnLoan";
            this.btnLoan.Size = new System.Drawing.Size(250, 23);
            this.btnLoan.TabIndex = 4;
            this.btnLoan.Text = "Loan";
            this.btnLoan.UseVisualStyleBackColor = true;
            this.btnLoan.Click += new System.EventHandler(this.btnLoan_Click);
            // 
            // btnEmpFaculty
            // 
            this.btnEmpFaculty.Location = new System.Drawing.Point(12, 86);
            this.btnEmpFaculty.Name = "btnEmpFaculty";
            this.btnEmpFaculty.Size = new System.Drawing.Size(250, 23);
            this.btnEmpFaculty.TabIndex = 2;
            this.btnEmpFaculty.Text = "Faculty";
            this.btnEmpFaculty.UseVisualStyleBackColor = true;
            this.btnEmpFaculty.Click += new System.EventHandler(this.btnEmpFaculty_Click);
            // 
            // frmhrmmastersetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 236);
            this.Controls.Add(this.btnEmpFaculty);
            this.Controls.Add(this.btnLoan);
            this.Controls.Add(this.btnLevel);
            this.Controls.Add(this.btncountry);
            this.Controls.Add(this.btndesignation);
            this.Controls.Add(this.btnDepartment);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmhrmmastersetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Master SetUp";
            this.Load += new System.EventHandler(this.frmhrmmastersetup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDepartment;
        private System.Windows.Forms.Button btndesignation;
        private System.Windows.Forms.Button btncountry;
        private System.Windows.Forms.Button btnLevel;
        private System.Windows.Forms.Button btnLoan;
        private System.Windows.Forms.Button btnEmpFaculty;
    }
}