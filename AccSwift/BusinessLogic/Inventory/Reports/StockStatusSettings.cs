using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;

namespace BusinessLogic
{
    public class StockStatusSettings
    {
        public ArrayList AccClassID = new ArrayList();
       // public DateTime ToDate = new DateTime();
        
        public int? ProductID = null;
        public int? ProductGroupID = null;
        public String Depot ;
        public DateTime? AtTheEndDate = new DateTime();
        public bool ShowZeroQunatity = false;
        public bool ClosingStock = false;
        public bool OpeningStock = false;
       // public ListItem GroupID = new ListItem();
       // public ListItem DepotID = new ListItem();
       // public bool HasDateRange = false;
        public int ProjectID = 0;


    }
}
