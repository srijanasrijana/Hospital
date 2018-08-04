using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace BusinessLogic
{

    /// <summary>
    /// Used to Manage Language based functions like language switching, font management etc.
    /// </summary>
    public class LangMgr
    {

        #region Language and Fonts
        public static Lang DefaultLanguage = Lang.English; //Language Variable Holder

        public static float FontSize = 8;
        #endregion

        List<TranslateLookup> transLookup;
        public Dictionary<string, string> LookUp = new Dictionary<string, string>();

        DataTable dtResult;

        public LangMgr()
        {
            transLookup = new List<TranslateLookup>();
            
            dtResult = new DataTable();
        }


        public static Lang LangToLangType(string DefualtLanguage)
        {
            Lang ReturnLangType = Lang.Nepali;
            switch (DefualtLanguage)
            {
                case "English":
                    ReturnLangType = Lang.English;
                    break;
                case "Nepali":
                    ReturnLangType = Lang.Nepali;
                    break;

            }
            return ReturnLangType;

        }


        public static Font GetFont()
        {
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    return new Font("Arial", LangMgr.FontSize);
                    break;
                case Lang.Nepali:
                    return new Font("Bentray Nepali", LangMgr.FontSize+(float)0.5);
                    break;
                default:
                    return new Font("Arial", LangMgr.FontSize);
            }
        }

        /// <summary>
        /// Translates the code to the provided Language
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        public static string Translate(string code, Lang Language)
        {
            try
            {
                string retVal = "";
                switch (Language)
                {
                    case Lang.English:
                        retVal = (string)Global.m_db.GetScalarValue("SELECT English FROM System.tblLanguage WHERE Code='" + code.ToString() + "'");
                        break;
                    case Lang.Nepali:
                        retVal = (string)Global.m_db.GetScalarValue("SELECT Nepali FROM System.tblLanguage WHERE Code='" + code.ToString() + "'");
                        break;
                    default:
                        return "ERROR";
                }

                if (retVal == null || retVal == "")//If the code is not found do nothing
                    throw new Exception();
                return retVal;
            }
            catch (Exception ex)
            {
               
                //Do nothing if not found
                return "ERROR";
                //throw ex;
                //return "ERROR";
            }
        }

        /// <summary>
        /// Automatically Translate the code to the system language
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public static string Translate(string code)
        {
            try
            {
                Lang Language = LangMgr.DefaultLanguage;
                string retVal = "";
                switch (Language)
                {
                    case Lang.English:
                        retVal = (string)Global.m_db.GetScalarValue("SELECT English FROM System.tblLanguage WHERE Code='" + code.ToString() + "'");
                        break;
                    case Lang.Nepali:
                        retVal = (string)Global.m_db.GetScalarValue("SELECT Nepali FROM System.tblLanguage WHERE Code='" + code.ToString() + "'");
                        break;
                    default:
                        return "ERROR";
                }

                if (retVal == null || retVal == "")//If the code is not found do nothing
                    throw new Exception();
                return retVal;
            }
            catch (Exception ex)
            {
                //Do nothing if not found!
                return "ERROR";
            }
        }
        /// <summary>
        /// Translates the code to the default language and if the language code is not found, then returns the default text in the parameter
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static string Translate(string code, string Default)
        {
            try
            {
                Lang Language = LangMgr.DefaultLanguage;
                string retVal = "";
                switch (Language)
                {
                    case Lang.English:
                        retVal= (string)Global.m_db.GetScalarValue("SELECT English FROM System.tblLanguage WHERE Code='" + code.ToString() + "'");
                        break;
                    case Lang.Nepali:
                        retVal = (string)Global.m_db.GetScalarValue("SELECT Nepali FROM System.tblLanguage WHERE Code='" + code.ToString() + "'");
                        break;
                    default:
                        return "ERROR";
                }

                if (retVal == null || retVal == "")//If the code is not found do nothing
                    throw new Exception() ;
                return retVal;

            }
            catch (Exception ex)
            {
                return Default;
                //Do nothing if not found!
            }
        }


        public void AddTranslation(string code, Control ctrl)
        {
            try
            {
                transLookup.Add(new TranslateLookup(code, ctrl));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding translation");
                
            }
        }

        public bool BulkTranslate()
        {
            string[] lookup=new string[transLookup.Count];
            
            
            for (int i = 0; i < transLookup.Count; i++)
            {
                TranslateLookup tl=(TranslateLookup)transLookup[i];
                lookup[i] = tl.code;
            }

            //Now join them with ","
            string TransCode = "'" + String.Join("','", lookup) + "'";

            string LangField="English";
            if(LangMgr.DefaultLanguage==Lang.English)
                LangField="English";
            else if(LangMgr.DefaultLanguage==Lang.Nepali)
                LangField="Nepali";

            try
            {
                dtResult=Global.m_db.SelectQry("SELECT " + LangField + ", Code FROM System.tblLanguage WHERE Code IN (" + TransCode + ")","Language");
                for(int i=0;i<dtResult.Rows.Count;i++)
                {
                    DataRow dr=dtResult.Rows[i];
                    LookUp.Add(dr["Code"].ToString(),dr[LangField].ToString());
                }

                   
                for (int i = 0; i < transLookup.Count-1; i++)
                {
                    TranslateLookup tl = (TranslateLookup)transLookup[i];
                    tl.ctrl.Font = LangMgr.GetFont();
                    tl.ctrl.Text = LookUp[tl.code].ToString();

                }
                

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
                return false;
            }
        }


    }

    public class TranslateLookup
    {
        public string code;
        public Control ctrl;

        public TranslateLookup(string code, Control ctrl)
        {
            this.code = code;
            this.ctrl=ctrl;
        }

    }


}
