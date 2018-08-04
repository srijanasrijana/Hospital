using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class Nationality
    {
        #region Nationality
        public static DataTable getNationality()
        {

            string str = "select * from System.tblNationality";
            return Global.m_db.SelectQry(str, "Nationality");
        }


        public static DataTable getNationalityByID(int ID)
        {
            string str = "select NationalityId,NationalityName from System.tblNationality  where NationalityId=" + ID + "";
            return Global.m_db.SelectQry(str, "Nationality");
        }

        public static int insertIntoNationality(string name)
        {
            string str = "insert into System.tblNationality values('" + name + "')";
            return Global.m_db.InsertUpdateQry(str);
        }

        public static int updateNationality(int ID, string name)
        {
            string str = "update System.tblNationality set NationalityName='" + name + "' where NationalityId =" + ID + "";
            return Global.m_db.InsertUpdateQry(str);
        }


        public static int deletefromNationality(int naId)
        {
            string str = "delete from System.tblNationality where NationalityId  =" + naId + "";
            return Global.m_db.InsertUpdateQry(str);
        }
        #endregion

    }
}
