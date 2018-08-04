using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
   public class District
    {
        public static DataTable GetDistrict()
        {

            string str = "select DistrictID as ID, DistrictName as Value from System.tblDistrict";
            return Global.m_db.SelectQry(str, "System.tblDistrict");
        }
        public static DataTable GetZoneByDist(int districtId)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.Text);
            Global.m_db.setCommandText("select d.*,z.ZoneName from System.tblDistrict d, System.tblZone z where DistrictID = @a and d.ZoneID=z.ZoneID");
            Global.m_db.AddParameter("@a", SqlDbType.Int, districtId);
            return Global.m_db.GetDataTable();
        }
    }
}
