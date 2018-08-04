using System.Windows.Forms;
using System.Linq;

namespace AccSwift
{
    public partial class frmSplashScreen : Form
    {
        private delegate void ProgressDelegate(int progress);

        private ProgressDelegate del;
        public frmSplashScreen()
        {
            InitializeComponent();
            this.progressBar1.Maximum = 100;
            del = this.UpdateProgressInternal;
        }

        private void UpdateProgressInternal(int progress)
        {
            if (this.Handle == null)
            {
                return;
            }

            this.progressBar1.Value = progress;
        }

        public void UpdateProgress(int progress)
        {
            this.Invoke(del, progress);
        }

        private void SplashScreen_Load(object sender, System.EventArgs e)
        {

        }

        private void frmSplashScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
