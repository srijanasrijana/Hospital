using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using System.Collections;
using CrystalDecisions.Shared;
using Common;
using System.Threading;
using Inventory.Reports;

namespace Inventory
{
    public interface IfrmPurchaseReturn
    {
        void PurchaseReturn(int RowID);
    }
    public partial class frmPurchaseReturn : Form, ListProduct, IVoucherRecurring, IVoucherList
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        private string Prefix = "";
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private int loopCounter = 0;
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;
        private double purchaserate = 0;
        private string productcode = "";
        private int CurrRowPos = 0;
        private double VAT = 0;
        private IMDIMainForm m_MDIForm;

        //for exprt menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        private enum GridColumn : int
        {
            Del = 0, SNo, Code_No, ProductName, Qty, PurchaseRate, Amount, SplDisc_Percent, SplDisc, NetAmount, ProductID
        };
        PurchaseReturn m_PurchaseReturn = new PurchaseReturn();
        ListItem liProjectID = new ListItem();
        private int PurchaseReturnIDCopy = 0;
        ListItem SeriesID = new ListItem();
        List<int> AccClassID = new List<int>();
        private Inventory.Model.dsPurchaseReturn dsPurchaseReturn = new Model.dsPurchaseReturn();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        DataTable dtAccClassID = new DataTable();
        private int m_PurchaseReturnID;
        private int PurchaseInvoiceIDCopy = 0;
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductCode = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscPercentage = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtGrossAmt = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtNetAmt = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        double GrossAmount = 0;
        double NetAmount = 0;

        private bool m_isRecurring;
        int m_RVID = 0; 
        public frmPurchaseReturn()
        {
            InitializeComponent();
        }
        public frmPurchaseReturn(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

        }
        public frmPurchaseReturn(int PurchaseReturnID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_PurchaseReturnID = PurchaseReturnID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmPurchaseReturn(int PurchaseReturnID)
        {
            InitializeComponent();
            this.m_PurchaseReturnID = PurchaseReturnID;
        }

        //Fill the cboUnder List box with Project Head
        private void ListProject(ComboBox ComboBoxControl)
        {
            #region Language Management
            ComboBoxControl.Font = LangMgr.GetFont();
            string LangField = "English";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            #endregion

            ComboBoxControl.Items.Clear();
            DataTable dt = Project.GetProjectTable(-1);
            ComboBoxControl.Items.Add(new ListItem((0), "None"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void EnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = txtVoucherNo.Enabled = cboDepot.Enabled = cboProjectName.Enabled = cmboPurchaseAcc.Enabled = cboSeriesName.Enabled = cboCashParty.Enabled = txtRemarks.Enabled = txtDate.Enabled = txtOrderNo.Enabled = grdPurchaseReturn.Enabled = cboSeriesName.Enabled = cboDepot.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled = tabControl1.Enabled = Enable;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false, true);
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel, bool AddAccClass)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
            btnAddAccClass.Enabled = AddAccClass;

        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetAccessInfo(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }

        //Recursive Function to Show Access Level in Treeview
        private void ShowAccClassInTreeView(TreeView tv, TreeNode n, int AccClassID)
        {
            tv.Nodes.Clear();
            TreeNode temp = new TreeNode();
            temp.Text = "ROOT";
            temp.Tag = 1;
            tv.Nodes.Add(temp);
            //#region Language Management

            //tv.Font = LangMgr.GetFont();

            //string LangField = "EngName";
            //switch (LangMgr.DefaultLanguage)
            //{
            //    case Lang.English:
            //        LangField = "EngName";
            //        break;
            //    case Lang.Nepali:
            //        LangField = "NepName";
            //        break;

            //}
            //#endregion
            //if (Global.GlobalAccClassID == 1 && Global.GlobalAccessRoleID == 37)
            //{
            //    DataTable dt = new DataTable();
            //    try
            //    {
            //        dt = AccountClass.GetAccClassTable(AccClassID);
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Msg(ex.Message);
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        DataRow dr = dt.Rows[i];

            //        TreeNode t = new TreeNode(dr[LangField].ToString());
            //        t.Tag = dr["AccClassID"].ToString();
            //        //Check if it is a parent Or if it has childs
            //        try
            //        {
            //            if (ChildCount((int)dr["AccClassID"]) > 0)
            //            {
            //                //t.IsContainer = true;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }

            //        ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //        if (n == null)
            //        {
            //            t.Checked = true;
            //            tv.Nodes.Add(t); //Primary Group
            //        }
            //        else
            //        {
            //            t.Checked = true;
            //            n.Nodes.Add(t); //Secondary Group
            //        }
            //    }
            //}
            //else
            //{

            //    DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
            //    DataRow drUserInfo = dtUserInfo.Rows[0];
            //    ArrayList AccClassChildIDs = new ArrayList();
            //    ArrayList tempParentAccClassChildIDs = new ArrayList();
            //    AccClassChildIDs.Clear();
            //    AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
            //    AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
            //    DataTable dt = new DataTable();
            //    try
            //    {
            //        dt = AccountClass.GetAccClassTable(AccClassID);
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Msg(ex.Message);
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        DataRow dr = dt.Rows[i];
            //        TreeNode t = new TreeNode(dr[LangField].ToString());
            //        t.Tag = dr["AccClassID"].ToString();
            //        tempParentAccClassChildIDs.Clear();
            //        AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
            //        //Check if it is a parent Or if it has childs
            //        try
            //        {
            //            if (ChildCount((int)dr["AccClassID"]) > 0)
            //            {
            //                //t.IsContainer = true;
            //            }

            //            foreach (int itemIDs in AccClassChildIDs)  //To check if 
            //            {
            //                if (Convert.ToInt32(t.Tag) == itemIDs)
            //                {
            //                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //                    loopCounter--;
            //                    t.Checked = true;
            //                    if (n == null)
            //                    {
            //                        tv.Nodes.Add(t); //Primary Group
            //                        return;
            //                    }
            //                    else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
            //                    {
            //                        t.Checked = true;
            //                        tv.Nodes.Add(t);
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        n.Nodes.Add(t); //Secondary Group
            //                    }
            //                }
            //                if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
            //                {
            //                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }
            //}
            //#region code not in  use
            ////DataTable dt = new DataTable();
            ////try
            ////{
            ////    dt = AccountClass.GetAccClassTable(AccClassID);
            ////}
            ////catch (Exception ex)
            ////{
            ////    Global.Msg(ex.Message);
            ////}

            ////for (int i = 0; i < dt.Rows.Count; i++)
            ////{
            ////    DataRow dr = dt.Rows[i];

            ////    TreeNode t = new TreeNode(dr[LangField].ToString());
            ////    t.Tag = dr["AccClassID"].ToString();
            ////    //Check if it is a parent Or if it has childs
            ////    try
            ////    {
            ////        if (ChildCount((int)dr["AccClassID"]) > 0)
            ////        {
            ////            //t.IsContainer = true;
            ////        }
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        MessageBox.Show(ex.Message);
            ////    }

            ////    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            ////    if (n == null)
            ////    {
            ////        tv.Nodes.Add(t); //Primary Group
            ////    }
            ////    else
            ////    {
            ////        n.Nodes.Add(t); //Secondary Group
            ////    }
            ////}
            ////Insert the tag on the selected node to carry AccClassID
            //#endregion
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdPurchaseReturn.RowsCount - 2)
                grdPurchaseReturn.Rows.Remove(ctx.Position.Row);
        }

        /// <summary>
        /// Sums up all NetAmount
        /// </summary>

        private void CalculateNetAmount()
        {
            try
            {
                NetAmount = 0;
                for (int i = 1; i < grdPurchaseReturn.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdPurchaseReturn.Rows.Count)
                        return;
                    double m_NetAmount = 0;
                    string m_Value = Convert.ToString(grdPurchaseReturn[i, (int)GridColumn.NetAmount].Value);
                    if (m_Value.Length == 0)
                        m_NetAmount = 0;
                    else
                        m_NetAmount = Convert.ToDouble(grdPurchaseReturn[i, (int)GridColumn.NetAmount].Value);

                    NetAmount += m_NetAmount;
                    double VatAmt = 0;

                    //VatAmt = (NetAmount * 13) / 100;
                    // lblVat.Text = VatAmt.ToString();
                    lblnetamount.Text = NetAmount.ToString();
                    string Vatvalue = Settings.GetSettings("DEFAULT_SALES_VAT");
                    // int productid = Convert.ToInt32(grdSalesInvoice[i, 11].Value);
                    string pname = grdPurchaseReturn[i, 3].Value.ToString();
                    int productid = Sales.GetProductIDFromName(pname, Lang.English);
                    DataTable dtcheckvatapplicable = Product.GetProductByID(productid);
                    DataRow drcheckvatapplicable = dtcheckvatapplicable.Rows[0];

                    if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1")
                    {
                        if (Vatvalue == "1")
                        {
                            VatAmt = (NetAmount * VAT) / 100;
                            lblVat.Text = VatAmt.ToString();

                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Global.MsgError("Error in NetAmount calucation!");
            }
        }

        /// <summary>
        /// Sums up all Gross Amount
        /// </summary>
        private void CalculateGrossAmount()
        {
            try
            {
                GrossAmount = 0;
                for (int i = 1; i < grdPurchaseReturn.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdPurchaseReturn.Rows.Count)
                        return;
                    double m_GrossAmount = 0;
                    string m_Value = Convert.ToString(grdPurchaseReturn[i, (int)GridColumn.Amount].Value);
                    if (m_Value.Length == 0)
                        m_GrossAmount = 0;
                    else
                        m_GrossAmount = Convert.ToDouble(grdPurchaseReturn[i, (int)GridColumn.Amount].Value);

                    GrossAmount += m_GrossAmount;
                    lblGross.Text = GrossAmount.ToString();

                }
            }
            catch (Exception ex)
            {

                Global.MsgError("Error in Gross Amount calucation!");
            }
        }

        /// <summary>
        /// Sums up all Quantity
        /// </summary>
        /// 
        private void CalculateTotalQuantity()
        {
            try
            {
                double TotalQuantity = 0;
                for (int i = 1; i < grdPurchaseReturn.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseReturn.Rows.Count)
                        return;
                    double m_TotalQuantity = 0;
                    string m_Value = Convert.ToString(grdPurchaseReturn[i, (int)GridColumn.Qty].Value);
                    if (m_Value.Length == 0)
                        m_TotalQuantity = 0;
                    else
                        m_TotalQuantity = Convert.ToDouble(grdPurchaseReturn[i, (int)GridColumn.Qty].Value);

                    TotalQuantity += m_TotalQuantity;

                    lblTotalQty.Text = TotalQuantity.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                //Global.MsgError("Error in Total Quantity calucation!"); 
            }
        }

        private void ClearNew(int RowCount)
        {
            ////if (grdProduct[RowCount, 2].Value == "(NEW)")
            ////    grdProduct[RowCount, 2].Value = "";
            //if (grdProduct[RowCount, 3].Value == "(NEW)")
            //    grdProduct[RowCount, 3].Value = "";
            //if (grdProduct[RowCount, 4].Value == "(NEW)")
            //    grdProduct[RowCount, 4].Value = "";
            //if (grdProduct[RowCount, 5].Value == "(NEW)")
            //    grdProduct[RowCount, 5].Value = "";
            //if (grdProduct[RowCount, 6].Value == "(NEW)")
            //    grdProduct[RowCount, 6].Value = "";
            //if (grdProduct[RowCount, 7].Value == "(NEW)")
            //    grdProduct[RowCount, 7].Value = "";
        }
        /// <summary>
        /// Sum up all discount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateTotalDiscount()
        {
            try
            {
                Double TotalDiscount = 0;
                for (int i = 1; i < grdPurchaseReturn.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseReturn.Rows.Count)
                        return;
                    double m_TotalDiscount = 0;
                    string m_Value = Convert.ToString(grdPurchaseReturn[i, (int)GridColumn.SplDisc].Value);
                    if (m_Value.Length == 0)
                        m_TotalDiscount = 0;
                    else
                        m_TotalDiscount = Convert.ToDouble(grdPurchaseReturn[i, (int)GridColumn.SplDisc].Value);

                    TotalDiscount += m_TotalDiscount;

                    lblSpecialDiscount.Text = TotalDiscount.ToString();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in Total Discount calucation!");
            }
        }

        private void ProductCode_Selected(object sender, EventArgs e)
        {
            #region Language Management


            string LangField = "English";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;

            }
            #endregion
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
            int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
            DataTable dt = Product.GetProductByCode(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdPurchaseReturn[(CurRow), (int)GridColumn.ProductName].Value = dr[LangField].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdPurchaseReturn[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grdPurchaseReturn[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdPurchaseReturn.RowsCount;
            string LastProductCell = (string)grdPurchaseReturn[RowsCount - 1, 3].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                //AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
            WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit
            WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
            CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
            CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
            CalculateTotalQuantity();
        }

        private void Product_Selected(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            //DataTable dt = Product.GetProductByName(ct.DisplayText);
            DataTable dt = Product.GetProductByName(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdPurchaseReturn[(CurRow), (int)GridColumn.Code_No].Value = dr["ProductCode"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdPurchaseReturn[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grdPurchaseReturn[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdPurchaseReturn.RowsCount;
            string LastProductCell = (string)grdPurchaseReturn[RowsCount - 1, 3].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                //AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
            WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit
            WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
            CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
            CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
            CalculateTotalQuantity();
        }

        private void WriteAmount(int CurRow, double Qty)
        {

            try
            {
                //string SalesRate = grdPurchaseReturn[CurRow, (int)GridColumn.PurchaseRate].Value.ToString();
                object purchRate = grdPurchaseReturn[CurRow, (int)GridColumn.PurchaseRate].Value;

                string SalesRate = (purchRate != null) ? purchRate.ToString() : "0";
                double m_SalesRate = 0;
                try
                {
                    m_SalesRate = Convert.ToDouble(SalesRate);
                }
                catch //(Exception)
                {
                }
                double Amount = Convert.ToDouble(Qty) * m_SalesRate;
                grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void WriteNetAmount(int CurRow)
        {
            string Amount = grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value.ToString();
            string Discount = grdPurchaseReturn[CurRow, (int)GridColumn.SplDisc].Value.ToString();
            double NetAmount = Convert.ToDouble(Amount) - Convert.ToDouble(Discount);
            grdPurchaseReturn[CurRow, (int)GridColumn.NetAmount].Value = NetAmount.ToString();
        }

        /// <summary>
        /// Check whether the Current Row is a new row or not
        /// </summary>
        /// <param name="CurRow"></param>
        /// <returns></returns>
        /// 

        private bool isNewRow(int CurRow)
        {
            if (grdPurchaseReturn[CurRow, (int)GridColumn.Code_No].Value == "(NEW)")// || grdPurchaseReturn[CurRow, (int)GridColumn.Code_No].Value == null)
                return true;
            else
                return false;
        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {

                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                //find the current row of source grid
                int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                int RowCount = grdPurchaseReturn.RowsCount;
                object Qty = ((TextBox)sender).Text;
                //bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?
                //if (IsInt == false)
                //{
                //    Global.MsgError("The quantity you posted is invalid! Please post the integer value");
                //    grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value = "";
                //    grdPurchaseReturn[CurRow, (int)GridColumn.Qty].Value = "1";
                //    return;
                //}

                //Check whether the value of quantity is zero or not?
                if (Convert.ToDouble(Qty) == 0)
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    grdPurchaseReturn[CurRow, (int)GridColumn.Qty].Value = "1";
                    grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value = "0";
                    ((TextBox)sender).Text = "1";
                    return;
                }
                WriteAmount(CurRow, Convert.ToDouble(Qty));
                CalculateGrossAmount();

                //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalQuantity();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This function help to edit the Rate.If anyone want to insert the rate according to his/her choice then this fuction does work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void PurchaseRate_Modified(object sender, EventArgs e)
        {
            try
            {

                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                //find the current row of source grid
                int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdPurchaseReturn.RowsCount;
                object PurchaseRate = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(PurchaseRate);//This function check whether variable is Double  or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Purchase Rate you posted is invalid! Please post the integer value");
                    grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value = "";
                    grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value = "1";
                    return;
                }

                string Qty = grdPurchaseReturn[CurRow, (int)GridColumn.Qty].Value.ToString();
                double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);
                grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
                CalculateGrossAmount();
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalQuantity();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void GrossAmount_Modified(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            if (isNewRow(CurRow))
                return;


        }

        private void NetAmount_Modified(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }


        }

        private void DiscPercentage_Modified(object sender, EventArgs e)
        {
            try
            {

                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
                if (isNewRow(CurRow))
                    return;
                double Amount = 0;
                Amount = Convert.ToDouble(grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value);
                object DisPercentage = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(DisPercentage);//This function check whether variable is double or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Discount Percentage you posted is invalid! Please post the  numeric value");
                    return;
                }
                double Discount = Math.Round(((Amount * Convert.ToDouble(DisPercentage)) / 100), Global.DecimalPlaces);

                //double NetAmount = Math.Round((Amount - Discount), Global.DecimalPlaces);


                grdPurchaseReturn[(CurRow), (int)GridColumn.SplDisc].Value = Discount.ToString();
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalDiscount();

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void Discount_Modified(object sender, EventArgs e)
        {

            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            if (isNewRow(CurRow))
                return;
            double Amount = 0;
            Amount = Convert.ToDouble(grdPurchaseReturn[CurRow, (int)GridColumn.Amount].Value);
            if (Amount == 0)
            {
                Global.MsgError("The Amount shouldnot be Zero. Please post the required value in corresponding field!");
                grdPurchaseReturn[CurRow, (int)GridColumn.SplDisc_Percent].Value = "0";
                grdPurchaseReturn[CurRow, (int)GridColumn.SplDisc].Value = "0";
                grdPurchaseReturn[CurRow, (int)GridColumn.NetAmount].Value = "0";
                return;
            }
            object Discount = ((TextBox)sender).Text;
            bool IsDouble = Misc.IsNumeric(Discount);//This function check whether variable is double or not?
            if (IsDouble == false)
            {
                Global.MsgError("The Discount Amount you posted is invalid! Please post the  numeric value");
                return;
            }
            double DiscPercentage = Math.Round(((Convert.ToDouble(Discount) * 100) / Amount), Global.DecimalPlaces);
            //double NetAmount = Math.Round((Amount - Convert.ToDouble(Discount)), Global.DecimalPlaces);

            grdPurchaseReturn[(CurRow), (int)GridColumn.SplDisc_Percent].Value = DiscPercentage.ToString();
            WriteNetAmount(CurRow);
            CalculateNetAmount();
            CalculateTotalDiscount();
        }

        private void AddGridHeader()
        {
            grdPurchaseReturn[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdPurchaseReturn[0, (int)GridColumn.Del].Column.Width = 25;

            grdPurchaseReturn[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");

            grdPurchaseReturn[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grdPurchaseReturn[0, (int)GridColumn.Code_No].Column.Width = 100;

            grdPurchaseReturn[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdPurchaseReturn[0, (int)GridColumn.ProductName].Column.Width = 120;

            grdPurchaseReturn[0, (int)GridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");
            grdPurchaseReturn[0, (int)GridColumn.Qty].Column.Width = 100;

            grdPurchaseReturn[0, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.ColumnHeader("PurchaseRate");
            grdPurchaseReturn[0, (int)GridColumn.PurchaseRate].Column.Width = 150;

            grdPurchaseReturn[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdPurchaseReturn[0, (int)GridColumn.Amount].Column.Width = 150;

            grdPurchaseReturn[0, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.ColumnHeader("Spl. Disc%");
            grdPurchaseReturn[0, (int)GridColumn.SplDisc_Percent].Column.Width = 150;

            grdPurchaseReturn[0, (int)GridColumn.SplDisc] = new SourceGrid.Cells.ColumnHeader("Spl. Disc");
            grdPurchaseReturn[0, (int)GridColumn.SplDisc].Column.Width = 100;

            grdPurchaseReturn[0, (int)GridColumn.NetAmount] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdPurchaseReturn[0, (int)GridColumn.NetAmount].Column.Width = 150;

            grdPurchaseReturn[0, (int)GridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("ProductID");
            grdPurchaseReturn[0, (int)GridColumn.ProductID].Column.Width = 15;
            grdPurchaseReturn[0, (int)GridColumn.ProductID].Column.Visible = false;
        }

        private void AddRowProduct(int RowCount)
        {
            try
            {
                //Add a new row
                grdPurchaseReturn.Redim(Convert.ToInt32(RowCount + 1), grdPurchaseReturn.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdPurchaseReturn[i, (int)GridColumn.Del] = btnDelete;
                grdPurchaseReturn[i, (int)GridColumn.Del].AddController(evtDiscount);
                grdPurchaseReturn[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdPurchaseReturn[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();
                grdPurchaseReturn[i, (int)GridColumn.Code_No].Value = "(NEW)";
                //SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProductCode = new SourceGrid.Cells.Editors.ComboBox(typeof(string));

                //DataTable dt = Product.GetProductList(0);
                DataTable dtProductInfo = Product.GetProductList(0);
                List<string> lstProduct = new List<string>();
                List<string> lstProductCode = new List<string>();
                #region Language Management


                string LangField = "English";
                switch (LangMgr.DefaultLanguage)
                {
                    case Lang.English:
                        LangField = "EngName";
                        break;
                    case Lang.Nepali:
                        LangField = "NepName";
                        break;

                }
                #endregion

                //List<string> lstProduct = new List<string>();
                for (int i1 = 0; i1 < dtProductInfo.Rows.Count; i1++)
                {
                    DataRow dr = dtProductInfo.Rows[i1];
                    lstProduct.Add(dr[LangField].ToString());
                    lstProductCode.Add(dr["ProductCode"].ToString());
                }
                cboProduct.StandardValues = (string[])lstProduct.ToArray<string>();
                cboProduct.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProduct.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProduct.Control.LostFocus += new EventHandler(Product_Selected);


                cboProduct.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseReturn[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", cboProduct);
                grdPurchaseReturn[i, (int)GridColumn.ProductName].AddController(evtProduct);
                grdPurchaseReturn[i, (int)GridColumn.ProductName].Value = "(NEW)";

                //for code
                cboProductCode.StandardValues = (string[])lstProductCode.ToArray<string>();
                cboProductCode.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProductCode.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProductCode.Control.LostFocus += new EventHandler(ProductCode_Selected);

                cboProductCode.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseReturn[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell("", cboProductCode);
                grdPurchaseReturn[i, (int)GridColumn.Code_No].AddController(evtProductCode);
                grdPurchaseReturn[i, (int)GridColumn.Code_No].Value = "(NEW)";

                //for Gross Amount


                grdPurchaseReturn[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("");
                grdPurchaseReturn[i, (int)GridColumn.Amount].AddController(evtGrossAmt);
                grdPurchaseReturn[i, (int)GridColumn.Amount].Value = "(NEW)";


                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseReturn[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("", txtQty);
                grdPurchaseReturn[i, (int)GridColumn.Qty].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseReturn[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                grdPurchaseReturn[i, (int)GridColumn.PurchaseRate].Value = "(NEW)";

                grdPurchaseReturn[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                grdPurchaseReturn[i, (int)GridColumn.Amount].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);

                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";
                grdPurchaseReturn[i, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdPurchaseReturn[i, (int)GridColumn.SplDisc_Percent].Value = "0";

                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseReturn[i, (int)GridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdPurchaseReturn[i, (int)GridColumn.SplDisc].Value = "0";

                grdPurchaseReturn[i, (int)GridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdPurchaseReturn[i, (int)GridColumn.NetAmount].Value = "0";

                grdPurchaseReturn[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("ID");
                grdPurchaseReturn[i, (int)GridColumn.ProductID].Value = "0";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void frmPurchaseReturn_Load(object sender, EventArgs e)
        {
            chkDoNotClose.Checked = true;
            //for displying slabs on Label
            DataTable dtSlabInfo = Slabs.GetSlabInfo(SlabType.None);//Getting all the info of Slabs
            foreach (DataRow drSlabInfo in dtSlabInfo.Rows)
            {
                if (drSlabInfo["Code"].ToString() == "TAX1")
                {
                    lblPurchaseTax1.Text = drSlabInfo["Name"].ToString();
                }

                if (drSlabInfo["Code"].ToString() == "TAX2")
                {
                    lblPurchaseTax2.Text = drSlabInfo["Name"].ToString();
                }

                if (drSlabInfo["Code"].ToString() == "TAX3")
                {
                    lblPurchaseTax3.Text = drSlabInfo["Name"].ToString();
                }

                if (drSlabInfo["Code"].ToString() == "VAT")
                {
                    label7.Text = drSlabInfo["Name"].ToString();
                }

            }

            double m_Tax1, m_Tax2, m_Tax3, m_Vat;
            m_Tax1 = m_Tax2 = m_Tax3 = m_Vat = 0;
            //Displaying Slabs according to Settings
            if (Global.Default_Purchase_Tax1Check == "1")
            {
                lblPurchaseTax1.Visible = true;
                txtPurchaseTax1.Visible = true;
            }
            if (Global.Default_Purchase_Tax1Check == "0")
            {
                lblPurchaseTax1.Visible = false;
                txtPurchaseTax1.Visible = false;
            }
            if (Global.Default_Purchase_Tax2Check == "1")
            {
                lblPurchaseTax2.Visible = true;
                txtPurchaseTax2.Visible = true;
            }
            if (Global.Default_Purchase_Tax2Check == "0")
            {
                lblPurchaseTax2.Visible = false;
                txtPurchaseTax2.Visible = false;
            }
            if (Global.Default_Purchase_Tax3Check == "1")
            {
                lblPurchaseTax3.Visible = true;
                txtPurchaseTax3.Visible = true;
            }
            if (Global.Default_Purchase_Tax3Check == "0")
            {
                lblPurchaseTax3.Visible = false;
                txtPurchaseTax3.Visible = false;
            }
            //ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            ChangeState(EntryMode.NEW);
            txtPurchaseReturnID.Visible = false;
            ShowAccClassInTreeView(treeAccClass, null, 0);

            #region BLOCK FOR SHOWING SERIES NAME IN COMBOBOX
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("PURCH_RTN");
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                // DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            cboSeriesName.SelectedIndex = 0;//to display combobox values 
            #endregion

            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            foreach (DataRow dr in dtDepotInfo.Rows)
            {
                cboDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboDepot.SelectedIndex = 0;
            #endregion
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
            OptionalFields();
            try
            {
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                //Event trigerred when delete button is clicked
                evtDiscount.Click += new EventHandler(Delete_Row_Click);
                //Event when account is selected
                evtProduct.FocusLeft += new EventHandler(Product_Selected);
                //Event when Quntity is selected
                evtProductCode.FocusLeft += new EventHandler(ProductCode_Selected);

                evtQty.FocusLeft += new EventHandler(Qty_Modified);
                //Event when Rate is selected
                evtRate.FocusLeft += new EventHandler(PurchaseRate_Modified);

                //Event when DiscPercentage is selected
                evtDiscPercentage.FocusLeft += new EventHandler(DiscPercentage_Modified);

                //Event when Discount is selected
                evtDiscount.FocusLeft += new EventHandler(Qty_Modified);

                //evtProductFocusLost.FocusLeft += new EventHandler(evtProductFocusLost_FocusLeft);

                evtGrossAmt.ValueChanged += new EventHandler(GrossAmount_Modified);

                evtNetAmt.ValueChanged += new EventHandler(NetAmount_Modified);


                #region BLOCK FOR SHOWING PURCHASE LEDGER IN COMBOBOX
                ////cmboSalesAcc.Items.Add(new ListItem(0, "Choose Sales Ledger"));//For "All" item of combobox,pass 0 ID manually
                //Displaying the all ledgers associated with Purchase AccountGroup in DropDownList
                int Purchase_ID = AccountGroup.GetGroupIDFromGroupNumber(143);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultPurchaseAcc Account according to user root or other users
                int DefaultPurchaseAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_PURCHASE_ACCOUNT") : UserPreference.GetValue("DEFAULT_PURCHASE_ACCOUNT", uid));
                string DefaultPurchaseName = "";

                //Add PurchaseAcc to comboPurchaseAccount
                DataTable dtPurchaseLedgers = Ledger.GetAllLedger(Purchase_ID);
                foreach (DataRow drPurchaseLedgers in dtPurchaseLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cmboPurchaseAcc.Items.Add(new ListItem((int)drPurchaseLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drPurchaseLedgers["LedgerID"]) == DefaultPurchaseAccNum)
                        DefaultPurchaseName = drLedgerInfo["LedName"].ToString();
                }
                cmboPurchaseAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cmboPurchaseAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cmboPurchaseAcc.Text = DefaultPurchaseName;
                #endregion

                #region BLOCK FOR SHOWING THE LEDGERS OF CASH IN HAND,DEBTOR AND CREDITOR IN CASH/PARTY COMBOBOX
                //cmboCashPartyAcc.Items.Add(new ListItem(0, "Choose Cash/Party Ledger"));//At the first index of combobox show the "Choose Cash/Party Ledger"
                int Cash_In_Hand = AccountGroup.GetGroupIDFromGroupNumber(102);
                DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
                foreach (DataRow drCash_In_HandLedgers in dtCash_In_HandLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
                }
                int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
                DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
                foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
                }
                int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
                DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);

                foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
                }

                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);

                foreach (DataRow drBankLedger in dtBankLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drBankLedger["LedgerID"], drBankLedger["EngName"].ToString()));
                }

                cboCashParty.DisplayMember = "value";//This value is  for showing at Load condition
                cboCashParty.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboCashParty.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
                #endregion
                grdPurchaseReturn.Redim(2, 11);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                //AddRowProduct(1);
                AddRowProduct1(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
                if (m_PurchaseReturnID > 0)
                {
                    //Show the values in fields
                    try
                    {
                        if (m_isRecurring)
                        {
                            ChangeState(EntryMode.NEW);
                        }
                        else
                            ChangeState(EntryMode.NORMAL);
                        int vouchID = 0;
                        try
                        {
                            vouchID = m_PurchaseReturnID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }
                        PurchaseReturn m_PurchaseReturn = new PurchaseReturn();
                        //Getting the value of SeriesID via MasterID or VouchID
                        int SeriesIDD = m_PurchaseReturn.GetSeriesIDFromMasterID(vouchID);
                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Purchase Return");
                            cboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();

                        }
                        DataTable dtPurchaseReturnMaster = m_PurchaseReturn.GetPurchaseReturnMasterInfo(vouchID.ToString());

                        if (dtPurchaseReturnMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        foreach (DataRow drPurchaseReturnMaster in dtPurchaseReturnMaster.Rows)
                        {
                            // if the form is recurring then donot load voucher number
                            if (!m_isRecurring)
                            {
                                txtVoucherNo.Text = drPurchaseReturnMaster["Voucher_No"].ToString();
                                txtDate.Text = Date.DBToSystem(drPurchaseReturnMaster["PurchaseReturn_Date"].ToString());
                            }
                            else
                            {
                                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                                //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                            }

                            //txtVoucherNo.Text = drPurchaseReturnMaster["Voucher_No"].ToString();
                            //txtDate.Text = Date.DBToSystem(drPurchaseReturnMaster["PurchaseReturn_Date"].ToString());
                            txtRemarks.Text = drPurchaseReturnMaster["Remarks"].ToString();
                            txtPurchaseReturnID.Text = drPurchaseReturnMaster["PurchaseReturnID"].ToString();
                            txtOrderNo.Text = drPurchaseReturnMaster["Order_No"].ToString();
                            lblNetAmout.Text = drPurchaseReturnMaster["Net_Amount"].ToString();
                            txtPurchaseTax1.Text = drPurchaseReturnMaster["Tax1"].ToString();
                            txtPurchaseTax2.Text = drPurchaseReturnMaster["Tax2"].ToString();
                            txtPurchaseTax3.Text = drPurchaseReturnMaster["Tax3"].ToString();
                            lblVat.Text = drPurchaseReturnMaster["VAT"].ToString();
                            lblTotalQty.Text = drPurchaseReturnMaster["Total_Qty"].ToString();
                            lblSpecialDiscount.Text = drPurchaseReturnMaster["SpecialDiscount"].ToString();
                            lblGross.Text = drPurchaseReturnMaster["Gross_Amount"].ToString();
                            lblnetamount.Text = drPurchaseReturnMaster["Net_Amount"].ToString();

                            lblAdjustment.Text = drPurchaseReturnMaster["Total_Amt"].ToString();
                            //For Additional Fields
                            if (NumberOfFields > 0)
                            {
                                if (NumberOfFields == 1)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = false;
                                    txtsecond.Visible = false;
                                    lblthird.Visible = false;
                                    txtthird.Visible = false;
                                    lblfourth.Visible = false;
                                    txtfourth.Visible = false;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();

                                    txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();
                                }
                                else if (NumberOfFields == 2)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = false;
                                    txtthird.Visible = false;
                                    lblfourth.Visible = false;
                                    txtfourth.Visible = false;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                    txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();
                                }
                                else if (NumberOfFields == 3)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = true;
                                    txtthird.Visible = true;
                                    lblfourth.Visible = false;
                                    txtfourth.Visible = false;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                    lblthird.Text = drdtadditionalfield["Field3"].ToString();

                                    txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();

                                }
                                else if (NumberOfFields == 4)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = true;
                                    txtthird.Visible = true;
                                    lblfourth.Visible = true;
                                    txtfourth.Visible = true;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                                    txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();

                                }
                                else if (NumberOfFields == 5)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = true;
                                    txtthird.Visible = true;
                                    lblfourth.Visible = true;
                                    txtfourth.Visible = true;
                                    lblfifth.Visible = true;
                                    txtfifth.Visible = true;

                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                                    lblfifth.Text = drdtadditionalfield["Field5"].ToString();

                                    txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();
                                }


                            }

                            dsPurchaseReturn.Tables["tblPurchaseReturnMaster"].Rows.Add(cboSeriesName.Text, drPurchaseReturnMaster["Voucher_No"].ToString(), Date.DBToSystem(drPurchaseReturnMaster["PurchaseReturn_Date"].ToString()), cboDepot.Text, drPurchaseReturnMaster["Order_No"].ToString(), cboCashParty.Text, cmboPurchaseAcc.Text, drPurchaseReturnMaster["Remarks"].ToString());
                            DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseReturnMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];
                            cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();

                            //Show the corresponding Purchase Account Ledger in Combobox
                            DataTable dtPurchaseLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseReturnMaster["PurchaseLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drPurchaseLedgerInfo = dtPurchaseLedgerInfo.Rows[0];
                            cmboPurchaseAcc.Text = drPurchaseLedgerInfo["LedName"].ToString();

                            //DataTable dtDepotDtlInfo = Depot.GetDepotInfo(Convert.ToInt32(drPurchaseReturnMaster["DepotID"].ToString()));
                            //foreach (DataRow drDepotInfo in dtDepotDtlInfo.Rows)
                            //{
                            //    cboDepot.Text = drDepotInfo["DepotName"].ToString();
                            //}
                        }

                        DataTable dtPurchaseReturnDetails = m_PurchaseReturn.GetPurchaseReturnDetails(vouchID);
                        for (int i = 1; i <= dtPurchaseReturnDetails.Rows.Count; i++)
                        {
                            DataRow drDetail = dtPurchaseReturnDetails.Rows[i - 1];
                            grdPurchaseReturn[i, (int)GridColumn.SNo].Value = i.ToString();
                            grdPurchaseReturn[i, (int)GridColumn.Code_No].Value = drDetail["Code"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.PurchaseRate].Value = drDetail["PurchaseRate"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.Amount].Value = drDetail["Amount"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.SplDisc_Percent].Value = drDetail["DiscPercentage"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.SplDisc].Value = drDetail["Discount"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.NetAmount].Value = drDetail["Net_Amount"].ToString();
                            grdPurchaseReturn[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();

                            AddRowProduct1(grdPurchaseReturn.RowsCount);
                            dsPurchaseReturn.Tables["tblPurchaseReturnDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Quantity"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());

                        }
                        // if recurring is true then donot load recurring settings for new voucher
                        if (!m_isRecurring)
                            CheckRecurringSetting(txtPurchaseReturnID.Text);
                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE
            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                {
                    //Enter in this block only when VoucherNumberingType is Manual
                    //Checking for Manual VoucherNumberingType
                    try
                    {
                        string returnStr = m_VouConfig.ValidateManualVouNum(txtVoucherNo.Text, Convert.ToInt32(SeriesID.ID));
                        switch (returnStr)
                        {
                            case "INVALID_SERIES":
                                {
                                    MessageBox.Show("Invalid Series Name,please select valid Series Name and try again!");
                                    return;
                                }
                                break;
                            case "BLANK_WARN":
                                if (MessageBox.Show("Voucher Number is Blank, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "BLANK_DONT_ALLOW":
                                MessageBox.Show("Voucher Number is Blank,Please fill the Voucher Number first!");
                                return;
                                break;
                            case "SUCCESS":

                                break;
                            case "DUPLICATE_WARN":
                                if (MessageBox.Show("Voucher Number is Duplicated, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "DUPLICATE_DONT_ALLOW":
                                {
                                    MessageBox.Show("Voucher Number is Duplicated,Please insert the unique Voucher Number");
                                    return;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                        Global.Msg(ex.Message);
                    }
                }
            }
            #endregion

            //Check Validation
            if (!Validate())
                return;
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (txtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (txtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (txtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (txtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (txtfifth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field5"].ToString() + " " + "is Required Field");
                    return;
                }
            }

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed

                    #region Add voucher number if voucher number is automatic and hidden from the setting
                    int increasedSeriesNum = 0;
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                    if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            return;
                        }

                        txtVoucherNo.Text = m_vounum.ToString();
                        txtVoucherNo.Enabled = false;
                    }
                    #endregion

                    isNew = true;
                    OldGrid = " ";
                    NewGrid = " ";
                    NewGrid = NewGrid + "Voucher No" + txtVoucherNo + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "Depot" + cboDepot.Text + "OrderNo" + txtOrderNo.Text;
                    //Collect the Contents of the grid for audit log
                    for (int i = 0; i < grdPurchaseReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string productname = grdPurchaseReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                        string qty = grdPurchaseReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                        string rate = grdPurchaseReturn[i + 1, (int)GridColumn.PurchaseRate].Value.ToString();
                        string amt = grdPurchaseReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                        OldGrid = OldGrid + string.Concat(productname, qty, rate, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable PurchaseReturnDetails = new DataTable();
                        PurchaseReturnDetails.Columns.Add("Code");
                        PurchaseReturnDetails.Columns.Add("Product");
                        PurchaseReturnDetails.Columns.Add("Quantity");
                        PurchaseReturnDetails.Columns.Add("PurchaseRate");
                        PurchaseReturnDetails.Columns.Add("Amount");
                        PurchaseReturnDetails.Columns.Add("DiscPercentage");
                        PurchaseReturnDetails.Columns.Add("Discount");
                        PurchaseReturnDetails.Columns.Add("Net_Amount");
                        PurchaseReturnDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grdPurchaseReturn.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            PurchaseReturnDetails.Rows.Add(grdPurchaseReturn[i + 1, (int)GridColumn.Code_No].Value, grdPurchaseReturn[i + 1, (int)GridColumn.ProductName].Value, grdPurchaseReturn[i + 1, (int)GridColumn.Qty].Value, grdPurchaseReturn[i + 1, (int)GridColumn.PurchaseRate].Value, grdPurchaseReturn[i + 1, (int)GridColumn.Amount].Value, grdPurchaseReturn[i + 1, (int)GridColumn.SplDisc_Percent].Value, grdPurchaseReturn[i + 1, (int)GridColumn.SplDisc].Value, grdPurchaseReturn[i + 1, (int)GridColumn.NetAmount].Value, grdPurchaseReturn[i + 1, (int)GridColumn.ProductID].Value);
                        }
                        DateTime PurchaseReturn_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liPurchaseLedgerID = new ListItem();
                        liPurchaseLedgerID = (ListItem)cmboPurchaseAcc.SelectedItem;

                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;

                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(25717);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(25718);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(25719);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(4698);
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (txtPurchaseTax1.Text == "")
                        {
                            txtPurchaseTax1.Text = Convert.ToDouble(0.000).ToString();
                        }
                        if (txtPurchaseTax2.Text == "")
                        {
                            txtPurchaseTax2.Text = Convert.ToDouble(0.000).ToString();
                        }
                        if (txtPurchaseTax3.Text == "")
                        {
                            txtPurchaseTax3.Text = Convert.ToDouble(0.000).ToString();
                        }


                        if (AccClassID.Count != 0)
                        {
                            m_PurchaseReturn.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), liPurchaseLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, PurchaseReturn_Date, txtRemarks.Text, PurchaseReturnDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, OldGrid, NewGrid, isNew, txtPurchaseTax1.Text, txtPurchaseTax2.Text, txtPurchaseTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblnetamount.Text, OF, m_dtRecurringSetting);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_PurchaseReturn.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), liPurchaseLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, PurchaseReturn_Date, txtRemarks.Text, PurchaseReturnDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, OldGrid, NewGrid, isNew, txtPurchaseTax1.Text, txtPurchaseTax2.Text, txtPurchaseTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblnetamount.Text, OF, m_dtRecurringSetting);
                        }

                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Purchase Return created successfully!");

                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_PurchaseReturnID, "PURCHASE_RETURN");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }
                        AccClassID.Clear();
                        ChangeState(EntryMode.NORMAL);

                        ////Do not close the form if do not close is checked
                        if (!chkDoNotClose.Checked)
                            this.Close();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
                        switch (ex.Number)
                        {
                            case 4060: // Invalid Database 
                                Global.Msg("Invalid Database", MBType.Error, "Error");
                                break;

                            case 18456: // Login Failed 
                                Global.Msg("Login Failed!", MBType.Error, "Error");
                                break;

                            case 547: // ForeignKey Violation , Check Constraint
                                Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                break;

                            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                Global.Msg("ERROR: The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                break;

                            case 2601: // Unique Index/Constriant Violation 
                                Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                break;

                            case 5000: //Trigger violation
                                Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                break;

                            default:
                                Global.MsgError(ex.Message);
                                break;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError("Please Insert All the Requirements such as Gov Taxes" + " " + ex.Message);
                    }

                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT: //if new button is pressed
                    isNew = false;
                    NewGrid = " ";
                    NewGrid = NewGrid + "Voucher No" + txtVoucherNo + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "Depot" + cboDepot.Text + "OrderNo" + txtOrderNo.Text;
                    //Collect the Contents of the grid for audit log
                    for (int i = 0; i < grdPurchaseReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string productname = grdPurchaseReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                        string qty = grdPurchaseReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                        string rate = grdPurchaseReturn[i + 1, (int)GridColumn.PurchaseRate].Value.ToString();
                        string amt = grdPurchaseReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                        OldGrid = OldGrid + string.Concat(productname, qty, rate, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable PurchaseReturnDetails = new DataTable();
                        PurchaseReturnDetails.Columns.Add("Code");
                        PurchaseReturnDetails.Columns.Add("Product");
                        PurchaseReturnDetails.Columns.Add("Quantity");
                        PurchaseReturnDetails.Columns.Add("PurchaseRate");
                        PurchaseReturnDetails.Columns.Add("Amount");
                        PurchaseReturnDetails.Columns.Add("DiscPercentage");
                        PurchaseReturnDetails.Columns.Add("Discount");
                        PurchaseReturnDetails.Columns.Add("NetAmount");
                        PurchaseReturnDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grdPurchaseReturn.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            PurchaseReturnDetails.Rows.Add(grdPurchaseReturn[i + 1, (int)GridColumn.Code_No].Value, grdPurchaseReturn[i + 1, (int)GridColumn.ProductName].Value, grdPurchaseReturn[i + 1, (int)GridColumn.Qty].Value, grdPurchaseReturn[i + 1, (int)GridColumn.PurchaseRate].Value, grdPurchaseReturn[i + 1, (int)GridColumn.Amount].Value, grdPurchaseReturn[i + 1, (int)GridColumn.SplDisc_Percent].Value, grdPurchaseReturn[i + 1, (int)GridColumn.SplDisc].Value, grdPurchaseReturn[i + 1, (int)GridColumn.NetAmount].Value, grdPurchaseReturn[i + 1, (int)GridColumn.ProductID].Value);
                        }

                        DateTime PurchaseReturn_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liPurchaseLedgerID = new ListItem();
                        liPurchaseLedgerID = (ListItem)cmboPurchaseAcc.SelectedItem;

                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;

                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;

                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(25717);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(25718);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(25719);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(4698);
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;

                        //if (chkRecurring.Checked)
                        //{
                        //    m_dtRecurringSetting.Rows[0]["RVID"] = RSID;  // send id of voucher setting for modification
                        //    m_dtRecurringSetting.Rows[0]["VoucherID"] = txtPurchaseReturnID.Text;
                        //}

                        if (txtPurchaseTax1.Text == "")
                        {
                            txtPurchaseTax1.Text = Convert.ToDouble(0.000).ToString();
                        }
                        if (txtPurchaseTax2.Text == "")
                        {
                            txtPurchaseTax2.Text = Convert.ToDouble(0.000).ToString();
                        }
                        if (txtPurchaseTax3.Text == "")
                        {
                            txtPurchaseTax3.Text = Convert.ToDouble(0.000).ToString();
                        }
                        if (AccClassID.Count != 0)
                        {

                            m_PurchaseReturn.Modify(Convert.ToInt32(txtPurchaseReturnID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, PurchaseReturn_Date, txtRemarks.Text, PurchaseReturnDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, txtPurchaseTax1.Text, txtPurchaseTax2.Text, txtPurchaseTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblnetamount.Text, OF, m_dtRecurringSetting);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };

                            m_PurchaseReturn.Modify(Convert.ToInt32(txtPurchaseReturnID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, PurchaseReturn_Date, txtRemarks.Text, PurchaseReturnDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, txtPurchaseTax1.Text, txtPurchaseTax2.Text, txtPurchaseTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblnetamount.Text, OF, m_dtRecurringSetting);

                        }
                        Global.Msg("Purchase Return modified successfully!");
                        AccClassID.Clear();
                        ChangeState(EntryMode.NORMAL);
                        ClearVoucher();
                        btnNew_Click(sender, e);
                        ////Do not close the form if do not close is checked
                        if (checkBox2.Checked)
                        {
                            Navigation(Navigate.Last);
                            ClearVoucher();
                            ChangeState(EntryMode.NEW);
                            btnNew_Click(sender, e);
                        }

                        if (!chkDoNotClose.Checked)
                            this.Close();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
                        switch (ex.Number)
                        {
                            case 4060: // Invalid Database 
                                Global.Msg("Invalid Database", MBType.Error, "Error");
                                break;

                            case 18456: // Login Failed 
                                Global.Msg("Login Failed!", MBType.Error, "Error");
                                break;

                            case 547: // ForeignKey Violation , Check Constraint
                                Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                break;

                            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                Global.Msg("ERROR: The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                break;

                            case 2601: // Unique Index/Constriant Violation 
                                Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                break;

                            case 5000: //Trigger violation
                                Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                break;

                            default:
                                Global.MsgError(ex.Message);
                                break;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError("Please Insert All the Requirements such as Gov Taxes" + " " + ex.Message);
                    }

                    break;

                #endregion
            }
            ClearVoucher();
        }

        public ListItem liPurchaseLedgerID { get; set; }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.First);
        }

        private void ClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0;
            ClearPurchaseReturn(false);
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdPurchaseReturn.Redim(2, 11);
            AddGridHeader(); //Write header part
            //AddRowServices(1);
            AddRowProduct1(1);
            //AddRowProduct(1);
            ClearRecurringSetting();
        }

        private void ClearPurchaseReturn(bool IsVouNumFinished)
        {
            txtVoucherNo.Clear();
            //actually generate a new voucher no.
            //txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtOrderNo.Clear();
            txtRemarks.Clear();
            txtPurchaseTax1.Clear();
            txtPurchaseTax2.Clear();
            txtPurchaseTax3.Clear();
            cboDepot.Text = string.Empty;
            cmboPurchaseAcc.Text = string.Empty;
            cboSeriesName.Text = string.Empty;
            cboCashParty.Text = string.Empty;
            if (!IsVouNumFinished)
            {
                grdPurchaseReturn.Rows.Clear();
            }
        }

        private void LoadAccClassInfo(int AccClassID, TreeNode tn, int[] CheckedIDs, TreeView tvAccClass)
        {
            foreach (TreeNode nd in tn.Nodes)
            {
                nd.Checked = false; //first clear the checkmark if anything is checked previously
                LoadAccClassInfo(AccClassID, nd, CheckedIDs, tvAccClass);

                foreach (int id in CheckedIDs)
                    if (Convert.ToInt32(nd.Tag) == id)
                        nd.Checked = true;
            }
            foreach (int parentid in CheckedIDs)
            {
                if (Convert.ToInt32(tn.Tag) != parentid)
                    tn.Checked = false;
            }
        }

        private bool Navigation(Navigate NavTo)
        {
            try
            {

                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtPurchaseReturnID.Text);
                    if (PurchaseReturnIDCopy > 0)
                    {
                        VouchID = PurchaseReturnIDCopy;
                        PurchaseReturnIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtPurchaseReturnID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtPurchaseReturnMaster = m_PurchaseReturn.NavigatePurchaseReturnMaster(VouchID, NavTo);
                if (dtPurchaseReturnMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drPurchaseReturnMaster = dtPurchaseReturnMaster.Rows[0]; //There is only one row. First row is the required record
                //Show the corresponding Cash/Party Account Ledger in Combobox
                DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseReturnMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drCashPartyLedgerInfo in dtCashPartyInfo.Rows)
                {
                    cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();

                }

                //Show the corresponding Purchase Account Ledger in Combobox
                DataTable dtPurchaseLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseReturnMaster["PurchaseLedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drPurchaseLedgerInfo in dtPurchaseLedgerInfo.Rows)
                {
                    cmboPurchaseAcc.Text = drPurchaseLedgerInfo["LedName"].ToString();
                }

                //Show the corresponding Depot in control

                //DataTable dtDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drPurchaseReturnMaster["DepotID"]));
                //if (dtDepotInfo.Rows.Count > 0)
                //{
                //    DataRow drDepotInfo = dtDepotInfo.Rows[0];
                //    cboDepot.Text = drDepotInfo["DepotName"].ToString();
                //}

                //show the corresoponding SeriesName in Combobox
                DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drPurchaseReturnMaster["SeriesID"]));
                if (dtSeriesInfo.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Purchase Invoice");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dtSeriesInfo.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                lblVouNo.Visible = true;
                txtVoucherNo.Visible = true;
                txtVoucherNo.Text = drPurchaseReturnMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drPurchaseReturnMaster["PurchaseReturn_Date"].ToString());
                txtRemarks.Text = drPurchaseReturnMaster["Remarks"].ToString();
                txtPurchaseReturnID.Text = drPurchaseReturnMaster["PurchaseReturnID"].ToString();
                //For Additional Fields
                if (NumberOfFields > 0)
                {
                    if (NumberOfFields == 1)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = false;
                        txtsecond.Visible = false;
                        lblthird.Visible = false;
                        txtthird.Visible = false;
                        lblfourth.Visible = false;
                        txtfourth.Visible = false;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();

                        txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 2)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = false;
                        txtthird.Visible = false;
                        lblfourth.Visible = false;
                        txtfourth.Visible = false;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();

                        txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 3)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = true;
                        txtthird.Visible = true;
                        lblfourth.Visible = false;
                        txtfourth.Visible = false;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        lblthird.Text = drdtadditionalfield["Field3"].ToString();

                        txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 4)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = true;
                        txtthird.Visible = true;
                        lblfourth.Visible = true;
                        txtfourth.Visible = true;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        lblthird.Text = drdtadditionalfield["Field3"].ToString();
                        lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                        txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 5)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = true;
                        txtthird.Visible = true;
                        lblfourth.Visible = true;
                        txtfourth.Visible = true;
                        lblfifth.Visible = true;
                        txtfifth.Visible = true;

                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        lblthird.Text = drdtadditionalfield["Field3"].ToString();
                        lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                        lblfifth.Text = drdtadditionalfield["Field5"].ToString();

                        txtfirst.Text = drPurchaseReturnMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseReturnMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseReturnMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseReturnMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseReturnMaster["Field5"].ToString();
                    }
                }

                txtVoucherNo.Text = drPurchaseReturnMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drPurchaseReturnMaster["PurchaseReturn_Date"].ToString());
                txtRemarks.Text = drPurchaseReturnMaster["Remarks"].ToString();
                txtPurchaseReturnID.Text = drPurchaseReturnMaster["PurchaseReturnID"].ToString();
                txtOrderNo.Text = drPurchaseReturnMaster["Order_No"].ToString();



                lblSpecialDiscount.Text = drPurchaseReturnMaster["SpecialDiscount"].ToString();
                lblNetAmout.Text = drPurchaseReturnMaster["Net_Amount"].ToString();
                lblTotalQty.Text = drPurchaseReturnMaster["Total_Qty"].ToString();
                lblGross.Text = drPurchaseReturnMaster["Gross_Amount"].ToString();
                lblAdjustment.Text = drPurchaseReturnMaster["Total_Amt"].ToString();

                dsPurchaseReturn.Tables["tblPurchaseReturnMaster"].Rows.Add(cboSeriesName.Text, drPurchaseReturnMaster["Voucher_No"].ToString(), Date.DBToSystem(drPurchaseReturnMaster["PurchaseReturn_Date"].ToString()), cboDepot.Text, cboCashParty.Text, cmboPurchaseAcc.Text, txtOrderNo.Text, drPurchaseReturnMaster["Remarks"].ToString());
                DataTable dtPurchaseReturnDetails = m_PurchaseReturn.GetPurchaseReturnDetails(Convert.ToInt32(txtPurchaseReturnID.Text));
                for (int i = 1; i <= dtPurchaseReturnDetails.Rows.Count; i++)
                {
                    DataRow drDetail = dtPurchaseReturnDetails.Rows[i - 1];
                    grdPurchaseReturn[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdPurchaseReturn[i, (int)GridColumn.Code_No].Value = drDetail["Code"].ToString();
                    grdPurchaseReturn[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdPurchaseReturn[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                    grdPurchaseReturn[i, (int)GridColumn.PurchaseRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    grdPurchaseReturn[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdPurchaseReturn[i, (int)GridColumn.SplDisc_Percent].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString();
                    grdPurchaseReturn[i, (int)GridColumn.SplDisc].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString();
                    grdPurchaseReturn[i, (int)GridColumn.NetAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString();
                    grdPurchaseReturn[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();
                    AddRowProduct1(grdPurchaseReturn.RowsCount);
                    dsPurchaseReturn.Tables["tblPurchaseReturnDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                }
                // drDetail["ServicesName"].ToString()
                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtPurchaseReturnID.Text), "PURCH_RTN");
                List<int> AccClassIDs = new List<int>();
                foreach (DataRow dr in dtAccClassDtl.Rows)
                {
                    AccClassIDs.Add(Convert.ToInt32(dr["AccClassID"]));
                }

                treeAccClass.ExpandAll();

                //Check for the treeview if it has Use
                foreach (TreeNode tn in treeAccClass.Nodes)
                {
                    LoadAccClassInfo(VouchID, tn, AccClassIDs.ToArray<int>(), treeAccClass);
                    int pid = Convert.ToInt32(tn.Tag);
                    if (AccClassIDs.ToArray<int>().Contains(pid))
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
                btnExport.Enabled = true;
                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    CheckRecurringSetting(txtPurchaseReturnID.Text);
                return true;
            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
                return false;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Last);
        }

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));
                    txtVoucherNo.Enabled = true;
                    if (NumberingType == "AUTOMATIC" && !m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            // disables all controls except cboSeriesName
                            ClearPurchaseReturn(true);
                            EnableControls(false);
                            cboSeriesName.Enabled = true;
                        }
                        else
                        {
                            lblVouNo.Visible = true;
                            txtVoucherNo.Visible = true;
                            EnableControls(true);
                            txtVoucherNo.Text = m_vounum.ToString();
                            txtVoucherNo.Enabled = false;
                        }
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        lblVouNo.Visible = false;
                        txtVoucherNo.Visible = false;
                    }
                    if (m_PurchaseReturnID > 0 && !m_isRecurring)
                    {
                        lblVouNo.Visible = true;
                        txtVoucherNo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void AddRowProduct1(int RowCount)
        {
            //add new row
            try
            {

                grdPurchaseReturn.Redim(Convert.ToInt32(RowCount + 1), grdPurchaseReturn.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdPurchaseReturn[i, (int)GridColumn.Del] = btnDelete;
                grdPurchaseReturn[i, (int)GridColumn.Del].AddController(evtDelete);
                grdPurchaseReturn[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdPurchaseReturn[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();

                #region Language Management


                string LangField = "English";
                switch (LangMgr.DefaultLanguage)
                {
                    case Lang.English:
                        LangField = "EngName";
                        break;
                    case Lang.Nepali:
                        LangField = "NepName";
                        break;

                }
                #endregion
                SourceGrid.Cells.Editors.TextBox txtProduct = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtProduct.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdPurchaseReturn[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                // txtProduct.Control.LostFocus += new EventHandler(Product_Selected);
                grdPurchaseReturn[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grdPurchaseReturn[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdPurchaseReturn[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseReturn[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                // grdPurchaseInvoice[i, 5].Value = "(NEW)";

                //grdPurchaseReturn[i, 6] = new SourceGrid.Cells.Cell("");
                //grdPurchaseReturn[i, 6].AddController(evtGrossAmt);
                grdPurchaseReturn[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";

                grdPurchaseReturn[i, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdPurchaseReturn[i, (int)GridColumn.SplDisc_Percent].Value = "0";
                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseReturn[i, (int)GridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdPurchaseReturn[i, (int)GridColumn.SplDisc].Value = "0";

                grdPurchaseReturn[i, (int)GridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdPurchaseReturn[i, (int)GridColumn.NetAmount].Value = "0";

                grdPurchaseReturn[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("ProductID");
                grdPurchaseReturn[i, (int)GridColumn.ProductID].Value = "0";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Product_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmListOfProduct flp = new frmListOfProduct(this);
                flp.ShowDialog();
                SendKeys.Send("{Tab}");
            }
        }
        private void Product_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowsCount = grdPurchaseReturn.RowsCount;
            string LastServicesCell = (string)grdPurchaseReturn[RowsCount - 1, 3].Value;

            ////Check whether the new row is already added
            if (LastServicesCell != "(NEW)")
            {
                // AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
        }
        private void Product_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmListOfProduct flp = new frmListOfProduct(this, e);
            flp.ShowDialog();
        }
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;

        }
        private void evtProductFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            // FillAllGridRow(CurrRowPos);
        }
        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //CurrRowPos = ctx.Position.Row;
            if (IsSelected)
            {
                int CurRow = grdPurchaseReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // CurrRowPos = ctx.Position.Row;
                ctx.Text = productname;
                purchaserate = purchaserate;
                productcode = productcode;
                grdPurchaseReturn[CurRow, (int)GridColumn.Code_No].Value = productcode;
                grdPurchaseReturn[CurRow, (int)GridColumn.PurchaseRate].Value = purchaserate;
                grdPurchaseReturn[CurRow, (int)GridColumn.ProductID].Value = productid.ToString();
                int RowsCount = grdPurchaseReturn.RowsCount;

                string LastServicesCell = (string)grdPurchaseReturn[RowsCount - 1, 3].Value;
                ////Check whether the new row is already added
                if (LastServicesCell == "(NEW)")
                {
                    // AddRowProduct(RowsCount);
                    AddRowProduct1(RowsCount);
                    //Clear (NEW) on other colums as well
                    //ClearNew(RowsCount - 1);
                }

            }
            hasChanged = true;



        }
        private void FillAllGridRow(int RowPosition)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (PurchaseInvoiceIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    Navigation(Navigate.ID);
                    EnableControls(true);
                    ChangeState(EntryMode.NEW);
                }
                else
                {
                    Global.Msg("Please press New Button for Pasting the copied Voucher");
                }
            }
            else
            {
                Global.Msg("Please navigate to a voucher and press copy button first to specify the source voucher");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            PurchaseInvoiceIDCopy = Convert.ToInt32(txtPurchaseReturnID.Text);
        }

        private void frmPurchaseReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.N && e.Control)
            {
                btnNew_Click(sender, e);
            }
            else if (e.KeyCode == Keys.E && e.Control)
            {
                btnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                btnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {

            }
            else if (e.KeyCode == Keys.F && e.Control)
            {
                btnFirst_Click(sender, e);
            }
            else if (e.KeyCode == Keys.P && e.Shift)
            {
                btnPrev_Click(sender, e);
            }
            else if (e.KeyCode == Keys.N && e.Shift)
            {
                btnNext_Click(sender, e);
            }
            else if (e.KeyCode == Keys.L && e.Control)
            {
                btnLast_Click(sender, e);
            }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                btnCopy_Click(sender, e);
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                btnPaste_Click(sender, e);
            }
            else if (e.KeyCode == Keys.P && e.Control)
            {

            }
        }

        private void LoadComboboxProject(ComboBox ComboBoxControl, int ProjectID)
        {
            #region Language Management
            string LangField = "EngName";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            #endregion
            DataTable dt = Project.GetProjectTable(ProjectID);
            //DataTable dt1 = AccountClass.GetAccClassTable(ProjectID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], Prefix + " " + dr[LangField].ToString()));
                Prefix += "----";
                LoadComboboxProject(ComboBoxControl, Convert.ToInt16(dr["ProjectID"].ToString()));
            }
            //Prefix = "--";
            if (Prefix.Length > 1)
            {
                Prefix = Prefix.Remove(Prefix.Length - 4, 4);
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }
        private void OptionalFields()
        {
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = false;
                    txtsecond.Visible = false;
                    lblthird.Visible = false;
                    txtthird.Visible = false;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = false;
                    txtthird.Visible = false;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = true;
                    txtfourth.Visible = true;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = true;
                    txtfourth.Visible = true;
                    lblfifth.Visible = true;
                    txtfifth.Visible = true;

                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    lblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                lblfirst.Visible = false;
                txtfirst.Visible = false;
                lblsecond.Visible = false;
                txtsecond.Visible = false;
                lblthird.Visible = false;
                txtthird.Visible = false;
                lblfourth.Visible = false;
                txtfourth.Visible = false;
                lblfifth.Visible = false;
                txtfifth.Visible = false;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void PrintPreviewCR(PrintType myPrintType)
        {
            dsPurchaseReturn.Clear();
            rptPurchaseReturn rpt = new rptPurchaseReturn();
            Misc.WriteLogo(dsPurchaseReturn, "tblImage");
            rpt.SetDataSource(dsPurchaseReturn);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            int uid = User.CurrUserID;
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());

            if (roleid == 37)//if user is root, get information from tblCompany
            {
                pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Name);
                rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                // pdvCompany_Address.Value = m_CompanyDetails.Address1;
                pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
            }
            else //if user is not root, take information from tblUserPreference
            {
                string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);

                pdvCompany_Name.Value = companyname;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Name);
                rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
            pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvPreparedBy);
            rpt.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

            pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvCheckedBy);
            rpt.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

            pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvApprovedBy);
            rpt.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

            pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
            pvCollection.Clear();
            pvCollection.Add(pdvPrintDate);
            rpt.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);
            bool empty = Navigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {
                //Navigation(Navigate.ID);
                frmProgress ProgressForm = new frmProgress();
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        ProgressForm.ShowDialog();
                    }
                ));
                backgroundThread.Start();
                //Update the progressbar
                ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");
                Common.frmReportViewer frm = new Common.frmReportViewer();
                frm.SetReportSource(rpt);

                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;
                switch (prntDirect)
                {
                    case 1:
                        rpt.PrintOptions.PrinterName = "";
                        rpt.PrintToPrinter(1, false, 0, 0);
                        prntDirect = 0;
                        return;
                    case 2:
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt.Export();
                        rpt.Close();
                        return;
                    case 3:
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt.Export();
                        rpt.Close();
                        return;
                    case 4:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        rpt.Export();
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        rpt.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
                frm.WindowState = FormWindowState.Maximized;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Menu_Export = new ContextMenu();
            MenuItem mnuExcel = new MenuItem();
            mnuExcel.Name = "mnuExcel";
            mnuExcel.Text = "E&xcel";
            MenuItem mnuPDF = new MenuItem();
            mnuPDF.Name = "mnuPDF";
            mnuPDF.Text = "&PDF";
            MenuItem mnuEmail = new MenuItem();
            mnuEmail.Name = "mnuEmail";
            mnuEmail.Text = "E&mail";

            Menu_Export.MenuItems.Add(mnuExcel);
            Menu_Export.MenuItems.Add(mnuPDF);
            Menu_Export.MenuItems.Add(mnuEmail);
            Menu_Export.Show(btnExport, new Point(0, btnExport.Height));

            foreach (MenuItem Item in Menu_Export.MenuItems)
                Item.Click += new EventHandler(Menu_Click);
        }
        private void Menu_Click(object sender, EventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "mnuExcel":
                    //Code for excel export
                    SaveFileDialog SaveFD = new SaveFileDialog();
                    SaveFD.InitialDirectory = "D:";
                    SaveFD.Title = "Enter Filename:";
                    SaveFD.Filter = "*.xls|*.xls";
                    if (SaveFD.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFD.FileName;
                        FileName = SaveFD.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 2;
                    btnPrintPreview_Click(sender, e);
                    break;
                case "mnuPDF":
                    //Code for pdf export
                    SaveFileDialog SaveFDPdf = new SaveFileDialog();
                    SaveFDPdf.InitialDirectory = "D:";
                    SaveFDPdf.Title = "Enter Filename:";
                    SaveFDPdf.Filter = "*.pdf|*.pdf";
                    if (SaveFDPdf.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDPdf.FileName;
                        FileName = SaveFDPdf.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 3;
                    btnPrintPreview_Click(sender, e);
                    break;
                case "mnuEmail":
                    //Code for pdf export
                    SaveFileDialog SaveFDExcelEmail = new SaveFileDialog();
                    SaveFDExcelEmail.InitialDirectory = "D:";
                    SaveFDExcelEmail.Title = "Enter Filename:";
                    SaveFDExcelEmail.Filter = "*.xls|*.xls"; ;
                    if (SaveFDExcelEmail.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDExcelEmail.FileName;
                        FileName = SaveFDExcelEmail.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 4;
                    btnPrintPreview_Click(sender, e);
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_RETURN_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + txtVoucherNo + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "Depot" + cboDepot.Text + "OrderNo" + txtOrderNo.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdPurchaseReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdPurchaseReturn[i + 1, 3].Value.ToString();
                string qty = grdPurchaseReturn[i + 1, 4].Value.ToString();
                string rate = grdPurchaseReturn[i + 1, 5].Value.ToString();
                string amt = grdPurchaseReturn[i + 1, 6].Value.ToString();
                OldGrid = OldGrid + string.Concat(productname, qty, rate, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;
            EnableControls(true);
            ChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                txtVoucherNo.Enabled = false;
        }

        private void chkDoNotClose_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            string GridValues = "";
            GridValues = GridValues + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdPurchaseReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdPurchaseReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                string qty = grdPurchaseReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                string rate = grdPurchaseReturn[i + 1, (int)GridColumn.PurchaseRate].Value.ToString();
                string amt = grdPurchaseReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                GridValues = GridValues + string.Concat(productname, qty, rate, amt);
            }
            GridValues = "GridValues" + GridValues;
            ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {
                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Invoice - " + txtPurchaseReturnID.Text + "?") == DialogResult.Yes)
                {
                    //Sales DelSales = new Sales();
                    PurchaseReturn Delpurchasereturn = new PurchaseReturn();
                    if (Delpurchasereturn.RemovePurchaseReturnEntry(Convert.ToInt32(txtPurchaseReturnID.Text), GridValues))
                    {
                        Global.Msg("Return -" + txtPurchaseReturnID.Text + " deleted successfully!");
                        RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "PURCHASE_RETURN"); // deleting the recurring setting if voucher is deleted
                        // Navigate to 1 step previous
                        if (!this.Navigation(Navigate.Prev))
                        {
                            //This must be because there are no records or this was the first one
                            //If this was the first, try to navigate to second
                            if (!this.Navigation(Navigate.Next))
                            {
                                //This was the last one, there are no records left. Simply clear the form and stay calm
                                ChangeState(EntryMode.NEW);
                            }
                        }
                    }
                    else
                        Global.MsgError("There was an error while deleting Return -" + txtPurchaseReturnID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtPurchaseReturnID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            m_MDIForm.OpenFormArrayParam("frmAccClass");
        }

        #region methods related to recurring
        DataTable m_dtRecurringSetting = new DataTable();
        public void GetRecurringSetting(DataTable dt)
        {
            if (dt == null)
                chkRecurring.Checked = false; // if cancel button is clicked then the chkrecurring is unchecked

            else
                this.m_dtRecurringSetting = dt;
        }
        private void ClearRecurringSetting()
        {
            m_dtRecurringSetting.Rows.Clear();
            chkRecurring.Checked = false;
            recurringVoucherID = "";
            RSID = "";
        }
        private void chkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkRecurring.Checked && m_dtRecurringSetting.Rows.Count == 0))
            {
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "PURCHASE_RETURN", m_dtRecurringSetting);
                fvr.ShowDialog();
                if (m_dtRecurringSetting.Rows.Count == 0)  // if settings are not available then uncheck the checkbox
                    chkRecurring.Checked = false;
            }
            else if (chkRecurring.Checked == false && m_dtRecurringSetting.Rows.Count > 0) //if previously saved settings are available
            {
                //if (txtSalesInvoiceID.Text != "" || txtSalesInvoiceID != null)
                //{
                if (Global.MsgQuest("Are you sure you want to delete the saved recurring voucher settings?") == DialogResult.Yes)
                {
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "PURCHASE_RETURN"); // delete from database
                    ClearRecurringSetting();  // clear local variables
                }
                else
                    chkRecurring.Checked = true;

                //}
                //else
                //    ClearRecurringSetting();
            }
            if ((chkRecurring.Checked == true && m_mode == EntryMode.EDIT) || (chkRecurring.Checked == true && m_mode == EntryMode.NEW))
                btnSetup.Enabled = true;
            else
                btnSetup.Enabled = false;
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "PURCHASE_RETURN", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "PURCHASE_RETURN");
            if (m_dtRecurringSetting.Rows.Count > 0)
            {
                chkRecurring.Checked = true;
                RSID = m_dtRecurringSetting.Rows[0]["RVID"].ToString();
                recurringVoucherID = m_dtRecurringSetting.Rows[0]["VoucherID"].ToString();
            }
            else
            {
                chkRecurring.Checked = false;
            }
        }
        
        #endregion

        #region methods related to voucher list
		
        private void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                // check if the user has permission to view the voucher
                if (!UserPermission.ChkUserPermission("PURCHASE_RETURN_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Purchase Return. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "PURCHASE_RETURN";               // voucherType
                vouchValues[1] = "Inv.tblPurchaseReturnMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Inv.tblPurchaseReturnDetails";  // details tableName for the given voucher type
                vouchValues[3] = "PurchaseReturnID";              // master ID for the given master table
                vouchValues[4] = "PurchaseReturn_Date";              // date field for a given voucher

                frmVoucherList fvl = new frmVoucherList(this, vouchValues);
                fvl.ShowDialog();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void GetVoucher(int VoucherID)
        {
            m_PurchaseReturnID = VoucherID;
            txtPurchaseReturnID.Text = VoucherID.ToString();
            Navigation(Navigate.ID);
            //frmPurchaseReturn_Load(null, null);
        } 
	#endregion

        #region method related to bank reconciliation closing
        public bool CheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;
                ListItem bankId = (ListItem)cboCashParty.SelectedItem;
                if (BankReconciliation.IsBankReconciliationClosed(Convert.ToInt32(bankId.ID), Date.ToDotNet(txtDate.Text)))
                {
                    Global.MsgError("Bank Reconciliation is closed for this Bank, So you cannot add, edit or delete the vocher !");
                    res = true;
                }
                return res;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return true;

            }
        } 
        #endregion
    }
}
