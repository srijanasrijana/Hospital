using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic.Inventory.Reports
{
  public class StockAgingSetting
    {
        public ArrayList AccClassID = new ArrayList();
       
        public int? ProductID = null;
        public int? ProductGroupID = null;
        public int DepotId;
        public DateTime AtTheEndDate = new DateTime();
        public bool allProduct = false;
        public bool singleProduct = false;
        public bool productGroup = false;
        public bool depot = false;
        public int ProjectID = 0;
    }
}
