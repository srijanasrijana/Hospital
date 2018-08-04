using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace BusinessLogic
{
    public class Currency
    {

        public static int Default_Curr_ID = 1;


        /// <summary>
        /// Gets the all Table of currencies along with their information from database
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCurrencyTable()
        {
            return Global.m_db.SelectQry("SELECT CCYID, Name, Substring, Code, Symbol, Country FROM System.tblCurrency", "Currency");
        }

        /// <summary>
        /// Gets Currency Code from ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetCodeFromID(int ID)
        {
            object objCode = Global.m_db.GetScalarValue("SELECT Code FROM System.tblCurrency WHERE CCYID='" + ID.ToString() + "'");
            if (objCode == null)
                throw new Exception("Invalid currency code!");
            return objCode.ToString();
        }

        public static double Convert(int Curr_ID)
        {
            //Get the current exchange rate

            //convert and then return 
            return 0;
        }

    }
}
