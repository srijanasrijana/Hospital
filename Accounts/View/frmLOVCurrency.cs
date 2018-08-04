using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace Accounts
{
    public interface ILOVCurrency
    {
        void AddCurrency(string Symbol);

    }

    public partial class frmLOVCurrency : Form
    {
        private Form m_Parent; //Holds the parent form

        private ILOVCurrency m_ParentForm; //holds the selected CCY Code

        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick; //Double click event holder

        public frmLOVCurrency(Form ParentForm)
        {
            InitializeComponent();
            

            m_ParentForm = (ILOVCurrency)ParentForm;

            //Set the selected font to everything
            this.Font = LangMgr.GetFont();

        }

        private void frmLOVCurrency_Load(object sender, EventArgs e)
        {
            dTable = Currency.GetCurrencyTable();
            
            drFound=dTable.Select(FilterString);

            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(LOVGrid_DoubleClick);

            
            //Let the whole row to be selected
            LOVGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            

            //Set the border width of the selection to thin
            //DevAge.Drawing.RectangleBorder b = LOVGrid.Selection.Border;
            //b.SetWidth(1);
            //LOVGrid.Selection.Border = b;

            //Disable multiple selection
            LOVGrid.Selection.EnableMultiSelection = false;

            //Finally fill all the values in the grid with no filter applied
            FillGrid();
        }


        //Fills the header of the grid with the required column names and also sets the width
        private void WriteHeader()
        {
            LOVGrid[0, 0] = new SourceGrid.Cells.ColumnHeader("S.No.");
            LOVGrid[0, 1] = new SourceGrid.Cells.ColumnHeader("Name");
            LOVGrid[0, 2] = new SourceGrid.Cells.ColumnHeader("Symbol");
            LOVGrid[0, 3] = new SourceGrid.Cells.ColumnHeader("Code");
            LOVGrid[0, 4] = new SourceGrid.Cells.ColumnHeader("Country");
            LOVGrid[0, 5] = new SourceGrid.Cells.ColumnHeader("Substring");
            LOVGrid[0, 0].Column.Width = 40;
            LOVGrid[0, 1].Column.Width = 140;
            LOVGrid[0, 2].Column.Width = 50;
            LOVGrid[0, 3].Column.Width = 50;
            LOVGrid[0, 4].Column.Width = 150;
            LOVGrid[0, 5].Column.Width = 60;
        }

        //Fills the grid with data with the filter applied
        private void FillGrid()
        {

            try
            {
                LOVGrid.Rows.Clear();
                LOVGrid.Redim(drFound.Count()+1, 6);

                
                WriteHeader();

                for (int i = 1; i <= drFound.Count(); i++)
                {
                    
                    DataRow dr = drFound[i - 1];

                    LOVGrid[i, 0] = new SourceGrid.Cells.Cell(i.ToString());
                    

                    LOVGrid[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                    LOVGrid[i, 1].AddController(dblClick);

                    LOVGrid[i, 2] = new SourceGrid.Cells.Cell(dr["Symbol"].ToString());
                    LOVGrid[i, 2].AddController(dblClick);

                    LOVGrid[i, 3] = new SourceGrid.Cells.Cell(dr["Code"].ToString());
                    LOVGrid[i, 3].AddController(dblClick);

                    LOVGrid[i, 4] = new SourceGrid.Cells.Cell(dr["Country"].ToString());
                    LOVGrid[i, 4].AddController(dblClick);

                    LOVGrid[i, 5] = new SourceGrid.Cells.Cell(dr["Substring"].ToString());
                    LOVGrid[i, 5].AddController(dblClick);


                    //LOVGrid[i, 1].AddController(clk);
                }
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Filter();

        }

        //Filters the datatable with the parameter name
        private void Filter()
        {


            this.FilterString = "Name LIKE '" + txtName.Text + "%' AND Substring LIKE '" + txtSubstring.Text + "%' AND Code LIKE '" + txtCode.Text + "%' AND Symbol LIKE '" + txtSymbol.Text + "%' AND Country LIKE '" + txtCountry.Text + "%'";
            
            try
            {

                drFound = dTable.Select(this.FilterString);
            }
            catch (Exception ex)
            {
                throw;
            }
            //LOVGrid.Rows.Clear();
            FillGrid();
        }

        private void txtSymbol_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtSubstring_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtCountry_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }


        //An event which is revoked when a cell is double clicked. 
        private void LOVGrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                //Get the Selected Row
                int CurRow = LOVGrid.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cell = new SourceGrid.CellContext(LOVGrid, new SourceGrid.Position(CurRow, 3));

                //Call the interface function to add the text in the parent form container
                m_ParentForm.AddCurrency(cell.Value.ToString());
                this.Dispose();
            }
            catch (Exception ex)
            {
                Global.Msg("Invalid selection");
            }
        }


        //When enter key is pressed
        private void LOVGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
                LOVGrid_DoubleClick(sender, null);
        }

        private void frmLOVCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }


    }
}
