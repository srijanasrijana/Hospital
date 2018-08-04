using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using DateManager;
using BusinessLogic.Inventory;
using CrystalDecisions.Shared;
using Inventory.Reports;
using Common;

namespace Inventory
{
    public partial class frmDamageItems : Form, IfrmDateConverter, ListProduct, IfrmAddAccClass
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;   
        private string Prefix = "";
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        ListItem SeriesID = new ListItem();
        ListItem liProjectID = new ListItem();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        //frmDamageItems damageitems = new frmDamageItems();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        Services m_Service = new Services();
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;
        private double purchaserate = 0;
        private string productcode = "";
        private int productid;
        private int m_DamageItemID;
        private bool isNew;
        private int CurrRowPos = 0;
        private string ProductQuantity;
        private int loopCounter = 0;
        private int PurchaseInvoiceIDCopy = 0;
        private Inventory.Model.dsDamageItems dsDamageItems = new Model.dsDamageItems();

        private IMDIMainForm m_MDIForm;
       
      //  private DataSet.dsSalesInvoice dsSalesInvoice = new DataSet.dsSalesInvoice();

        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        List<int> AccClassID = new List<int>();
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
            Del = 0, SNo, Code_No, ProductName, Qty, PurchaseRate, Total_Amount,ProductID
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

        public frmDamageItems()
        {
            InitializeComponent();
        }
        public frmDamageItems(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;
            
        }

       
        private void frmDamageItems_Load(object sender, EventArgs e)
        {
            //ListProject(cboProjectName);
          
            ChangeState(EntryMode.NEW);
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate());
            LoadComboboxProject(cboProjectName, 0);
            //For Loading The Optional Fields
           

            #region BLOCK FOR SHOWING SERIES NAME IN COMBOBOX
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("DAMAGE");
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition
           cboSeriesName.SelectedIndex = 0;
            #endregion

          #region BLOCK OF SHOWING DEPOT IN COMBOBOX
           // DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
           //// cboDepot.Items.Add(new ListItem((0), "None"));
           // foreach (DataRow dr in dtDepotInfo.Rows)
           // {
           //     cboDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
           // }
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            foreach (DataRow dr in dtDepotInfo.Rows)
            {
                cboDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
          cboDepot.SelectedIndex = 0;
            #endregion
            OptionalFields();
            try
            {
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                //Event when account is selected
                evtProduct.FocusLeft += new EventHandler(Product_Selected);
                //Event when ProductCode is selected
                evtProuctCode.FocusLeft += new EventHandler(ProductCode_Selected);
                //Event when Quntity is selected
                evtQty.FocusLeft += new EventHandler(Qty_Modified);
                //evtQty.FocusLeft += new EventHandler(PurchaseRate_Modified);
                //Event when Rate is selected
                evtRate.FocusLeft += new EventHandler(PurchaseRate_Modified);
            grddamage.Redim(2, 8);
            AddGridHeader();
            AddRowProduct1(1);
            AddRowDamage(1);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           // ShowAccClassInTreeView(treeAccClass, null, 0);

            #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
          //if (m_DamageItemID > 0)
          //      {
          //          //Show the values in fields
          //          try
          //          {
          //              ChangeState(EntryMode.NORMAL);
          //              int vouchID = 0;
          //              try
          //              {
          //                  vouchID = m_DamageItemID;
          //              }
          //              catch (Exception)
          //              {
          //                  vouchID = 999999999; //set to maximum so that it automatically gets the highest
          //              }

          //              DamageItems m_Sales = new DamageItems();

          //              //Getting the value of SeriesID via MasterID or VouchID
          //              int SeriesIDD = m_Sales.GetSeriesIDFromMasterID(vouchID);

          //              DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
          //              if (dt.Rows.Count <= 0)
          //              {
          //                  Global.Msg("There is no any SeriesName in this Purchase Invoice");
          //                  cboSeriesName.Text = "";
          //              }
          //              else
          //              {
          //                  DataRow dr = dt.Rows[0];
          //                  cboSeriesName.Text = dr["EngName"].ToString();
          //              }
          //              DataTable dtDamageItemMaster = m_Sales.GetDamageItemMasterInfo (vouchID.ToString());
          //              if (dtDamageItemMaster.Rows.Count <= 0)//this is the first record
          //              {
          //                  Global.Msg("No more records found!");
          //                  return;
          //              }
          //              foreach (DataRow drDamageItemMaster in dtDamageItemMaster.Rows)
          //              {
          //                  //txtproductpin.Text = drSalesInvoiceMaster["PartyBillNumber"].ToString();
          //                  txtVoucherNo.Text = drDamageItemMaster["Voucher_No"].ToString();
          //                  txtDate.Text = Date.DBToSystem(drDamageItemMaster["DPInvoice_Date"].ToString());

          //                  txtDamageProductID.Text = drDamageItemMaster["DamageProductID"].ToString();

          //                  //txtOrderNo.Text = drSalesInvoiceMaster["OrderNo"].ToString();
          //                  //lblNetAmout.Text = drSalesInvoiceMaster["Net_Amount"].ToString();
          //                  //lbltax1.Text = drSalesInvoiceMaster["Tax1"].ToString();
          //                  //lbltax2.Text = drSalesInvoiceMaster["Tax2"].ToString();
          //                  //lbltax3.Text = drSalesInvoiceMaster["Tax3"].ToString();
          //                  //lblVat.Text = drSalesInvoiceMaster["VAT"].ToString();
          //                  //lblTotalQty.Text = drSalesInvoiceMaster["TotalQty"].ToString();
          //                  //lblSpecialDiscount.Text = drSalesInvoiceMaster["SpecialDiscount"].ToString();
          //                  //lblGross.Text = drSalesInvoiceMaster["Gross_Amount"].ToString();
          //                  //lblgrandtotal.Text = (Convert.ToDouble(lblNetAmout.Text) + Convert.ToDouble(lbltax1.Text) + Convert.ToDouble(lbltax2.Text) + Convert.ToDouble(lbltax3.Text) + Convert.ToDouble(lblVat.Text)).ToString();  
          //                  //For Additional Fields
          //                  if (NumberOfFields > 0)
          //                  {
          //                      if (NumberOfFields == 1)
          //                      {
          //                          lblfirst.Visible = true;
          //                          txtfirst.Visible = true;
          //                          lblsecond.Visible = false;
          //                          txtsecond.Visible = false;
          //                          lblthird.Visible = false;
          //                          txtthird.Visible = false;
          //                          lblfourth.Visible = false;
          //                          txtfourth.Visible = false;
          //                          lblfifth.Visible = false;
          //                          txtfifth.Visible = false;
          //                          lblfirst.Text = drdtadditionalfield["Field1"].ToString();

          //                          txtfirst.Text = drDamageItemMaster["Field1"].ToString();
          //                          txtsecond.Text = drDamageItemMaster["Field2"].ToString();
          //                          txtthird.Text = drDamageItemMaster["Field3"].ToString();
          //                          txtfourth.Text = drDamageItemMaster["Field4"].ToString();
          //                          txtfifth.Text = drDamageItemMaster["Field5"].ToString();
          //                      }
          //                      else if (NumberOfFields == 2)
          //                      {
          //                          lblfirst.Visible = true;
          //                          txtfirst.Visible = true;
          //                          lblsecond.Visible = true;
          //                          txtsecond.Visible = true;
          //                          lblthird.Visible = false;
          //                          txtthird.Visible = false;
          //                          lblfourth.Visible = false;
          //                          txtfourth.Visible = false;
          //                          lblfifth.Visible = false;
          //                          txtfifth.Visible = false;
          //                          lblfirst.Text = drdtadditionalfield["Field1"].ToString();
          //                          lblsecond.Text = drdtadditionalfield["Field2"].ToString();

          //                          txtfirst.Text = drDamageItemMaster["Field1"].ToString();
          //                          txtsecond.Text = drDamageItemMaster["Field2"].ToString();
          //                          txtthird.Text = drDamageItemMaster["Field3"].ToString();
          //                          txtfourth.Text = drDamageItemMaster["Field4"].ToString();
          //                          txtfifth.Text = drDamageItemMaster["Field5"].ToString();
          //                      }
          //                      else if (NumberOfFields == 3)
          //                      {
          //                          lblfirst.Visible = true;
          //                          txtfirst.Visible = true;
          //                          lblsecond.Visible = true;
          //                          txtsecond.Visible = true;
          //                          lblthird.Visible = true;
          //                          txtthird.Visible = true;
          //                          lblfourth.Visible = false;
          //                          txtfourth.Visible = false;
          //                          lblfifth.Visible = false;
          //                          txtfifth.Visible = false;
          //                          lblfirst.Text = drdtadditionalfield["Field1"].ToString();
          //                          lblsecond.Text = drdtadditionalfield["Field2"].ToString();
          //                          lblthird.Text = drdtadditionalfield["Field3"].ToString();

          //                          txtfirst.Text = drDamageItemMaster["Field1"].ToString();
          //                          txtsecond.Text = drDamageItemMaster["Field2"].ToString();
          //                          txtthird.Text = drDamageItemMaster["Field3"].ToString();
          //                          txtfourth.Text = drDamageItemMaster["Field4"].ToString();
          //                          txtfifth.Text = drDamageItemMaster["Field5"].ToString();

          //                      }
          //                      else if (NumberOfFields == 4)
          //                      {
          //                          lblfirst.Visible = true;
          //                          txtfirst.Visible = true;
          //                          lblsecond.Visible = true;
          //                          txtsecond.Visible = true;
          //                          lblthird.Visible = true;
          //                          txtthird.Visible = true;
          //                          lblfourth.Visible = true;
          //                          txtfourth.Visible = true;
          //                          lblfifth.Visible = false;
          //                          txtfifth.Visible = false;
          //                          lblfirst.Text = drdtadditionalfield["Field1"].ToString();
          //                          lblsecond.Text = drdtadditionalfield["Field2"].ToString();
          //                          lblthird.Text = drdtadditionalfield["Field3"].ToString();
          //                          lblfourth.Text = drdtadditionalfield["Field4"].ToString();

          //                          txtfirst.Text = drDamageItemMaster["Field1"].ToString();
          //                          txtsecond.Text = drDamageItemMaster["Field2"].ToString();
          //                          txtthird.Text = drDamageItemMaster["Field3"].ToString();
          //                          txtfourth.Text = drDamageItemMaster["Field4"].ToString();
          //                          txtfifth.Text = drDamageItemMaster["Field5"].ToString();

          //                      }
          //                      else if (NumberOfFields == 5)
          //                      {
          //                          lblfirst.Visible = true;
          //                          txtfirst.Visible = true;
          //                          lblsecond.Visible = true;
          //                          txtsecond.Visible = true;
          //                          lblthird.Visible = true;
          //                          txtthird.Visible = true;
          //                          lblfourth.Visible = true;
          //                          txtfourth.Visible = true;
          //                          lblfifth.Visible = true;
          //                          txtfifth.Visible = true;

          //                          lblfirst.Text = drdtadditionalfield["Field1"].ToString();
          //                          lblsecond.Text = drdtadditionalfield["Field2"].ToString();
          //                          lblthird.Text = drdtadditionalfield["Field3"].ToString();
          //                          lblfourth.Text = drdtadditionalfield["Field4"].ToString();
          //                          lblfifth.Text = drdtadditionalfield["Field5"].ToString();

          //                          txtfirst.Text = drDamageItemMaster["Field1"].ToString();
          //                          txtsecond.Text = drDamageItemMaster["Field2"].ToString();
          //                          txtthird.Text = drDamageItemMaster["Field3"].ToString();
          //                          txtfourth.Text = drDamageItemMaster["Field4"].ToString();
          //                          txtfifth.Text = drDamageItemMaster["Field5"].ToString();
          //                      }


          //                  }
          //                  dsDamageItems.Tables["tblDamageProductInvoiceMaster"].Rows.Add(cboSeriesName.Text, drDamageItemMaster["Voucher_No"].ToString(), Date.DBToSystem(drDamageItemMaster["DPInvoice_Date"].ToString()), cboDepot.Text);





          //                  DataTable dtDepotDtlInfo = Depot.GetDepotInfo(Convert.ToInt32(drDamageItemMaster["DepotID"].ToString()));
          //                  foreach (DataRow drDepotInfo in dtDepotDtlInfo.Rows)
          //                  {
          //                      cboDepot.Text = drDepotInfo["DepotName"].ToString();
          //                  }

          //              }

          //              DataTable dtDamageItemsDetails = m_Sales.GetDamageItemsDetails(vouchID);
          //              if (dtDamageItemsDetails.Rows.Count > 0)
          //              {
          //                  for (int i = 1; i <= dtDamageItemsDetails.Rows.Count; i++)
          //                  {
          //                      DataRow drDetail = dtDamageItemsDetails.Rows[i - 1];
          //                      grddamage[i, (int)GridColumn.SNo].Value = i.ToString();
          //                      grddamage[i, (int)GridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
          //                     // grddamage[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
          //                      grddamage[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
          //                      grddamage[i, (int)GridColumn.PurchaseRate].Value = drDetail["PurchaseRate"].ToString();
          //                      grddamage[i, (int)GridColumn.Total_Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
          //                      grddamage[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();
          //                      AddRowProduct1(grddamage.RowsCount);
          //                      dsDamageItems.Tables["tblDamageProductInvoiceDetail"].Rows.Add(drDetail["ProductCode"].ToString(), drDetail["ProductID"].ToString(), drDetail["Quantity"].ToString(), drDetail["PurchaseRate"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString());
          //                  }
          //              }
          //          }

          //          catch (Exception ex)
          //          {

          //              MessageBox.Show(ex.Message);
          //          }
          //      }

                #endregion

         

        }

        private void AddRowProduct1(int RowCount)
        {
            //Add a new row
            try
            {
                grddamage.Redim(Convert.ToInt32(RowCount + 1), grddamage.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grddamage[i, (int)GridColumn. Del] = btnDelete;
                grddamage[i, (int)GridColumn.Del].AddController(evtDelete);
                grddamage[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grddamage[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();

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
                grddamage[i, 3] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grddamage[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grddamage[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grddamage[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                txtQty.Control.TextChanged += new EventHandler(Text_Change);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";


                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grddamage[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);

                grddamage[i, (int)GridColumn.Total_Amount] = new SourceGrid.Cells.Cell();
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";

                grddamage[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("");
                grddamage[i, (int)GridColumn.ProductID].Value = "0";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


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


        private void btnDate_Click(object sender, EventArgs e)
        {
           // m_MDIForm.OpenForm("frmDateConverter");
            Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
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
                           // ClearPurchaseInvoice(true);
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
                    if (m_DamageItemID > 0)
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
            txtVoucherNo.Enabled = txtDate.Enabled = grddamage.Enabled = cboSeriesName.Enabled = cboDepot.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled =tabControl1.Enabled= Enable;
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

        private void AddGridHeader()
        {
            grddamage[0,(int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grddamage[0, (int)GridColumn.Del].Column.Width = 25;
            grddamage[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grddamage[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grddamage[0, (int)GridColumn.Code_No].Column.Width = 80;
            grddamage[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grddamage[0, (int)GridColumn.ProductName].Column.Width = 300;
            grddamage[0, (int)GridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");
            grddamage[0, (int)GridColumn.Qty].Column.Width = 80;
            grddamage[0, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.ColumnHeader("PurchaseRate");
            grddamage[0, (int)GridColumn.PurchaseRate].Column.Width = 100;
            grddamage[0, (int)GridColumn.Total_Amount] = new SourceGrid.Cells.ColumnHeader("Total Amount");
            grddamage[0, (int)GridColumn.Total_Amount].Column.Width = 120;
            grddamage[0, (int)GridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("ProductID");
            grddamage[0, (int)GridColumn.ProductID].Column.Width = 2;
            grddamage[0, (int)GridColumn.ProductID].Column.Visible = false;
           // grddamage[0, 0].Column.Visible = false;
        }
        private void AddRowDamage(int RowCount)
        {
            try
            {
                grddamage.Redim(Convert.ToInt32(RowCount + 1), grddamage.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grddamage[i, (int)GridColumn.Del] = btnDelete;
                grddamage[i, (int)GridColumn.Del].AddController(evtDelete);
                grddamage[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();

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
                grddamage[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grddamage[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grddamage[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grddamage[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("", txtQty);

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;
                grddamage[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);

                grddamage[i, (int)GridColumn.Total_Amount] = new SourceGrid.Cells.Cell("Total Amount");
                grddamage[i, (int)GridColumn.Total_Amount].Value = "";

                grddamage[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("");
                grddamage[i, (int)GridColumn.ProductID].Value = "0";
                //IsSelected = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grddamage.RowsCount - 2)
                grddamage.Rows.Remove(ctx.Position.Row);
        }

        private void Product_Selected(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;

                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {

            }
            int CurRow = grddamage.Selection.GetSelectionRegion().GetRowsIndex()[0];
            DataTable dt = Product.GetProductByName(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row

                    grddamage[(CurRow), 2].Value = dr["ProductCode"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.

                    grddamage[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grddamage[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
                int RowsCount = grddamage.RowsCount;
                string LastServicesCell = (string)grddamage[RowsCount - 1, (int)GridColumn.ProductName].Value;
                ////Check whether the new row is already added
                if (LastServicesCell != "(NEW)")
                {
                    // AddRowProduct(RowsCount);
                    AddRowDamage(RowsCount);
                    //Clear (NEW) on other colums as well
                    ClearNew(RowsCount - 1);
                }

                //WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit
                //WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
                //CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
                //CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
                //CalculateTotalQuantity();
            }
        }
        private bool isNewRow(int CurRow)
        {
            if (grddamage[CurRow, (int)GridColumn.ProductName].Value == "(NEW)")
                return true;
            else
                return false;
        }
        private void ClearNew(int RowCount)
        {
           
        }

        private void ProductCode_Selected(object sender, EventArgs e)
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

            }
            int CurRow = grddamage.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            DataTable dt = Product.GetProductByCode(ct.DisplayText);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row

                    grddamage[(CurRow), (int)GridColumn.ProductName].Value = dr["EngName"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.

                    grddamage[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grddamage[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
                int RowsCount = grddamage.RowsCount;
                string LastServicesCell = (string)grddamage[RowsCount - 1, 3].Value;
                ////Check whether the new row is already added
                if (LastServicesCell != "(NEW)")
                {
                    AddRowDamage(RowsCount);
                    //Clear (NEW) on other colums as well
                    ClearNew(RowsCount - 1);
                }
                //WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit
                //WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
                //CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
                //CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
                //CalculateTotalQuantity();
                //CalculateTotalQuantity();
                //CalculateNetAmount();
            }
        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grddamage.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grddamage.RowsCount;

                object Qty = ((TextBox)sender).Text;
                //bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?

                //ProductQuantity = Qty.ToString();

                //if (IsInt == false)
                //{
                //    //Global.MsgError("The quantity you posted is invalid! Please post the integer value");
                //    Global.MsgError("The quantity Cannot be blank, Please Enter the valid quantity");

                //    grddamage[CurRow, (int)GridColumn.Qty].Value = "1";
                //    ((TextBox)sender).Text = "1";
                //    return;
                //}

                //Check whether the value of quantity is zero or not?
                if (Convert.ToDouble(Qty) == 0)
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    grddamage[CurRow, (int)GridColumn.Qty].Value = "1";
                    ((TextBox)sender).Text = " ";
                    return;
                }
                //string Qty1 = grddamage[CurRow, 4].Value.ToString();
                string PurchaseRate1 = grddamage[CurRow, (int)GridColumn.PurchaseRate].Value.ToString();
                double Amount = Convert.ToDouble(Qty.ToString()) * Convert.ToDouble(PurchaseRate1);
                grddamage[CurRow, (int)GridColumn.Total_Amount].Value = Amount.ToString();
                //WriteAmount(CurRow, Convert.ToInt32(Qty));
                //CalculateGrossAmount();
                ////Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                //WriteNetAmount(CurRow);
                //CalculateNetAmount();
                //CalculateTotalQuantity();
                //CalculateTotalQuantity();
               // CalculateNetAmount();

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
                //find the current row of source grid
                int CurRow = grddamage.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;
                int RowCount = grddamage.RowsCount;

                object PurchaseRate = ((TextBox)sender).Text;

                bool IsDouble = Misc.IsNumeric(PurchaseRate);//This function check whether variable is Double  or not?

                if (IsDouble == false)
                {
                    Global.MsgError("The Purchase Rate you posted is invalid! Please post the integer value");
                    //grddamage[CurRow, 6].Value = "";
                    grddamage[CurRow, (int)GridColumn.Qty].Value = "1";
                    return;
                }
                if (grddamage[CurRow, (int)GridColumn.Qty].Value.ToString() == " " || grddamage[CurRow, (int)GridColumn.Qty].Value.ToString() == "0")
                {
                    // MessageBox.Show("Please Enter Valid Quantity");
                    grddamage.Selection.Focus(new SourceGrid.Position(CurRow, (int)GridColumn.Qty), true);
                }
                else
                {
                    string Qty = grddamage[CurRow, (int)GridColumn.Qty].Value.ToString();
                    double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);
                    grddamage[CurRow, (int)GridColumn.Total_Amount].Value = Amount.ToString();
                    //CalculateTotalQuantity();
                   // CalculateNetAmount();
                    //CalculateGrossAmount();
                    //WriteNetAmount(CurRow);
                    //CalculateNetAmount();
                    //CalculateTotalQuantity();

                }

                // grdPurchaseInvoice.Selection.Focus(new SourceGrid.Position(CurRow, 4), true);
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
                Inventory.frmListOfProduct flp = new Inventory.frmListOfProduct(this);
                flp.ShowDialog();
            }
        }
        private void Product_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowsCount = grddamage.RowsCount;
            string LastServicesCell = (string)grddamage[RowsCount - 1, 3].Value;

            //Check whether the new row is already added
            if (LastServicesCell != "(NEW)")
            {
                AddRowDamage(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
        }
        private void Product_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            Inventory.frmListOfProduct flp = new Inventory.frmListOfProduct(this, e);
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
            FillAllGridRow(CurrRowPos);
        }
        private void FillAllGridRow(int RowPosition)
        {
            
        }

        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {
            if (IsSelected)
            {
                int CurRow = grddamage.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // CurrRowPos = ctx.Position.Row;
                ctx.Text = productname;
                purchaserate = purchaserate;
                productcode = productcode;
                productid = productid;
                grddamage[CurRow, (int)GridColumn.Code_No].Value = productcode;
                grddamage[CurRow, (int)GridColumn.PurchaseRate].Value = purchaserate;
                grddamage[CurRow, (int)GridColumn.ProductID].Value = productid;
                int RowsCount = grddamage.RowsCount;
                string LastServicesCell = (string)grddamage[RowsCount - 1, (int)GridColumn.ProductName].Value;
                //Check whether the new row is already added
                if (LastServicesCell == "(NEW)")
                {  
                    AddRowDamage(RowsCount);
                    //Clear (NEW) on other colums as well
                    ClearNew(RowsCount - 1);
                }

            }
            hasChanged = true;
        }

        //private void CalculateTotalQuantity()
        //{
        //    try
        //    {
        //        int TotalQuantity = 0;
        //        for (int i = 1; i < grddamage.RowsCount - 1; i++)                
        //        {
        //            if (i == grddamage.Rows.Count)
        //                return;
        //            int m_TotalQuantity = 0;
        //            string m_Value = Convert.ToString(grddamage[i, 4].Value);
        //            //string m_Value = ProductQuantity;
        //            if (m_Value.Length == 0)
        //                m_TotalQuantity = 0;
        //            else
        //                m_TotalQuantity = Convert.ToInt32(grddamage[i, 4].Value);
        //                //m_TotalQuantity = Convert.ToInt32(ProductQuantity);

        //            TotalQuantity += m_TotalQuantity;

        //            //lblTotalQty.Text = TotalQuantity.ToString();
        //            txtqty.Text = TotalQuantity.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //        //Global.MsgError("Error in Total Quantity calucation!"); 
        //    }
        //}
        //private void CalculateNetAmount()
        //{
        //    try
        //    {

        //        for (int i = 1; i < grddamage.RowsCount -1; i++)                
        //        {
        //            //Check if it is the (NEW) row.If it is ,it must be the last row.
        //            double NetAmount = 0;
        //            if (i == grddamage.Rows.Count)
        //                return;
        //            double m_NetAmount = 0;
        //            string m_Value = Convert.ToString(grddamage[i, 6].Value);
        //            if (m_Value.Length == 0)
        //                m_NetAmount = 0;
        //            else
        //                m_NetAmount = Convert.ToDouble(grddamage[i, 6].Value);

        //            NetAmount += m_NetAmount;
        //            //lblNetAmout.Text = NetAmount.ToString();
        //            txttotalvalue.Text = NetAmount.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError("Error in NetAmount calucation!");
        //    }
        //}
      //  public IMDIMainForm frmAccClass;
      // public static string frmAcclass;
        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            m_MDIForm.OpenFormArrayParam("frmAccClass");
        }
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
            //DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
            //DataRow drUserInfo = dtUserInfo.Rows[0];
            //ArrayList AccClassChildIDs = new ArrayList();
            //ArrayList tempParentAccClassChildIDs = new ArrayList();
            //AccClassChildIDs.Clear();
            //AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
            //AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
            //DataTable dt = new DataTable();
            //try
            //{
            //    dt = AccountClass.GetAccClassTable(AccClassID);
            //}
            //catch (Exception ex)
            //{
            //    Global.Msg(ex.Message);
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    TreeNode t = new TreeNode(dr[LangField].ToString());
            //    t.Tag = dr["AccClassID"].ToString();
            //    tempParentAccClassChildIDs.Clear();
            //    AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
            //    //Check if it is a parent Or if it has childs
            //    try
            //    {
            //        if (ChildCount((int)dr["AccClassID"]) > 0)
            //        {
            //            //t.IsContainer = true;
            //        }

            //        foreach (int itemIDs in AccClassChildIDs)  //To check if 
            //        {
            //            if (Convert.ToInt32(t.Tag) == itemIDs)
            //            {
            //                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //                loopCounter--;
            //                t.Checked = true;
            //                if (n == null)
            //                {
            //                    tv.Nodes.Add(t); //Primary Group
            //                    return;
            //                }
            //                else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
            //                {
            //                    t.Checked = true;
            //                    tv.Nodes.Add(t);
            //                    return;
            //                }
            //                else
            //                {
            //                    n.Nodes.Add(t); //Secondary Group
            //                }
            //            }
            //            if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
            //            {
            //                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE

           // isNew = true;
            //bool chkUserPermission = UserPermission.ChkUserPermission("DAMAGE_SAVE");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
            //    return;
            //}



            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));

                foreach (DataRow drVouConfigInfo in dtVouConfigInfo.Rows)
                {

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
                    try
                    {
                        //Code To Insert Damage Product in DataBAse
                        //Read from sourcegrid and store it to table
                        DataTable DamageInvoiceDetails = new DataTable();
                        DamageInvoiceDetails.Columns.Add("ProductCode");
                        DamageInvoiceDetails.Columns.Add("ProductName");
                        DamageInvoiceDetails.Columns.Add("Quantity");
                        DamageInvoiceDetails.Columns.Add("PurchaseRate");
                        DamageInvoiceDetails.Columns.Add("Amount");                
                        DamageInvoiceDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grddamage.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            DamageInvoiceDetails.Rows.Add(grddamage[i + 1, 2].Value, grddamage[i + 1, 3].Value, grddamage[i + 1, 4].Value, grddamage[i + 1, 5].Value, grddamage[i + 1, 6].Value, grddamage[i + 1, 7].Value);
                        }
                        DateTime DamageInvoice_Date = Date.ToDotNet(txtDate.Text);
                       // DateTime DebitNoteDate = Date.ToDotNet(txtDate.Text);

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        DateTime DamageProductInvoice_Date = Date.ToDotNet(txtDate.Text);

                        int seriesid = SeriesID.ID;
                        int depotid = LiDepotID.ID;
                        int projectid = liProjectID.ID;
                        BusinessLogic.Inventory.DamageItems dmgitems = new BusinessLogic.Inventory.DamageItems();
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
                            dmgitems.Create(DamageInvoiceDetails, seriesid, depotid, projectid, txtVoucherNo.Text, DamageProductInvoice_Date, AccClassID.ToArray(),OF);
                            //DataTable DamageInvoiceDetail,int seriesid,int depotid,int projectid,string voucher_No,DateTime invoicedate,int[] AccClassID
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            dmgitems.Create(DamageInvoiceDetails, seriesid, depotid, projectid, txtVoucherNo.Text, Convert.ToDateTime(txtDate.Text), AccClassID.ToArray(), OF);
                            //m_PurchaseInvoice.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), OrderNo, txtVoucherNo.Text, PurchaseInvoice_Date, txtRemarks.Text, PurchaseInvoiceDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check);
                        }

                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Damage Items  Invoice created successfully!");
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);


                        //////Do not close the form if do not close is checked
                        //if (!chkDoNotClose.Checked)
                        //    this.Close();
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
                        //Read from sourcegrid and store it to table
                        DataTable DamageInvoiceDetails = new DataTable();
                        DamageInvoiceDetails.Columns.Add("ProductCode");
                        DamageInvoiceDetails.Columns.Add("ProductName");
                        DamageInvoiceDetails.Columns.Add("Quantity");
                        DamageInvoiceDetails.Columns.Add("PurchaseRate");
                        DamageInvoiceDetails.Columns.Add("Amount");
                        DamageInvoiceDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grddamage.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            DamageInvoiceDetails.Rows.Add(grddamage[i + 1, 2].Value, grddamage[i + 1, 3].Value, grddamage[i + 1, 4].Value, grddamage[i + 1, 5].Value, grddamage[i + 1, 6].Value, grddamage[i + 1, 7].Value);
                        }
                        DateTime DamageInvoice_Date =Date.ToDotNet(txtDate.Text);
                     

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int seriesid = SeriesID.ID;
                        int depotid = LiDepotID.ID;
                        int projectid = liProjectID.ID;
                        BusinessLogic.Inventory.DamageItems dmgitems = new BusinessLogic.Inventory.DamageItems();
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (AccClassID.Count != 0)
                        {
                            dmgitems.EditDefectITems(Convert.ToInt32(txtDamageProductID.Text), DamageInvoiceDetails, seriesid, depotid, projectid, txtVoucherNo.Text, DamageInvoice_Date, AccClassID.ToArray(),OF);
                            //DataTable DamageInvoiceDetail,int seriesid,int depotid,int projectid,string voucher_No,DateTime invoicedate,int[] AccClassID
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            dmgitems.EditDefectITems(Convert.ToInt32(txtDamageProductID.Text), DamageInvoiceDetails, seriesid, depotid, projectid, txtVoucherNo.Text, Convert.ToDateTime(txtDate.Text), AccClassID.ToArray(), OF);
                           // dmgitems.Create(DamageInvoiceDetails, seriesid, depotid, projectid, txtVoucherNo.Text, Convert.ToDateTime(txtDate.Text), a);
                            //m_PurchaseInvoice.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), OrderNo, txtVoucherNo.Text, PurchaseInvoice_Date, txtRemarks.Text, PurchaseInvoiceDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check);
                        }
                        Global.Msg("Damage Items Updated Successfully successfully!");
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
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
        }
        private void ClearVoucher()
        {
            ClearJournal();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grddamage.Redim(2, 9);
            AddGridHeader(); //Write header part
            AddRowDamage(1);
        }
        private void ClearJournal()
        {
            txtVoucherNo.Clear(); //actually generate a new voucher no.
            //txtDate.Clear();
            grddamage.Rows.Clear();
            //cboSeriesName.SelectedIndex = 0;
            cboSeriesName.Text = string.Empty;
            cboProjectName.SelectedIndex = 0;   
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DAMAGE_ITEMS_MODIFY");
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

        private bool Navigation(Navigate NavTo)
        {
            try
            {
               // txtDamageProductID
                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtDamageProductID.Text);
                    if (PurchaseInvoiceIDCopy > 0)
                    {
                        VouchID = PurchaseInvoiceIDCopy;
                        PurchaseInvoiceIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtDamageProductID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }


                BusinessLogic.Inventory.DamageItems ditems = new BusinessLogic.Inventory.DamageItems();
                DataTable dtdamageinvoicemaster = ditems.NavigateDamageInvoiceMaster(VouchID, NavTo);
                //DataTable dtPurchaseInvoiceMaster = m_PurchaseInvoice.NavigatePurchaseInvoiceMaster(VouchID, NavTo);
                if (dtdamageinvoicemaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drDamage = dtdamageinvoicemaster.Rows[0]; //There is only one row. First row is the required record

               
               

                //Show the corresponding Depot in control

                DataTable dtDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drDamage["DepotID"]));
                if (dtDepotInfo.Rows.Count > 0)
                {
                    DataRow drDepotInfo = dtDepotInfo.Rows[0];
                    cboDepot.Text = drDepotInfo["DepotName"].ToString();
                }

                //show the corresoponding SeriesName in Combobox
                DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drDamage["SeriesID"]));
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
                txtVoucherNo.Text = drDamage["Voucher_No"].ToString();
                //txtDate.Text =Date.DBToSystem(drDamage["DPInvoice_Date"].ToString());
             

                txtDamageProductID.Text = drDamage["DamageProductID"].ToString();
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

                        txtfirst.Text = drDamage["Field1"].ToString();
                        txtsecond.Text = drDamage["Field2"].ToString();
                        txtthird.Text = drDamage["Field3"].ToString();
                        txtfourth.Text = drDamage["Field4"].ToString();
                        txtfifth.Text = drDamage["Field5"].ToString();
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

                        txtfirst.Text = drDamage["Field1"].ToString();
                        txtsecond.Text = drDamage["Field2"].ToString();
                        txtthird.Text = drDamage["Field3"].ToString();
                        txtfourth.Text = drDamage["Field4"].ToString();
                        txtfifth.Text = drDamage["Field5"].ToString();
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

                        txtfirst.Text = drDamage["Field1"].ToString();
                        txtsecond.Text = drDamage["Field2"].ToString();
                        txtthird.Text = drDamage["Field3"].ToString();
                        txtfourth.Text = drDamage["Field4"].ToString();
                        txtfifth.Text = drDamage["Field5"].ToString();

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

                        txtfirst.Text = drDamage["Field1"].ToString();
                        txtsecond.Text = drDamage["Field2"].ToString();
                        txtthird.Text = drDamage["Field3"].ToString();
                        txtfourth.Text = drDamage["Field4"].ToString();
                        txtfifth.Text = drDamage["Field5"].ToString();

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

                        txtfirst.Text = drDamage["Field1"].ToString();
                        txtsecond.Text = drDamage["Field2"].ToString();
                        txtthird.Text = drDamage["Field3"].ToString();
                        txtfourth.Text = drDamage["Field4"].ToString();
                        txtfifth.Text = drDamage["Field5"].ToString();
                    }


                }

                dsDamageItems.Tables["tblDamageItemMaster"].Rows.Add(cboSeriesName.Text, txtVoucherNo.Text,txtDate.Text,cboDepot.Text,cboProjectName.Text);
               // dsPurchaseInvoice.Tables["tblPurchaseInvoiceMaster"].Rows.Add(cboSeriesName.Text, drDamage["Voucher_No"].ToString(), Date.DBToSystem(drDamage["DPInvoice_Date"].ToString()), cboDepot.Text);
               // DataTable dtPurchaseInvoiceDetails = m_PurchaseInvoice.GetPurchaseInvoiceDetails(Convert.ToInt32(txtDamageProductID.Text));
                DataTable dtdamageinvoicedetails = ditems.GetDamageInvoiceDetails(Convert.ToInt32(txtDamageProductID.Text));
                for (int i = 1; i <= dtdamageinvoicedetails.Rows.Count; i++)
                {
                    DataRow drDetail = dtdamageinvoicedetails.Rows[i - 1];
                  //  grddamage[i, (int)GridColumn.SNo].Value = i.ToString();
                    grddamage[i, (int)GridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
                    grddamage[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grddamage[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                    grddamage[i, (int)GridColumn.PurchaseRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    grddamage[i, (int)GridColumn.Total_Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    



                    // AddRowProduct(grdPurchaseInvoice.RowsCount);
                    AddRowDamage(grddamage.RowsCount);
                    dsDamageItems.Tables["tblDamageItemDetails"].Rows.Add(i, drDetail["ProductCode"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Convert.ToDecimal(drDetail["PurchaseRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    //dsPurchaseInvoice.Tables["tblPurchaseInvoiceDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ServicesName"].ToString(),drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString(),Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(),Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(),Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(),Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtDamageProductID.Text), "DAMAGE");
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
            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Damage ITems Invoice?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }

            Navigation(Navigate.First);
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

        private void btnPrev_Click(object sender, EventArgs e)
        {

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Damage Items Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }

            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }

            Navigation(Navigate.Last);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            PurchaseInvoiceIDCopy = Convert.ToInt32(txtDamageProductID.Text);
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DAMAGE_ITEMS_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {
                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Invoice - " + txtDamageProductID.Text + "?") == DialogResult.Yes)
                {
                    DamageItems DelDamageItems = new DamageItems();
                    if (DelDamageItems.RemoveDamageItemsEntry(Convert.ToInt32(txtDamageProductID.Text)))
                    {
                        Global.Msg("Invoice -" + txtDamageProductID.Text + " deleted successfully!");
                        // Navigate to 1 step previous
                        if (!this.Navigation(Navigate.Prev))
                        {
                            //This must be because there are no records or this was the first one
                            //If this was the first, try to navigate to second
                            if (!this.Navigation(Navigate.Next))
                            {
                                //This was the last one, there are no records left. Simply clear the form and stay calm
                                ClearVoucher();
                                ChangeState(EntryMode.NEW);
                            }
                        }
                    }
                    else
                        Global.MsgError("There was an error while deleting invoice -" + txtDamageProductID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }
        }

      
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
            dsDamageItems.Clear();
            rptDamageItems rpt = new rptDamageItems();
            Misc.WriteLogo(dsDamageItems, "tblImage");
            rpt.SetDataSource(dsDamageItems);

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

            pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Name);
            rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

            pdvCompany_Address.Value = m_CompanyDetails.Address1;
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

            Navigation(Navigate.ID);

            //frmReportViewer frm = new frmReportViewer();
            
            //frm.SetReportSource(rpt);

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
                    //frmemail sendemail = new frmemail(FileName, 1);
                    //sendemail.ShowDialog();
                    rpt.Close();
                    return;
                default:
                    m_MDIForm.OpenFormArrayParam("frmReportViewer");
                    //frm.WindowState = FormWindowState.Maximized;
                    break;
            }

            //frm.Show();
           // frm.WindowState = FormWindowState.Maximized;
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
                    btnPrint_Click(sender, e);
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
                    btnPrint_Click(sender, e);
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
                    btnPrint_Click(sender, e);
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
            dsDamageItems.Clear();
            rptDamageItems rpt = new rptDamageItems();
            Misc.WriteLogo(dsDamageItems, "tblImage");
            rpt.SetDataSource(dsDamageItems);

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
                        //  sendemail.ShowDialog();
                       
                        rpt.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
                //m_MDIForm.OpenForm("frmReportViewer");
                frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("DAMAGE_ITEMS_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
            IsFieldChanged = false;
           // AddAdjustmentRow();
            cboSeriesName.SelectedIndex = 0;

        }
    }
}
