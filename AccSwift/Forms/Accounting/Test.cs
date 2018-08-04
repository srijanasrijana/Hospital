using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using BusinessLogic;
using System.Data;
using Inventory.DataSet;
using System.Xml.Serialization;

namespace Inventory.Forms.Accounting
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void AddGridHeader()
        {
            grdJournal[0, 0] = new SourceGrid.Cells.ColumnHeader("Sno");
            grdJournal[0, 1] = new SourceGrid.Cells.ColumnHeader("LedgerName");
            grdJournal[0, 2] = new SourceGrid.Cells.ColumnHeader("Amount");           
            grdJournal[0, 0].Column.Width = 50;
            grdJournal[0, 1].Column.Width = 200;
            grdJournal[0, 2].Column.Width = 60;         
        }

        private void AddRowJournal(int RowCount)
        {
            //Add a new row
            grdJournal.Redim(Convert.ToInt32(RowCount + 1), grdJournal.ColumnsCount);           
            int i = RowCount;
            grdJournal[i, 0] = new SourceGrid.Cells.Cell(i.ToString());          
            grdJournal[i, 1] = new SourceGrid.Cells.Cell(i.ToString());
            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus;
            grdJournal[i, 2] = new SourceGrid.Cells.Cell("", txtAccount);
            grdJournal[i, 2].Value = "(NEW)";
        }

        private void Test_Load(object sender, EventArgs e)
        {
            frmLOVLedger frm = new frmLOVLedger();
            frm.ShowDialog();
            return;

            grdJournal.Redim(1, 3);
            AddGridHeader();
            AddRowJournal(1);
            AddRowJournal(2);

            string strEmpDetails = TestOnlyJournalMasterWithXMLType();
            //string strEmpDetails =  TestOnlyJournalMasterWithPropertyXMLType();
            //return;
            //Database connection string
            string strConString =
              @"Data Source=RAMU\SQLSERVER;Integrated Security=false;Initial Catalog=AccSwift_v1.10; uid = sa; password = a;";

            using (StringWriter swStringWriter = new StringWriter())
            {
                //// Emp details datatable â€“ ADO.NET DataTable 
                //DataTable dtEmpDetails = GetTable();
                //dtEmpDetails.TableName = "Test";
                //// Datatable as XML format 
                //dtEmpDetails.WriteXml(swStringWriter);               
                //// Datatable as XML string 
                //string strEmpDetails = swStringWriter.ToString();
                //MessageBox.Show(strEmpDetails);
                //return;

                using (SqlConnection dbConnection = new SqlConnection(strConString))
                //Create database connection  
                {
                    // Database command with stored - procedure  
                    using (SqlCommand dbCommand =
                           new SqlCommand("Acc.xmlJournalInsert", dbConnection))
                    {
                        // we are going to use store procedure  
                        dbCommand.CommandType = CommandType.StoredProcedure;
                        // Add input parameter and set its properties.
                        SqlParameter parameter = new SqlParameter();
                        // Store procedure parameter name  
                        parameter.ParameterName = "@journal";
                        // Parameter type as XML 
                        parameter.DbType = DbType.Xml;
                        parameter.Direction = ParameterDirection.Input; // Input Parameter  
                        parameter.Value = strEmpDetails; // XML string as parameter value  
                        // Add the parameter in Parameters collection.
                        dbCommand.Parameters.Add(parameter);
                        dbConnection.Open();
                        int intRetValue = dbCommand.ExecuteNonQuery();
                        MessageBox.Show(intRetValue.ToString());
                    }
                }
            }
        }

        private DataTable GetEmpDetails()
        {
            DataTable dtEmpDetails = new DataTable("EmpDetails");
            // Emp table and its schema 

            dtEmpDetails.Columns.Add(new DataColumn("EmpName",
                         Type.GetType("System.String"))); //EmpName Colume 

            dtEmpDetails.Columns.Add(new DataColumn("EmpAddress",
                         Type.GetType("System.String"))); //EmpAddress Colume

            DataRow drRow = dtEmpDetails.NewRow(); // First Employee  
            drRow["EmpName"] = "Emp-1";
            drRow["EmpAddress"] = "Emp1-Addr-1";
            dtEmpDetails.Rows.Add(drRow);
            drRow = dtEmpDetails.NewRow(); // Second Employee  
            drRow["EmpName"] = "Emp-2";
            drRow["EmpAddress"] = "Emp2-Addr-2";
            dtEmpDetails.Rows.Add(drRow);
            return dtEmpDetails;
        }

        /// <summary>
        /// This example method generates a DataTable.
        /// </summary>
        private DataTable GetTable()
        {
            ////Journal_DetailID, JournalID, LedgerID, Amount, DrCr,Remarks
            DataTable table = new DataTable();
            table.Columns.Add("Journal_DetailID", typeof(int));
            table.Columns.Add("JournalID", typeof(int));
            table.Columns.Add("LedgerID", typeof(int));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("DrCr", typeof(string));
            table.Columns.Add("Remarks", typeof(string));           

            FormHandle m_FHandle = new FormHandle();
            for (int i = 0; i < grdJournal.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
            {               
            table.Rows.Add(22, 1,24,2000,"Dr","JNL Detail");
                    //table.Rows.Add(grdJournal[i + 1, 0].ToString(), grdJournal[i + 1, 1].ToString(), grdJournal[i + 1, 2].ToString());                
            }
            return table;
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{

        //    DataTable dt = _customers.GetCustomers();

        //    dataGridView1.DataSource = dt;

        //}



        //private void btnExportXML_Click(object sender, EventArgs e)
        //{

        //    DataTable dt = new DataTable();

        //    dt = (DataTable)grdJournal.DataSource;

        //    DataSet ds = new DataSet();

        //    ds.Tables.Add(dt);

        //    ds.WriteXml("D:\\Customers.xml", System.Data.XmlWriteMode.IgnoreSchema);

        //}

        private string TestOnlyJournalMasterWithXMLType()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode; 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //System.Xml.XmlTextWriter tw =
            //  new System.Xml.XmlTextWriter(ms, new System.Text.UTF32Encoding());// System.Text.ASCIIEncoding());
            System.Xml.XmlTextWriter tw =
             new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            tw.WriteStartElement("JOURNAL");
            {
                //int JournalID = System.Convert.ToInt32(9);
                int SeriesID = System.Convert.ToInt32(284);               
                string Voucher_No = System.Convert.ToString("VN1");
                DateTime Journal_Date = System.Convert.ToDateTime(DateTime.Now);
                string Remarks = System.Convert.ToString("Master Remarks");
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Master");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Master");

                tw.WriteStartElement("JOURNALMASTER");
                //JournalID, SeriesID, Voucher_No, Journal_Date,Remarks,Created_By,Created_Date,Modified_By,Modified_Date
                //tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("Journal_Date", Journal_Date.ToString());
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("Created_Date", Created_Date.ToString());
                 tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Modified_Date.ToString());
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();

                ////Journal_DetailID, JournalID, LedgerID, Amount, DrCr,Remarks
                //int Journal_DetailID;
                //Decimal Amount;
                //int LedgerID;
                //String DrCr;
                //string remarks;
                int JournalID = System.Convert.ToInt32(288);
                int LedgerID = System.Convert.ToInt32(320);
                Decimal Amount = System.Convert.ToDecimal(550);
                string DrCr = System.Convert.ToString("Detail");
                string RemarksD = System.Convert.ToString("Detail");
              

                tw.WriteStartElement("JOURNALDETAIL");
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("RemarksD", RemarksD.ToString());
                tw.WriteEndElement();

                JournalID = System.Convert.ToInt32(289);
                //LedgerID = System.Convert.ToInt32(320);
                Amount = System.Convert.ToDecimal(500);
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("RemarksD", RemarksD.ToString());
                tw.WriteEndElement();

                tw.WriteEndElement();

                //System.Data.DataSet dsEmpDetails = new System.Data.DataSet("JOURNALDETAIL");
                //DataTable dtEmpDetails = GetTable();
                //dtEmpDetails.TableName = "DETAIL";
                //dsEmpDetails.Tables.Add(dtEmpDetails);
                //// Datatable as XML format 
                //dsEmpDetails.WriteXml(tw);
                
                //for (int i = 0; i < grdJournal.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
                //{
                    ////Journal_DetailID, JournalID, LedgerID, Amount, DrCr,Remarks
                    //int Journal_DetailID;
                    //Decimal Amount;
                    //int LedgerID;
                    //String DrCr;
                    //string remarks;
                    // tw.WriteStartElement("JOURNALDETAIL");
                    // [XmlAttribute]



               //      [XmlAttribute]
               //public int ProductID
               //{
               //     get
               //     {
               //          return this.PID;
               //     }
               //     set 
               //     {
               //          PID=value;
               //     }
               //}


                //    table.Rows.Add(grdJournal[i + 1, 0].ToString(), grdJournal[i + 1, 1].ToString(), grdJournal[i + 1, 2].ToString());
                //}
                

            }

            tw.WriteFullEndElement();
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();

            string strXML = AEncoder.GetString(ms.ToArray());

            //strXML = strXML.Replace("<DocumentElement>", "<JOURNALDETAIL>");
            //strXML = strXML.Replace("</DocumentElement>", "<JOURNALDETAIL>");

           // MessageBox.Show(strXML);

            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UploadByXMLType";
            //cmd.Parameters.Add("@DataInXMLType", SqlDbType.Text).Value = strXML;
            //cmd.CommandType = CommandType.StoredProcedure;
            //DataTable aTable = SQLServerUtility.GetADataTable(cmd);
            return strXML;

           // return aTable;
        }

        private string TestOnlyJournalMasterWithPropertyXMLType()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //System.Xml.XmlTextWriter tw =
            //  new System.Xml.XmlTextWriter(ms, new System.Text.UTF32Encoding());// System.Text.ASCIIEncoding());
            System.Xml.XmlTextWriter tw =
             new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            tw.WriteStartElement("JOURNAL");
            {
                int JournalID = System.Convert.ToInt32(9);
                int SeriesID = System.Convert.ToInt32(24);
                string Voucher_No = System.Convert.ToString("VN1");
                DateTime Journal_Date = System.Convert.ToDateTime(DateTime.Now);
                string Remarks = System.Convert.ToString("Master Remarks");
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Master");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Master");

                tw.WriteStartElement("JOURNALMASTER");
                //JournalID, SeriesID, Voucher_No, Journal_Date,Remarks,Created_By,Created_Date,Modified_By,Modified_Date
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("Journal_Date", Journal_Date.ToString());
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("Created_Date", Created_Date.ToString());
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Modified_Date.ToString());
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();

                //int IDD = System.Convert.ToInt32(4);
                //string NameD = System.Convert.ToString("Detail");
                //int ScoreD = System.Convert.ToInt32(55);

                //tw.WriteStartElement("JOURNALDETAIL");
                //tw.WriteElementString("ID", ID.ToString());
                //tw.WriteElementString("Name", Name);
                //tw.WriteElementString("Score", Score.ToString());
                //tw.WriteEndElement();

                //System.Data.DataSet dsEmpDetails = new System.Data.DataSet("JOURNALDETAIL");
                //DataTable dtEmpDetails = GetTable();
                //dtEmpDetails.TableName = "DETAIL";
                //dsEmpDetails.Tables.Add(dtEmpDetails);
                //// Datatable as XML format 
                //dsEmpDetails.WriteXml(tw);

                //for (int i = 0; i < grdJournal.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
                //{
                ////Journal_DetailID, JournalID, LedgerID, Amount, DrCr,Remarks
                //int Journal_DetailID;
                //Decimal Amount;
                //int LedgerID;
                //String DrCr;
                //string remarks;
                // tw.WriteStartElement("JOURNALDETAIL");
                // [XmlAttribute]



                //      [XmlAttribute]
                //public int ProductID
                //{
                //     get
                //     {
                //          return this.PID;
                //     }
                //     set 
                //     {
                //          PID=value;
                //     }
                //}


                //    table.Rows.Add(grdJournal[i + 1, 0].ToString(), grdJournal[i + 1, 1].ToString(), grdJournal[i + 1, 2].ToString());
                //}


            }

            tw.WriteFullEndElement();
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();

            string strXML = AEncoder.GetString(ms.ToArray());

            //strXML = strXML.Replace("<DocumentElement>", "<JOURNALDETAIL>");
            //strXML = strXML.Replace("</DocumentElement>", "<JOURNALDETAIL>");

           // MessageBox.Show(strXML);

            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UploadByXMLType";
            //cmd.Parameters.Add("@DataInXMLType", SqlDbType.Text).Value = strXML;
            //cmd.CommandType = CommandType.StoredProcedure;
            //DataTable aTable = SQLServerUtility.GetADataTable(cmd);
            return strXML;

            // return aTable;
        }


        /// <summary>
        /// This method will convert the supplied DataTable 
        /// to XML string.
        /// </summary>
        /// <param name="dtBuildSQL">DataTable to be converted.</param>
        /// <returns>XML string format of the DataTable.</returns>
        private static string ConvertDataTableToXML(DataTable dtBuildSQL)
        {
            System.Data.DataSet dsBuildSQL = new System.Data.DataSet();
            StringBuilder sbSQL;
            StringWriter swSQL;
            string XMLformat;
            try
            {
                sbSQL = new StringBuilder();
                swSQL = new StringWriter(sbSQL);
                dsBuildSQL.Merge(dtBuildSQL, true, MissingSchemaAction.AddWithKey);
                dsBuildSQL.Tables[0].TableName = "SampleDataTable";
                foreach (DataColumn col in dsBuildSQL.Tables[0].Columns)
                {
                    col.ColumnMapping = MappingType.Attribute;
                }
                dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
                XMLformat = sbSQL.ToString();
                return XMLformat;
            }
            catch (Exception sysException)
            {
                throw sysException;
            }
        }

        private void CollectDetail()
        {

            for (int i = 0; i < grdJournal.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
            {
              

            }
            ////Create a datatable based on the gridview
            //DataTable dt = (DataTable)grdJournal.DataSource;
	 
            ////Header row - add columns
            //if (grdJournal.HeaderRow != null)
            //{
	 
            //    for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
            //    {
            //        dt.Columns.Add(GridView1.HeaderRow.Cells[i].Text);
            //    }
            //}
	 
            ////fill the rows
            //DataRow dr;
            //foreach (GridViewRow row in GridView1.Rows)
            //{
            //    dr = dt.NewRow();
	 
            //    for (int i = 0; i < row.Cells.Count; i++)
            //    {
            //        dr[i] = row.Cells[i].Text.Replace("&nbsp;", "");
            //    }
            //    dt.Rows.Add(dr);
            //}

            //<CollectDetail  jour
            //Create a new XML doc
            //give it a name and location
            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.Load(HttpContext.Current.Server.MapPath("directory.xml"));

            //foreach (DataRow row in dt.Rows)
            //{
            //    //create the element
            //    XmlElement newDept = xmldoc.CreateElement("Department");

            //    //Create an attribute
            //    XmlAttribute attrDept = xmldoc.CreateAttribute("dept");

            //    //tell the attribute what data to populate
            //    attrDept.Value = row["dept"].ToString();
            //    //Set Attribute
            //    newDept.SetAttributeNode(attrDept);

            //    //First node and data source
            //    XmlElement nameNode = xmldoc.CreateElement("name"); newDept.AppendChild(nameNode);
            //    nameNode.InnerText = row["name"].ToString();

            //    //Second node and data source
            //    XmlElement titleNode = xmldoc.CreateElement("title");
            //    newDept.AppendChild(titleNode);
            //    titleNode.InnerText = row["title"].ToString();

            //    //Third node and data source
            //    XmlElement NumberNode = xmldoc.CreateElement("number");
            //    newDept.AppendChild(NumberNode);
            //    NumberNode.InnerText = row["number"].ToString();

            //    //write to the xml document
            //    xmldoc.DocumentElement.InsertAfter(newDept, xmldoc.DocumentElement.LastChild);
            //}

            //xmldoc.Save(HttpContext.Current.Server.MapPath("directory.xml"));

        }

    }
}
