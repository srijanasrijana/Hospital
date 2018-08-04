namespace AccSwift
{
    partial class frmCompanyOpen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompanyOpen));
            this.lstCompany = new System.Windows.Forms.ListView();
            this.imlCompanyOpen = new System.Windows.Forms.ImageList(this.components);
            this.btnToggleView = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lstCompany
            // 
            this.lstCompany.LargeImageList = this.imlCompanyOpen;
            this.lstCompany.Location = new System.Drawing.Point(3, 71);
            this.lstCompany.Name = "lstCompany";
            this.lstCompany.Size = new System.Drawing.Size(502, 151);
            this.lstCompany.SmallImageList = this.imlCompanyOpen;
            this.lstCompany.TabIndex = 12;
            this.lstCompany.UseCompatibleStateImageBehavior = false;
            this.lstCompany.SelectedIndexChanged += new System.EventHandler(this.lstCompany_SelectedIndexChanged);
            this.lstCompany.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstCompany_MouseDoubleClick);
            // 
            // imlCompanyOpen
            // 
            this.imlCompanyOpen.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlCompanyOpen.ImageStream")));
            this.imlCompanyOpen.TransparentColor = System.Drawing.Color.Transparent;
            this.imlCompanyOpen.Images.SetKeyName(0, "company_icon.png");
            // 
            // btnToggleView
            // 
            this.btnToggleView.Location = new System.Drawing.Point(511, 129);
            this.btnToggleView.Name = "btnToggleView";
            this.btnToggleView.Size = new System.Drawing.Size(75, 27);
            this.btnToggleView.TabIndex = 13;
            this.btnToggleView.Text = "&Toggle View";
            this.btnToggleView.UseVisualStyleBackColor = true;
            this.btnToggleView.Click += new System.EventHandler(this.btnToggleView_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(594, 68);
            this.label1.TabIndex = 14;
            this.label1.Text = "Please select a company in the list below to open it.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Location = new System.Drawing.Point(511, 195);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(75, 27);
            this.btnCreateNew.TabIndex = 15;
            this.btnCreateNew.Text = "&Create New";
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::Inventory.Properties.Resources.company;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(511, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen.Location = new System.Drawing.Point(511, 71);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 27);
            this.btnOpen.TabIndex = 10;
            this.btnOpen.Text = "&Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(70, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Open Company";
            // 
            // frmCompanyOpen
            // 
            this.AcceptButton = this.btnOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 227);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnToggleView);
            this.Controls.Add(this.lstCompany);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCompanyOpen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Company";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCompanyOpen_FormClosing);
            this.Load += new System.EventHandler(this.frmCompanyOpen_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCompanyOpen_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ListView lstCompany;
        private System.Windows.Forms.ImageList imlCompanyOpen;
        private System.Windows.Forms.Button btnToggleView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreateNew;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
    }
}