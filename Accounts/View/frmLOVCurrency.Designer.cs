using SComponents;
namespace Accounts
{
    partial class frmLOVCurrency
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
            this.LOVGrid = new SourceGrid.Grid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCountry = new STextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCode = new STextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSubstring = new STextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSymbol = new STextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new STextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LOVGrid
            // 
            this.LOVGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LOVGrid.Location = new System.Drawing.Point(1, 0);
            this.LOVGrid.Name = "LOVGrid";
            this.LOVGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.LOVGrid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.LOVGrid.Size = new System.Drawing.Size(569, 279);
            this.LOVGrid.TabIndex = 0;
            this.LOVGrid.TabStop = true;
            this.LOVGrid.ToolTipText = "";
            this.LOVGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LOVGrid_PreviewKeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtCountry);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSubstring);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSymbol);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 70);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By:";
            // 
            // txtCountry
            // 
            this.txtCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCountry.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCountry.FocusLostColor = System.Drawing.Color.White;
            this.txtCountry.Location = new System.Drawing.Point(431, 37);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(100, 20);
            this.txtCountry.TabIndex = 4;
            this.txtCountry.TextChanged += new System.EventHandler(this.txtCountry_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(452, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Country:";
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtCode.FocusLostColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(219, 37);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 20);
            this.txtCode.TabIndex = 2;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Code:";
            // 
            // txtSubstring
            // 
            this.txtSubstring.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubstring.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSubstring.FocusLostColor = System.Drawing.Color.White;
            this.txtSubstring.Location = new System.Drawing.Point(325, 37);
            this.txtSubstring.Name = "txtSubstring";
            this.txtSubstring.Size = new System.Drawing.Size(100, 20);
            this.txtSubstring.TabIndex = 3;
            this.txtSubstring.TextChanged += new System.EventHandler(this.txtSubstring_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(343, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Substring:";
            // 
            // txtSymbol
            // 
            this.txtSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbol.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtSymbol.FocusLostColor = System.Drawing.Color.White;
            this.txtSymbol.Location = new System.Drawing.Point(113, 37);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(100, 20);
            this.txtSymbol.TabIndex = 1;
            this.txtSymbol.TextChanged += new System.EventHandler(this.txtSymbol_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Symbol:";
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.txtName.FocusLostColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(6, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // frmLOVCurrency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 365);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LOVGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmLOVCurrency";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Currency - List Of Values";
            this.Load += new System.EventHandler(this.frmLOVCurrency_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLOVCurrency_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.Grid LOVGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private STextBox txtCountry;
        private System.Windows.Forms.Label label5;
        private STextBox txtCode;
        private System.Windows.Forms.Label label4;
        private STextBox txtSubstring;
        private System.Windows.Forms.Label label3;
        private STextBox txtSymbol;
        private System.Windows.Forms.Label label2;
        private STextBox txtName;
        private System.Windows.Forms.Label label1;
    }
}