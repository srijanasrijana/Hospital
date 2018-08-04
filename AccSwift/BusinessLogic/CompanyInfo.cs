using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBLogic;
using DateManager;
using BusinessLogic;
using BusinessLogic.Accounting.Reports;

namespace BusinessLogic
{
    public  class CompanyDetails
    {
        private string _CompanyName;

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        private string _CompanyCode;

        public string CompanyCode
        {
            get { return _CompanyCode; }
            set { _CompanyCode = value; }
        }

        private string _Address1;

        public string Address1
        {
            get { return _Address1; }
            set { _Address1 = value; }
        }
        private string  _Address2;

        public string  Address2
        {
            get { return _Address2; }
            set { _Address2 = value; }
        }
        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _District;

        public string District
        {
            get { return _District; }
            set { _District = value; }
        }

        private string _Zone;

        public string Zone
        {
            get { return _Zone; }
            set { _Zone = value; }
        }

        private string _Telephone;

        public string Telephone
        {
            get { return _Telephone; }
            set { _Telephone = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Website;

        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }

        private string _POBox;

        public string POBox
        {
            get { return _POBox; }
            set { _POBox = value; }
        }
        private string _PAN;

        public string PAN
        {
            get { return _PAN; }
            set { _PAN = value; }
        }

        private byte[] _Logo;

        public byte[] Logo
        {
            get { return _Logo; }
            set { _Logo = value; }
        }

        private DateTime _FYFrom;
        public DateTime FYFrom
        {
            get { return _FYFrom; }
            set { _FYFrom = value; }
        }

        private DateTime _BookBeginFrom;
        public DateTime BookBeginFrom
        {
            get { return _BookBeginFrom; }
            set { _BookBeginFrom = value; }
        }

        private string _DBName;
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        private string _FiscalYear;
        public string FiscalYear
        {
            get { return _FiscalYear; }
            set { _FiscalYear = value; }
        }

        private string _ProductName;
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }


        private int _ClosingQunatity;
        public int ClosingQuanity
        {
            get { return _ClosingQunatity; }
            set { _ClosingQunatity = value; }
        }
        private int _OpeningQuantity;
        public int OpeningQuantity
        {
            get { return _OpeningQuantity; }
            set { _OpeningQuantity = value; }
        }

         private string _PartyProductName;
         public string PartyProductName
        {
            get { return _PartyProductName; }
            set { _PartyProductName = value; }
        }

          
        public DateTime _PrintDate;
        public DateTime PrintDate
        {
            get { return _PrintDate; }
            set { _PrintDate = value;  }
        }

#region  for user preferences 
        private string _CName;
        public string CName
        {
            get { return _CName; }
            set { _CName = value; }
        }

        private string _CAddress;
        public string CAddress
        {
            get { return _CAddress; }
            set { _CAddress = value; }
        }
        private string _CCity;
        public string CCity
        {
            get { return _CCity; }
            set { _CCity = value; }
        }
        private string _CPAN;
        public string CPAN
        {
            get { return _CPAN; }
            set { _CPAN = value; }
        }
        private string _CPhone;
        public string CPhone
        {
            get { return _CPhone; }
            set { _CPhone = value; }
        }
        private string _CSlogan;
        public string CSlogan
        {
            get { return _CSlogan; }
            set { _CSlogan = value; }
        }
 #endregion
        public static DataTable GetCompanyInfo()
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT * FROM System.tblCompanyInfo";
            dt = Global.m_db.SelectQry(strQuery, "tblCompanyInfo");
            return dt;
        }
    }

    public class CompanyInfo
    {
        public string Insert(CompanyDetails Details)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spCompanyInfoCreate");
                Global.m_db.AddParameter("@CompanyName", SqlDbType.NVarChar, 80, Details.CompanyName);
                Global.m_db.AddParameter("@CompanyCode", SqlDbType.NVarChar, 50, Details.CompanyCode);
                Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 80, Details.Address1);
                Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 80, Details.Address2);
                Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 80, Details.City);
                Global.m_db.AddParameter("@District", SqlDbType.NVarChar, 50, Details.District);
                Global.m_db.AddParameter("@Zone", SqlDbType.NVarChar, 50, Details.Zone);
                Global.m_db.AddParameter("@Telephone", SqlDbType.NVarChar, 50, Details.Telephone);
                Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 80, Details.Email);
                Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 80, Details.Website);
                Global.m_db.AddParameter("@POBox", SqlDbType.NVarChar, 50, Details.POBox);
                Global.m_db.AddParameter("@PAN", SqlDbType.NVarChar, 50, Details.PAN);
                Global.m_db.AddParameter("@FYFrom", SqlDbType.DateTime, 50, Details.FYFrom);
                Global.m_db.AddParameter("@BookBeginFrom", SqlDbType.DateTime, 50, Details.BookBeginFrom);
                Global.m_db.AddParameter("@FiscalYear", SqlDbType.NVarChar, 20, Details.FiscalYear);
                Global.m_db.AddParameter("@DBName", SqlDbType.NVarChar, 50, Details.DBName);
                if (Details.Logo != null)
                    Global.m_db.AddParameter("@Logo", SqlDbType.Image, Details.Logo);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 20, User.CurrUserID.ToString());

                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                if (paramReturn.Value.ToString() == "SUCCESS")
                    return "SUCCESS";
                else
                    return "FAILURE";


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Update(CompanyDetails Details)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spCompanyInfoUpdate");
                Global.m_db.AddParameter("@CompanyName", SqlDbType.NVarChar, 50, Details.CompanyName);
                Global.m_db.AddParameter("@CompanyCode", SqlDbType.NVarChar, 50, Global.Company_Code);
                //Global.m_db.AddParameter("@CompanyCode", SqlDbType.NVarChar, 50, Details.CompanyCode);
                Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 80, Details.Address1);
                Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 80, Details.Address2);
                Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, Details.City);
                Global.m_db.AddParameter("@District", SqlDbType.NVarChar, 50, Details.District);
                Global.m_db.AddParameter("@Zone", SqlDbType.NVarChar, 50, Details.Zone);
                Global.m_db.AddParameter("@Telephone", SqlDbType.NVarChar, 80, Details.Telephone);
                Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 80, Details.Email);
                Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 80, Details.Website);
                Global.m_db.AddParameter("@POBox", SqlDbType.NVarChar, 50, Details.POBox);
                Global.m_db.AddParameter("@PAN", SqlDbType.NVarChar, 50, Details.PAN);
                Global.m_db.AddParameter("@FYFrom", SqlDbType.DateTime,  Details.FYFrom);
                Global.m_db.AddParameter("@FiscalYear", SqlDbType.NVarChar, 20, Details.FiscalYear);
                Global.m_db.AddParameter("@BookBeginFrom", SqlDbType.DateTime, Details.BookBeginFrom);
                if (Details.Logo != null)
                    Global.m_db.AddParameter("@Logo", SqlDbType.Image, Details.Logo);
                //string s = User.GetCurrUser();
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 20, User.CurrentUserName);
                

                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                string check = paramReturn.Value.ToString();
                if (paramReturn.Value.ToString() == "SUCCESS")
                    return "SUCCESS";
                else
                    return "FAILURE";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //for to get info for user preferences with child class 
        public static CompanyDetails GetUPCompanyInfo()
        { 
        try
            {
            int uid = User.CurrUserID;
            
            CompanyDetails UPCompInfo = new CompanyDetails();
            DataTable dtComp = Global.m_db.SelectQry("SELECT * FROM System.tblUser_Preference where UserID='" + uid + "'", "user_Preference");
            if (dtComp.Rows.Count == 0)
                return null;
            DataRow drComp = dtComp.Rows[0];
            UPCompInfo.CName = drComp["Name"].ToString();
            UPCompInfo.CAddress = drComp["CompanyCode"].ToString();
            UPCompInfo.CCity = drComp["Address1"].ToString();
            UPCompInfo.CPAN = drComp["Address2"].ToString();
            UPCompInfo.CPhone = drComp["City"].ToString();
            UPCompInfo.CSlogan = drComp["District"].ToString();
            return UPCompInfo;
            }
        catch (Exception ex)
        {
            Global.Msg(ex.Message);
            return null;
        }
        }
        //for company set up from root
        public static CompanyDetails GetInfo()
        {
            try
            {
                CompanyDetails CompDetails = new CompanyDetails();
                DataTable dtComp = Global.m_db.SelectQry("SELECT * FROM System.tblCompanyInfo", "CompanyInfo");
                if (dtComp.Rows.Count == 0)
                    return null;
                DataRow drComp = dtComp.Rows[0];
                CompDetails.CompanyName = drComp["Name"].ToString();
                CompDetails.CompanyCode = drComp["CompanyCode"].ToString();
                CompDetails.Address1 = drComp["Address1"].ToString();
                CompDetails.Address2 = drComp["Address2"].ToString();
                CompDetails.City = drComp["City"].ToString();
                CompDetails.District = drComp["District"].ToString();
                CompDetails.Email = drComp["Email"].ToString();
                CompDetails.PAN = drComp["PAN"].ToString();
                CompDetails.POBox = drComp["POBox"].ToString();
                CompDetails.Telephone = drComp["Telephone"].ToString();
                CompDetails.Website = drComp["Website"].ToString();
                CompDetails.Zone = drComp["Zone"].ToString();
                CompDetails.FYFrom = Convert.ToDateTime(drComp["FYFrom"]);
                CompDetails.BookBeginFrom =Convert.ToDateTime(drComp["BookBeginFrom"]);
                CompDetails.FiscalYear = drComp["FiscalYear"].ToString();
                if (drComp["Logo"].ToString() == "")
                    CompDetails.Logo = null;
                else
                    CompDetails.Logo = (byte[])drComp["Logo"];

                return CompDetails;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
                return null;
            }
        }

        public static CompanyDetails GetProductName()
        {

            try
            {
                CompanyDetails CompDetails = new CompanyDetails();
                DataTable dtComp = Global.m_db.SelectQry("SELECT EngName FROM Inv.tblProduct", "ProductInfo");
                
                DataRow drComp = dtComp.Rows[0];
                CompDetails.ProductName = drComp["EngName"].ToString();

                return CompDetails;
                
            }
            catch (Exception ex)
            {
                
                Global.Msg(ex.Message);
                return null;
               
            }
            
        }

        public static string GetFiscalYearInfo()
        {   
            try
            {
                //Check Fiscal year(By default in English)
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();

                //IF there are no companies created, simply return
                if (CompDetails == null)
                {
                    return string.Empty;
                }

                //get first month from start fiscal date 
                DateTime start=new DateTime();
                if (CompDetails.FYFrom != null)
                {
                    start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year
                }               

                //Convert Fiscal year to nepali
                int refYear = start.Year;
                int FYMonth = start.Month;
                int refDay = start.Day;

                //If DateType is Nepali, load Nepali month
                if (Date.DefaultDate == Date.DateType.Nepali)
                    Date.EngToNep(start, ref refYear, ref FYMonth, ref refDay);

                //Get the nepali fiscal year starting month
                int MonthCounter = FYMonth;

                if (MonthCounter == 1)
                    MonthCounter = 12;
                else
                    MonthCounter--;
                        refYear++; // 1 year FY

                    //If it was Nepali, set back to DateTime type
                    DateTime FinalDate;
                    if (Date.DefaultDate == Date.DateType.Nepali)
                    {
                        //Get Last Day
                        DataTable LastDay = Date.LastDayofMonthNep(refYear,MonthCounter);
                        FinalDate = Date.NepToEng(refYear, MonthCounter, Convert.ToInt16(LastDay.Rows[0][0]));
                    }
                    else
                        FinalDate = new DateTime(refYear,MonthCounter, DateTime.DaysInMonth(refYear,MonthCounter));
                    return Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
                return string.Empty;
            }   
  
        }
        //"insert into Acc.tblOpeningBalance(LedgerID,AccClassID,OpenBal,OpenBalDrCr,OpenBalDate,OpenBalCCYID) values('" + dr["LedgerID"].ToString() + "','" + dr["AccClassID"].ToString() + "','" + dr["Opening Balance"].ToString() + "','" + dr["DrCr"].ToString() + "','"+Created_Date+"','"+1+"')");
        public static void InsertOpeningBalance(DataTable dtopening)
        {
            for(int i=0;i<dtopening.Rows.Count;i++)
            {
                DataRow dr=dtopening.Rows[i];
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spInsertOpeningBal");
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int,dr["LedgerID"].ToString());
                Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, dr["AccClassID"].ToString());
                Global.m_db.AddParameter("@OpenBal", SqlDbType.Float, dr["Opening Balance"].ToString());
                Global.m_db.AddParameter("@OpenBalDrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
               // System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
              
            }
            }
        }
        public static void UpdateOpeningQty(DataTable dtopening)
        {
            for (int i = 0; i < dtopening.Rows.Count; i++)
            {
                DataRow dr = dtopening.Rows[i];
                try
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spUpdateOpeningQuantity");
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, dr["ProductID"].ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, dr["AccClassID"].ToString());
                    Global.m_db.AddParameter("@OpeningQty", SqlDbType.Int, dr["OpeningPurchaseQty"].ToString());
                    Global.m_db.AddParameter("@OpeningPurchaseRate", SqlDbType.Float, dr["OpeningPurchaseRate"].ToString());
                    Global.m_db.AddParameter("@OpeningSalesRate", SqlDbType.Float, dr["OpeningSalesRate"].ToString());
                    // System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);

                }
            }
        }
         public static void InsertOpeningQty(DataTable dtopening)
        {
            for (int i = 0; i < dtopening.Rows.Count; i++)
            {
                DataRow dr = dtopening.Rows[i];
                try
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("System.spInsertOpeningQuantity");
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, Convert.ToInt32(dr["ProductID"].ToString()));
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, Convert.ToInt32(dr["AccClassID"].ToString()));
                    Global.m_db.AddParameter("@OpenPurchaseQty", SqlDbType.Decimal, Convert.ToDecimal(dr["OpeningPurchaseQty"].ToString()));
                    Global.m_db.AddParameter("@OpenPurchaseRate", SqlDbType.Decimal,Convert.ToDecimal(dr["OpeningPurchaseRate"].ToString()));
                    Global.m_db.AddParameter("@OpenSalesRate", SqlDbType.Decimal,Convert.ToDecimal(dr["OpeningSalesRate"].ToString()));
                    // System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);

                }
            }
        }

      
   
    }
}
