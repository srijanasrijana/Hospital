using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Drawing.Printing;
using DateManager;

namespace POS.POSInventory
{
    public partial class frmcheckout : Form
    {
        Sales m_SalesInvoice = new Sales();
        //int[] arraylist = new int[];
        private double AmtDue;
        private double Tender;
        private double TotalTender;
        private string SmallAmount;
        public bool Paid;
        public double AmountPaid;
        public double AmountReturn;
        //For Saving Invoice
        List<int> AccClassIDCO = new List<int>();
        private int SeriesIDCO = 0;
        private int SalesIDCO = 0;
        private string SalesACCO;
        private int PartyIDCO = 0;
        private string PartyACCO;
        private int DepotIDCO = 0;
        private int OrderNoCO = 0;
        private string VoucherNumberCO;
        private DateTime POSSalesInvoiceDateCO;
        private string RemarksCO;
        private int ProjectIDCO;
        private string CreatedByCO;
        private DateTime CreatedDateCO;
        private string ModifiedByCO;
        private DateTime ModifiedDateCO;
        private int QuantityCO;
        private double GrossAmountCO;
        private double DiscountCO;
        private double NetAmountCO;
        private double TotalAmountCO;
        //for detail purpose
        private string ProductCodeCO;
        private int ProductIDCO;
        private double SalesRateCO;
        private double DiscountPercentageCO;
        //For Tax and VAT
        private double VATCO = 0;
        private double TAX1CO = 0;
        private double TAX2CO = 0;
        private double TAX3CO = 0;

        private double tax1amtCO = 0;
        private double tax2amtCO = 0;
        private double tax3amtCO = 0;
        private double vatamtCO = 0;

        private DataTable dtinsertdetailCO;
        //For Printing Purpose
        private DataTable dtPrint;
        private string PrintAmount;
        private string PrintTenderAmount;
        private string PrintChange;
        private string PrintDate;
        private string PrintVAT;
        private string PrintDiscount;
        private string PrintSubTotal;
        private string PrintUser;
        public frmcheckout()
        {
            InitializeComponent();
        }
        public frmcheckout(int SeriesID, int SalesID, string SalesAC, int PartyID, string PartyAC, int DepotID, int OrderNo, string VoucherNumber, DateTime POSSalesInvoiceDate, string Remarks, int ProjectID, string CreatedBy, DateTime CreatedDate, int Quantity, double GrossAmount, double Discount, double NetAmount, double tax1amt, double tax2amt, double tax3amt, double vatamt, double TotalAmount, DataTable dtproductdetail,string subtotal,string discount,string vat)
        {
            SeriesIDCO = SeriesID;
            SalesIDCO = SalesID;
            SalesACCO = SalesAC;
            PartyIDCO = PartyID;
            PartyACCO = PartyAC;
            DepotIDCO = DepotID;
            OrderNoCO = OrderNo;
            VoucherNumberCO = VoucherNumber;

            POSSalesInvoiceDateCO = POSSalesInvoiceDate;
            RemarksCO = Remarks;
            ProjectIDCO = ProjectID;
            CreatedByCO = CreatedBy;
            CreatedDateCO = CreatedDate;
            QuantityCO = Quantity;
            GrossAmountCO =GrossAmount;
            DiscountCO = Discount;
            NetAmountCO = NetAmount;
            tax1amtCO = tax1amt;
            tax2amtCO = tax2amt;
            tax3amtCO = tax3amt;
            vatamtCO = vatamt;
            TotalAmountCO = TotalAmount;
            dtinsertdetailCO = dtproductdetail;
            dtPrint = dtproductdetail;
            PrintSubTotal = Convert.ToDouble(subtotal).ToString("###0.00");
            PrintDiscount = Convert.ToDouble(discount).ToString("###0.00");
            PrintVAT =Convert.ToDouble(vat).ToString("###0.00");
            AmtDue = Convert.ToDouble(TotalAmount);
            InitializeComponent();
        }
        //public frmcheckout(string TotalBill)
        //{
        //    AmtDue =Convert.ToDouble( TotalBill);
        //    InitializeComponent();
        //}

        private void btn1_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 1;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 2;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 3;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 4;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 5;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 6;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 7;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 8;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 9;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + 0;
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            txtTender.Text = txtTender.Text + ".";
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            txtTender.Text = "0";
        }

        private void frmcheckout_Load(object sender, EventArgs e)
        {
            Paid = false;
            Tender = 0;
            SmallAmount = "0";
            SetTextAmount();
        }

        private void btnrs5_Click(object sender, EventArgs e)
        {
            Tender += 5;
            SetTextAmount();
        }
        private void SetTextAmount()
        {
            txtAmount.Text =Convert.ToString( AmtDue);
            double changeamt;
            TotalTender = Tender + Double.Parse(SmallAmount);
            txtTender.Text = TotalTender.ToString();
            changeamt = Double.Parse(txtTender.Text.ToString()) - Double.Parse(txtAmount.Text.ToString());
            txtChange.Text = changeamt.ToString();
            if (changeamt >= 0)
            {
                lblChange.Text = "Change";
                lblChange.Tag = "Change";
            }
            else
            {
                lblChange.Text = "Outstanding";
                lblChange.Tag = "Outstanding";
            }
        }

        private void btnrs10_Click(object sender, EventArgs e)
        {
            Tender += 10;
            SetTextAmount();
        }

        private void btnrs20_Click(object sender, EventArgs e)
        {
            Tender += 20;
            SetTextAmount();
        }

        private void btnrs50_Click(object sender, EventArgs e)
        {
            Tender += 50;
            SetTextAmount();
        }

        private void btnrs100_Click(object sender, EventArgs e)
        {
            Tender += 100;
            SetTextAmount();
        }

        private void btnrs500_Click(object sender, EventArgs e)
        {
            Tender += 500;
            SetTextAmount();
        }

        private void btnrs1000_Click(object sender, EventArgs e)
        {
            Tender += 1000;
            SetTextAmount();
        }

        private void btnExact_Click(object sender, EventArgs e)
        {
            Tender = AmtDue;
            SetTextAmount();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Paid = false;
            Tender = 0;
            AmountReturn = 0;
            Global.isFillGrid = true;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Global.isFillGrid = false;
            if(Convert.ToDouble(txtChange.Text)<0)
            {
                MessageBox.Show("Please First Pay The Bill");
                return;
            }
            int[] a = new int[] { 1 };
            m_SalesInvoice.InsertPOSSalesInvoice(SeriesIDCO, SalesIDCO, SalesACCO, PartyIDCO, PartyACCO, DepotIDCO, OrderNoCO, VoucherNumberCO, POSSalesInvoiceDateCO, RemarksCO, ProjectIDCO, CreatedByCO, CreatedDateCO, QuantityCO, GrossAmountCO, DiscountCO, NetAmountCO, tax1amtCO, tax2amtCO, tax3amtCO, vatamtCO, TotalAmountCO, dtinsertdetailCO, a.ToArray());
            MessageBox.Show("Sales Invoice Created Successfully");
            PrintAmount=Convert.ToDouble( txtAmount.Text).ToString("###0.00");
            PrintTenderAmount=Convert.ToDouble( txtTender.Text).ToString("###0.00");
            PrintChange=Convert.ToDouble( txtChange.Text).ToString("###0.00");
            PrintDate = Date.DBToSystem(Date.GetServerDate().ToString());
            PrintUser = User.CurrentUserName;

            if(chkprintwhilesave.Checked==true)
            {
            frmprintingtemplate frmtemplate = new frmprintingtemplate();
            DialogResult printoption=frmtemplate.ShowDialog();
            if(printoption==DialogResult.OK && Global.templateOption=="POS")
            {
                 PrintReceipt();
            }
            else if (printoption == DialogResult.OK && Global.templateOption == "DOTMATRIX")
            {
                MessageBox.Show("Print Sytle Dot Matrix");
            }
           Global.isPrintBill = true;
            }
           this.Close();
        }

        private void PrintReceipt()
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDialog.Document = printDocument;

            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            //DialogResult result = printDialog.ShowDialog();
            //if (result == DialogResult.OK)
            //{
                printDocument.Print();
            //}
        }
        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 8);
            string drawline="------------------------------------------";
            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offSetY = 0;
            int offSetX = 40;
            graphic.DrawString("BentRay Technology", new Font("Courier New", 10), new SolidBrush(Color.Black), startX+offSetX, startY);
            offSetY = offSetY + 20;
            offSetX = offSetX + 10;
            graphic.DrawString("Kathmandu,Nepal", new Font("Courier New", 10), new SolidBrush(Color.Black), startX+offSetX, startY + offSetY);
            offSetY = offSetY + 20;
            offSetX = offSetX + 13;
            graphic.DrawString("Tax Invoice", new Font("Courier New", 10), new SolidBrush(Color.Black), startX+offSetX, startY + offSetY);
            offSetY = offSetY + 20;
            graphic.DrawString("Date:"+" "+PrintDate, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 20;
            string sn = "Sn".PadRight(3);
            string ProductHeadParticular = "Particulars".PadRight(12);
            string ProductHeadQuantity = "Qty".PadRight(4);
            string ProductHeadRate = "Rate".PadRight(10);
            string ProductHeadAmount = "Amount".PadRight(10);

            string ProductHeadLine = sn + ProductHeadParticular + ProductHeadQuantity + ProductHeadRate + ProductHeadAmount;
            graphic.DrawString(ProductHeadLine, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 20;
            graphic.DrawString(drawline, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 20;
            for (int i = 0; i < dtPrint.Rows.Count; i++)
            {   
                DataRow drPrint = dtPrint.Rows[i];
                string SalesRateValue = Convert.ToDouble(drPrint["SalesRate"].ToString()).ToString("###0.00"); ;
                string TotalValue =Convert.ToDouble( drPrint["Total"].ToString()).ToString("###0.00");
                string SnValue = (i + 1).ToString().PadRight(3); ;
                string ProductName = drPrint["ProductName"].ToString().PadRight(12);
                string ProductQuantity = string.Format("{0:c}", drPrint["Quantity"].ToString().PadRight(4));
                string ProductRate = string.Format("{0:c}", SalesRateValue.PadRight(10));
                string ProducValue = string.Format("{0:c}", TotalValue.PadRight(10));
                
                string ProductLine = SnValue + ProductName + ProductQuantity + ProductRate + ProducValue;

                graphic.DrawString(ProductLine, font, new SolidBrush(Color.Black), startX, startY + offSetY);

                offSetY = offSetY + (int)fontHeight + 5;
            }
           // txtSub.Text = SubTotal.ToString("#,##0.00");
            offSetY = offSetY + 15;
            graphic.DrawString(drawline, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            //graphic.DrawString("Sub Total".PadRight(30) + string.Format("{0:c}", PrintSubTotal), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("Discount".PadRight(30) + string.Format("{0:c}", PrintDiscount), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("VAT".PadRight(30) + string.Format("{0:c}", PrintVAT), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("Total Amount".PadRight(30) + string.Format("{0:c}", PrintAmount), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString(drawline, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("Tender".PadRight(30) + string.Format("{0:c}", PrintTenderAmount), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("Change".PadRight(30) + string.Format("{0:c}", PrintChange), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString(drawline, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("Counter".PadRight(28) + string.Format("{0:c}", "TERMINAL A1"), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            //offSetY = offSetY + 15;

            //graphic.DrawString("Cashier".PadRight(28) + string.Format("{0:c}", PrintUser), font, new SolidBrush(Color.Black), startX, startY + offSetY);

            graphic.DrawString("Sub Total" + string.Format("{0:c}", PrintSubTotal.PadLeft(26)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("Discount" + string.Format("{0:c}", PrintDiscount.PadLeft(27)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("VAT" + string.Format("{0:c}", PrintVAT.PadLeft(32)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("Total Amount" + string.Format("{0:c}", PrintAmount.PadLeft(23)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString(drawline, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("Tender" + string.Format("{0:c}", PrintTenderAmount.PadLeft(29)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("Change" + string.Format("{0:c}", PrintChange.PadLeft(29)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString(drawline, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("Counter" + string.Format("{0:c}", "TERMINAL A1".PadLeft(28)), font, new SolidBrush(Color.Black), startX, startY + offSetY);
            offSetY = offSetY + 15;

            graphic.DrawString("Cashier" + string.Format("{0:c}", PrintUser.PadLeft(28)), font, new SolidBrush(Color.Black), startX, startY + offSetY);

            

        }

    }
}
