using SComponents;
namespace AccSwift
{
    partial class frmDepot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDepot));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvDepot = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtDepotID = new STextBox();
            this.txtAddress = new STextBox();
            this.txtRemarks = new STextBox();
            this.txtRegNo = new STextBox();
            this.txtMobile = new STextBox();
            this.txtPostalCode = new STextBox();
            this.txtDepotName = new STextBox();
            this.txtLicenceNo = new STextBox();
            this.txtContactPerson = new STextBox();
            this.txtTelephone = new STextBox();
            this.txtCity = new STextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "*Depot Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "City:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Telephone:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Contact Person:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Licence No. :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(270, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Address:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(270, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Mobile:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(270, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Postal Code:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(270, 118);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Reg. No. :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(270, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Remarks:";
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Inventory.Properties.Resources.edit_add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(124, 180);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 10;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::Inventory.Properties.Resources.document_edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(205, 180);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(286, 180);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Inventory.Properties.Resources.document_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(367, 180);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(448, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lvDepot
            // 
            this.lvDepot.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvDepot.FullRowSelect = true;
            this.lvDepot.GridLines = true;
            this.lvDepot.Location = new System.Drawing.Point(504, 29);
            this.lvDepot.Name = "lvDepot";
            this.lvDepot.Size = new System.Drawing.Size(214, 128);
            this.lvDepot.TabIndex = 27;
            this.lvDepot.UseCompatibleStateImageBehavior = false;
            this.lvDepot.View = System.Windows.Forms.View.Details;
            this.lvDepot.SelectedIndexChanged += new System.EventHandler(this.lvDepot_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "S.No.";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Depot Name:";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "DepotID";
            // 
            // txtDepotID
            // 
            this.txtDepotID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDepotID.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDepotID.FocusLostColor = System.Drawing.Color.White;
            this.txtDepotID.Location = new System.Drawing.Point(101, 7);
            this.txtDepotID.Name = "txtDepotID";
            this.txtDepotID.Size = new System.Drawing.Size(75, 20);
            this.txtDepotID.TabIndex = 28;
            // 
            // txtAddress
            // 
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtAddress.FocusLostColor = System.Drawing.Color.White;
            this.txtAddress.Location = new System.Drawing.Point(342, 33);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(138, 20);
            this.txtAddress.TabIndex = 1;
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRemarks.FocusLostColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(342, 137);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(138, 20);
            this.txtRemarks.TabIndex = 9;
            // 
            // txtRegNo
            // 
            this.txtRegNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtRegNo.FocusLostColor = System.Drawing.Color.White;
            this.txtRegNo.Location = new System.Drawing.Point(342, 111);
            this.txtRegNo.Name = "txtRegNo";
            this.txtRegNo.Size = new System.Drawing.Size(138, 20);
            this.txtRegNo.TabIndex = 7;
            // 
            // txtMobile
            // 
            this.txtMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMobile.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtMobile.FocusLostColor = System.Drawing.Color.White;
            this.txtMobile.Location = new System.Drawing.Point(342, 85);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(138, 20);
            this.txtMobile.TabIndex = 5;
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPostalCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtPostalCode.FocusLostColor = System.Drawing.Color.White;
            this.txtPostalCode.Location = new System.Drawing.Point(342, 59);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(138, 20);
            this.txtPostalCode.TabIndex = 3;
            // 
            // txtDepotName
            // 
            this.txtDepotName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDepotName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtDepotName.FocusLostColor = System.Drawing.Color.White;
            this.txtDepotName.Location = new System.Drawing.Point(101, 33);
            this.txtDepotName.Name = "txtDepotName";
            this.txtDepotName.Size = new System.Drawing.Size(149, 20);
            this.txtDepotName.TabIndex = 0;
            // 
            // txtLicenceNo
            // 
            this.txtLicenceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLicenceNo.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtLicenceNo.FocusLostColor = System.Drawing.Color.White;
            this.txtLicenceNo.Location = new System.Drawing.Point(101, 137);
            this.txtLicenceNo.Name = "txtLicenceNo";
            this.txtLicenceNo.Size = new System.Drawing.Size(149, 20);
            this.txtLicenceNo.TabIndex = 8;
            // 
            // txtContactPerson
            // 
            this.txtContactPerson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContactPerson.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtContactPerson.FocusLostColor = System.Drawing.Color.White;
            this.txtContactPerson.Location = new System.Drawing.Point(101, 111);
            this.txtContactPerson.Name = "txtContactPerson";
            this.txtContactPerson.Size = new System.Drawing.Size(149, 20);
            this.txtContactPerson.TabIndex = 6;
            // 
            // txtTelephone
            // 
            this.txtTelephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephone.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtTelephone.FocusLostColor = System.Drawing.Color.White;
            this.txtTelephone.Location = new System.Drawing.Point(101, 85);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(149, 20);
            this.txtTelephone.TabIndex = 4;
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCity.FocusLostColor = System.Drawing.Color.White;
            this.txtCity.Location = new System.Drawing.Point(101, 59);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(149, 20);
            this.txtCity.TabIndex = 2;
            // 
            // frmDepot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 216);
            this.Controls.Add(this.txtDepotID);
            this.Controls.Add(this.lvDepot);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.txtRegNo);
            this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.txtDepotName);
            this.Controls.Add(this.txtLicenceNo);
            this.Controls.Add(this.txtContactPerson);
            this.Controls.Add(this.txtTelephone);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmDepot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Depot";
            this.Load += new System.EventHandler(this.frmDepot_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDepot_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private STextBox txtCity;
        private STextBox txtDepotName;
        private STextBox txtTelephone;
        private STextBox txtContactPerson;
        private STextBox txtLicenceNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private STextBox txtAddress;
        private STextBox txtPostalCode;
        private STextBox txtMobile;
        private STextBox txtRegNo;
        private STextBox txtRemarks;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvDepot;
        private STextBox txtDepotID;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}