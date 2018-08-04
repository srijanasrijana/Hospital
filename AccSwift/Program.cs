using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using AccSwift;
//using AccSwiftPOS;

namespace Inventory
{
    static class Program
    {
        static String _mutexID = "a8b65a4f-9ffb-46fd-a432-bdd3338c423e";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Boolean _isNotRunning;
            Mutex _mutex = new Mutex(true, _mutexID, out _isNotRunning);
            if (_isNotRunning)
            {
                Application.Run(new MDIMain());
                //Application.Run(new POSLogIn.frmposlogin());
             
            }
            else
            {
                MessageBox.Show("An instance is already running.");
                return;
            }
            //Application.Run(new MDIMain());
        }
    }
}
