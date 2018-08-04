namespace Hospital.View
{
    partial class frmHosSpecialization
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHosSpecialization));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdListview1 = new SourceGrid.Grid();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtSpecilization_id = new System.Windows.Forms.TextBox();
            this.btnSpecilizationCancel = new System.Windows.Forms.Button();
            this.btnSpecilizationEdit = new System.Windows.Forms.Button();
            this.btnSpecilizationDelete = new System.Windows.Forms.Button();
            this.btnSpecilizationSave = new System.Windows.Forms.Button();
            this.btnSpecilizationNew = new System.Windows.Forms.Button();
            this.txtSpecilizationcode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSpecilizationname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(203, 254);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdListview1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(195, 228);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "List View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdListview1
            // 
            this.grdListview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdListview1.EnableSort = true;
            this.grdListview1.Location = new System.Drawing.Point(3, 3);
            this.grdListview1.Name = "grdListview1";
            this.grdListview1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdListview1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdListview1.Size = new System.Drawing.Size(189, 222);
            this.grdListview1.TabIndex = 1;
            this.grdListview1.TabStop = true;
            this.grdListview1.ToolTipText = "";
            this.grdListview1.Click += new System.EventHandler(this.grdListView1_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(221, 18);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(423, 254);
            this.tabControl2.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtSpecilization_id);
            this.tabPage2.Controls.Add(this.btnSpecilizationCancel);
            this.tabPage2.Controls.Add(this.btnSpecilizationEdit);
            this.tabPage2.Controls.Add(this.btnSpecilizationDelete);
            this.tabPage2.Controls.Add(this.btnSpecilizationSave);
            this.tabPage2.Controls.Add(this.btnSpecilizationNew);
            this.tabPage2.Controls.Add(this.txtSpecilizationcode);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtSpecilizationname);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(415, 228);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Specialization";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtSpecilization_id
            // 
            this.txtSpecilization_id.Location = new System.Drawing.Point(6, 6);
            this.txtSpecilization_id.Name = "txtSpecilization_id";
            this.txtSpecilization_id.Size = new System.Drawing.Size(26, 20);
            this.txtSpecilization_id.TabIndex = 10;
            this.txtSpecilization_id.Visible = false;
            // 
            // btnSpecilizationCancel
            // 
            this.btnSpecilizationCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnSpecilizationCancel.Image")));
            this.btnSpecilizationCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSpecilizationCancel.Location = new System.Drawing.Point(330, 199);
            this.btnSpecilizationCancel.Name = "btnSpecilizationCancel";
            this.btnSpecilizationCancel.Size = new System.Drawing.Size(75, 23);
            this.btnSpecilizationCancel.TabIndex = 6;
            this.btnSpecilizationCancel.Text = "&Cancel";
            this.btnSpecilizationCancel.UseVisualStyleBackColor = true;
            this.btnSpecilizationCancel.Click += new System.EventHandler(this.btnSpecilizationCancel_Click_1);
            // 
            // btnSpecilizationEdit
            // 
            this.btnSpecilizationEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnSpecilizationEdit.Image")));
            this.btnSpecilizationEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSpecilizationEdit.Location = new System.Drawing.Point(87, 199);
            this.btnSpecilizationEdit.Name = "btnSpecilizationEdit";
            this.btnSpecilizationEdit.Size = new System.Drawing.Size(75, 23);
            this.btnSpecilizationEdit.TabIndex = 3;
            this.btnSpecilizationEdit.Text = "&Edit";
            this.btnSpecilizationEdit.UseVisualStyleBackColor = true;
            this.btnSpecilizationEdit.Click += new System.EventHandler(this.btnSpecilizationEdit_Click);
            // 
            // btnSpecilizationDelete
            // 
            this.btnSpecilizationDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnSpecilizationDelete.Image")));
            this.btnSpecilizationDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSpecilizationDelete.Location = new System.Drawing.Point(249, 199);
            this.btnSpecilizationDelete.Name = "btnSpecilizationDelete";
            this.btnSpecilizationDelete.Size = new System.Drawing.Size(75, 23);
            this.btnSpecilizationDelete.TabIndex = 5;
            this.btnSpecilizationDelete.Text = "&Delete";
            this.btnSpecilizationDelete.UseVisualStyleBackColor = true;
            this.btnSpecilizationDelete.Click += new System.EventHandler(this.btnSpecilizationDelete_Click_1);
            // 
            // btnSpecilizationSave
            // 
            this.btnSpecilizationSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSpecilizationSave.Image")));
            this.btnSpecilizationSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSpecilizationSave.Location = new System.Drawing.Point(168, 199);
            this.btnSpecilizationSave.Name = "btnSpecilizationSave";
            this.btnSpecilizationSave.Size = new System.Drawing.Size(75, 23);
            this.btnSpecilizationSave.TabIndex = 4;
            this.btnSpecilizationSave.Text = "&Save";
            this.btnSpecilizationSave.UseVisualStyleBackColor = true;
            this.btnSpecilizationSave.Click += new System.EventHandler(this.btnSpecilizationSave_Click_1);
            // 
            // btnSpecilizationNew
            // 
            this.btnSpecilizationNew.Image = ((System.Drawing.Image)(resources.GetObject("btnSpecilizationNew.Image")));
            this.btnSpecilizationNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSpecilizationNew.Location = new System.Drawing.Point(6, 199);
            this.btnSpecilizationNew.Name = "btnSpecilizationNew";
            this.btnSpecilizationNew.Size = new System.Drawing.Size(75, 23);
            this.btnSpecilizationNew.TabIndex = 2;
            this.btnSpecilizationNew.Text = "&New";
            this.btnSpecilizationNew.UseVisualStyleBackColor = true;
            this.btnSpecilizationNew.Click += new System.EventHandler(this.btnSpecilizationNew_Click);
            // 
            // txtSpecilizationcode
            // 
            this.txtSpecilizationcode.Enabled = false;
            this.txtSpecilizationcode.Location = new System.Drawing.Point(140, 26);
            this.txtSpecilizationcode.Name = "txtSpecilizationcode";
            this.txtSpecilizationcode.Size = new System.Drawing.Size(137, 20);
            this.txtSpecilizationcode.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Specialization Code";
            // 
            // txtSpecilizationname
            // 
            this.txtSpecilizationname.Enabled = false;
            this.txtSpecilizationname.Location = new System.Drawing.Point(140, 65);
            this.txtSpecilizationname.Name = "txtSpecilizationname";
            this.txtSpecilizationname.Size = new System.Drawing.Size(137, 20);
            this.txtSpecilizationname.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Specilization Name";
            // 
            // frmHosSpecialization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 281);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabControl2);
            this.Name = "frmHosSpecialization";
            this.Text = "Specialization";
            this.Load += new System.EventHandler(this.frmHosSpecialization_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdListview1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtSpecilization_id;
        private System.Windows.Forms.Button btnSpecilizationCancel;
        private System.Windows.Forms.Button btnSpecilizationEdit;
        private System.Windows.Forms.Button btnSpecilizationDelete;
        private System.Windows.Forms.Button btnSpecilizationSave;
        private System.Windows.Forms.Button btnSpecilizationNew;
        private System.Windows.Forms.TextBox txtSpecilizationcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSpecilizationname;
        private System.Windows.Forms.Label label1;

    }
}