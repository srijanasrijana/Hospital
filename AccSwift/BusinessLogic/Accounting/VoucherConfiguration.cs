using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic
{
    public class VoucherConfiguration
    {
        public static DataTable GetSeriesInfo(string VoucherType)
        {
            if (VoucherType == null)
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblSeries", "table");
            }
            else
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblSeries WHERE Voucher_Type = '" + VoucherType + "'", "SeriesInfo");
            }
        }

        public static DataTable GetSeriesInfo(int SeriesID)
        {
            return Global.m_db.SelectQry("SELECT * FROM System.tblSeries WHERE SeriesID = '" + SeriesID + "'", "SeriesInfo");       
        }
    
        // code for inserting the data in tblVouNumConfig
        public void Insert(string NumeringType, string DuplicateVouNum, string BlankVouNum, string StartingNo, bool SpecifyEndingNo, string EndingNo, string WarningVouNo, string WarningMsg, string RenumberingFrq, bool FixLengthNumPart, string TotalLengthNumPart, string PaddingChar,string SeriesName,string VoucherType,DataTable VoucherFormat,OptionalField of,bool HideVouNum)
        {
            try
            {
                #region BLOCK FOR RETURNING THE CORRESPONDING EXCEPTION FOR VALIDATION WHILE INSERTING JOURNAL VOUCHER CONFIGURATION

                if (SeriesName == "")
                {
                    throw new Exception("BLANK_SERIES");             
                    return;
                }
                if (StartingNo == "")
                {
                    throw new Exception("BLANK_STARTING_NUM");
                    return;
                }
                if (SpecifyEndingNo == true && EndingNo=="")
                {
                    throw new Exception("SPECIFY_ENDING_NUM");
                    return;
                }
                if (SpecifyEndingNo == true && WarningVouNo == "")
                {
                    throw new Exception("WARNING_VOU_NUM");
                    return;                
                }
                if (SpecifyEndingNo == true && WarningMsg == "")
                {
                    throw new Exception("WARNING_MESSAGE");
                    return;                               
                }
              
                if (FixLengthNumPart == true && TotalLengthNumPart == "")
                {
                    throw new Exception("FIX_LENGTH_NUMERIC_PART");
                    return;                
                }
                if (FixLengthNumPart == true && PaddingChar == "")
                {
                    throw new Exception("BLANK_PADDING_CHAR");
                    return;
                }
                
                //If NumberingType is Manual then validate for the Blank Voucher Number and Duplicate Voucher Number
                if (NumeringType == "Manual")
                {
                    if (DuplicateVouNum == "")
                    {
                        throw new Exception("DUPLICATE_VOU_NUM");
                        return;                  
                    }
                }
                #endregion

                //For inserting the Series information
                #region BLOCK FOR INSERTING IN THE SERIES INFORMATION
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spSeriesCreate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, SeriesName);
                switch (VoucherType)
                {
                    case "SALES":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);//Set same for both for time being
                        break;
                    case "PURCH":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "SLS_RTN":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "PURCH_RTN":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "CASH_PMNT":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "CASH_RCPT":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "BANK_PMNT":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "BANK_RCPT":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;                 
                    case "JNL":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "CNTR":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "DR_NOT":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "CR_NOT":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "BRECON":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                    case "STOCK_TRANS":
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 30, VoucherType);
                        break;
                }
                Global.m_db.AddParameter("@AutoNumber", SqlDbType.Int,0);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.Int);
                Global.m_db.ProcessParameter();

                if (Convert.ToInt32(objReturn.Value)== -1)
                    throw new Exception("COULD_NT_INSERT_SERIES");

                int ReturnID = Convert.ToInt32(objReturn.Value);
                #endregion

                //Inserting the data on the Procudure spCreateVouNumConfig
                #region BLOCK FOR INSERTING THE VOUCHER NUMBER CONFIGURATION
                //BLOCK FOR VOUCHER NUMBERING CONFIGURATION

                //Block for Inserting the values of Numbering Configuration

                string duplicteVouNum = "";

                if (DuplicateVouNum == "Warning Only")
                {
                    duplicteVouNum = "WARNING_ONLY";
                }
                if (DuplicateVouNum == "Dont Allow")
                {
                    duplicteVouNum = "DONT_ALLOW";
                }
                if (DuplicateVouNum == "No Action")
                {
                    duplicteVouNum = "NO_ACTION";
                }
                // Using Switch for changing the User values in Database type by making Capital letter and replacing space by Undrescore
                string blankVouNum = "";
                switch (BlankVouNum)
                {
                    case "Warning Only":
                        blankVouNum = "WARNING_ONLY";
                        break;
                    case "Dont Allow":
                        blankVouNum = "DONT_ALLOW";
                        break;
                    case "No action":
                        blankVouNum = "NO_ACTION";
                        break;
                    default:
                        break;
                }             
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spCreateVouNumConfig");
                Global.m_db.AddParameter("@NumberingType", SqlDbType.NVarChar, 50, NumeringType);

                Global.m_db.AddParameter("@DuplicateVouNum", SqlDbType.NVarChar, 50, duplicteVouNum);
                Global.m_db.AddParameter("@BlankVouNum", SqlDbType.NVarChar, 50, blankVouNum);
                if (StartingNo == "")
                {
                    StartingNo = "0";
                }
                Global.m_db.AddParameter("@StartingNo", SqlDbType.Int, StartingNo);
                Global.m_db.AddParameter("@SpecifyEndNo", SqlDbType.Bit, SpecifyEndingNo);
                if (EndingNo == "")
                {
                    EndingNo = "0";
                }
                Global.m_db.AddParameter("@EndNo", SqlDbType.Int, EndingNo);
                if (WarningVouNo == "")
                {
                    WarningVouNo = "0";
                }
                Global.m_db.AddParameter("@WarningVouLeft", SqlDbType.Int, WarningVouNo);
                Global.m_db.AddParameter("@WarningMsg", SqlDbType.NVarChar, 50, WarningMsg);
                Global.m_db.AddParameter("@RenumberingFrq", SqlDbType.NVarChar, 50, RenumberingFrq);
                Global.m_db.AddParameter("@NumericPart", SqlDbType.Bit, FixLengthNumPart);
                if (TotalLengthNumPart == "")
                {
                    TotalLengthNumPart = "0";
                }
                Global.m_db.AddParameter("@TotalLengthNumPart", SqlDbType.Int, TotalLengthNumPart);
                Global.m_db.AddParameter("@PaddingChar", SqlDbType.NVarChar, 50, PaddingChar);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, ReturnID.ToString());
                Global.m_db.AddParameter("@HideVouNum", SqlDbType.Bit, HideVouNum);

                Global.m_db.ProcessParameter();
                    #endregion

                //Block for inserting the values of Voucher Format
                #region BLOCK FOR INSERTING THE VOUCHER NUMBERING FORMAT
                for (int i = 0; i < VoucherFormat.Rows.Count; i++)
                {
                    DataRow dr = VoucherFormat.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("System.spVoucherFormatCreate");
                    Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Type", SqlDbType.NVarChar, 30,dr["Type"].ToString());
                    Global.m_db.AddParameter("@Param", SqlDbType.NVarChar,50,dr["Param"].ToString());
                    Global.m_db.ProcessParameter();
                }

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spOptionalFieldCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, ReturnID.ToString());
                Global.m_db.AddParameter("@NumberOfFields", SqlDbType.Int, of.NoOfFields);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                Global.m_db.AddParameter("@IsFirstRequired", SqlDbType.Bit, of.IsField1Required);
                Global.m_db.AddParameter("@IsSecondRequired", SqlDbType.Bit, of.IsField2Required);
                Global.m_db.AddParameter("@IsThirdRequired", SqlDbType.Bit, of.IsField3Required);
                Global.m_db.AddParameter("@IsFourthRequired", SqlDbType.Bit, of.IsField4Required);
                Global.m_db.AddParameter("@IsFifthRequired", SqlDbType.Bit, of.IsField5Required);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 50, VoucherType);
                Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
                #endregion
        }

        public void Modify(int SeriesID, string NumeringType, string DuplicateVouNum, string BlankVouNum, string StartingNo, bool SpecifyEndingNo, string EndingNo, string WarningVouNo, string WarningMsg, string RenumberingFrq, bool FixLengthNumPart, string TotalLengthNumPart, string PaddingChar, string SeriesName, string VoucherType, DataTable VoucherFormat, OptionalField of, bool HideVouNum)
        {
            try
            {
                //BLOCK FOR MODIFYING SERIES NAME
                string LangField = "ENGLISH";
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spSeriesNameModify");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, SeriesName);
                if (LangMgr.DefaultLanguage == Lang.English)
                    LangField = "ENGLISH";
                else if (LangMgr.DefaultLanguage == Lang.Nepali)
                    LangField = "NEPALI";

                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 50, LangField);
                
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                // BLOCK OF EDITING VOUCHER FORMAT

                ////First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM System.tblVoucherFormat WHERE SeriesID='" + SeriesID.ToString() + "'");
                //Now insert the editable values in System.tblVoucherFormat

                for (int i = 0; i < VoucherFormat.Rows.Count; i++)
                {
                    DataRow dr = VoucherFormat.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("System.spVoucherFormatCreate");
                    Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                    Global.m_db.AddParameter("@Type", SqlDbType.NVarChar, 30, dr["Type"].ToString());
                    Global.m_db.AddParameter("@Param", SqlDbType.NVarChar, 50, dr["Param"].ToString());
                    Global.m_db.ProcessParameter();
                }

                //BLOCK FOR MODIFYING VOUCHER NUMBER CONFIGURATION
                string duplicteVouNum = "";
                if (DuplicateVouNum == "Warning Only")
                {
                    duplicteVouNum = "WARNING_ONLY";
                }
                if (DuplicateVouNum == "Dont Allow")
                {
                    duplicteVouNum = "DONT_ALLOW";
                }
                if (DuplicateVouNum == "No Action")
                {
                    duplicteVouNum = "NO_ACTION";
                }

                // Using Switch for changing the User values in Database type by making Capital letter and replacing space by Undrescore
                string blankVouNum = "";
                switch (BlankVouNum)
                {
                    case "Warning Only":
                        blankVouNum = "WARNING_ONLY";
                        break;
                    case "Dont Allow":
                        blankVouNum = "DONT_ALLOW";
                        break;
                    case "No action":
                        blankVouNum = "NO_ACTION";
                        break;
                    default:
                        break;
                }
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spVouNumConfigModify");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@NumberingType", SqlDbType.NVarChar, 50, NumeringType);
                Global.m_db.AddParameter("@DuplicateVouNum", SqlDbType.NVarChar, 50, duplicteVouNum);
                Global.m_db.AddParameter("@BlankVouNum", SqlDbType.NVarChar, 50, blankVouNum);
                if (StartingNo == "")
                {
                    StartingNo = "0";
                }
                Global.m_db.AddParameter("@StartingNo", SqlDbType.Int, StartingNo);
                Global.m_db.AddParameter("@SpecifyEndNo", SqlDbType.Bit, SpecifyEndingNo);
                if (EndingNo == "")
                {
                    EndingNo = "0";
                }
                Global.m_db.AddParameter("@EndNo", SqlDbType.Int, EndingNo);
                if (WarningVouNo == "")
                {
                    WarningVouNo = "0";
                }
                Global.m_db.AddParameter("@WarningVouLeft", SqlDbType.Int, WarningVouNo);
                Global.m_db.AddParameter("@WarningMsg", SqlDbType.NVarChar, 50, WarningMsg);
                Global.m_db.AddParameter("@RenumberingFrq", SqlDbType.NVarChar, 50, RenumberingFrq);
                Global.m_db.AddParameter("@NumericPart", SqlDbType.Bit, FixLengthNumPart);
                if (TotalLengthNumPart == "")
                {
                    TotalLengthNumPart = "0";
                }
                Global.m_db.AddParameter("@TotalLengthNumPart", SqlDbType.Int, TotalLengthNumPart);
                Global.m_db.AddParameter("@PaddingChar", SqlDbType.NVarChar, 50, PaddingChar);
                Global.m_db.AddParameter("@HideVouNum", SqlDbType.Bit, HideVouNum);
                Global.m_db.ProcessParameter();


                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spOptionalFieldModify");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@NumberOfFields", SqlDbType.Int, of.NoOfFields);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                Global.m_db.AddParameter("@IsFirstRequired", SqlDbType.Bit,of.IsField1Required);
                Global.m_db.AddParameter("@IsSecondRequired", SqlDbType.Bit, of.IsField2Required);
                Global.m_db.AddParameter("@IsThirdRequired", SqlDbType.Bit, of.IsField3Required);
                Global.m_db.AddParameter("@IsFourthRequired", SqlDbType.Bit, of.IsField4Required);
                Global.m_db.AddParameter("@IsFifthRequired", SqlDbType.Bit, of.IsField5Required);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 50, VoucherType);
                Global.m_db.ProcessParameter();

                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;

                #region SQLException
                //switch (ex.Number)
                //{
                //    case 4060: // Invalid Database 
                //        Global.Msg("Invalid Database", MBType.Error, "Error");
                //        break;

                //    case 18456: // Login Failed 
                //        Global.Msg("Login Failed!", MBType.Error, "Error");
                //        break;

                //    case 547: // ForeignKey Violation , Check Constraint
                //        Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                //        break;

                //    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                //        Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                //        break;

                //    case 2601: // Unique Index/Constriant Violation 
                //        Global.Msg("Unique index violation!", MBType.Warning, "Error");
                //        break;

                //    case 5000: //Trigger violation
                //        Global.Msg("Trigger violation!", MBType.Warning, "Error");
                //        break;

                //    default:
                //        break;
                //}
                #endregion
            }
        }

        public static void Delete(int SeriesID)
        {
            Global.m_db.SelectQry("DELETE FROM System.tblSeries WHERE SeriesID ='"+SeriesID+"'", "System.tblSeries");
        }

        public DataTable GetVouNumConfiguration(int SeriesID)
        {
            string numberingtype = "";
            DataTable dt = Global.m_db.SelectQry("SELECT * FROM System.tblVouNumConfig WHERE SeriesID ='" + SeriesID + "' ", "vouNum");
            return dt;
        }

        public DataTable GetOptionalField(int SeriesID)
        {

            DataTable dt = Global.m_db.SelectQry("SELECT * FROM System.tblOptionalFields WHERE SeriesID ='" + SeriesID + "' ", "OptionalField");
            return dt;
        }

        public string GetVouNumberingType(int SeriesID)
        {
            string numberingtype = "";
            DataTable dt = Global.m_db.SelectQry("SELECT *FROM System.tblVouNumConfig WHERE SeriesID ='" + SeriesID + "' ", "vouNum");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                // using switch concept here
                switch (dr["NumberingType"].ToString())
                {
                    case "Automatic":
                        return "AUTOMATIC";
                        break;
                    case "Manual":
                        return "MANUAL";
                        break;
                    case "No Required":
                        return "NOT_REQUIRED";
                        break;
                    case "default":
                        return null;
                        break;
                }
            }
            return numberingtype;
        }

        public bool GetIsVouHideType(int SeriesID)
        {
            DataTable dt = Global.m_db.SelectQry("SELECT *FROM System.tblVouNumConfig WHERE SeriesID ='" + SeriesID + "' ", "vouNum");
            if (dt.Rows[0]["HideVouNum"].ToString() == "True")
                return true;
            else
                return false;
        }

        public string GenerateVouNumType(int SeriesID)
        {
            string VoucherFormat = "";
            try
            {                
            string sql = "SELECT * FROM System.tblVoucherFormat WHERE SeriesID = '"+SeriesID+"' ";
            DataTable dt = Global.m_db.SelectQry(sql, "vouNum");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //Global.Msg(dr["Type"].ToString()); 

                    if (dr["Type"].ToString() == "Symbol")
                    {
                        VoucherFormat += dr["Parameter"].ToString();
                    }
                    if (dr["Type"].ToString() == "Date")
                    {
                        if (dr["Parameter"].ToString() == "NEPALI_FISCAL_YEAR")
                        {                         
                            VoucherFormat += Global.Fiscal_Nepali_Year;
                        }
                        if (dr["Parameter"].ToString() == "ENGLISH_FISCAL_YEAR")
                        {
                            VoucherFormat += Global.Fiscal_English_Year;
                        }
                        if (dr["Parameter"].ToString() == "NEPALI_YEAR")
                        {
                            VoucherFormat += Global.Fiscal_Nepali_Year;
                            //DateTime time = DateTime.Parse(Date.GetServerDate().ToString());
                            //int retYear = 0;
                            //int retMonth = 0;
                            //int retDay = 0;
                            //string NepDate = Date.EngToNep(time, ref retYear, ref retMonth, ref retDay);
                            //string[] split = NepDate.Split('/');
                            //foreach (string s in split)
                            //{
                            //    if (s.Length==4)
                            //        VoucherFormat += s.ToString();
                            //}         
                        }
                        if (dr["Parameter"].ToString() == "ENGLISH_YEAR")
                        {
                            DateTime time = DateTime.Parse(Date.GetServerDate().ToString());
                            VoucherFormat += time.Year.ToString();                     
                        }
                        //VoucherFormat += dr["Parameter"].ToString();
                    }

                    if (dr["Type"].ToString() == "(AutoNumber)")
                    {
                        int autonum = 0;
                        string sqlTest;
                        DataTable dtTest = new DataTable();
                        string sql1 = "SELECT * FROM System.tblVouNumConfig WHERE SeriesID ='" + SeriesID + "'";
                        DataTable dt1 = Global.m_db.SelectQry(sql1, "vouNum");

                        for (int i1 = 0; i1 < dt1.Rows.Count; i1++)
                        {
                            DataRow dr1 = dt1.Rows[i1];
                            if (dr1["NumberingType"].ToString() == "Automatic")
                            {
                                string sql2 = "SELECT AutoNumber,Voucher_Type FROM System.tblSeries WHERE SeriesID ='" + SeriesID + "'";
                                object m_Number = Global.m_db.GetScalarValue(sql2);
                                DataTable dtVouType = Global.m_db.SelectQry(sql2, "Vtype");
                                DataRow drVouType = dtVouType.Rows[0];
                                string VoucherType = drVouType["Voucher_Type"].ToString();
                                
                                ////Checking whether the data are available or not in tblNumbering 
                                for (int i3 = 1; i3 <= Convert.ToInt32(m_Number)+1; i3++)
                                {
                                    string txtvno = i3.ToString().PadLeft(Convert.ToInt32(dr1["TotalLengthNumPart"]), Convert.ToChar(dr1["PaddingChar"]));
                                    //if txtvno is in other format then we have to add it
                                    switch (VoucherType)
                                    {
                                        case "SALES":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblSalesInvoiceMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "PURCH":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblPurchaseInvoiceMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "SLS_RTN":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblSalesReturnMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "PURCH_RTN":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblPurchaseReturnMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CASH_PMNT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblCashPaymentMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CASH_RCPT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblCashReceiptMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "BANK_PMNT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblBankPaymentMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "BANK_RCPT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblBankReceiptMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "JNL":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblJournalMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CNTR":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblContraMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "DR_NOT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblDebitNoteMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CR_NOT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblCreditNoteMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "BRECON":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblBankReconciliationMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;

                                        case "STOCK_TRANS":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblStockTransferMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;

                                        case "DAMAGE":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblDamageProductInvoiceMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CHEQUERCPT":
                                            sqlTest = "SELECT VoucherNo FROM Acc.tblChequeReceiptMaster WHERE VoucherNo LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                    }

                                    //sqlTest = "SELECT Voucher_No FROM Acc.tblJournalMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                    //dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                    //DataRow dr3 = dt3.Rows[i3];
                                   if (Convert.ToInt32(m_Number) == 1)
                                   {
                                       autonum = 2;
                                   }
                                   if (VoucherType == "BANK_RCPT")
                                   {
                                       sqlTest = "SELECT VoucherNo FROM Acc.tblChequeReceiptMaster WHERE VoucherNo LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                       DataTable dtTest1 = Global.m_db.SelectQry(sqlTest, "vouNum");

                                       if (dtTest.Rows.Count == 0 && dtTest1.Rows.Count==0)
                                       {
                                           if (m_Number != null)
                                           {
                                               int count = Convert.ToInt32(i3);
                                               if (Convert.ToInt32(dr1["StartingNo"]) > count)
                                               {
                                                   autonum = Convert.ToInt32(dr1["StartingNo"]);
                                               }
                                               else
                                               {
                                                   autonum = count;
                                               }
                                               break;
                                           }
                                       }
                                   }
                                   else
                                   {
                                       if (dtTest.Rows.Count == 0)
                                       {
                                           if (m_Number != null)
                                           {
                                               int count = Convert.ToInt32(i3);
                                               if (Convert.ToInt32(dr1["StartingNo"]) > count)
                                               {
                                                   autonum = Convert.ToInt32(dr1["StartingNo"]);
                                               }
                                               else
                                               {
                                                   autonum = count;
                                               }
                                               break;
                                           }
                                       }
                                   }
                                }
                                //VoucherFormat += autonum.ToString();

                                //Check if it meets the ending number
                                if (dr1["SpecifyEndNo"].ToString() == "True")
                                {
                                    // checking Ending no to autonum
                                    if (Convert.ToInt32(dr1["EndNo"]) >= autonum)
                                    {
                                        //MessageBox.Show("Ya EndNO is greater than count");
                                        if (autonum >= Convert.ToInt32(dr1["WarningVouLeft"]))
                                        {

                                            MessageBox.Show(dr1["WarningMsg"].ToString());
                                        }
                                    }
                                    else //The number is greater than available ending number from setting
                                    {
                                        return null;
                                    }

                                   /// from here i cut the code
                                }
                                else
                                {
                                    //MessageBox.Show(" you havnt choosed SpecifyEndNo");

                                }
                                // This is code for incrementing the value of Number column in tblNumbering 

                                //this code is temporarily commented to check the voucher number of ram's type
                                
                                int incNumber = Convert.ToInt32(m_Number)+1;

                                string sql3 = "UPDATE System.tblSeries set AutoNumber='" +incNumber + "'WHERE SeriesID = '" + SeriesID + "'";
                                Global.m_db.InsertUpdateQry(sql3);

                                // checking for the Renumbering Frequency****************

                                if (dr1["RenumberingFrq"].ToString() == "Yearly")
                                {
                                    //MessageBox.Show("Yearly");

                                }

                                else if (dr1["RenumberingFrq"].ToString() == "Daily")
                                {
                                    //MessageBox.Show("Daily");
                                }
                                else
                                {
                                    //MessageBox.Show("you havnt choosed the RenumberingFrq");

                                }
                                // for Fix Length of Numeric Part........................

                                if (dr1["NumericPart"].ToString() == "True")
                                {
                                    //VoucherFormat += autonum.ToString().PadLeft(Convert.ToInt32(dr1["TotalLengthNumPart"]), Convert.ToChar(dr1["PaddingChar"]));
                                    VoucherFormat += incNumber.ToString().PadLeft(Convert.ToInt32(dr1["TotalLengthNumPart"]), Convert.ToChar(dr1["PaddingChar"]));
                                }
                                else
                                {
                                    //MessageBox.Show("you are not allowed for choosing Fix Length of Numeric Part");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return VoucherFormat;
        }

        /// <summary>
        /// Gets new voucher number but does not update the increased voucher number in tblSeries
        /// </summary>
        /// <param name="SeriesID"></param>
        /// <returns></returns>
        public string GenerateVouNumTypeNoUpdate(int SeriesID, out int IncreasedNumber)
        {
            string VoucherFormat = "";
            int incNumber = 0;
            try
            {
                string sql = "SELECT * FROM System.tblVoucherFormat WHERE SeriesID = '" + SeriesID + "' ";
                DataTable dt = Global.m_db.SelectQry(sql, "vouNum");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //Global.Msg(dr["Type"].ToString()); 

                    if (dr["Type"].ToString() == "Symbol")
                    {
                        VoucherFormat += dr["Parameter"].ToString();
                    }
                    if (dr["Type"].ToString() == "Date")
                    {
                        if (dr["Parameter"].ToString() == "NEPALI_FISCAL_YEAR")
                        {
                            VoucherFormat += Global.Fiscal_Nepali_Year;
                        }
                        if (dr["Parameter"].ToString() == "ENGLISH_FISCAL_YEAR")
                        {
                            VoucherFormat += Global.Fiscal_English_Year;
                        }
                        if (dr["Parameter"].ToString() == "NEPALI_YEAR")
                        {
                            VoucherFormat += Global.Fiscal_Nepali_Year;
                            //DateTime time = DateTime.Parse(Date.GetServerDate().ToString());
                            //int retYear = 0;
                            //int retMonth = 0;
                            //int retDay = 0;
                            //string NepDate = Date.EngToNep(time, ref retYear, ref retMonth, ref retDay);
                            //string[] split = NepDate.Split('/');
                            //foreach (string s in split)
                            //{
                            //    if (s.Length==4)
                            //        VoucherFormat += s.ToString();
                            //}         
                        }
                        if (dr["Parameter"].ToString() == "ENGLISH_YEAR")
                        {
                            DateTime time = DateTime.Parse(Date.GetServerDate().ToString());
                            VoucherFormat += time.Year.ToString();
                        }
                        //VoucherFormat += dr["Parameter"].ToString();
                    }

                    if (dr["Type"].ToString() == "(AutoNumber)")
                    {
                        int autonum = 0;
                        string sqlTest;
                        DataTable dtTest = new DataTable();
                        string sql1 = "SELECT * FROM System.tblVouNumConfig WHERE SeriesID ='" + SeriesID + "'";
                        DataTable dt1 = Global.m_db.SelectQry(sql1, "vouNum");

                        for (int i1 = 0; i1 < dt1.Rows.Count; i1++)
                        {
                            DataRow dr1 = dt1.Rows[i1];
                            if (dr1["NumberingType"].ToString() == "Automatic")
                            {
                                string sql2 = "SELECT AutoNumber,Voucher_Type FROM System.tblSeries WHERE SeriesID ='" + SeriesID + "'";
                                object m_Number = Global.m_db.GetScalarValue(sql2);
                                DataTable dtVouType = Global.m_db.SelectQry(sql2, "Vtype");
                                DataRow drVouType = dtVouType.Rows[0];
                                string VoucherType = drVouType["Voucher_Type"].ToString();

                                ////Checking whether the data are available or not in tblNumbering 
                                for (int i3 = 1; i3 <= Convert.ToInt32(m_Number) + 1; i3++)
                                {
                                    string txtvno = i3.ToString().PadLeft(Convert.ToInt32(dr1["TotalLengthNumPart"]), Convert.ToChar(dr1["PaddingChar"]));
                                    //if txtvno is in other format then we have to add it
                                    switch (VoucherType)
                                    {
                                        case "SALES":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblSalesInvoiceMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "PURCH":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblPurchaseInvoiceMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "SLS_RTN":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblSalesReturnMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "PURCH_RTN":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblPurchaseReturnMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CASH_PMNT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblCashPaymentMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CASH_RCPT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblCashReceiptMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "BANK_PMNT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblBankPaymentMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "BANK_RCPT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblBankReceiptMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "JNL":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblJournalMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CNTR":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblContraMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "DR_NOT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblDebitNoteMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CR_NOT":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblCreditNoteMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "BRECON":
                                            sqlTest = "SELECT Voucher_No FROM Acc.tblBankReconciliationMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;

                                        case "STOCK_TRANS":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblStockTransferMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;

                                        case "DAMAGE":
                                            sqlTest = "SELECT Voucher_No FROM Inv.tblDamageProductInvoiceMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                        case "CHEQUERCPT":
                                            sqlTest = "SELECT VoucherNo FROM Acc.tblChequeReceiptMaster WHERE VoucherNo LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                            dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                            break;
                                    }

                                    //sqlTest = "SELECT Voucher_No FROM Acc.tblJournalMaster WHERE Voucher_No LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                    //dtTest = Global.m_db.SelectQry(sqlTest, "vouNum");
                                    //DataRow dr3 = dt3.Rows[i3];
                                    if (Convert.ToInt32(m_Number) == 1)
                                    {
                                        autonum = 2;
                                    }
                                    if (VoucherType == "BANK_RCPT")
                                    {
                                        sqlTest = "SELECT VoucherNo FROM Acc.tblChequeReceiptMaster WHERE VoucherNo LIKE '%" + txtvno + "%' AND SeriesID ='" + SeriesID + "'";
                                        DataTable dtTest1 = Global.m_db.SelectQry(sqlTest, "vouNum");

                                        if (dtTest.Rows.Count == 0 && dtTest1.Rows.Count == 0)
                                        {
                                            if (m_Number != null)
                                            {
                                                int count = Convert.ToInt32(i3);
                                                if (Convert.ToInt32(dr1["StartingNo"]) > count)
                                                {
                                                    autonum = Convert.ToInt32(dr1["StartingNo"]);
                                                }
                                                else
                                                {
                                                    autonum = count;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (dtTest.Rows.Count == 0)
                                        {
                                            if (m_Number != null)
                                            {
                                                int count = Convert.ToInt32(i3);
                                                if (Convert.ToInt32(dr1["StartingNo"]) > count)
                                                {
                                                    autonum = Convert.ToInt32(dr1["StartingNo"]);
                                                }
                                                else
                                                {
                                                    autonum = count;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                //VoucherFormat += autonum.ToString();

                                //Check if it meets the ending number
                                if (dr1["SpecifyEndNo"].ToString() == "True")
                                {
                                    // checking Ending no to autonum
                                    if (Convert.ToInt32(dr1["EndNo"]) >= autonum)
                                    {
                                        //MessageBox.Show("Ya EndNO is greater than count");
                                        if (autonum >= Convert.ToInt32(dr1["WarningVouLeft"]))
                                        {

                                            MessageBox.Show(dr1["WarningMsg"].ToString());
                                        }
                                    }
                                    else //The number is greater than available ending number from setting
                                    {
                                        IncreasedNumber = -1;
                                        return null;
                                    }

                                    /// from here i cut the code
                                }
                                else
                                {
                                    //MessageBox.Show(" you havnt choosed SpecifyEndNo");

                                }
                                // This is code for incrementing the value of Number column in tblNumbering 

                                //this code is temporarily commented to check the voucher number of ram's type

                                incNumber = Convert.ToInt32(m_Number) + 1;

                                //string sql3 = "UPDATE System.tblSeries set AutoNumber='" + incNumber + "'WHERE SeriesID = '" + SeriesID + "'";
                                //Global.m_db.InsertUpdateQry(sql3);

                                // checking for the Renumbering Frequency****************

                                if (dr1["RenumberingFrq"].ToString() == "Yearly")
                                {
                                    //MessageBox.Show("Yearly");

                                }

                                else if (dr1["RenumberingFrq"].ToString() == "Daily")
                                {
                                    //MessageBox.Show("Daily");
                                }
                                else
                                {
                                    //MessageBox.Show("you havnt choosed the RenumberingFrq");

                                }
                                // for Fix Length of Numeric Part........................

                                if (dr1["NumericPart"].ToString() == "True")
                                {
                                    //VoucherFormat += autonum.ToString().PadLeft(Convert.ToInt32(dr1["TotalLengthNumPart"]), Convert.ToChar(dr1["PaddingChar"]));
                                    VoucherFormat += incNumber.ToString().PadLeft(Convert.ToInt32(dr1["TotalLengthNumPart"]), Convert.ToChar(dr1["PaddingChar"]));

                                }
                                else
                                {
                                    //MessageBox.Show("you are not allowed for choosing Fix Length of Numeric Part");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IncreasedNumber = -1;
                throw ex;

            }
            IncreasedNumber = incNumber;
            return VoucherFormat;
        }

        /// <summary>
        /// Update autonumber i.e. Last numberic part of voucher number in tblseries
        /// </summary>
        /// <param name="SeriesID"></param>
        /// <param name="incNumber"></param>
        /// <returns></returns>
        public int UpdateLastVoucherNum(int SeriesID, int incNumber)
        {
            string sql3 = "UPDATE System.tblSeries set AutoNumber='" + incNumber + "'WHERE SeriesID = '" + SeriesID + "'";
            return Global.m_db.InsertUpdateQry(sql3);
        }

        public string ValidateManualVouNum(string voucherNO, int SeriesID)
        {
            //for saving the manual VoucherNumber

            string sql = "SELECT * FROM System.tblVouNumConfig WHERE SeriesID = '" + SeriesID + "'";
            DataTable dt = Global.m_db.SelectQry(sql, "vouNum");

            if (dt.Rows.Count <= 0)
            {
                return ("INVALID_SERIES");
            }

            DataRow dr = dt.Rows[0];
            if (dr["NumberingType"].ToString() == "Manual")
            {                
                //CHECK IF THE VOUCHER NUMBER IS BLANK
                if (voucherNO == "")
                {
                    if (dr["BlankVouNum"].ToString() == "WARNING_ONLY")
                    {

                        return ("BLANK_WARN");

                    }
                    if (dr["BlankVouNum"].ToString() == "DONT_ALLOW")
                    {
                        return ("BLANK_DONT_ALLOW");
                        
                    }
                    if (dr["BlankVouNum"].ToString() == "NO_ACTION")
                    {
                        return "SUCCESS";

                    }
                }
                else //VOUCHER NOT BLANK, NOW CHECK FOR DUPLICITY
                {

                    //selecting VoucherNo from tblJournalMaster
                    string sql1 = "SELECT * FROM Acc.tblJournalMaster WHERE Voucher_No='" + voucherNO + "'";
                    object m_manualVouNum = Global.m_db.GetScalarValue(sql1);
                    if (m_manualVouNum != null) // if duplicated voucher number is Found
                    {
                        if (dr["DuplicateVouNum"].ToString() == "WARNING_ONLY")
                        {
                            return ("DUPLICATE_WARN");
                        }
                        if (dr["DuplicateVouNum"].ToString() == "DONT_ALLOW")
                        {
                            return ("DUPLICATE_DONT_ALLOW");
                        }
                        if (dr["DuplicateVouNum"].ToString() == "NO_ACTION")
                        {
                            return "SUCCESS";
                        }
                    }
                }
            }

            return "SUCCESS";
        }

        /// <summary>
        /// Voucher number is checked according to particular table and also for edit or new record 
        /// </summary>
        /// <param name="voucherNO"></param>
        /// <param name="SeriesID"></param>
        /// <param name="voucherID"></param>
        /// <param name="tblName"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public string ValidateManualVouNum(string voucherNO, int SeriesID,int voucherID,string tblName,bool isNew)
        {
            //for saving the manual VoucherNumber

            string sql = "SELECT * FROM System.tblVouNumConfig WHERE SeriesID = '" + SeriesID + "'";
            DataTable dt = Global.m_db.SelectQry(sql, "vouNum");

            if (dt.Rows.Count <= 0)
            {
                return ("INVALID_SERIES");
            }

            DataRow dr = dt.Rows[0];
            if (dr["NumberingType"].ToString() == "Manual")
            {
                //CHECK IF THE VOUCHER NUMBER IS BLANK
                if (voucherNO == "")
                {
                    if (dr["BlankVouNum"].ToString() == "WARNING_ONLY")
                    {

                        return ("BLANK_WARN");

                    }
                    if (dr["BlankVouNum"].ToString() == "DONT_ALLOW")
                    {
                        return ("BLANK_DONT_ALLOW");

                    }
                    if (dr["BlankVouNum"].ToString() == "NO_ACTION")
                    {
                        return "SUCCESS";

                    }
                }
                else //VOUCHER NOT BLANK, NOW CHECK FOR DUPLICITY
                {

                    //selecting VoucherNo from tblJournalMaster
                    string columnIDName = "";
                    switch (tblName)
                    {
                        case "Inv.tblSalesInvoiceMaster":
                            columnIDName = "SalesInvoiceID";
                            break;

                        case "Inv.tblSalesReturnMaster":
                            columnIDName ="SalesReturnID";
                            break;
                    }
                    

                    string sql1;
                    if(isNew)
                        sql1 = "SELECT * FROM " + tblName + " WHERE Voucher_No='" + voucherNO + "'";
                    else
                    {
                        sql1 = "SELECT * FROM " + tblName + " WHERE Voucher_No='" + voucherNO + "' and "+columnIDName+" != '"+voucherID+"'";
                    }
                    object m_manualVouNum = Global.m_db.GetScalarValue(sql1);
                    if (m_manualVouNum != null) // if duplicated voucher number is Found
                    {
                        if (dr["DuplicateVouNum"].ToString() == "WARNING_ONLY")
                        {
                            return ("DUPLICATE_WARN");
                        }
                        if (dr["DuplicateVouNum"].ToString() == "DONT_ALLOW")
                        {
                            return ("DUPLICATE_DONT_ALLOW");
                        }
                        if (dr["DuplicateVouNum"].ToString() == "NO_ACTION")
                        {
                            return "SUCCESS";
                        }
                    }
                }
            }

            return "SUCCESS";
        }

        public static DataTable  GetVouFormatInfo(int SeriesID)
        {
            return Global.m_db.SelectQry("SELECT * FROM System.tblVoucherFormat WHERE SeriesID = '" + SeriesID + "' ORDER BY VoucherFormatID ASC", "table");
        }

        public static DataTable GetAllMasterInfoByRowIDVouType(int RowID, string VoucherType)
        {
            DataTable dtMasterInfo = new DataTable();
            switch (VoucherType)
            {
                case "JRNL":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblJournalMaster WHERE JournalID ='"+RowID+"'", "tbl");
                    break;
                case "PURCH":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Inv.tblPurchaseInvoiceMaster WHERE PurchaseInvoiceID ='" + RowID + "'", "tbl");
                    break;
                case "PURCH_RTN":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Inv.tblPurchaseReturnMaster WHERE PurchaseReturnID ='" + RowID + "'", "tbl");
                    break;
                case "SALES":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Inv.tblSalesInvoiceMaster WHERE SalesInvoiceID ='" + RowID + "'", "tbl");
                    break;                  
                case "SLS_RTN":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Inv.tblSalesReturnMaster WHERE SalesReturnID ='" + RowID + "'", "tbl");
                    break;
                case "CNTR":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblContraMaster WHERE ContraID ='" + RowID + "'", "tbl");
                    break;
                case "CASH_RCPT":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblCashReceiptMaster WHERE CashReceiptID ='" + RowID + "'", "tbl");
                    break;
                case "CASH_PMNT":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblCashPaymentMaster WHERE CashPaymentID ='" + RowID + "'", "tbl");
                    break;
                case "BANK_RCPT":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblBankReceiptMaster WHERE BankReceiptID ='" + RowID + "'", "tbl");
                    break;
                case "BANK_PMNT":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblBankPaymentMaster WHERE BankPaymentID ='" + RowID + "'", "tbl");
                    break;
                case "BRECON":
                    dtMasterInfo = Global.m_db.SelectQry("SELECT Voucher_NO,SeriesID,ProjectID FROM Acc.tblBankReconciliationMaster WHERE BankReconciliationID ='" + RowID + "'", "tbl");
                    break;

            }
            return dtMasterInfo;

        }
    }
}
