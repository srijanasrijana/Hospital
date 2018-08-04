using BusinessLogic;
using BusinessLogic.HOS;
using BusinessLogic.HRM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hospital.View
{
    public partial class frmDoctorList : Form,IDoctorList
    {
        public frmDoctorList()
        {
            InitializeComponent();
        }

        private IDoctorList m_ParentForm;
        public frmDoctorList(IDoctorList ParentForm)
        {
            m_ParentForm = (IDoctorList)ParentForm;
            InitializeComponent();
        }

        private IDoctorInfo m_doctrorinfo;
        public frmDoctorList(IDoctorInfo DoctorForm)
        {
            m_doctrorinfo = (IDoctorInfo)DoctorForm;
            InitializeComponent();
        }

       
        private DataRow[] drFound;
        private DataTable dTable;
        private string FilterString = "";
        private SourceGrid.Cells.Views.Cell RowView;
        private DataTable tempTable;

        ListItem liSpecilization = new ListItem();
        ListItem liFaculty = new ListItem();
        private bool isSelected = false;

       

        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        private DataView mView;
        private void frmDoctorList_Load(object sender, EventArgs e)
        {
            LoadForm();
            txtFName.Select();
        }

        private void LoadForm()
        {

            BusinessLogic.HOS.Doctor doctor = new BusinessLogic.HOS.Doctor();
            dTable = doctor.GetDoctorList(null);         
            
            drFound = dTable.Select(FilterString);

            LoadComboboxItems(cmbSpecilization);
            LoadComboboxItems(cmbFaculty);
            cmbType.SelectedIndex = 0;
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(Grid_DoubleClick);
        

            FillGrid();

        }
        private void Grid_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                isSelected = true;
                int CurRow = grdEmployee.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext DocID = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 0));
                SourceGrid.CellContext Code = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 2));
                SourceGrid.CellContext Name = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 3));
                SourceGrid.CellContext Specilization = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 4));
                SourceGrid.CellContext BankAC = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 7));
                
               AddDoctor(Convert.ToInt32(DocID.Value.ToString()), Code.Value.ToString(), Name.Value.ToString(), Specilization.Value.ToString(), BankAC.Value.ToString(), isSelected);
                    this.Dispose();
               
               
            }
            catch (Exception)
            {
                Global.Msg("Invalid selection");
            }

        }

       
      
        private void FillGrid()
        {
            try
            {
                grdEmployee.Rows.Clear();
                grdEmployee.Redim(drFound.Count() + 1, 8);
                WriteHeader();
                if (drFound.Count() > 0)
                {
                    for (int i = 1; i <= drFound.Count(); i++)
                    {
                        DataRow dr = drFound[i - 1];
                        RowView = new SourceGrid.Cells.Views.Cell();
                        if (i % 2 == 0)
                        {
                            RowView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        }
                        else
                        {
                            RowView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        }
                        int typeIndex = Convert.ToInt32(dr["Type"].ToString());
                        grdEmployee[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                        grdEmployee[i, 0].AddController(dblClick);
                        grdEmployee[i, 0].View = RowView;
                        grdEmployee[i, 1] = new SourceGrid.Cells.Cell(i);
                        grdEmployee[i, 1].AddController(dblClick);
                        grdEmployee[i, 1].View = RowView;
                        grdEmployee[i, 2] = new SourceGrid.Cells.Cell(dr["DoctorCode"].ToString());
                        grdEmployee[i, 2].AddController(dblClick);
                        grdEmployee[i, 2].View = RowView;
                        grdEmployee[i, 3] = new SourceGrid.Cells.Cell(dr["DoctorName"].ToString());
                        grdEmployee[i, 3].AddController(dblClick);
                        grdEmployee[i, 3].View = RowView;
                        grdEmployee[i, 4] = new SourceGrid.Cells.Cell(dr["SpecilizationName"].ToString());
                        grdEmployee[i, 4].AddController(dblClick);
                        grdEmployee[i, 4].View = RowView;
                        grdEmployee[i, 5] = new SourceGrid.Cells.Cell(dr["FacultyName"].ToString());
                        grdEmployee[i, 5].AddController(dblClick);
                        grdEmployee[i, 5].View = RowView;
                        grdEmployee[i, 6] = new SourceGrid.Cells.Cell(typeIndex == 0 ? "Permanent" : typeIndex == 1 ? "Contract" : "Part Time");
                        grdEmployee[i, 6].AddController(dblClick);
                        grdEmployee[i, 6].View = RowView;
                        grdEmployee[i, 7] = new SourceGrid.Cells.Cell(dr["BankACNumber"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void WriteHeader()
        {
            grdEmployee[0, 0] = new SourceGrid.Cells.ColumnHeader("DoctorID");
            grdEmployee[0, 0].Column.Visible = false;
            grdEmployee[0, 1] = new SourceGrid.Cells.ColumnHeader("SN");
            grdEmployee[0, 1].Column.Width = 40;
            grdEmployee[0, 2] = new SourceGrid.Cells.ColumnHeader("Code");
            grdEmployee[0, 2].Column.Width = 70;
            grdEmployee[0, 3] = new SourceGrid.Cells.ColumnHeader("Doctor Name");
            grdEmployee[0, 3].Column.Width = 270;
            grdEmployee[0, 4] = new SourceGrid.Cells.ColumnHeader("Specilization");
            grdEmployee[0, 4].Column.Width = 120;
            grdEmployee[0, 5] = new SourceGrid.Cells.ColumnHeader("Faculty");
            grdEmployee[0, 5].Column.Width = 95;
            grdEmployee[0, 6] = new SourceGrid.Cells.ColumnHeader("Type");
            grdEmployee[0, 6].Column.Width = 95;
            grdEmployee[0, 7] = new SourceGrid.Cells.ColumnHeader("BankAC");
            grdEmployee[0, 7].Column.Visible = false;
        }

     
      
            private void FillDoctorDetails()
         {
            #region NEW ONE
            try
            {
                BusinessLogic.HOS.Doctor doctor = new BusinessLogic.HOS.Doctor();
                DataTable dtDocotrDetails = doctor.GetDoctorList(null);               
 
                    tempTable = new DataTable();
                    tempTable.Columns.Add("ID", typeof(int));
                    tempTable.Columns.Add("DoctorCode", typeof(string)); 
                    tempTable.Columns.Add("DoctorName", typeof(string));                
                    tempTable.Columns.Add("SpecilizationName", typeof(string));
                    tempTable.Columns.Add("FacultyName", typeof(string));
                    tempTable.Columns.Add("Type", typeof(string));
                    tempTable.Columns.Add("BankACNumber", typeof(string));
                   
                    foreach (DataRow dr in dtDocotrDetails.Rows)
                    {
                        int typeIndex = Convert.ToInt32(dr["Type"].ToString());
                      
                        string type = typeIndex == 0 ? "Permanent" : typeIndex == 1 ? "Contract" : "Part Time";


                        tempTable.Rows.Add(Convert.ToInt32(dr["ID"]),dr["DoctorCode"].ToString(), dr["DoctorName"].ToString(),
                           dr["SpecilizationName"].ToString(), dr["FacultyName"].ToString(), type, dr["BankACNumber"].ToString());
                    }

                    System.Data.DataView view = new System.Data.DataView(tempTable);
                    System.Data.DataTable selected = view.ToTable("tblDoctor", false,"ID", "DoctorCode", "DoctorName", "Type", "FacultyName", "SpecilizationName", "BankACNumber");
                        
            
        
                #region TEST for datagrid binding

               mView = selected.DefaultView;
               grdEmployee.FixedRows =1; //Allocated for Header
               grdEmployee.FixedColumns =0;

                //Header row
              //  dgListOfLedger.Columns.Insert( 0,SourceGrid.DataGridColumn.CreateRowHeader(dgListOfLedger));//this Create one balnk extra column in the very first of a table
                DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);

                //Create default columns
             //   CreateColumns(grdEmployee.Columns, bindList);
               // grdEmployee.DataSource = bindList;

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
            }
            #endregion  

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

        private enum GridColumn : int
        {
            DoctorID = 0, SN, Code, DoctorName, Specilization, Faculty, Type, BankAC
        };

       
        private void Filter()
        {
            try
            {
                int id;
                string str = " and e.DoctorCode like '" + txtCode.Text.Trim() + "%' and e.FirstName like '" + txtFName.Text.Trim() + "%' and e.MiddleName like '" + txtMName.Text.Trim() + "%' and e.LastName like '" + txtLName.Text.Trim() + "%' ";
                if (cmbSpecilization.SelectedIndex > 0)
                {
                    liSpecilization = (ListItem)cmbSpecilization.SelectedItem;
                    id = liSpecilization.ID;
                    str += " and e.SpecilizationID = '" + id + "'";
                }

                if (cmbFaculty.SelectedIndex > 0)
                {
                    liFaculty = (ListItem)cmbFaculty.SelectedItem;
                    id = liFaculty.ID;
                    str += " and e.FacultyID = '" + id + "'";
                }

                if (cmbType.SelectedIndex > 0)
                {
                    id = cmbType.SelectedIndex - 1;
                    str += " and e.Type = '" + id + "'";
                }

                BusinessLogic.HOS.Doctor doctor = new BusinessLogic.HOS.Doctor();
                dTable = doctor.GetDoctorList(str);
                drFound = dTable.Select(FilterString);

                FillDoctorDetails();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }


        }

       

       
        

        private void LoadComboboxItems(ComboBox comboboxitems)
        {
            try
            {
                comboboxitems.Items.Clear();
               
                if (comboboxitems == cmbSpecilization)
                {

                    DataTable dtSpecilization = Hos.getSpecilization();
                    if (dtSpecilization.Rows.Count > 0)
                    {
                        foreach (DataRow drSpecilization in dtSpecilization.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drSpecilization["SpecilizationID"], drSpecilization["SpecilizationName"].ToString()));

                        }
                        comboboxitems.SelectedIndex = 0;
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                    }
                }
                else if (comboboxitems == cmbFaculty)
                {

                    DataTable dtFaculty = Hos.GetFaculty();
                    if (dtFaculty.Rows.Count > 0)
                    {
                        foreach (DataRow drFaculty in dtFaculty.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drFaculty["FID"], drFaculty["FacultyName"].ToString()));

                        }
                        comboboxitems.SelectedIndex = 0;
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                    }
                }
              
               

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void FilterGrid()
        {
            try
            {
                int id;
                string str = " and E.DoctorCode like '" + txtCode.Text.Trim() + "%' and E.FirstName like '" + txtFName.Text.Trim() + "%' and E.MiddleName like '" + txtMName.Text.Trim() + "%' and E.LastName like '" + txtLName.Text.Trim() + "%' ";
                if (cmbSpecilization.SelectedIndex > 0)
                {
                    liSpecilization = (ListItem)cmbSpecilization.SelectedItem;
                    id = liSpecilization.ID;
                    str += " and a.Specilization = '" + id + "'";
                }

                if (cmbFaculty.SelectedIndex > 0)
                {
                    liFaculty = (ListItem)cmbFaculty.SelectedItem;
                    id = liFaculty.ID;
                    str += " and a.FacultyID = '" + id + "'";
                }

                if (cmbType.SelectedIndex > 0)
                {
                    id = cmbType.SelectedIndex - 1;
                    str += " and a.Type = '" + id + "'";
                }

                BusinessLogic.HOS.Doctor doctor = new BusinessLogic.HOS.Doctor();
                dTable = doctor.GetDoctorList(str);  
               
                drFound = dTable.Select(FilterString);

                FillGrid();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
          //  Filter();
            FilterGrid();
        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {
              // Filter();
            FilterGrid();
        }

        private void evtValueChanged(object sender, EventArgs e)
        {
            //   Filter();
            FilterGrid();
        }


        public void AddDoctor(int DocID, string Code, string Name, string Specilization, string BankAC, bool IsSelected)
        {
            m_ParentForm.AddDoctor(DocID, Code, Name, Specilization, BankAC, isSelected);
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            frmDoctorRegistration fer = new frmDoctorRegistration();
            fer.ShowDialog();
            LoadForm();
        }

        public void AddDoctorInfo(int docID, string code, string Name, string Specilization, bool IsSelected)
        {
              m_doctrorinfo.AddDoctorInfo(docID, code, Name, Specilization,  isSelected);
        }
    }
}
