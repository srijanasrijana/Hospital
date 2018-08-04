using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic
{
    public class LedgerCodeConfig
    {
        public static void InsertLedgerCodeConfiguration(int NumberingType, int IsCompulsan, DataTable dtLedgerCodeFormat)
        {
            try
            {
                if (dtLedgerCodeFormat.Rows.Count <= 0)
                    dtLedgerCodeFormat.Rows.Add(0);

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spLedgerCodeConfigCreate");
                Global.m_db.AddParameter("@LedgerNumberingType", SqlDbType.Int, NumberingType);
                Global.m_db.AddParameter("@IsCompulsory", SqlDbType.Int, IsCompulsan);
                Global.m_db.AddParameter("@LedgerCodeFormat", SqlDbType.Structured, dtLedgerCodeFormat);

                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void InsertGroupCodeConfiguration(int NumberingType, int IsCompulsan, DataTable dtLedgerCodeFormat)
        {
            try
            {
                if (dtLedgerCodeFormat.Rows.Count <= 0)
                    dtLedgerCodeFormat.Rows.Add(0);

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGroupCodeConfigCreate");
                Global.m_db.AddParameter("@GroupNumberingType", SqlDbType.Int, NumberingType);
                Global.m_db.AddParameter("@IsCompulsory", SqlDbType.Int, IsCompulsan);
                Global.m_db.AddParameter("@LedgerCodeFormat", SqlDbType.Structured, dtLedgerCodeFormat);

                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetLedgerNumberingType()
        {
            object objResult = Global.m_db.GetScalarValue("SELECT NumberingType FROM Acc.tblLedgerCodeConfig ");
            return Convert.ToInt32(objResult);
        }

        public static int GetLedgerGrpNumberingType()
        {
            object objResult = Global.m_db.GetScalarValue("SELECT NumberingType FROM Acc.tblGroupCodeConfig ");
            return Convert.ToInt32(objResult);
        }
    }
}
