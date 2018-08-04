namespace Hospital.View
{
    partial class frmHosDepartment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHosDepartment));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtd_id = new System.Windows.Forms.TextBox();
            this.btnProductCancel = new System.Windows.Forms.Button();
            this.btnProductEdit = new System.Windows.Forms.Button();
            this.btnProductDelete = new System.Windows.Forms.Button();
            this.btnProductSave = new System.Windows.Forms.Button();
            this.btnProductNew = new System.Windows.Forms.Button();
            this.txtdepcode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtdname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdListview1 = new SourceGrid.Grid();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(230, 17);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(423, 254);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtd_id);
            this.tabPage2.Controls.Add(this.btnProductCancel);
            this.tabPage2.Controls.Add(this.btnProductEdit);
            this.tabPage2.Controls.Add(this.btnProductDelete);
            this.tabPage2.Controls.Add(this.btnProductSave);
            this.tabPage2.Controls.Add(this.btnProductNew);
            this.tabPage2.Controls.Add(this.txtdepcode);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtdname);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(415, 228);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Department";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtd_id
            // 
            this.txtd_id.Location = new System.Drawing.Point(6, 6);
            this.txtd_id.Name = "txtd_id";
            this.txtd_id.Size = new System.Drawing.Size(26, 20);
            this.txtd_id.TabIndex = 10;
            this.txtd_id.Visible = false;
            // 
            // btnProductCancel
            // 
            this.btnProductCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnProductCancel.Image")));
            this.btnProductCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductCancel.Location = new System.Drawing.Point(330, 199);
            this.btnProductCancel.Name = "btnProductCancel";
            this.btnProductCancel.Size = new System.Drawing.Size(75, 23);
            this.btnProductCancel.TabIndex = 6;
            this.btnProductCancel.Text = "&Cancel";
            this.btnProductCancel.UseVisualStyleBackColor = true;
            this.btnProductCancel.Click += new System.EventHandler(this.btnProductCancel_Click);
            // 
            // btnProductEdit
            // 
            this.btnProductEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnProductEdit.Image")));
            this.btnProductEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductEdit.Location = new System.Drawing.Point(87, 199);
            this.btnProductEdit.Name = "btnProductEdit";
            this.btnProductEdit.Size = new System.Drawing.Size(75, 23);
            this.btnProductEdit.TabIndex = 3;
            this.btnProductEdit.Text = "&Edit";
            this.btnProductEdit.UseVisualStyleBackColor = true;
            this.btnProductEdit.Click += new System.EventHandler(this.btnProductEdit_Click);
            // 
            // btnProductDelete
            // 
            this.btnProductDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnProductDelete.Image")));
            this.btnProductDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductDelete.Location = new System.Drawing.Point(249, 199);
            this.btnProductDelete.Name = "btnProductDelete";
            this.btnProductDelete.Size = new System.Drawing.Size(75, 23);
            this.btnProductDelete.TabIndex = 5;
            this.btnProductDelete.Text = "&Delete";
            this.btnProductDelete.UseVisualStyleBackColor = true;
            this.btnProductDelete.Click += new System.EventHandler(this.btnProductDelete_Click);
            // 
            // btnProductSave
            // 
            this.btnProductSave.Image = ((System.Drawing.Image)(resources.GetObject("btnProductSave.Image")));
            this.btnProductSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductSave.Location = new System.Drawing.Point(168, 199);
            this.btnProductSave.Name = "btnProductSave";
            this.btnProductSave.Size = new System.Drawing.Size(75, 23);
            this.btnProductSave.TabIndex = 4;
            this.btnProductSave.Text = "&Save";
            this.btnProductSave.UseVisualStyleBackColor = true;
            this.btnProductSave.Click += new System.EventHandler(this.btnProductSave_Click);
            // 
            // btnProductNew
            // 
            this.btnProductNew.Image = ((System.Drawing.Image)(resources.GetObject("btnProductNew.Image")));
            this.btnProductNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductNew.Location = new System.Drawing.Point(6, 199);
            this.btnProductNew.Name = "btnProductNew";
            this.btnProductNew.Size = new System.Drawing.Size(75, 23);
            this.btnProductNew.TabIndex = 2;
            this.btnProductNew.Text = "&New";
            this.btnProductNew.UseVisualStyleBackColor = true;
            this.btnProductNew.Click += new System.EventHandler(this.btnProductNew_Click);
            // 
            // txtdepcode
            // 
            this.txtdepcode.Enabled = false;
            this.txtdepcode.Location = new System.Drawing.Point(140, 26);
            this.txtdepcode.Name = "txtdepcode";
            this.txtdepcode.Size = new System.Drawing.Size(137, 20);
            this.txtdepcode.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Department Code";
            // 
            // txtdname
            // 
            this.txtdname.Enabled = false;
            this.txtdname.Location = new System.Drawing.Point(140, 65);
            this.txtdname.Name = "txtdname";
            this.txtdname.Size = new System.Drawing.Size(137, 20);
            this.txtdname.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Department Name";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(21, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(203, 254);
            this.tabControl1.TabIndex = 4;
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
            this.grdListview1.Paint += new System.Windows.Forms.PaintEventHandler(this.grdListView1_Paint);
            // 
            // frmHosDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 281);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabControl2);
            this.Name = "frmHosDepartment";
            this.Text = "frmDepartment";
            this.Load += new System.EventHandler(this.frmDepartment_Load);
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
        private System.Windows.Forms.TextBox txtd_id;
        private System.Windows.Forms.Button btnProductCancel;
        private System.Windows.Forms.Button btnProductEdit;
        private System.Windows.Forms.Button btnProductDelete;
        private System.Windows.Forms.Button btnProductSave;
        private System.Windows.Forms.Button btnProductNew;
        private System.Windows.Forms.TextBox txtdepcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtdname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private SourceGrid.Grid grdListview1;
    }
}