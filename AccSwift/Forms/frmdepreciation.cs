using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using ErrorManager;
using System.IO;
using Inventory.CrystalReports;
using Inventory.DataSet;
using System.Data.SqlClient;

namespace AccSwift.Forms
{
    public partial class frmdepreciation : Form
    {
        DataTable depreciation = new DataTable();
        frmfiscalyearclosing m_frmfiscalyearclosing = null;
        public frmdepreciation()
        {
            InitializeComponent();
        }
        public frmdepreciation(frmfiscalyearclosing frmfiscalyearclosing)
        {
            InitializeComponent();
            m_frmfiscalyearclosing = frmfiscalyearclosing;
        }
        private void frmdepreciation_Load(object sender, EventArgs e)
        {
            depreciation.Columns.Add("LedgerID");
            depreciation.Columns.Add("LedgerName");
            depreciation.Columns.Add("Depreciation");
            ShowDepreciation();
        }

        private void ShowDepreciation()
        {
            DataTable dt = new DataTable();

            //int Liabilities = AccountGroup.GetGroupIDFromGroupNumber(2);
            //dtLiabilitiesLedgers = Ledger.GetAllLedger(Liabilities);
            int FixedAssest = AccountGroup.GetGroupIDFromGroupNumber(13);
            dt = Ledger.GetAllLedger1(FixedAssest);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                depreciation.Rows.Add(dr["LedgerID"].ToString(), dr["EngName"].ToString(), dr["Depreciatition"].ToString());
            }

            FillGrid(depreciation);
           
            int count = grddepreciation.Rows.Count;
        }
        private void FillGrid(DataTable dt)
        {
            try
            {
                grddepreciation.Rows.Clear();
                grddepreciation.Redim(dt.Rows.Count+1,3);
                ShowHeader();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = depreciation.Rows[i];
                    //grddepreciation.Rows.Insert(i+1);

                    SourceGrid.Cells.Editors.TextBox txtledgerid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txtledgerid.EditableMode = SourceGrid.EditableMode.None;
                    grddepreciation[i + 1, 0] = new SourceGrid.Cells.Cell("",txtledgerid);
                    grddepreciation[i + 1, 0].Value = dr["LedgerID"].ToString();

                    SourceGrid.Cells.Editors.TextBox txtledgername = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txtledgername.EditableMode = SourceGrid.EditableMode.None;
                    grddepreciation[i + 1, 1] = new SourceGrid.Cells.Cell("",txtledgername);
                    grddepreciation[i + 1, 1].Value = dr["LedgerName"].ToString();

                    SourceGrid.Cells.Editors.TextBox txtDepreciation = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txtDepreciation.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grddepreciation[i + 1,2] = new SourceGrid.Cells.Cell("",txtDepreciation);
                    grddepreciation[i + 1, 2].Value = dr["Depreciation"].ToString();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void ShowHeader()
        {
            grddepreciation[0, 0] = new MyHeader("LedgerID");
            grddepreciation[0, 1] = new MyHeader("LedgerName");
            grddepreciation[0, 2] = new MyHeader("Depreciation %");
            grddepreciation[0, 0].Column.Width = 50;
            grddepreciation[0, 1].Column.Width = 150;
            grddepreciation[0, 2].Column.Width = 150;
            grddepreciation[0, 0].Column.Visible = false;
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

        private void btnsavedepreciation_Click(object sender, EventArgs e)
        {
            DataTable dtsavedepreciation = new DataTable();
            dtsavedepreciation.Columns.Add("LedgerID");
            dtsavedepreciation.Columns.Add("LedgerName");
            dtsavedepreciation.Columns.Add("DepreciationValue");
            for (int i = 0; i < grddepreciation.Rows.Count - 1;i++ )
            {
                if (!Misc.IsNumeric(grddepreciation[i + 1, 2].Value))
                {
                    Global.MsgError("!Enter Numeric Value Only");
                       return;
                }
                else
                {
                dtsavedepreciation.Rows.Add(grddepreciation[i + 1, 0].Value, grddepreciation[i + 1, 1].Value, grddepreciation[i + 1, 2].Value);
                }
            }
            BusinessLogic.Depreciation dep = new Depreciation();
            dep.SaveDepreciation(dtsavedepreciation);
            
            // send the depreciation table to frmfiscalyearclosing
            if (m_frmfiscalyearclosing != null)
            {
                m_frmfiscalyearclosing.GetDepreciation(dtsavedepreciation);
            }

            //if(Global.fclose=true)
            //{
                //frmfiscalyearclosing frm = new frmfiscalyearclosing();
                //frm.ShowDialog();
                this.Close();
            //}

        }
    }
}
