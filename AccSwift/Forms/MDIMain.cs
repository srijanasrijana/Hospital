using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Configuration;
using BusinessLogic;
using Cryptography;
using RegistryManager;
using DateManager;
using System.Threading;
using System.Globalization;
using DBLogic;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Mail;
using Accounts;
using IniParser;
using POS;
using Inventory;
using Common;
using AccSwift;
using AccSwift.Forms;
using Accounts.View.Reports;
using Inventory.View;
using Inventory.View.Reports;
using Hospital;
using Hospital.View;

using HRM;
using HRM.View.Reports;
using Hospital.View.Report;



namespace AccSwift
{
    public partial class MDIMain : Form, IMDIClientNotify, IMDIMainForm
    {

        public static string iniPath = "";
        private MDIClientWindow mdiClient = null;
        private frmSplashScreen SplashScreen;
        private bool SplashComplete = false;

        private frmDamageItems fdmg;
        private frmPurchaseInvoice fpinvoice;
        private frmSalesInvoice fsalesinvoice;

        DataTable dtSelectedReminder = new DataTable();
        #region Private Structures
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public override string ToString()
            {
                string ret = String.Format(
                    "left = {0}, top = {1}, right = {2}, bottom = {3}",
                    left, top, right, bottom);
                return ret;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;

            public override string ToString()
            {
                string ret = String.Format(
                    "hdc = {0} , fErase = {1}, rcPaint = {2}, fRestore = {3}, fIncUpdate = {4}",
                    hdc, fErase, rcPaint.ToString(), fRestore, fIncUpdate);
                return ret;
            }
        }

        #endregion

        #region UnManagedMethods
        private class UnManagedMethods
        {
            [DllImport("user32")]
            public extern static int GetClientRect(
                IntPtr hwnd,
                ref RECT lpRect);
            [DllImport("user32")]
            public extern static int BeginPaint(
                IntPtr hwnd,
                ref PAINTSTRUCT lpPaint);
            [DllImport("user32")]
            public extern static int EndPaint(
                IntPtr hwnd,
                ref PAINTSTRUCT lpPaint);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static uint SetClassLong(
                IntPtr hwnd,
                int nIndex,
                uint dwNewLong);
            [DllImport("user32")]
            public extern static int InvalidateRect(
                IntPtr hwnd,
                ref RECT lpRect,
                int bErase);

            public const int WM_PAINT = 0xF;
            public const int WM_ERASEBKGND = 0x14;
            public const int WM_SIZE = 0x5;
            public const int GCL_HBRBACKGROUND = (-10);

        }
        #endregion

        #region MDI Background Painting
        public void WndProc(ref Message m, ref bool doDefault)
        {
            // Don't need to do anything if the form is minimized:
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (m.Msg == UnManagedMethods.WM_PAINT)
                {
                    //
                    // Here we draw a logo on the "right" hand side 

                    // of the form (depends on RTL)
                    //
                    PAINTSTRUCT ps = new PAINTSTRUCT();
                    UnManagedMethods.BeginPaint(m.HWnd, ref ps);
                    RECT rc = new RECT();
                    UnManagedMethods.GetClientRect(m.HWnd, ref rc);

                    // Convert to managed code world				
                    Graphics gfx = Graphics.FromHdc(ps.hdc);
                    RectangleF rcClient = new RectangleF(
                        rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top);
                    Rectangle rcPaint = new Rectangle(
                        ps.rcPaint.left,
                        ps.rcPaint.top,
                        ps.rcPaint.right - ps.rcPaint.left,
                        ps.rcPaint.bottom - ps.rcPaint.top);

                    #region Draw Images
                    // gfx.FillRectangle(new SolidBrush(Color.Red), rcClient);

                    gfx.DrawImage(picBckgnd.Image, rcClient);
                    Rectangle rcLogo = new Rectangle(10, rc.bottom - 200, picLogo.Width, picLogo.Height);
                    //Rectangle rcShadow = rcLogo;
                    //rcShadow.Offset(4, 4);
                    //gfx.FillRectangle(new SolidBrush(Color.Gray), rcShadow);
                    gfx.DrawImage(picLogo.Image, rcLogo);
                    // gfx.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rcLogo);

                    #endregion

                    // Draw the logo bottom right:
                    SolidBrush brText = new SolidBrush(Color.FromArgb(100, 100, 100));
                    StringFormat strFormat = new StringFormat();

                    //strFormat.Alignment = StringAlignment.Center;
                    // strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft |
                    //     StringFormatFlags.NoWrap;
                    // strFormat.LineAlignment = StringAlignment.Center;
                    Font logoFont = new Font("Arial Black", 15, FontStyle.Regular);
                    gfx.DrawString(lblAccountingandInventory.Text, lblAccountingandInventory.Font, brText, new Rectangle(20, rc.bottom - 150, 500, 100));
                    gfx.DrawString(lblMarketedBy.Text, lblMarketedBy.Font, brText, new Rectangle(20, rc.bottom - 120, 500, 100));
                    gfx.DrawString(lblHeading.Text, lblHeading.Font, brText, new Rectangle(20, rc.bottom - 100, 500, 100));
                    gfx.DrawString(lblAddress.Text, lblAddress.Font, brText, new Rectangle(20, rc.bottom - 80, 500, 100), strFormat);
                    gfx.DrawString(lblPhone.Text, lblPhone.Font, brText, new Rectangle(20, rc.bottom - 60, 500, 100), strFormat);
                    gfx.DrawString(lblEmail.Text, lblEmail.Font, brText, new Rectangle(20, rc.bottom - 40, 500, 100), strFormat);

                    //logoFont.Dispose();
                    strFormat.Dispose();
                    brText.Dispose();

                    gfx.Dispose();
                    UnManagedMethods.EndPaint(m.HWnd, ref ps);
                }
                else if (m.Msg == UnManagedMethods.WM_ERASEBKGND)
                {
                    //
                    // Fill the background:
                    //

                    RECT rc = new RECT();
                    UnManagedMethods.GetClientRect(m.HWnd, ref rc);

                    // Convert to managed code world
                    Graphics gfx = Graphics.FromHdc(m.WParam);
                    Rectangle rcClient = new Rectangle(
                        rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top);

                    //int angle = 45;
                    //LinearGradientBrush linGrBrush = new LinearGradientBrush(
                    //    rcClient,
                    //   Color.FromArgb(255, 230, 242, 255),// pale blue
                    //    Color.FromArgb(255, 0, 72, 160),   // deep blue
                    //    angle);

                    //linGrBrush.Dispose();
                    gfx.Dispose();

                    // Tell Windows we've filled the background:
                    m.Result = (IntPtr)1;

                    // Don't call the default procedure:
                    doDefault = false;

                }
                else if (m.Msg == UnManagedMethods.WM_SIZE)
                {
                    // If your background is a tiled image then
                    // you don't need to do this.  This is only required
                    // when the entire background needs to be updated 
                    // in response to the size of the object changing.
                    RECT rect = new RECT();
                    rect.left = 0;
                    rect.top = 0;
                    rect.right = ((int)m.LParam) & 0xFFFF;
                    rect.bottom = (int)(((uint)(m.LParam) & 0xFFFF0000) >> 16);
                    //Console.WriteLine("WM_SIZE {0}", rect.ToString());
                    UnManagedMethods.InvalidateRect(m.HWnd, ref rect, 1);
                }
            }
        }
        #endregion

        private int childFormNumber = 0;

        private void ShowSplashScreen()
        {
            SplashScreen.Show();
            while (!SplashComplete)
            {
                Application.DoEvents();
            }
            SplashScreen.Close();
            this.SplashScreen.Dispose();
        }


        void Common.IMDIMainForm.OpenFormArrayParam(string formname, object[] param = null)
        {
            try
            {
                switch (formname)
                {
                    case "frmDateConverter":
                        frmDateConverter frm = new frmDateConverter();
                        frm.Show();

                        break;
                    case "frmAccClass":
                        frmAccountClass frmAccClass = new frmAccountClass();
                        frmAccClass.Show();
                        break;
                    case "frmReportViewer":
                        frmReportViewer frmreportviewer = new frmReportViewer();
                        // frmreportviewer.SetReportSource((CrystalDecisions.CrystalReports.Engine.ReportClass)param);
                        frmreportviewer.Show();
                        break;
                    case "frmVoucherFormat":
                        frmVoucherFormat frmrVoucherFormat = new frmVoucherFormat();
                        frmrVoucherFormat.Show();
                        break;
                    case "frmAccSetUp":
                        Accounts.frmAccountSetup frmAccSetUp = new Accounts.frmAccountSetup();
                        frmAccSetUp.ShowDialog();
                        break;
                    case "frmSelectAccClass":
                        frmSelectAccClass frmSelectAccClass = new frmSelectAccClass(this);
                        frmSelectAccClass.Show();
                        break;
                    case "frmVoucherConfiguration":
                        frmVoucherConfiguration frmVoucherConfiguration = new frmVoucherConfiguration();
                        frmVoucherConfiguration.Show();
                        break;
                    case "frmSalesInvoice":
                        int RowID = (int)param[0];
                        frmSalesInvoice frmSalesInvoice = new frmSalesInvoice(RowID);
                        frmSalesInvoice.Show();
                        break;
                    case "frmPurchaseInvoice":
                        int RowID1 = (int)param[0];
                        frmPurchaseInvoice frmPurchaseInvoice = new frmPurchaseInvoice(RowID1);
                        frmPurchaseInvoice.Show();
                        break;
                    case "frmSalesReturn":
                        int RowID2 = (int)param[0];
                        frmSalesReturn frmSalesReturn = new frmSalesReturn(RowID2);
                        frmSalesReturn.Show();
                        break;
                    case "frmPurchaseReturn":
                        int RowID3 = (int)param[0];
                        frmPurchaseReturn frmPurchaseReturn = new frmPurchaseReturn(RowID3);
                        frmPurchaseReturn.Show();
                        break;

                    #region for recurring
                    case "frmSalesInvoiceRecurring":
                        int RowID4 = (int)param[0];
                        int RVPID4 = (int)param[1];
                        frmSalesInvoice frmSalesRecurring = new frmSalesInvoice(RowID4, true, RVPID4);
                        frmSalesRecurring.ShowDialog();
                        break;
                    case "frmSalesReturnRecurring":
                        int RowID5 = (int)param[0];
                        int RVPID5 = (int)param[1];
                        frmSalesReturn frmSalesRecurringR = new frmSalesReturn(RowID5, true, RVPID5);
                        frmSalesRecurringR.ShowDialog();
                        break;
                    case "frmSalesOrderRecurring":
                        int RowID6 = (int)param[0];
                        int RVPID6 = (int)param[1];
                        frmSalesOrder frmSalesOrderRecurring = new frmSalesOrder(RowID6, true, RVPID6);
                        frmSalesOrderRecurring.ShowDialog();
                        break;
                    case "frmPurchaseInvoiceRecurring":
                        int RowID7 = (int)param[0];
                        int RVPID7 = (int)param[1];
                        frmPurchaseInvoice frmPurchaseRecurring = new frmPurchaseInvoice(RowID7, true, RVPID7);
                        frmPurchaseRecurring.ShowDialog();
                        break;
                    case "frmPurchaseReturnRecurring":
                        int RowID8 = (int)param[0];
                        int RVPID8 = (int)param[1];
                        frmPurchaseReturn frmPurchaseRecurringR = new frmPurchaseReturn(RowID8, true, RVPID8);
                        frmPurchaseRecurringR.ShowDialog();
                        break;
                    case "frmPurchaseOrderRecurring":
                        int RowID9 = (int)param[0];
                        int RVPID9 = (int)param[1];
                        frmPurchaseOrder frmPurchaseOrderRecurring = new frmPurchaseOrder(RowID9, true, RVPID9);
                        frmPurchaseOrderRecurring.ShowDialog();
                        break;
                    case "frmJournalRecurring":
                        int RowID10 = (int)param[0];
                        int RVPID10 = (int)param[1];
                        frmJournal frmJournalRecurring = new frmJournal(RowID10, true, RVPID10);
                        frmJournalRecurring.ShowDialog();
                        break;
                    case "frmBankReceiptRecurring":
                        int RowID11 = (int)param[0];
                        int RVPID11 = (int)param[1];
                        frmBankReceipt frmaBankRecurringR = new frmBankReceipt(RowID11, true, RVPID11);
                        frmaBankRecurringR.ShowDialog();
                        break;
                    case "frmBankPaymentRecurring":
                        int RowID12 = (int)param[0];
                        int RVPID12 = (int)param[1];
                        frmBankPayment frmaBankRecurringP = new frmBankPayment(RowID12, true, RVPID12);
                        frmaBankRecurringP.ShowDialog();
                        break;
                    case "frmCashReceiptRecurring":
                        int RowID13 = (int)param[0];
                        int RVPID13 = (int)param[1];
                        frmCashReceipt frmaCashRecurringR = new frmCashReceipt(RowID13, true, RVPID13);
                        frmaCashRecurringR.ShowDialog();
                        break;
                    case "frmCashPaymentRecurring":
                        int RowID14 = (int)param[0];
                        int RVPID14 = (int)param[1];
                        frmCashPayment frmaCashRecurringP = new frmCashPayment(RowID14, true, RVPID14);
                        frmaCashRecurringP.ShowDialog();
                        break;
                    case "frmContraRecurring":
                        int RowID15 = (int)param[0];
                        int RVPID15 = (int)param[1];
                        frmContra frmContraRecurring = new frmContra(RowID15, true, RVPID15);
                        frmContraRecurring.ShowDialog();
                        break;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
    }

        //Below code is not being used as the parameter param is converted to Array parameter in above method.
        //void Common.IMDIMainForm.OpenForm(string formname, object param = null)
        //{

        //    switch (formname)
        //    {
        //        case "frmDateConverter":
        //            frmDateConverter frm = new frmDateConverter();
        //            frm.Show();

        //            break;
        //        case "frmAccClass":
        //            frmAccountClass frmAccClass = new frmAccountClass();
        //            frmAccClass.Show();
        //            break;

        //        case "frmReportViewer":
        //            frmReportViewer frmreportviewer = new frmReportViewer();
        //            // frmreportviewer.SetReportSource((CrystalDecisions.CrystalReports.Engine.ReportClass)param);
        //            frmreportviewer.Show();
        //            break;
        //        case "frmVoucherFormat":
        //            frmVoucherFormat frmrVoucherFormat = new frmVoucherFormat();
        //            frmrVoucherFormat.Show();
        //            break;
        //        case "frmAccSetUp":
        //            Accounts.frmAccountSetup frmAccSetUp = new Accounts.frmAccountSetup();
        //            frmAccSetUp.Show();
        //            break;
        //        case "frmSelectAccClass":
        //            frmSelectAccClass frmSelectAccClass = new frmSelectAccClass(this);
        //            frmSelectAccClass.Show();
        //            break;
        //        case "frmVoucherConfiguration":
        //            frmVoucherConfiguration frmVoucherConfiguration = new frmVoucherConfiguration();
        //            frmVoucherConfiguration.Show();
        //            break;
        //        case "frmSalesInvoice":
        //            int RowID = (int)param;
        //            frmSalesInvoice frmSalesInvoice = new frmSalesInvoice(RowID);
        //            frmSalesInvoice.Show();
        //            break;
        //        case "frmPurchaseInvoice":
        //            int RowID1 = (int)param;
        //            frmPurchaseInvoice frmPurchaseInvoice = new frmPurchaseInvoice(RowID1);
        //            frmPurchaseInvoice.Show();
        //            break;
        //        case "frmSalesReturn":
        //            int RowID2 = (int)param;
        //            frmSalesReturn frmSalesReturn = new frmSalesReturn(RowID2);
        //            frmSalesReturn.Show();
        //            break;
        //        case "frmPurchaseReturn":
        //            int RowID3 = (int)param;
        //            frmPurchaseReturn frmPurchaseReturn = new frmPurchaseReturn(RowID3);
        //            frmPurchaseReturn.Show();
        //            break;
        //        //case "":
        //        //    break;
        //        //default:
        //        //  Global.MsgError("Form not found to open");
        //    }
        //}




        public MDIMain()
        {
#if !DEBUG
            bool done = false;
            frmSplashScreen SplashForm = new frmSplashScreen();
            SplashForm.TopMost = true;
            SplashForm.StartPosition = FormStartPosition.CenterScreen;
            ThreadPool.QueueUserWorkItem((x) =>
            {
                SplashForm.Show();
                while (!done)
                    Application.DoEvents();
                SplashForm.Close();
            });

#endif

            InitializeComponent();
            //Read Registry
            //Apply those on Databasesettings
            //Check if connection succeeds

            //if setting is read from or write to registry then 
            // use this code
            //********************************//

#if !DEBUG
            SplashForm.UpdateProgress(10);  
            

            #region Using Regedit

            //1.Try to Open using existing connection

            //In case of SQL Server connection
            SqlDb m_db = new SqlDb();
            //In case of SQL Server connection
            m_db.ServerName = RegManager.ServerName;
            //m_db.DbName = RegManager.DataBase;
            m_db.UserName = RegManager.DBUser;
            m_db.Password = RegManager.DBPassword;



            int LoopCounter = 0;
            while ((!m_db.Connect()) || (m_db.ServerName == ""))
            {
                if (LoopCounter > 0) //This is not the first time showing the server detail form due to the loop
                {
                    MessageBox.Show("Invalid server details. Please enter the correct server details or contact your system administrator.", "Error connecting to server", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }


                //1.2 Failure 
                //1.2.1 Show Database connection dialogue box
                frmDBConnect frmDB = new frmDBConnect();
                if (frmDB.ShowDialog() == DialogResult.OK)
                {
                    m_db.ServerName = frmDB.cbServer.Text;
                    m_db.DbName = frmDB.cbDataBase.Text;
                    m_db.UserName = frmDB.txtUser.Text;
                    m_db.Password = frmDB.txtPassword.Text;

                    //Now write it to the registry also
                    RegManager.ServerName = m_db.ServerName;
                    RegManager.DataBase = m_db.DbName;
                    RegManager.DBUser = m_db.UserName;
                    RegManager.DBPassword = m_db.Password;
                }
                else //Cancel button has been pressed
                {
                    System.Environment.Exit(0);//This exits all the system even if there is another main form
                    return;
                }
                LoopCounter++;
            }
            
            ////In case of SQL Server connection
            //SqlDb m_db = new SqlDb();
            ////In case of SQL Server connection
            //m_db.ServerName = RegManager.ServerName;
            //m_db.DbName = RegManager.DataBase;          
            //m_db.UserName = RegManager.DBUser;
            //m_db.Password = RegManager.DBPassword;

           // Global.ConnectionString = "Data Source=" + m_SqlDb.ServerName + ";Initial Catalog=" + m_SqlDb.DbName + "; uid = " + m_SqlDb.UserName + "; password = " + m_SqlDb.Password + "; Integrated Security=false; ";
                
            #endregion

#else
            try
            {
                //Read from INI File
                //Create an instance of a ini file parser
                IniParser.FileIniDataParser parser = new FileIniDataParser();
                //Load the INI file which also parses the INI data
                IniData parsedData = parser.LoadFile("../../../database.ini");

                SqlDb m_db = new SqlDb();
                //In case of SQL Server connection

                m_db.ServerName = parsedData["DatabaseConfiguration"]["ServerName"];
                m_db.DbName = parsedData["DatabaseConfiguration"]["DbName"];
                m_db.UserName = parsedData["DatabaseConfiguration"]["UserName"];
                m_db.Password = parsedData["DatabaseConfiguration"]["Password"];
                if (m_db.Connect())
                {
                    RegManager.ServerName = m_db.ServerName;
                    RegManager.DataBase = m_db.DbName;
                    RegManager.DBUser = m_db.UserName;
                    RegManager.DBPassword = m_db.Password;

                    Global.m_db.cn = m_db.cn;
                }


            }
            catch (Exception ex)
            {
                Global.Msg("Seems like database connection is wrong or database.ini is not present. Message:" + ex.Message);
            }
#endif


            //********************************//

            //above code connects only to master database
            //Now we have to create company or allow to select company.
            //for this display select company form if there exists a company
            //or display create company form.
            //If no Database is specified and the connection was successful, show the DB Creation dialogue box

#if !DEBUG
            SplashForm.UpdateProgress(80);

#endif
            //Attempt connecting the database            

            //Splash screen complete
#if !DEBUG            
            SplashForm.UpdateProgress(100);
            Thread.Sleep(1000); //Make it 1 second late than usual to show splash screen for little more time
            done = true;
            this.WindowState = FormWindowState.Maximized;
            Show();
#endif

        }

        #region Method to check user's permission
        private bool UserPermissionNotGranted(string accessCode, string permissionTo)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission(accessCode);
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to " + permissionTo + ". Please contact your administrator for permission.");
                return true;
            }
            else
                return false;
        }
        #endregion
        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        //Method to set Current Date to Global when DEFAULT_DATE = Nepali
        private void SetCurrDateNep()
        {
            Global.CurrentDateNep = Date.ToSystem(Date.GetServerDate());

            string[] NepaliDate = Global.CurrentDateNep.Split('/');
            Global.CurrentDateEng = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));
        }

        //Method to set Current Date to Global when DEFAULT_DATE = English
        private void SetCurrDateEng()
        {
            Global.CurrentDateEng = Convert.ToDateTime(Date.ToSystem(Date.GetServerDate()));

            string[] EnglishDate = Date.ToSystem(Date.GetServerDate()).Split('/');
            DateTime dtEnglish = new DateTime(Convert.ToInt16(EnglishDate[0]), Convert.ToInt16(EnglishDate[1]), Convert.ToInt16(EnglishDate[2]));
            int retYear = 0;
            int retMonth = 0;
            int retDay = 0;

            Date.EngToNep(dtEnglish, ref retYear, ref retMonth, ref retDay);
            Global.CurrentDateNep = retYear.ToString().PadLeft(4, '0') + "/" + retMonth.ToString().PadLeft(2, '0') + "/" + retDay.ToString().PadLeft(2, '0');
        }
        //Method to load the accessRoleId and all other settings according to userid
        private void LoadAccessRoleIdAndSettings(int uid)
        {
            try
            {
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow dr = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(dr["AccessRoleID"].ToString());
                Global.GlobalAccessRoleID = Convert.ToInt32(dr["AccessRoleID"].ToString());
                Global.GlobalAccClassID = Convert.ToInt32(dr["AccClassID"].ToString());
                Global.ParentAccClassID = GetRootAccClassID();

                #region Load Settings

                if (roleid == 37)
                {
                    try
                    {
                        switch (Settings.GetSettings("DEFAULT_DATE"))
                        {
                            case "Nepali":
                                //Date.DefaultDate = Date.DateType.Nepali;  //This line of code must be removed if any error appears
                                Global.Default_Date = Date.DateType.Nepali;
                                //Assigning current english and nepali date to Global
                                SetCurrDateNep();
                                break;
                            case "English":
                                //Date.DefaultDate = Date.DateType.English;  //This line of code must be removed if any error appears
                                Global.Default_Date = Date.DateType.English;

                                //Assigning current english and nepali date to Global
                                SetCurrDateEng();
                                break;
                        }
                        switch (Settings.GetSettings("DATE_FORMAT"))
                        {
                            case "YYYY/MM/DD":
                                Date.DefaultFormat = Date.DateFormat.YYYY_MM_DD;
                                break;
                            case "DD/MM/YYYY":
                                Date.DefaultFormat = Date.DateFormat.DD_MM_YYYY;
                                break;
                            case "MM/DD/YYYY":
                                Date.DefaultFormat = Date.DateFormat.MM_DD_YYYY;
                                break;
                        }
                        switch (Settings.GetSettings("DEFAULT_DECIMALPLACES"))
                        {
                            case "2":
                                Global.DecimalPlaces = 2;
                                break;
                            case "3":
                                Global.DecimalPlaces = 3;
                                break;
                            case "4":
                                Global.DecimalPlaces = 4;
                                break;
                        }
                        switch (Settings.GetSettings("COMMA_SEPARATED"))
                        {
                            case "1":
                                Global.Comma_Separated = true;
                                break;
                            case "0":
                                Global.Comma_Separated = false;
                                break;
                        }

                        Global.mailserver = Settings.GetSettings("MAIL_SERVER");
                        Global.serverport = Settings.GetSettings("SERVER_PORT");
                        Global.useremail = Settings.GetSettings("USER_EMAIL");
                        Global.password = Settings.GetSettings("PASSWORD");
                        Global.Fiscal_Year_Start = CompanyInfo.GetInfo().FYFrom;
                    }
                    catch
                    {
                        //Date.DefaultDate = Date.DateType.Nepali;  //This line of code must be removed if any error appears
                        Global.Default_Date = Date.DateType.Nepali;
                        //Assigning current english and nepali date to Global
                        SetCurrDateNep();
                        Date.DefaultFormat = Date.DateFormat.YYYY_MM_DD;
                        Global.DecimalPlaces = 0;
                        Global.Comma_Separated = true;
                    }
                }
                else
                {
                    try
                    {
                        //string SettingValue = UserPreference.GetValue("DEFAULT_DECIMALPLACES", uid);
                        switch (UserPreference.GetValue("DEFAULT_DATE", uid))
                        {
                            case "Nepali":
                                //Date.DefaultDate = Date.DateType.Nepali;   //This line of code must be removed if any error appears
                                Global.Default_Date = Date.DateType.Nepali;

                                //Assigning current english and nepali date to Global
                                SetCurrDateNep();
                                break;
                            case "English":
                                // Date.DefaultDate = Date.DateType.English;  //This line of code must be removed if any error appears
                                Global.Default_Date = Date.DateType.English;

                                //Assigning current english and nepali date to Global
                                SetCurrDateEng();
                                break;

                        }
                        switch (UserPreference.GetValue("DATE_FORMAT", uid))
                        {
                            case "YYYY/MM/DD":
                                Date.DefaultFormat = Date.DateFormat.YYYY_MM_DD;
                                break;
                            case "DD/MM/YYYY":
                                Date.DefaultFormat = Date.DateFormat.DD_MM_YYYY;
                                break;
                            case "MM/DD/YYYY":
                                Date.DefaultFormat = Date.DateFormat.MM_DD_YYYY;
                                break;
                        }
                        //switch (UserPreference.GetValue("DEFAULT_DECIMALPLACES", uid))
                        //{
                        //    case "2":
                        //        Global.DecimalPlaces = 2;
                        //        break;
                        //    case "3":
                        //        Global.DecimalPlaces = 3;
                        //        break;
                        //    case "4":
                        //        Global.DecimalPlaces = 4;
                        //        break;
                        //}
                        switch (Settings.GetSettings("DEFAULT_DECIMALPLACES"))
                        {
                            case "2":
                                Global.DecimalPlaces = 2;
                                break;
                            case "3":
                                Global.DecimalPlaces = 3;
                                break;
                            case "4":
                                Global.DecimalPlaces = 4;
                                break;
                        }
                        switch (UserPreference.GetValue("COMMA_SEPARATED", uid))
                        {
                            case "1":
                                Global.Comma_Separated = true;
                                break;
                            case "0":
                                Global.Comma_Separated = false;
                                break;
                        }

                                try
                                {
                                    Global.mailserver = UserPreference.GetValue("MAIL_SERVER", uid);
                                    Global.serverport = UserPreference.GetValue("SERVER_PORT", uid);
                                    Global.useremail = UserPreference.GetValue("USER_EMAIL", uid);
                                    Global.password = UserPreference.GetValue("PASSWORD", uid);
                                }
                                catch
                                {
                                    // do nothing
                                }
                    }
                    catch
                    {
                        //Date.DefaultDate = Date.DateType.Nepali;  //This line of code must be removed if any error appears
                        Global.Default_Date = Date.DateType.Nepali;

                        //Assigning current english and nepali date to Global
                        SetCurrDateNep();
                        Date.DefaultFormat = Date.DateFormat.YYYY_MM_DD;
                        Global.DecimalPlaces = 0;
                        Global.Comma_Separated = true;
                    }
                }
                DataTable dtSlabInfo = new DataTable();
                dtSlabInfo = Slabs.GetSlabInfo(SlabType.CUSTOMDUTY);
                DataRow drslabinfo = dtSlabInfo.Rows[0];
                Global.Default_CustomDuty = Convert.ToDouble(drslabinfo["Rate"].ToString());

                Global.Default_Sales_Report_Type = Settings.GetSettings("SALES_REPORT_TYPE");
                #endregion
            }
            catch (Exception ex)
            {
                Global.MsgError("Error Occurred. Reason: " + ex.Message);
            }
        }
        private void MDIMain_Load(object sender, EventArgs e)
        {
            hrmMenu.Visible = true;
            mnuSchool.Visible = false;
            mnuAttendance.Visible = false;

            //// Check if the system is activated or not
            Crypto.CheckActivation();


            #region CUSTOM MDI CLIENTAREA DRAW
            // Start processing for MDIClient window messages:
            mdiClient = new MDIClientWindow(this, this.Handle);
            // Stop the default window proc from drawing the MDI background
            // with the brush:
            UnManagedMethods.SetClassLong(
            mdiClient.Handle,
            UnManagedMethods.GCL_HBRBACKGROUND,
            0);
            #endregion

            //Gives focus to the form 
            this.Activate();

            //Set the exact project name and version to the title
            //this.Text = Application.ProductName + " Version " + Application.ProductVersion;
            this.Text = "AccSwift" + " Version " + "2.0.1";

            //For MAC Address Of The Computer
            Global.MacAddess = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        Global.MacAddess = nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            // MessageBox.Show(Global.MacAddess);

            Global.ComputerName = Environment.MachineName;
            //MessageBox.Show(Global.ComputerName);

            Global.IpAddress = Dns.GetHostAddresses(Environment.MachineName)[0].ToString();
            // MessageBox.Show(Global.IpAddress);

            Global.ConcatAllCompInfo = "MAC" + " " + Global.MacAddess + " " + "Computer Name" + " " + Global.ComputerName + " " + "IP Address" + " " + Global.IpAddress;
            //MessageBox.Show(Global.ConcatAllCompInfo);

            //1.1.1.2 If there are no companies, show company creation dialogue box
            //1.2.1.1 Login Success
            //1.2.1.1.1 Go to 1.1.1
            //1.2.1.2 Login Failure
            //1.2.1.2.1 Go to 1.2.1

            //User.CurrUserID = 1;
            //User.CurrentUserName = "root";


#if !DEBUG
            //Open the company
            openToolStripMenuItem_Click(sender, e);
           

#endif
          
#if DEBUG
            int uid = 1;

            LoadAccessRoleIdAndSettings(uid);
#endif

            #region visibility setting enable and disable
            //////Enable/Disable menu items at form load
            //panel1.Enabled = false;
            //foreach (ToolStripMenuItem item in menuStrip.Items)
            //{
            //    if (item.Name == "fileMenu")
            //    {
            //        item.Enabled = true;
            //        foreach (ToolStripItem subitem in item.DropDownItems)
            //        {
            //            if (subitem.Name == "maintainuserToolStripMenuItem" || subitem.Name == "accessRoleToolStripMenuItem")
            //            {
            //                subitem.Enabled = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        item.Enabled = false;
            //    }
            //};

            ////enable all required menus after selecting company or creating company                 
            #endregion

        }


        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ACCOUNT", "Account Setup"))
                return;

            frmAccountSetup acSetup = new frmAccountSetup();
            //acSetup.MdiParent = this;
            acSetup.ShowDialog();
        }

        private void logInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the current user to nobody
            User.CurrUserID = 0;

            //Disable all Menu
            Misc.EnableControls(this.Controls, false);
            menuStrip.Enabled = true;

            mnuSystem.Enabled = true;
            //Now disable some menus inside system
            maintainuserToolStripMenuItem.Enabled = false;
            accessRoleToolStripMenuItem.Enabled = false;

            newToolStripMenuItem.Enabled = false;

            //1.1.1.1 If there are existing companies and user selects a company
            //1.1.1.1.1 Show Login dialog box
            mnuLogin.Enabled = true;
            mnuLogOut.Enabled = false;
            frmLogin m_frmLogin = new frmLogin();
            if (m_frmLogin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                LoadAccessRoleIdAndSettings(User.CurrUserID);
                //Enable all controls
                Misc.EnableControls(this.Controls, true);
                mnuLogin.Enabled = false;
                mnuLogOut.Enabled = true;
                //Method to display status
                LoadStatus();
                //If the user type is administrator then only show access role and mantain user menu
                if (Global.GlobalAccessRoleID == 37)
                {
                    maintainuserToolStripMenuItem.Enabled = true;
                    accessRoleToolStripMenuItem.Enabled = true;
                }


            }
        }

        private void ekqL5ggxfToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmLoginCreate lc = new frmLoginCreate();
            //lc.MdiParent = this;
            lc.ShowDialog();
        }

        private void accessPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccessRole frm = new frmAccessRole();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmJournal frm = new frmJournal();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SETTING_VIEW", "view current setting"))
                return;

            frmSettings frmSet = new frmSettings();
            //frmSet.MdiParent = this;
            frmSet.ShowDialog();
        }

        private void trialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTrialBalanceSetting frmTSet = new frmTrialBalanceSetting(this);
            //frmTSet.MdiParent = this;
            frmTSet.ShowDialog();
        }

        private void MDIMain_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.R)
            //    MessageBox.Show("hsdf");
        }

        private void profitAndLossACToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PROFIT_LOSS", "view profit and loss A/C"))
                return;

            frmProfitLossAccSettings frm = new frmProfitLossAccSettings(this);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void dayBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DAY_BOOKS", "view Day Book"))
                return;

            Accounts.frmDayBookSettings frm = new Accounts.frmDayBookSettings(this);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }



        private void ssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ACCOUNT_LEDGER", "view Account Ledger"))
                return;

            frmAccountLedgerSettings frm = new frmAccountLedgerSettings(this);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_RECEIPT_VIEW", "Cash Receipt"))
                return;

            frmCashReceipt frm = new frmCashReceipt();
            //frm.MdiParent = this;
            frm.ShowDialog();

        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_PAYMENT_VIEW", "Cash Payment"))
                return;

            frmCashPayment frm = new frmCashPayment();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void receiptToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_RECEIPT_VIEW", "Bank Receipt"))
                return;

            frmBankReceipt frm = new frmBankReceipt();
            //frm.MdiParent = this;
            frm.ShowDialog();

        }

        private void paymentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_PAYMENT_VIEW", "Bank Payment"))
                return;

            frmBankPayment frm = new frmBankPayment();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SERVICE", "Services"))
                return;

            frmServices frm = new frmServices();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void bankRecociliatonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_RECONCILATION_VIEW", "view Bank Reconcilation"))
                return;

            frmBankReconciliation frm = new frmBankReconciliation(this);
            // frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void salesInvoiceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SALE_INVOICE_VIEW", "view Sale Invoice"))
                return;

            try
            {
                fsalesinvoice = new frmSalesInvoice(this);
                fsalesinvoice.Show();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);

            }
        }

        private void salesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REPORTS_SALES_REPORT", "view Sales Report"))
                return;

            frmSalesReportSettings frm = new frmSalesReportSettings();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PRODUCT", "Product"))
                return;

            Inventory.frmProduct frm = new Inventory.frmProduct();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void vATReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("VAT_REPORT", "view VAT Report"))
                return;

            frmVATReportSettings frm = new frmVATReportSettings();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void anojToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccessRole frm = new frmAccessRole();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void accountClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ACCOUNT_CLASS_VIEW", "view Account Class"))
                return;

            frmAccountClass frm = new frmAccountClass();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void contraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CONTRA", "Contra Voucher"))
                return;

            frmContra frm = new frmContra();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void debitNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DEBIT_NOTE_VIEW", "view Debit Note"))
                return;

            frmDebitNote frm = new frmDebitNote();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void creditNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CREDIT_NOTE_VIEW", "view Credit Note"))
                return;

            frmCreditNote frm = new frmCreditNote();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void debitNoteRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DEBITNOTE_REGISTER", "Debit Note Registration"))
                return;

            frmDebitNoteRegisterSettings frm = new frmDebitNoteRegisterSettings();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void creditNoteRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CREDITNOTE_REGISTER", "Credit Note Registration"))
                return;

            frmCreditNoteRegisterSettings frm = new frmCreditNoteRegisterSettings();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void voucherConfigurationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CONFIGURATION", "Configuration"))
                return;

            frmVoucherConfiguration frm = new frmVoucherConfiguration();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void journalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("JOURNAL_VIEW", "Journal Voucher"))
                return;

            frmJournal frm = new frmJournal();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BALANCE_SHEET", "Balance Sheet"))
                return;

            frmBalanceSheetSettings frm = new frmBalanceSheetSettings(this);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void trialBalanceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmTrialBalanceSetting m_frmTB = new frmTrialBalanceSetting();
            // m_frmTB.MdiParent = this;
            m_frmTB.ShowDialog();
        }

        private void taxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SLAB_TAX1", "Tax1 Slab"))
                return;

            frmSlabs frm = new frmSlabs(SlabType.TAX1);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void tax2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SLAB_TAX2", "Tax2 Slab"))
                return;

            frmSlabs frm = new frmSlabs(SlabType.TAX2);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void tax3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SLAB_TAX3", "Tax3 Slab"))
                return;

            frmSlabs frm = new frmSlabs(SlabType.TAX3);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void vATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SLAB_VAT", "VAT Slab"))
                return;

            frmSlabs frm = new frmSlabs(SlabType.VAT);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void backupAndRestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BACKUP_RESTORE", "Backup or Restore database"))
                return;

            frmBackUpRestore frm = new frmBackUpRestore();
            frm.ShowDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("COMPANY_INFO_VIEW", "view company information"))
                return;

            frmCompanyInfo frm = new frmCompanyInfo();
            //frm.MdiParent = this;
            frm.ShowDialog();

        }

        private void unitMaintenaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUnitMaitenace frm = new frmUnitMaitenace();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PURCHASE_INVOICE_VIEW", "Purchase Invoice"))
                return;

            try
            {
                fpinvoice = new frmPurchaseInvoice(this);
                fpinvoice.Show();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);

            }
        }

        private void depotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DEPOTE", "Depot"))
                return;

            frmDepot frm = new frmDepot();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void cashFlowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_FLOW", "Cash Flow"))
                return;

            frmCashFlowSettings frm = new frmCashFlowSettings(this);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("JOURNAL_VIEW", "Journal Voucher"))
                return;

            frmJournal frm = new frmJournal();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_RECEIPT_VIEW", "Cash Receipt"))
                return;

            frmCashReceipt frm = new frmCashReceipt();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_PAYMENT_VIEW", "Cash Payment"))
                return;

            frmCashPayment frm = new frmCashPayment();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_RECEIPT_VIEW", "Bank Receipt"))
                return;

            frmBankReceipt frm = new frmBankReceipt();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_PAYMENT_VIEW", "Bank Payment"))
                return;

            frmBankPayment frm = new frmBankPayment();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmBankReconciliation frm = new frmBankReconciliation();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ACCOUNT", "Account"))
                return;

            frmAccountSetup frm = new frmAccountSetup();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PROJECT", "Project"))
                return;

            frmProject frm = new frmProject();
            // frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void trialBalanceToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            //frmTrialBalanceSetting m_frmTB = new frmTrialBalanceSetting();
            ////m_frmTB.MdiParent = this;
            //m_frmTB.ShowDialog();
        }

        private void accessRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAccessRole frm = new frmAccessRole();
            // frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void databaseSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DB_CONNECTION_SETTING", "setup new database connection"))
                return;

            if (MessageBox.Show("WARNING!!! Wrong configuration on these field may result in failure to run this application. Continue only if you understand what you are doing. If you are not sure, simply press cancel.", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;

                frmDBConnect _frmDBConnect = new frmDBConnect(frmDBConnect.Method.Menu);
                _frmDBConnect.ShowDialog();

                this.Cursor = Cursors.Default;
            }
        }

        //This variable is used to check if it is the second time open company has been clicked
        private bool isSecondTimeOpenCompany = false;
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if a user is logged in and 'Open Company' is clicked, a confirmation message will shown'
            if (User.CurrUserID != 0 && isSecondTimeOpenCompany)
            {

                DialogResult dlgResult = MessageBox.Show("You will be logged off if you go to 'Open Company'. Do you want to continue?", "Conformation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgResult == DialogResult.Yes)
                {
                    User.CurrUserID = 0;
                }
                else
                {
                    return;
                }
            }

            //Disable all controls
            Misc.EnableControls(this.Controls, false);

            //1.1 Success

            //m_frmCompanyOpen.ShowDialog();
            //Just enable Open company, system and exit menu

            //Control[] tlsItem={mnuSystem,mnuExit,mnuOpenCompany,helpMenu};

            menuStrip.Enabled = true;
            mnuSystem.Enabled = true;
            //Now disable some menus inside system
            maintainuserToolStripMenuItem.Enabled = false;
            accessRoleToolStripMenuItem.Enabled = false;
            mnuLogOut.Enabled = false;
            mnuLogin.Enabled = false;
            newToolStripMenuItem.Enabled = false;

            //
            //isOpenCompany = true;
            //1.1.1 Show Company open dialgue box

            frmCompanyOpen m_frmCompanyOpen = new frmCompanyOpen();
            if (m_frmCompanyOpen.ShowDialog() == DialogResult.OK)
            {
                //1.1.1.1 If there are existing companies and user selects a company
                //1.1.1.1.1 Show Login dialog box
                mnuLogin.Enabled = true;
                mnuLogOut.Enabled = false;

                frmLogin m_frmLogin = new frmLogin();
                if (m_frmLogin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //This variable is used to confirm log out if the Open Company is clicked and a user is logged in
                    isSecondTimeOpenCompany = true;

                    LoadAccessRoleIdAndSettings(User.CurrUserID);
                    //Enable all controls
                    Misc.EnableControls(this.Controls, true);
                    mnuLogin.Enabled = false;
                    mnuLogOut.Enabled = true;
                    //Method to display status
                    LoadStatus();
                    //if it is system administrator
                    if (Global.GlobalAccessRoleID == 37)
                    {
                        maintainuserToolStripMenuItem.Enabled = true;
                        accessRoleToolStripMenuItem.Enabled = true;
                    }
                    //Load recurring settings here
                    //Checks Reminder and Voucher recurring and opens ReminderList
                    dtSelectedReminder = Reminder.GetReminderIfExistToday(User.CurrUserID, 1);

                    bool isVoucherRecurring = CheckVoucherRecurringPosting();

                    if (dtSelectedReminder.Rows.Count > 0 || isVoucherRecurring == true)
                    {
                        frmReminderList frm = new frmReminderList(this);
                        frm.ShowDialog();
                    }

                }
            }
        }

        //Method to display status with username and access type
        private void LoadStatus()
        {
            //Data table to get access role to display in toolStripStatusLabel
            DataTable dtAccessInfo = User.GetAcessRoleInfo(Global.GlobalAccessRoleID);
            toolStripStatusLabel.Text = " Welcome, " + User.CurrentUserName + "! Logged in as : " + dtAccessInfo.Rows[0]["EngName"].ToString() + " ";

            // display info in the main form
            this.Activated += new System.EventHandler(this.MDIMain_Activated);
            LoadTransactInfo();
        }
        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ACTIVATE", "Activate your application"))
                return;

            frmActivate _frmActivate = new frmActivate();
            _frmActivate.ShowDialog();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewCompany frm = new frmNewCompany();
            frm.ShowDialog();
        }

        private void dateConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DATE_CONVERTER", "Convert Date"))
                return;

            frmDateConverter m_frmDateConverter = new frmDateConverter();
            m_frmDateConverter.StartPosition = FormStartPosition.CenterScreen;
            m_frmDateConverter.Show();
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        }

        private void MDIMain_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    this.Close();
            //}
            if (e.Alt && e.Control && e.KeyCode == Keys.T)
            {
                frmTrialBalanceSetting frm = new frmTrialBalanceSetting(this);
                frm.Show();
            }
            if (e.Alt && e.Control && e.KeyCode == Keys.B)
            {
                frmBalanceSheetSettings frm = new frmBalanceSheetSettings(this);
                frm.Show();
            }
            if (e.Alt && e.Control && e.KeyCode == Keys.P)
            {
                frmProfitLossAccSettings frm = new frmProfitLossAccSettings(this);
                frm.Show();
            }
            if (e.Alt && e.Control && e.KeyCode == Keys.L)
            {
                frmAccountLedgerSettings frm = new frmAccountLedgerSettings(this);
                frm.Show();
            }
            if (e.Alt && e.Control && e.KeyCode == Keys.D)
            {
                frmDayBookSettings frm = new frmDayBookSettings();
                frm.Show();
            }
            if (e.Alt && e.Control && e.KeyCode == Keys.C)
            {
                frmCashFlowSettings frm = new frmCashFlowSettings(this);
                frm.Show();
            }
        }

        private void shortcutKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShortCutKeys frm = new frmShortCutKeys();
            frm.Show();
        }

        private void reminderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REMINDER_VIEW", "view all reminder"))
                return;

            frmReminder frm = new frmReminder();
            frm.ShowDialog();
        }

        private void btnReminder_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REMINDER_VIEW", "view all reminder"))
                return;

            frmReminderList frm = new frmReminderList(this);
            frm.ShowDialog();
        }

        private void slabsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void discountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void receiptToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Accounts.frmchequereceipt chequereceipt = new frmchequereceipt();
            chequereceipt.ShowDialog();
        }

        private void mnuLogin_Click(object sender, EventArgs e)
        {

            //Same as pressing log out
            logInToolStripMenuItem_Click(sender, e);


        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SALE_RETURN_VIEW", "Sale Return"))
                return;

            frmSalesReturn frm = new frmSalesReturn(this);
            frm.Show();
        }

        private void orderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PURCHASE_ORDER_VIEW", "Purchase Order"))
                return;

            frmPurchaseOrder frm = new frmPurchaseOrder(this);
            frm.Show();
        }

        private void orderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SALES_ORDER_VIEW", "Sales Order"))
                return;

            frmSalesOrder frm = new frmSalesOrder(this);
            frm.Show();
        }

        private void returnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PURCHASE_RETURN_VIEW", "Purchase Return"))
                return;

            frmPurchaseReturn frm = new frmPurchaseReturn(this);
            frm.Show();
        }

        private void chequeRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CHEQUE_REGISTER", "Cheque Register"))
                return;

            frmChequeSummarySettings frm = new frmChequeSummarySettings();
            frm.ShowDialog();
        }

        private void freezeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("FREEZE_DATE", "Freeze Date"))
                return;

            frmFreeze frm = new frmFreeze();
            frm.ShowDialog();
        }

        private void purchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REPORTS_PURCHASE_REPORT", "view Purchase Report"))
                return;

            frmPurchaseReportSettings frm = new frmPurchaseReportSettings();
            frm.ShowDialog();
        }

        private void ledgerConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CONFIGURATION", "Configuration"))
                return;

            frmLedgerConfiguration frm = new frmLedgerConfiguration();
            frm.ShowDialog();
        }

        private void MDIMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            ////Check connection. If the system is not connected to the database, in case of activation or other issues
            try
            {
                //Try connecting using the new connection
                SqlDb m_db = new SqlDb();
                //In case of SQL Server connection
                m_db.ServerName = RegManager.ServerName;
                m_db.DbName = RegManager.DataBase;
                m_db.UserName = RegManager.DBUser;
                m_db.Password = RegManager.DBPassword;

                if (m_db.Connect())
                {
                    RegManager.ServerName = m_db.ServerName;
                    RegManager.DataBase = m_db.DbName;
                    RegManager.DBUser = m_db.UserName;
                    RegManager.DBPassword = m_db.Password;
                    Global.m_db.cn = m_db.cn;
                    this.Dispose();
                }
                else //Connection failure
                {
                    Global.MsgError("There was an error selecting the company, please select another company and try again. If it doesnt resolve, try closing and reopening the application again!");
                }

            }
            catch (Exception)
            {
                Global.Msg("Please select the  company and try again");
            }
            if (Settings.GetSettings("AUTO_BACKUP") == "1")
            {
                //int result = DateTime.Compare( Convert.ToDateTime(Settings.GetSettings("DATE_TO_BACKUP")), DateTime.Today )
                if (Convert.ToDateTime(Settings.GetSettings("LAST_BACKUP_DATE")).AddDays(Convert.ToInt32(Settings.GetSettings("BACKUP_INTERVAL_DAY"))) <= Date.GetServerDate())
                {
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
                    ProgressForm.UpdateProgress(20, "Initializing backup...");

                    //calculate backupname
                    string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                    appPath = Directory.GetParent(appPath).ToString();
                    //System.IO.DirectoryInfo directoryInfo = System.IO.Directory.GetParent(appPath);
                    //System.IO.DirectoryInfo directoryInfo1 = System.IO.Directory.GetParent(directoryInfo.FullName);
                    //string path = directoryInfo1.FullName + @"\Backup";

                    string path = appPath + @"\AccSwift Database Backup";
                    //to create directory
                    if (Directory.Exists(path))
                    {
                        //Do nothing
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Update the progressbar
                    ProgressForm.UpdateProgress(40, "Calculating Parameters...");

                    string filename = string.Format("AccSwift-DB-{0:yyyy-MM-dd_hh-mm-ss-tt}.bak", DateTime.Now);
                    string PathToBackUp = "";
                    if (Settings.GetSettings("BACKUP_PATH") == "0")
                    {
                        PathToBackUp = path;
                    }
                    else if (Settings.GetSettings("BACKUP_PATH") != "0")
                    {
                        PathToBackUp = Settings.GetSettings("BACKUP_PATH");
                    }
                    else
                    {
                        PathToBackUp = path;
                    }
                    //Update the progressbar
                    ProgressForm.UpdateProgress(80, "Saving database...");
                    try
                    {
                        string strQuery = "backup database [" + SqlDb._DbName + "] to disk ='" + PathToBackUp + "\\" + filename + "'";
                        Global.m_db.SelectQry(strQuery, "tblBackup");
                        //SqlDb.BackupDatabase(SqlDb._DbName, PathToBackUp + "\\" + filename, Global.m_db.cn);
                    }
                    catch
                    {
                        MessageBox.Show("Backup failed!");
                        ProgressForm.UpdateProgress(100, "Backup failed ....");
                        ProgressForm.CloseForm();
                        return;
                    }

                    //Update the progressbar
                    ProgressForm.UpdateProgress(100, "Creating new backup date...");
                    // Close the dialog
                    ProgressForm.CloseForm();
                    //Update the date
                    Settings m_Settings = new Settings();
                    string Code, Value;
                    Code = "LAST_BACKUP_DATE";
                    //Value = Date.GetServerDate().AddDays(Convert.ToInt32(Settings.GetSettings("BACKUP_INTERVAL_DAY"))).ToString("yyyy/MM/dd");
                    Value = Date.GetServerDate().ToString("yyyy/MM/dd");
                    m_Settings.SetSettings(Code, Value);
                    Application.Exit();
                }
            }
        }

        private void dayBookToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_DAY_BOOK", "View Inventory Day Book"))
                return;

            frmInventoryDayBookSettings frm = new frmInventoryDayBookSettings();
            frm.Show();
        }

        private void purchaseRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_PURCHASE_REGISTER", "View Inventory Purchase Register"))
                return;
            frmPurchaseRegisterSettings frm = new frmPurchaseRegisterSettings();
            frm.Show();
        }

        private void purchaseReturnRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_PURCHSE_RETURN_REGISTER", "View Inventory Purchase Return Register"))
                return;
            frmPurchaseReturnSettings frm = new frmPurchaseReturnSettings();
            frm.Show();
        }

        private void salesRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_SALES_REGISTER", "View Inventory Sales Register"))
                return;
            frmSalesRegisterSettings frm = new frmSalesRegisterSettings();
            frm.Show();
        }

        private void salesReturnRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_SALES_RETURN_REGISTER", "View Inventory Sales Return Register"))
                return;
            frmSalesReturnRegisterSettings frm = new frmSalesReturnRegisterSettings();
            frm.Show();
        }

        private void stockLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_STOCK_LEDGER", "View Inventory Stock Ledger"))
                return;
            frmStockLedgerSettings frm = new frmStockLedgerSettings(this);
            frm.Show();
        }

        private void testCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestCalculator frm = new frmTestCalculator();
            frm.Show();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccSwift.Forms.frmchangepassword c = new Forms.frmchangepassword(User.CurrUserID);
            c.ShowDialog();
        }

        private void menuStrip_Click(object sender, EventArgs e)
        {
            //if (User.CurrUserID == 1) 

            //if (Global.GlobalAccessRoleID==37)
            //{
            //    maintainuserToolStripMenuItem.Enabled = true;
            //    accessRoleToolStripMenuItem.Enabled = true;
            //}
            //else
            //{
            //    maintainuserToolStripMenuItem.Enabled = false;
            //    accessRoleToolStripMenuItem.Enabled = false;
            //}
        }

        private void testReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataSet.dsTest mydataset = new DataSet.dsTest();

            mydataset.Tables["test"].Rows.Add(1, "Bimal Khadka", "9806682613");
            mydataset.Tables["test"].Rows.Add(2, "Samit Shrestha", "9841256987");
            mydataset.Tables["test"].Rows.Add(3, "Prabhu Batas", "9845678985");

            AccSwift.CrystalReports.CRtest rpt = new CrystalReports.CRtest();

            rpt.SetDataSource(mydataset);

            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);

            frm.Show();
        }


        private void createMultipleLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AccSwift.Forms.Accounting.frmmultipleledger ml = new Forms.Accounting.frmmultipleledger();
            //ml.ShowDialog();
        }

        private void bulkVoucherPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BULK_VOUCHER_POSTING", "Bulk Voucher Posting"))
                return;

            Accounts.frmbulkvoucherposting fvp = new frmbulkvoucherposting();
            fvp.ShowDialog();
        }

        private void fiscalYearClosingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("FISCALYEAR_CLOSING", "Fiscal Year Closing"))
                return;

            DataTable dt = new DataTable();
            dt = User.GetUserInfo(User.CurrUserID);
            DataRow druserinfo = dt.Rows[0];
            if (Convert.ToInt32(druserinfo["AccessRoleID"].ToString()) == 37)
            {
                AccSwift.Forms.frmfiscalyearclosing fyclosing = new Forms.frmfiscalyearclosing(this);
                fyclosing.ShowDialog();
            }
            else
            {
                Global.Msg("You Are Not Authorised To View");
                return;
            }
        }

        private void calculateDepreciationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ADJUST_DEPRECIATION", "Adjust Depreciation"))
                return;

            AccSwift.Forms.frmdepreciation dep = new Forms.frmdepreciation();
            dep.ShowDialog();
        }

        private void stockTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("STOCK_TRANSFER_VIEW", "Stock Transfer"))
                return;

            frmStockTransfer frm = new frmStockTransfer(this);
            //frm.MdiParent = this;
            frm.ShowDialog();

        }

        //public IMDIMainForm frmAccClass;
        private void damageITemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DAMAGE_ITEMS_VIEW", "Damage Items"))
                return;

            try
            {
                fdmg = new frmDamageItems();
                fdmg.Show();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);

            }
        }

        private void auditLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("AUDIT_LOG", "Audit Log"))
                return;

            AccSwift.Forms.frmAuditLog frmauditlog = new Forms.frmAuditLog();
            frmauditlog.ShowDialog();
        }

        private void stockStatusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("STOCK_STATUS", "Stock Status"))
                return;

            frmStockSettings frm = new frmStockSettings(this);
            frm.Show();
        }

        private void testloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AccSwift.Forms.kfrmlogin klogin = new Forms.kfrmlogin();
            //klogin.ShowDialog();
        }

        private void userPreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("USER_PREFERENCES", "user preferences"))
                return;

            AccSwift.Forms.frmUserPreference frmuserpref = new Forms.frmUserPreference();
            frmuserpref.ShowDialog();
        }

        private void notePadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad");
        }

        private void pOSVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // POS.frmpossalesinvoice frmposinv = new POS.frmpossalesinvoice();
            //frmposinv.ShowDialog();
        }

        private void salesInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private int GetRootAccClassID()
        {
            if (Global.GlobalAccClassID > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(Global.GlobalAccClassID));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
        }

        private void userEnableDisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("USER_ENABLE/DISABLE", "enable or disable user"))
                return;

            frmuserenabledisable frmenabledisable = new frmuserenabledisable();
            frmenabledisable.ShowDialog();
        }

        private void MDIMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }

        private void accountBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void purchaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void mBalanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounts.Reports.frmMBalanceSheetSettings frmmbalancesheet = new Accounts.Reports.frmMBalanceSheetSettings();
            frmmbalancesheet.ShowDialog();
        }

        private void closingTrialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CLOSING_TRIAL_BALANCE", "close trial balance"))
                return;

            Global.isOpeningTrial = false;
            frmTrialBalanceSetting m_frmTB = new frmTrialBalanceSetting(this);
            //m_frmTB.MdiParent = this;
            m_frmTB.ShowDialog();
        }

        private void openingTrialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("OPENING_TRIAL_BALANCE", "open trial balance"))
                return;

            Global.isOpeningTrial = true;
            frmTrialBalanceSetting m_frmTB = new frmTrialBalanceSetting(this);
            m_frmTB.ShowDialog();
        }

        private void generalStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBalanceSheetSettings frm = new frmBalanceSheetSettings(this);
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void standardStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounts.Reports.frmMBalanceSheetSettings frmmbalancesheet = new Accounts.Reports.frmMBalanceSheetSettings();
            frmmbalancesheet.ShowDialog();
        }

        private void ledgerDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLedgerDetails ld = new frmLedgerDetails();
            ld.ShowDialog();
        }

        private void createBarCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Common.frmemail email = new Common.frmemail();
            email.ShowDialog();
        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmbulksms sms = new frmbulksms();
            sms.ShowDialog();

        }

      

        private void customDutyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SLAB_CUSTOM_DUTY", "Custom Duty Slab"))
                return;

            frmSlabs frm = new frmSlabs(SlabType.CUSTOMDUTY);
            frm.ShowDialog();
        }

        private void ageingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DEBTORS_AGEING", "Debtors Ageing"))
                return;
            Accounts.frmDebtorAgeing dageing = new Accounts.frmDebtorAgeing(this);
            dageing.ShowDialog();
        }

        private void chequeReminderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChequeCashReminder rem = new frmChequeCashReminder();
            rem.ShowDialog();
        }

        private void dueDaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("DEBTORS_DUE_DAYS", "Debtors Due Days"))
                return;
            //Forms.Accounting.frmDebtorsDue due = new Forms.Accounting.frmDebtorsDue();
            //due.ShowDialog();
            Accounts.frmDebtorsDue debdue = new Accounts.frmDebtorsDue(this);
            debdue.ShowDialog();
        }

        private void importToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImportFromExcel excelimport = new frmImportFromExcel();
            excelimport.ShowDialog();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importDataFromTallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmexportdatafromtally tally = new frmexportdatafromtally();
            //tally.ShowDialog();
        }

        private void stockAgeingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("INVENTORY_BOOK_STOCK_AGEING", "View Inventory Stock Ageing"))
                return;
            frmStockAgingSetting frmagingsetting = new frmStockAgingSetting();
            frmagingsetting.ShowDialog();
        }

        private void leaveSetUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmLeaveSetUp leavesetup = new frmLeaveSetUp();
            //leavesetup.ShowDialog();
        }

        private void officeCalanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmOfficeCalander officecalender = new frmOfficeCalander();
            //officecalender.ShowDialog();
        }

        private void leaveRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmLeaveRequest leaverequest = new frmLeaveRequest();
            //leaverequest.ShowDialog();
        }

        private void leaveApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmleaveapproval leaveapproval = new frmleaveapproval();
            //leaveapproval.ShowDialog();
        }

       

        private void paymentToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmBankPayment bankpay = new frmBankPayment();
            bankpay.ShowDialog();
        }

       

        private void button8_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CONTRA", "Contra Voucher"))
                return;

            frmContra frm = new frmContra();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }





        private void accountsTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Ticks every second
        /// Displays Date according to Default Date type selected by user
        /// Date type is checked here so that it status changes as the user changes the setting in frmSetting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Displaying english or nepali date according to user preference and time
            string NepOrEng;
            if (Global.Default_Date == Date.DateType.Nepali)
            {
                NepOrEng = " Nep Date: " + Global.CurrentDateNep + " ⁞ Time: " + DateTime.Now.ToString("hh:mm:ss tt "); ;
            }
            else if (Global.Default_Date == Date.DateType.English)
            {

                DateTime EngDate = Global.CurrentDateEng;
                NepOrEng = " Eng Date: " + EngDate.ToString("yyyy'/'MM'/'dd") + " ⁞ Time: " + DateTime.Now.ToString("hh:mm:ss tt ");
            }
            else
            {
                NepOrEng = " Time: " + DateTime.Now.ToString("hh:mm:ss tt ");
            }
            toolStripStatusLabelDate.Text = NepOrEng;

        }

        private void createBarCodeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmBarCode frmbarcode = new frmBarCode();
            frmbarcode.ShowDialog();
            //// Create linear barcode object
            //Linear barcode = new Linear();
            //// Set barcode symbology type to Code-39
            ////barcode.Type = BarcodeType.CODE39;
            //barcode.Type = BarcodeType.CODABAR;
            //// Set barcode data to encode
            //barcode.Data = "36";
            //// Set barcode bar width (X dimension) in pixel
            //barcode.X = 2;
            //// Set barcode bar height (Y dimension) in pixel
            //barcode.Y = 60;
            //// Draw & print generated barcode to png image file
            ////barcode.drawBarcode("C://csharp-code39.png");
            // barcode.drawBarcode("E://productbarcode.png");

            //// Create linear barcode object
            //Linear barcode = new Linear();
            //// Set barcode symbology type to Code-39
            //barcode.Type = OnBarcode.Barcode.BarcodeType.CODE39;
            //// Set barcode data to encode
            //barcode.Data = "36";
            //// Encode barcodes to other image format, by change file extension
            //barcode.Format = System.Drawing.Imaging.ImageFormat.Gif;
            //barcode.drawBarcode("E://csharp-barcode-code39.gif");
            // MessageBox.Show("Barcode Created Successfully");
        }

        private void btnSalesInvoice_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SALE_INVOICE_VIEW", "view Sale Invoice"))
                return;

            try
            {
                fsalesinvoice = new frmSalesInvoice(this);
                fsalesinvoice.Show();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);

            }
        }

        #region related to recurring
        private DataTable dtRecurringVoucherPosting, dtRecurringVoucherPosting1, dtRecurringVoucher;

        /// <summary>
        /// check if voucher recurring exists for today
        /// </summary>
        /// <returns></returns>
        private bool CheckVoucherRecurringPosting()
        {
            try
            {
                RecurringVoucher.DeleteRecurringVoucherPosting(); // first delete all postings from dates before today with isPosting = true

                bool res = true;
                dtRecurringVoucherPosting = RecurringVoucher.GetRecurringVoucherPosting((System.DateTime.Today).Date);
                if (dtRecurringVoucherPosting.Rows.Count == 0)
                {
                    res = ConvertSetitngToDate();
                }
                else
                {
                    dtRecurringVoucherPosting = RecurringVoucher.GetNotPostedRecurring((System.DateTime.Today).Date);
                    if (dtRecurringVoucherPosting.Rows.Count == 0)
                        res = false;

                }
                return res;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return false;
            }
        }



        //private void ConvertSetitngToDate()
        //{
        //    try
        //    {
        //        //dtRecurringVoucher.Columns.Add("RVPID");
        //        //dtRecurringVoucher.Columns.Add("VoucherType");
        //        //dtRecurringVoucher.Columns.Add("Date");
        //        //dtRecurringVoucher.Columns.Add("VoucherID");
        //        //dtRecurringVoucher.Columns.Add("isPosted");

        //        dtRecurringVoucher = RecurringVoucher.GetRecurringVoucher();
        //        int dateToday = (int)DateTime.Today.DayOfWeek; // get todays English date
        //        int Year = (int)DateTime.Today.Year;            // get todays year
        //        int Month = (int)DateTime.Today.Month;          // get todays month
        //        int Day1 = (int)DateTime.Today.Day;          // get todays day
        //        int Day = 0, Month2 = 0;

        //        // convert English date into Nepali
        //        Date.EngToNep(DateTime.Today.Date, ref Year, ref Month, ref Day1);
        //        DataTable LastDay = Date.LastDayofMonthNep(Year, Month);  // Calculate last day or no. of days of the month

        //        if (Convert.ToInt32(LastDay.Rows[0][0]) < Day1)        // if no. of days in the month is greater than stored day then last day is used
        //            Day1 = Convert.ToInt32(LastDay.Rows[0][1]);

        //        foreach (DataRow dr in dtRecurringVoucher.Rows) // check each row of voucher recurring setting
        //        {
        //            if (dr["RecurringType"].ToString() == "DAILY")
        //            {
        //                if (dr["Unit1"].ToString().Contains((dateToday + 1).ToString()))
        //                {
        //                    AddPosting(dr);
        //                }
        //            }
        //            else if (dr["RecurringType"].ToString() == "MONTHLY")
        //            {

        //                if (dr["Unit1"].ToString() != "100")
        //                {

        //                    Day = Convert.ToInt32(dr["Unit1"]);

        //                }
        //                else
        //                    Day = Convert.ToInt32(LastDay.Rows[0][1]);


        //                if (Day1 == Day)
        //                {
        //                    AddPosting(dr);
        //                }
        //            }
        //            else if (dr["RecurringType"].ToString() == "YEARLY")
        //            {
        //                if (dr["Unit2"].ToString() != "100")
        //                {
        //                    Month2 = Convert.ToInt32(dr["Unit1"]);
        //                    Day = Convert.ToInt32(dr["Unit2"]);
        //                }
        //                else
        //                {
        //                    int month3 = 12;
        //                    DataTable LastDay2 = Date.LastDayofMonthNep(Year, month3);  // Calculate last day or no. of days of the month
        //                    Day = Convert.ToInt32(LastDay.Rows[0][1]);
        //                }
        //                if (Day1 == Day && Month2 == Month)
        //                {
        //                    AddPosting(dr);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError(ex.Message);
        //    }
        //}

        //public void AddPosting(DataRow dr)
        //{
        //    try
        //    {
        //        AddColumns();

        //        dtRecurringVoucherPosting1.Rows.Add( dr["VoucherType"], DateTime.Today.Date, dr["VoucherID"],0);
        //        //MessageBox.Show(dtRecurringVoucherPosting.Columns.Count.ToString())
        //        ////dtRecurringVoucherPosting.Rows[0]["RVPID"] = "";
        //        //dtRecurringVoucherPosting1.Rows[1]["VoucherType"] = dr["VoucherType"];
        //        //dtRecurringVoucherPosting1.Rows[1]["Date"] = DateTime.Today.Date;
        //        //dtRecurringVoucherPosting1.Rows[1]["VoucherID"] = dr["VoucherID"];
        //        //dtRecurringVoucherPosting1.Rows[1]["isPosted"] = 0;
        //        //RecurringVoucher.CreateRecurringVoucherPosting(dtRecurringVoucherPosting1);
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError(ex.Message);
        //    }
        //}

        /// <summary>
        /// check if voucher recurring exists for today
        /// </summary>
        /// <returns></returns>
        private bool ConvertSetitngToDate()
        {
            try
            {
                dtRecurringVoucher = RecurringVoucher.GetRecurringVoucher(System.DateTime.Today);
                int dateToday = (int)DateTime.Today.DayOfWeek; // get todays English date
                int CurYear = (int)DateTime.Today.Year;            // get todays year
                int CurMonth = (int)DateTime.Today.Month;          // get todays month
                int CurDay = (int)DateTime.Today.Day;          // get todays day
                int RecurringDay = 0, RecurringMonth = 0;

                // convert English date into Nepali
                Date.EngToNep(DateTime.Today.Date, ref CurYear, ref CurMonth, ref CurDay);
                DataTable LastDay = Date.LastDayofMonthNep(CurYear, CurMonth);  // Calculate last day or no. of days of the month

                if (Convert.ToInt32(LastDay.Rows[0][0]) < CurDay)        // if no. of days in the month is greater than stored day then last day is used
                    CurDay = Convert.ToInt32(LastDay.Rows[0][0]);

                foreach (DataRow dr in dtRecurringVoucher.Rows) // check each row of voucher recurring setting
                {
                    if (dr["RecurringType"].ToString() == "DAILY")
                    {
                        if (dr["Unit1"].ToString().Contains((dateToday + 1).ToString()))
                        {
                            AddPosting(dr);
                        }
                    }
                    else if (dr["RecurringType"].ToString() == "MONTHLY")
                    {

                        if (dr["Unit1"].ToString() != "100")
                        {

                            RecurringDay = Convert.ToInt32(dr["Unit1"]);

                        }
                        else
                            RecurringDay = Convert.ToInt32(LastDay.Rows[0][0]);


                        if (CurDay == RecurringDay)
                        {
                            AddPosting(dr);
                        }
                    }
                    else if (dr["RecurringType"].ToString() == "YEARLY")
                    {
                        if (dr["Unit2"].ToString() != "100")
                        {
                            RecurringMonth = Convert.ToInt32(dr["Unit1"]);
                            RecurringDay = Convert.ToInt32(dr["Unit2"]);
                        }
                        else
                        {
                            RecurringMonth = 12;
                            DataTable lastDayOfLastMonth = Date.LastDayofMonthNep(CurYear, RecurringMonth);  // Calculate last day or no. of days of the month
                            RecurringDay = Convert.ToInt32(lastDayOfLastMonth.Rows[0][0]);
                        }
                        if (CurDay == RecurringDay && RecurringMonth == CurMonth)
                        {
                            AddPosting(dr);
                        }
                    }
                }
                if (RecurringVoucher.GetNotPostedRecurring((System.DateTime.Today).Date).Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// if voucher recurring exists for today then add related data in voucherposting table
        /// </summary>
        /// <param name="dr"></param>
        public void AddPosting(DataRow dr)
        {
            //dtRecurringVoucherPosting = new DataTable();

            //dtRecurringVoucherPosting.Columns.Add("VoucherType");
            //dtRecurringVoucherPosting.Columns.Add("Date");
            //dtRecurringVoucherPosting.Columns.Add("VoucherID");
            //dtRecurringVoucherPosting.Columns.Add("isPosted");
            try
            {
                dtRecurringVoucherPosting.Rows.Clear();
                DataRow dr1 = dtRecurringVoucherPosting.NewRow();
                dr1["VoucherType"] = dr["VoucherType"];
                dr1["Date"] = DateTime.Today.Date;
                dr1["VoucherID"] = dr["VoucherID"];
                dr1["isPosted"] = 0;
                dtRecurringVoucherPosting.Rows.Add(dr1);
                RecurringVoucher.CreateRecurringVoucherPosting(dtRecurringVoucherPosting);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        } 
        #endregion
     
        private void budgetSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BUDGET_SETUP_VIEW", "view Budget Setup"))
                return;

            Accounts.View.frmBudget fb = new Accounts.View.frmBudget();
            fb.ShowDialog();
        }

        private void budgetAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (UserPermissionNotGranted("BUDGET_ALLOC_VIEW", "view Budget Allocation"))
            //    return;
            Accounts.View.frmBudgetAllocation fba = new Accounts.View.frmBudgetAllocation();
            fba.ShowDialog();
        }

        private void budgetReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BUDGET_REPORT", "view Budget Report"))
                return;
            frmBudgetReportSetting fbr = new frmBudgetReportSetting();
            fbr.ShowDialog();
        }

        #region Button Mouse enter and leave events for displaying short info of the button on 

        //Dictionary for key and value of the button
        Dictionary<string, string> dMenuList = new Dictionary<string, string>()
        {
            //Quick Reach
            {"btnQRJournal", "Journal Voucher."},
            {"btnQRCashRcpt", "Cash Receipt."},
            {"btnQRCashPmnt", "Cash Payment."},
            {"btnQRContraVch", "Contra Voucher."},
            {"btnQRBankRcpt", "Bank Receipt."},
            {"btnQRBankPmnt", "Bank Payment."},
            {"btnQRInitializeAccnt", "Account Group and Ledger setup."},
            {"btnQRSalesInvoice", "Sales Invoice."},
            {"btnQRReminder", "General Reminder and Recurring Voucher Reminder."},

            //System
            {"mnuOpenCompany", "Select a Company."},
            {"newToolStripMenuItem", "Create a New Company."},
            {"mnuLogin", "Log In."},
            {"mnuLogOut", "Log Out."},
            {"maintainuserToolStripMenuItem", "Create, Edit or Delete a User."},
            {"accessRoleToolStripMenuItem", "Maintain Access Control."},
            {"mnuExit", "Exit the System."},

            //Initialize
            {"accountsToolStripMenuItem", "Account Group and Ledger setup."},
            {"productToolStripMenuItem", "Product Group and Product setup."},
            //{"servicesToolStripMenuItem", "."},
            {"voucherConfigurationToolStripMenuItem1", "Configure Voucher Number."},
            {"ledgerConfigurationToolStripMenuItem", "Configure Ledger and Account Group Code."},
            {"taxToolStripMenuItem", "Assign the Rate of Tax1."},
            {"tax2ToolStripMenuItem", "Assign the Rate of Tax2."},
            {"tax3ToolStripMenuItem", "Assign the Rate of Tax3."},
            {"vATToolStripMenuItem", "Assign the Rate of VAT."},
            {"customDutyToolStripMenuItem", "Assign the Rate of Custom Duty."},
            {"accountClassToolStripMenuItem", "Setup Account Class."},
            {"depotToolStripMenuItem", "Setup Depot."},
            {"projectToolStripMenuItem", "Setup Project."},
            {"budgetSetupToolStripMenuItem", "Setup Budget."},
            {"budgetAllocationToolStripMenuItem", "Allocate Ledger and Budget for the Ledger to a Budget."},
            //{"", "."},
            //{"", "."},
            //{"", "."},

            //Transactions
            {"receiptToolStripMenuItem", "Cash Receipt."},
            {"paymentToolStripMenuItem", "Cash Payment."},
            {"receiptToolStripMenuItem1", "Bank Receipt."},
            {"paymentToolStripMenuItem1", "Bank Payment."},
            {"receiptToolStripMenuItem2", "Cheques Receipt."},
            {"paymentToolStripMenuItem2", "Cheques Payment."},
            {"journalToolStripMenuItem", "Journal Voucher."},
            {"contraToolStripMenuItem1", "Contra Voucher."},
            {"bankRecociliatonToolStripMenuItem", "Bank Reconciliation."},
            {"debitNoteToolStripMenuItem", "Debit Note."},
            {"creditNoteToolStripMenuItem", "Credit Note."},
            {"invoiceToolStripMenuItem", "Purchase Invoice."},
            {"returnToolStripMenuItem1", "Purchase Return."},
            {"orderToolStripMenuItem1", "Purchase Order."},
            {"salesInvoiceToolStripMenuItem1", "Sales Invoice."},
            {"returnToolStripMenuItem", "Sales Return."},
            {"orderToolStripMenuItem", "Sales Order."},
            {"stockTransferToolStripMenuItem", "Stock Transfer."},
            {"damageITemsToolStripMenuItem", "Damage Items."},
            {"bulkVoucherPostingToolStripMenuItem", "Bulk Voucher Posting."},
            //{"", "."},
            //{"", "."},
            //{"", "."},
            //{"", "."},
            //{"", "."},

            //Tools
            {"optionsToolStripMenuItem", "Settings of the System."},
            {"databaseSettingsToolStripMenuItem", "Database Settings. Proceed only if you know what you are doing."},
            {"toolStripMenuItem4", "Company Informations."},
            {"backupAndRestoreToolStripMenuItem", "Backup and Restore Database."},
            {"unitMaintenaceToolStripMenuItem", "Maintain Unit for Product."},
            {"dateConverterToolStripMenuItem", "Date Converter."},
            {"calculatorToolStripMenuItem", "Calculator."},
            {"reminderToolStripMenuItem", "General Reminder and Recurring Voucher Reminder."},
            {"freezeDateToolStripMenuItem", "Freeze Date for Transactions."},
            {"activateToolStripMenuItem", "Activate the System."},
            {"changePasswordToolStripMenuItem", "Change Password."},
            {"fiscalYearClosingToolStripMenuItem", "Fiscal Year Closing."},
            {"calculateDepreciationToolStripMenuItem", "Provide % for Asset Depreciation."},
            {"userPreferencesToolStripMenuItem", "User Preferences."},
            {"userEnableDisableToolStripMenuItem", "Enable or Disable a User."},
            {"chequeReminderToolStripMenuItem", "Cheque Reminder."},
            {"createBarCodeToolStripMenuItem1", "Create Bar Code for Products."},
            
            //Reports
            {"balanceSheetToolStripMenuItem", "Financial statement of assets and liabilities."},
            {"trialBalanceToolStripMenuItem1", "A bookkeeping worksheet in which the balances of all ledgers are compiled into debit and credit columns"},
            {"openingTrialToolStripMenuItem", "Trial Balance of all the Opening balance."},
            {"closingTrialToolStripMenuItem", "Trial Balance of all the Opening balance and transactions."},
            {"profitAndLossACToolStripMenuItem", "A financial statement showing a company's net profit or loss in a given period."},
            {"dayBookToolStripMenuItem", "An account book in which all the transactions are entered in the order of their occurrence."},
            {"ssToolStripMenuItem", "The Account Ledger Report shows all transactions from all accounts for a chosen date range."},
            {"purchaseReportToolStripMenuItem", "A report of all the purchase transaction."},
            {"salesToolStripMenuItem1", "A report of all sales transaction."},
            {"vATReportToolStripMenuItem", "A report of all payable and paid VAT."},
            {"debitNoteRegisterToolStripMenuItem", "A report of all the debit note."},
            {"creditNoteRegisterToolStripMenuItem", "A report of all the credit note."},
            {"cashFlowToolStripMenuItem", "A financial statement showing all cash inflow and outflow."},
            {"chequeRegisterToolStripMenuItem", "A report of all the cheque payment and receipt."},
            {"dayBookToolStripMenuItem1", "A report of all the transactions related to inventory."},
            {"purchaseRegisterToolStripMenuItem", "A report of all the purchase transactions."},
            {"purchaseReturnRegisterToolStripMenuItem", "A report of all the purchase return transactions."},
            {"salesRegisterToolStripMenuItem", "A report of all the sales transactions."},
            {"salesReturnRegisterToolStripMenuItem", "A report of all the sales return transactions."},
            {"stockLedgerToolStripMenuItem", "A report to show the flow of stock."},
            //{"stockAgeingToolStripMenuItem", "."},
            {"stockStatusToolStripMenuItem1", "A stock availability report."},
            {"auditLogToolStripMenuItem", "A log report."},
            {"budgetReportToolStripMenuItem", "Budget report of all the assigned budget and actual transaction amount."},
        };

        private void btn_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if(sender is Button)
                {
                    Button obj = (Button)sender;
                    toolStripStatusLabelInfo.Text = dMenuList[obj.Name];
                }
                else if (sender is ToolStripMenuItem)
                {
                    ToolStripMenuItem obj = sender as ToolStripMenuItem;
                    toolStripStatusLabelInfo.Text = dMenuList[obj.Name];
                }

            }
            catch (Exception)
            {

            }
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelInfo.Text = "";
            }
            catch (Exception )
            {

            }
        }
        #endregion

        private void btnQRJournal_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("JOURNAL_VIEW", "Journal Voucher"))
                return;

            frmJournal frm = new frmJournal();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRCashRcpt_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_RECEIPT_VIEW", "Cash Receipt"))
                return;

            frmCashReceipt frm = new frmCashReceipt();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRCashPmnt_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CASH_PAYMENT_VIEW", "Cash Payment"))
                return;

            frmCashPayment frm = new frmCashPayment();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRContraVch_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("CONTRA", "Contra Voucher"))
                return;

            frmContra frm = new frmContra();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRBankRcpt_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_RECEIPT_VIEW", "Bank Receipt"))
                return;

            frmBankReceipt frm = new frmBankReceipt();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRBankPmnt_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("BANK_PAYMENT_VIEW", "Bank Payment"))
                return;

            frmBankPayment frm = new frmBankPayment();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRInitializeAccnt_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("ACCOUNT", "Account"))
                return;

            frmAccountSetup frm = new frmAccountSetup();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void btnQRSalesInvoice_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("SALE_INVOICE_VIEW", "view Sale Invoice"))
                return;

            try
            {
                fsalesinvoice = new frmSalesInvoice(this);
                fsalesinvoice.Show();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);

            }
        }

        private void btnQRReminder_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REMINDER_VIEW", "view all reminder"))
                return;

            frmReminderList frm = new frmReminderList(this);
            frm.ShowDialog();
        }

        #region display different info about transactions, sales ...
        public static void DisplayBanners()
        {

        } 
        #endregion

        #region display transactons and sales info in main form
        public enum TransactType { All_Today = 0, All_Month, All_Year, Sales_Today, Sales_Month, Sales_Year, Sales_Amount_Today, Sales_Amount_Month, Sales_Amount_Year, Recurring_Reminder };
        public void LoadTransactInfo()
        {
            try
            {
                DataTable dtInfo = Sales.GetTransactInfo();
                lblUser.Text = " Welcome, " + User.CurrentUserName + "!";
                if (dtInfo.Rows.Count > 0)
                {
                    #region old code as switch case, loop is not required
                    //foreach (DataRow dr in dtInfo.Rows)
                    //{
                    //string value = dr["Value"].ToString();
                    //switch (dr["TransactionType"].ToString())
                    //{
                    //    case "All_Today":
                    //        lblTransactionToday.Text = value;
                    //        break;

                    //    case "All_Month":
                    //        lblTransactionMonth.Text = value;
                    //        break;

                    //    case "All_Year":
                    //        lblTransactionYear.Text = value;
                    //        break;

                    //    case "Sales_Today":
                    //        lblSalesToday.Text = value;
                    //        break;

                    //    case "Sales_Month":
                    //        lblSalesMonth.Text = value;
                    //        break;

                    //    case "Sales_Year":
                    //        lblSalesYear.Text = value;
                    //        break;

                    //    case "Sales_Amount_Today":
                    //        lblSalesToday.Text += ( " / Rs." + value);
                    //        break;

                    //    case "Sales_Amount_Month":
                    //        lblSalesMonth.Text += (" / Rs." + value);
                    //        break;

                    //    case "Sales_Amount_Year":
                    //        lblSalesYear.Text += (" / Rs." + value);
                    //        break;

                    //    case "Recurring_Reminder":
                    //        lblRecurringReminder.Text = value;
                    //        lblAllReminder.Text = (dtSelectedReminder.Rows.Count + Convert.ToInt32(value)).ToString();
                    //        break;
                    //}
                    //} 
                    #endregion

                    lblTransactionToday.Text = Convert.ToDecimal(dtInfo.Rows[(int)TransactType.All_Today]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, 0));
                    lblTransactionMonth.Text = Convert.ToDecimal(dtInfo.Rows[(int)TransactType.All_Month]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, 0));
                    lblTransactionYear.Text = Convert.ToDecimal(dtInfo.Rows[(int)TransactType.All_Year]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, 0));
                    lblSalesToday.Text = Convert.ToDecimal(dtInfo.Rows[(int)TransactType.Sales_Today]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, 0));
                    lblSalesMonth.Text = Convert.ToDecimal(dtInfo.Rows[(int)TransactType.Sales_Month]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, 0));
                    lblSalesYear.Text = Convert.ToDecimal(dtInfo.Rows[(int)TransactType.Sales_Year]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, 0));
                    lblSalesToday.Text += " / Rs." + Convert.ToDecimal(dtInfo.Rows[(int)TransactType.Sales_Amount_Today]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblSalesMonth.Text += " / Rs." + Convert.ToDecimal(dtInfo.Rows[(int)TransactType.Sales_Amount_Month]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblSalesYear.Text += " / Rs." + Convert.ToDecimal(dtInfo.Rows[(int)TransactType.Sales_Amount_Year]["Value"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblRecurringReminder.Text = dtInfo.Rows[(int)TransactType.Recurring_Reminder]["Value"].ToString();
                    lblAllReminder.Text = (dtSelectedReminder.Rows.Count + Convert.ToInt32(dtInfo.Rows[(int)TransactType.Recurring_Reminder]["Value"])).ToString();

                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        #endregion

        private void MDIMain_Activated(object sender, EventArgs e)
        {
            LoadTransactInfo();
        }
        private void compoundUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCompoundUnit frmCompoundUnit = new frmCompoundUnit();
            frmCompoundUnit.ShowDialog();
        }
      
        private void btnQRPurchaseInvoice_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("PURCHASE_INVOICE_VIEW", "Purchase Invoice"))
                return;

            try
            {
                fpinvoice = new frmPurchaseInvoice(this);
                fpinvoice.Show();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);

            }
        }

     
        private void masterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHosMasterSetup mastersetup = new frmHosMasterSetup();
            mastersetup.ShowDialog();
        }

        private void doctorRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoctorRegistration reg = new frmDoctorRegistration();
            reg.ShowDialog();
        }

        
        private void diseasesSetUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDiseasesSetUp setup = new frmDiseasesSetUp();
            setup.ShowDialog();

        }

        private void patientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPatientRegistration patient = new frmPatientRegistration();
            patient.ShowDialog();
        }
     

        private void employeeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (UserPermissionNotGranted("REPORTS_EMPLOYEE_REPORT", "view Employee Report"))
                return;
            HRM.View.Reports.frmEmployeeReportSettings ers = new HRM.View.Reports.frmEmployeeReportSettings();
            ers.MdiParent = this;
            ers.Show();
        }

        private void loanReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REPORTS_LOAN_REPORT", "view Employee Loan Report"))
               return;
            frmEmployeeLoanSettings els = new frmEmployeeLoanSettings();
            els.MdiParent = this;
            els.Show();
        }

        private void detailReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REPORTS_DETAILED_REPORT", "view Detailed Employee Report"))
                return;
            frmDetailEmployeeReportSettings frmDetailEmployeeReportSettings = new frmDetailEmployeeReportSettings();
            frmDetailEmployeeReportSettings.Show();
        }

        private void advanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("REPORTS_ADVANCE_REPORT", "view Employee Advance Report"))
                return;
            frmEmployeeAdvanceSettings frmEmployeeAdvanceSettings = new frmEmployeeAdvanceSettings();
            frmEmployeeAdvanceSettings.Show();
        }

        private void employeeRegistrationToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            if (UserPermissionNotGranted("HRM_VIEW", "view Employee Registration"))
               return;
            frmEmployeeRegistration employee = new frmEmployeeRegistration();
            employee.ShowDialog();
        }

        private void masterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmhrmmastersetup mastersetup = new frmhrmmastersetup();
            mastersetup.ShowDialog();
        }

       

        private void additionDeductionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmAddition add = new frmAddition();
            add.ShowDialog();
        }

        private void partTimeSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserPermissionNotGranted("HRM_PTSS_VIEW", "Part Time Salary Sheet"))
                  return;
            HRM.View.frmPTSalarySheet pt = new HRM.View.frmPTSalarySheet(this);
            pt.ShowDialog();

        }


        void payslip_FormClosed(object sender, FormClosedEventArgs e)
        {
           payslip = null;
            //throw new NotImplementedException();
        }
       frmPaySlip payslip;
        private void salarySheetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (UserPermissionNotGranted("HRM_SALARY_VIEW", "view Salary Sheet"))
                   return;
               //frmPaySlip payslip = new frmPaySlip();
              // payslip.ShowDialog();
                
                if (payslip == null)
                {
                    payslip = new frmPaySlip(this);
                    payslip.FormClosed += new FormClosedEventHandler(payslip_FormClosed);
                    payslip.ShowDialog();
                }
                else
                {
                    payslip.Activate();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);

            }
        }

        private void employeeAnnualReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployeeAnnualReportSettings frmEmployeeAnnualReportSettings = new frmEmployeeAnnualReportSettings();
            frmEmployeeAnnualReportSettings.ShowDialog();
        }

       

       
     
        private void patientReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPatientSetting patient = new frmPatientSetting();
            patient.ShowDialog();
        }
       


       
    }
}
