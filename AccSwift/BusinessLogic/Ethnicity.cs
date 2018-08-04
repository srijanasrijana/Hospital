using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
  public  class Ethnicity
    {
      public static DataTable GetEthIdValue()
      {
          String str = "SELECT EthnicityID as ID,EthnicityName as Value FROM System.tblEthnicity";
          return Global.m_db.SelectQry(str, "System.tblEthnicity");
      }
    }
}
