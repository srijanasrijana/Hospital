using AccSwift;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public partial class frmCompoundUnit : Form
    {
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Views.Cell alternate = null;
        SourceGrid.Cells.Views.Cell rightAlign = null;
        SourceGrid.Cells.Views.Cell centerAlign = null;

        SourceGrid.Cells.Controllers.CustomEvents evtDblClick = new SourceGrid.Cells.Controllers.CustomEvents();

        DataTable dtCompoundUnit = null;
        int m_CompoundUnitID = 0;
        bool m_isNew = true;

        public frmCompoundUnit()
        {
            InitializeComponent();
        }

        public enum GridColumns { Unit_Relation_ID = 0, Del, SN, Value_of_First_Unit, Name_of_First_Unit, Relation, Value_of_Second_Unit, Name_of_Second_Unit, Remarks }
        private void frmCompoundUnit_Load(object sender, EventArgs e)
        {
            evtDelete.Click += new EventHandler(Delete_Row_Click);
            evtDblClick.DoubleClick += new EventHandler(Row_Double_Click);
            LoadCombobox();
            LoadGrid();
        }

        public void LoadCombobox()
        {
            try
            {
                DataTable dt = UnitMaintenance.GetUnitMaintenaceInfo(-1);

                cmbFirstUnit.DataSource = dt;
                cmbFirstUnit.DisplayMember = "UnitName";
                cmbFirstUnit.ValueMember = "UnitMaintenanceID";

                DataTable dtSecond = dt.Copy(); // use different data source for both the comboboxes as using same datasource for both results in some inconvenience
                cmbSecondUnit.DataSource = dtSecond;
                cmbSecondUnit.DisplayMember = "UnitName";
                cmbSecondUnit.ValueMember = "UnitMaintenanceID";
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void AddGridHeader()
        {
            grdCompoundUnit[0, (int)GridColumns.Unit_Relation_ID] = new MyHeader("CompoundUnitID");
            grdCompoundUnit[0, (int)GridColumns.Unit_Relation_ID].Column.Visible = false;

            grdCompoundUnit[0, (int)GridColumns.Del] = new MyHeader("Del");
            grdCompoundUnit[0, (int)GridColumns.Del].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdCompoundUnit[0, (int)GridColumns.Del].Column.Width = 20;

            grdCompoundUnit[0, (int)GridColumns.SN] = new MyHeader("SN");
            grdCompoundUnit[0, (int)GridColumns.Del].Column.Width = 25;
            grdCompoundUnit[0, (int)GridColumns.SN].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCompoundUnit[0, (int)GridColumns.Value_of_First_Unit] = new MyHeader("Value of First Unit");
            grdCompoundUnit[0, (int)GridColumns.Value_of_First_Unit].Column.Width = 50;
            grdCompoundUnit[0, (int)GridColumns.Value_of_First_Unit].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdCompoundUnit[0, (int)GridColumns.Name_of_First_Unit] = new MyHeader("Name of First Unit");
            grdCompoundUnit[0, (int)GridColumns.Name_of_First_Unit].Column.Width = 50;
            grdCompoundUnit[0, (int)GridColumns.Name_of_First_Unit].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdCompoundUnit[0, (int)GridColumns.Relation] = new MyHeader("Equals");
            grdCompoundUnit[0, (int)GridColumns.Relation].Column.Width = 30;
            grdCompoundUnit[0, (int)GridColumns.Relation].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCompoundUnit[0, (int)GridColumns.Value_of_Second_Unit] = new MyHeader("Value of Second Unit");
            grdCompoundUnit[0, (int)GridColumns.Value_of_Second_Unit].Column.Width = 50;
            grdCompoundUnit[0, (int)GridColumns.Value_of_Second_Unit].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdCompoundUnit[0, (int)GridColumns.Name_of_Second_Unit] = new MyHeader("Name of Second Unit");
            grdCompoundUnit[0, (int)GridColumns.Name_of_Second_Unit].Column.Width = 50;
            grdCompoundUnit[0, (int)GridColumns.Name_of_Second_Unit].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdCompoundUnit[0, (int)GridColumns.Remarks] = new MyHeader("Remarks");
            grdCompoundUnit[0, (int)GridColumns.Remarks].Column.Width = 50;
            grdCompoundUnit[0, (int)GridColumns.Remarks].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdCompoundUnit.FixedRows = 1; //always display the header
        }

        public void LoadGrid()
        {
            try
            {
                //decimal amount = 0;
                dtCompoundUnit = UnitMaintenance.GetCompoundUnit();
                int rowsCount = 0;
                rowsCount = dtCompoundUnit.Rows.Count;
                grdCompoundUnit.Rows.Clear();

                grdCompoundUnit.SelectionMode = SourceGrid.GridSelectionMode.Row;
                grdCompoundUnit.Redim(rowsCount + 1, 9);

                AddGridHeader();
                for (int i = 1; i <= rowsCount; i++)
                {
                    SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                    btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;

                    alternate = new SourceGrid.Cells.Views.Cell();
                    rightAlign = new SourceGrid.Cells.Views.Cell();                          // right alignment for RelationValue
                    centerAlign = new SourceGrid.Cells.Views.Cell();                         // center alignment for =

                    rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;
                    centerAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        centerAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }

                    DataRow dr = dtCompoundUnit.Rows[i - 1];
                    grdCompoundUnit[i, (int)GridColumns.Unit_Relation_ID] = new SourceGrid.Cells.Cell(dr["CompoundUnitID"].ToString());

                    grdCompoundUnit[i, (int)GridColumns.Del] = btnDelete;
                    grdCompoundUnit[i, (int)GridColumns.Del].AddController(evtDelete);

                    grdCompoundUnit[i, (int)GridColumns.SN] = new SourceGrid.Cells.Cell((i).ToString());
                    grdCompoundUnit[i, (int)GridColumns.SN].View = alternate;
                    grdCompoundUnit[i, (int)GridColumns.SN].AddController(evtDblClick);

                    grdCompoundUnit[i, (int)GridColumns.Value_of_First_Unit] = new SourceGrid.Cells.Cell((1).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdCompoundUnit[i, (int)GridColumns.Value_of_First_Unit].View = alternate;
                    grdCompoundUnit[i, (int)GridColumns.Value_of_First_Unit].AddController(evtDblClick);

                    grdCompoundUnit[i, (int)GridColumns.Name_of_First_Unit] = new SourceGrid.Cells.Cell(dr["ParentUnit"]);
                    grdCompoundUnit[i, (int)GridColumns.Name_of_First_Unit].View = alternate;
                    grdCompoundUnit[i, (int)GridColumns.Name_of_First_Unit].AddController(evtDblClick);

                    grdCompoundUnit[i, (int)GridColumns.Relation] = new SourceGrid.Cells.Cell("=");
                    grdCompoundUnit[i, (int)GridColumns.Relation].View = centerAlign;
                    grdCompoundUnit[i, (int)GridColumns.Relation].AddController(evtDblClick);

                    grdCompoundUnit[i, (int)GridColumns.Name_of_Second_Unit] = new SourceGrid.Cells.Cell(dr["ChildUnit"]);
                    grdCompoundUnit[i, (int)GridColumns.Name_of_Second_Unit].View = alternate;
                    grdCompoundUnit[i, (int)GridColumns.Name_of_Second_Unit].AddController(evtDblClick);

                    grdCompoundUnit[i, (int)GridColumns.Value_of_Second_Unit] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["RelationValue"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdCompoundUnit[i, (int)GridColumns.Value_of_Second_Unit].View = rightAlign;
                    grdCompoundUnit[i, (int)GridColumns.Value_of_Second_Unit].AddController(evtDblClick);

                    grdCompoundUnit[i, (int)GridColumns.Remarks] = new SourceGrid.Cells.Cell(dr["Remarks"].ToString());
                    grdCompoundUnit[i, (int)GridColumns.Remarks].View = alternate;
                    grdCompoundUnit[i, (int)GridColumns.Remarks].AddController(evtDblClick);

                }
                grdCompoundUnit.AutoStretchColumnsToFitWidth = true;

                grdCompoundUnit.Columns.StretchToFit();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    int parentUnit = 0;
                    int childUnit = 0;
                    int firstUnitID = Convert.ToInt32(cmbFirstUnit.SelectedValue);
                    int secondUnitID = Convert.ToInt32(cmbSecondUnit.SelectedValue);
                    decimal relationValue = Convert.ToDecimal(txtValueOfSecondUnit.Text);
                    decimal firstUnitValue = Convert.ToDecimal(txtValueOfFirstUnit.Text);
                    if (relationValue > firstUnitValue) // if the  value of second unit is greater than first then make the first one parent
                    {
                        parentUnit = firstUnitID;
                        childUnit = secondUnitID;
                    }
                    else   // if the value of first unit is greater than second then make the second one parent unit
                    {
                        childUnit = firstUnitID;
                        parentUnit = secondUnitID;
                        relationValue = firstUnitValue / relationValue; // if 1 gm = 0.001 kg then 1 kg = (1/0.001) = 1000 gm
                    }
                    //int result = 0;
                    if (m_isNew)
                    {
                        if(UnitMaintenance.CompoundUnitCreate(childUnit, parentUnit, relationValue, txtRemarks.Text, User.CurrUserID, DateTime.Today)>0)
                        {
                            Global.Msg("Compound Unit created successfully !");
                        }
                    }
                    else // for edit mode
                    {
                        if (UnitMaintenance.CompoundUnitModify(m_CompoundUnitID,childUnit, parentUnit, relationValue, txtRemarks.Text, User.CurrUserID, DateTime.Today) > 0)
                        {
                            Global.Msg("Compound Unit modified successfully !");
                        }
                    }

                    ClearData();
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        public bool ValidateData()
        {
            try
            {
                int firstUnitID = Convert.ToInt32(cmbFirstUnit.SelectedValue);
                int secondUnitID = Convert.ToInt32(cmbSecondUnit.SelectedValue);
                //bool res = true;
                if (!UserValidation.validDecimal(txtValueOfSecondUnit.Text))
                {
                    Global.MsgError("Value of second unit must be decimal !");
                    return false;
                }

                if (cmbFirstUnit.SelectedIndex < 0 || cmbSecondUnit.SelectedIndex < 0)
                {
                    Global.MsgError("You must select both first and the second unit !");
                    return false;
                }

                if (firstUnitID == secondUnitID)
                {
                    Global.MsgError("You cannot select same units !");
                    return false;
                }

                if (m_isNew)
                {
                    if (UnitMaintenance.CheckIfCompoundUnitExists(firstUnitID, secondUnitID))
                    {
                        Global.MsgError("Compound unit already exists for " + cmbFirstUnit.Text + " and " + cmbSecondUnit.Text + " !");
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return false;
            }
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

                //Do not delete if its the first Row because it contains the header
                if (ctx.Position.Row != 0)
                {
                    int CurRow = grdCompoundUnit.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdCompoundUnit, new SourceGrid.Position(CurRow, 0));
                    int unitID = Convert.ToInt32(cellType.Value);
                    UnitMaintenance.DeleteCompoundUnit(unitID);
                    grdCompoundUnit.Rows.Remove(ctx.Position.Row);
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void ClearData()
        {
            txtValueOfFirstUnit.Text = "1";
            txtValueOfSecondUnit.Text = "";
            cmbFirstUnit.SelectedIndex = 0;
            cmbSecondUnit.SelectedIndex = 0;
            txtRemarks.Text = "";
            m_CompoundUnitID = 0;
            m_isNew = true;
        }

        public void LoadData()
        {
            try
            {
                DataTable dtUnit = UnitMaintenance.GetCompoundUnit(m_CompoundUnitID);
                if (dtUnit.Rows.Count > 0)
                {
                    DataRow dr = dtUnit.Rows[0];
                    txtValueOfFirstUnit.Text = (1).ToString();//(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    txtValueOfSecondUnit.Text = Convert.ToDecimal(dr["RelationValue"]).ToString();//.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    cmbFirstUnit.Text = dr["ParentUnit"].ToString();
                    cmbSecondUnit.Text = dr["ChildUnit"].ToString();
                    txtRemarks.Text = dr["Remarks"].ToString();
                    m_isNew = false;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void Row_Double_Click(object sender, EventArgs e)
        {
            try
            {
                int CurRow = grdCompoundUnit.Selection.GetSelectionRegion().GetRowsIndex()[0];

                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdCompoundUnit, new SourceGrid.Position(CurRow, 0));
                m_CompoundUnitID = Convert.ToInt32(cellType.Value);

                LoadData();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnAddNewUnit_Click(object sender, EventArgs e)
        {
            try
            {
                frmUnitMaitenace frmUnitMaintenance = new frmUnitMaitenace();
                frmUnitMaintenance.ShowDialog();

                LoadCombobox(); // reload the combobox to reflect the changes made to the units
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

    }

    class MyHeader : SourceGrid.Cells.ColumnHeader
    {
        public MyHeader(object value)
            : base(value)
        {

            SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            view.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            View = view;

            AutomaticSortEnabled = false;
        }

    }

}
