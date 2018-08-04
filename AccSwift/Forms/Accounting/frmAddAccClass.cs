using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic;

namespace Inventory
{
  
    
    public interface ifrmAccClassID
    {
        void AddAccClassID(DataTable AccClassID);
    
    }
    public partial class frmAddAccClass : Form
    {

      
        private ifrmAccClassID m_ParentForm;//Holds the datatable
        private int RowID;
        private string VoucherType;
        public frmAddAccClass(Form ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (ifrmAccClassID)ParentForm;
        }

        public frmAddAccClass(Form ParentForm, int RowID,string VoucherType1)
        {
            InitializeComponent();
            m_ParentForm = (ifrmAccClassID)ParentForm;
            this.RowID = RowID;
            this.VoucherType = VoucherType1;

        }
        private void frmAddAccClass_Load(object sender, EventArgs e)
        {
            //SHOW  ALL THE ACOOUNT CLASS IN LISTBOX

            if (RowID > 0)
            {
                    #region Language Management



                    string LangField = "EngName";
                    switch (LangMgr.DefaultLanguage)
                    {
                        case Lang.English:
                            LangField = "EngName";
                            break;
                        case Lang.Nepali:
                            LangField = "NepName";
                            break;

                    }
                    #endregion
                    DataTable dtTransactClassInfo = AccountClass.GetTransactClassInfo(RowID, VoucherType);

                    DataTable dtAccClassInfo = AccountClass.GetAccClassTable(-1);

                    for (int i1 = 0; i1 < dtAccClassInfo.Rows.Count; i1++)
                    {
                        DataRow drAccClassInfo = dtAccClassInfo.Rows[i1];

                        ListItem liItem = new ListItem();
                        liItem.ID = Convert.ToInt32(drAccClassInfo["AccClassID"]);
                        liItem.Value = drAccClassInfo[LangField].ToString();

                        bool bAvail = false;

                        foreach(DataRow drTransactClassInfo in dtTransactClassInfo.Rows)
                        {
                            
                            if (Convert.ToInt32(drTransactClassInfo["AccClassID"]) == Convert.ToInt32(drAccClassInfo["AccClassID"]))
                            {


                                lbRightAccClass.Items.Add(liItem);
                                bAvail = true;
                            }
                        }

                        if (!bAvail)
                        {
                            
                            lbLeftAccClass.Items.Add(liItem);
                        }


                    }
                
                lbLeftAccClass.DisplayMember = "value";
                lbLeftAccClass.ValueMember = "id";

               
              

            }
            else
            {

                #region Language Management



                string LangField = "EngName";
                switch (LangMgr.DefaultLanguage)
                {
                    case Lang.English:
                        LangField = "EngName";
                        break;
                    case Lang.Nepali:
                        LangField = "NepName";
                        break;

                }
                #endregion

                DataTable dtAccClassInfo = AccountClass.GetAccClassTable(-1);
                for (int i = 0; i < dtAccClassInfo.Rows.Count; i++)
                {
                    DataRow drAccClassInfo = dtAccClassInfo.Rows[i];


                    lbLeftAccClass.Items.Add(new ListItem((int)drAccClassInfo["AccClassID"], drAccClassInfo[LangField].ToString()));


                }
                lbLeftAccClass.DisplayMember = "value";
                lbLeftAccClass.ValueMember = "id";
            }


  
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbLeftAccClass.SelectedItems.Count>0)
                {
                    lbRightAccClass.Items.Add(lbLeftAccClass.SelectedItem);
                
                    lbLeftAccClass.Items.Remove(lbLeftAccClass.SelectedItem);
                }
                else
                {
                    MessageBox.Show("Please select at least one item in the list");
               
                }
           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            try
            {
                if (lbRightAccClass.SelectedItems.Count > 0)
                {
                    lbLeftAccClass.Items.Add(lbRightAccClass.SelectedItem);
                    lbRightAccClass.Items.Remove(lbRightAccClass.SelectedItem);
                }
                else 
                {
                    MessageBox.Show("Please select at least one item in the list");
                
                
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

                ListItem LiAccClassID = new ListItem();
                LiAccClassID = (ListItem)lbRightAccClass.SelectedItem;
        
                DataTable dtAccClassID = new DataTable();
             
                dtAccClassID.Columns.Add("AccClassID", typeof(int));

                foreach (ListItem li in lbRightAccClass.Items)
                {

                   dtAccClassID.Rows.Add (li.ID.ToString());
              
                }

                //Call the interface function to add the text in the parent form container
                m_ParentForm.AddAccClassID(dtAccClassID);
                this.Dispose();
         
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           



      
           
        }
    }
}
