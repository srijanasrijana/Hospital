using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using BarcodeLib;


namespace AccSwift.Forms
{
    public partial class frmBarCode : Form
    {
        // Create linear barcode object
       
       Barcode barcode = new Barcode();
        public frmBarCode()
        {
            InitializeComponent();
        }

        private void frmbarcode_Load(object sender, EventArgs e)
        {

            
            this.cbBarcodeAlign.SelectedIndex = 0;
            this.cbLabelLocation.SelectedIndex = 0;
            cboBarCodeType.SelectedIndex = 18;
            chkAddPadding.Checked = false;
            chkAddPadding_CheckedChanged(sender, e);



            //Show the Ledgers of Product in form_Load condition
            DataTable dtProduct = Product.GetProductList(0);
            if (dtProduct.Rows.Count > 0)
            {
                for (int i = 1; i <= dtProduct.Rows.Count; i++)
                {
                    DataRow drProduct = dtProduct.Rows[i - 1];
                    cboProductSingle.Items.Add(new ListItem((int)drProduct["ProductID"], drProduct["EngName"].ToString()));
                }
                cboProductSingle.DisplayMember = "value";//This value is  for showing at Load condition
                cboProductSingle.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboProductSingle.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
            }


            this.cbRotateFlip.DataSource = System.Enum.GetNames(typeof(RotateFlipType));


            rdSpecificProduct_CheckedChanged(sender, e);

            
        }



        private bool GenerateBarCode()
        {
            barcode.Alignment = BarcodeLib.AlignmentPositions.CENTER;

            //barcode alignment
            switch (cbBarcodeAlign.SelectedItem.ToString().Trim().ToLower())
            {
                case "left": barcode.Alignment = BarcodeLib.AlignmentPositions.LEFT; break;
                case "right": barcode.Alignment = BarcodeLib.AlignmentPositions.RIGHT; break;
                default: barcode.Alignment = BarcodeLib.AlignmentPositions.CENTER; break;
            }//switch

            // Set barcode symbology type to Code-39
            int W = Convert.ToInt32(this.txtWidth.Text.Trim());
            int H = Convert.ToInt32(this.txtHeight.Text.Trim());


            BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;

            switch (cboBarCodeType.SelectedItem.ToString().Trim())
            {
                case "UPC-A": type = BarcodeLib.TYPE.UPCA; break;
                case "UPC-E": type = BarcodeLib.TYPE.UPCE; break;
                case "UPC 2 Digit Ext.": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
                case "UPC 5 Digit Ext.": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
                case "EAN-13": type = BarcodeLib.TYPE.EAN13; break;
                case "JAN-13": type = BarcodeLib.TYPE.JAN13; break;
                case "EAN-8": type = BarcodeLib.TYPE.EAN8; break;
                case "ITF-14": type = BarcodeLib.TYPE.ITF14; break;
                case "Codabar": type = BarcodeLib.TYPE.Codabar; break;
                case "PostNet": type = BarcodeLib.TYPE.PostNet; break;
                case "Bookland/ISBN": type = BarcodeLib.TYPE.BOOKLAND; break;
                case "Code 11": type = BarcodeLib.TYPE.CODE11; break;
                case "Code 39": type = BarcodeLib.TYPE.CODE39; break;
                case "Code 39 Extended": type = BarcodeLib.TYPE.CODE39Extended; break;
                case "Code 39 Mod 43": type = BarcodeLib.TYPE.CODE39_Mod43; break;
                case "Code 93": type = BarcodeLib.TYPE.CODE93; break;
                case "LOGMARS": type = BarcodeLib.TYPE.LOGMARS; break;
                case "MSI": type = BarcodeLib.TYPE.MSI_Mod10; break;
                case "Interleaved 2 of 5": type = BarcodeLib.TYPE.Interleaved2of5; break;
                case "Standard 2 of 5": type = BarcodeLib.TYPE.Standard2of5; break;
                case "Code 128": type = BarcodeLib.TYPE.CODE128; break;
                case "Code 128-A": type = BarcodeLib.TYPE.CODE128A; break;
                case "Code 128-B": type = BarcodeLib.TYPE.CODE128B; break;
                case "Code 128-C": type = BarcodeLib.TYPE.CODE128C; break;
                case "Telepen": type = BarcodeLib.TYPE.TELEPEN; break;
                case "FIM": type = BarcodeLib.TYPE.FIM; break;
                case "Pharmacode": type = BarcodeLib.TYPE.PHARMACODE; break;
                default: MessageBox.Show("Please specify the encoding type."); break;
            }//switch

            barcode.IncludeLabel = chkIncludeLabel.Checked;

            ListItem liProductID = new ListItem();
            liProductID = (ListItem)cboProductSingle.SelectedItem;
            int ProductID = liProductID.ID;
            string ProductName = liProductID.Value;

            string BarCodeData = "";
            if (rdSpecificProduct.Checked)
                BarCodeData = ProductID.ToString();
            else if (rdSpecificValue.Checked)
                BarCodeData = txtData.Text;








            try
            {

                if (type != BarcodeLib.TYPE.UNSPECIFIED)
                {
                    try
                    {
                        barcode.BarWidth = textBoxBarWidth.Text.Trim().Length < 1 ? null : (int?)Convert.ToInt32(textBoxBarWidth.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to parse BarWidth: " + ex.Message, ex);
                    }
                    try
                    {
                        barcode.AspectRatio = textBoxAspectRatio.Text.Trim().Length < 1 ? null : (double?)Convert.ToDouble(textBoxAspectRatio.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to parse AspectRatio: " + ex.Message, ex);
                    }




                    barcode.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), this.cbRotateFlip.SelectedItem.ToString(), true);

                    //label alignment and position
                    switch (this.cbLabelLocation.SelectedItem.ToString().Trim().ToUpper())
                    {
                        case "BOTTOMLEFT": barcode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMLEFT; break;
                        case "BOTTOMRIGHT": barcode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMRIGHT; break;
                        case "TOPCENTER": barcode.LabelPosition = BarcodeLib.LabelPositions.TOPCENTER; break;
                        case "TOPLEFT": barcode.LabelPosition = BarcodeLib.LabelPositions.TOPLEFT; break;
                        case "TOPRIGHT": barcode.LabelPosition = BarcodeLib.LabelPositions.TOPRIGHT; break;
                        default: barcode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER; break;
                    }//switch


                    if (rdAllProducts.Checked) //If All product selected
                    {
                        try
                        {
                            DataTable dtallproduct = Product.GetAllProduct();


                            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                            DialogResult result = folderBrowserDialog1.ShowDialog();
                            if (result == DialogResult.OK)
                            {

                                foreach (DataRow drproduct in dtallproduct.Rows)
                                {
                                    ProductName = drproduct["Name"].ToString();
                                    BarCodeData = drproduct["productid"].ToString();
                                    if(chkAddPadding.Checked)
                                    {
                                        BarCodeData = BarCodeData.PadLeft(Convert.ToInt32(txtNoOfDigits.Text), Convert.ToChar(txtPadChar.Text));
                                    }
                                    //===== Encoding performed here =====
                                    grpBarCode.BackgroundImage = barcode.Encode(type, BarCodeData.ToString(), Color.Black, Color.White, W, H);
                                    //reposition the barcode image to the middle
                                    grpBarCode.Location = new Point((this.grpBarCode.Location.X + this.grpBarCode.Width / 2) - grpBarCode.Width / 2, (grpBarCode.Location.Y + grpBarCode.Height / 2) - grpBarCode.Height / 2);
                                    //===================================



                                    barcode.SaveImage(folderBrowserDialog1.SelectedPath + "\\" + ProductName + ".jpg", BarcodeLib.SaveTypes.JPG);
                                }
                                MessageBox.Show("File Saved Successfully!");
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return false;
                        }
                    
                    }//All products
                    else //For specific product
                    {


                        if (chkAddPadding.Checked)
                        {
                            BarCodeData = BarCodeData.PadLeft(Convert.ToInt32(txtNoOfDigits.Text), Convert.ToChar(txtPadChar.Text));
                        }
                        //===== Encoding performed here =====
                        grpBarCode.BackgroundImage = barcode.Encode(type, BarCodeData.ToString(), Color.Black, Color.White, W, H);

                        //reposition the barcode image to the middle
                        grpBarCode.Location = new Point((this.grpBarCode.Location.X + this.grpBarCode.Width / 2) - grpBarCode.Width / 2, (grpBarCode.Location.Y + grpBarCode.Height / 2) - grpBarCode.Height / 2);
                        
                        //===================================


                        return true;
                    }
                } //type != BarcodeLib.TYPE.UNSPECIFIED

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }


        }
        private void btnprint_Click(object sender, EventArgs e)
        {
            //if (this.printDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    this.printDocument1.Print();
            //}
            if (!GenerateBarCode())
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|TIFF (*.tif)|*.tif";
            sfd.FilterIndex = 2;
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.UNSPECIFIED;
                switch (sfd.FilterIndex)
                {
                    case 1: /* BMP */  savetype = BarcodeLib.SaveTypes.BMP; break;
                    case 2: /* GIF */  savetype = BarcodeLib.SaveTypes.GIF; break;
                    case 3: /* JPG */  savetype = BarcodeLib.SaveTypes.JPG; break;
                    case 4: /* PNG */  savetype = BarcodeLib.SaveTypes.PNG; break;
                    case 5: /* TIFF */ savetype = BarcodeLib.SaveTypes.TIFF; break;
                    default: break;
                }//switch
                barcode.SaveImage(sfd.FileName, savetype);
                MessageBox.Show("Barcode Saved Successfully!");
            }//if


          
           
        }



        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void rdSpecificValue_CheckedChanged(object sender, EventArgs e)
        {
            cboProductSingle.Enabled = false;
            txtData.Enabled = true;
        }

        private void rdSpecificProduct_CheckedChanged(object sender, EventArgs e)
        {
            cboProductSingle.Enabled = true;
            txtData.Enabled = false;
        }

        private void rdAllProducts_CheckedChanged(object sender, EventArgs e)
        {
            cboProductSingle.Enabled = false;
            txtData.Enabled = false;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if(rdAllProducts.Checked)
            {
                Global.Msg("Cannot show preview in All Product mode");
                return;
            }
            GenerateBarCode();
        }

        private void chkAddPadding_CheckedChanged(object sender, EventArgs e)
        {
            txtPadChar.Enabled = chkAddPadding.Checked;
            txtNoOfDigits.Enabled = chkAddPadding.Checked;
        }

       



     
    }
}
