using BusinessLogic;
using BusinessLogic.HOS;
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

    public interface IListOfPatient
    {
        void AddPatient(string RegistrationNo, string Name, string Address, string Age,string Telephone, string Gender, int PatientID, int LedgerID);
    }
    public partial class FrmListOfPatientInfo : Form,IListOfPatient
    {
        public FrmListOfPatientInfo()
        {
            InitializeComponent();
        }
        public FrmListOfPatientInfo(IListOfPatient ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (IListOfPatient)ParentForm;
           
            this.Font = LangMgr.GetFont();
        }
        private DataRow[] drFound;
        private DataTable dTable;
        private string FilterString = "";
        private SourceGrid.Cells.Views.Cell RowView;
        private DataTable tempTable;

        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        private DataView mView;
        private void FrmListOfPatientInfo_Load(object sender, EventArgs e)
        {
            LoadForm();
            txtFName.Select();
        }
        private IListOfPatient m_ParentForm;
        private void LoadForm()
        {


            BusinessLogic.HOS.Patient patient = new BusinessLogic.HOS.Patient();
            dTable = Patient.GetPatientList(null);
            drFound = dTable.Select(FilterString);


            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
          
          //  dblClick.DoubleClick += new EventHandler(Grid_DoubleClick);
      
            FillGrid();

        }
        private bool isSelected = false;
        private void Grid_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                isSelected = true;
                int CurRow = grdEmployee.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext RegistrationNo = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 1));
                SourceGrid.CellContext Name = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 2));             
                SourceGrid.CellContext Address = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 5));
                SourceGrid.CellContext Age = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 3));              
                SourceGrid.CellContext Telehone = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 5));
                SourceGrid.CellContext Gender = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 4));
                SourceGrid.CellContext PatientID = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 10));
                SourceGrid.CellContext LedgerID = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 9));

                AddPatient(RegistrationNo.Value.ToString(), Name.Value.ToString(), Address.Value.ToString(), Age.Value.ToString(),
                    Telehone.Value.ToString(), Gender.Value.ToString(), Convert.ToInt32(PatientID.Value), Convert.ToInt32(LedgerID.Value));
                this.Dispose();


            }
            catch (Exception)
            {
                Global.Msg("Invalid selection");
            }

        }
        public void AddPatient(string RegistrationNo, string Name, string Address, string Age, string Telephone, string Gender, int PatientID, int LedgerID)
        {
            m_ParentForm.AddPatient(RegistrationNo, Name, Address, Age, Telephone, Gender, PatientID, LedgerID);
        }

        private void FillGrid()
        {
            try
            {
                grdEmployee.Rows.Clear();
                grdEmployee.Redim(drFound.Count() + 1, 12);
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
                    
                        grdEmployee[i, 0] = new SourceGrid.Cells.Cell(i);
                        grdEmployee[i, 0].AddController(dblClick);
                        grdEmployee[i, 0].View = RowView;
                        grdEmployee[i, 1] = new SourceGrid.Cells.Cell(dr["RegistrationNo"].ToString());
                        grdEmployee[i, 1].AddController(dblClick);
                        grdEmployee[i, 1].View = RowView;
                        grdEmployee[i, 2] = new SourceGrid.Cells.Cell(dr["PatientName"].ToString());
                        grdEmployee[i, 2].AddController(dblClick);
                        grdEmployee[i, 2].View = RowView;
                        grdEmployee[i, 3] = new SourceGrid.Cells.Cell(dr["Age"].ToString());
                        grdEmployee[i, 3].AddController(dblClick);
                        grdEmployee[i, 3].View = RowView;

                        int typeGender = Convert.ToInt32(dr["Sex"].ToString());
                        grdEmployee[i, 4] = new SourceGrid.Cells.Cell(typeGender == 1 ? "Male" : typeGender == 2 ? "Female" : "Other");                     
                        grdEmployee[i, 4].AddController(dblClick);

                        grdEmployee[i, 4].View = RowView;
                        grdEmployee[i, 5] = new SourceGrid.Cells.Cell(dr["Telephone"].ToString());
                        grdEmployee[i, 5].AddController(dblClick);
                        grdEmployee[i, 5].View = RowView;
                        grdEmployee[i, 6] = new SourceGrid.Cells.Cell(dr["Mobile"].ToString());
                        grdEmployee[i, 6].AddController(dblClick);
                        grdEmployee[i, 6].View = RowView;
                        grdEmployee[i, 7] = new SourceGrid.Cells.Cell(dr["Date"].ToString());
                        grdEmployee[i, 7].AddController(dblClick);
                        grdEmployee[i, 7].View = RowView; 
                        grdEmployee[i, 8] = new SourceGrid.Cells.Cell(dr["PatientType"].ToString());
                        grdEmployee[i, 8].AddController(dblClick);
                        grdEmployee[i, 8].View = RowView;
                        grdEmployee[i, 9] = new SourceGrid.Cells.Cell(dr["LedgerID"].ToString());
                        grdEmployee[i, 9].AddController(dblClick);
                      //  grdEmployee[i, 9].Value = false;
                        grdEmployee[i, 9].View = RowView;
                        grdEmployee[i, 10] = new SourceGrid.Cells.Cell(dr["PatientID"].ToString());
                      //  grdEmployee[i, 10].Value = false;
                        grdEmployee[i, 10].AddController(dblClick);
                        grdEmployee[i, 10].View = RowView;

                        grdEmployee[i, 11] = new SourceGrid.Cells.Cell(dr["DoctorName"].ToString());
                        grdEmployee[i, 11].AddController(dblClick);
                        grdEmployee[i, 11].View = RowView;  
                      
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
            grdEmployee[0, 0] = new SourceGrid.Cells.ColumnHeader("SN");
            grdEmployee[0, 0].Column.Width = 40;
            grdEmployee[0, 1] = new SourceGrid.Cells.ColumnHeader("RegNo");
            grdEmployee[0, 1].Column.Width = 50;         
            grdEmployee[0, 2] = new SourceGrid.Cells.ColumnHeader("Patient Name");
            grdEmployee[0, 2].Column.Width = 180;
            grdEmployee[0, 3] = new SourceGrid.Cells.ColumnHeader("Age");
            grdEmployee[0, 3].Column.Width = 40;
            grdEmployee[0, 4] = new SourceGrid.Cells.ColumnHeader("Sex");
            grdEmployee[0, 4].Column.Width = 70;
            grdEmployee[0, 5] = new SourceGrid.Cells.ColumnHeader("Telephoane");
            grdEmployee[0, 5].Column.Width = 100;
            grdEmployee[0, 6] = new SourceGrid.Cells.ColumnHeader("Mobile");
            grdEmployee[0, 6].Column.Width = 100;
            grdEmployee[0, 7] = new SourceGrid.Cells.ColumnHeader("Date");
            grdEmployee[0, 7].Column.Width = 150;
            grdEmployee[0, 8] = new SourceGrid.Cells.ColumnHeader("Patient Type");
            grdEmployee[0, 8].Column.Width = 120;
            grdEmployee[0, 9] = new SourceGrid.Cells.ColumnHeader("LedgerID");
            grdEmployee[0, 9].Column.Visible = false;
            grdEmployee[0, 10] = new SourceGrid.Cells.ColumnHeader("PatientID");
            grdEmployee[0, 10].Column.Visible = false;

            grdEmployee[0, 11] = new SourceGrid.Cells.ColumnHeader("DoctorName");
            grdEmployee[0, 11].Column.Width = 180;




           
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }

        private void FilterGrid()
        {
            try
            {
               
                

              //string str = "and RegistrationNo like '" + txtCode.Text.Trim() + "%' and FirstName like '" + txtFName.Text.Trim() + "%' and  MiddleName like '" + txtMName.Text.Trim() + "%' and LastName like '" + txtLName.Text.Trim() + "%' ";
                string str = " where p.RegistrationNo LIKE '" + txtCode.Text + "%' and p.FirstName like '" + txtFName.Text.Trim() + "%' and p.MiddleName Like '%" + txtMName.Text + "%' and p.LastName Like '%" + txtLName.Text + "%' ";
             //   string str = " ";

              
                dTable = Patient.GetPatientList(str);
                drFound = dTable.Select(FilterString);

                FillGrid();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }




      
    }
}
