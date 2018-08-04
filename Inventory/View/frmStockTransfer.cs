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
using BusinessLogic.Inventory;
using CrystalDecisions.Shared;
using Inventory;
using Common;
using Inventory.Reports;

namespace Inventory
{
    public partial class frmStockTransfer : Form, IfrmDateConverter, ListProduct, IfrmAddAccClass
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;   
        int TotalQuantity = 0;
        double TotalAmount = 0;
        int AvailableQuantity = 0;
        private bool IsFieldChanged = false;
        private double purchaserate = 0;
        private string productcode = "";
        bool hasChanged = false;
        private int m_StockTransferID=0;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        string totalRptAmount = "";
        private IMDIMainForm m_MDIForm;
        private int StockTransferIDCopy = 0;
        private Inventory.Model.dsStockTransfer dsStockTransfer = new Model.dsStockTransfer();
        ListItem SeriesID = new ListItem();
        ListItem DepotID = new ListItem();
        List<int> AccClassID = new List<int>();
        StockTransfer m_StockTransfer = new StockTransfer();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private int loopCounter = 0;
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
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
            Del = 0, SNo, Code, ProductName,Qty,PurchaseRate ,Amount,PurchaseOrderDetailsID
        };
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProuctCode = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscPercentage = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtOrderNo = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();

        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        public frmStockTransfer()
        {
            InitializeComponent();
        }

        public frmStockTransfer(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;
            
        }
        private void AddGridHeader()
        {
            grdStockTransfer[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdStockTransfer[0, (int)GridColumn.Del].Column.Width = 25;
            grdStockTransfer[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdStockTransfer[0, (int)GridColumn.Code] = new SourceGrid.Cells.ColumnHeader("Code");
            grdStockTransfer[0, (int)GridColumn.Code].Column.Width = 150;
            grdStockTransfer[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdStockTransfer[0, (int)GridColumn.ProductName].Column.Width = 250;
            grdStockTransfer[0, (int)GridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");
            grdStockTransfer[0, (int)GridColumn.Qty].Column.Width = 150;
            grdStockTransfer[0, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.ColumnHeader("PurchaseRate");
            grdStockTransfer[0, (int)GridColumn.PurchaseRate].Column.Width = 150;
            grdStockTransfer[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdStockTransfer[0, (int)GridColumn.Amount].Column.Width = 100;
            grdStockTransfer[0, (int)GridColumn.PurchaseOrderDetailsID] = new SourceGrid.Cells.ColumnHeader("PurchaseOrderDetailsID");
            grdStockTransfer[0, (int)GridColumn.PurchaseOrderDetailsID].Column.Width = 50;
            grdStockTransfer[0, (int)GridColumn.PurchaseOrderDetailsID].Column.Visible = false;


        }

        private void AddRowProduct1(int RowCount)
        {

            try
            {
                grdStockTransfer.Redim(Convert.ToInt32(RowCount + 1), grdStockTransfer.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdStockTransfer[i, (int)GridColumn.Del] = btnDelete;
                grdStockTransfer[i, (int)GridColumn.Del].AddController(evtDelete);
                grdStockTransfer[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdStockTransfer[i, (int)GridColumn.Code] = new SourceGrid.Cells.Cell();

                SourceGrid.Cells.Editors.TextBox txtProduct = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtProduct.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdStockTransfer[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grdStockTransfer[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grdStockTransfer[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
               // txtQty.Control.Focus += new EventHandler(Qty_Focused);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdStockTransfer[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("", txtQty);
                //grdStockTransfer[i, 4].Value = "(NEW)";


                grdStockTransfer[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell();


                grdStockTransfer[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                // grdStockTransfer[i, 6].Value = "(NEW)";
                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                // txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";
                grdStockTransfer[i, (int)GridColumn.PurchaseOrderDetailsID] = new SourceGrid.Cells.Cell("");
                grdStockTransfer[i, (int)GridColumn.PurchaseOrderDetailsID].Value = "0";
                //IsSelected = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void frmStockTransfer_Load(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NEW);
            m_mode = EntryMode.NEW;
            #region BLOCK FOR SHOWING SERIES NAME IN COMBOBOX
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("STOCK_TRANS");
            if (dtSeriesInfo.Rows.Count > 0)
            {
                for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
                {
                    DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                    cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
                cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboSeriesName.SelectedIndex = 0;
            }
            #endregion

            #region BLOCK OF SHOWING FROM DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            if (dtDepotInfo.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDepotInfo.Rows)
                {
                    cboFromDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds DepotID as well as DepotName in combobox
                }


            }

            #endregion

            #region BLOCK OF SHOWING TO DEPOT IN COMBOBOX
            DataTable dtDepotInfoTo = Depot.GetDepotInfo(-1);
            if (dtDepotInfoTo.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDepotInfoTo.Rows)
                {
                    cboToDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }

            }

            #endregion

            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());

            //For Loading The Optional Fields
            OptionalFields();
            grdStockTransfer.Redim(2, 11);
            btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            AddGridHeader();
            AddRowProduct1(1);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            
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
                    if (m_StockTransferID > 0)
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
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    break;
            }
        }

        private void EnableControls(bool Enable)
        {
            txtVoucherNo.Enabled = txtRemarks.Enabled = txtDate.Enabled = cboSeriesName.Enabled = cboFromDepot.Enabled = cboToDepot.Enabled=grdStockTransfer.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled =tabControl1.Enabled= Enable;
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

        private void Product_Focused(object sender, EventArgs e)
        {
            if (cboFromDepot.SelectedItem == null)
            {
                cboFromDepot.Focus();
                MessageBox.Show("Please, select the from depot first.");



            }
            else if (cboToDepot.SelectedItem == null)
            {
                cboToDepot.Focus();
                MessageBox.Show("Please, select the to depot first.");

            }
            else if (cboFromDepot.SelectedIndex == cboToDepot.SelectedIndex)
            {
                cboFromDepot.Focus();
                MessageBox.Show("Sorry, you are trying to transfer stock in same depot.");
            }
            else
            {
                if (!hasChanged)
                {
                    ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                   frmListOfProduct flp = new frmListOfProduct(this);
                    flp.ShowDialog();
                    SendKeys.Send("{Tab}");
                }
            }
        }

        private bool isNewRow(int CurRow)
        {
            if (grdStockTransfer[CurRow, 2].Value == "(NEW)")
                return true;
            else
                return false;
        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            if (m_mode == EntryMode.EDIT)
            {
                int CurRow = grdStockTransfer.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                string code = grdStockTransfer[CurRow, 2].Value.ToString();
                string sql2 = "select Productid from inv.tblproduct where productcode='" + code + "'";
                object m_id = Global.m_db.GetScalarValue(sql2);
                int productid = Convert.ToInt32(m_id);

               

                string sql1 = "select quantity from inv.tblStocktransferdetails where stocktransferid=" + Convert.ToInt32(txtStockTransferID.Text) + " and productid="+ productid;
                object m_Number = Global.m_db.GetScalarValue(sql1);
                int quantity = Convert.ToInt32(m_Number);

               

                DepotID = (ListItem)cboFromDepot.SelectedItem;
                string sql3 = "select Quantity from inv.tblProduct where productid=" + productid + " and depotid=" + Convert.ToInt32(DepotID.ID);
                 object m_quantityOpening = Global.m_db.GetScalarValue(sql3);
                 int QuantityOpening = Convert.ToInt32(m_quantityOpening);

               
                string sql = "select SUM(incoming)-SUM(outgoing)-SUM(Damage) as stock from inv.tblInventoryTrans where depotid=" + Convert.ToInt32(DepotID.ID) + " and productid="+ productid;
                object m_quantity = Global.m_db.GetScalarValue(sql);
                int quantityTrans=Convert.ToInt32(m_quantity);
                AvailableQuantity = QuantityOpening + quantityTrans + quantity;
            }
            try
            {
                //find the current row of source grid
                int CurRow = grdStockTransfer.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdStockTransfer.RowsCount;

                object Qty = ((TextBox)sender).Text;
                bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?

                //If quantity column is entered without specifying the product name.
                if (grdStockTransfer[CurRow, (int)GridColumn.ProductName].Value == "(NEW)" || grdStockTransfer[CurRow, (int)GridColumn.ProductName].Value == "")
                {
                    Global.Msg("Please provide a valid product name");
                    return;
                }


                if (IsInt == false)
                {
                    //Global.MsgError("The quantity you posted is invalid! Please post the integer value");
                    Global.MsgError("The quantity cannot be blank or negative, Please enter the valid quantity");

                    // grdStockTransfer[CurRow, 6].Value = "";
                    grdStockTransfer[CurRow, (int)GridColumn.Qty].Value = Qty.ToString();
                    ((TextBox)sender).Focus();
                    grdStockTransfer[CurRow, (int)GridColumn.Amount].Value = "0";
                    lblTotalQty.Text = "0";
                    lblTotalAmout.Text = "0";

                    //grdPurchaseInvoice.Selection.Focus(new SourceGrid.Position(CurRow, 4), true);

                    return;
                }

                //Check whether the value of quantity is zero or not?
                if (Convert.ToInt32(Qty) == 0)
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");

                    ((TextBox)sender).Focus();
                    grdStockTransfer[CurRow, (int)GridColumn.Amount].Value = "0";
                    lblTotalQty.Text = "0";
                    lblTotalAmout.Text = "0";

                    return;
                }

                if (Convert.ToInt32(Qty) > AvailableQuantity)
                {
                    Global.MsgError("The Quantity exceeded the available quantity. Fill the Quantity again!");

                    ((TextBox)sender).Text = AvailableQuantity.ToString();
                    ((TextBox)sender).Focus();
                    grdStockTransfer[CurRow, (int)GridColumn.Amount].Value = AvailableQuantity * Convert.ToDouble(grdStockTransfer[CurRow, (int)GridColumn.PurchaseRate].Value);
                    
                    CalculateTotalQuantity();
                    CalculateAmount();
                    return;
                }
                grdStockTransfer[CurRow, (int)GridColumn.Qty].Value = Qty.ToString();
                WriteAmount(CurRow, Convert.ToInt32(Qty));
                CalculateTotalQuantity();
                CalculateAmount();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

       

        private void WriteAmount(int CurRow, int Qty)
        {
            string PurchaseRate = grdStockTransfer[CurRow, (int)GridColumn.PurchaseRate].Value.ToString();
            double Amount = Qty * Convert.ToDouble(PurchaseRate);
            grdStockTransfer[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
        }

        private void CalculateAmount()
        {
            try
            {
                double GrossAmount = 0;
                for (int i = 1; i < grdStockTransfer.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdStockTransfer.Rows.Count)
                        return;
                    double m_GrossAmount = 0;
                    string m_Value = Convert.ToString(grdStockTransfer[i, (int)GridColumn.Amount].Value);
                    if (m_Value.Length == 0)
                        m_GrossAmount = 0;
                    else
                        m_GrossAmount = Convert.ToDouble(grdStockTransfer[i, (int)GridColumn.Amount].Value);

                    GrossAmount += m_GrossAmount;
                    //GAmount = GrossAmount;
                    lblTotalAmout.Text = GrossAmount.ToString();



                }
            }
            catch (Exception ex)
            {

                Global.MsgError("Error in Gross Amount calucation!");
            }
        }


        private void CalculateTotalQuantity()
        {
            try
            {
                int TotalQuantity = 0;
                for (int i = 1; i < grdStockTransfer.RowsCount - 1; i++)
                {
                    if (i == grdStockTransfer.Rows.Count)
                        return;
                    int m_TotalQuantity = 0;
                    string m_Value = Convert.ToString(grdStockTransfer[i, (int)GridColumn.Qty].Value);
                    if (m_Value.Length == 0)
                        m_TotalQuantity = 0;
                    else
                        m_TotalQuantity = Convert.ToInt32(grdStockTransfer[i, (int)GridColumn.Qty].Value);

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


        private void Product_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowsCount = grdStockTransfer.RowsCount;
            string LastServicesCell = (string)grdStockTransfer[RowsCount - 1, 3].Value;

            ////Check whether the new row is already added
            if (LastServicesCell != "(NEW)")
            {
                // AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);

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

        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //CurrRowPos = ctx.Position.Row;
            if (IsSelected)
            {
                int Quantity = 0;

                double Amount = 0;

                bool ProductPresent = false;
                DepotID = (ListItem)cboFromDepot.SelectedItem;

                string sql = "select SUM(incoming)-SUM(outgoing)-SUM(Damage) as stock from inv.tblInventoryTrans where depotid=" + Convert.ToInt32(DepotID.ID) + " and productid="+ productid;
                object m_Number = Global.m_db.GetScalarValue(sql);

                string sql1 = "Select Quantity from inv.tblproduct where depotid=" + Convert.ToInt32(DepotID.ID) + " and productid=" + productid;
                object m_OpeningQuantity = Global.m_db.GetScalarValue(sql1);
                int OpeningQuantity = Convert.ToInt32(m_OpeningQuantity);

              
                if ( m_Number != DBNull.Value )
                     {
                          Quantity = Convert.ToInt32(m_Number);
                           ProductPresent = true;

                     }

                int TotalProductQuantity = Quantity + OpeningQuantity;
                
                if (ProductPresent)
                {
                    //If the stock for that product is zero or less than zero
                    if(TotalProductQuantity <= 0)
                    {
                        Global.Msg("There is no stock available for this product.");
                        return;
                    }

                    int CurRow = grdStockTransfer.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    // CurrRowPos = ctx.Position.Row;
                    ctx.Text = productname;
                    AvailableQuantity = Quantity;
                    purchaserate = purchaserate;
                    productcode = productcode;
                    grdStockTransfer[CurRow, (int)GridColumn.Code].Value = productcode;
                    grdStockTransfer[CurRow, (int)GridColumn.Qty].Value = TotalProductQuantity;
                    grdStockTransfer[CurRow, (int)GridColumn.PurchaseRate].Value = purchaserate;
                    Amount = Quantity * purchaserate;
                    grdStockTransfer[CurRow, (int)GridColumn.Amount].Value = Amount;
                    TotalQuantity += TotalProductQuantity;
                    TotalAmount += Amount;
                    lblTotalQty.Text = TotalQuantity.ToString();
                    lblTotalAmout.Text = TotalAmount.ToString();
                    int RowsCount = grdStockTransfer.RowsCount;
                    AvailableQuantity = TotalProductQuantity;
                    string LastServicesCell = (string)grdStockTransfer[RowsCount - 1, 3].Value;
                    ////Check whether the new row is already added
                    if (LastServicesCell == "(NEW)")
                    {
                        AddRowProduct1(RowsCount);

                    }
                }
                else
                {
                    //int CurRow = grdStockTransfer.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    MessageBox.Show("Sorry, the product is not present in this depot.");
                    //grdStockTransfer[CurRow,(int)GridColumn.ProductName].Controller.FindController(typeof(string)) = 
                    //Product_Focused(new object(),new EventArgs());
                }
            }
            hasChanged = true;

        }


        public void AddAccClass()
        {
            try
            {
                //Clear the checked nodes of Treeview and relaoding the tree view
                treeAccClass.Nodes.Clear();
                ShowAccClassInTreeView(treeAccClass, null, 0);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
            #region Language Management

            tv.Font = LangMgr.GetFont();

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

            if (Global.GlobalAccClassID == 1 && Global.GlobalAccessRoleID == 37)
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = AccountClass.GetAccClassTable(AccClassID);
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                    if (n == null)
                    {
                        t.Checked = true;
                        tv.Nodes.Add(t); //Primary Group
                    }
                    else
                    {
                        t.Checked = true;
                        n.Nodes.Add(t); //Secondary Group
                    }
                }
            }
            else
            {

                DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
                DataRow drUserInfo = dtUserInfo.Rows[0];
                ArrayList AccClassChildIDs = new ArrayList();
                ArrayList tempParentAccClassChildIDs = new ArrayList();
                AccClassChildIDs.Clear();
                AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
                AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
                DataTable dt = new DataTable();
                try
                {
                    dt = AccountClass.GetAccClassTable(AccClassID);
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    tempParentAccClassChildIDs.Clear();
                    AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }

                        foreach (int itemIDs in AccClassChildIDs)  //To check if 
                        {
                            if (Convert.ToInt32(t.Tag) == itemIDs)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                                loopCounter--;
                                t.Checked = true;
                                if (n == null)
                                {
                                    tv.Nodes.Add(t); //Primary Group
                                    return;
                                }
                                else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
                                {
                                    t.Checked = true;
                                    tv.Nodes.Add(t);
                                    return;
                                }
                                else
                                {
                                    n.Nodes.Add(t); //Secondary Group
                                }
                            }
                            if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

           
        }


        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }
        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            //frmAccountClass frm = new frmAccountClass(this);
            //frm.Show();
            m_MDIForm.OpenFormArrayParam("frmAccClass");
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

        private void btnEdit_Click(object sender, EventArgs e)
        {

            string value = Settings.GetSettings("FREEZE_STATUS");
            string startdate = Settings.GetSettings("FREEZE_START_DATE");
            string enddate = Settings.GetSettings("FREEZE_END_DATE");
            DateTime dstart = Convert.ToDateTime(startdate);
            DateTime dend = Convert.ToDateTime(enddate);
            DateTime tdate = Convert.ToDateTime(txtDate.Text);
            if (value == "1" && tdate >= dstart && tdate <= dend)
            {
                MessageBox.Show("The transaction is freeze you can not Edit");
            }
            else
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("STOCK_TRANSFER_MODIFY");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                    return;
                }
                EnableControls(true);
                ChangeState(EntryMode.EDIT);

                //if automatic voucher number increment is selected
                string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                if (NumberingType == "AUTOMATIC")
                    txtVoucherNo.Enabled = false;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("STOCK_TRANSFER_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("STOCK_TRANSFER_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            else
                try
                {
                    m_StockTransfer.Delete(Convert.ToInt32(txtStockTransferID.Text));
                    Global.Msg("Stock Transferred was deleted successfully!");
                    ChangeState(EntryMode.NORMAL);
                    if (!chkDoNotClose.Checked)
                        this.Close();

                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }




        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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
                    try
                    {

                        //Read from sourcegrid and store it to table
                        DataTable StockTransferDetails = new DataTable();
                        StockTransferDetails.Columns.Add("Code");
                        StockTransferDetails.Columns.Add("Product");
                        StockTransferDetails.Columns.Add("Quantity");
                        StockTransferDetails.Columns.Add("PurchaseRate");
                        StockTransferDetails.Columns.Add("Amount");
                        StockTransferDetails.Columns.Add("PurchaseOrderDetailsID");
                        for (int i = 0; i < grdStockTransfer.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            StockTransferDetails.Rows.Add(grdStockTransfer[i + 1, 2].Value, grdStockTransfer[i + 1, 3].Value, grdStockTransfer[i + 1, 4].Value, grdStockTransfer[i + 1, 5].Value, grdStockTransfer[i + 1, 6].Value, grdStockTransfer[i + 1, 7].Value);
                        }
                        DateTime StockTransfer_Date = Date.ToDotNet(txtDate.Text);


                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem LiFromDepotID = new ListItem();
                        LiFromDepotID = (ListItem)cboFromDepot.SelectedItem;
                        ListItem LiToDepotID = new ListItem();
                        LiToDepotID = (ListItem)cboToDepot.SelectedItem;

                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;

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

                        if (AccClassID.Count != 0)
                        {
                             m_StockTransfer.Create(Convert.ToInt32(SeriesID.ID),txtVoucherNo.Text, StockTransfer_Date, Convert.ToInt32(LiFromDepotID.ID),Convert.ToInt32(LiToDepotID.ID),  txtRemarks.Text, StockTransferDetails, AccClassID.ToArray(),OF );
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_StockTransfer.Create(Convert.ToInt32(SeriesID.ID), txtVoucherNo.Text, StockTransfer_Date, Convert.ToInt32(LiFromDepotID.ID), Convert.ToInt32(LiToDepotID.ID), txtRemarks.Text, StockTransferDetails, a.ToArray(),OF);
                        }

                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Stock Transferred successfully!");
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
                                Global.Msg("ERROR: The Order Number already exists! Please Enter another Order Number!", MBType.Warning, "Error");
                                
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
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                #region EDIT
                case EntryMode.EDIT: //if new button is pressed
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable StockTransferDetails = new DataTable();
                        StockTransferDetails.Columns.Add("Code");
                        StockTransferDetails.Columns.Add("Product");
                        StockTransferDetails.Columns.Add("Quantity");
                        StockTransferDetails.Columns.Add("PurchaseRate");
                        StockTransferDetails.Columns.Add("Amount");
                        StockTransferDetails.Columns.Add("PurchaseOrderDetailsID");
                        for (int i = 0; i < grdStockTransfer.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            StockTransferDetails.Rows.Add(grdStockTransfer[i + 1, 2].Value, grdStockTransfer[i + 1, 3].Value, grdStockTransfer[i + 1, 4].Value, grdStockTransfer[i + 1, 5].Value, grdStockTransfer[i + 1, 6].Value, grdStockTransfer[i + 1, 7].Value);
                        }
                        DateTime StockModify_Date = Date.ToDotNet(txtDate.Text);


                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem LiFromDepotID = new ListItem();
                        LiFromDepotID = (ListItem)cboFromDepot.SelectedItem;
                        ListItem LiToDepotID = new ListItem();
                        LiToDepotID = (ListItem)cboToDepot.SelectedItem;
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (AccClassID.Count != 0)
                        {
                            m_StockTransfer.Modify(Convert.ToInt32(txtStockTransferID.Text),Convert.ToInt32(SeriesID.ID), txtVoucherNo.Text, StockModify_Date, Convert.ToInt32(LiFromDepotID.ID), Convert.ToInt32(LiToDepotID.ID), txtRemarks.Text, StockTransferDetails, AccClassID.ToArray(),OF);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_StockTransfer.Modify(Convert.ToInt32(txtStockTransferID.Text), Convert.ToInt32(SeriesID.ID), txtVoucherNo.Text, StockModify_Date, Convert.ToInt32(LiFromDepotID.ID), Convert.ToInt32(LiToDepotID.ID), txtRemarks.Text, StockTransferDetails, a.ToArray(),OF);

                        }
                        Global.Msg("Stock Transfer modified successfully!");
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
                        Global.MsgError(ex.Message);
                    }

                    break;

                #endregion
            }
            if (checkBox2.Checked)
            {
                prntDirect = 1;
                Navigation(Navigate.Last);
                Print_Click(sender, e);
                ClearVoucher();
                ChangeState(EntryMode.NEW);
                btnNew_Click(sender, e);
            }
            if (!chkDoNotClose.Checked)
                this.Close();

        }

        private void btn_tenderamount_Click(object sender, EventArgs e)
        {
       
            double totalamount;
           
           
            totalamount = (Convert.ToDouble(lblTotalAmout.Text)) ;
            frmtenderamount ft = new frmtenderamount(totalamount);
            ft.ShowDialog();
       
        }

        private void ClearStockTransfer(bool IsVouNumFinished)
        {
            txtVoucherNo.Clear();
            //actually generate a new voucher no.
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
           
            txtRemarks.Clear();
            cboFromDepot.Text = string.Empty;
            cboToDepot.Text = string.Empty;
           
            cboSeriesName.Text = string.Empty;
           
            if (!IsVouNumFinished)
            {
                grdStockTransfer.Rows.Clear();
            }
        }


        private void ClearVoucher()
        {
            ClearStockTransfer(false);
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            lblTotalQty.Text = "0.00";
            lblTotalAmout.Text = "0.00";
            grdStockTransfer.Redim(2, 11);
            AddGridHeader(); //Write header part
            //AddRowServices(1);
            //AddRowProduct(1);
            AddRowProduct1(1);

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Prev);
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
                    VouchID = Convert.ToInt32(txtStockTransferID.Text);
                    if (StockTransferIDCopy > 0)
                    {
                        VouchID = StockTransferIDCopy;
                        StockTransferIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtStockTransferID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                 DataTable dtStockTransferMaster = m_StockTransfer.NavigateStockTransferMaster(VouchID, NavTo);

                 if (dtStockTransferMaster.Rows.Count <= 0)//this is the first record
                 {
                     Global.Msg("No more records found!");
                     btnExport.Enabled = false;
                     return false;
                 }

                 //Clear everything in the form
                 ClearVoucher();
                 //Write the corresponding textboxes
                 DataRow drStockTransferMaster = dtStockTransferMaster.Rows[0]; //There is only one row. First row is the required record

                 //Show the corresponding FromDepot in control

                 DataTable dtFromDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drStockTransferMaster["FromDepotID"]));
                 if (dtFromDepotInfo.Rows.Count > 0)
                 {
                     DataRow drFromDepotInfo = dtFromDepotInfo.Rows[0];
                     cboFromDepot.Text = drFromDepotInfo["DepotName"].ToString();
                 }

                 //Show the corresponding ToDepot in control

                 DataTable dtToDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drStockTransferMaster["ToDepotID"]));
                 if (dtToDepotInfo.Rows.Count > 0)
                 {
                     DataRow drToDepotInfo = dtToDepotInfo.Rows[0];
                     cboToDepot.Text = drToDepotInfo["DepotName"].ToString();
                 }

                 //show the corresoponding SeriesName in Combobox
                 DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drStockTransferMaster["SeriesID"]));
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
                 txtVoucherNo.Text = drStockTransferMaster["Voucher_No"].ToString();
                 txtDate.Text = Date.DBToSystem(drStockTransferMaster["Created_Date"].ToString());
                 txtRemarks.Text = drStockTransferMaster["Remarks"].ToString();
                 txtStockTransferID.Text = drStockTransferMaster["StockTransferID"].ToString();
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

                         txtfirst.Text = drStockTransferMaster["Field1"].ToString();
                         txtsecond.Text = drStockTransferMaster["Field2"].ToString();
                         txtthird.Text = drStockTransferMaster["Field3"].ToString();
                         txtfourth.Text = drStockTransferMaster["Field4"].ToString();
                         txtfifth.Text = drStockTransferMaster["Field5"].ToString();
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

                         txtfirst.Text = drStockTransferMaster["Field1"].ToString();
                         txtsecond.Text = drStockTransferMaster["Field2"].ToString();
                         txtthird.Text = drStockTransferMaster["Field3"].ToString();
                         txtfourth.Text = drStockTransferMaster["Field4"].ToString();
                         txtfifth.Text = drStockTransferMaster["Field5"].ToString();
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

                         txtfirst.Text = drStockTransferMaster["Field1"].ToString();
                         txtsecond.Text = drStockTransferMaster["Field2"].ToString();
                         txtthird.Text = drStockTransferMaster["Field3"].ToString();
                         txtfourth.Text = drStockTransferMaster["Field4"].ToString();
                         txtfifth.Text = drStockTransferMaster["Field5"].ToString();

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

                         txtfirst.Text = drStockTransferMaster["Field1"].ToString();
                         txtsecond.Text = drStockTransferMaster["Field2"].ToString();
                         txtthird.Text = drStockTransferMaster["Field3"].ToString();
                         txtfourth.Text = drStockTransferMaster["Field4"].ToString();
                         txtfifth.Text = drStockTransferMaster["Field5"].ToString();

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

                         txtfirst.Text = drStockTransferMaster["Field1"].ToString();
                         txtsecond.Text = drStockTransferMaster["Field2"].ToString();
                         txtthird.Text = drStockTransferMaster["Field3"].ToString();
                         txtfourth.Text = drStockTransferMaster["Field4"].ToString();
                         txtfifth.Text = drStockTransferMaster["Field5"].ToString();
                     }


                 }

                 int totalqty = 0;
                 decimal totalamt = 0;
                //Fill the dataset(tblStockTransferMaster)
                 dsStockTransfer.Tables["tblStockTransferMaster"].Rows.Add(cboSeriesName.Text, drStockTransferMaster["Voucher_No"].ToString(), cboFromDepot.Text,cboToDepot.Text,Date.ToSystem(Convert.ToDateTime(drStockTransferMaster["Created_Date"].ToString())), drStockTransferMaster["Remarks"].ToString());

                 DataTable dtStockTransferDetails = m_StockTransfer.GetStockTransferDetails(Convert.ToInt32(txtStockTransferID.Text));
                 for (int i = 1; i <= dtStockTransferDetails.Rows.Count; i++)
                 {
                     DataRow drDetail = dtStockTransferDetails.Rows[i - 1];
                     grdStockTransfer[i, (int)GridColumn.SNo].Value = i.ToString();
                     grdStockTransfer[i, (int)GridColumn.Code].Value = drDetail["Code"].ToString();
                     grdStockTransfer[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                     grdStockTransfer[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                     grdStockTransfer[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                     grdStockTransfer[i, (int)GridColumn.PurchaseOrderDetailsID].Value = "";
                     // AddRowProduct(grdPurchaseInvoice.RowsCount);
                     
                     int Qty = Convert.ToInt32(drDetail["Quantity"].ToString());
                     totalqty += Qty;
                     lblTotalQty.Text = totalqty.ToString();

                   //  double amt = Convert.ToDouble(drDetail["Amount"].ToString());
                     decimal amt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"].ToString()));
                     totalamt += amt;
                     lblTotalAmout.Text = totalamt.ToString();

                     AddRowProduct1(grdStockTransfer.RowsCount);
                     dsStockTransfer.Tables["tblStockTransferDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(),drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString(),Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString());
                     
                 }
                 totalRptAmount = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalamt)).ToString();    
                
                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtStockTransferID.Text), "STOCK_TRANS");
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
                 return true;
                 

        }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
                return false;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.First);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Last);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            StockTransferIDCopy = Convert.ToInt32(txtStockTransferID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (StockTransferIDCopy > 0)
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

        private void Print_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    Print_Click(sender, e);
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
                    Print_Click(sender, e);
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
                    Print_Click(sender, e);
                    break;
            }
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

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void PrintPreviewCR(PrintType myPrintType)
        {
            dsStockTransfer.Clear();
            rptStockTransfer rpt = new rptStockTransfer();
            Misc.WriteLogo(dsStockTransfer, "tblImage");
            rpt.SetDataSource(dsStockTransfer);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

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
                rpt.SetParameterValue("TotalAmount", totalRptAmount);
                if (totalRptAmount == "")
                {
                    totalRptAmount = "0";
                }
                string inwords = AmountToWords.ConvertNumberAsText(totalRptAmount);
                rpt.SetParameterValue("AmtInWords", inwords);

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
                // frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }

        }
    }
}