using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using Inventory.DataSet;
using Inventory.CrystalReports;
using DateManager;
using ErrorManager;
using System.IO;

namespace Inventory.Forms.Accounting
{
    public partial class frmGroupVoucherList : Form
    {
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private DataTable dTable;
        private DataRow[] drFound;
        private string FilterString = "";
        private bool isNew;
        private int Sid;
        private int PID;
        private int Rowid;


        DataTable dtbulkposting;
        public frmGroupVoucherList()
        {
            InitializeComponent();
        }
        public frmGroupVoucherList(DataTable dt,int seriesid,int projectid,int rowid)
        {
            dtbulkposting = dt;
            Sid = seriesid;
            PID = projectid;
            Rowid = rowid;
            InitializeComponent();
        }

        private void frmGroupVoucherList_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            evtDelete.Click += new EventHandler(Delete_Row_Click);
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(BulkVouchergrid_DoubleClick);
            //BusinessLogic.Accounting.BulkPosting bp = new BusinessLogic.Accounting.BulkPosting();
            //dTable = bp.GetBulkMasterinfo();
            //drFound = dTable.Select(FilterString);
            //grdbulkvoucher.Redim(2, 3);
            //FillGrid();
            Callevents();
           
        }
        private void AddGridHeading()
        {
            grdbulkvoucher[0, 0] = new MyHeader("Delete");
            grdbulkvoucher[0, 1] = new MyHeader("ID");
            grdbulkvoucher[0, 2] = new MyHeader("Category Name");
            grdbulkvoucher[0, 0].Column.Width = 50;
            grdbulkvoucher[0, 1].Column.Width = 40;
            grdbulkvoucher[0, 2].Column.Width = 100;
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
        public void FillGrid()
        {
            try
            {
                grdbulkvoucher.Rows.Clear();
                grdbulkvoucher.Redim(drFound.Count() + 1, 3);
                AddGridHeading();

                for (int i = 1; i <= drFound.Count(); i++)
                {
                    SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                    btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                    //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    grdbulkvoucher[i, 0] = btnDelete;
                    grdbulkvoucher[i, 0].AddController(evtDelete);
                    DataRow dr = drFound[i - 1];

                    //grdbulkvoucher[i, 0] = new SourceGrid.Cells.Cell(i.ToString());

                    grdbulkvoucher[i, 1] = new SourceGrid.Cells.Cell(dr["id"].ToString());
                    grdbulkvoucher[i, 1].AddController(dblClick);


                    grdbulkvoucher[i, 2] = new SourceGrid.Cells.Cell(dr["name"].ToString());
                    grdbulkvoucher[i, 2].AddController(dblClick);
                   // grdbulkvoucher[i, 2].AddController(gridKeyDown);
                }


            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            tabControl1.SelectedIndex = 1;
            //tabControl1.SelectedTab = tabPagenew;
            //tabControl1.SelectedTab.
            txtcategory.Text = "";
            txtid.Text = "";
            txtcategory.Focus();

        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            int CurRow = grdbulkvoucher.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext Bulkvoucherid = new SourceGrid.CellContext(grdbulkvoucher, new SourceGrid.Position(CurRow, 1));
            string vid = Bulkvoucherid.Value.ToString();
            BusinessLogic.Accounting.BulkPosting bp1 = new BusinessLogic.Accounting.BulkPosting();
            bool rvalue = bp1.deletebulkvoucherrow(Convert.ToInt32(vid));
            if (rvalue == true)
            {
                Global.Msg("Deleted Successfully");
            }
            else
            {
                Global.Msg("Fail To Delete");
            }
           // frmGroupVoucherList_Load(sender, e);
            Callevents();
           
        }

        private void grdbulkvoucher_DoubleClick(object sender, EventArgs e)
        {
        }
        private void BulkVouchergrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                isNew = false;
                tabControl1.SelectedIndex = 1;
               
                //Get the Selected Row
                int CurRow = grdbulkvoucher.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext Bulkvoucherid = new SourceGrid.CellContext(grdbulkvoucher, new SourceGrid.Position(CurRow, 1));
                SourceGrid.CellContext Bulkvouchername = new SourceGrid.CellContext(grdbulkvoucher, new SourceGrid.Position(CurRow, 2));
                txtcategory.Text =Bulkvouchername.Value.ToString();
                txtid.Text = Bulkvoucherid.Value.ToString();
                txtcategory.Focus();
           
            }
            catch (Exception ex)
            {
                Global.Msg("Invalid selection");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BusinessLogic.Accounting.BulkPosting bp = new BusinessLogic.Accounting.BulkPosting();
            if (isNew == true)//insert for new
            {
                
               string returnvalue= bp.insertbulkvoucher(txtcategory.Text);
                txtcategory.Text = "";
                if (returnvalue == "SUCCESS")
                {
                    MessageBox.Show("Inserted Successfully");
                }
                else
                    MessageBox.Show("Fail to insert");
                    tabControl1.SelectedIndex = 0;

            }
            else//update the existing voucher category
            {
               string rvalue= bp.updatebulkvoucher(txtcategory.Text, Convert.ToInt32(txtid.Text));
                txtcategory.Text = "";
                txtid.Text = "";
                if (rvalue == "SUCCESS")
                {
                    Global.Msg("Updated Successfully");
                }
                else
                    MessageBox.Show("Fail to Update");
                
                tabControl1.SelectedIndex = 0;
            }
            frmGroupVoucherList_Load(sender, e);
        }

        private void Callevents()
        {
            BusinessLogic.Accounting.BulkPosting bp = new BusinessLogic.Accounting.BulkPosting();
            dTable = bp.GetBulkMasterinfo();
            drFound = dTable.Select(FilterString);
            grdbulkvoucher.Redim(2, 3);
            FillGrid();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (tabControl1.SelectedTab == tabPagelist)
            //{
            //    tabControl1.SelectedTab = tabPagenew;
            //}
            //if (tabControl1.SelectedTab == tabPagenew)
            //{
            //    tabControl1.SelectedTab = tabPagelist;
            //}
            

            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 0;

            //}
            //else
            //{
            //    tabControl1.SelectedIndex = 1;
            //}

        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 1;
            //}
            //else
            //{
            //    tabControl1.SelectedIndex = 0;
               
            //}
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (this.ActiveControl != grdbulkvoucher)
            //{
            //    MessageBox.Show("Select the Posting Type");
            //    return;
            //}
            int CurRow = grdbulkvoucher.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext Bulkvoucherid = new SourceGrid.CellContext(grdbulkvoucher, new SourceGrid.Position(CurRow, 1));
            string vidd = Bulkvoucherid.Value.ToString();
            BusinessLogic.Accounting.BulkPosting bulkpostingdetail = new BusinessLogic.Accounting.BulkPosting();
            bulkpostingdetail.insertintobulkpostingdetail(dtbulkposting,Convert.ToInt32(vidd),Sid,PID,Rowid);
            Global.Msg("Inserted Successfully");
        }

        private void tabControl1_TabStopChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 1;
            //}
            //else
            //{
            //    tabControl1.SelectedIndex = 0;

            //}
           
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 1;
            //}
            //else
            //{
            //    tabControl1.SelectedIndex = 0;

            //}
        }

        private void tabPagelist_Click(object sender, EventArgs e)
        {
            //tabControl1.SelectedIndex = 1;
        }

        private void tabPagelist_Enter(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 1;
            //}
            //else
            //    tabControl1.SelectedIndex = 0;
        }

        private void tabPagelist_MouseClick(object sender, MouseEventArgs e)
        {
            //tabControl1.SelectedIndex = 1;
        }

        private void tabPagenew_Enter(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 1;
            //}
            //else
            //    tabControl1.SelectedIndex = 0;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    tabControl1.SelectedIndex = 1;
            //}
            //else
            //    tabControl1.SelectedIndex = 0;
        }
    }
}
