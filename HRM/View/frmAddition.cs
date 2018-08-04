using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;

namespace HRM
{
    public partial class frmAddition : Form
    {
        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        private bool IsSelected = false;
        private EntryMode m_mode = EntryMode.NORMAL;
        int isAddition;
        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        public frmAddition()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            both();
        }

        private void frmAddition_Load(object sender, EventArgs e)
        {
           
            dTable = employees.getAdditionDeduction();
            drFound = dTable.Select(FilterString);
            fillGrid();
            ChangeState(EntryMode.NEW);
            cleartextbox();
            txtname.Focus();
            radbtnAddition.Checked = true;
            radbtnBothInGrid.Checked = true;
           
        }

        private void sTextBox1_TextChanged(object sender, EventArgs e)
        {
            search();
            cleartextbox();
            ChangeState(EntryMode.NORMAL);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void fillGrid()
        {
            grdAddition.Rows.Clear();
            grdAddition.Redim(drFound.Count() + 1, 4);
            writeHeader();

            // SourceGrid.Cells.CheckBox chkMatch = new SourceGrid.Cells.CheckBox();

            for (int i = 1; i <= drFound.Count(); i++)
            {

                DataRow dr = drFound[i - 1];
                grdAddition[i, 0] = new SourceGrid.Cells.Cell(dr["AdditionDeductionID"].ToString());
                grdAddition[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                grdAddition[i, 2] = new SourceGrid.Cells.Cell(dr["Code"].ToString());
                if (dr["IsAddition"].ToString() == "True")
                {
                    string type = "Addition";
                    grdAddition[i, 3] = new SourceGrid.Cells.Cell(type);
                }
               else if (dr["IsAddition"].ToString() == "False")
                {
                    string type = "Deduction";
                    grdAddition[i, 3] = new SourceGrid.Cells.Cell(type);
                }
                

            }
        }
        private void writeHeader()
        {
            grdAddition[0, 0] = new MyHeader("AdditionDeductionID");
            grdAddition[0, 1] = new MyHeader("Addition/Deduction");
            grdAddition[0, 2] = new MyHeader("Code");
            grdAddition[0, 3] = new MyHeader("Type");
           

            grdAddition[0, 0].Column.Width = 1;
            grdAddition[0, 1].Column.Width = 150;
            grdAddition[0, 2].Column.Width = 80;
            grdAddition[0, 3].Column.Width = 95;

            grdAddition[0, 0].Column.Visible = false;
            grdAddition[0, 2].Column.Visible = false;
            
        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }

        private void EnableControls(bool Enable)
        {
            txtname.Enabled = txtCode.Enabled = radbtnAddition.Enabled = radbtnDeduction.Enabled = Enable;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(true, false, true, false, true);
                    
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (m_mode)
            {
                case EntryMode.NEW:
                    if(employees.GetAddDedIdByName(txtname.Text.Trim()) != "")
                    {
                        MessageBox.Show("The name you are trying to create is already in use.");
                        return;
                    }
                    if(radbtnAddition.Checked==true)
                    {
                         isAddition=1;
                    }
                    else if(radbtnAddition.Checked==false)
                    {
                        isAddition = 0;
                    }
                   
                    int isInserted = employees.createAdditionDeduction(txtname.Text, txtCode.Text,isAddition);

                    TextBox txtLedgerCode = new TextBox();
                        //Ledger Code Creation
                        #region  For Automation of Ledgercode 
                        int numberingType = 0;
                            DataTable config = new DataTable();
                            DataTable format = new DataTable();

                            config = Ledger.GetLedgerConfig();
                        foreach (DataRow getconfig in config.Rows)
                        {
                            numberingType = Convert.ToInt32(getconfig["NumberingType"]);

                        }
                        if (numberingType == 1)
                        {
                            format = Ledger.GetFormatParameter();
                            string str = "";
                            foreach (DataRow dr in format.Rows)
                            {
                                switch (dr["Type"].ToString())
                                {
                                    case "Symbol":
                                        str = str + dr["Parameter"].ToString();
                                        break;
                                    case "(AutoNumber)":
                                        {
                                            if (Convert.ToInt32(dr["Parameter"]) < 10)
                                                str = str + "00000" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 10 && (Convert.ToInt32(dr["Parameter"]) < 100))
                                                str = str + "0000" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 100 && (Convert.ToInt32(dr["Parameter"]) < 1000))
                                                str = str + "000" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 1000 && (Convert.ToInt32(dr["Parameter"]) < 10000))
                                                str = str + "00" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 10000 && (Convert.ToInt32(dr["Parameter"]) < 100000))
                                                str = str + "0" + dr["parameter"].ToString();
                                            else
                                                str = str + dr["parameter"].ToString();
                                            break;

                                        }
                                    case "Date":
                                        {

                                            if (dr["Parameter"].ToString() == "NEPALI_FISCAL_YEAR")
                                                str = str + Global.Fiscal_Nepali_Year;

                                            else if (dr["Parameter"].ToString() == "ENGLISH_FISCAL_YEAR")
                                                str = str + Global.Fiscal_English_Year;
                                            break;

                                        }
                                }

                            }

                            txtLedgerCode.Text = str;
                            this.Refresh();
                        }
                        #endregion
                       
                      
                        
                        //Creation Of the Ledger of Respective ledgername
                        ILedger acLedger = new Ledger();
                        DateTime currentdate = Date.GetServerDate();
                        if (radbtnAddition.Checked == true)
                        {

                            int ledgerIDOut = 0;
                            bool result = acLedger.Create(txtLedgerCode.Text.Trim(), txtname.Text.Trim(), 40, 0.00, "DEBIT", 1, 0.00, currentdate, "", "", "", "", "", "", "", "", "", "", 0.0, false, 0, "", "New Ledger Name=" + txtCode + txtname.Text + "Parent Group" + "Allowances", true,out ledgerIDOut);
                            //Increment the ledger code (automated number)
                            Global.m_db.InsertUpdateQry("update acc.tblLedgerCodeFormat set Parameter=Parameter+1 where TYPE='(AutoNumber)' ");
                        }
                    MessageBox.Show("Addition and Deduction Head Created Successfully");
                    break;
                case EntryMode.EDIT:
                    if (employees.GetAddDedIdByName(txtname.Text.Trim(),Convert.ToInt32(txtid.Text)) != "")
                    {
                        MessageBox.Show("The name you are trying to create is already in use.");
                        return;
                    }
                    if(radbtnAddition.Checked==true)
                    {
                         isAddition=1;
                    }
                    else if(radbtnAddition.Checked==false)
                    {
                        isAddition = 0;
                    }

                    int isSuccess = employees.updateAdditionDeduction(Convert.ToInt32(txtid.Text), txtname.Text, txtCode.Text, isAddition);
                    if (isSuccess > 0)
                    {
                        MessageBox.Show("Addition and Deduction Head Updated Successfully");
                        cleartextbox();
                    }
                    break;
            }
            ChangeState(EntryMode.NEW);
            frmAddition_Load(sender, e);
        }
        private void additiononly()
        {
            dTable = employees.getAdditionOnly();
            drFound = dTable.Select(FilterString);
            
            grdAddition.Rows.Clear();
            grdAddition.Redim(drFound.Count() + 1, 4);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {

                DataRow dr = drFound[i - 1];
                grdAddition[i, 0] = new SourceGrid.Cells.Cell(dr["AdditionDeductionID"].ToString());
                grdAddition[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                grdAddition[i, 2] = new SourceGrid.Cells.Cell(dr["Code"].ToString());
                if (dr["IsAddition"].ToString() == "True")
                {
                    string type = "Addition";
                    grdAddition[i, 3] = new SourceGrid.Cells.Cell(type);
                }
               
                    

            }
           
        }

        private void radbtnAdditionInGrid_CheckedChanged(object sender, EventArgs e)
        {
            additiononly();
            cleartextbox();
            ChangeState(EntryMode.NEW);
            btnDelete.Enabled = false;
        }

        private void radbtnDeductionInGrid_CheckedChanged(object sender, EventArgs e)
        {
            deductionOnly();
            cleartextbox();
            ChangeState(EntryMode.NEW);
            btnDelete.Enabled = false;
        }
        private void deductionOnly()
        {
            dTable = employees.getDeductionOnly();
            drFound = dTable.Select(FilterString);

            grdAddition.Rows.Clear();
            grdAddition.Redim(drFound.Count() + 1, 4);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {

                DataRow dr = drFound[i - 1];
                grdAddition[i, 0] = new SourceGrid.Cells.Cell(dr["AdditionDeductionID"].ToString());
                grdAddition[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                grdAddition[i, 2] = new SourceGrid.Cells.Cell(dr["Code"].ToString());
                if (dr["IsAddition"].ToString() == "False")
                {
                    string type = "Deduction";
                    grdAddition[i, 3] = new SourceGrid.Cells.Cell(type);
                }



            }

        }
        private void both()
        {
            dTable = employees.getAdditionDeduction();
            drFound = dTable.Select(FilterString);

            grdAddition.Rows.Clear();
            grdAddition.Redim(drFound.Count() + 1, 4);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {

                DataRow dr = drFound[i - 1];
                grdAddition[i, 0] = new SourceGrid.Cells.Cell(dr["AdditionDeductionID"].ToString());
                grdAddition[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                grdAddition[i, 2] = new SourceGrid.Cells.Cell(dr["Code"].ToString());
                if (dr["IsAddition"].ToString() == "True")
                {
                    string type = "Addition";
                    grdAddition[i, 3] = new SourceGrid.Cells.Cell(type);
                }
                else if (dr["IsAddition"].ToString() == "False")
                {
                    string type = "Deduction";
                    grdAddition[i, 3] = new SourceGrid.Cells.Cell(type);
                }


            }

        }
        private void search()
        {
            if (radbtnBothInGrid.Checked == true)
            {
                this.FilterString = "Name like '" + txtadditionSearch.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (radbtnAdditionInGrid.Checked == true)
            {
                this.FilterString = "Name like '" + txtadditionSearch.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (radbtnDeductionInGrid.Checked == true)
            {
                this.FilterString = "Name like '" + txtadditionSearch.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
        }

        private void grdAddition_Click(object sender, EventArgs e)
        {
            IsSelected = true;
            int currentRow = grdAddition.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cell = new SourceGrid.CellContext(grdAddition, new SourceGrid.Position(currentRow, 0));
            int id = Convert.ToInt32(grdAddition[currentRow, 0].Value.ToString());
            fillAdditionDed(id);
            ButtonState(true, true, false, true, true);
            txtname.Enabled = false;
            txtCode.Enabled = false;
            radbtnAddition.Enabled = false;
            radbtnDeduction.Enabled = false;
            ChangeState(EntryMode.NORMAL);
          
        }
        private void fillAdditionDed(int ID)
        {
            DataTable dt = employees.getAdditionDeductionID(ID);
            DataRow dr = dt.Rows[0];
            txtid.Text = dr["AdditionDeductionID"].ToString();
            txtname.Text = dr["Name"].ToString();
            txtCode.Text = dr["Code"].ToString();
            if (dr["IsAddition"].ToString() == "True")
            {
                radbtnAddition.Checked = true;
            }
            else if (dr["IsAddition"].ToString() == "False")
            {
                radbtnDeduction.Checked = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.EDIT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = employees.deleteFromAdditionDeduction(Convert.ToInt32(txtid.Text));
            MessageBox.Show("record Deleted");
            cleartextbox();
            frmAddition_Load(sender, e);
        }
        private void cleartextbox()
        {
            txtname.Clear();
            txtCode.Clear();
            txtid.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            cleartextbox();
            ChangeState(EntryMode.NEW);
        }
    }
}
