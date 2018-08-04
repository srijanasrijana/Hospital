using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
    public class InventoryBookSettings
    {
        public ArrayList AccClassID = new ArrayList();
        public int? PartyID = null;
        public int? partyGroupID = null;
        public int? ProductID = null;
        public int? ProductGroupID = null;
        public int? DepotID = null;
        public int? ProjectID = null;
        public DateTime? FromDate = new DateTime();
        public DateTime? ToDate = new DateTime();
        public int? OpeningQty=null;
        public string ProductName = null;
        public string VoucherType = "PURCH";

    }
}
