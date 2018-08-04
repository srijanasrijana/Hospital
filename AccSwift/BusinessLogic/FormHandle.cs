using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public enum DType{
         INT     //ACCEPTS ONLY INTEGER
        ,FLOAT  //ONLY FLOAT TYPE
        ,NAME   //ONLY Characters and spaces(No numbers)
        ,DATE,  //Only Dates
        USERNAME //Characters and numbers but no spaces
    } //holds the document types to handle the form

    /// <summary>
    /// All form handling task is done by this class, such as form validation etc.
    /// </summary>
    public class FormHandle
    {
        private bool m_Valid = true; //holds if the form is valid

        private ValidateParam[] m_Param;

        private List<ValidateParam> m_List=new List<ValidateParam>();//Used a template to hold the parameters into a list

        /// <summary>
        /// A constructor
        /// </summary>
        public FormHandle()
        {
        }

        
        /// <summary>
        /// Validates a specific control, textbox or a combobox for its input
        /// </summary>
        /// <param name="Ctrl"></param>
        /// <param name="DataType"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public void AddValidate(Control Ctrl, DType DataType, string ErrorMsg)
        {
            m_List.Add(new ValidateParam(Ctrl, DataType, ErrorMsg));
        }
 
        /// <summary>
        /// Returns true if all the input is correct.
        /// </summary>
        /// <returns></returns>
        public  bool Validate()
        {
            for(int i=0;i<m_List.Count;i++)
            {
                ValidateParam myParam = (ValidateParam)m_List[i];
                string Value = myParam.Ctrl.Text;

                switch (myParam.DocType)
                {
                    case DType.INT:
                        try
                        {
                            int intVal = Convert.ToInt32(Value);
                        }
                        catch (Exception)
                        {
                            myParam.Ctrl.Focus();
                            Global.Msg(myParam.ErrorMsg);
                            return false;
                                                      

                        }
                        break;

                    case DType.FLOAT:
                        try
                        {
                            double dbl = Convert.ToDouble(Value);
                        }
                        catch (Exception)
                        {
                            myParam.Ctrl.Focus();
                            Global.Msg(myParam.ErrorMsg);
                            return false;
                        }
                        break;
                    case DType.NAME:
                        try
                        {

                            if (String.IsNullOrEmpty(Value))
                                throw new Exception();

                            //Regular expression for allowing just numbers and alphabets but with spaces
                            Regex rgx = new Regex("[^a-zA-Z0-9 ]");
                            if (rgx.IsMatch(Value))
                                throw new Exception();


                            //Check if it is only a number or not
                            float outFloat;
                            if (float.TryParse(Value, out outFloat))
                                throw new Exception();

                            string str = Convert.ToString(Value);
                        }
                        catch (Exception)
                        {
                            myParam.Ctrl.Focus();
                            Global.Msg(myParam.ErrorMsg);
                            return false;
                        }
                        break;

                    case DType.USERNAME:  //This should only accept numbers and alphabets
                        try
                        {

                            //Regular expression for allowing just numbers and alphabets(Not even spaces)
                            Regex rgx = new Regex("[^a-zA-Z0-9]");
                            if (rgx.IsMatch(Value))
                                throw new Exception();

                            //Username can only be a number


                        }
                        catch (Exception)
                        {
                            myParam.Ctrl.Focus();
                            Global.Msg(myParam.ErrorMsg);
                            return false;
                        }
                        break;



                    case DType.DATE:
                        try
                        {
                            //Split
                            int DateHolder;
                            char SplitChar='/';
                            if (LangMgr.DefaultLanguage == Lang.Nepali)
                                SplitChar = '÷';
                            else if (LangMgr.DefaultLanguage == Lang.English)
                                if(Value.Contains('-'))
                                    SplitChar='-';
                                else 
                                    SplitChar='/';
                            string[] myDate = Value.Split(SplitChar);
                            int length = myDate[0].Length + myDate[1].Length + myDate[2].Length;
                            if (length < 8)
                                throw new Exception();

                            DateHolder = Convert.ToInt32(myDate[0]);
                            DateHolder = Convert.ToInt32(myDate[1]);
                            DateHolder = Convert.ToInt32(myDate[2]);


                        }
                        catch (Exception)
                        {
                            myParam.Ctrl.Focus();
                            Global.Msg(myParam.ErrorMsg);
                            return false;
                        }
                        break;
                }
            }//end for


            return true;
        }//end function





    }//end of class formhandle
   

    /// <summary>
    /// Just a parameter to handle forms
    /// </summary>
    class ValidateParam
    {
        public Control Ctrl;
        public DType DocType;
        public string ErrorMsg;

        public ValidateParam(Control ctrl, DType dtype, string errorMsg)
        {
            this.Ctrl = ctrl;
            this.DocType = dtype;
            this.ErrorMsg = errorMsg;
        }
    }


}
