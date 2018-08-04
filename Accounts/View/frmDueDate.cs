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
using BusinessLogic.Accounting;

namespace Accounts
{
    public interface IDueDate
    {
        void AddLedgerDueDate(DataTable dtDue);
    }
    public partial class frmDueDate : Form,IfrmDateConverter
    {
        private IDueDate m_Parent;
        SourceGrid.Cells.Controllers.CustomEvents evtDueDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        DataTable dtLedgerList;
        int Sno = 1;
        int RowID = 0;
        SourceGrid.CellContext ctx2 = new SourceGrid.CellContext();
        string dueDate = "";
        string VoucherType = "";
        public frmDueDate()
        {
            InitializeComponent();
        }
        public frmDueDate(Form parentForm,DataTable dt)
        {
            m_Parent = (IDueDate)parentForm;
            dtLedgerList = dt;
            InitializeComponent();
        }
        public frmDueDate(Form parentForm, DataTable dt, int rowID, string voucherType)
        {
            m_Parent = (IDueDate)parentForm;
            dtLedgerList = dt;
            this.RowID = rowID;
            this.VoucherType = voucherType;
            InitializeComponent();
        }
        private void frmDueDate_Load(object sender, EventArgs e)
        {
            evtDueDateFocusLost.Click += new EventHandler(dueDate_click);
            fillGrid();
        }
        private void fillGrid()
        {
            try
            {
                grdDueDate.Rows.Clear();
                grdDueDate.Redim(dtLedgerList.Rows.Count + 1, 3);
                AddGridHeader(); ;
                for (int i = 1; i <= dtLedgerList.Rows.Count; i++)
                {
                    DataRow dr = dtLedgerList.Rows[i - 1];
                    grdDueDate[i, 0] = new SourceGrid.Cells.Cell(dr["LedgerName"].ToString());

                    grdDueDate[i, 2] = new SourceGrid.Cells.Cell(dr["LedgerID"].ToString());
                    if (RowID > 0)
                    {
                        dueDate = DebtorDueDate.GetDueDate(RowID, Convert.ToInt32(dr["LedgerID"].ToString()), VoucherType);
                    }
                    else if (VoucherType == "OPBAL")
                    {
                        dueDate = DebtorDueDate.GetDueDate(Convert.ToInt32(dr["LedgerID"].ToString()), VoucherType);
                    }
                    if (dueDate == "" || dueDate == null)
                        dueDate = Date.ToSystem(DateTime.Today).ToString();
                    SourceGrid.Cells.Button btnDueDate = new SourceGrid.Cells.Button(dueDate); //Date.ToSystem(DateTime.Today)
                    //txtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
                    grdDueDate[i, 1] = btnDueDate;
                    grdDueDate[i, 1].AddController(evtDueDateFocusLost);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dueDate_click(object sender,EventArgs e)
        {
            ctx2 = (SourceGrid.CellContext)sender;
           // string dueDate = ctx2.Value.ToString();
            string due = ctx2.DisplayText.ToString();
            if (ctx2.DisplayText.ToString() != "")
            {
                frmDateConverter fr = new frmDateConverter(this, Date.ToDotNet(due));
                fr.ShowDialog();
            }
            else
            {
                frmDateConverter fr = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Date.GetServerDate())));
                fr.ShowDialog();
            }
        }

        private void AddGridHeader()
        {
            grdDueDate[0, 0] = new MyHeader("Debtors Name");
            grdDueDate[0, 1] = new MyHeader("Due Date");
            grdDueDate[0, 2] = new MyHeader("LedgerID");

            grdDueDate[0, 0].Column.Width = 200;
            grdDueDate[0, 1].Column.Width = 100;
            grdDueDate[0, 2].Column.Width = 1;

            grdDueDate[0, 2].Column.Visible = false;
        }
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
        public void DateConvert(DateTime DotNetDate)
        {
               ctx2.Value = Date.ToSystem(DotNetDate);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("DueDate",typeof(string));
            dt2.Columns.Add("LedgerID", typeof(string));

            for (int i = 1; i < grdDueDate.Rows.Count; i++)
            {
                if (grdDueDate[i, 1].Value == null || grdDueDate[i, 1].Value == " ")
                {
                    MessageBox.Show("Please Insert Due Date first", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                dt2.Rows.Add(Date.ToDotNet(grdDueDate[i, 1].Value.ToString()), grdDueDate[i, 2].Value.ToString());

            }
           
            //for (int i = 0; i < grdDueDate.Rows.Count-1; i++)
            //{
            //    if (dt2.Rows.Count <= 0)
            //    {
            //        MessageBox.Show("Please Enter the Due Date ");
            //    }
            //    else 

            //    dt2.Rows.Add(Date.ToDotNet( grdDueDate[i + 1, 1].Value.ToString()), grdDueDate[i + 1, 2].Value);
            //}
            m_Parent.AddLedgerDueDate(dt2);
            this.Dispose();
            
        }
    }
}
