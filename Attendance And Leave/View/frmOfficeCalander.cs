using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.Leave;
using DateManager;
using System.Threading;
using BusinessLogic;
using Common;
using CrystalDecisions.Shared;
using Attendance_And_Leave.Reports;


namespace Attendance_And_Leave
{
    public partial class frmOfficeCalander : Form
    {
        private string FileName = "";
        private SourceGrid.Cells.Views.Cell CloseOffice;
        private SourceGrid.Cells.Views.Cell OpenOffice;
        SourceGrid.Cells.Controllers.CustomEvents evtStatusFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        Attendance_And_Leave.Model.dsOfficeCalender dsofficecalender = new Attendance_And_Leave.Model.dsOfficeCalender();
        int CurrRowPos;
        public frmOfficeCalander()
        {
            InitializeComponent();
        }

        private void frmOfficeCalander_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtyear;
            CloseOffice = new SourceGrid.Cells.Views.Cell();
            // CloseOffice.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Red);
            CloseOffice.ForeColor = Color.Red;

            OpenOffice = new SourceGrid.Cells.Views.Cell();
            // CloseOffice.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Red);
            // OpenOffice.ForeColor = Color.FromArgb(240, 240, 240);
            OpenOffice.ForeColor = Color.Black;
            evtStatusFocusLost.FocusLeft += new EventHandler(evtStatusFocusLost_FocusLeft);
            grdofficecalender.Redim(1, 6);
            AddGridHeader();
        }
        private void AddGridHeader()
        {
            grdofficecalender[0, 0] = new MyHeader("Date(BS)");
            grdofficecalender[0, 1] = new MyHeader("Date(AD)");
            grdofficecalender[0, 2] = new MyHeader("Days");
            grdofficecalender[0, 3] = new MyHeader("WHours");
            grdofficecalender[0, 4] = new MyHeader("Status");
            grdofficecalender[0, 5] = new MyHeader("Reason");

            grdofficecalender[0, 0].Column.Width = 80;
            grdofficecalender[0, 1].Column.Width = 80;
            grdofficecalender[0, 2].Column.Width = 100;
            grdofficecalender[0, 3].Column.Width = 80;
            grdofficecalender[0, 4].Column.Width = 80;
            grdofficecalender[0, 5].Column.Width = 150;
          
        }
        //Customized header
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }                

        private void btngenerate_Click(object sender, EventArgs e)
        {
            DataTable dtNepaliDate = officecalender.GetEndofmonth(Convert.ToInt32(txtyear.Text));
            FillCalender(dtNepaliDate);
            btnexcel.Enabled = true;
            btnsave.Enabled = true;
        }
        private void FillCalender(DataTable dtEndofNepDate)
        {
            int RowCount;
            string EnglishDate = "";
            string DayName = "";
           // int workhour = 0;
            DataRow drEndofMonth = dtEndofNepDate.Rows[0];
            frmProgress ProgressForm = new frmProgress();
            // Initialize the thread that will handle the background process
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {

                    ProgressForm.ShowDialog();
                }
            ));

            backgroundThread.Start();
            ProgressForm.UpdateProgress(10, "Initializing report...");
            int Baishak =Convert.ToInt32( drEndofMonth["Baisakh"].ToString());
            for (int i = 0; i < Baishak; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string firstmonth = "";
                if (i < 9)
                {
                   
                     firstmonth =  txtyear.Text+"/01/" + "0" + (i + 1);
                }
                else
                {
                    firstmonth = txtyear.Text + "/01/" + (i + 1);
                }
                try
                {

                    if (firstmonth.Length < 10 )
                    {
                        return;
                    }

                    string[] NepaliDate = firstmonth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));

                   
                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");
                   
                }
                catch (Exception ex)
                {
                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(firstmonth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
               
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                   
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                  
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;

                   
                }
                else
                {
                    if (rbtnclose.Checked && DayName=="Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
            
              
            }


            int jestha = Convert.ToInt32(drEndofMonth["Jestha"].ToString());
            for (int i = 0; i < jestha; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string secondmonth = "";
                if (i < 9)
                {
                    secondmonth = txtyear.Text + "/02/" + "0" + (i + 1);
                }
                else
                {
                    secondmonth = txtyear.Text + "/02/" + (i + 1);
                }
                try
                {

                    if (secondmonth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = secondmonth.Split('/');
                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));
                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {
                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(secondmonth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);

                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
              
            }
            ProgressForm.UpdateProgress(20, "Calculating Asar..............");
            int Asar = Convert.ToInt32(drEndofMonth["Asadh"].ToString());
            for (int i = 0; i < Asar; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string thirdmonth = "";
                if (i < 9)
                {
                    thirdmonth = txtyear.Text + "/03/" + "0" + (i + 1);
                }
                else
                {
                    thirdmonth = txtyear.Text + "/03/" + (i + 1);
                }
                try
                {
                    if (thirdmonth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = thirdmonth.Split('/');
                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));
                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {
                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(thirdmonth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
              
            }
            int Sharwan = Convert.ToInt32(drEndofMonth["Shrawan"].ToString());
            for (int i = 0; i < Sharwan; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string fourth = "";
                if (i < 9)
                {
                    fourth = txtyear.Text + "/04/" + "0" + (i + 1);
                }
                else
                {
                    fourth = txtyear.Text + "/04/" + (i + 1);
                }
                try
                {
                    if (fourth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = fourth.Split('/');
                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));
                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(fourth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
              
            }
            ProgressForm.UpdateProgress(40, "Calculating Bhadra..............");
            int Bhadra = Convert.ToInt32(drEndofMonth["Bhadra"].ToString());
            for (int i = 0; i < Bhadra; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string fifth = "";
                if (i < 9)
                {
                    fifth = txtyear.Text + "/05/" + "0" + (i + 1);
                }
                else
                {
                    fifth = txtyear.Text + "/05/" + (i + 1);
                }
                try
                {

                    if (fifth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = fifth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(fifth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
               
            }
            int Asoj = Convert.ToInt32(drEndofMonth["Aswin"].ToString());
            for (int i = 0; i < Asoj; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string sixth = "";
                if (i < 9)
                {
                    sixth = txtyear.Text + "/06/" + "0" + (i + 1);
                }
                else
                {
                    sixth = txtyear.Text + "/06/" + (i + 1);
                }
                try
                {

                    if (sixth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = sixth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(sixth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
              
            }
            ProgressForm.UpdateProgress(60, "Calculating Kartik..............");
            int Kartik = Convert.ToInt32(drEndofMonth["Kartik"].ToString());
            for (int i = 0; i < Kartik; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string seventh = "";
                if (i < 9)
                {
                    seventh = txtyear.Text + "/07/" + "0" + (i + 1);
                }
                else
                {
                    seventh = txtyear.Text + "/07/" + (i + 1);
                }
                try
                {

                    if (seventh.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = seventh.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(seventh);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
               
            }
            int Mansir = Convert.ToInt32(drEndofMonth["Mangsir"].ToString());
            for (int i = 0; i < Mansir; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string eightth = "";
                if (i < 9)
                {
                    eightth = txtyear.Text + "/08/" + "0" + (i + 1);
                }
                else
                {
                    eightth = txtyear.Text + "/08/" + (i + 1);
                }
                try
                {

                    if (eightth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = eightth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(eightth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
                
            }
            int Poush = Convert.ToInt32(drEndofMonth["Poush"].ToString());
            for (int i = 0; i < Poush; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string ninth = "";
                if (i < 9)
                {
                    ninth = txtyear.Text + "/09/" + "0" + (i + 1);
                }
                else
                {
                    ninth = txtyear.Text + "/09/" + (i + 1);
                }
                try
                {

                    if (ninth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = ninth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));
                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {
                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(ninth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
               
            }
            ProgressForm.UpdateProgress(80, "Calculating Magh..............");
            int Magh = Convert.ToInt32(drEndofMonth["Magh"].ToString());
            for (int i = 0; i < Magh; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string tenth = "";
                if (i < 9)
                {
                    tenth = txtyear.Text + "/10/" + "0" + (i + 1);
                }
                else
                {
                    tenth = txtyear.Text + "/10/" + (i + 1);
                }
                try
                {

                    if (tenth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = tenth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(tenth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
               
            }
            int Falgun = Convert.ToInt32(drEndofMonth["Falgun"].ToString());
            for (int i = 0; i < Falgun; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string eleventh = "";
                if (i < 9)
                {
                    eleventh = txtyear.Text + "/11/" + "0" + (i + 1);
                }
                else
                {
                    eleventh = txtyear.Text + "/11/" + (i + 1);
                }
                try
                {

                    if (eleventh.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = eleventh.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(eleventh);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
               
            }
            ProgressForm.UpdateProgress(100, "Preparing Office Calender...");
            int Chaitra = Convert.ToInt32(drEndofMonth["Chaitra"].ToString());
            for (int i = 0; i < Chaitra; i++)
            {
                RowCount = grdofficecalender.RowsCount;
                grdofficecalender.Rows.Insert(RowCount);
                string twelveth = "";
                if (i < 9)
                {
                    twelveth = txtyear.Text + "/12/" + "0" + (i + 1);
                }
                else
                {
                    twelveth = txtyear.Text + "/12/" + (i + 1);
                }
                try
                {

                    if (twelveth.Length < 10)
                    {
                        return;
                    }

                    string[] NepaliDate = twelveth.Split('/');

                    DateTime EngDate = Date.NepToEng(Convert.ToInt16(NepaliDate[0]), Convert.ToInt16(NepaliDate[1]), Convert.ToInt16(NepaliDate[2]));


                    EnglishDate = EngDate.Year.ToString().PadLeft(4, '0') + "/" + EngDate.Month.ToString().PadLeft(2, '0') + "/" + EngDate.Day.ToString().PadLeft(2, '0');

                    ////Write 
                    // DayName = EngDate.ToString("dddd") + ", " + Date.GetNepaliMonthInText(Convert.ToInt16(NepaliDate[1]), Language.LanguageType.English) + " " + Convert.ToInt16(NepaliDate[2]) + ", " + Convert.ToInt16(NepaliDate[0]);
                    DayName = EngDate.ToString("dddd");

                }
                catch (Exception ex)
                {


                    //DO NOTHING
                }
                grdofficecalender[RowCount, 0] = new SourceGrid.Cells.Cell(twelveth);
                grdofficecalender[RowCount, 1] = new SourceGrid.Cells.Cell(EnglishDate);
                grdofficecalender[RowCount, 2] = new SourceGrid.Cells.Cell(DayName);
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                grdofficecalender[RowCount, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                if (DayName == "Saturday")
                {
                    grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                    SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    txtStatus.StandardValues = new string[] { "C", "O" };
                    txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                    txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                    string status = "C";
                    grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                    grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    grdofficecalender[RowCount, 0].View = CloseOffice;
                    grdofficecalender[RowCount, 1].View = CloseOffice;
                    grdofficecalender[RowCount, 2].View = CloseOffice;
                    grdofficecalender[RowCount, 3].View = CloseOffice;
                    grdofficecalender[RowCount, 4].View = CloseOffice;
                    grdofficecalender[RowCount, 5].View = CloseOffice;
                }
                else
                {
                    if (rbtnclose.Checked && DayName == "Sunday")
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(0);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "C";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                        grdofficecalender[RowCount, 0].View = CloseOffice;
                        grdofficecalender[RowCount, 1].View = CloseOffice;
                        grdofficecalender[RowCount, 2].View = CloseOffice;
                        grdofficecalender[RowCount, 3].View = CloseOffice;
                        grdofficecalender[RowCount, 4].View = CloseOffice;
                        grdofficecalender[RowCount, 5].View = CloseOffice;
                    }
                    else
                    {
                        grdofficecalender[RowCount, 3] = new SourceGrid.Cells.Cell(9);
                        SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                        txtStatus.StandardValues = new string[] { "C", "O" };
                        txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                        txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                        string status = "O";
                        grdofficecalender[RowCount, 4] = new SourceGrid.Cells.Cell(status, txtStatus);
                        grdofficecalender[RowCount, 4].AddController(evtStatusFocusLost);
                    }
                }
               
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            }

        }
        private void evtStatusFocusLost_FocusLeft(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            if (grdofficecalender[CurrRowPos, 4].Value.ToString() == "O" || grdofficecalender[CurrRowPos, 4].Value.ToString() == "o")
            {
               // MessageBox.Show("Open");
                grdofficecalender[CurrRowPos, 3].Value = 9;
                grdofficecalender[CurrRowPos, 0].View = OpenOffice;
                grdofficecalender[CurrRowPos, 1].View = OpenOffice;
                grdofficecalender[CurrRowPos, 2].View = OpenOffice;
                grdofficecalender[CurrRowPos, 3].View = OpenOffice;
                grdofficecalender[CurrRowPos, 4].View = OpenOffice;
                grdofficecalender[CurrRowPos, 5].View = OpenOffice;
            }
            else if (grdofficecalender[CurrRowPos, 4].Value.ToString() == "C" || grdofficecalender[CurrRowPos, 4].Value.ToString() == "c")
            {
               // MessageBox.Show("Close");
                grdofficecalender[CurrRowPos, 3].Value = 0;
                grdofficecalender[CurrRowPos, 0].View = CloseOffice;
                grdofficecalender[CurrRowPos, 1].View = CloseOffice;
                grdofficecalender[CurrRowPos, 2].View = CloseOffice;
                grdofficecalender[CurrRowPos, 3].View = CloseOffice;
                grdofficecalender[CurrRowPos, 4].View = CloseOffice;
                grdofficecalender[CurrRowPos, 5].View = CloseOffice;
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            DataTable dtcalenderlist = new DataTable();
            dtcalenderlist.Columns.Add("DateBS");
            dtcalenderlist.Columns.Add("DateAD");
            dtcalenderlist.Columns.Add("Day");
            dtcalenderlist.Columns.Add("WorkHour");
            dtcalenderlist.Columns.Add("Status");
            dtcalenderlist.Columns.Add("Reason");
            for (int i = 0; i < grdofficecalender.Rows.Count - 1; i++)
            {
                dtcalenderlist.Rows.Add(grdofficecalender[i+1,0].Value,grdofficecalender[i+1,1].Value,grdofficecalender[i+1,2].Value,grdofficecalender[i+1,3].Value,grdofficecalender[i+1,4].Value,grdofficecalender[i+1,5].Value);
            }
            officecalender.createofficeCalender(dtcalenderlist);
            MessageBox.Show("Office Calander Created Successfully");
        }

        private void btnexcel_Click(object sender, EventArgs e)
        {
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
            ProgressForm.UpdateProgress(20, "Initializing ......");

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

         
               dsofficecalender.Clear();

               
                rptofficecalender rpt = new rptofficecalender();

                //Fill the logo on the report
                Misc.WriteLogo(dsofficecalender, "tblImage");
                rpt.SetDataSource(dsofficecalender);
            
                //Fill The Data in Dataset
                for (int i = 0; i < grdofficecalender.Rows.Count - 1; i++)
                {
                    dsofficecalender.Tables["tblofficecalender"].Rows.Add(grdofficecalender[i + 1, 0].Value, grdofficecalender[i + 1, 1].Value, grdofficecalender[i + 1, 2].Value, grdofficecalender[i + 1, 3].Value, grdofficecalender[i + 1, 4].Value, grdofficecalender[i + 1, 5].Value);
                }
              
                //Provide values to the parameters on the report
                CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
              
                //Update the progressbar
                ProgressForm.UpdateProgress(50, "Initializing To Send in Excel...");

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


                pdvReport_Head.Value = "Office Calender";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);


                pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);


             
                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

              
                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing In Excel...");

                // Close the dialog
                ProgressForm.CloseForm();
              
               ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
               CrExportOptions = rpt.ExportOptions;
               CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
               CrExportOptions.ExportFormatType = ExportFormatType.Excel;
               CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
               CrExportOptions.FormatOptions = CrFormatTypeOptions;
               rpt.Export();
               rpt.Close();
                      
            
        }

    }
}
