using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

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

                sendEmail.SendEmailMessageForTesting(txtSenderEmail.Text, txtReceivedEmail.Text, txtSubject.Text);
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
            Text = string.Format("{0} - Version {1}", Text, Assembly.GetExecutingAssembly().GetName().Version.ToString());
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
                    MessageBox.Show(string.Format("Sender's email '{0}' is default email in Outlook", senderEmail));
                else if (sendEmail.IsDelegateForEmailAddress(senderEmail))
                {
                    var defaultEmailAddress = sendEmail.GetDefaultEmailAddress();
                    MessageBox.Show(
                        string.Format("Sender's email '{0}' is delegrated to current user (email : {1}) in Outlook",
                                      senderEmail, defaultEmailAddress));
                }
                else if (sendEmail.IsExchangeSharedMailBoxAccountEmailAddress(senderEmail))
                {
                    MessageBox.Show(string.Format("Sender's email '{0}' is shared exchange mail box email account",
                                                  senderEmail));
                }
                else if (sendEmail.FindGoogleAppsEmailAccountBySenderEmailAddress(senderEmail) != null)
                {
                    MessageBox.Show(string.Format("Sender's email '{0}' is Google Apps email account", senderEmail));
                }
                else
                {
                    SenderEmailAccountType senderEmailAccountType;
                    if (sendEmail.SetToSenderEmailProfile(txtSenderEmail.Text, out senderEmailAccountType))
                    {
                        MessageBox.Show(string.Format("Sender's email '{0}' found in profile('{1}')", senderEmail,
                                                      sendEmail.ProfileName));
                    }
                    else
                    {
                        string errorMessage =
                            string.Format("Error : Sender's email '{0}' could not found in any profile\n\n", senderEmail);
                        errorMessage += "Sender's email address is Not default email addresses.\n\n";
                        errorMessage += "Sender's email address is Not Exhcnage shared mail box email addresses.\n\n";
                        errorMessage += "Sender's email address is Not 'delegrated for' email addresses.\n\n";
                        errorMessage += "Sender's email address is Not 'Google App' email addresses.\n\n";
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            Get64BiitegistryTest();
            /*
                        var mailClient = GetCurrentUserValue(@"SOFTWARE\Clients\Mail", "");
                        MessageBox.Show("Default Mail Client = " + mailClient);

                        if (string.IsNullOrEmpty(mailClient))
                        {    
                            mailClient = GetHklmValue(@"SOFTWARE\Clients\Mail", "");
                            MessageBox.Show("Default Mail Client (hklm) = " + mailClient);
                        }

                        MessageBox.Show("Email Client Bitness (OutLook 2010) = " + GetOutlookBitness("14.0"));
                        MessageBox.Show("Email Client Bitness (OutLook 2013) = " + GetOutlookBitness("15.0"));
             */

        }

        public string GetEmailApplicationExecutable(string name)
        {
            return GetHklmValue(string.Format(@"SOFTWARE\Clients\Mail\{0}\shell\open\command", name), "");
        }

        public string GetCurrentUserValue(string keyPath, string keyName)
        {
            if (string.IsNullOrEmpty(keyPath.Trim()))
                return string.Empty;
            try
            {
                var registryKey = Registry.CurrentUser.OpenSubKey(keyPath);
                return registryKey != null ? registryKey.GetValue(keyName.Trim()).ToString() : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetHklmValue(string keyPath, string keyName)
        {
            MessageBox.Show(string.Format(" 1. GetHklmValue, keyPath = {0}, keyName= {1}", keyPath, keyName));
            if (string.IsNullOrEmpty(keyPath.Trim()))
                return string.Empty;
            try
            {
                MessageBox.Show(string.Format(" 2. GetHklmValue, keyPath = {0}, keyName= {1}", keyPath, keyName));
                var registryKey = Registry.LocalMachine.OpenSubKey(keyPath);
                MessageBox.Show(string.Format(" 3. GetHklmValue, keyPath = {0}, keyName= {1}", keyPath, keyName));
                //var result =  registryKey != null ? registryKey.GetValue(keyName.Trim()).ToString() : string.Empty;

                string result;
                if (registryKey != null)
                {
                    result = registryKey.GetValue(keyName.Trim()).ToString();
                    MessageBox.Show(string.Format(" 3.1. GetHklmValue, result = {0}", result));
                }
                else
                {
                    result = string.Empty;
                    MessageBox.Show(string.Format(" 3.2. GetHklmValue, result = {0}", result));
                }
                MessageBox.Show(string.Format(" 4. GetHklmValue, result = {0}", result));
                return result;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return string.Empty;
            }
        }

        private bool IsOutlook64Bit()
        {
            var outlookBitness = GetOutlookBitness().Trim();
            if (string.IsNullOrEmpty(outlookBitness)) return false;

            return outlookBitness == "x64";
        }

        private string GetOutlookBitness()
        {
            var outlook2010 = GetOutlookBitness("14.0");
            if (!string.IsNullOrEmpty(outlook2010)) return outlook2010;

            var outlook2013 = GetOutlookBitness("15.0");
            return !string.IsNullOrEmpty(outlook2013) ? outlook2013 : string.Empty;
        }

        private string GetOutlookBitness(string version)
        {
            var officeBitness =
                GetHklmValue(string.Format(@"SOFTWARE\Wow6432Node\Microsoft\Office\{0}\Outlook", version), "Bitness");

            return string.IsNullOrEmpty(officeBitness) ? string.Empty : officeBitness;
        }


        private string Get64BiitegistryTest()
        {
            //var data = GetHklmValue(@"SOFTWARE\Alps\Apoint", "HomeDirectory");


            //MessageBox.Show(data);

            //var data2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Alps\Apoint", "HomeDirectory", "test");

            //MessageBox.Show(data2 == null ? "No value" : data2.ToString());

            var keyName = @"SOFTWARE\Alps\Apoint";
            var propertyName = "HomeDirectory";


            string value64 = RegistryWOW6432.GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, keyName, propertyName);
            string value32 = RegistryWOW6432.GetRegKey32(RegHive.HKEY_LOCAL_MACHINE, keyName, propertyName);

            return value64;

        }


    }
}
