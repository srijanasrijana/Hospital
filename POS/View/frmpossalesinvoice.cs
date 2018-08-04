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
using System.IO;
using DateManager;
using System.Drawing.Printing;
using POS.POSInventory;

namespace Inventory.POSInventory
{
    public partial class frmpossalesinvoice : Form, ISalesInvoice,ICalculateDiscount
    {
        private bool checkisfirst=true;
        private long txtColor = 0;
        private int getColor = 0;
        private long txtProductColor = 0;
        private int getProductColor = 0;
        private double subtotal = 0;
        private int currentPage, pageCount;
        private string[] ProdId = new string[20];
        DataRow dtrow;
        private int gridindex;
        private DataTable dTable;
        private DataTable dtCategory;
        private DataTable dtProduct;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button[] btnPage;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeftProduct;
        private System.Windows.Forms.Button[] btnPageProduct;
        private System.Windows.Forms.Button btnRightProduct;
        private DataTable orderTab = new DataTable();
        private string CatID;
        private DataTable dtPrint;
        private string TotalPrintAmount;

        //for saving purpose
        List<int> AccClassID = new List<int>();
        private int SeriesID=0 ;
        private int SalesID = 0;
        private string SalesAC;
        private int PartyID = 0;
        private string PartyAC;
        private int DepotID = 0;
        private int OrderNo = 0;
        private string VoucherNumber;
        private DateTime POSSalesInvoiceDate;
        private string Remarks;
        private int ProjectID;
        private string CreatedBy;
        private DateTime CreatedDate;
        private string ModifiedBy;
        private DateTime ModifiedDate;
        private int Quantity;
        private double GrossAmount;
        private double Discount;
        private double NetAmount;
        private double TotalAmount;
        //for detail purpose
        private string ProductCode;
        private int ProductID;
        private double SalesRate;
        private double DiscountPercentage;
        //For Tax and VAT
        private double VAT = 0;
        private double TAX1 = 0;
        private double TAX2 = 0;
        private double TAX3 = 0;

        private double tax1amt = 0;
        private double tax2amt = 0;
        private double tax3amt = 0;
        private double vatamt = 0;
        
        //For Voucher Number
        TextBox txtVoucherNo = new TextBox();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        public frmpossalesinvoice()
        {
            InitializeComponent();
        }

        private void btncheckout_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtTotal.Text)<=0)
            {
                MessageBox.Show("Please Check The Products in the List","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            Global.isPrintBill = false;
            //For Sales Master
            SeriesID = 278;
            SalesID = 445;
            SalesAC = "Sales A/C";
            PartyID = 191;
            //PartyAC = "Other A/C";
            PartyAC="Cash In Hand";
            DepotID = 12;
            OrderNo = 0;
            VoucherNumber=GetVoucherNumber();
            //VoucherNumber = txtVoucherNo.Text;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            TextBox txtDate = new TextBox();
            TextBox txtCreatedDate=new TextBox();
           // txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            POSSalesInvoiceDate = Date.ToDotNet(txtDate.Text);
            Remarks = "POS Sales Invoice";
            ProjectID = 0;
            CreatedBy = User.CurrentUserName;

            CreatedDate = POSSalesInvoiceDate;
            //txtCreatedDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            //CreatedDate = Date.ToDotNet(txtCreatedDate.Text);
            ModifiedBy = " ";
           // ModifiedDate = Date.ToDotNet(txtCreatedDate.Text);
            ModifiedDate = POSSalesInvoiceDate;
            for (int i = 0; i < dgvDetail.Rows.Count - 1;i++ )
            {
                Quantity += Convert.ToInt32(dgvDetail.Rows[i].Cells[1].Value.ToString());
            }
            //MessageBox.Show(Quantity.ToString());
            
            GrossAmount = Convert.ToDouble(txtSub.Text);
            Discount = Convert.ToDouble(txtdiscount.Text);
            NetAmount = GrossAmount - Discount;
            tax1amt = 0;
            tax2amt = 0;
            tax3amt = 0;
            //vatamt = 0;
            TotalAmount = Convert.ToDouble(txtTotal.Text);
            TotalPrintAmount = TotalAmount.ToString();
            //For Sales Detail
            DataTable dtproductdetail = new DataTable();
            dtproductdetail.Columns.Add("ProductID");
            dtproductdetail.Columns.Add("ProductCode");
            dtproductdetail.Columns.Add("Quantity");
            dtproductdetail.Columns.Add("SalesRate");
            dtproductdetail.Columns.Add("Total");
            dtproductdetail.Columns.Add("ProductName");
            
            for (int j = 0; j < dgvDetail.Rows.Count - 1;j++)
            {
                //Convert.ToInt32(dgvDetail.Rows[i].Cells[1].Value.ToString());
                //string test = dgvDetail.Rows[j].Cells[2].Value.ToString();
                //MessageBox.Show(test);
                double Total = Convert.ToDouble(dgvDetail.Rows[j].Cells[2].Value.ToString());
                double qty = Convert.ToDouble(dgvDetail.Rows[j].Cells[1].Value.ToString());
                double rate = Total / qty;
                dtproductdetail.Rows.Add(dgvDetail.Rows[j].Cells[3].Value.ToString(), dgvDetail.Rows[j].Cells[4].Value.ToString(), qty.ToString(), rate.ToString(), Total.ToString(), dgvDetail.Rows[j].Cells[0].Value.ToString());
            }
            dtPrint = dtproductdetail;
            frmcheckout checkout = new frmcheckout(SeriesID, SalesID, SalesAC, PartyID, PartyAC, DepotID, OrderNo, VoucherNumber, POSSalesInvoiceDate, Remarks, ProjectID, CreatedBy, CreatedDate, Quantity, GrossAmount, Discount, NetAmount, tax1amt, tax2amt, tax3amt, vatamt, TotalAmount, dtproductdetail,txtSub.Text,txtdiscount.Text,txttax.Text);
            checkout.ShowDialog();
            if(Global.isFillGrid==false)
            {
                dgvDetail.Rows.Clear();
                orderTab.Rows.Clear();
                txtSub.Text = "";
                txtdiscount.Text = "";
                txttax.Text = "";
                txtTotal.Text = "";
            }
            //if(Global.isPrintBill==true)
            //{
            //    PrintReceipt();
            //}
        }

        private void frmpossalesinvoice_Load(object sender, EventArgs e)
        {
            txtproductpin.Focus();
            this.ActiveControl = txtproductpin;
            RefreshPage();
            panelnavigate.Visible = true;
            initOrderTable();

            string Vatvalue = Settings.GetSettings("DEFAULT_SALES_VAT");

            if (Vatvalue == "1")
            {
                DataTable dtvat = new DataTable();
                dtvat = Slabs.GetSlabInfo(SlabType.VAT);
                DataRow drvat = dtvat.Rows[0];
                VAT = Convert.ToDouble(drvat[3].ToString());
            }
            else
            {
                VAT = 0;
            }

           
           // var dgv = new DataGridView();
            //dgvDetail.RowTemplate.Height = 50;
            //foreach (DataGridViewRow row in dgvDetail.Rows)
            //{
            //    row.Height = 40;
            //}
        }
        private void RefreshPage()
        {
            panelCategory.Visible = true;
            panelitemlist.Visible = false;

            currentPage = 1;
            pageCount = FillCategory(currentPage);
            Navigation(pageCount, currentPage);
        }
        private void initOrderTable()
        {
            orderTab.Columns.Add("id");
            orderTab.Columns.Add("qty");
            orderTab.Columns.Add("item");
            orderTab.Columns.Add("code");
            orderTab.Columns.Add("cost");
            orderTab.Columns.Add("price");
            orderTab.Columns.Add("value");
        }

        private void btncategory_Click(object sender, EventArgs e)
        {
            panelnavigate.Visible = true;
            panelnavigate.Enabled = true;
            panelproductnavigate.Visible = false;
            panelproductnavigate.Enabled = false;
            RefreshPage();
            //checkisfirst = false;
            //panelCategory.Visible = true;
            //panelitemlist.Visible = false;
           // orderTab.Rows.Clear();
           //orderTab.Columns.Clear();
           // frmpossalesinvoice_Load(sender,e);
             
        }

        private void btncat1_Click(object sender, EventArgs e)
        {
            panelCategory.Visible = false;
            panelitemlist.Visible = true;
            //pageCount = FillCategory(currentPage);
           // Navigation(pageCount, currentPage);
            Button theCat = (Button)sender;
            string whichbtn = theCat.Name.Replace("btncat", "");
            string nowId = ProdId[int.Parse(whichbtn) - 1];
            currentPage = 1;
            pageCount = FillProduct(currentPage,nowId);
           // dTable = Product.GetAllProduct();
           
        }
        private void btncat_Click(object sender, EventArgs e)
        {
            panelCategory.Visible = false;
            panelitemlist.Visible = true;
            panelnavigate.Visible = false;
            panelnavigate.Enabled = false;
            panelproductnavigate.Visible = true;
            panelproductnavigate.Enabled = true;
            //pageCount = FillCategory(currentPage);
            // Navigation(pageCount, currentPage);
            Button theCat = (Button)sender;
            string whichbtn = theCat.Name.Replace("btncat", "");
            string nowId = ProdId[int.Parse(whichbtn) - 1];
            CatID = nowId;
            currentPage = 1;
            pageCount = FillProduct(currentPage, nowId);
            NavigationProduct(pageCount, currentPage);
           // Navigation(pageCount, currentPage);
            // dTable = Product.GetAllProduct();

        }
        private int FillCategory(int defaultPage)
        {
            int i, j, pageCount, StartCat, endCat;
            double pageCountdbl;
            string contname;
            SalesInvoiceModel smodel = new SalesInvoiceModel();
            try
            {
             //string strQuery = "select GroupID,EngName from Inv.tblProductGroup";
             //dtCategory = Global.m_db.SelectQry(strQuery, "tblProductGroup");
              dtCategory=smodel.GetProductCategory();
                //dtCategory = Product.GetAllProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            pageCountdbl = dtCategory.Rows.Count / 16.0;
           
            pageCount = (int)Math.Ceiling(pageCountdbl);
            
            StartCat = (defaultPage - 1) * 16;
            endCat = StartCat + 15;
           

            j = 0;
            for (i = StartCat; i <= endCat; i++)
            {
                if (i < dtCategory.Rows.Count)
                {
                    DataRow drCategory = dtCategory.Rows[i];
                    string productname = drCategory["EngName"].ToString();


                    getColor = Convert.ToInt32(drCategory["BackColor"].ToString());
                    contname = "btnCat" + (j + 1).ToString();
                    SetBtnImage(contname,drCategory["GroupID"].ToString(),getColor);
                    ProdId[j] = drCategory["GroupID"].ToString();

                    panelCategory.Controls[contname].Text = productname;

                    //int test = Convert.ToInt32(prodData[i]["TextColor"].ToString());
                    //panelCategory.Controls[contname].ForeColor = Color.FromArgb(test);
                }
                else
                {
                    contname = "btnCat" + (j + 1).ToString();
                    SetBtnImage(contname, "0",0);
                    ProdId[j] = "0";
                    panelCategory.Controls[contname].Text = "";
                }
                // MessageBox.Show("Product" + i+1 + "Saved");
                j++;
            }
            return pageCount;
        }
        private void SetBtnImage(string btnName, string prodId,int bcolor)
        {
            int ImageSize;
            MemoryStream ms;
            byte[] ImageData;
            Button btnImg = (Button)panelCategory.Controls[btnName];
            if (prodId != "0")
            {
                //ImageData = prod.getProductImage(prodId);
            }
            else
            {
                ImageData = null;
            }
            //if (ImageData != null)
            //{
            //    ImageSize = ImageData.Length;
            //    ms = new MemoryStream(ImageData, 0, ImageSize);
            //    panelCategory.Controls[btnName].BackgroundImage = Image.FromStream(ms);
            //    btnImg.TextAlign = ContentAlignment.TopRight;
               
            //    btnImg.Height = 101;
            //    btnImg.Width = 88;
              
            //}
            
                panelCategory.Controls[btnName].BackgroundImage = null;
                panelCategory.Controls[btnName].BackColor = Color.FromArgb(bcolor);
                btnImg.TextAlign = ContentAlignment.MiddleCenter;
                
                btnImg.Height =115;
                btnImg.Width = 122;
              

            //}
        }
        private void Navigation(int numPages, int currentPage)
        {
            if (numPages == 0) numPages = 1;
            int MaxPages = 8;
            int firstPage;
            if (numPages > MaxPages)
            {
                if (numPages - currentPage < 8)
                {
                    firstPage = numPages - 7;
                }
                else
                {
                    firstPage = currentPage;
                }
            }
            else
            {
                MaxPages = numPages;
                firstPage = 1;
            }
            panelnavigate.Controls.Clear();
            btnLeft = new Button();
            btnPage = new Button[numPages];
            btnRight = new Button();
            //available space : panel width - left button width and margin - right button width and margin
            //int btnSize = ((372 - 72) / MaxPages) - 2;
            int btnSize = ((461 - 84) / MaxPages) - 2;

            panelnavigate.Controls.Add(btnLeft);
            btnLeft.Name = "btnLeft";
            btnLeft.Size = new Size(40, 40);
            btnLeft.Text = "<";
            btnLeft.Margin = new Padding(1);
            btnLeft.UseVisualStyleBackColor = true;
            btnLeft.Click += new System.EventHandler(btnLeft_Click);

            if (currentPage == 1)
                btnLeft.Enabled = false;
            else
                btnLeft.Enabled = true;

            for (int i = 0; i < MaxPages; i++)
            {
                btnPage[i] = new Button();
                panelnavigate.Controls.Add(btnPage[i]);
                btnPage[i].Name = "btnPage[" + i.ToString() + "]";
                btnPage[i].Size = new Size(btnSize, 40);
               // btnPage[i].Size = new Size(375, 40);
               // btnPage[i].Text = (i + firstPage).ToString();
                btnPage[i].Text = (i + currentPage).ToString();
                btnPage[i].Margin = new Padding(1);
                btnPage[i].UseVisualStyleBackColor = true;
                btnPage[i].Click += new System.EventHandler(btnPage_Click);
                if (i + firstPage == currentPage) btnPage[i].Enabled = false;
            }

            panelnavigate.Controls.Add(btnRight);
            btnRight.Name = "btnRight";
            btnRight.Size = new Size(40, 40);

            btnRight.Text = ">";
            btnRight.Margin = new Padding(1);
            btnRight.UseVisualStyleBackColor = true;
            btnRight.Click += new System.EventHandler(btnRight_Click);
        }
        private void btnLeft_Click(object sender, EventArgs e)
        {
            currentPage -= 1;
            if (currentPage > pageCount) currentPage = pageCount;
            pageCount = FillCategory(currentPage);
            Navigation(pageCount, currentPage);
        }

        private void btnPage_Click(object sender, EventArgs e)
        {
            Button btnClk = (Button)sender;
            currentPage = int.Parse(btnClk.Text);
            pageCount = FillCategory(currentPage);
            Navigation(pageCount, currentPage);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if(btncat16.Text!="")
            {
            currentPage += 1;
            if (currentPage <1) currentPage = 1;
            pageCount = FillCategory(currentPage);
            Navigation(pageCount, currentPage);
            }
        }

        private void btnitem1_Click(object sender, EventArgs e)
        {

        }
        private void btnitem_Click(object sender, EventArgs e)
        {
            //dgvDetail.Rows.Clear();
            Button theProd = (Button)sender;
            string whichbtn = theProd.Name.Replace("btnitem", "");
            string nowId = ProdId[int.Parse(whichbtn) - 1];
            //for (int i = 0; i < dgvDetail.Rows.Count-1 ;i++ )
            //{
                
            //    if (nowId == dgvDetail.Rows[i].Cells[3].Value.ToString())
            //    {
            //       //int AddProductQuantity=Convert.ToInt32( dgvDetail.Rows[i].Cells[1].Value.ToString());
            //        dgvDetail.Rows[i].Cells[1].Value =Convert.ToString( Convert.ToInt32( dgvDetail.Rows[i].Cells[1].Value.ToString()) + 1);
            //    }

            //}GetAllProductByIDAndAccClassID
            //DataTable dtgetprod = SalesInvoiceModel.GetAllProductByID(Convert.ToInt32(nowId));
            DataTable dtgetprod = SalesInvoiceModel.GetAllProductByIDAndAccClassID(Convert.ToInt32(nowId),Global.ParentAccClassID);
            if (dtgetprod.Rows.Count > 0)
            {
                DataRow drprod = dtgetprod.Rows[0];
                string productname = drprod["engname"].ToString();

               //if(checkisfirst==false)
               //{
               //    initOrderTable();
               //}
               
                dtrow = orderTab.NewRow();
                dtrow["id"] = drprod["ProductID"].ToString();
                dtrow["qty"] = 1;
                for (int i = 0; i < dgvDetail.Rows.Count - 1; i++)
                {
                    if (nowId == dgvDetail.Rows[i].Cells[3].Value.ToString())
                    {   
                        //int AddProductQuantity=Convert.ToInt32( dgvDetail.Rows[i].Cells[1].Value.ToString());
                        //dgvDetail.Rows[i].Cells[1].Value = Convert.ToString(Convert.ToInt32(dgvDetail.Rows[i].Cells[1].Value.ToString()) + 1);
                        dtrow["qty"] = Convert.ToString(Convert.ToInt32(dgvDetail.Rows[i].Cells[1].Value.ToString()) + 1);
                        orderTab.Rows[i].Delete();
                        RefreshDataGrid();
                    }

                }
                dtrow["item"] = productname;
                dtrow["code"] = drprod["ProductCode"].ToString();
                dtrow["cost"] = drprod["PurchaseRate"].ToString();
                dtrow["price"] = drprod["SalesRate"].ToString();
                dtrow["value"] = double.Parse(dtrow["qty"].ToString()) * double.Parse(dtrow["price"].ToString());
                orderTab.Rows.Add(dtrow);
            }
            RefreshDataGrid();
        }
        private void RefreshDataGrid()
        {
            int i;
            double SubTotal = 0, GST = 0, Service = 0, Total = 0;
            dgvDetail.Rows.Clear();
            if (orderTab.Rows.Count > 0)
            {
                for (i = 0; i < orderTab.Rows.Count; i++)
                {
                    //string[] rowContent = { "", orderTab.Rows[i]["qty"].ToString(), orderTab.Rows[i]["item"].ToString(), orderTab.Rows[i]["value"].ToString() };
                    string[] rowContent = { orderTab.Rows[i]["item"].ToString(), orderTab.Rows[i]["qty"].ToString(), orderTab.Rows[i]["value"].ToString(), orderTab.Rows[i]["id"].ToString(), orderTab.Rows[i]["code"].ToString() };
                    dgvDetail.Rows.Add(rowContent);
                    SubTotal += double.Parse(orderTab.Rows[i]["value"].ToString());
                }
                //Service = SubTotal * Core.Config.Service / 100;
                //GST = SubTotal * Core.Config.Tax / 100;
                //Total = SubTotal + GST + Service;
                //Total=SubTotal+Convert.ToDouble( txttax.Text)+Convert.ToDouble( txtdiscount.Text);
            }
            txtSub.Text = SubTotal.ToString("#,##0.00");
            vatamt = (VAT / 100) * SubTotal;
            txttax.Text = vatamt.ToString();
            //txtdiscount.Text = "0";
            //txtGST.Text = GST.ToString("#,##0.00");
            //txtService.Text = Service.ToString("#,##0.00");
            Total = SubTotal + Convert.ToDouble(txttax.Text) - Convert.ToDouble(txtdiscount.Text);
            txtTotal.Text = Total.ToString("#,##0.00");
            foreach (DataGridViewRow row in dgvDetail.Rows)
            {
                row.Height = 40;
            } 
        }

        private void txtGST_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //DataGridViewCell clickedCell;
            //if (e.Button == MouseButtons.Right)
            //{
            //    dgvDetail.ClearSelection();
            //    clickedCell = dgvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //    clickedCell.Selected = true;
            //    cmsDataGridView.Show(dgvDetail, e.Location);
            //}
        }

        private void dgvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                //MessageBox.Show("You Have Selected " + (e.RowIndex + 1).ToString() + " Row Button");
            }

        }

        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string productaName=dgvDetail.Rows[e.RowIndex].Cells[0].Value.ToString();
                int quantity =Convert.ToInt32( dgvDetail.Rows[e.RowIndex].Cells[1].Value.ToString());
                double value = Convert.ToDouble(dgvDetail.Rows[e.RowIndex].Cells[2].Value.ToString());
                int rowindexvalue = e.RowIndex;
                frmposupdategrid updategrid = new frmposupdategrid(rowindexvalue,this,productaName,quantity,value);
                updategrid.ShowDialog();
                gridindex = e.RowIndex;
            }
            if (e.ColumnIndex == 0)
            {
                gridindex = e.RowIndex;
                //MessageBox.Show("You Have Selected " + (e.RowIndex + 1).ToString() + " Row Button");
            }
        }
        public void UpdateQuantity(int Quantity, int rindex,double finalvalue)
        {
            dgvDetail.Rows[rindex].Cells[1].Value = Quantity.ToString();
            dgvDetail.Rows[rindex].Cells[2].Value = finalvalue.ToString();
            int count = dgvDetail.Rows.Count;
            subtotal = 0;
            for (int i = 0; i < dgvDetail.Rows.Count-1; i++)
            {
                subtotal +=Convert.ToDouble(dgvDetail.Rows[i].Cells[2].Value.ToString());
            }
            //subtotal =Convert.ToDouble( finalvalue.ToString());
            txtSub.Text = subtotal.ToString();
            txtTotal.Text = (subtotal + Convert.ToDouble(txttax.Text)).ToString();
            
            
        }
        private int FillProduct(int defaultPage,string categoryid)
        {
            int i, j, pageCount, StartProduct, endProduct;
            double pageCountdbl;
            string contname;
            SalesInvoiceModel smodel = new SalesInvoiceModel();
            try
            {
                dtProduct = SalesInvoiceModel.GetAllProductbycategoryid(categoryid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            pageCountdbl = dtProduct.Rows.Count / 16.0;

            pageCount = (int)Math.Ceiling(pageCountdbl);

            StartProduct = (defaultPage - 1) * 16;
            endProduct = StartProduct + 15;


            j = 0;
            for (i = StartProduct; i <= endProduct; i++)
            {
                if (i < dtProduct.Rows.Count)
                {
                    DataRow drProduct = dtProduct.Rows[i];
                    string productname = drProduct["EngName"].ToString();
                    getProductColor = Convert.ToInt32(drProduct["BackColor"].ToString());
                    contname = "btnitem" + (j + 1).ToString();
                    SetBtnImageProduct(contname, drProduct["ProductID"].ToString(),getProductColor);
                    ProdId[j] = drProduct["ProductID"].ToString();
                    panelitemlist.Controls[contname].Text = productname;

                    //int test = Convert.ToInt32(prodData[i]["TextColor"].ToString());
                    //panelCategory.Controls[contname].ForeColor = Color.FromArgb(test);
                }
                else
                {
                    contname = "btnitem" + (j + 1).ToString();
                    SetBtnImageProduct(contname, "0",0);
                    ProdId[j] = "0";
                    panelitemlist.Controls[contname].Text = "";
                }
                
                j++;
            }
            return pageCount;
        }
        private void SetBtnImageProduct(string btnName, string prodId,int bcolor)
        {
            //For Image Purpose
            Bitmap bmp,image,resized;


            int ImageSize=0;
            MemoryStream ms;
            byte[] ImageData;
            Button btnImg = (Button)panelitemlist.Controls[btnName];
           //Button btnImgage = (Button)panelitemlist.Controls[btnName];
            SalesInvoiceModel sm=new SalesInvoiceModel();
            if (prodId != "0")
            {
                ImageData = sm.GetProductImgFromID(prodId);
            }
            else
            {
                ImageData = null;
            }
          
            if (ImageData != null)
            {
                //Bitmap image = Bitmap.FromFile(ImageData) as Bitmap;
                //Bitmap resized = new Bitmap(image, new Size(30, 30));
                //btnImg.Image = resized;
                //btnImg.Text = "Button";
                //btnImg.ImageAlign = ContentAlignment.MiddleLeft;
                //btnImg.TextImageRelation = TextImageRelation.ImageBeforeText;
                //btnImg.TextAlign = ContentAlignment.MiddleRight;
                ImageSize = ImageData.Length;
                ms = new MemoryStream(ImageData, 0, ImageSize);
                btnImg.Image = Image.FromStream(ms);

                bmp = new Bitmap(System.Drawing.Image.FromStream(ms));
                image = bmp;
                //resized = new Bitmap(image, new Size(110, 83));
                resized = new Bitmap(image, new Size(80, 53));
                btnImg.Image = resized;
                btnImg.TextImageRelation = TextImageRelation.ImageAboveText;
                panelitemlist.Controls[btnName].BackColor = Color.FromArgb(bcolor);
               // panelitemlist.Controls[btnName].BackgroundImage = Image.FromStream(ms);
                //panelitemlist.Controls[btnName].BackgroundImageLayout = ImageLayout.Stretch;
                btnImg.TextAlign = ContentAlignment.BottomCenter;
                btnImg.ImageAlign = ContentAlignment.MiddleCenter;
                btnImg.Height = 115;
                btnImg.Width = 122;
               
            }
            else
            {
                btnImg.Image = null;
                panelitemlist.Controls[btnName].BackgroundImage = null;
                panelitemlist.Controls[btnName].BackColor = Color.FromArgb(bcolor);
                btnImg.TextAlign = ContentAlignment.MiddleCenter;
                btnImg.Height = 115;
                btnImg.Width = 122;
            }


            //}
        }

        private void txtdiscount_Click(object sender, EventArgs e)
        {
            frmkeypad keypad = new frmkeypad(this);
            keypad.ShowDialog();
        }
        public void DiscountAmount(string DiscountAmount)
        {
            txtdiscount.Text = DiscountAmount.ToString();
            double amtbeforetax = Convert.ToDouble(txtSub.Text) -Convert.ToDouble( DiscountAmount);
            vatamt = (VAT / 100) * amtbeforetax;
            txttax.Text = vatamt.ToString("#,##0.00");
            double amtaftertax = amtbeforetax + vatamt;
            //txtTotal.Text =Convert.ToString(Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(DiscountAmount));
            txtTotal.Text = amtaftertax.ToString();
        }

        public void deletegridrow()
        {
            orderTab.Rows[gridindex].Delete();
            RefreshDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            orderTab.Rows[gridindex].Delete();
            RefreshDataGrid();
        }

        public string GetVoucherNumber()
        {
            try
            {
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(278));
                    txtVoucherNo.Enabled = true;
                    if (NumberingType == "AUTOMATIC")
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(278));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            // disables all controls except cboSeriesName
                        }
                        else
                        {
                            txtVoucherNo.Text = m_vounum.ToString();
                        }
                    }
                    return txtVoucherNo.Text;
                }
           
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return "";
            }
        }
        private void NavigationProduct(int numPages, int currentPage)
        {
            if (numPages == 0) numPages = 1;
            int MaxPages = 8;
            int firstPage;
            if (numPages > MaxPages)
            {
                if (numPages - currentPage < 8)
                {
                    firstPage = numPages - 7;
                }
                else
                {
                    firstPage = currentPage;
                }
            }
            else
            {
                MaxPages = numPages;
                firstPage = 1;
            }

            panelproductnavigate.Controls.Clear();
            btnLeftProduct = new Button();
            btnPageProduct = new Button[numPages];
            btnRightProduct = new Button();
            //available space : panel width - left button width and margin - right button width and margin
            //int btnSize = ((372 - 72) / MaxPages) - 2;
            int btnSize = ((461 - 84) / MaxPages) - 2;

            panelproductnavigate.Controls.Add(btnLeftProduct);
            btnLeftProduct.Name = "btnLeft";
            btnLeftProduct.Size = new Size(40, 40);
            btnLeftProduct.Text = "<";
            btnLeftProduct.Margin = new Padding(1);
            btnLeftProduct.UseVisualStyleBackColor = true;
            btnLeftProduct.Click += new System.EventHandler(btnLeftProduct_Click);

            if (currentPage == 1)
                btnLeftProduct.Enabled = false;
            else
                btnLeftProduct.Enabled = true;

            for (int i = 0; i < MaxPages; i++)
            {
                btnPageProduct[i] = new Button();
                panelproductnavigate.Controls.Add(btnPageProduct[i]);
                btnPageProduct[i].Name = "btnPage[" + i.ToString() + "]";
                btnPageProduct[i].Size = new Size(btnSize, 40);
                // btnPage[i].Size = new Size(375, 40);
                // btnPage[i].Text = (i + firstPage).ToString();
                btnPageProduct[i].Text = (i + currentPage).ToString();
                btnPageProduct[i].Margin = new Padding(1);
                btnPageProduct[i].UseVisualStyleBackColor = true;
                btnPageProduct[i].Click += new System.EventHandler(btnPageProduct_Click);
                if (i + firstPage == currentPage) btnPageProduct[i].Enabled = false;
            }

            panelproductnavigate.Controls.Add(btnRightProduct);
            btnRightProduct.Name = "btnRight";
            btnRightProduct.Size = new Size(40, 40);

            btnRightProduct.Text = ">";
            btnRightProduct.Margin = new Padding(1);
            btnRightProduct.UseVisualStyleBackColor = true;
            btnRightProduct.Click += new System.EventHandler(btnRightProduct_Click);
        }
        private void btnLeftProduct_Click(object sender, EventArgs e)
        {
            currentPage -= 1;
            if (currentPage > pageCount) currentPage = pageCount;
            pageCount = FillProduct(currentPage, CatID);
            NavigationProduct(pageCount, currentPage);
        }

        private void btnPageProduct_Click(object sender, EventArgs e)
        {
            Button btnClk = (Button)sender;
            currentPage = int.Parse(btnClk.Text);
            pageCount = FillProduct(currentPage, CatID);
            NavigationProduct(pageCount, currentPage);
        }

        private void btnRightProduct_Click(object sender, EventArgs e)
        {
            if(btnitem16.Text!="")
            {
            currentPage += 1;
            if (currentPage < 1) currentPage = 1;
            pageCount = FillProduct(currentPage,CatID);
            NavigationProduct(pageCount, currentPage);
            }
           
        }

        private void txtproductpin_KeyDown(object sender, KeyEventArgs e)
        {
            
            //e.KeyValue==13
            if(e.KeyCode==Keys.Enter)
            {
                FormHandle m_FHandle = new FormHandle();
                if (txtproductpin.Text == "")
                {
                    MessageBox.Show("ProductPin Cant Be Blank");
                    return;
                }
                //if (txtproductpin.TextLength > 0)
                //{
                //    m_FHandle.AddValidate(txtproductpin, DType.INT, "Please Insert Number Only ");
                //}
                Regex regex = new Regex("^[0-9]*$");
                if (regex.IsMatch(txtproductpin.Text))
                {
                   // DataTable dtgetprod = SalesInvoiceModel.GetAllProductByID(Convert.ToInt32(txtproductpin.Text));
                    DataTable dtgetprod = SalesInvoiceModel.GetAllProductByIDAndAccClassID(Convert.ToInt32(txtproductpin.Text), Global.ParentAccClassID);
                    if (dtgetprod.Rows.Count > 0)
                    {
                        DataRow drprod = dtgetprod.Rows[0];
                        string productname = drprod["engname"].ToString();
                        dtrow = orderTab.NewRow();
                        dtrow["id"] = drprod["ProductID"].ToString();
                        dtrow["qty"] = 1;

                        dtrow["item"] = productname;
                        dtrow["code"] = drprod["ProductCode"].ToString();
                        dtrow["cost"] = drprod["PurchaseRate"].ToString();
                        dtrow["price"] = drprod["SalesRate"].ToString();
                        dtrow["value"] = double.Parse(dtrow["qty"].ToString()) * double.Parse(dtrow["price"].ToString());
                        orderTab.Rows.Add(dtrow);
                    }
                    RefreshDataGrid();
                }
                else
                {
                    MessageBox.Show("Number Only Allowed");
                    return;
                }
                txtproductpin.Text = "";
            }
        }
        private void PrintReceipt()
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument pringDocument = new PrintDocument();
            printDialog.Document = pringDocument;

            pringDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            DialogResult result = printDialog.ShowDialog();
            if(result==DialogResult.OK)
            {
                pringDocument.Print();
            }
        }
        void printDocument_PrintPage(object sender,PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New",10);

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offSet = 40;

            graphic.DrawString("BentRay Technology",new Font("Courier New",14),new SolidBrush(Color.Black),startX,startY);
            offSet = offSet + 5;

            graphic.DrawString("Kathmandu,Nepal", new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY+offSet);
            offSet = offSet + 20;

            graphic.DrawString("Tax Invoice", new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offSet);
            offSet = offSet + 20;
            string sn = "sn".PadRight(5);
            string ProductHeadParticular = "Particulars".PadRight(15);
            string ProductHeadQuantity = "Qty".PadRight(5);
            string ProductHeadRate = "Rate".PadRight(5);
            string ProductHeadAmount = "Amount".PadRight(10);

            string ProductHeadLine =sn+ProductHeadParticular + ProductHeadQuantity + ProductHeadRate + ProductHeadAmount;
            graphic.DrawString(ProductHeadLine, new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offSet);
            offSet = offSet + 20;
            graphic.DrawString("------------------------------------", new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offSet);
            offSet = offSet + 20;
            for (int i = 0; i < dtPrint.Rows.Count;i++ )
            {
                DataRow drPrint = dtPrint.Rows[i];
                string SnValue = (i + 1).ToString().PadRight(5); ;
                string ProductName = drPrint["ProductName"].ToString().PadRight(15);
                string ProductQuantity = string.Format("{0:c}", drPrint["Quantity"].ToString().PadRight(5));
                string ProductRate = string.Format("{0:c}", drPrint["SalesRate"].ToString().PadRight(5));
                string ProducValue = string.Format("{0:c}", drPrint["Total"].ToString().PadRight(10));
                string ProductLine = SnValue + ProductName + ProductQuantity + ProductRate + ProducValue;

                graphic.DrawString(ProductLine,font,new SolidBrush(Color.Black),startX,startY+offSet);

                offSet = offSet + (int)fontHeight + 5;
            }
            offSet = offSet + 15;

            graphic.DrawString("Total Amount".PadRight(30)+string.Format("{0:c}",TotalPrintAmount),font,new SolidBrush(Color.Black),startX,startY+offSet);
        }
    }
}
