using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Common;
using BusinessLogic;

namespace Common
{
    public partial class frmemail : Form
    {
       
        private System.Windows.Forms.OpenFileDialog dlgopenFile;
        private string SendEmail="";
        private string Attachementlocation = "";
        public frmemail()
        {
            InitializeComponent();    
        }
        public frmemail(IMDIMainForm frm)
        {
            frm.OpenFormArrayParam(frm.ToString());
            InitializeComponent(); 
        }
        public frmemail(string toemail)
        {
            SendEmail = toemail;
            InitializeComponent();
        }
        public frmemail(string attachement,int attachementtest)
        {
            Attachementlocation = attachement;
            InitializeComponent();
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            if(txtto.Text=="")
            {
                MessageBox.Show("Please Enter Receipant Address");
                return;
            }
            bool checkemail = UserValidation.validateEmail(txtto.Text);
            if (checkemail == false)
            {
                MessageBox.Show("Invalid Email ID","Check Email Address",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;

            }
            try
            {
                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("rameshp@bentraytech.com", "AccSwift");
                mail.From = new MailAddress(Global.useremail, "AccSwift");
               // mail.To.Add("rameshprajapati247@gmail.com");
                mail.To.Add(txtto.Text);
                mail.IsBodyHtml = true;
               // mail.Subject = "Registration";
                mail.Subject=txtsubject.Text;
               // mail.Body = "Some Text";
                mail.Body = txtcontents.Text;
                System.Net.Mail.Attachment attachment;
                string sFile;

                if (lstfiles.Items.Count > 0)
                {
                    for (int i = 0; i < lstfiles.Items.Count; i++)
                    {
                        lstfiles.SelectedIndex = i;
                        sFile = lstfiles.Text;
                        attachment = new System.Net.Mail.Attachment(sFile);
                        mail.Attachments.Add(attachment);
                    }
                }
                mail.Priority = MailPriority.High;

                //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                SmtpClient smtp = new SmtpClient(Global.mailserver,Convert.ToInt32( Global.serverport));
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new System.Net.NetworkCredential("rameshp@bentraytech.com", "ramesh1234");
                string DecryptPasswprd = Cryptography.Crypto.Decrypt(Global.password, "Ac104");
                smtp.Credentials = new System.Net.NetworkCredential(Global.useremail, DecryptPasswprd);
                smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(mail);
                MessageBox.Show("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n Please check the email address and Password properly in " + (Global.GlobalAccessRoleID == 37 ? "Tools>>Settings" : "Tools>>User Preference") + " and try again !");
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtcontents.Text = "";
            txtsubject.Text = "";
            txtto.Text = "";
            txtAttachment.Text = "";
            lstfiles.Items.Clear();
        }

        private void btnshowfile_Click(object sender, EventArgs e)
        {
            this.dlgopenFile = new System.Windows.Forms.OpenFileDialog();
            dlgopenFile.ShowDialog();
            txtAttachment.Text = dlgopenFile.FileName;
            lstfiles.Items.Add(txtAttachment.Text);
        }
        //public bool ValidateEmail(string sEmail)
        //{
        //    Regex exp = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        //    Match m = exp.Match(sEmail);

        //    if (m.Success && m.Value.Equals(sEmail)) return true;
        //    else return false;
        //}

        private void frmemail_Load(object sender, EventArgs e)
        {
            txtto.Text = SendEmail;
            txtAttachment.Text = Attachementlocation;
            if (txtAttachment.Text != "")
            {
                lstfiles.Items.Add(Attachementlocation);
            }
        }
    }
}
