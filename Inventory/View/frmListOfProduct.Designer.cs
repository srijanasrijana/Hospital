using SComponents;
namespace Inventory
{
    partial class frmListOfProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListOfProduct));
            this.grdProduct = new SourceGrid.Grid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtproductGroup = new SComponents.STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtproductCode = new SComponents.STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProductName = new SComponents.STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_addproduct = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdProduct
            // 
            this.grdProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdProduct.EnableSort = true;
            this.grdProduct.Location = new System.Drawing.Point(3, 6);
            this.grdProduct.Name = "grdProduct";
            this.grdProduct.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grdProduct.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdProduct.Size = new System.Drawing.Size(784, 380);
            this.grdProduct.TabIndex = 7;
            this.grdProduct.TabStop = true;
            this.grdProduct.ToolTipText = "";
            this.grdProduct.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Productgrid_PreviewKeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtproductGroup);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtproductCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtProductName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 427);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(784, 64);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By:";
            // 
            // txtproductGroup
            // 
            this.txtproductGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtproductGroup.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtproductGroup.FocusLostColor = System.Drawing.Color.White;
            this.txtproductGroup.Location = new System.Drawing.Point(459, 34);
            this.txtproductGroup.Name = "txtproductGroup";
            this.txtproductGroup.Size = new System.Drawing.Size(154, 20);
            this.txtproductGroup.TabIndex = 4;
            this.txtproductGroup.TextChanged += new System.EventHandler(this.txtProductGroup_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(498, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "GroupName:";
            // 
            // txtproductCode
            // 
            this.txtproductCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtproductCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtproductCode.FocusLostColor = System.Drawing.Color.White;
            this.txtproductCode.Location = new System.Drawing.Point(96, 34);
            this.txtproductCode.Name = "txtproductCode";
            this.txtproductCode.Size = new System.Drawing.Size(100, 20);
            this.txtproductCode.TabIndex = 2;
            this.txtproductCode.TextChanged += new System.EventHandler(this.txtproductCode_TextChanged);
            this.txtproductCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtproductCode_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(126, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Code:";
            // 
            // txtProductName
            // 
            this.txtProductName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProductName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtProductName.FocusLostColor = System.Drawing.Color.White;
            this.txtProductName.Location = new System.Drawing.Point(255, 34);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(154, 20);
            this.txtProductName.TabIndex = 3;
            this.txtProductName.TextChanged += new System.EventHandler(this.txtProductName_TextChanged);
            this.txtProductName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductName_KeyDown);
            this.txtProductName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProductName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(310, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // btn_addproduct
            // 
            this.btn_addproduct.Location = new System.Drawing.Point(705, 399);
            this.btn_addproduct.Name = "btn_addproduct";
            this.btn_addproduct.Size = new System.Drawing.Size(75, 23);
            this.btn_addproduct.TabIndex = 5;
            this.btn_addproduct.Text = "Add Product";
            this.btn_addproduct.UseVisualStyleBackColor = true;
            this.btn_addproduct.Click += new System.EventHandler(this.btn_addproduct_Click);
            // 
            // frmListOfProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 496);
            this.Controls.Add(this.btn_addproduct);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grdProduct);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmListOfProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List Of Product";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmListOfProduct_FormClosing);
            this.Load += new System.EventHandler(this.frmListOfProduct_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmListOfProduct_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid grdProduct;
        private System.Windows.Forms.GroupBox groupBox1;
        private STextBox txtproductCode;
        private System.Windows.Forms.Label label4;
        private STextBox txtProductName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_addproduct;
        private STextBox txtproductGroup;
        private System.Windows.Forms.Label label2;
    }
}