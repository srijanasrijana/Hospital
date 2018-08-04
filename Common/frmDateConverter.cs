using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DateManager;
using Common;

namespace Common
{
    public interface IfrmDateConverter
    {
        void DateConvert(DateTime ReturnDotNetDate);
        //void ConvertedDate(DateTime Converted);
    }
    public partial class frmDateConverter : Form
    {
        private IfrmDateConverter m_ParentForm;
        public DateTime strDate;
         public string result { get; set; }
        bool PopUp = false;

        public frmDateConverter()
        {
            InitializeComponent();
            btnConInsert.Text = "&OK";
        }
       // frmDateConverter frmdate = new frmDateConverter();

        //public frmDateConverter(Form frm)
        //{
        //    m_ParentForm = (IfrmDateConverter)frm;
        //    //PopUp = true;
        //    InitializeComponent();
        //}

      
        public frmDateConverter(Form ParentForm, DateTime ReturnDotNetDate)
        {
            InitializeComponent();
            btnConInsert.Text = "&Insert";
            PopUp = true;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;

            m_ParentForm = (IfrmDateConverter)ParentForm;

            txtEnglishDate.Text = ReturnDotNetDate.ToString("yyyy/MM/dd");
        
        }


        private void txtEnglishDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEnglishDate.Text.Length < 10 && txtEnglishDate.Focused)
                {
                    txtNepaliDate.Clear();
                    lblNepaliDate.Text = "";
                    lblEnglishDate.Text = "";
                    return;
                }

                string[] EnglishDate = txtEnglishDate.Text.Split('/','-');
                DateTime dtEnglish = new DateTime(Convert.ToInt16(EnglishDate[0]), Convert.ToInt16(EnglishDate[1]), Convert.ToInt16(EnglishDate[2]));
                int retYear = 0;
                int retMonth = 0;
                int retDay = 0;
               

                Date.EngToNep(dtEnglish, ref retYear, ref retMonth, ref retDay);
                txtNepaliDate.Text = retYear.ToString().PadLeft(4, '0') + "/" + retMonth.ToString().PadLeft(2, '0') + "/" + retDay.ToString().PadLeft(2, '0');
                lblEnglishDate.Text = dtEnglish.ToString("dddd, MMMM dd, yyyy");
            }
            catch
            {
                //DO NOTHING
            }


        }

        private void frmDateConverter_Load(object sender, EventArgs e)
        {
            //txtEnglishDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            //Date.DefaultDate = Date.DateType.English;
            //Date.DefaultFormat = Date.DateFormat.MM_DD_YYYY;
            ////Date.DateFormat df = Date.DefaultFormat;

        }

        private void txtEnglishDate_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtNepaliDate_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (txtNepaliDate.Text.Length < 10 && txtNepaliDate.Focused)
                {
                    txtEnglishDate.Clear();
                    lblNepaliDate.Text = "";
                    lblEnglishDate.Text = "";
                    return;
                }

                string[] NepaliDate = txtNepaliDate.Text.Split('/','-');
                
                DateTime EngDate=Date.NepToEng(Convert.ToInt16(NepaliDate[0]),Convert.ToInt16(NepaliDate[1]),Convert.ToInt16(NepaliDate[2]));

                txtEnglishDate.Text = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                //Write 
                lblNepaliDate.Text = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                //DO NOTHING
            }

        }

        private void btnConInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (PopUp) //This is a popup window
                {
                    string[] SplitEng =txtEnglishDate. Text.Split('/', '-');
                    m_ParentForm.DateConvert(new DateTime(Convert.ToInt16(SplitEng[0]), Convert.ToInt16(SplitEng[1]), Convert.ToInt16(SplitEng[2])));
                    result=SplitEng[0]+"/"+SplitEng[1]+"/"+ SplitEng[2];
                }
                
                this.Dispose();
            }
            catch
            {
                MessageBox.Show("Could not process date. Invalid date found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void txtNepaliDate_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void frmDateConverter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }


    }
}
