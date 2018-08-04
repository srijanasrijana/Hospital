using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    class PurchaseReturnRegisterSettings
    {
        public int? PartyID = null;
        public int? partyGroupID = null;
        public int? DepotID = null;
        public int? ProjectID = null;
        public DateTime? FromDate = new DateTime();
        public DateTime? ToDate = new DateTime();
    }
}
