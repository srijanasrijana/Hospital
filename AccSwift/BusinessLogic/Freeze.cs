using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DateManager;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class Freeze
    {
        public static bool IsDateFreeze(DateTime? TestDate)
        {
            if (Settings.GetSettings("FREEZE_STATUS") != "0")
            {
                if (TestDate >= Date.ToDotNet(Settings.GetSettings("FREEZE_START_DATE")) && TestDate <= Date.ToDotNet(Settings.GetSettings("FREEZE_END_DATE")))
                {
                    MessageBox.Show("Input date is out of range!");
                    return true;
                }
            }

            ////read fiscal year
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();

            if (TestDate < CompDetails.FYFrom || TestDate > Date.ToDotNet(CompanyInfo.GetFiscalYearInfo()))
            {
                MessageBox.Show("Input date is out of Fiscal Year!");
                return true;
            }                

            if (Date.GetServerDate() < TestDate)
            {
                switch (Settings.GetSettings("POST_DATE_TRANSACTION"))
                {
                    case "Allow":
                        return false;

                    case "Warn":
                        if (MessageBox.Show("Post date transaction is enabled in you system. Do you want to continue?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            return false;
                        else
                            return true;

                    case "Deny":
                        MessageBox.Show("Post date transaction is enabled in you system.");
                        return true;
                }
            }  
          
            return false;   //else
                
        }

    }
}
