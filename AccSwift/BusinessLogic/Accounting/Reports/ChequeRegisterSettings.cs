using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
    public enum ChequeStatus
    { 
        Cleared = 0,
        Uncleared = 1,
        Both = 2
    }

    public class ChequeRegisterSettings
    {
        public int ChequeReceived = 0;
        public ChequeStatus Status = 0;
        public bool HasDateRange = false;
        public DateTime? FromDate = null;
        public DateTime? ToDate = null;
        public bool HasBank = false;
        public bool HasParty = false;
        public int BankID = 0;
        public int PartyID = 0;
        public ArrayList AccClassID = new ArrayList();
        public int ProjectID = 0;
    }
}
