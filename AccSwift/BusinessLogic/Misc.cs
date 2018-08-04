using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class Misc
    {
        
        // IsNumeric Function
        public static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        //Checks whether the parameter is integer data type
        public static bool IsInt(object Expression, bool AcceptNegative)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            int retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = int.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            if (AcceptNegative)
                return isNum;
            else
            {
                return ((retNum >= 0) ? isNum : false);
            }
            return isNum;
        }


        /// <summary>
        /// Returns the format mask of the number for its decimal places and comma separation settings
        /// </summary>
        /// <param name="CommaSeparated"></param>
        /// <param name="DecimalPlaces"></param>
        /// <returns></returns>
        public static string FormatNumber(bool CommaSeparated, int DecimalPlaces)
        {
            //string str1 = (DecimalPlaces > 0 ? ".".PadRight(DecimalPlaces + 1, '0') : "");
            string str1 = (DecimalPlaces > 0 ? "0.".PadRight(DecimalPlaces + 2, '0') : "0");

            string str2 = (CommaSeparated == true ? "#," : "##");
            string FormatNumber = str2 + str1;
            
                return FormatNumber;
            
        }

        public static byte[] ReadBitmap2ByteArray(string fileName)
        {
            if ((fileName == null) || (fileName.Length == 0))
                return null;
            using (Bitmap image = new Bitmap(fileName))
            {
                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
            
        }


        /// <summary>
        /// Returns image from the object of type image from database
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public static Image GetImageFromByte(Object Image)
        {
            // Get bytes return from stored proc
            //drow[1] = imgData == null ? DBNull.Value : (object)imgData;
            byte[] b = (byte[])Image;
            if (b != null)
            {
                
                // Open a stream for the image and write the bytes into it

                MemoryStream stream = new MemoryStream(b);
                stream.Write(b, 0, b.Length);
                //picTemplate.Image = Image.FromStream(stream);
                // Create a bitmap from the stream
                Bitmap bmp = new Bitmap(System.Drawing.Image.FromStream(stream));


                // Close the stream
                stream.Close();
                return (Image)bmp;
            }
            else
                return null;
        }



        public static void WriteLogo(System.Data.DataSet ds, string Table)
        {
            try
            {
                
                Global.m_db.DataAdaptarFill(ds, Table, "SELECT Logo FROM System.tblCompanyInfo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteLogo1(System.Data.DataSet ds, string Table)
        {
            try
            {
                Global.m_db.DataAdaptarFill(ds, Table, "SELECT cast(Logo as bit) Logo FROM System.tblCompanyInfo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal GetDecimalValueByDecimalPlaces(decimal DecimalValue)
        {
            return decimal.Round(DecimalValue, Convert.ToInt32(Global.DecimalPlaces), MidpointRounding.AwayFromZero);

        }

        public static void WriteToolTips(Control ctrl,string ctrlText)
        {
            ToolTip buttonToolTip = new ToolTip();
            //buttonToolTip.ToolTipTitle = "Button Tooltip";
            buttonToolTip.UseFading = true;
            buttonToolTip.ForeColor = Color.RoyalBlue;
            buttonToolTip.UseAnimation = true;
            buttonToolTip.ShowAlways = true;
            buttonToolTip.AutoPopDelay = 5000;
            buttonToolTip.InitialDelay = 1000;
            buttonToolTip.ReshowDelay = 500;
            buttonToolTip.SetToolTip(ctrl, ctrlText);
        }

        /// <summary>
        /// Enables or disables the control collection
        /// </summary>
        /// <param name="Controls"></param>
        public static void EnableControls(Control.ControlCollection Controls,bool Enable)
        {
            foreach (Control c in Controls)
            {
                c.Enabled = Enable;
                if (c is MenuStrip)
                {
                    foreach (ToolStripItem item in ((MenuStrip)c).Items)
                    {
                        item.Enabled = Enable;
                    }
                }
                if (c.Controls.Count > 0)
                   EnableControls(c.Controls,Enable);

            }
        }


        public static void EnableControls(Control[] Controls, bool Enable)
        {
            foreach (Control c in Controls)
                c.Enabled = Enable;

        }      

    }
}
