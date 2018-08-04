using BusinessLogic;
using BusinessLogic.Accounting;
using Common;
using DateManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Accounts.View
{
    public partial class frmBudget : Form, IfrmDateConverter
    {
        private bool IsStartDate=false;
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        string gstdate="";
        string genddate="";
        DateTime de;
        int id=0;
        Budget bgt = new Budget();
        //declare custom event
        SourceGrid.Cells.Controllers.CustomEvents rowSelect = new SourceGrid.Cells.Controllers.CustomEvents();

        public frmBudget()
        {
            InitializeComponent();
        }

        private void frmBudget_Load(object sender, EventArgs e)
        {
            txtStartDate.Mask = Date.FormatToMask();
            txtStartDate.Text = Date.ToSystem(Date.GetServerDate());

            txtEndDate.Mask = Date.FormatToMask();
            txtEndDate.Text = Date.ToSystem(Date.GetServerDate());
            enableDisable(false,true, true, false, true, false, true);
            createGridColumnHeader();
            FillGridWithData();

        }
        public void DateConvert(DateTime DotNetDate)
        {
            if (IsStartDate)
                txtStartDate.Text = Date.ToSystem(DotNetDate);
            if (!IsStartDate)
                txtEndDate.Text = Date.ToSystem(DotNetDate);
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            try
            {               
                Date.ToSystem(Date.GetServerDate());
                IsStartDate = true;
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtStartDate.Text));
                frm.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Date is not in correct format."+ex.Message);
            }
        }

        private void btnEndDate_Click(object sender, EventArgs e)
        {
            try
            {
                IsStartDate = false;
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtEndDate.Text));
                frm.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Date is not in correct format."+ex.Message);
            }           
        }

        private void enableDisable(bool entrygroupbox,bool bnew,bool bedit,bool bsave,bool bdelete,bool bclear,bool grid)
        {
            groupBox1.Enabled = entrygroupbox;
            btnNew.Enabled = bnew;
            btnEdit.Enabled = bedit;
            btnSave.Enabled = bsave;
            btnDelete.Enabled = bdelete;
            btnCancel.Enabled = bclear;
            grdShowBudget.Enabled = grid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBudgetName.Text = "";
            txtDescription.Text = "";           
            txtStartDate.Mask = Date.FormatToMask();
            txtStartDate.Text = Date.ToSystem(Date.GetServerDate());

            txtEndDate.Mask = Date.FormatToMask();
            txtEndDate.Text = Date.ToSystem(Date.GetServerDate());
            m_mode = EntryMode.NORMAL;
            id = 0;
            enableDisable(false, true, true, false, true, false, true);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            enableDisable(true, false, false, true, false, true, false);
            m_mode = EntryMode.NEW;
            txtBudgetName.Text = "";
            txtDescription.Text = "";
            txtStartDate.Mask = Date.FormatToMask();
            txtStartDate.Text = Date.ToSystem(Date.GetServerDate());

            txtEndDate.Mask = Date.FormatToMask();
            txtEndDate.Text = Date.ToSystem(Date.GetServerDate());
        }

        private void btnEdit_Click(object sender, EventArgs e)        
        {
            try
            {
                if (grdShowBudget.Rows.Count <= 1)
                {
                    MessageBox.Show("No budget id avilable to edit.");
                    return;
                }
                if (txtBudgetName.Text == "")
                {
                    MessageBox.Show("No budget is selected.");
                    return;
                }
                if (id < 1)
                {
                    MessageBox.Show("Invalid budget selected.");
                    return;
                }               
                m_mode = EntryMode.EDIT;
                
                enableDisable(true, false, false, true, false, true, false);
            }
            catch (Exception ex)
            {
                m_mode = EntryMode.NORMAL;
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdShowBudget.Rows.Count <= 1)
            {
                MessageBox.Show("No budget is avilable to delete.");
                return;
            }
            //just in case id is set to random number
            if (txtBudgetName.Text == "")
            {
                MessageBox.Show("No budget selected.");
                return;
            }
            if(id<1)
            {
                MessageBox.Show("No budget selected.");
                return;
            }
            bgt.DeleteBudget(id);
            txtBudgetName.Text = "";
            txtDescription.Text = "";
            txtStartDate.Mask = Date.FormatToMask();
            txtStartDate.Text = Date.ToSystem(Date.GetServerDate());
            txtEndDate.Mask = Date.FormatToMask();
            txtEndDate.Text = Date.ToSystem(Date.GetServerDate());
            FillGridWithData();
            id = 0;
        }

        private void createGridColumnHeader()
        {
            SourceGrid.Cells.Views.ColumnHeader headview = new SourceGrid.Cells.Views.ColumnHeader();
            headview.Font = new Font("Arial", 8, FontStyle.Bold);
            headview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            //  headview.Background = colhead;

            grdShowBudget.EnableSort = false;
            grdShowBudget.ColumnsCount = 7;
            grdShowBudget.FixedRows = 1;
            grdShowBudget.Rows.Insert(0);

            grdShowBudget[0, 0] = new SourceGrid.Cells.ColumnHeader();
            grdShowBudget[0, 0].View = headview;
            grdShowBudget[0, 1] = new SourceGrid.Cells.ColumnHeader("S.NO");
            grdShowBudget[0, 1].View = headview;
            grdShowBudget[0, 2] = new SourceGrid.Cells.ColumnHeader("Budget ID");          
            grdShowBudget[0, 2].Column.Visible = false;
            grdShowBudget[0, 3] = new SourceGrid.Cells.ColumnHeader("Budget Name");
            grdShowBudget[0, 3].View = headview;
            grdShowBudget[0, 4] = new SourceGrid.Cells.ColumnHeader("Start Date");
            grdShowBudget[0, 4].View = headview;
            grdShowBudget[0, 5] = new SourceGrid.Cells.ColumnHeader("End Date");
            grdShowBudget[0, 5].View = headview;
            grdShowBudget[0, 6] = new SourceGrid.Cells.ColumnHeader("Description");
            grdShowBudget[0, 6].View = headview;

            grdShowBudget[0, 0].Column.Width = 15;
            grdShowBudget[0, 1].Column.Width = 40;
            grdShowBudget[0, 2].Column.Width = 50;
            grdShowBudget[0, 3].Column.Width = 125;
            grdShowBudget[0, 4].Column.Width = 125;
            grdShowBudget[0, 5].Column.Width = 125;
            grdShowBudget[0, 6].Column.Width = 150;

            //defining event handler to the custom event
            rowSelect.Click += new EventHandler(Row_Click);
        }

        private void Row_Click(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
                int cr = context.Position.Row;
                id = Convert.ToInt32(grdShowBudget[cr, 2].Value);

                txtBudgetName.Text = grdShowBudget[cr, 3].Value.ToString();

                gstdate = grdShowBudget[cr, 4].Value.ToString();
                txtStartDate.Text = grdShowBudget[cr, 4].Value.ToString();

                txtEndDate.Text = grdShowBudget[cr, 5].Value.ToString();
                genddate = grdShowBudget[cr, 5].Value.ToString();
                if (grdShowBudget[cr, 6].Value.ToString() != "")
                {
                    txtDescription.Text = grdShowBudget[cr, 6].Value.ToString();
                }
            else
                {
                    txtDescription.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
               
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBudgetName.Text == "")
                {
                    MessageBox.Show("Budget name required.", "MESSAGE");
                    return;
                }
                if (Date.ToDotNet(txtStartDate.Text) > Date.ToDotNet(txtEndDate.Text))
                {
                    MessageBox.Show("Start date can not be greater than end date.");
                    txtStartDate.Focus();
                    return;
                }
                if(!txtStartDate.MaskCompleted)
                {
                    MessageBox.Show("Starting date is not in correct format.");
                    return;
                }                
                if(!txtEndDate.MaskCompleted)
                {
                    MessageBox.Show("End date is not in correct format.");
                    return;
                }
                DateTime startdate=Date.ToDotNet(txtStartDate.Text);
                DateTime enddate=Date.ToDotNet(txtEndDate.Text);
                switch (m_mode)
                {
                    case EntryMode.NEW:
                        if (Date.ToDotNet(txtStartDate.Text) < Date.ToDotNet(Date.ToSystem(Date.GetServerDate())))
                        {
                            MessageBox.Show("Invalid Start date, you cannot create budget for past date.");
                            txtStartDate.Focus();
                            return;
                        }
                        //not neccessary
                        if (Date.ToDotNet(txtEndDate.Text) < Date.ToDotNet(Date.ToSystem(Date.GetServerDate())))
                        {
                            MessageBox.Show("Invalid End date!");
                            txtEndDate.Focus();
                            return;
                        }
                        DataTable dtNchk = bgt.CheckRedundency(startdate,enddate,true,0);
                        if(dtNchk.Rows.Count>0)
                        {
                            MessageBox.Show("Provided Budget range clashed with Budget range of '"+dtNchk.Rows[0]["budgetName"]+"'");
                            return;
                        }
                        bgt.AddBudget(txtBudgetName.Text.Trim(),startdate,enddate,txtDescription.Text.Trim());
                        txtBudgetName.Text = "";
                        txtDescription.Text = "";
                        m_mode = EntryMode.NORMAL;
                        FillGridWithData();
                        enableDisable(false,true,true,false,true,false,true);
                        break;
                        //edit case
                    case EntryMode.EDIT:

                        if (Date.ToDotNet(txtEndDate.Text) < Date.ToDotNet(Date.ToSystem(Date.GetServerDate())))
                        {
                            MessageBox.Show("Invalid End date!, you cannot set end date less than current date.");
                            txtEndDate.Focus();
                            return;
                        }
                        //if user update the starting or end date then show warnning message
                      if(Date.ToDotNet(txtStartDate.Text)!=Date.ToDotNet( gstdate))
                        {
                         DialogResult res=   MessageBox.Show("You have changed the budget starting date from " + gstdate + " " + " to" + " " + txtStartDate.Text  + ". It might affect the previous reports. Do you want to continue?","CONFIRMATION",MessageBoxButtons.YesNo);
                            if(res==DialogResult.No)
                            {
                                return;
                            }
                        }

                        if(Date.ToDotNet( txtEndDate.Text)!=Date.ToDotNet(genddate))
                        {
                            DialogResult res = MessageBox.Show("You have changed the budget ending date from " + genddate + " " + " to" + " " + txtEndDate.Text  + ". It might affect the previous reports. Do you want to continue?", "CONFIRMATION", MessageBoxButtons.YesNo);
                            if (res == DialogResult.No)
                            {
                                return;
                            }
                        }
                     //check wheather start or end date fall in between the already allocated budget
                      DataTable dtEchk = bgt.CheckRedundency(startdate, enddate,false,id);
                      if (dtEchk.Rows.Count > 0)
                      {
                          MessageBox.Show("Provided Budget range clashed with Budget range of '" + dtEchk.Rows[0]["budgetName"] + "'");
                          return;
                      }

                      bgt.EditBudgetName(id,txtBudgetName.Text.Trim(), startdate, enddate, txtDescription.Text.Trim());
                      id = 0;
                         txtBudgetName.Text = "";
                        txtDescription.Text = "";
                        m_mode = EntryMode.NORMAL;
                        FillGridWithData();
                        enableDisable(false, true, true, false, true, false, true);
                        break;
                }
            }
            catch (Exception ex)
            {
              
                MessageBox.Show(ex.Message);
            }           
        }

        private void FillGridWithData()
        {
            try
            {
                if (grdShowBudget.Rows.Count > 1)
                {
                    int j = grdShowBudget.Rows.Count;
                    for (int i = j - 1; i > 0; i--)
                    {
                        grdShowBudget.Rows.Remove(i);
                    }
                }

                DataTable dt = bgt.GetCurrentNUpcommingBudgetData();
                if (dt.Rows.Count > 0)
                {
                    string startDate = "";
                    string endDate = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        int styear = 0;
                        int stmonth = 0;
                        int stday = 0;
                        int enyear = 0;
                        int enmonth = 0;
                        int enday = 0;
                        DateTime stdate = Convert.ToDateTime(dt.Rows[i][2].ToString());
                        Date.EngToNep(stdate, ref styear, ref stmonth, ref stday);
                        startDate = styear.ToString().PadLeft(4, '0') + "/" + stmonth.ToString().PadLeft(2, '0') + "/" + stday.ToString().PadLeft(2, '0');
                        DateTime endate = Convert.ToDateTime(dt.Rows[i][3].ToString());
                        Date.EngToNep(endate, ref enyear, ref enmonth, ref enday);
                        endDate = enyear.ToString().PadLeft(4, '0') + "/" + enmonth.ToString().PadLeft(2, '0') + "/" + enday.ToString().PadLeft(2, '0');
                        grdShowBudget.Rows.Insert(i + 1);
                        grdShowBudget[i + 1, 0] = new SourceGrid.Cells.Cell();
                        grdShowBudget[i + 1, 0].AddController(rowSelect);
                        grdShowBudget[i + 1, 1] = new SourceGrid.Cells.Cell(i+1);
                        grdShowBudget[i + 1, 1].AddController(rowSelect);
                        grdShowBudget[i + 1, 2] = new SourceGrid.Cells.Cell(dt.Rows[i][0]);
                        grdShowBudget[i + 1, 2].AddController(rowSelect);
                        grdShowBudget[i + 1, 3] = new SourceGrid.Cells.Cell(dt.Rows[i][1]);
                        grdShowBudget[i + 1, 3].AddController(rowSelect);
                        grdShowBudget[i + 1, 4] = new SourceGrid.Cells.Cell(startDate);
                        grdShowBudget[i + 1, 4].AddController(rowSelect);
                        grdShowBudget[i + 1, 5] = new SourceGrid.Cells.Cell(endDate);
                        grdShowBudget[i + 1, 5].AddController(rowSelect);
                        if (dt.Rows[i][4].ToString() == "")
                        {
                            grdShowBudget[i + 1, 6] = new SourceGrid.Cells.Cell(" ");
                        }
                        else
                        {
                            grdShowBudget[i + 1, 6] = new SourceGrid.Cells.Cell(dt.Rows[i][4]);
                        }

                        grdShowBudget[i + 1, 6].AddController(rowSelect);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void grdShowBudget_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
