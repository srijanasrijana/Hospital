﻿namespace Hospital.View
{
    partial class FrmListOfPatientInfo
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
            this.btnAddEmployee = new System.Windows.Forms.Button();
            this.taPatient = new System.Windows.Forms.TabPage();
            this.grdEmployee = new SourceGrid.Grid();
            this.txtLName = new SComponents.STextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMName = new SComponents.STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFName = new SComponents.STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCode = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabEmployees = new System.Windows.Forms.TabControl();
            this.txtDate = new SComponents.STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.taPatient.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabEmployees.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEmployee.Location = new System.Drawing.Point(832, 473);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new System.Drawing.Size(115, 23);
            this.btnAddEmployee.TabIndex = 20;
            this.btnAddEmployee.Text = "Add Patient";
            this.btnAddEmployee.UseVisualStyleBackColor = true;
            // 
            // taPatient
            // 
            this.taPatient.Controls.Add(this.grdEmployee);
            this.taPatient.Location = new System.Drawing.Point(4, 22);
            this.taPatient.Name = "taPatient";
            this.taPatient.Padding = new System.Windows.Forms.Padding(3);
            this.taPatient.Size = new System.Drawing.Size(932, 354);
            this.taPatient.TabIndex = 0;
            this.taPatient.Text = "Patient";
            this.taPatient.UseVisualStyleBackColor = true;
            // 
            // grdEmployee
            // 
            this.grdEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdEmployee.EnableSort = true;
            this.grdEmployee.Location = new System.Drawing.Point(4, 7);
            this.grdEmployee.Name = "grdEmployee";
            this.grdEmployee.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdEmployee.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdEmployee.Size = new System.Drawing.Size(922, 343);
            this.grdEmployee.TabIndex = 7;
            this.grdEmployee.TabStop = true;
            this.grdEmployee.ToolTipText = "";
            // 
            // txtLName
            // 
            this.txtLName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtLName.FocusLostColor = System.Drawing.Color.White;
            this.txtLName.Location = new System.Drawing.Point(360, 33);
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(98, 20);
            this.txtLName.TabIndex = 3;
            this.txtLName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(366, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Last Name:";
            // 
            // txtMName
            // 
            this.txtMName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtMName.FocusLostColor = System.Drawing.Color.White;
            this.txtMName.Location = new System.Drawing.Point(246, 33);
            this.txtMName.Name = "txtMName";
            this.txtMName.Size = new System.Drawing.Size(98, 20);
            this.txtMName.TabIndex = 2;
            this.txtMName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Middle Name:";
            // 
            // txtFName
            // 
            this.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtFName.FocusLostColor = System.Drawing.Color.White;
            this.txtFName.Location = new System.Drawing.Point(130, 33);
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(98, 20);
            this.txtFName.TabIndex = 1;
            this.txtFName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "First Name:";
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCode.FocusLostColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(23, 33);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(98, 20);
            this.txtCode.TabIndex = 0;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "RegNo:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(21, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(936, 64);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By:";
            // 
            // tabEmployees
            // 
            this.tabEmployees.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabEmployees.Controls.Add(this.taPatient);
            this.tabEmployees.Location = new System.Drawing.Point(17, 78);
            this.tabEmployees.Name = "tabEmployees";
            this.tabEmployees.SelectedIndex = 0;
            this.tabEmployees.Size = new System.Drawing.Size(940, 380);
            this.tabEmployees.TabIndex = 19;
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDate.FocusLostColor = System.Drawing.Color.White;
            this.txtDate.Location = new System.Drawing.Point(477, 33);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(98, 20);
            this.txtDate.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(483, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Date";
            // 
            // FrmListOfPatientInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 529);
            this.Controls.Add(this.btnAddEmployee);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabEmployees);
            this.MaximizeBox = false;
            this.Name = "FrmListOfPatientInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List Of Patient ";
            this.Load += new System.EventHandler(this.FrmListOfPatientInfo_Load);
            this.taPatient.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabEmployees.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddEmployee;
        private System.Windows.Forms.TabPage taPatient;
        private SourceGrid.Grid grdEmployee;
        private SComponents.STextBox txtLName;
        private System.Windows.Forms.Label label3;
        private SComponents.STextBox txtMName;
        private System.Windows.Forms.Label label1;
        private SComponents.STextBox txtFName;
        private System.Windows.Forms.Label label5;
        private SComponents.STextBox txtCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabEmployees;
        private SComponents.STextBox txtDate;
        private System.Windows.Forms.Label label2;
    }
}