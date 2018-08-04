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
    public partial class frmReference : Form
    {
        public int width, height;
        public int m_VoucherID;
        public int m_LedgerID;
        public string m_VoucherType;
        IVoucherReference m_VouchRef;
        public DataTable dtReference = null;
        SourceGrid.Cells.Controllers.CustomEvents evtClick = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtGridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();


        public frmReference()
        {
            InitializeComponent();
        }

        public frmReference(IVoucherReference VouchRef, int LedgerID)
        {
            InitializeComponent();
            this.m_LedgerID = LedgerID;
            this.m_VouchRef = VouchRef;

        }
        public frmReference(IVoucherReference VouchRef, int VoucherID, string VoucherType, int LedgerID)
        {
            InitializeComponent();
            this.m_VouchRef = VouchRef;
            this.m_VoucherID = VoucherID;
            this.m_VoucherType = VoucherType;
            this.m_LedgerID = LedgerID;
        }

        private void frmReference_Load(object sender, EventArgs e)
        {
            rbNone.Focus();
            //rbNone.Focus();
            width = this.Width; // first save the width of the form so that width of the form can be easily increased and decreased from this variable
            height = this.Height;

            rbNone.Checked = true;
            evtClick.DoubleClick += new EventHandler(Row_Click);
            evtGridKeyDown.KeyDown += new KeyEventHandler(grid_KeyDown);

            //dtReference = VoucherReference.GetReference(m_VoucherID, m_VoucherType, m_LedgerID);
            dtReference = VoucherReference.GetReference(m_LedgerID, m_VoucherID, m_VoucherType);

            LoadReference(dtReference);
        }
        public void IncreaseFormWidth(bool IsIncrease)
        {
            this.Width = width;

            if (!IsIncrease)
            {
                this.Width -= (grpRefList.Width);
            }
            grdRefList.Enabled = grpRefList.Visible = rbAgainst.Checked;
            grpNewRef.Enabled = grpNewRef.Visible = rbNewReference.Checked;
            
        }

        public void SetFormSize(bool none, bool against, bool newref)
        {
            IncreaseFormWidth(against | newref);
            rbAgainst.TabStop = rbNone.TabStop = rbNewReference.TabStop = true;

            rbNone.Checked = none;
            rbAgainst.Checked = against;
            rbNewReference.Checked = newref;

            grpRefList.Visible = (!newref | against);
            grpNewRef.Visible = newref;

            if(none)
            {
                //btnSubmit.Location = new Point(
                //    21, 167
                // );

                //btnCancel.Location = new Point(
                //    102, 167
                // );
                //btnSubmit.Visible = btnCancel.Visible = none;
                this.Height = height;
                btnSubmitNone.Visible = btnCancelNone.Visible = true;
            }
            else if (against)
            {
                //btnSubmit.Location = new Point(
                //   60, 174
                //);

                //btnCancel.Location = new Point(
                //    141, 174
                // );
                //btnSubmit.Visible = btnCancel.Visible = against;
                this.Height = height;
                btnSubmitNone.Visible = btnCancelNone.Visible = false;
            }
            else
            {
                //btnSubmit.Location = new Point(
                //    54, 91
                //);

                //btnCancel.Location = new Point(
                //    135, 91
                //);
                //btnSubmit.Visible = btnCancel.Visible = newref;

                this.Height -= 70;
                btnSubmitNone.Visible = btnCancelNone.Visible = true;
            }
        }
        private void WriteHeaderRecurring()
        {
            grdRefList[0, 0] = new SourceGrid.Cells.ColumnHeader("RefID");
            grdRefList[0, 0].Column.Visible = false;

            grdRefList[0, 1] = new SourceGrid.Cells.ColumnHeader("SN");
            grdRefList[0, 1].Column.Width = 30;
            grdRefList[0, 1].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdRefList[0, 2] = new SourceGrid.Cells.ColumnHeader("Reference Name");
            grdRefList[0, 2].Column.Width = 140;
            grdRefList[0, 2].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdRefList[0, 3] = new SourceGrid.Cells.ColumnHeader("Remaining Amt");
            grdRefList[0, 3].AddController(evtClick);
            grdRefList[0, 3].Column.Width = 90;
            grdRefList[0, 3].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;


        }
        private void LoadReference(DataTable dtv)
        {
            try
            {
                int rowsCount = 0;
                int i = 1;
                rowsCount = dtv.Rows.Count;
                grdRefList.Rows.Clear();

                grdRefList.Redim(rowsCount + 1, 4);

               // alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);

                WriteHeaderRecurring();
                if (dtv.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtv.Select("[CrAmt] - [DrAmt] <> 0"))
                    {
                        SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();

                        if (i % 2 == 0)
                        {
                            alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightGray);
                            alternate.Border = new DevAge.Drawing.RectangleBorder(DevAge.Drawing.BorderLine.NoBorder);
                        }
                        else
                        {
                            alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Silver);
                            alternate.Border = new DevAge.Drawing.RectangleBorder(DevAge.Drawing.BorderLine.NoBorder);
                        }
                       // DataRow dr = dtv.Rows[i - 1];
                        grdRefList[i, 0] = new SourceGrid.Cells.Cell(dr["RefID"].ToString());
                        grdRefList.Visible = false;

                        grdRefList[i, 1] = new SourceGrid.Cells.Cell(i.ToString());
                        grdRefList[i, 1].AddController(evtClick);
                        grdRefList[i, 1].AddController(evtGridKeyDown);
                        grdRefList[i, 1].View = alternate;

                        grdRefList[i, 2] = new SourceGrid.Cells.Cell(dr["RefName"].ToString());
                        grdRefList[i, 2].AddController(evtClick);
                        grdRefList[i, 2].AddController(evtGridKeyDown);
                        grdRefList[i, 2].View = alternate;

                        decimal res = Convert.ToDecimal(dr["CrAmt"]) - Convert.ToDecimal(dr["DrAmt"]);
                        string result = (Math.Abs(res)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdRefList[i, 3] = new SourceGrid.Cells.Cell((res <= 0) ? (result + "(Cr)") : (result + "(Dr)").ToString());
                        grdRefList[i, 3].AddController(evtClick);
                        grdRefList[i, 3].AddController(evtGridKeyDown);
                        grdRefList[i, 3].View = alternate;

                        i++;
                    } 
                }
                grdRefList.Columns.StretchToFit();
                grdRefList.Visible = true;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void Row_Click(object sender, EventArgs e)
        {
            try
            {
                int CurRow = grdRefList.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (CurRow > 0)
                {
                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdRefList, new SourceGrid.Position(CurRow, 0));
                    int refID = Convert.ToInt32(cellType.Value);
                    SourceGrid.CellContext cellType1 = new SourceGrid.CellContext(grdRefList, new SourceGrid.Position(CurRow, 3));
                    string res = cellType1.Value.ToString();
                    decimal amt = Convert.ToDecimal(res.Substring(0, res.Length - 4));
                    m_VouchRef.GetAgainstReference(refID, amt, res.Substring(res.Length - 4));
                    this.Close(); 
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Row_Click(null, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
            }
        }
        private void rbNewReference_CheckedChanged(object sender, EventArgs e)
        {           
            if (rbNewReference.Checked)
                SetFormSize(false, false, true);
        }

        private void rbAgainst_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAgainst.Checked)
                SetFormSize(false, true, false);
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {           
            if (rbNone.Checked)
                SetFormSize(true, false, false);
        }

        private void frmReference_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReferenceName.Text.Trim().Length == 0)
                {
                    Global.MsgError("Reference Name cannot be empty.");
                    txtReferenceName.Focus();
                    return;
                }
                else if (VoucherReference.IsReferenceExist(txtReferenceName.Text, m_LedgerID))
                {
                    Global.MsgError("Reference Name already exists.\n Reference name must be unique !");
                    txtReferenceName.Clear();
                    txtReferenceName.Focus();
                    return;
                }
                m_VouchRef.GetNewReference(txtReferenceName.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtReferenceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveNewRef.PerformClick();
            }
        }

        private void _KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void rbNone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if(rbNone.Checked) m_VouchRef.RemoveReference();
                this.Close();
            }
            if (e.KeyCode == Keys.Down)
            {
                rbNewReference.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btnSubmitNone.PerformClick();
            }
        }

        private void rbNewReference_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                rbNone.Focus();
            else if (e.KeyCode == Keys.Down)
                rbAgainst.Focus();
        }

        private void rbAgainst_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmitNone_Click(object sender, EventArgs e)
        {
            m_VouchRef.RemoveReference();
            this.Close();
        }

        private void btnSubmitAgainst_Click(object sender, EventArgs e)
        {
            Row_Click(null, null);
        }

    }
}
