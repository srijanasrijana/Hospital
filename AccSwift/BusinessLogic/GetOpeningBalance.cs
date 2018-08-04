using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;
using DateManager;


namespace BusinessLogic
{
    public class GetOpeningBalance
    {
        public  DataTable GetOpeningBalanceByParent(int parentid,int ledgerid)
        {
            string strQuery = "";
            strQuery = " select * from Acc.tblOpeningBalance where accClassID='"+parentid+"' and ledgerid='"+ledgerid+"'";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tblProductGroup");
            return dt;
            // return Global.m_db.SelectQry(strQuery, "tblProduct");
        }
    }
}
