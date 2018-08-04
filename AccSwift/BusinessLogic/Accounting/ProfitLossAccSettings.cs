using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace BusinessLogic
{
    public class ProfitLossAccSettings:TransactSettings
    {

        public enum DisplayFormat
        {
            TFormat = 0,
            Vertical = 1
        }

        
        public bool ShowZeroBalance = false;
        public bool ShowSecondLevelDtl = false;
        public bool Summary = false;
        public bool Detail = false;
        //public bool AllGroups = false;
        //public bool OnlyPrimaryGroups = false;
        public DisplayFormat DispFormat = DisplayFormat.Vertical;

    }
}
