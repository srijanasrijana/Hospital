using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
   public class Zone
    {
        public static DataTable GetZone()
        {

            string str = "select ZoneID as ID, ZoneName as Value from System.tblZone";
            return Global.m_db.SelectQry(str, "System.tblNationality");
        }

    }
}
