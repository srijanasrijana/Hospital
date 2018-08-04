using SComponents;
namespace Common
{
    partial class frmSelectAccClass
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.treeAccClass = new SComponents.STreeView(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cboParentClass = new SComponents.SComboBox();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(248, 52);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 24;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // treeAccClass
            // 
            this.treeAccClass.AutoCheckChild = true;
            this.treeAccClass.CheckBoxes = true;
            this.treeAccClass.Location = new System.Drawing.Point(14, 52);
            this.treeAccClass.Name = "treeAccClass";
            this.treeAccClass.Size = new System.Drawing.Size(228, 312);
            this.treeAccClass.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Parent Class:";
            // 
            // cboParentClass
            // 
            this.cboParentClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParentClass.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.cboParentClass.FocusLostColor = System.Drawing.Color.White;
            this.cboParentClass.FormattingEnabled = true;
            this.cboParentClass.Location = new System.Drawing.Point(12, 25);
            this.cboParentClass.Name = "cboParentClass";
            this.cboParentClass.Size = new System.Drawing.Size(230, 21);
            this.cboParentClass.TabIndex = 27;
            this.cboParentClass.SelectedIndexChanged += new System.EventHandler(this.cboParentClass_SelectedIndexChanged);
            // 
            // frmSelectAccClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 370);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboParentClass);
            this.Controls.Add(this.treeAccClass);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSelectAccClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Account Class";
            this.Load += new System.EventHandler(this.frmSelectAccClass_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSelectAccClass_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private STreeView treeAccClass;
        private System.Windows.Forms.Label label2;
        private SComboBox cboParentClass;
    }
}