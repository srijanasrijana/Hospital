using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace BusinessLogic
{
   public class Project
    {
       public void Create(string ProjectName, string ParentProjectName, string Description)
       {
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spProjectCreate");
               Global.m_db.AddParameter("@EngName", SqlDbType.NVarChar, 50, ProjectName);
               Global.m_db.AddParameter("@NepName", SqlDbType.NVarChar, 50, ProjectName);//Set same for both for time being
               Global.m_db.AddParameter("@Under_EngName", SqlDbType.NVarChar, 50, ParentProjectName);
               Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 200, Description);
               object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

           }
           catch (Exception ex)
           {

               throw ex;
     
           }
       }

       public void Modify(int ProjectID, string NewProjectName, string ParentProjectName, string Description)
       {
           //Find which language is there
           string CurrLang;
           switch (LangMgr.DefaultLanguage)
           {
               case Lang.English:
                   CurrLang = "ENGLISH";
                   break;
               case Lang.Nepali:
                   CurrLang = "NEPALI";
                   break;
               default:
                   CurrLang = "ENGLISH";
                   break;
           }
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spProjectModify");
               Global.m_db.AddParameter("@OldID", SqlDbType.Int, ProjectID);
               Global.m_db.AddParameter("@NewProjectName", SqlDbType.NVarChar, 50, NewProjectName);
               Global.m_db.AddParameter("@ParentProject", SqlDbType.NVarChar, 50, ParentProjectName);
               Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 20, "ENGLISH");
               Global.m_db.AddParameter("@User", SqlDbType.NVarChar, 20, User.CurrUserID.ToString());
               Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 200, Description);
               object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
           }
           catch (Exception ex)
           {
               throw ex;
           }

       }

       public void Delete(string ProjectName)
       {
           //Language Management
           string LangField = "EngName";
           if (LangMgr.DefaultLanguage == Lang.English)
               LangField = "EngName";
           else if (LangMgr.DefaultLanguage == Lang.Nepali)
               LangField = "NepName";

           Global.m_db.SelectQry("DELETE FROM Acc.tblProject WHERE " + LangField + "='" + ProjectName + "'", "Acc.tblProject");

       }

       /// <summary>
       /// Gets all the Project informations under the given parent ID. If parent ID is 0 it returns only parent Project information.
       /// If you want to get all Projects, introduce ParentProjectID as -1
       /// </summary>
       /// <param name="ParentProjectID"></param>
       /// <returns></returns>
       public static DataTable GetProjectTable(int ParentProjectID)
       {
           DataTable dt = new DataTable();
           string strQuery = "";
           if (ParentProjectID == 0)//Only Parent group
           {
               strQuery = "SELECT * FROM Acc.tblProject WHERE ParentProjectID is null";
           }
           else if (ParentProjectID == -1)//All Groups
           {
               strQuery = "SELECT * FROM Acc.tblProject";
           }
           else
           {
               strQuery = "SELECT * FROM Acc.tblProject WHERE ParentProjectID =" + ParentProjectID.ToString();
           }
           return Global.m_db.SelectQry(strQuery, "tblProject");

       }

       /// <summary>
       /// Returns the table of Project which falls under given ProjectID and fills the result in ReturnTable. Recursive function
       /// </summary>
       /// <param name="ProjectID"></param>
       /// <param name="ReturnTable"></param>
       public static void GetProjectsUnder(int ProjectID, ArrayList ReturnIDs)
       {
           #region Language Management
           //tv.Font = LangMgr.GetFont();
           string LangField = "EngName";
           switch (LangMgr.DefaultLanguage)
           {
               case Lang.English:
                   LangField = "EngName";
                   break;
               case Lang.Nepali:
                   LangField = "NepName";
                   break;

           }
           #endregion
           DataTable dt;
           dt = Project.GetProjectTable(ProjectID);
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               DataRow dr = dt.Rows[i];
               ReturnIDs.Add(dr["ProjectID"]);
               GetProjectsUnder(Convert.ToInt32(dr["ProjectID"].ToString()), ReturnIDs);
           }

       }

       public static void GetChildProjects(int ProjectID,ref ArrayList ReturnIDs)
       {
           #region Language Management
           //tv.Font = LangMgr.GetFont();
           string LangField = "EngName";
           switch (LangMgr.DefaultLanguage)
           {
               case Lang.English:
                   LangField = "EngName";
                   break;
               case Lang.Nepali:
                   LangField = "NepName";
                   break;

           }
           #endregion
           DataTable dt;
           dt = Project.GetProjectTable(ProjectID);
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               DataRow dr = dt.Rows[i];
               ReturnIDs.Add(dr["ProjectID"]);
               GetProjectsUnder(Convert.ToInt32(dr["ProjectID"].ToString()), ReturnIDs);
           }

       }

       /// <summary>
       /// Generates XML of Project IDs ready to feed stored procedure.
       /// </summary>
       /// <param name="ProjectIDCollection"></param>
       /// <returns></returns>
       public static string GetXMLProject(ArrayList ProjectIDCollection)
       {


           System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
           System.IO.MemoryStream ms = new System.IO.MemoryStream();
           System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

           tw.WriteStartDocument();
           #region  XML Project ID
           try
           {
               tw.WriteStartElement("ProjectIDSettings");
               
               foreach (int tag in ProjectIDCollection)
               {
                   //AccClassID.Add(Convert.ToInt32(tag));
                   tw.WriteElementString("ProjectID", tag.ToString());
               }
               tw.WriteEndElement();
           }
           catch
           {
               throw new Exception("Error creating XML for ProjectID");
           }

           //  }
           // tw.WriteFullEndElement();
           #endregion
           tw.WriteEndDocument();
           tw.Flush();
           tw.Close();
           string strXML = AEncoder.GetString(ms.ToArray());
           return strXML;
       }
       /// <summary>
       /// Gets the ProjectID of the given Project name
       /// </summary>
       /// <param name="Name"></param>
       /// <param name="LangField"></param>
       /// <returns></returns>
       public static int GetIDFromName(string Name, Lang Language)
       {
           string LangField = "EngName";
           switch (Language)
           {
               case Lang.English:
                   LangField = "EngName";
                   break;
               case Lang.Nepali:
                   LangField = "NepName";
                   break;
           }


           object objResult = Global.m_db.GetScalarValue("SELECT ProjectID FROM Acc.tblProject WHERE " + LangField + "='" + Name + "'");
           return Convert.ToInt32(objResult);
       }

       public static DataTable GetProjectByID(int ProjectID, Lang Language)
       {
           string LangField = "EngName";
           if (Language == Lang.English)
               LangField = "EngName";
           else if (Language == Lang.Nepali)
               LangField = "NepName";
           string strQuery = "SELECT a.ProjectID ID, a." + LangField + " Name, b." + LangField + " Parent, a.Description Description FROM Acc.tblProject a LEFT OUTER JOIN Acc.tblProject b ON a.ParentProjectID=b.ProjectID WHERE a.ProjectID='" + ProjectID.ToString() + "'";
           return Global.m_db.SelectQry(strQuery, "Search");
       }

       /// <summary>
       /// Get all the rows from tblProject.
       /// </summary>
       /// <param name="Language"></param>
       /// <returns></returns>
       public static DataTable GetAllProject(Lang Language)
       {
           string LangField = "EngName";
           if (Language == Lang.English)
               LangField = "EngName";
           else if (Language == Lang.Nepali)
               LangField = "NepName";
           string strQuery = "select pg.ProjectID, pg." + LangField + " as Project,ISNULL( p." + LangField + ",'No Parent') as ParentProject from acc.tblProject pg left join acc.tblProject p on pg.ParentProjectID = p.ProjectID";
            return Global.m_db.SelectQry(strQuery,"acc.tblProject");
       }

    }

}
