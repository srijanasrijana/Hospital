using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
    public class BalanceSheetSettings:TransactSettings
    {
        public enum DisplayFormat
        {
            TFormat=0,
            Vertical=1,
            Standard=2
        }
        
        public bool Detail = false;
        public bool AllGroups = false;
        public bool OnlyPrimaryGroups = false;
        public bool ShowSecondLevelDtl = false;
        public DisplayFormat DispFormat=DisplayFormat.TFormat;
    }
}
