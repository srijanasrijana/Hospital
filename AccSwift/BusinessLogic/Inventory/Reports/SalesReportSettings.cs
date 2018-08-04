using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
   public  class SalesReportSettings
    {

        //For Voucher_No
        public bool VoucherNo = false;
        public bool VoucherNoAll = false;
        public bool VocherNoSingle = false;
        public string VoucherNoSingleValue = "";

        //For Product
        public bool IsProductwise = false;
        public bool IsProductSingle = false;
        public int? ProductSingleID = null;
        public bool IsProductGroup = false;
        public int? ProductGroupID = null;
        public bool IsProductAll = false;


        //For Party
        public bool IsPartywise = false;
        public bool IsPartySingle = false;
        public int? PartySingleID = null;
        public bool IsPartyGroup = false;
        public int? PartyGroupID = null;
        public bool IsPartyAll = false;


        //For Date Range
        public bool IsDateRange = false;
        public DateTime? FromDate = new DateTime();

        public DateTime? ToDate = new DateTime();

        //For Purchase Account
        public bool IsSalesLedger = false;
        public int? SalesLedgerID = null;

        //for project
        public int? ProjectID = null;

        //for Depot
        public int? DepotID = null;

        public ArrayList AccClassID = new ArrayList();

    }
}
