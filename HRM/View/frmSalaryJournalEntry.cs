using BusinessLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HRM.View
{
    public partial class frmSalaryJournalEntry : Form
    {
        private int m_paySlipID;
        private bool isEdit;
        private DataTable dtFound;
        private DataRow[] drFoundTotal;

        decimal totalSalary = 0;
        decimal totalAllowance = 0;
        decimal totalKKKosh = 0;
        decimal tPF = 0, tPFDeduct = 0, tTaxDeduct = 0, tSLInstalment = 0, tNLKosh = 0, tSLInterest = 0, tNetSalary = 0;
        public frmSalaryJournalEntry()
        {
            InitializeComponent();
        }
        public frmSalaryJournalEntry(int paySlipID, bool edit)
        {
            InitializeComponent();
            m_paySlipID = paySlipID;
            isEdit = edit;
        }

        BusinessLogic.HRM.Employee employee = new BusinessLogic.HRM.Employee();
        private void frmSalaryJournalEntry_Load(object sender, EventArgs e)
        {
            if (isEdit == true)
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = true;
            }
            LoadBank();
            //dtFound = employee.GetSalaryMaster(m_paySlipID);
            //drFoundTotal = dtFound.Select();
            //WriteHeader();
            //LoadGrid();
        }

        private void LoadBank()
        {
            try
            {
                //#region Load cboBankAccount According to User Setting
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultBank Account according to user root or other users
                int DefaultBankAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_BANK_ACCOUNT") : UserPreference.GetValue("DEFAULT_BANK_ACCOUNT", uid));
                string DefaultBankName = "";

                //Add Banks to comboBankAccount
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cmboBankAccount.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drBankLedgers["LedgerID"]) == DefaultBankAccNum)
                        DefaultBankName = drLedgerInfo["LedName"].ToString();
                }



                cmboBankAccount.DisplayMember = "value";//This value is  for showing at Load condition
                cmboBankAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                //cmboBankAccount.Text = DefaultBankName;
                cmboBankAccount.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
                backHeader.BackColor = ColorTranslator.FromHtml("#e6f7ff");
                backHeader.Border = DevAge.Drawing.RectangleBorder.RectangleBlack1Width;
                view.Background = backHeader;
                view.Font = new Font("Arial", 9, FontStyle.Regular);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }
        private void WriteHeader()
        {
            try
            {
                grdSalaryJournal.Rows.Clear();
                grdSalaryJournal.Redim(12, 4);
                grdSalaryJournal[0, 0] = new MyHeader("SN");
                grdSalaryJournal[0, 1] = new MyHeader("Title");
                grdSalaryJournal[0, 2] = new MyHeader("Debit");
                grdSalaryJournal[0, 3] = new MyHeader("Credit");

                grdSalaryJournal[0, 0].Column.Width = 30;
                grdSalaryJournal[0, 1].Column.Width = 230;
                grdSalaryJournal[0, 2].Column.Width = 100;
                grdSalaryJournal[0, 3].Column.Width = 100;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }


        private void LoadBanner()
        {
            DataRow drFound = drFoundTotal[0];
            lblMonthYear.Text = drFound["year"].ToString() + " " + drFound["monthName"].ToString();
            if (isEdit)
            {
                VoucherConfiguration m_VouConfig = new VoucherConfiguration();
                int serieid = 284;//Journal Main Series ID
                string NumberingType = m_VouConfig.GetVouNumberingType(serieid);
                if (NumberingType == "AUTOMATIC")
                {
                    object m_vounum = m_VouConfig.GenerateVouNumType(serieid);
                    if (m_vounum == null)
                    {
                        MessageBox.Show("Your voucher numbers are totally finished!");
                        return;
                    }
                    lblVoucherNo.Text = m_vounum.ToString();
                    //txtVchNo.Enabled = false;
                }
            }
            else
            {
                //load from the database
                lblVoucherNo.Text = drFound["Voucher_No"].ToString();
            }
        }
        private void LoadGrid()
        {
            try
            {
                DataRow drFound = drFoundTotal[0];
                decimal tSalary = Convert.ToDecimal(drFound["tSalary"].ToString());
                decimal tGrade = Convert.ToDecimal(drFound["tGrade"].ToString());
                tPF = Convert.ToDecimal(drFound["tPF"].ToString());
                decimal tBasicAllow = Convert.ToDecimal(drFound["tBasicAllow"].ToString());
                decimal tFoodAllow = Convert.ToDecimal(drFound["tFoodAllow"].ToString());
                decimal tGrossAmount = Convert.ToDecimal(drFound["tGrossAmount"].ToString());
                tPFDeduct = Convert.ToDecimal(drFound["tPFDeduct"].ToString());
                tTaxDeduct = Convert.ToDecimal(drFound["tTaxDeduct"].ToString());
                decimal tKKShort = Convert.ToDecimal(drFound["tKKShort"].ToString());
                decimal tKKLong = Convert.ToDecimal(drFound["tKKLong"].ToString());
                decimal tKKInterest = Convert.ToDecimal(drFound["tKKInterest"].ToString());
                tSLInstalment = Convert.ToDecimal(drFound["tSLInstalment"].ToString());
                tSLInterest = Convert.ToDecimal(drFound["tSLInterest"].ToString());
                tNLKosh = Convert.ToDecimal(drFound["tNLKosh"].ToString());
                decimal tTotalDeduct = Convert.ToDecimal(drFound["tTotalDeduct"].ToString());
                tNetSalary = Convert.ToDecimal(drFound["tNetSalary"].ToString());
                decimal tMiscAllow = Convert.ToDecimal(drFound["tMiscAllow"].ToString());
                //decimal tMiscDeduct = Convert.ToDecimal(drFound["tMiscDeduct"].ToString());
                


                int i = 1;
                if (tSalary != 0)
                {
                    totalSalary = tSalary + tGrade;
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Salary");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell(totalSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell();
                    i++;
                }

                if (tPF != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("PF Addition");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell(tPF.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell();
                    i++;
                }

                if (tBasicAllow != 0 || tFoodAllow != 0 || tMiscAllow != 0)
                {
                    totalAllowance = tBasicAllow + tFoodAllow + tMiscAllow;
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Allowance");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell(totalAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell();
                    i++;
                }

                if (tPFDeduct != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("PF Deduction");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(tPFDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                if (tTaxDeduct != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Income Tax");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(tTaxDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                totalKKKosh = tKKShort + tKKLong + tKKInterest;
                if (totalKKKosh != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Kalyankari Kosh");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(totalKKKosh.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                if (tSLInstalment != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Staff Account");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(tSLInstalment.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                if (tSLInterest != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Income from Interest(Staff Loan)");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(tSLInterest.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                if (tNLKosh != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Nagarik Lagani Kosh");
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(tNLKosh.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                if (tNetSalary != 0)
                {
                    grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                    grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell(cmboBankAccount.Text);
                    grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell();
                    grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(tNetSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    i++;
                }

                decimal totalDebit = totalSalary + tPF + totalAllowance;
                decimal totalCredit = tPF + tTaxDeduct + totalKKKosh + tSLInstalment + tSLInterest + tNLKosh + tNetSalary;
                grdSalaryJournal[i, 0] = new SourceGrid.Cells.Cell((i).ToString());
                grdSalaryJournal[i, 1] = new SourceGrid.Cells.Cell("Total");
                grdSalaryJournal[i, 2] = new SourceGrid.Cells.Cell(totalDebit.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                grdSalaryJournal[i, 3] = new SourceGrid.Cells.Cell(totalDebit.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));

                
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void cmboBankAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtFound = employee.GetSalaryMaster(m_paySlipID);
            drFoundTotal = dtFound.Select();
            WriteHeader();
            LoadBanner();
            LoadGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmboBankAccount.SelectedIndex == -1)
                {
                    Global.Msg("Please select a bank.");
                    return;
                }

                ListItem liLedgerID = new ListItem();
                liLedgerID = (ListItem)cmboBankAccount.SelectedItem;
                int bankId = Convert.ToInt32(liLedgerID.ID);
                double totalgrdAmount = 0;
                bool IsNegativeBank = false;

                #region BLOCK FOR CHECKING NEGATIVE BANK
                //execute following code when only if setting of Negative Bank is in Warn or Deny
                if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                {
                    //Incase of BankhReceipt and BankPayment master ledger is bydefault Bank soo we neednot to check ledger weather it falls uder Bank or not?? 
                    double mDbalBank = 0;
                    double mCbalBank = 0;
                    double totalDrBank, totalCrBank;
                    totalDrBank = totalCrBank = 0;
                    Transaction.GetLedgerBalance(null, null, bankId, ref mDbalBank, ref mCbalBank, null, 0);//we dont need to check according to project soo ProjecID is kept as zero

                    //Incase of Bank Payment,master ledger is bydefulat Bank...and here Bank is Payment soo it would be Credit because Bank is paying amount for other account
                    //Here Total Credit amount of master is calculated from Details section by adding all amount of all account in Details section
                    totalCrBank += (mCbalBank + totalgrdAmount);//this is the amount of self ledger(Bank Ledger) and all the amount of detail portion.Actually amount of detail section and master section should be equall soo whatever amount reamin in detail section will be same in master section                        
                    totalDrBank = mDbalBank;
                    if ((totalDrBank - totalCrBank) >= 0)
                        IsNegativeBank = false;
                    else
                        IsNegativeBank = true;

                    //If -ve cash not allowed in settings

                    #region NEGATIVE BANK SETTINNGS
                    switch (Global.Default_NegativeBank)
                    {
                        case NegativeBank.Allow:
                            //if (IsNegativeCash == true)
                            //Do nothing
                            break;
                        case NegativeBank.Warn:
                            if (IsNegativeBank == true)
                            {
                                if (MessageBox.Show("Your Bank Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                    return;
                            }
                            break;
                        case NegativeBank.Deny:
                            if (IsNegativeBank == true)
                            {
                                Global.MsgError("Your Bank amount is negative,you are not allowed to submit this voucher!!!");
                                return;
                            }
                            break;
                    }
                    #endregion

                }
                #endregion
                int[] accID = new int[] { 1 };
                employee.SalaryJournalEntry(m_paySlipID, DateTime.Today, totalSalary, totalAllowance, tPF, tPFDeduct, tTaxDeduct, totalKKKosh, tSLInstalment, tNLKosh, tSLInterest, tNetSalary, bankId, accID);

                //int isIt = employee.VoucherEntered(m_paySlipID,lblVoucherNo.Text,DateTime.Today);
                //if(isIt > 0)
                //{
                //    Global.Msg("Journal entry successfull");
                //    btnSave.Enabled = false;
                //    btnDelete.Enabled = true;
                //}
                //else
                //    Global.Msg("Unable to save journal entry state");
                
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Global.MsgQuest("Do you really want to delete this journal entry?") == DialogResult.Yes)
            {
                try
                {
                    employee.RemoveSalaryJournalEntry(m_paySlipID);
                    Global.Msg("Journal deleted successfully.");
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
            }
        }
    }
}
