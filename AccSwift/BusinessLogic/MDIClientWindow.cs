using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using BusinessLogic;

namespace BusinessLogic
{

    public interface IMDIClientNotify
    {
        void WndProc(ref Message m, ref bool doDefault);
    }

    /// <summary>
    /// Summary description for MDIClientWindow.
    /// </summary>
    public class MDIClientWindow : NativeWindow
    {

        #region Delegates
        private delegate int EnumWindowsProc(IntPtr hwnd, int lParam);
        private IMDIClientNotify notify = null;
        #endregion

        #region UnManagedMethods
        private class UnManagedMethods
        {
            [DllImport("user32")]
            public extern static int EnumWindows(
                EnumWindowsProc lpEnumFunc,
                int lParam);
            [DllImport("user32")]
            public extern static int EnumChildWindows(
                IntPtr hWndParent,
                EnumWindowsProc lpEnumFunc,
                int lParam);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static int GetClassName(
                IntPtr hWnd,
                StringBuilder lpClassName,
                int nMaxCount);
        }
        #endregion

        #region Member variables
        private IntPtr hWndMdiClient = IntPtr.Zero;
        #endregion

        #region EnumWindows Code
        /// <summary>
        /// Gets all child windows of the specified window
        /// </summary>
        /// <param name="hWndParent">Window Handle to get children for</param>
        private void GetWindows(
            IntPtr hWndParent)
        {
            UnManagedMethods.EnumChildWindows(
                hWndParent,
                new EnumWindowsProc(this.WindowEnum),
                0);
        }

        /// <summary>
        /// The enum Windows callback.
        /// </summary>
        /// <param name="hWnd">Window Handle</param>
        /// <param name="lParam">Application defined value</param>
        /// <returns>1 to continue enumeration, 0 to stop</returns>
        private int WindowEnum(
            IntPtr hWnd,
            int lParam)
        {
            StringBuilder className = new StringBuilder(260, 260);
            UnManagedMethods.GetClassName(hWnd, className, className.Capacity);
            if (className.ToString().ToUpper().IndexOf("MDICLIENT") > 0)
            {
                // stop
                hWndMdiClient = hWnd;
                return 0;
            }
            else
            {
                // continue
                return 1;
            }
        }
        #endregion

        protected override void WndProc(ref Message m)
        {
            bool doDefault = true;
            if (notify != null)
            {
                notify.WndProc(ref m, ref doDefault);
            }
            if (doDefault)
            {
                base.WndProc(ref m);
            }
        }

        public MDIClientWindow(IMDIClientNotify i, IntPtr handle)
        {
            // Find the MDI Client window handle:
            GetWindows(handle);
            if (hWndMdiClient != IntPtr.Zero)
            {
                this.AssignHandle(hWndMdiClient);
            }
            this.notify = i;
        }
    }
}
