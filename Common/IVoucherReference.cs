using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IVoucherReference
    {
        void GetAgainstReference(int refID, decimal amt, string crDr);
        void GetNewReference(string refName);
        void RemoveReference();
    }
}
