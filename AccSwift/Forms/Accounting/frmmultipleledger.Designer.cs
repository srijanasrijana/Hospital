using SComponents;
namespace Inventory.Forms.Accounting
{
    partial class frmmultipleledger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmmultipleledger));
            this.cboLdrAcGroup = new SComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLdrSave = new System.Windows.Forms.Button();
            this.btnLdrCancel = new System.Windows.Forms.Button();
            this.grdmultipleledger = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // cboLdrAcGroup
            // 
            this.cboLdrAcGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLdrAcGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLdrAcGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.cboLdrAcGroup.FocusLostColor = System.Drawing.Color.White;
            this.cboLdrAcGroup.FormattingEnabled = true;
            this.cboLdrAcGroup.Location = new System.Drawing.Point(119, 12);
            this.cboLdrAcGroup.Name = "cboLdrAcGroup";
            this.cboLdrAcGroup.Size = new System.Drawing.Size(156, 21);
            this.cboLdrAcGroup.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Account Head:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLdrSave
            // 
            this.btnLdrSave.Image = global::Inventory.Properties.Resources.save;
            this.btnLdrSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLdrSave.Location = new System.Drawing.Point(25, 212);
            this.btnLdrSave.Name = "btnLdrSave";
            this.btnLdrSave.Size = new System.Drawing.Size(75, 26);
            this.btnLdrSave.TabIndex = 19;
            this.btnLdrSave.Text = "&Save";
            this.btnLdrSave.UseVisualStyleBackColor = true;
            this.btnLdrSave.Click += new System.EventHandler(this.btnLdrSave_Click);
            // 
            // btnLdrCancel
            // 
            this.btnLdrCancel.Image = global::Inventory.Properties.Resources.gnome_window_close;
            this.btnLdrCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLdrCancel.Location = new System.Drawing.Point(125, 212);
            this.btnLdrCancel.Name = "btnLdrCancel";
            this.btnLdrCancel.Size = new System.Drawing.Size(75, 26);
            this.btnLdrCancel.TabIndex = 20;
            this.btnLdrCancel.Text = "&Close";
            this.btnLdrCancel.UseVisualStyleBackColor = true;
            this.btnLdrCancel.Click += new System.EventHandler(this.btnLdrCancel_Click);
            // 
            // grdmultipleledger
            // 
            this.grdmultipleledger.Location = new System.Drawing.Point(13, 39);
            this.grdmultipleledger.Name = "grdmultipleledger";
            this.grdmultipleledger.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdmultipleledger.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grdmultipleledger.Size = new System.Drawing.Size(385, 167);
            this.grdmultipleledger.TabIndex = 21;
            this.grdmultipleledger.TabStop = true;
            this.grdmultipleledger.ToolTipText = "";
            // 
            // frmmultipleledger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 248);
            this.Controls.Add(this.grdmultipleledger);
            this.Controls.Add(this.btnLdrCancel);
            this.Controls.Add(this.btnLdrSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboLdrAcGroup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmmultipleledger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multiple Ledger Creation";
            this.Load += new System.EventHandler(this.frmmultipleledger_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SComboBox cboLdrAcGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLdrSave;
        private System.Windows.Forms.Button btnLdrCancel;
        private SourceGrid.Grid grdmultipleledger;
    }
}