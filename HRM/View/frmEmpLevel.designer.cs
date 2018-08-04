namespace HRM.View
{
    partial class frmEmpLevel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmpLevel));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtGradeAmt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGradeNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLvlBasicSal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLvlID = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnedit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.btnnew = new System.Windows.Forms.Button();
            this.txtLvlCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLvlName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdListView = new SourceGrid.Grid();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(223, 14);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(423, 254);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtGradeAmt);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtGradeNo);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtRemarks);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtLvlBasicSal);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtLvlID);
            this.tabPage2.Controls.Add(this.btnCancel);
            this.tabPage2.Controls.Add(this.btnedit);
            this.tabPage2.Controls.Add(this.btnDelete);
            this.tabPage2.Controls.Add(this.btnsave);
            this.tabPage2.Controls.Add(this.btnnew);
            this.tabPage2.Controls.Add(this.txtLvlCode);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtLvlName);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(415, 228);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Level";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtGradeAmt
            // 
            this.txtGradeAmt.Enabled = false;
            this.txtGradeAmt.Location = new System.Drawing.Point(147, 130);
            this.txtGradeAmt.Name = "txtGradeAmt";
            this.txtGradeAmt.Size = new System.Drawing.Size(137, 20);
            this.txtGradeAmt.TabIndex = 4;
            this.txtGradeAmt.Text = "0.00";
            this.txtGradeAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Per Grade Amount";
            // 
            // txtGradeNo
            // 
            this.txtGradeNo.Enabled = false;
            this.txtGradeNo.Location = new System.Drawing.Point(147, 104);
            this.txtGradeNo.Name = "txtGradeNo";
            this.txtGradeNo.Size = new System.Drawing.Size(137, 20);
            this.txtGradeNo.TabIndex = 3;
            this.txtGradeNo.Text = "0";
            this.txtGradeNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Max. Grade Number";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(147, 156);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(258, 20);
            this.txtRemarks.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Remarks";
            // 
            // txtLvlBasicSal
            // 
            this.txtLvlBasicSal.Enabled = false;
            this.txtLvlBasicSal.Location = new System.Drawing.Point(147, 78);
            this.txtLvlBasicSal.Name = "txtLvlBasicSal";
            this.txtLvlBasicSal.Size = new System.Drawing.Size(137, 20);
            this.txtLvlBasicSal.TabIndex = 2;
            this.txtLvlBasicSal.Text = "0.00";
            this.txtLvlBasicSal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Basic Salary";
            // 
            // txtLvlID
            // 
            this.txtLvlID.Location = new System.Drawing.Point(6, 6);
            this.txtLvlID.Name = "txtLvlID";
            this.txtLvlID.Size = new System.Drawing.Size(26, 20);
            this.txtLvlID.TabIndex = 10;
            this.txtLvlID.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(330, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnedit
            // 
            this.btnedit.Image = ((System.Drawing.Image)(resources.GetObject("btnedit.Image")));
            this.btnedit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnedit.Location = new System.Drawing.Point(87, 199);
            this.btnedit.Name = "btnedit";
            this.btnedit.Size = new System.Drawing.Size(75, 23);
            this.btnedit.TabIndex = 7;
            this.btnedit.Text = "&Edit";
            this.btnedit.UseVisualStyleBackColor = true;
            this.btnedit.Click += new System.EventHandler(this.btnedit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(249, 199);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnsave
            // 
            this.btnsave.Image = ((System.Drawing.Image)(resources.GetObject("btnsave.Image")));
            this.btnsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.Location = new System.Drawing.Point(168, 199);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 8;
            this.btnsave.Text = "&Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btnnew
            // 
            this.btnnew.Image = ((System.Drawing.Image)(resources.GetObject("btnnew.Image")));
            this.btnnew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnnew.Location = new System.Drawing.Point(6, 199);
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(75, 23);
            this.btnnew.TabIndex = 6;
            this.btnnew.Text = "&New";
            this.btnnew.UseVisualStyleBackColor = true;
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // txtLvlCode
            // 
            this.txtLvlCode.Enabled = false;
            this.txtLvlCode.Location = new System.Drawing.Point(147, 26);
            this.txtLvlCode.Name = "txtLvlCode";
            this.txtLvlCode.Size = new System.Drawing.Size(137, 20);
            this.txtLvlCode.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Level Code";
            // 
            // txtLvlName
            // 
            this.txtLvlName.Enabled = false;
            this.txtLvlName.Location = new System.Drawing.Point(147, 52);
            this.txtLvlName.Name = "txtLvlName";
            this.txtLvlName.Size = new System.Drawing.Size(137, 20);
            this.txtLvlName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Level Name";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(14, 14);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(203, 254);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(195, 228);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "List View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdListView
            // 
            this.grdListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdListView.EnableSort = true;
            this.grdListView.Location = new System.Drawing.Point(3, 3);
            this.grdListView.Name = "grdListView";
            this.grdListView.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListView.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListView.Size = new System.Drawing.Size(189, 222);
            this.grdListView.TabIndex = 1;
            this.grdListView.TabStop = true;
            this.grdListView.ToolTipText = "";
            // 
            // frmEmpLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 279);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmpLevel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEmpLevel";
            this.Load += new System.EventHandler(this.frmEmpLevel_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtLvlID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnedit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnnew;
        private System.Windows.Forms.TextBox txtLvlCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLvlName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdListView;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLvlBasicSal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGradeAmt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGradeNo;
        private System.Windows.Forms.Label label5;
    }
}