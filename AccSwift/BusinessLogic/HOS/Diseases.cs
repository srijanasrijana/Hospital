using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessLogic.HOS
{

    public class DiseaseInfo
    {
        public int DiseseLisID { get; set; }

        public int Disease { get; set; }
        public int DoctorConsult { get; set; }
        public decimal charge { get; set; }
    }

    public interface IDiseases
    {

        void CreateGrp(string GroupName, string ParentGroup, string Remarks);
        void ModifyGrp(int GroupID, string NewGroupName, string ParentGroupName, string Remark);
        void Delete(string DiseasesHeadName);
        bool CreateDiseases(string DiseasesName, int GroupID, string Remarks,byte[] Image);
        bool ModifyDiseases(int DiseasesID, string DiseasesName, int GroupID, string Remarks, byte[] Image);
        void DeleteDisease(int DiseasesID);
    }

    public class Disease1 : IDiseases
    {
        public void CreateGrp(string GroupName, string ParentGroup, string Remarks)
        {
       
          try
          {
              
              Global.m_db.BeginTransaction();
              Global.m_db.ClearParameter();
              Global.m_db.setCommandType(CommandType.StoredProcedure);
              Global.m_db.setCommandText("Hos.spDiseasesGroupCreate");
              Global.m_db.AddParameter("@EngName", SqlDbType.NVarChar, 50, GroupName);
              Global.m_db.AddParameter("@NepName", SqlDbType.NVarChar, 50, GroupName);//Set same for both for time being
              Global.m_db.AddParameter("@Under_EngName", SqlDbType.NVarChar, 50, ParentGroup);
              Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
              object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
              Global.m_db.ProcessParameter();
              Global.m_db.CommitTransaction();
            
           }
          catch (Exception ex)
          {
              Global.m_db.RollBackTransaction();
              throw ex;
              
          }
          
   
        }

        public void ModifyGrp(int GroupID, string NewGroupName, string ParentGroupName, string Remarks)
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
                Global.m_db.setCommandText("Hos.spDiseasesGroupModify");
                Global.m_db.AddParameter("@OldID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@NewGroupName", SqlDbType.NVarChar, 50, NewGroupName);
                Global.m_db.AddParameter("@ParentGroup", SqlDbType.NVarChar, 50, ParentGroupName);
                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 20, "ENGLISH");
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(string GroupName)
        {
            string LangField = "EngName";
            if (LangMgr.DefaultLanguage == Lang.English)
                LangField = "EngName";
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                LangField = "NepName";

            Global.m_db.SelectQry("DELETE FROM Hos.tblDiseasesGroups WHERE " + LangField + "='" + GroupName + "'", "Inv.tblProductGroup");
        }

        public bool CreateDiseases(string DiseasesName, int GroupID, string Remarks, byte[] Image)
          
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Hos.spDiseasesCreate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, DiseasesName);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Image", SqlDbType.Binary, Image);
             


                SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                Global.m_db.ProcessParameter();
                Global.m_db.CommitTransaction();
                return (objReturn.Value.ToString() == "SUCCESS" ? true : false);
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
              
            }
        }

        public bool ModifyDiseases(int DiseasesID, string DiseasesName, int GroupID, string Remarks, byte[] Image)
        {

            try
            {
                Global.m_db.BeginTransaction();
                string LangField = "ENGLISH";
                //string str1 = (string)Global.MakeNull(Person_Name);
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Hos.spDiseasesModify");
                Global.m_db.AddParameter("@DiseaseID", SqlDbType.Int, DiseasesID);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, DiseasesName);
                if (LangMgr.DefaultLanguage == Lang.English)
                    LangField = "ENGLISH";
                else if (LangMgr.DefaultLanguage == Lang.Nepali)
                    LangField = "NEPALI";
                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 50, LangField);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 50, Remarks);
                Global.m_db.AddParameter("@Image", SqlDbType.Binary, Image);
               
                SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                Global.m_db.ProcessParameter();

                Global.m_db.CommitTransaction();
                return (objReturn.Value.ToString() == "SUCCESS" ? true : false);

            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
                
            }
        }


        public void DeleteDisease(int DiseasesID)
        {
          
             Global.m_db.BeginTransaction();

             try
             {
                 Global.m_db.InsertUpdateQry("DELETE FROM Hos.tblDiseases WHERE DiseaseID ='" + DiseasesID + "'");
             }
             catch
             {
                 Global.m_db.RollBackTransaction();
                 throw new Exception("Unable to delete the Disease");
             }
             Global.m_db.CommitTransaction();

         

        }
    }

  public  class Diseases
    {
     
   
     public static DataTable UpdateDiseasesGroup(int diseasesGrpID,int diseaseListID, int DiseasesID, string DiseasesGroupName, string remarks)
     {
         string str = "update Hos.tblDiseasesGroup set DisesesListID='" + diseaseListID + "', DiseasesID='" + DiseasesID + "',DiseasesGroupName='" + DiseasesGroupName + "',Remarks='" + remarks + "' where DiseasesGrpID='" + diseasesGrpID + "'";
         return Global.m_db.SelectQry(str, "tblDiseasesGroup");
     }

     public static DataTable GetDoctorInfoByName()
     {
         string str = "select ID as ID, CONCAT( FirstName,' ',MiddleName,' ',LastName)  as DoctorName from Hos.tblDoctor";
         return Global.m_db.SelectQry(str, "Hos.tblDoctor");
     }
     public static DataTable GetGroupTable(int ParentGroupID)
     {
         if (ParentGroupID == 0)//Only Parent group
         {
             return Global.m_db.SelectQry("SELECT * FROM Hos.tblDiseasesGroups WHERE Parent_GrpID is null", "tblDiseasesGroups");
         }
         else if (ParentGroupID == -1)//All Groups
         {
             return Global.m_db.SelectQry("SELECT * FROM Hos.tblDiseasesGroups", "tblDiseasesGroups");
         }
         else
         {
             return Global.m_db.SelectQry("SELECT * FROM Hos.tblDiseasesGroups WHERE Parent_GrpID =" + ParentGroupID.ToString(), "tblDiseasesGroups");
         }
     }

     public static DataTable GetDiseaseTable(int Group_ID)
     {
         return Global.m_db.SelectQry("SELECT * FROM Hos.tblDiseases WHERE GroupID ='" + Group_ID.ToString() + "'", "tblGroup");
     }


         public static DataTable GetGroupByID(int GroupID, Lang Language)
        {
            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";

            return Global.m_db.SelectQry("SELECT a.GroupID ID, a." + LangField + " Name, b." + LangField + " Parent, a.Remarks Remarks FROM Hos.tblDiseasesGroups a LEFT OUTER JOIN Hos.tblDiseasesGroups b ON a.Parent_GrpID=b.GroupID WHERE a.GroupID='" + GroupID.ToString() + "'", "Search");

        }

         public static DataTable GetDiseaseInfo(int ProductID, Lang Language)
         {
             #region Language Management

             string LangField = "EngName";
             if (Language == Lang.English)
                 LangField = "EngName";
             else if (Language == Lang.Nepali)
                 LangField = "NepName";

             #endregion
             string strQuery = "";
             if (ProductID == -1)
             {
                 strQuery = "SELECT Prod.DiseaseID ID, Prod." + LangField + " DisName, grp." + LangField + " GroupName, Prod.GroupID,Prod.Remarks Remarks,prod.Image FROM Hos.tblDiseases Prod LEFT OUTER JOIN Hos.tblDiseasesGroups grp ON Prod.GroupID=grp.GroupID ";
                 return Global.m_db.SelectQry(strQuery, "Search");
             }

             else
             {
                 strQuery = "SELECT Prod.DiseaseID ID, Prod." + LangField + " DisName, grp." + LangField + " GroupName, Prod.GroupID,Prod.Remarks Remarks,prod.Image FROM Hos.tblDiseases Prod LEFT OUTER JOIN Hos.tblDiseasesGroups grp ON Prod.GroupID=grp.GroupID WHERE Prod.DiseaseID='" + ProductID + "'";
                 return Global.m_db.SelectQry(strQuery, "Search");
             }
         }

         public static int GetDiseaseIDFromName(string Name, Lang Language)
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


             object objResult = Global.m_db.GetScalarValue("SELECT DiseaseID FROM Hos.tblDiseases WHERE " + LangField + "='" + Name + "'");
             return Convert.ToInt32(objResult);
         }


         public static int GetGroupIDFromName(string Name, Lang Language)
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


             object objResult = Global.m_db.GetScalarValue("SELECT GroupID FROM Hos.tblDiseasesGroups  WHERE " + LangField + "='" + Name + "'");
             return Convert.ToInt32(objResult);
         }

         public static void GetDiseaseUnder(int GroupID, ArrayList ReturnIDs)
         {


             #region Language Management
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
             dt = Diseases.GetGroupTable(GroupID);
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 DataRow dr = dt.Rows[i];
                 ReturnIDs.Add(dr["GroupID"]);
                 GetDiseaseUnder(Convert.ToInt32(dr["GroupID"].ToString()), ReturnIDs);
             }
         }

         public static DataTable getDisease()
         {
             return Global.m_db.SelectQry("select pg.EngName DiseaseGroup,p.EngName DiseaseName from Hos.tblDiseases p,Hos.tblDiseasesGroups pg where p.GroupID=pg.GroupID ", "Hos.tblDiseases");

         }


         public static DataTable GetDoctorSpecification()
         {

             string str = "select * from Hos.tblWorkExperiences";
             return Global.m_db.SelectQry(str, "tblWorkExperiences");
         }

     
         public static DataTable GetDocotrSpecilistInfo(int id)
         {

             string str = "select d.ID AS id,w.ExperiencesID AS id,CONCAT(FirstName, '' ,MiddleName ,'',LastName) as DoctorName,d.DoctorCode,d.Phone2 as ContactNo, d.Email, d.PermAddress as [Address],dis.DistrictName as City,w.CompanyName,Date.fnEngToNep(d.StartDate) StartDate, w.Specilization from hos.tblDoctor d join hos.tblWorkExperiences w on d.Id=w.DoctorID  join System.tblDistrict dis on d.PermDistID=dis.DistrictID where d.ID='" + id + "' OR w.ExperiencesID='" + id + "'";
             return Global.m_db.SelectQry(str, "tblDoctor");
         }

        
         public static DataTable GetDetailByDoctorName(string name)
         {

             string str = "select CONCAT(FirstName, '' ,MiddleName ,'',LastName) as DoctorName,d.DoctorCode from Hos.tblDoctor d where DoctorCode='Dr001''" + name + "'";
             return Global.m_db.SelectQry(str, "tblDoctor");
         }
}
}
