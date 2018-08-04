using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public interface IMDIMainForm
    {
        //void OpenForm(string formname, object param=null);
        void OpenFormArrayParam(string formname, object[] param = null);
 
    }  
}
