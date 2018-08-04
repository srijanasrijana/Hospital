using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;


namespace BusinessLogic.Inventory
{
    public class OpeningQuantity
    {
        public void opqty(int ProductID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            Global.m_db.setCommandText("select oq.OpenPurchaseQty as opqty from inv.tblOpeningQuantity oq where oq.ProductID=@ProductID");
            Global.m_db.AddParameter("@ProductID", SqlDbType.Int,ProductID);
        }
    }
}
