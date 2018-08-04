using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using RegistryManager;
using Common;


namespace AccSwift
{
    public interface IfrmCompanyFiscalYear
    {
        void FiscalYearCalculate(CompanyDetails CompDetails);
    }

    public partial class frmCompanyFiscalYear : Form, IfrmDateConverter
    {
        private IfrmCompanyFiscalYear m_ParentForm;
        CompanyDetails CmpDetails = new CompanyDetails();
        private bool isStartDate = false;

        #region Globals

        internal const int SC_CLOSE = 0xF060;           //close button's code in windows api
        internal const int MF_GRAYED = 0x1;             //disabled button status (enabled = false)
        internal const int MF_ENABLED = 0x00000000;     //enabled button status
        internal const int MF_DISABLED = 0x00000002;    //disabled button status

        [DllImport("user32.dll")] //Importing user32.dll for calling required function
        private static extern IntPtr GetSystemMenu(IntPtr HWNDValue, bool Revert);

        /// HWND: An IntPtr typed handler of the related form
        /// It is used from the Win API "user32.dll"

        [DllImport("user32.dll")] //Importing user32.dll for calling required function again
        private static extern int EnableMenuItem(IntPtr tMenu, int targetItem, int targetStatus);

        #endregion

        public frmCompanyFiscalYear()
        {
            InitializeComponent();           
        }
        public frmCompanyFiscalYear(Form ParentForm, CompanyDetails CompDetails)
        {
            InitializeComponent();
            m_ParentForm = (IfrmCompanyFiscalYear)ParentForm;
            CmpDetails = CompDetails;
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (isStartDate)
                txtDateFYStart.Text = Date.ToSystem(DotNetDate);
            if (!isStartDate)
                txtDateBookBegin.Text = Date.ToSystem(DotNetDate);
        }


        public void EnableCloseButton() //A standard void function to invoke EnableMenuItem()
        {
            EnableMenuItem(GetSystemMenu(this.Handle, false), SC_CLOSE, MF_ENABLED);
        }

        public void DisableCloseButton() //A standard void function to invoke EnableMenuItem()
        {
            EnableMenuItem(GetSystemMenu(this.Handle, false), SC_CLOSE, MF_GRAYED);
        }

        private void frmCompanyFiscalYear_Load(object sender, EventArgs e)
        {
           // EnableCloseButton(); //Invoking main void method of the EnableMenuItem()
            DisableCloseButton();//Invoking main void method of the DisableMenuItem()

            txtDateFYStart.Mask = Date.FormatToMask();
            txtDateFYStart.Text = Date.ToSystem(Date.GetServerDate());
            txtDateBookBegin.Mask = Date.FormatToMask();
            txtDateBookBegin.Text = Date.ToSystem(Date.GetServerDate()); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CmpDetails.FYFrom = Date.ToDotNet(txtDateFYStart.Text);
            CmpDetails.BookBeginFrom = Date.ToDotNet(txtDateBookBegin.Text);
            m_ParentForm.FiscalYearCalculate(CmpDetails);
            this.Close();
        }

        private void btnDateStartFY_Click(object sender, EventArgs e)
        {
            isStartDate = true;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDateFYStart.Text));
            frm.ShowDialog();
        }

        private void btnDateBookBegin_Click(object sender, EventArgs e)
        {
            isStartDate = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDateBookBegin.Text));
            frm.ShowDialog();
        }

       
    }
}
