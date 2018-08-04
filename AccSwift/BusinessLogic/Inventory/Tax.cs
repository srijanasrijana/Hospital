using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
   public class Tax
    {

        public enum TaxSettings { NetAmt, Gross, Tax1, Tax2, Tax3 };


        int TaxPercentage;
        TaxSettings CurrentTaxSettings;

    }
}
