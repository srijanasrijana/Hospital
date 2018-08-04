using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BusinessLogic;
using System.Text.RegularExpressions;
using BusinessLogic.Inventory;

namespace Inventory
{
    public partial class frmProduct : Form,IfrmOpeningQuantity
    {
        //declaring variable
        private int? DepotID=null;
        private int? UnitMaintenanceID = null;
        private double? SalesRate=null;
        private int? Quantity=null;
        private double? PurchaseRate=null;
        private double? PurchaseDiscount=null;
        private double? TotalValue=null;
        private long txtColor = 0;
        private int getColor = 0;
        private long txtColorLocation = 0;
        private int getColorLocation = 0;
        private int ProductID = 0;
       
        public DataTable FromOpeningQuantity=new DataTable();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        private byte[] imgProduct = null;
        //For Search OPeration
        private ProductSearchIn m_SearchIn = ProductSearchIn.Product_Groups;
        private string filterString = "";
        private DataTable dtTable;
        private DataRow[] drFound;

        //Store the expansion state of treeview
        private bool IsExpand = false;
        // for the purpose of saving in auditlog
        private string OldPGroupdesc = " ";
        private string NewPGroupdesc = " ";
        private string OldProductdesc = " ";
        private string NewProductdesc = " ";
        private bool isNew;

        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            dtTable = Product.getProduct();
            drFound = dtTable.Select(filterString);

            PicProduct.SizeMode = PictureBoxSizeMode.StretchImage;
            PicProduct.BorderStyle = BorderStyle.FixedSingle;
           

             //show the units in combobox            
            DataTable dt = UnitMaintenance.GetUnitMaintenaceInfo(-1);
            //cboBaseUnit.Items.Add(new ListItem((0), "None"));
            foreach (DataRow dr in dt.Rows)
            {
                cboBaseUnit.Items.Add(new ListItem((int)dr["UnitMaintenanceID"],dr["UnitName"].ToString()));
           
            }
            //cboBaseUnit.SelectedIndex = 0;

            //Show Depot/Location in corresponding combobox

           DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
           cboDepot.Items.Add(new ListItem((0), "None"));
           foreach (DataRow drDepotInfo in dtDepotInfo.Rows)
           {
               cboDepot.Items.Add(new ListItem((int)drDepotInfo["DepotID"], drDepotInfo["DepotName"].ToString()));    
           }
           cboDepot.SelectedIndex = 0;
            //Resize to exact height and width
            this.Height = 489;
            this.Width = 920;


            cboSrchSearchIn1.Items.Clear();
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Account_Groups, LangMgr.Translate("PRODUCT_GROUP", "Product Groups")));
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Ledgers, LangMgr.Translate("PRODUCT", "Product")));
            //  cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Accounts_Under, LangMgr.Translate("DEBTORS_NAME", "Debtors Name")));


            cboSrchSearchIn1.SelectedIndex = 0;
            cboSrchOP1.SelectedIndex = 0;
            //Fill the account head in the treeview

            ShowProductHeadInTreeView(tvProduct, null, 0);
            ListProductGroup(cboParentGrp);
           

            //ListAccGroup(cboLdrAcGroup); //Ledger tab's Account group combo filling
            ListProductGroup(cboProductGroup);//Product tab's 

            ChangeState(EntryMode.NORMAL);
            if(Global.product==true)
            {
                tabProductSetup.SelectedIndex = 1;
                btnProductNew_Click(sender, e);
               // btnProductNew(sender,e);
            //tvProduct_AfterSelect
            }
          
        }

        //Recursive Function to Show Product Group in Treeview
        public void ShowProductHeadInTreeView(TreeView tv, TreeNode n, int Product_ID)
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

            DataTable dt;
 
            dt = Product.GetGroupTable(Product_ID);
          
   
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TreeNode t = new TreeNode(dr[LangField].ToString(), 0, 0);

                ShowProductHeadInTreeView(tv, t, Convert.ToInt16(dr["GroupID"].ToString()));

                if (n == null)

                    tv.Nodes.Add(t); //Primary Group

                else
                {
                    n.Nodes.Add(t); //Secondary Group


                }
                //Also add Product
                DataTable dtProduct = Product.GetProductTable(Convert.ToInt32(dr["GroupID"]));
              
                for (int j = 0; j < dtProduct.Rows.Count; j++)
                {
                    DataRow drProduct = dtProduct.Rows[j];
                    TreeNode tnProduct = new TreeNode(drProduct[LangField].ToString());
                    tnProduct.ForeColor = Color.DarkBlue;
                    t.Nodes.Add(tnProduct);
                }

            }

        }
  
        private void ChangeState(EntryMode Mode)
     
        {
            FromOpeningQuantity.Rows.Clear();
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                   
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    
                    break;
                case EntryMode.NEW:
                    
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    btncolor.Enabled = true;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;

            }
        }


        private void EnableControls(bool Enable)
        {
         
            txtGroupName.Enabled = cboParentGrp.Enabled = txtDescription.Enabled = 
            txtProductName.Enabled = cboProductGroup.Enabled = txtProductRemarks.Enabled = txtProductColor.Enabled = 
            txtProductCode.Enabled = cboBaseUnit.Enabled = txtQuantity.Enabled = txtPurchaseRate.Enabled = 
            txtTotalValue.Enabled = txtSalesRate.Enabled =txtPurchaseDiscount.Enabled=cboDepot.Enabled=txtImage.Enabled=btnBrowse.Enabled = 
            chkisvatapplicable.Enabled = chkisinventoryapplicable.Enabled = chkIsDecimal.Enabled =
            txtLdrAddress1.Enabled = txtLdrAddress2.Enabled = txtLdrCity.Enabled = txtLdrCompany.Enabled = txtLdrEmail.Enabled = txtLdrPerName.Enabled = 
            txtLdrTelephone.Enabled = txtLdrWebsite.Enabled = txtmaxlimit.Enabled = txtImage.Enabled = 
            Enable;
        }

        //Enables and disables the button states
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnGrpNew.Enabled = btnProductNew.Enabled = New;
            btnGrpEdit.Enabled = btnProductEdit.Enabled = Edit;
            btnGrpSave.Enabled = btnProductSave.Enabled = Save;
            btnGrpDelete.Enabled = btnProductDelete.Enabled = Delete;
            btnGrpCancel.Enabled = btnProductCancel.Enabled = Cancel;
        }

        //Fill the cboUnder List box with Product Head
        private void ListProductGroup(ComboBox ComboBoxControl)
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
            DataTable dt = Product.GetGroupTable(-1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));

            }

            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";

        }

        //Language switchability
        public void ManageLanguage()
        {
            //Set the font of whole form 
            this.Font = LangMgr.GetFont();


            LangMgr langMgr = new LangMgr();

            langMgr.AddTranslation("GROUP_NAME", lblGroupName);
            langMgr.AddTranslation("PARENT_GROUP", lblParentGroup);
            langMgr.AddTranslation("DESCRIPTION", lblDescription);
            langMgr.AddTranslation("TREEVIEW", tabDisplay.TabPages["tbTree"]);
            langMgr.AddTranslation("LISTVIEW", tabDisplay.TabPages["tbList"]);
            langMgr.AddTranslation("PRODUCT_GROUP", tabProductSetup.TabPages["tabGroup"]);
            langMgr.AddTranslation("PRODUCT", tabProductSetup.TabPages["tabProduct"]);
            langMgr.AddTranslation("NEW", btnGrpNew);
            langMgr.AddTranslation("EDIT", btnGrpEdit);
            langMgr.AddTranslation("SAVE", btnGrpSave);
            langMgr.AddTranslation("DELETE", btnGrpDelete);
            langMgr.AddTranslation("CANCEL", btnGrpCancel);
            langMgr.AddTranslation("NEW", btnProductNew);
            langMgr.AddTranslation("EDIT", btnProductEdit);
            langMgr.AddTranslation("SAVE", btnProductSave);
            langMgr.AddTranslation("DELETE", btnProductDelete);
            langMgr.AddTranslation("CANCEL", btnProductCancel);
            langMgr.BulkTranslate();

        
        }

        private void btnGrpSave_Click(object sender, EventArgs e)
        {
           
            //Check Validation
            if (!ValidateGroup())
                return;

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    isNew = true;
                    OldPGroupdesc = " ";
                    NewPGroupdesc = " ";
                    NewPGroupdesc = NewPGroupdesc + "Group Name" + "-" + txtGroupName.Text +","+ "ParentGroup" + "-" + cboParentGrp.Text+",";
                    if (tabProductSetup.SelectedIndex == 0) //Product Head
                    {

                        try
                        {
                            IProduct productHead = new Product();
                            productHead.Create(txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim(), OldPGroupdesc, NewPGroupdesc, isNew, txtColor);

                            Global.Msg("Product head created successfully!");
                 
                            ChangeState(EntryMode.NORMAL);
                        }
                        catch (SqlException ex)
                        {

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
                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                    }

                    else if (tabProductSetup.SelectedIndex == 2) //Product Selected
                    {
                        //Is Data Freezed?
                        //Get Datafreeze variable from Database
                        //if freeze then show error message and return

                        try
                        {

                            IProduct productHead = new Product();

                            productHead.Create(txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim(), OldPGroupdesc, NewPGroupdesc, isNew, txtColor);

                            Global.Msg("Product head created successfully!");
                            ChangeState(EntryMode.NORMAL);

                            ListProductGroup(cboParentGrp);

                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                    }

                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT:
                    
                    //Check whether the modified parent group is not under our own group
                    ArrayList ReturnIDs = new ArrayList();
                    
                    Product.GetProductsUnder(Convert.ToInt32(txtGroupID.Text.Trim()),ReturnIDs);
                    ListItem liGroupID = new ListItem();
                    liGroupID = (ListItem)cboParentGrp.SelectedItem;

                    if (ReturnIDs.BinarySearch(liGroupID.ID) > -1) //if found returns greater index than -1
                    {
                        Global.Msg("The parent group name selected is the child of its own group");
                        return;
                    }
                    else if(Convert.ToInt32(txtGroupID.Text.Trim()) == liGroupID.ID)
                    {
                        Global.Msg("The parent group name selected can not be itself.");
                        return;
                    }


                    try
                    {

                        isNew = false;
                        NewPGroupdesc = " ";
                        NewPGroupdesc = NewPGroupdesc + "Group Name" + "-" + txtGroupName.Text + "," + "ParentGroup" + "-" + cboParentGrp.Text + ",";
                        IProduct ProdHead = new Product();
                        long testcolor = txtColor;
                        ProdHead.Modify(Convert.ToInt32(txtGroupID.Text.Trim()), txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim(), OldPGroupdesc, NewPGroupdesc, isNew, testcolor);

                        Global.Msg("Product Group modified successfully!");
                        ChangeState(EntryMode.NORMAL);

                        
                        ListProductGroup(cboParentGrp);
                    }
                    catch (SqlException ex)
                    {

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
                                Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
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
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                    }//end switch(m_mode)

                  
                    UpdateProductTree();               
                    ListProductGroup(cboParentGrp);
                    ListProductGroup(cboProductGroup);

        }


        //Validate Form inputs under Group Tab
        private bool ValidateGroup()
        {
            FormHandle m_FHandle = new FormHandle();
            m_FHandle.AddValidate(txtGroupName, DType.NAME, "Invalid account group name. Please choose a valid group name");

            return m_FHandle.Validate();

        }

        private void UpdateProductTree()
        {
         
            tvProduct.Nodes.Clear();
            ShowProductHeadInTreeView(tvProduct, null, 0);
        }

        private void btnGrpNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("PRODUCT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
     
            ClearProductHeadForm();
            
            ChangeState(EntryMode.NEW);
            
           

        }

        //Clears the text of every field of Product Head form
        private void ClearProductHeadForm()
        {
            
            txtGroupName.Clear();
            cboParentGrp.Text = "";      
            txtDescription.Clear();
        }

        private void btnGrpEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PRODUCT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }

            //Not Editable untill a node is selected in the tvProduct treeView
            if (string.IsNullOrEmpty(txtGroupID.Text))
            {
                Global.MsgError("Please select a Group in the Tree View first.");
                return;
            }

            isNew = false;
            OldPGroupdesc = " ";
            OldPGroupdesc = OldPGroupdesc + "Group Name" + "-" + txtGroupName.Text+"," + "ParentGroup" + "-" + cboParentGrp.Text+",";
            btncolor.Enabled = true;
            ChangeState(EntryMode.EDIT);
        }

        /// <summary>
        /// Fills the Product fields from Product group id
        /// </summary>
        /// <param name="ID"></param>
        /// 
        private void FillProdGroupForm(int ID)
        {

            tabProductSetup.SelectedIndex = 0;

        //Get details from ID

            DataTable dtGroup = Product.GetGroupByID(ID, LangMgr.DefaultLanguage);
         

            DataRow drGroup = dtGroup.Rows[0];

            txtGroupID.Text = drGroup["ID"].ToString();

            txtGroupName.Text = drGroup["Name"].ToString();

            txtDescription.Text = drGroup["Remarks"].ToString();
            string strUnder = drGroup["Parent"].ToString();

            int previousColor =Convert.ToInt32(drGroup["BackColor"].ToString());
            btnpreviouscolor.ForeColor = Color.FromArgb(previousColor);
           
            if (strUnder == null)
                cboParentGrp.Text = "(MAIN ACCOUNT)";
            else
                cboParentGrp.Text = strUnder;
        }

        private void tvProduct_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearProductHeadForm();
            try
            {
                //Show the selected value in respective fields
                if (e.Node.ForeColor == Color.DarkBlue)//Product is being Selected
                {
                    tabProductSetup.SelectedIndex = 1;
                    txtProductName.Text = e.Node.Text;

                    int ID = Product.GetProductIDFromName(e.Node.Text, LangMgr.DefaultLanguage);
                    if (ID == null) //if no data is found
                    {
                        Global.Msg("ERROR: Please select the Product properly and then try again");
                        ClearProductHeadForm();
                        return;

                    }


                    //Fill the ledger form 
                 
                    FillProductForm(ID);
              
                }
                else//Product group is being selected
                {
                 
                    tabProductSetup.SelectedIndex = 0;

                    object m_GroupID = Product.GetGroupIDFromName(e.Node.Text, LangMgr.DefaultLanguage);

                    //Fill the textboxes and other fields
                    FillProdGroupForm(Convert.ToInt32(m_GroupID));
                }

                tvProduct.Focus();//Let the focus remain to treeview
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                //throw;

            }
            btnOpeningBalance.Enabled = false;

        }

        /// <summary>
        /// Takes the Product ID and fills the Product form of the corresponding ID
        /// </summary>
        /// <param name="ID"></param>

        private void FillProductForm(int ID)
        {
            try
            {

                //Get details from ID
                DataTable dtProduct = Product.GetProductInfo(ID, LangMgr.DefaultLanguage);
                foreach (DataRow drProduct in dtProduct.Rows)
                {
                    ClearProductForm();
                    txtProductID.Text = drProduct["ID"].ToString();
                    txtProductName.Text = drProduct["ProdName"].ToString();
                    txtProductCode.Text = drProduct["Code"].ToString();
                    txtProductColor.Text = drProduct["Color"].ToString();
                    txtProductRemarks.Text = drProduct["Remarks"].ToString();
                    if (drProduct["UnitMaintenanceID"].ToString() != "")
                    {
                        DataTable dtUnitInfo = UnitMaintenance.GetUnitMaintenaceInfo(Convert.ToInt32(drProduct["UnitMaintenanceID"]));
                        foreach (DataRow drUnitInfo in dtUnitInfo.Rows)
                        {
                            cboBaseUnit.Text = drUnitInfo["UnitName"].ToString();
                        }
                    }
                    cboProductGroup.Text = drProduct["GroupName"].ToString();
                    txtSalesRate.Text = drProduct["SalesRate"].ToString();
                    txtQuantity.Text = drProduct["Quantity"].ToString();
                    txtPurchaseRate.Text = drProduct["PurchaseRate"].ToString();
                    txtTotalValue.Text = drProduct["TotalValue"].ToString();
                    int ProductColor =Convert.ToInt32( drProduct["BackColor"].ToString());
                    btnPreviousProductColor.ForeColor = Color.FromArgb(ProductColor);
                    btnProductCurrentColor.ForeColor = Color.FromArgb(ProductColor);
                    txtColor =Convert.ToInt64( ProductColor);
                    if (Convert.ToInt32(drProduct["IsVatApplicable"].ToString()) == 1)
                    {
                        chkisvatapplicable.Checked = true;
                    }
                    else
                    {
                        chkisvatapplicable.Checked = false;
                    }
                    if (Convert.ToInt32(drProduct["IsInventoryApplicable"].ToString()) == 1)
                    {
                        chkisinventoryapplicable.Checked = true;
                    }
                    else
                    {
                        chkisinventoryapplicable.Checked = false;
                    }

                    chkIsDecimal.Checked = Convert.ToBoolean(drProduct["IsDecimalApplicable"]);

                    if (drProduct["Image"].ToString() == "")
                    {
                       PicProduct.Image = null;
                    }

                    else
                    {
                        PicProduct.Image = Misc.GetImageFromByte((byte[])drProduct["Image"]);
                    }
                    if(drProduct["DepotID"].ToString()!="")
                    {
                    DataTable dtDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drProduct["DepotID"]));
                    foreach (DataRow dr in dtDepotInfo.Rows)
                    {
                        cboDepot.Text = dr["DepotName"].ToString();                    
                    }
                    }

                    txtPurchaseDiscount.Text = drProduct["PurchaseDiscount"].ToString();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Clears all the fields of Product form
        private void ClearProductForm()
        {
         
            txtProductName.Clear();
            cboProductGroup.Text = "";
            txtProductRemarks.Clear();
            txtProductCode.Clear();
            txtProductColor.Clear();
            cboBaseUnit.Text = "";
            cboDepot.Text = "";
            txtSalesRate.Clear();
            txtQuantity.Clear();
            txtPurchaseRate.Clear();
            txtTotalValue.Clear();
            chkIsDecimal.Checked = chkisinventoryapplicable.Checked = chkisvatapplicable.Checked = false;
            txtLdrAddress1.Text = txtLdrAddress2.Text = txtLdrCity.Text = txtLdrCompany.Text = txtLdrEmail.Text = txtLdrPerName.Text = txtLdrTelephone.Text =
            txtLdrWebsite.Text = txtmaxlimit.Text = txtImage.Text = string.Empty;

            PicProduct.Image = null;
            btnProductCurrentColor.ForeColor = btnPreviousProductColor.ForeColor = Color.Black;
        }

        private void btnGrpDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PRODUCT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            //Not Delete able untill a node is selected in the tvProduct treeView
            if (string.IsNullOrEmpty(txtGroupID.Text))
            {
                Global.MsgError("Please select a Group in the Tree View first.");
                return;
            }

            //Check if a group has any child group or ledger.
            //If yes than do not allow to delete
            int groupId = Convert.ToInt32(txtGroupID.Text.Trim());
            if (Product.IsGroupBuiltIn(groupId))
            {
                Global.Msg("The Product Group is a System group.Sorry! cannot be deleted.");
                return;
            }
            DataTable dtGroup = Product.GetGroupTable(groupId);
            DataTable dtProduct = Product.GetProductTable(groupId);
            if(dtGroup.Rows.Count>0)
            {
                Global.MsgError("Sorry! you can not delete a Product Group that has child Product Group. First delete the child Product Group.");
                return;
            }else if(dtProduct.Rows.Count >0)
            {
                Global.MsgError("Sorry! you can not delete a Product Group that has a Ledger. First delete the Ledger.");
                return;
            }

            //Ask if the Product head is to be deleted
            if (Global.MsgQuest("Are you sure you want to delete the Product  head - " + txtGroupName.Text.Trim() + "?") == DialogResult.No)
                return;

            //If the user confirms deletion

            DataTable ProductGroupTrans = Product.FindProductGroupTransactPresent(Convert.ToInt32(txtGroupID.Text));
            if (ProductGroupTrans.Rows.Count != 0)
            {
                Global.Msg("Sorry, Transaction to the Product Group " + txtGroupName.Text + " has been found. Cannot delete the Product Group !");
                return;
            }
            else
            {

                IProduct prodHead = new Product();
                string delProdHead = txtGroupName.Text.Trim();
                prodHead.Delete(delProdHead);

                Global.Msg("Product Group - " + delProdHead + " deleted successfully!");

                ChangeState(EntryMode.NORMAL);
                tvProduct.Nodes.Clear();
                ShowProductHeadInTreeView(tvProduct, null, 0);
                ClearProductHeadForm();
            }
        }
        /// <summary>
        /// To check if btnopeningbalance has been clicked
        /// </summary>
        private bool isOpenedQty = false;
        
        /// <summary>
        /// Method to check if The Opening Quantity is empty or not.
        /// Empty Quantity is not allowed, Aleast 0 has to be provided.
        /// Works for both edit and new button
        /// </summary>
        /// <returns></returns>
        private bool isOpeningQtyEmpty()
        {
            if (FromOpeningQuantity.Rows.Count < 1 && isNew)
            {
                MessageBox.Show("Please Enter Opening Quantity along with cost price and selling price", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnOpeningBalance.PerformClick();
                return true;
            }
            else if (FromOpeningQuantity.Rows.Count < 1 && isOpenedQty == true)
            {
                MessageBox.Show("Please Enter Opening Quantity along with cost price and selling price", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnOpeningBalance.PerformClick();
                isOpenedQty = false;
                return true;
            }
            else
                return false;
        }
        private void btnProductSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtProductCode.Text == "")
                {
                    Global.Msg("Please Insert Proper Product Code and Try Again!");
                    txtProductCode.Focus();
                    return;
                }

                if (txtProductName.Text == "")
                {
                    Global.Msg("Please Insert Proper Product Name and Try Again!");
                    txtProductName.Focus();
                    return;
                }
                else if (cboProductGroup.Text == "")
                {
                    Global.Msg("Please Select Proper Product Group and Try Again!");
                    cboProductGroup.Focus();
                    return;
                }
                if (cboBaseUnit.SelectedIndex == -1)
                {
                    Global.Msg("Please Select a Base Unit and Try Again!");
                    cboBaseUnit.DroppedDown = true;
                    return;
                }
                //First Quantity must be added if empty value is supplied it is checked again.
                if (isOpeningQtyEmpty())
                    if (FromOpeningQuantity.Rows.Count < 1)
                    {
                        isOpeningQtyEmpty();
                    }

                btnOpeningBalance.Enabled = false;
                //Check Validation
                bool chkValidation = false;
                chkValidation = ValidateProduct();
                if (!chkValidation)
                {

                    return;
                }
                IProduct Product = new Product();
                ListItem lstProductGrp = (ListItem)cboProductGroup.SelectedItem;
                if (txtPurchaseDiscount.Text == "")
                    txtPurchaseDiscount.Text = "0";
                ListItem lstDepotID = (ListItem)cboDepot.SelectedItem;

                ListItem lstUnitMaintenceID = (ListItem)cboBaseUnit.SelectedItem;
                //initializing the variables
                if (cboDepot.SelectedIndex > 0)
                    DepotID = Convert.ToInt32(lstDepotID.ID);

                if (cboBaseUnit.SelectedIndex >= 0)
                    UnitMaintenanceID = Convert.ToInt32(lstUnitMaintenceID.ID);

                if (txtSalesRate.Text != "")
                    SalesRate = Convert.ToDouble(txtSalesRate.Text);

                if (txtQuantity.Text != "")
                    Quantity = Convert.ToInt32(txtQuantity.Text);

                if (txtPurchaseRate.Text != "")
                    PurchaseRate = Convert.ToDouble(txtPurchaseRate.Text);

                if (txtPurchaseDiscount.Text != "")
                    PurchaseDiscount = Convert.ToDouble(txtPurchaseDiscount.Text);

                if (txtTotalValue.Text != "")
                    TotalValue = Convert.ToDouble(txtTotalValue.Text);
                int isVatApplicable = 0;
                int isInventoryApplicable = 0;
                int isDecimalApplicable = 0;
                switch (m_mode)
                {
                    #region NEW
                    case EntryMode.NEW: //if new button is pressed
                        //Check if the Product code is already in use
                        if (!BusinessLogic.Product.ValidProductCode(txtProductCode.Text.Trim()))
                        {
                            Global.Msg("The product code is already in use.");
                            txtProductCode.Select();
                            return;
                        }
                        isNew = true;
                        NewProductdesc = " ";
                        OldProductdesc = " ";
                        NewProductdesc = NewProductdesc + "productname " + "-" + txtProductCode.Text + "," + "productgroup" + "-" + cboProductGroup.Text + "," + "color" + "-" + txtProductColor.Text + "," + "baseunit" + "-" + cboBaseUnit.Text + "," + "purchaserate" + "-" + txtPurchaseRate.Text + "," + "salesrate" + "-" + txtSalesRate.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "Opening:Qty" + "-" + txtQuantity.Text + "," + "Opening:value" + "-" + txtTotalValue.Text + ",";
                        if (tabProductSetup.SelectedIndex == 1) //Product Selected
                        {

                            try
                            {

                                if (chkisvatapplicable.Checked)
                                {
                                    isVatApplicable = 1;
                                }
                                else
                                    isVatApplicable = 0;

                                if (chkisinventoryapplicable.Checked)
                                {
                                    isInventoryApplicable = 1;
                                }
                                else
                                    isInventoryApplicable = 0;

                                isDecimalApplicable = chkIsDecimal.Checked ? 1 : 0;
                                bool result = Product.CreateProduct(txtProductName.Text.Trim(), Convert.ToInt32(lstProductGrp.ID), txtProductCode.Text.Trim(), txtProductColor.Text.Trim(), DepotID, txtProductRemarks.Text.Trim(), UnitMaintenanceID, SalesRate, Quantity, PurchaseRate, PurchaseDiscount, TotalValue, imgProduct, OldProductdesc, NewProductdesc, isNew, txtColor, isVatApplicable, isInventoryApplicable, isDecimalApplicable);

                                //Get current ProductID and save it to tblOpeningQuantity
                                //int ProdID = Ledger.GetLedgerIdFromName(txtProductName.Text.Trim(), LangMgr.DefaultLanguage);
                                Product pd = new BusinessLogic.Product();
                                int ProdID = pd.GetProductIdFromName(txtProductName.Text.Trim(), LangMgr.DefaultLanguage);

                                OpeningBalance.InsertProductOpeningQuantity(ProdID, FromOpeningQuantity);

                                if (result)
                                    Global.Msg("Product created successfully!");
                                else
                                    Global.Msg("There is some problem in Product creation");
                                ChangeState(EntryMode.NORMAL);

                            }
                            catch (SqlException ex)
                            {
                                #region catch sql exception

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
                        }
                        break;
                    #endregion

                    #region EDIT
                    case EntryMode.EDIT:
                        //Check if the Product code is already in use
                        if (!BusinessLogic.Product.ValidProductCode(txtProductCode.Text.Trim(), Convert.ToInt32(txtProductID.Text.Trim())))
                        {
                            Global.Msg("The product code is already in use.");
                            txtProductCode.Select();
                            return;
                        }
                        if (tabProductSetup.SelectedIndex == 1) //Product Selected
                        {
                            if (chkisvatapplicable.Checked)
                            {
                                isVatApplicable = 1;
                            }
                            else
                                isVatApplicable = 0;

                            if (chkisinventoryapplicable.Checked)
                            {
                                isInventoryApplicable = 1;
                            }
                            else
                                isInventoryApplicable = 0;

                            isDecimalApplicable = chkIsDecimal.Checked ? 1 : 0;

                            isNew = false;
                            NewProductdesc = " ";
                            NewProductdesc = NewProductdesc + "productname " + "-" + txtProductCode.Text + "," + "productgroup" + "-" + cboProductGroup.Text + "," + "color" + "-" + txtProductColor.Text + "," + "baseunit" + "-" + cboBaseUnit.Text + "," + "purchaserate" + "-" + txtPurchaseRate.Text + "," + "salesrate" + "-" + txtSalesRate.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "Opening:Qty" + "-" + txtQuantity.Text + "," + "Opening:value" + "-" + txtTotalValue.Text + ",";

                            try
                            {

                                IProduct prod = new Product();
                                bool result = prod.ModifyProduct(Convert.ToInt32(txtProductID.Text), txtProductName.Text.Trim(), Convert.ToInt32(lstProductGrp.ID), txtProductCode.Text.Trim(), txtProductColor.Text.Trim(), DepotID, txtProductRemarks.Text.Trim(), UnitMaintenanceID, SalesRate, Quantity, PurchaseRate, PurchaseDiscount, TotalValue, imgProduct, OldProductdesc, NewProductdesc, isNew, txtColor, isVatApplicable, isInventoryApplicable, isDecimalApplicable);
                                if (FromOpeningQuantity.Rows.Count > 0)
                                {
                                    OpeningBalance.InsertProductOpeningQuantity(Convert.ToInt32(txtProductID.Text), FromOpeningQuantity);
                                }
                                if (result)
                                    Global.Msg("Product modified successfully!");
                                else
                                    Global.Msg("There is some problem in Product modification");
                                ChangeState(EntryMode.NORMAL);



                            }
                            catch (Exception ex)
                            {
                                Global.MsgError(ex.Message);
                            }
                        }
                        break;
                    #endregion

                }//end switch(m_mode)

                UpdateProductTree();
                if (Global.product == true)
                {
                    this.Close();
                }
                IsExpand = true;
                btnToggleExpand.PerformClick();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex("[^a-zA-Z0-9]");

            //if has non AlpahNumeric char, return false, else return true.
            return rg.IsMatch(strToCheck) == true ? false : true;
        }
      
        //Validate Product fields
        private bool ValidateProduct()
        {
            bool chkAlphanumeric = false;
            bool bValidate = false;
            FormHandle m_FHandle = new FormHandle();
            IProduct Product = new Product();
                ListItem lstProductGrp = (ListItem)cboProductGroup.SelectedItem;
            if(txtSalesRate.TextLength>0)
                m_FHandle.AddValidate(txtSalesRate, DType.FLOAT, "Please Insert Numeric Sales Rate and Try Again!");  
            if(txtQuantity.TextLength>0)
            m_FHandle.AddValidate(txtQuantity, DType.INT, "Please Insert Integer Quantity and Try Again! ");
            if(txtPurchaseRate.TextLength>0)
                m_FHandle.AddValidate(txtPurchaseRate, DType.FLOAT, "Please Insert Numeric Purchase Rate and Try Again!");
            if(txtPurchaseDiscount.TextLength>0)
                m_FHandle.AddValidate(txtPurchaseDiscount, DType.FLOAT, "Please Insert Numeric Purchase Discount Rate and Try Again!");
            bValidate = m_FHandle.Validate();
                       
            return bValidate;
        }

        private void btnProductNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            
            btnProductColor.Enabled = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("PRODUCT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearProductForm();
            btnOpeningBalance.Enabled = true;
            ChangeState(EntryMode.NEW);
            this.ActiveControl = txtProductCode;
            
        }

        /// <summary>
        /// Clears the form if New is clicked.
        /// Clears and load the previous data if the Edit is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProductCancel_Click(object sender, EventArgs e)
        {
            ClearProductForm();
            if(isNew == false)
            {
                ClearProductHeadForm();
                FillProductForm(Convert.ToInt32(txtProductID.Text));
            }
            ChangeState(EntryMode.NORMAL);
        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PRODUCT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            
            //Not editable untill a node is selected in the tvProduct treeView
            if(string.IsNullOrEmpty(txtProductID.Text))
            {
                Global.MsgError("Please select a Product in the Tree View first.");
                return;
            }

            btnProductColor.Enabled = true;
            isNew = false;
            OldProductdesc = " ";
            OldProductdesc = OldProductdesc + "productname " + "-" + txtProductCode.Text + "," + "productgroup" + "-" + cboProductGroup.Text + "," + "color" + "-" + txtProductColor.Text + "," + "baseunit" + "-" + cboBaseUnit.Text + "," + "purchaserate" + "-" + txtPurchaseRate.Text + "," + "salesrate" + "-" + txtSalesRate.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "Opening:Qty" + "-" + txtQuantity.Text + "," + "Opening:value" + "-" + txtTotalValue.Text+",";
            ChangeState(EntryMode.EDIT);
            btnOpeningBalance.Enabled = true;
            //check the the corresponding Product is being used in transaction or not?          
            cboBaseUnit.Enabled = true;
        }

        private void tabProduct_Click(object sender, EventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {

            //Calculate the total field using purchase rate, purchase discount and Quantity
            //try
            //{
                
            //    double TotalValue = 0;
            //    if (txtPurchaseDiscount.Text == "")
            //        txtPurchaseDiscount.Text = "0";
            //    double PurchaseRateAfterDisc = ((Convert.ToDouble(txtPurchaseRate.Text)) - (Convert.ToDouble(txtPurchaseDiscount.Text)));
            //    TotalValue = Convert.ToInt32(txtQuantity.Text) * PurchaseRateAfterDisc;

            //    //Finally show the total amount in the total text box
            //    txtTotalValue.Text = TotalValue.ToString();
            //}
            //catch
            //{
            //    //Do nothing
            //}

        }

        private void txtPurchaseRate_TextChanged(object sender, EventArgs e)
        {


           // txtQuantity_TextChanged(sender , e);
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {

            try
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("PRODUCT_DELETE");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                    return;
                }

                //Not Delete able untill a node is selected in the tvProduct treeView
                if (string.IsNullOrEmpty(txtProductID.Text))
                {
                    Global.MsgError("Please select a Product in the Tree View first.");
                    return;
                }

                if (Product.IsProductBuiltIn(Convert.ToInt32(txtProductID.Text)))
                {
                    Global.Msg("The Product is a System Product.Sorry! cannot be deleted.");
                    return;
                }

                //DELETING THE SELECTED Product 

                if (Global.MsgQuest("Are you sure you want to delete the Product -" + txtProductName.Text + "?") == DialogResult.No)
                    return;


                DataTable ProductTrans = Product.FindProductTransactPresent(Convert.ToInt32(txtProductID.Text));
                if (ProductTrans.Rows.Count != 0)
                {
                    Global.Msg("Sorry, Transaction to the Product " + txtProductName.Text + " has been found. Cannot delete the Product !");
                    return;
                }
                else
                {
                    int totalQty = Product.GetTotalProductQuantity(Convert.ToInt32(txtProductID.Text));
                    if (totalQty > 0)
                    {
                        if (Global.MsgQuest(txtProductName.Text + " has total of " + totalQty.ToString() + " opening quantity. Do you want to continue?") == DialogResult.No)
                            return;
                    }

                    Product m_Product = new Product();
                    m_Product.DeleteProduct(Convert.ToInt32(txtProductID.Text));

                    Global.Msg("Product - " + txtProductName.Text + " deleted successfully!");

                    ChangeState(EntryMode.NORMAL);

                    tvProduct.Nodes.Clear();

                    ShowProductHeadInTreeView(tvProduct, null, 0);

                    ClearProductForm();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void txtPurchaseDiscount_TextChanged(object sender, EventArgs e)
        {
            txtQuantity_TextChanged(sender, e);
        }

        private void cboSrchSearchIn1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSrchOP1.Items.Clear();
            cboSrchOP2.Items.Clear();

            switch ((ProductSearchIn)cboSrchSearchIn1.SelectedIndex)
            {
                case ProductSearchIn.Product_Groups:

                // case ProductSearchIn.Product_Group_Under:
                case ProductSearchIn.Product:
                case ProductSearchIn.Debtors_Name:
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Begins_With, LangMgr.Translate("BEGINS_WITH", "Begins With")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Contains, LangMgr.Translate("CONTAINS", "Contains")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));
                    cboSrchOP2.Enabled = false;
                    txtSrchParam2.Enabled = false;
                    cboSrchOP1.SelectedIndex = 0;
                    break;

            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openIMG = new OpenFileDialog();
            openIMG.Filter = "Known graphics format (*.bmp,*.jpg,*.gif,*.png)|*.bmp;*.jpg;*.gif;*.jpeg;*.png";
            openIMG.ShowDialog();
            string imgPath = openIMG.FileName;
            txtImage.Text = imgPath;

            try
            {

                PicProduct.Image = Image.FromFile(imgPath);
                imgProduct = Misc.ReadBitmap2ByteArray(imgPath.ToString());
                //Global.Msg(imgPestideReg.Se);
            }
            catch (Exception ex)
            {
                //Probably cancelled the selected
            }
        }

        private void txtProductCode_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void txtProductCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (e.KeyChar.ToString()).ToUpper().ToCharArray()[0];
        }

        private void btncolor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            DialogResult result = colorDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                //btncolor.ForeColor = colorDlg.Color;
                btncurrentcolor.ForeColor = colorDlg.Color;
                txtColor = colorDlg.Color.ToArgb();
                //txtGroupName.ForeColor = colorDlg.Color;
            }
        }

        private void btnproductcolor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            DialogResult result = colorDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                //btncolor.ForeColor = colorDlg.Color;
                btnProductCurrentColor.ForeColor = colorDlg.Color;
                txtColor = colorDlg.Color.ToArgb();
                //txtGroupName.ForeColor = colorDlg.Color;
            }
        }

        private void btnproductcurrentcolor_Click(object sender, EventArgs e)
        {

        }

        private void btnopeningbalance_Click(object sender, EventArgs e)
        {
            isOpenedQty = true;
            if (m_mode == EntryMode.NEW)
                ProductID = 0;
            else
                ProductID = Convert.ToInt32(txtProductID.Text);

            OpeningQuantitySetting OBS = new OpeningQuantitySetting();
            try
            {

                OBS.ProductID= ProductID;
                OBS.ParentGroupID = ((ListItem)cboProductGroup.SelectedItem).ID;
            }
            catch
            {
                //Ignore 
            }
            frmOpeningProductQuantity frm = new frmOpeningProductQuantity(this, OBS);
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }
        public void AddOpeningQuantity(DataTable AllOpeningQuantity)
        {
            FromOpeningQuantity = AllOpeningQuantity;

        }

        private void tabDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillProductGroupInGrid(drFound);
        }
        private void FillProductGroupInGrid(DataRow[] drFound)
        {
            try
            {
                grdListView.Rows.Clear();
                //WriteHeader();
                switch (m_SearchIn)
                {
                    case ProductSearchIn.Product_Groups:
                    case ProductSearchIn.Product:
                        grdListView.Redim(drFound.Count() + 1, 2);
                        grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("Product Group");
                        grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Product Name");
                       // grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Debtors Name");
                        // grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                        grdListView[0, 0].Column.Width = 100;
                        grdListView[0, 1].Column.Width = 100;
                        //grdListView[0, 2].Column.Width = 100;
                        // grdListView[0, 3].Column.Width = 50;
                        break;


                }

                //Initialise the event handler
                SourceGrid.Cells.Controllers.CustomEvents CellClick = new SourceGrid.Cells.Controllers.CustomEvents();
                // CellClick.Click += new EventHandler(Grid_Click);

                SourceGrid.Cells.Controllers.CustomEvents CellMove = new SourceGrid.Cells.Controllers.CustomEvents();
                // CellMove.FocusEntered += new EventHandler(Grid_Click);

                for (int i = 1; i <= drFound.Count(); i++)
                {

                    DataRow dr = drFound[i - 1];

                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["ProductGroup"].ToString());


                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["ProductName"].ToString());
                    grdListView[i, 1].AddController(CellClick);
                    grdListView[i, 1].AddController(CellMove);

                    //grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["DebtorsName"].ToString());

                    //grdListView[i, 2].AddController(CellClick);
                    //grdListView[i, 2].AddController(CellMove);

                    //grdListView[i, 3] = new SourceGrid.Cells.Cell(DebitCredit);
                    grdListView[i, 0].AddController(CellClick);
                    grdListView[i, 0].AddController(CellMove);


                }
                //grdListView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ListItem li1 = (ListItem)cboSrchOP1.SelectedItem;
            ListItem m_SearchInItem = (ListItem)cboSrchSearchIn1.SelectedItem;
            ListItem li2 = (ListItem)cboSrchOP2.SelectedItem;
            if (li2 == null)
                li2 = new ListItem(0, "");//Begins_With
            m_SearchIn = (ProductSearchIn)m_SearchInItem.ID; //Set the private function searchIn so that gridclick() may know what is the current mode.
            try
            {
                Search((ProductSearchIn)m_SearchInItem.ID, (SearchOpProduct)li1.ID, txtSrchParam1.Text, (SearchOpProduct)li2.ID, txtSrchParam2.Text);
                tabDisplay.SelectedIndex = 1; //Select the listview
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void Search(ProductSearchIn m_SearchIn, SearchOpProduct SrchOP1, string SearchParam1, SearchOpProduct SrchOP2, string SearchParam2)
        {

            //Let the whole row to be selected
            grdListView.SelectionMode = SourceGrid.GridSelectionMode.Row;




            //Disable multiple selection
            grdListView.Selection.EnableMultiSelection = false;
            switch (m_SearchIn)
            {
                #region Product Groups Search
                case ProductSearchIn.Product_Groups:
                    this.filterString = "ProductGroup Like '" + SearchParam1 + "%'";
                    drFound = dtTable.Select(this.filterString);
                    FillProductGroupInGrid(drFound);
                    break;

                #endregion

                case ProductSearchIn.Product:
                    filterString = "ProductName like '" + SearchParam1 + "%'";
                    drFound = dtTable.Select(filterString);
                    FillProductGroupInGrid(drFound);
                    break;

               
            }
        }

        private void txtSrchParam1_TextChanged(object sender, EventArgs e)
        {
            if (txtSrchParam1.Text == "")
            {
                filterString = "";
                drFound = dtTable.Select(filterString);

                FillProductGroupInGrid(drFound);
            }
        }

        private void btnToggleExpand_Click(object sender, EventArgs e)
        {
            if (IsExpand)
            {
                tvProduct.CollapseAll();
                IsExpand = false;
                btnToggleExpand.Text = "Expand";
            }
            else
            {
                tvProduct.ExpandAll();
                IsExpand = true;
                btnToggleExpand.Text = "Collapse";
            }
        }

        private void btnGrpCancel_Click_1(object sender, EventArgs e)
        {
            ClearProductHeadForm();
            if(isNew == false)
            {
                FillProdGroupForm(Convert.ToInt32(txtGroupID.Text));
            }
            ChangeState(EntryMode.NORMAL);
        }

        private void tabGroup_Click(object sender, EventArgs e)
        {

        }
    }
}
