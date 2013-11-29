using System;
using System.Windows.Forms;

namespace TestEmail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            txtSenderEmail.Text = "afcm.myob01@outlook.com";
            txtSubject.Text = "This is from Testing";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                var sendEmail = new SendEmail();
                sendEmail.Load();

                sendEmail.SendTestEmailMessage(txtSenderEmail.Text, txtReceivedEmail.Text, txtSubject.Text);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var sendEmail = new SendEmail();
                
                sendEmail.Load();

                this.txtResults.Text = sendEmail.GetInformation(chkEmailAcctOnly.Checked).ToString();

                //sendEmail.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDefaultEmailSend_Click(object sender, EventArgs e)
        {
            try
            {
                var sendEmail = new SendEmail();
                sendEmail.Load();

                sendEmail.SendDefaultTestEmailMessage(txtSenderEmail.Text, txtReceivedEmail.Text, txtSubject.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnVerifySenderEmail_Click(object sender, EventArgs e)
        {
            try
            {
                var sendEmail = new SendEmail();
                sendEmail.Load();

                var senderEmail = txtSenderEmail.Text;
                if (sendEmail.IsDefaultEmailAddress(senderEmail))
                    MessageBox.Show(string.Format("Sender's email '{0}' is default email in Outlook",senderEmail));
                else if (sendEmail.IsDelegateForEmailAddress(senderEmail))
                {
                    var defaultEmailAddress = sendEmail.GetDefaultEmailAddress();
                    MessageBox.Show(string.Format("Sender's email '{0}' is delegrated to current user (email : {1}) in Outlook", senderEmail, defaultEmailAddress));
                }
                else if (sendEmail.IsSharedMailBoxAccountEmailAddress(senderEmail))
                {
                    MessageBox.Show(string.Format("Sender's email '{0}' is shared mail box email account", senderEmail));
                }
                else
                {
                    if (sendEmail.SetToSenderEmailProfile(txtSenderEmail.Text))
                    {
                        MessageBox.Show(string.Format("Sender's email '{0}' found in profile('{1}')", senderEmail, sendEmail.ProfileName));
                    }
                    else
                    {
                        string errorMessage =
                            string.Format( "Error : Sender's email '{0}' could not found in any profile\n\n", senderEmail);
                        errorMessage += "Sender's email address is Not default email addresses.\n\n";
                        errorMessage += "Sender's email address is Not 'delegrated for' email addresses.\n\n";
                        errorMessage +=
                            "Please check if the email account name in Control Panel\\Mail Setup is the same as sender's email address!!!";
                        MessageBox.Show(errorMessage);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnTestProfMan_Click(object sender, EventArgs e)
        {
        }

        private void btnVerifyOnBehafOf_Click(object sender, EventArgs e)
        {

        }

        private void btnResolve_Click(object sender, EventArgs e)
        {
            try
            {
                var sendEmail = new SendEmail();
                sendEmail.Load();

                sendEmail.ResolveEmailAddress(txtSenderEmail.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

   
    }
}
