using System;
using System.Collections;


namespace BusinessLogic
{
    public class AccountingClass
    {

        /// <summary>
        /// Converts ArrayList of Accounting Class IDs to XML so that we can pass it to the stored procedure. It doesnt recursively get child classes. That is done by stored procedure.
        /// </summary>
        /// <param name="arrAccountingClass">ArrayList of Accounting Class IDs</param>
        /// <returns></returns>
        public static string GetXMLAccClass(ArrayList arrAccountingClass)
        {

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();

            #region  Accountclass
            //tw.WriteStartElement("LEDGERTRANSACT");
            //{            
            //    //Write Checked Accounting class ID
            try
            {
                tw.WriteStartElement("AccClassIDSettings");
                foreach (string tag in arrAccountingClass)
                {
                    
                    tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                }
                tw.WriteEndElement();
            }
            catch
            { }

            // }
            // tw.WriteFullEndElement();
            #endregion

            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);
            return strXML;
        }
    }
}
