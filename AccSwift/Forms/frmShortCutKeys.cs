using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inventory;

namespace AccSwift
{
    public partial class frmShortCutKeys : Form
    {
        public frmShortCutKeys()
        {
            InitializeComponent();
        }

        private void frmShortCutKeys_Load(object sender, EventArgs e)
        {

            //lstShortcutKeys.View = View.Details;
            lstShortcutKeys.GridLines = true;
            lstShortcutKeys.FullRowSelect = true;
            
            //for(int i=1;i<100;i++)
            //{
                //List<string> shortcutvalues = new List<string>();
                //shortcutvalues.Add(i.ToString());
                //shortcutvalues.Add("Ctrl+Alt+T");
                //shortcutvalues.Add("To show Trial Balance Settings Form");

                string[] arr = new string[3];
                ListViewItem itm;
                arr[0] = 1.ToString();
                arr[1] = "Ctrl+Alt+T";
                arr[2] = "To show Trial Balance Settings Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);
                
                arr[0] = 2.ToString();
                arr[1] = "Ctrl+N";
                arr[2] = "Press New Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 3.ToString();
                arr[1] = "Ctrl+E";
                arr[2] = "Press Edit Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 4.ToString();
                arr[1] = "Ctrl+S";
                arr[2] = "Press Save Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 5.ToString();
                arr[1] = "Ctrl+Delete";
                arr[2] = "Press Delete Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 6.ToString();
                arr[1] = "Ctrl+F";
                arr[2] = "Press First Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 7.ToString();
                arr[1] = "Shift+P";
                arr[2] = "Press Previous Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 8.ToString();
                arr[1] = "Shift+N";
                arr[2] = "Press Next Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 9.ToString();
                arr[1] = "Ctrl+L";
                arr[2] = "Press Last Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 10.ToString();
                arr[1] = "Ctrl+C";
                arr[2] = "Press Copy Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 10.ToString();
                arr[1] = "Ctrl+V";
                arr[2] = "Press Paste Button";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 11.ToString();
                arr[1] = "Ctrl+P";
                arr[2] = "For Printing ";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 12.ToString();
                arr[1] = "Escape";
                arr[2] = "For Closing Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 13.ToString();
                arr[1] = "Ctrl+Alt+B";
                arr[2] = "To show Balance Sheet Settings Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 14.ToString();
                arr[1] = "Ctrl+Alt+P";
                arr[2] = "To show Profit And Loss Settings Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 15.ToString();
                arr[1] = "Ctrl+Alt+D";
                arr[2] = "To show DayBook Settings Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 16.ToString();
                arr[1] = "Ctrl+Alt+L";
                arr[2] = "To show Account Ledger Settings Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);

                arr[0] = 17.ToString();
                arr[1] = "Ctrl+Alt+C";
                arr[2] = "To show Cash Flow Settings Form";
                itm = new ListViewItem(arr);
                lstShortcutKeys.Items.Add(itm);
                

            //}



        }
    }
}
