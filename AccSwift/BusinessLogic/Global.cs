using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using DBLogic;
using System.Data;
using System.IO;
using DateManager;
using RegistryManager;
using System.Drawing;

namespace BusinessLogic
{
    public enum InventoryReportType
    {
        PartyWise,
        ProductWise,
        ProductTransactWise,
        PartyTransactWise

    }

    public enum InvenotryBookType
    {
        InventoyDayBook,
        PurchaseRegister,
        PurchaseReturnRegister,
        SalesRegister,
        SalesReturnRegister       
    }

    public enum StockStatusType
    {
        OpeningStock,
        ClosingStock,
        AtLeastOneResult,
        DiffInStock
    }

    public enum SearchIn { Account_Groups, Ledgers, Accounts_Under, Ledgers_Under, Ledger_Op_Bal,Disease_Group,Disease }; //Search In Combo box Items

    public enum SearchInServices { Service_Code, Service_Name };

    public enum SearchOpServices { Begin_With, Contains, Equall };

    public enum SearchOp { Begins_With, Contains, Equals, Greater_Or_Equals, Smaller_Or_Equals };

    public enum ProductSearchIn { Product_Groups, Product, Debtors_Name };

    public enum DiseasesSearchIn { Disease_Group, Disease, Debtors_Name }

    public enum SearcjOpDisease { Begins_With, Contains, Equals };
    public enum SearchOpProduct { Begins_With, Contains, Equals };

    //public enum AccountType { TAssets,VerticalAssets, TLiabilities,VerticalLiabilities, TIncome,VerticalIncome, TExpenditure,VerticalExpenditure };
    //public enum ReportAccountType { TAssets, VerticalAssets, TLiabilities, VerticalLiabilities, TIncome, VerticalIncome, TExpenditure, VerticalExpenditure };

    public enum EntryMode { NORMAL, NEW, EDIT }; //Used in entry forms. To find which mode(new, edit or normal) is the form currently running

    public enum Navigate { Prev, Next, First, Last , ID};
    public enum MBType
    {
        Information,
        Warning,
        Error
    }  
    //Available Languages
    public enum Lang
    {
        English,
        Nepali
    }
    public enum NegativeCash
    { 
        Allow,
        Warn,
        Deny
    }

    public enum BudgetLimit
    {
        Allow,
        Warn,
        Deny
    }
    public enum CreditLimit
    {      
        Null,
        Warn,
        Deny
    }

    public enum NegativeBank
    {
        Allow,
        Warn,
        Deny
    }
    //Check for the negative stock 
    public enum NegativeStock
    {
        Allow,
        Warn,
        Deny
    }
    public enum SalesReport
    {
        Patient
    }
        //Stores the type of the voucher
        public enum VoucherType
        {
            Journal=1,
            BankReceipt=2,
            BankPayment=3,
            CashReceipt=4,
            CashPayment=5,
            Contra=6
         }

        public enum AccountType
        {
            Assets = 1,
            Liabilities = 2,
            Income = 3,
            Expenditure = 4,
            Debtor=5,
            Creditor=6,
            ShareHolder = 7

        }
            



    public static class Global
    {
        public static string ConnectionString = "Data Source=" + RegManager.ServerName + ";Initial Catalog=" + RegManager.DataBase + "; uid = " + RegManager.DBUser + "; password = " + RegManager.DBPassword + "; Integrated Security=true; ";
        public static string ConnectionStringPrevious = "Data Source=" + RegManager.ServerName + ";Initial Catalog=" + Global.PreviousYearDbConnection + "; uid = " + RegManager.DBUser + "; password = " + RegManager.DBPassword + "; Integrated Security=true; ";

        public static SqlDb m_db = new SqlDb(ConnectionString); //Instantiate the database variable
        public static SqlDb m_dbPY = new SqlDb(ConnectionStringPrevious); //Instantiate the database variable


        #region passparameter after successful login
        ////Detect whether data is freezed
        public static bool DataFreeze = false;


        //Number of decimal places
        public static string acc;

        public static Boolean checkbox;

        public static string salesreturn;

        public static bool product;// check for addition of product

        public static bool disease;

        public static bool fclose;

        public static string MacAddess;

        public static string IpAddress;

        public static string ComputerName;

        public static string ConcatAllCompInfo;

        public static bool isPrintBill;

        public static string templateOption;

        public static bool isFillGrid;

        public static string CheckAcc;

        public static int isProfit;

        public static double plamount;

        public static bool CreateCompany = false;

        public static bool isOpeningTrial;
        //public static DataTable dtSlabInfo = Slabs.GetSlabInfo("VAT");
        //public static DataRow drSlabInfo = dtSlabInfo.Rows[0];
       
        public static int VATPayableID =412; //AccountGroup.GetLedgerIDFromLedgerNumber(10);  
        public static int VATReceivableID = 4698;   

        //only for sales
        public static int Tax1LedgerID = 314;
        public static int Tax2LedgerID = 315;
        public static int Tax3LedgerID = 316;

        public static int PurchaseTax1ID = 25717;
        public static int PurchaseTax2ID = 25718;
        public static int PurchaseTax3ID = 25719;

        public static string VATLedgerName="Vat A/c";
        public static string VATReceivableLdg = "Vat Receivable";
        public static string VATPayableLdg = "Vat Payable";
        public static string Tax1Name = "Tax1";
        public static string Tax2Name = "Tax2";
        public static string Tax3Name = "Tax3";


        public static int SalaryAcID = 3590;
        public static int BasicAllowanceID = 496;
        public static int FestivalAllowanceID = 6650;
        public static int PFPayableID = 493;
        public static int PensionFPayableID = 5644;
        public static int PFContributionID = 495;
        public static int PensionFContributionID = 5645;
        public static int TDSonSalaryID = 492;
        public static int KalyankariFundID = 3592;
        public static int StaffLoanAcID = 3591;
        //public static int StaffLoanInterestID = 3594;
        public static int NagarikLaganiFundID = 3593;
        public static int QuarterAccommodationID = 5646;
        public static int QuarterElectricityID = 5647;
        public static int StaffAdvanceID = 5643;
        public static int MiscDeductionID = 3596;

        public static string PurchaseTax1Name = "Purchase Tax1";
        public static string PurchaseTax2Name = "Purchase Tax2";
        public static string PurchaseTax3Name = "Purchase Tax3";
    
        public static Date.DateType Default_Date = Date.DateType.Nepali;
        public static string CurrentDateNep;
        public static DateTime CurrentDateEng;
        public static Date.DateFormat Default_Formate = Date.DateFormat.YYYY_MM_DD;
        public static Lang Default_Language = Lang.English;//LangMgr.LangToLangType(Settings.GetSettings("DEFAULT_LANGUAGE"));
        public static NegativeCash Default_NegativeCash = NegativeCash.Warn; //Settings.NegativeCashToNegativeCashType(Settings.GetSettings("DEFAULT_NEGATIVECASH"));
        public static BudgetLimit Default_BudgetLimit = BudgetLimit.Deny;
        public static NegativeBank Default_NegativeBank = NegativeBank.Warn ; //Settings.NegativeBankToNegativeBankType(Settings.GetSettings("DEFAULT_NEGATIVEBANK"));
        public static NegativeStock Default_NegativeStock = NegativeStock.Warn;
        public static string VAT_Settings = "1";//Settings.GetSettings("VAT");
        public static string Fiscal_English_Year = "13/14";
        public static string Fiscal_Nepali_Year = "070/071";
        ////public static DateTime Fiscal_Year_Start = Date.ToDotNet("2012/12/06");
        public static DateTime Fiscal_Year_Start = Convert.ToDateTime("2014-07-17");
        public static DateTime Fiscal_Year_End = Convert.ToDateTime("2015-07-1");

        public static int GlobalAccClassID;
        public static int GlobalAccessRoleID;
        public static int ParentAccClassID;
        //After Append

        public static string Company_Code = ""; //Stores the Company Code
        public static string Company_RegCode = ""; //Stores company code of the registry
        public static string PreviousYearDb = "";//Storing Previous Year Database
        public static string NextYearDb = "";//Storing NextYear Database
        public static string PreviousYearDbConnection = "";
        public static DateTime PYFromDate ;
        public static DateTime PYToDate;

        public static int DecimalPlaces = 3;
        public static bool Comma_Separated = true;// Settings.GetSettings("COMMA_SEPARATED");
        public static string Decimal_Format = "1";// Settings.GetSettings("DECIMAL_FORMAT");
        public static string Multi_Currency = "1";//Settings.GetSettings("MULTI_CURRENCY");
        public static string Default_Cash_Account = ""; //Settings.GetSettings("DEFAULT_CASH_ACCOUNT");
        public static string Default_Bank_Account = "";//Settings.GetSettings("DEFAULT_BANK_ACCOUNT");
        public static CreditLimit Credit_Limit = CreditLimit.Warn; //Settings.GetSettings("CREDIT_LIMIT");
        public static string Auto_BackUp = "0"; //Settings.GetSettings("AUTO_BACKUP");
        public static string Backup_Interval_Day = "2"; //Settings.GetSettings("BACKUP_INTERVAL_DAY");
        public static string Backup_Path = ""; //Settings.GetSettings("BACKUP_PATH");
        public static string Prepared_By = ""; //Settings.GetSettings("PREPARED_BY");
        public static string Checked_By = ""; //Settings.GetSettings("CHECKED_BY");
        public static string Approved_By = ""; //Settings.GetSettings("APPROVED_BY");

        //For Sales Invoice Report Preference
        public static string Default_Sales_Report_Type = "General";

        //For slabs Settings

        public static string  Default_Sales_Tax1On = "Nt Amt";
        public static string Default_Sales_Tax1Check = "1";
        public static string Default_Sales_Tax2On = "Nt Amt";
        public static string Default_Sales_Tax2Check = "1";
        public static string Default_Sales_Tax3On = "Nt Amt";
        public static string Default_Sales_Tax3Check = "1";
        public static string Default_Sales_Vat = "1";

        public static string Default_Purchase_Tax1On = "Nt Amt";
        public static string Default_Purchase_Tax1Check = "1";
        public static string Default_Purchase_Tax2On = "Nt Amt";
        public static string Default_Purchase_Tax2Check = "1";
        public static string Default_Purchase_Tax3On = "Nt Amt";
        public static string Default_Purchase_Tax3Check = "1";
        public static string Default_Purchase_Vat = "1";

        //Default value of Tax1,Tax2,Tax3 and Vat
        public static double Default_Tax1 = 0;
        public static double Default_Tax2 = 0;
        public static double Default_Tax3 = 0;
        public static double Default_Vat = 0;
        public static double Default_CustomDuty = 0;
        //Grid Color   
        public static Color Grid_Color = Color.FromArgb(240,240,240);

        //For Email Settings
        public static string mailserver = "";
        public static string serverport = "";
        public static string useremail = "";
        public static string password="";


       


        #endregion

        public static void Msg(string Message, MBType Type,string Title)
        {
            
            switch (Type)
            {
                case MBType.Error:
                    MessageBox.Show(Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case MBType.Warning:
                    MessageBox.Show(Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case MBType.Information:
                    MessageBox.Show(Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                //default:
                //    MessageBox.Show(Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Shows simply an information messagebox
        public static void Msg(string Message)
        {              
            MessageBox.Show(Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Shows simply an error messagebox
        public static void MsgError(string Message)
        {
            MessageBox.Show(Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult MsgQuest(string Message)
        {
            return MessageBox.Show(Message, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static object MakeNull(string toNull)
        {
            return (toNull.Trim().Length<=0?null:toNull);
        }




    }
}
