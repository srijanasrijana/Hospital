using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//New added to Acc_swift for to print in dot matrix printer
//Created By :Bimal Khadka

namespace AccSwift.Forms
{
    public partial class Printer : Form
    {
        public Printer()
        {
            InitializeComponent();
        }
        public string PrinterName;
        private void Printer_Load(object sender, EventArgs e)
        {
            foreach (String printer in PrinterSettings.InstalledPrinters)
            {

                cboPrinterName.Items.Add(printer.ToString());

            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            PrinterName = cboPrinterName.SelectedItem.ToString();
            this.Close();
        }
    }
}
