namespace Hospital.View
{
    partial class frmHosMasterSetup
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
            this.btnEmpFaculty = new System.Windows.Forms.Button();
            this.btnLevel = new System.Windows.Forms.Button();
            this.btnspecialization = new System.Windows.Forms.Button();
            this.btnDepartment = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEmpFaculty
            // 
            this.btnEmpFaculty.Location = new System.Drawing.Point(12, 89);
            this.btnEmpFaculty.Name = "btnEmpFaculty";
            this.btnEmpFaculty.Size = new System.Drawing.Size(250, 23);
            this.btnEmpFaculty.TabIndex = 8;
            this.btnEmpFaculty.Text = "Faculty";
            this.btnEmpFaculty.UseVisualStyleBackColor = true;
            this.btnEmpFaculty.Click += new System.EventHandler(this.btnEmpFaculty_Click);
            // 
            // btnLevel
            // 
            this.btnLevel.Location = new System.Drawing.Point(12, 125);
            this.btnLevel.Name = "btnLevel";
            this.btnLevel.Size = new System.Drawing.Size(250, 23);
            this.btnLevel.TabIndex = 9;
            this.btnLevel.Text = "Level";
            this.btnLevel.UseVisualStyleBackColor = true;
            this.btnLevel.Click += new System.EventHandler(this.btnLevel_Click);
            // 
            // btnspecialization
            // 
            this.btnspecialization.Location = new System.Drawing.Point(13, 48);
            this.btnspecialization.Name = "btnspecialization";
            this.btnspecialization.Size = new System.Drawing.Size(250, 24);
            this.btnspecialization.TabIndex = 7;
            this.btnspecialization.Text = "Specialization";
            this.btnspecialization.UseVisualStyleBackColor = true;
            this.btnspecialization.Click += new System.EventHandler(this.btnspecialization_Click);
            // 
            // btnDepartment
            // 
            this.btnDepartment.Location = new System.Drawing.Point(12, 15);
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Size = new System.Drawing.Size(250, 23);
            this.btnDepartment.TabIndex = 6;
            this.btnDepartment.Text = "Department";
            this.btnDepartment.UseVisualStyleBackColor = true;
            this.btnDepartment.Click += new System.EventHandler(this.btnDepartment_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(250, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "List Of Doctor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmHosMasterSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 202);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEmpFaculty);
            this.Controls.Add(this.btnLevel);
            this.Controls.Add(this.btnspecialization);
            this.Controls.Add(this.btnDepartment);
            this.MaximizeBox = false;
            this.Name = "frmHosMasterSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMasterSetup";
            this.Load += new System.EventHandler(this.frmHosMasterSetup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEmpFaculty;
        private System.Windows.Forms.Button btnLevel;
        private System.Windows.Forms.Button btnspecialization;
        private System.Windows.Forms.Button btnDepartment;
        private System.Windows.Forms.Button button1;
    }
}