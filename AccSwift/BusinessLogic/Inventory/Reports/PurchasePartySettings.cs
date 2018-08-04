using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.Inventory.Reports;
using System.Collections;

namespace BusinessLogic
{
    public class PurchasePartyTransactSettings
    {
        public int? ProductGroupID = null;
        public int? PartyGroupID = null;
        public int? ProductID = null;
        public int? PartyID = null;
        public int? PurchaseLedgerID = null;
        public int? DepotID = null;
        public int? ProjectID = null;
        public DateTime? FromDate = new DateTime();
        public DateTime? ToDate = new DateTime();
        public string ReportType = "";
        public ArrayList AccClassID = new ArrayList();
    }
}
