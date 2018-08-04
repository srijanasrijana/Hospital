using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic;

namespace Inventory
{
    public interface ListProduct
    {
        void AddProduct(int productid, string productcode, string productname, bool IsSpecialKey, double purchaserate, double salesrate, int qty, int defaultUnitID = 0);
    }
    public partial class frmListOfProduct : Form
    {
        private int ChildAccClassID;
        private int ParentAccClassID;            
        private string FilterString = "";
        private bool isSelected = false;
        private DataRow[] drFound;
        private DataTable dTable;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick; //Double click event holder
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        private enum GridColumn : int
        {
           SNo, Code_No, ProductName, GroupName, ProductPrice, SaleRate, ProductID, ClosingStock, UnitID
        };
        private Form m_Parent; //Holds the parent form
        private bool IsSpecialKey = false;
        private ListProduct m_ParentForm; //holds the selected CCY Code
        public frmListOfProduct()
        {
            InitializeComponent();
            
        }
        bool m_isInventory = false;
        public frmListOfProduct(Form ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (ListProduct)ParentForm;
            ////Set the selected font to everything
            this.Font = LangMgr.GetFont();
        }
        public frmListOfProduct(Form ParentForm, KeyEventArgs e)
        {
            InitializeComponent();
            m_ParentForm = (ListProduct)ParentForm;
            txtProductName.Focus();
            //string[] test = { "1" };
            //dTable = Ledger.GetLedgerDetail(test[0].ToString(), null, null,0);

            //dTable = Currency.GetCurrencyTable();
            dTable = Product.GetAllProduct();
            //drFound = dTable.Select();
            drFound = dTable.Select(FilterString);

            if (e.KeyData.ToString().Length == 1)
            {
                txtProductName.Text += e.KeyData.ToString().ToUpper();
                txtProductName.Text = txtProductName.Text.ToUpper();
                txtProductName.SelectionStart = txtProductName.Text.Length + 1;
            }
            //Set the selected font to everything
            this.Font = LangMgr.GetFont();
        }
   
        private void frmListOfProduct_Load(object sender, EventArgs e)
        {
            try
            {
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow dr = dtroleinfo.Rows[0];
                ChildAccClassID = Convert.ToInt32(dr["AccClassID"].ToString());
                ParentAccClassID = GetRootAccClassID();

                // dTable = Product.GetAllProduct();
                // dTable = Product.GetAllProduct(Global.ParentAccClassID);

               // if()
                dTable = Product.GetAllProduct(null, 0, " ", DateTime.Today, true, StockStatusType.DiffInStock, false);
                //drFound = dTable.Select();
                drFound = dTable.Select(FilterString);
                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(Productgrid_DoubleClick);
                gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
                gridKeyDown.KeyDown += new KeyEventHandler(Handle_KeyDown);
                grdProduct.SelectionMode = SourceGrid.GridSelectionMode.Row;


                //Finally fill all the values in the grid with no filter applied
                FillGrid();
                //this.ActiveControl = txtProductName;
                this.ActiveControl = txtproductCode;
                //Productgrid_DoubleClick(sender, null);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
           
        }
        public void Productgrid_KeyPress(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        public void Handle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                isSelected = false;
                // m_ParentForm.AddLedger("", 0, isSelected);
                this.Close();
                //m_ParentForm.AddLedger("");
            }

            if (e.KeyCode == Keys.Down)
            {
                this.grdProduct.Focus();
            }

            else if (e.KeyCode == Keys.Up)
            {
                this.grdProduct.Focus();
            }
            else
            {
                char key = (char)e.KeyData;

                if (char.IsLetterOrDigit(key))
                {
                    if (this.txtproductCode.Focused )
                    {
                        return;
                    }
                    if (this.txtproductCode.Text.Trim().Length > 0)
                    {
                        this.txtproductCode.Focus();
                        this.txtproductCode.Text += key.ToString().ToUpper();
                        txtproductCode.Text = txtproductCode.Text.ToUpper();
                        this.txtproductCode.SelectionStart = this.txtproductCode.Text.Length + 1;
                        return;
                    }

                    if (this.txtproductGroup.Text.Trim().Length > 0)
                    {
                        this.txtproductGroup.Focus();
                        this.txtproductGroup.Text += key.ToString().ToUpper();
                        txtproductGroup.Text = txtproductGroup.Text.ToUpper();
                        this.txtproductGroup.SelectionStart = this.txtproductGroup.Text.Length + 1;
                        return;
                    }
                   
                    if (!this.txtProductName.Focused)
                    {
                        this.txtProductName.Focus();
                        this.txtProductName.Text += key.ToString().ToUpper();
                        txtProductName.Text = txtProductName.Text.ToUpper();
                        this.txtProductName.SelectionStart = this.txtProductName.Text.Length + 1;
                    }


                }
            }
        }
        //Filters the datatable with the parameter name
        private void Filter()
        {
            //this.FilterString = "and p.ProductCode LIKE '%" + txtproductCode.Text + "%' AND p.EngName LIKE '%" + txtProductName.Text + "%";

            try
            {
                //dTable = Product.filterproduct(txtProductName.Text, txtproductCode.Text, txtproductGroup.Text);
                this.FilterString = "code LIKE '%" + txtproductCode.Text.Trim() + "%' AND ProductName LIKE '%" + txtProductName.Text.Trim() + "%'  AND GroupName LIKE '%" + txtproductGroup.Text.Trim() + "%' ";
                drFound = dTable.Select(this.FilterString);
            // BusinessLogic.Product.filterproduct(txtProductName.Text);
            }
                 //dTable.Product.filterproduct(txtProductName.Text);
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
           // Productgrid_DoubleClick(sender, null);  

            
            FillGrid();
            grdProduct.Width -= 5;
            grdProduct.Width += 5;
            
        }

        private void Productgrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                isSelected = true;
                //Get the Selected Row
                int CurRow = grdProduct.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext ProductName = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.ProductName));
                SourceGrid.CellContext Productid = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.ProductID));
                SourceGrid.CellContext productcode = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.Code_No));
                SourceGrid.CellContext purchaserate = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.ProductPrice));
                SourceGrid.CellContext salesrate = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.SaleRate));
                SourceGrid.CellContext qty = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.ClosingStock));
                SourceGrid.CellContext unitID = new SourceGrid.CellContext(grdProduct, new SourceGrid.Position(CurRow, (int)GridColumn.UnitID));

                //Call the interface function to add the text in the parent form container
                m_ParentForm.AddProduct(Convert.ToInt32(Productid.Value), productcode.Value.ToString(), ProductName.Value.ToString(), isSelected, Convert.ToDouble(purchaserate.Value), Convert.ToDouble(salesrate.Value), Convert.ToInt32(qty.Value),Convert.ToInt32(unitID.Value) );
                this.Dispose();
            }
            catch (Exception ex)
            {
                Global.Msg("Invalid selection");
            }
        }
        //Fills the grid with data with the filter applied
        private void FillGrid()
        {
            try
            {
                grdProduct.Rows.Clear();
                grdProduct.Redim(drFound.Count() + 1, 9);
                WriteHeader();

                for (int i = 1; i <= drFound.Count(); i++)
                {

                    DataRow dr = drFound[i - 1];

                    grdProduct[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());

                    //Productgrid[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell(dr["code"].ToString());
                    //Productgrid[i, (int)GridColumn.Code_No].AddController(dblClick);


                    //Productgrid[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell(dr["name"].ToString());
                    //Productgrid[i, (int)GridColumn.ProductName].AddController(dblClick);
                    //Productgrid[i, (int)GridColumn.ProductName].AddController(gridKeyDown);

                    //Productgrid[i, (int)GridColumn.GroupName] = new SourceGrid.Cells.Cell(dr["grouptype"].ToString());
                    //Productgrid[i, (int)GridColumn.GroupName].AddController(dblClick);
                    //Productgrid[i, (int)GridColumn.GroupName].AddController(gridKeyDown);

                    //Productgrid[i, (int)GridColumn.ProductPrice] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["purchaserate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    //Productgrid[i, (int)GridColumn.ProductPrice].AddController(dblClick);
                    //Productgrid[i, (int)GridColumn.ProductPrice].AddController(gridKeyDown);

                    //Productgrid[i, (int)GridColumn.SaleRate] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["salesrate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    //Productgrid[i, (int)GridColumn.SaleRate].AddController(dblClick);
                    //Productgrid[i, (int)GridColumn.SaleRate].AddController(gridKeyDown);

                    //Productgrid[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell(dr["productid"].ToString());
                    //Productgrid[i, (int)GridColumn.ProductID].AddController(dblClick);

                    grdProduct[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell(dr["code"].ToString());
                    grdProduct[i, (int)GridColumn.Code_No].AddController(dblClick);


                    grdProduct[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell(dr["ProductName"].ToString());
                    grdProduct[i, (int)GridColumn.ProductName].AddController(dblClick);
                    grdProduct[i, (int)GridColumn.ProductName].AddController(gridKeyDown);

                    grdProduct[i, (int)GridColumn.GroupName] = new SourceGrid.Cells.Cell(dr["GroupName"].ToString());
                    grdProduct[i, (int)GridColumn.GroupName].AddController(dblClick);
                    grdProduct[i, (int)GridColumn.GroupName].AddController(gridKeyDown);

                    grdProduct[i, (int)GridColumn.ProductPrice] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["OpenPurchaseRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdProduct[i, (int)GridColumn.ProductPrice].AddController(dblClick);
                    grdProduct[i, (int)GridColumn.ProductPrice].AddController(gridKeyDown);

                    grdProduct[i, (int)GridColumn.SaleRate] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["OpenSalesRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdProduct[i, (int)GridColumn.SaleRate].AddController(dblClick);
                    grdProduct[i, (int)GridColumn.SaleRate].AddController(gridKeyDown);

                    grdProduct[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell(dr["productID"].ToString());
                    grdProduct[i, (int)GridColumn.ProductID].AddController(dblClick);

                    grdProduct[i, (int)GridColumn.ClosingStock] = new SourceGrid.Cells.Cell(dr["ClosingStock"].ToString());
                    grdProduct[i, (int)GridColumn.ClosingStock].AddController(dblClick);

                    grdProduct[i, (int)GridColumn.UnitID] = new SourceGrid.Cells.Cell(dr["UnitID"].ToString());
                }

            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }

        //Fills the header of the grid with the required column names and also sets the width
        private void WriteHeader()
        {
            grdProduct[0,(int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S No.");
            grdProduct[0, (int)GridColumn.SNo].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdProduct[0, (int)GridColumn.SNo].Column.Width = 40;

            grdProduct[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grdProduct[0, (int)GridColumn.Code_No].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdProduct[0, (int)GridColumn.Code_No].Column.Width = 70;

            grdProduct[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdProduct[0, (int)GridColumn.ProductName].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            grdProduct[0, (int)GridColumn.ProductName].Column.Width = 200;

            grdProduct[0, (int)GridColumn.GroupName] = new SourceGrid.Cells.ColumnHeader("Group Name");
            grdProduct[0, (int)GridColumn.GroupName].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            grdProduct[0, (int)GridColumn.GroupName].Column.Width = 150;

            grdProduct[0, (int)GridColumn.ProductPrice] = new SourceGrid.Cells.ColumnHeader("Product Price"); 
            grdProduct[0, (int)GridColumn.ProductPrice].Column.Width = 120;
            grdProduct[0, (int)GridColumn.ProductPrice].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdProduct[0, (int)GridColumn.SaleRate] = new SourceGrid.Cells.ColumnHeader("Sales Rate");
            grdProduct[0, (int)GridColumn.SaleRate].Column.Width = 120;
            grdProduct[0, (int)GridColumn.SaleRate].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdProduct[0, (int)GridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("Product ID");
            grdProduct[0, (int)GridColumn.ProductID].Column.Width = 50;
            grdProduct[0, (int)GridColumn.ProductID].Column.Visible = false;

            grdProduct[0, (int)GridColumn.ClosingStock] = new SourceGrid.Cells.ColumnHeader("Closing Stock");
            grdProduct[0, (int)GridColumn.ClosingStock].Column.Width = 80;
            grdProduct[0, (int)GridColumn.ClosingStock].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdProduct[0, (int)GridColumn.UnitID] = new SourceGrid.Cells.ColumnHeader("UnitID");
            grdProduct[0, (int)GridColumn.UnitID].Column.Visible = false;

        }

        private void txtproductCode_TextChanged(object sender, EventArgs e)
        {
            Filter();
            //Productgrid_DoubleClick(sender, null);  
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            Filter();
            //Productgrid_DoubleClick(sender, null);  
        }


        private void txtProductGroup_TextChanged(object sender, EventArgs e)
        {
            Filter();
            //Productgrid_DoubleClick(sender, null);  
        }

        private void Productgrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Productgrid_DoubleClick(sender, null);  
        }

        private void frmListOfProduct_FormClosing(object sender, EventArgs e)
        {
            this.Close();
            //this.Dispose();
        }

        private void frmListOfProduct_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        private void frmListOfProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSelected = false;
             // void AddProduct(int productid, string productcode, string productname, bool IsSpecialKey,double purchaserate,double salesrate);
              m_ParentForm.AddProduct(0, " "," ", isSelected, 0, 0, 0);
            //m_ParentForm.AddProduct(0, "", "", isSelected, 0);
            this.Dispose();
        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        private void txtProductName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Handle_KeyDown(sender, e);
        }

        private void txtproductCode_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        private void btn_addproduct_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct();
            frm.ShowDialog();
            frmListOfProduct_Load(sender,e);
        }

        private int GetRootAccClassID()
        {
            if (ChildAccClassID > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(ChildAccClassID));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
        }
    }
}
