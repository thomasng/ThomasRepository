using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Redemption;

namespace TestEmail
{
    public enum EmailPriority
    {
        Low = 0,
        Default = 1,
        High = 2
    }

    public class SendEmail
    {

        private const int AccountCategoryMail = 2;
        private const int AccountCategoryMailAndStore = 3;

        public MailAddress FromAddress { get; set; }
        public List<MailAddress> ToRecipients { get; set; }
        public List<MailAddress> CcRecipients { get; set; }
        public List<MailAddress> BccRecipients { get; set; }

        public string ProfileName { get; set; }
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set { _subject = value.Replace(Environment.NewLine, ""); }
        }

        public string Body { get; set; }
        public string RtfBody { get; set; }
        public string HtmlBody { get; set; }

        public List<string> Attachments { get; set; }

        public EmailPriority Priority { get; set; }

        private RDOSession RdoSession { get; set; }
        private RDOMail Mail { get; set; }

        public void Load()
        {
            //check that the path exists
            var redemptionFilePath = RedemptionLoader.DllLocation32Bit;

            //MessageBox.Show("Load path = " + redemptionFilePath.ToString());
            if (!File.Exists(redemptionFilePath))
            {
                var currentPath = Assembly.GetExecutingAssembly().Location;
                currentPath = Path.GetDirectoryName(currentPath);
                if (currentPath != null)
                {
                    RedemptionLoader.DllLocation64Bit = Path.Combine(currentPath, "redemption64.dll");
                    RedemptionLoader.DllLocation32Bit = Path.Combine(currentPath, "redemption.dll");
                }
            }

            //MessageBox.Show("Redemption loaded");
            RdoSession = RedemptionLoader.new_RDOSession();

            //MessageBox.Show("Start Session logon");
            SessionLogon();
            //MessageBox.Show("Start Session logon done");

        }

        public void Close()
        {
            if (RdoSession.FastShutdownSupported)
                RdoSession.DoFastShutdown();
            else
                RdoSession.Logoff();
        }

        public void Clear()
        {
            Mail = null;
        }


        private void SessionLogon()
        {
            if (RdoSession.LoggedOn)                            
                RdoSession.Logoff();


            if (RdoSession.Profiles.Count == 0)
                throw new Exception("No Profile found");

            if (!string.IsNullOrEmpty(ProfileName))
                RdoSession.Logon(ProfileName, null, false, true, null, false);
            else
            {
                RdoSession.Logon(null, null, false, true, null, false);
                ProfileName = RdoSession.ProfileName;

            

                //int x = 3;

                //if (RdoSession.ExchangeConnectionMode > 0)
                //{
                //    var username = RdoSession.CurrentUser.Name;
                //    var serverName = RdoSession.ExchangeMailboxServerName;


                //    RdoSession.Logoff();

                //    RdoSession.LogonExchangeMailbox(username, serverName);

                //    var sharedDraftFolder = RdoSession.GetSharedDefaultFolder("christine.munyard@myob.com", rdoDefaultFolders.olFolderDrafts);

                //    var isDelegateFor = RdoSession.CurrentUser.IsDelegateFor;

                //    var sharedBox = RdoSession.GetSharedMailbox("christine.munyard@myob.com");

                
                //    int x = 3;

                //}
            }
        }

        private const int EmailAccountCategoryFlag = 2;

        /// <summary>
        /// check if RdoAccount is email account
        ///     Notes: AccountCategories is a combination of the rdoAccountCategory enums (acStore = 1, acMail = 2, acAddressBook = 4).
        /// </summary>
        /// <param name="rdoAccount"></param>
        /// <returns></returns>
        private static bool IsEmailAccountCategory(RDOAccount rdoAccount)
        {
            return (rdoAccount.AccountCategories & EmailAccountCategoryFlag) != 0;
        }



        public string LineText = "---------------------------------------------------";

        public StringBuilder GetInformation(bool emailAcctOnly)
        {

            StringBuilder resultText = new StringBuilder();


            
            // no of profiles
            resultText.AppendLine(string.Format("A. No of Profile : {0}", RdoSession.Profiles.Count));
            resultText.AppendLine(LineText);

            MessageBox.Show(resultText.ToString());

            resultText.AppendLine(string.Format("Default Profiles : {0}", RdoSession.Profiles.DefaultProfileName));



            int profileIndex = 0;
            foreach (string profileName in RdoSession.Profiles)
            {
                try
                {
                    resultText.AppendLine(LineText);
                    profileIndex++;
                    resultText.AppendLine(string.Format("B. Profile {1}: {0}", profileName, profileIndex));
                    resultText.AppendLine(string.Empty);



                    //MessageBox.Show(string.Format("Profile name = {0}", profileName));

                    if (ProfileName != profileName)
                    {
                        // switch to other email profile
                        ProfileName = profileName;

                        // logon to other profile
                        SessionLogon();

                    }

                    //MessageBox.Show(resultText.ToString());
                    try
                    {
                        if (RdoSession.CurrentWindowsUser != null)
                            resultText.AppendLine(string.Format("A1. Profile {0}: Windows User SMTPAddress : {1}",
                                                                profileName,
                                                                RdoSession.CurrentWindowsUser == null
                                                                    ? "None"
                                                                    : RdoSession.CurrentWindowsUser.SMTPAddress));
                        else
                            resultText.AppendLine("Window Current user is not found");

                    }
                    catch (Exception ex)
                    {
                        resultText.AppendLine("Windows current user is not supported.");
                        resultText.AppendLine(ex.Message);
                    }

                    var defaultEmail = GetDefaultEmailAddress();
                    //resultText.AppendLine(string.Format("A1. Profile {0}: Current User SMTPAddress : {1}", profileName, RdoSession.CurrentUser == null ? "None" : RdoSession.CurrentUser.SMTPAddress));
                    resultText.AppendLine(string.Format("A1. Profile {0}: Current User SMTPAddress : {1}", profileName, defaultEmail == null ? "None" : defaultEmail));
                    resultText.AppendLine(string.Empty);


                    //MessageBox.Show(string.Format("Logon done Profile name = {0}", profileName));

                    // all email accounts
                    resultText.AppendLine(string.Format("B1. Searching Emails Account in Profile : {0}", profileName));
                    resultText.AppendLine(string.Empty);
                    foreach (RDOAccount rdoAccount in RdoSession.Accounts)
                    {

                        if (emailAcctOnly)
                        {
                            if (!IsEmailAccountCategory(rdoAccount))
                                continue;
                        }

                        resultText.AppendLine(string.Format("** Email Account: {0}, Type = {1}, Category = {2}",
                                                            rdoAccount.Name, rdoAccount.AccountTypeStr,
                                                            rdoAccount.AccountCategories.ToString()));

   
                        switch (rdoAccount.AccountType)
                        {                                
                                case rdoAccountType.atPOP3:
                                {
                                    resultText.AppendLine(string.Format(" ----- Pop3 account, SMTPAddress = {0}, ",((IRDOPOP3Account) rdoAccount).SMTPAddress));
                                    break;
                                }
                                case rdoAccountType.atHTTP:
                                {
                                    resultText.AppendLine(string.Format(" ----- HTTPAccount account, SMTPAddress = {0}, ",((RDOHTTPAccount) rdoAccount).SMTPAddress));
                                    break;
                                }
                                case rdoAccountType.atIMAP:
                                {
                                    resultText.AppendLine(string.Format(" ----- atIMAP account, SMTPAddress = {0}, ",((RDOIMAPAccount) rdoAccount).SMTPAddress));
                                    break;
                                }
                                case rdoAccountType.atExchange:
                                {

                                    foreach( RDOStore store in RdoSession.Stores)
                                    {
                                        try
                                        {
                                            if (store.StoreKind == TxStoreKind.skPrimaryExchangeMailbox)
                                            {
                                                RDOExchangeMailboxStore exStore = (RDOExchangeMailboxStore)store;
                                                resultText.AppendLine("----- atExchange account, SMTPAddress" + exStore.Owner.SMTPAddress);
                                            }

                                        }
                                        catch (Exception ex)
                                        {

                                            //resultText.AppendLine(ex.Message);
                                        }
                                    }
                                    //RDOMAPIAccount acct = rdoAccount as RDOMAPIAccount;
                                    //var isExchange = acct.IsExchange;
                                    //var x1 = acct.ServiceName;
                                    //var x2 = acct.ServiceUID;
                                    //var x3 = acct.Name;
                                    //var x4 = acct.ID;
                                    //var x5 = acct.Stamp;
                                    //resultText.AppendLine(string.Format(" ----- atExchange account, SMTPAddress = {0}, ", ((RDOIMAPAccount)rdoAccount).SMTPAddress));
                                    break;
                                }
                                //case rdoAccountType.atMAPI:
                                //{
                                //    resultText.AppendLine(string.Format(" ----- atMAPI account, SMTPAddress = {0}, ", ((RDOIMAPAccount)rdoAccount).SMTPAddress));
                                //    break;
                                //}


                        }

                        
                    }


                    resultText.AppendLine(string.Empty);
                    resultText.AppendLine(string.Empty);
                    resultText.AppendLine(string.Format("B2. Searching Delegated for (On behalf of) : Current default email : {0}", GetDefaultEmailAddress()));

                    if (RdoSession.CurrentUser == null)
                    {
                        resultText.AppendLine("No current user or default email address");
                        continue;
                    }

                    var isDelegateForEntries = RdoSession.CurrentUser.IsDelegateFor;
                    if (isDelegateForEntries == null || isDelegateForEntries.Count == 0)
                    {
                        resultText.AppendLine("No Delegate For accounts - default email address");
                        continue;
                    }

                    for (var i = 1; i <= isDelegateForEntries.Count; i++)
                    {
                        var delegateForEmailAddress = isDelegateForEntries[i].SMTPAddress;
                        resultText.AppendLine(string.Format(" -- DelegerateFor  = {0}, ", delegateForEmailAddress));
                    }
                    resultText.AppendLine(string.Empty);


                    //MessageBox.Show(resultText.ToString());
/*
                    resultText.AppendLine(string.Empty);
                    resultText.AppendLine("");
                    resultText.AppendLine(string.Format("B2. Searching Stores in Profile, Store Count = {1}: {0}",
                                                        profileName, RdoSession.Stores.Count));
                    resultText.AppendLine(string.Empty);


                    int storeIndex = 0;
                    //foreach (RDOStore rdoStore in RdoSession.Stores)
                    ////for (int i = 1; i <= RdoSession.Stores.Count; i++ )
                    //{
                    //    storeIndex++;
                    //    try
                    //    {
                    //        //RDOStore rdoStore = RdoSession.Stores[i];
                    //        resultText.AppendLine(string.Format("{0} Store name = {1}", storeIndex, rdoStore.Name));
                    //        var isValidStore = true;
                    //        if (rdoStore.StoreAccount == null || rdoStore.StoreKind == TxStoreKind.skPublicFolders)
                    //            isValidStore = false;


                    //        resultText.AppendLine(string.Format("++ {3} Store: {0}, Type = {1}, IsValid = {2}",
                    //                                            rdoStore.Name, rdoStore.StoreKind, isValidStore,
                    //                                            storeIndex));
                    //        resultText.AppendLine(string.Empty);

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        resultText.AppendLine(string.Format("Error : {0}\n Stack: {1}", e.Message, e.StackTrace));
                    //        resultText.AppendLine(string.Empty);
                    //    }


                    //    foreach (RDOFolder rdoFolder in rdoStore.RootFolder.Folders)
                    //    {
                    //        resultText.AppendLine(string.Format(" ######### Folder: {0}, ShowAsOutlookAB = {1}, FolderKind = {2}, Description = {3}",
                    //            rdoFolder.Name, rdoFolder.ShowAsOutlookAB, rdoFolder.FolderKind, rdoFolder.Description));
                    //    }

                    //    resultText.AppendLine("-------------End Stores -----------------");
                    //}


                    // get default outbox folder
                    resultText.AppendLine(LineText);
                    resultText.AppendLine("Get default Outbox folder...");
                    var defaultFolder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderOutbox);

                    resultText.AppendLine(string.Format("Name : {0}, Description = {1}", defaultFolder.Name,
                                                        defaultFolder.Description));

                    resultText.AppendLine(string.Format("Default Outbox folder Store Name : {0}",
                                                        defaultFolder.Store.Name));

                    resultText.AppendLine(LineText);
                    resultText.AppendLine(string.Empty);
 */ 
                }
                catch (Exception ex)
                {
                    resultText.AppendLine(string.Format("Error : {0}\n Stack: {1}", ex.Message, ex.StackTrace));
                }
            }



            return resultText;
        }


        public string GetEmailAddressForAccount(RDOAccount rdoAccount)
        {
            string accountEmailAddress = null;

            switch (rdoAccount.AccountType)
            {
                case rdoAccountType.atPOP3:
                    {
                        accountEmailAddress =  ((IRDOPOP3Account)rdoAccount).SMTPAddress;
                        break;
                    }
                case rdoAccountType.atHTTP:
                    {
                        accountEmailAddress =  ((RDOHTTPAccount)rdoAccount).SMTPAddress;
                        break;
                    }
                case rdoAccountType.atIMAP:
                    {
                        accountEmailAddress = ((RDOIMAPAccount)rdoAccount).SMTPAddress;
                        break;
                    }               
            }

            return accountEmailAddress;

        }

        public List<RDOAccount> FindAllEmailAccountsBySenderEmailAddress(string senderEmailAddress)
        {
            var accountList = new List<RDOAccount>();
            foreach (RDOAccount rdoAccount in RdoSession.Accounts)
            {
                if (rdoAccount.AccountCategories != AccountCategoryMail &&
                    rdoAccount.AccountCategories != AccountCategoryMailAndStore)
                    continue;

                if (String.Compare(senderEmailAddress, rdoAccount.Name, true, CultureInfo.InvariantCulture) == 0)
                    accountList.Add(rdoAccount);
            }

            return accountList;
        }

/*

        public void SendTestEmailMessage(string senderEmailAddress, string receiverEmailAddress, string subject)
        {
            if (!SetToSenderEmailProfile(senderEmailAddress))
            {
                MessageBox.Show(string.Format("Cannot found Profiles having Sender's Email '{0}' account", senderEmailAddress));
                return;
            }
            else
            {
                MessageBox.Show(string.Format("Profile '{0}' contain sender email account.", ProfileName));
            }


            RDOMail Msg;
            
            if (IsSharedMailBoxAccountEmailAddress(senderEmailAddress))
            {
                // for shared mailbox email address
                var userName = RdoSession.CurrentUser.Name;
                var serverName = RdoSession.ExchangeMailboxServerName;

                RdoSession.Logoff();

                RdoSession.LogonExchangeMailbox(userName, serverName);

                // set draft folder from shared mailbox
                var drafts = RdoSession.GetSharedDefaultFolder(senderEmailAddress, rdoDefaultFolders.olFolderDrafts);

                // create new message
                Msg = drafts.Items.Add(Type.Missing);

                MessageBox.Show("Sending email to exchange share mail box");
            }
            else
            {
                var drafts = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderDrafts);
                Msg = drafts.Items.Add(Type.Missing);

                if (!IsDefaultEmailAddress(senderEmailAddress))
                {
                    // check if the sender email is Delegated for address
                    var DelegatedForEntry = FindDelegateAddressEntry(senderEmailAddress);
                    if (DelegatedForEntry != null)
                    {
                        // use default email account to send on behalf for                  
                        Msg.SentOnBehalfOf = DelegatedForEntry;
                    }
                    else
                        Msg.Account = RdoSession.Accounts[senderEmailAddress];
                }                
            } 


            Msg.To = receiverEmailAddress;
            Msg.Recipients.ResolveAll(true, 0);
            Msg.Subject = subject;
            Msg.Body = "Test body";
            Msg.Save();
            Msg.Send();

            MessageBox.Show("Email is sent!");

        }
*/

        public void SendTestEmailMessage(string senderEmailAddress, string receiverEmailAddress, string subject)
        {
            if (!SetToSenderEmailProfile(senderEmailAddress))
            {
                MessageBox.Show(string.Format("Cannot found Profiles having Sender's Email '{0}' account", senderEmailAddress));
                return;
            }
            else
            {
                MessageBox.Show(string.Format("Profile '{0}' contain sender email account.", ProfileName));
            }


            RDOMail Msg;

            if (IsDefaultEmailAddress(senderEmailAddress))
            {
                var drafts = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderDrafts);
                Msg = drafts.Items.Add(Type.Missing);
                Msg.Account = null;

                MessageBox.Show("Sending email to default email account");
            }
            else if (IsSharedMailBoxAccountEmailAddress(senderEmailAddress))
            {
                // for shared mailbox email address
                var userName = RdoSession.CurrentUser.Name;
                var serverName = RdoSession.ExchangeMailboxServerName;

                RdoSession.Logoff();

                RdoSession.LogonExchangeMailbox(userName, serverName);
                //RdoSession.LogonExchangeMailbox(senderEmailAddress, serverName);

                // set draft folder from shared mailbox
                var drafts = RdoSession.GetSharedDefaultFolder(senderEmailAddress, rdoDefaultFolders.olFolderDrafts);

                // create new message
                Msg = drafts.Items.Add(Type.Missing);

                var DelegatedForEntry = FindDelegateAddressEntry(senderEmailAddress);
                if (DelegatedForEntry != null)
                {
                    // use default email account to send on behalf for                  
                    Msg.SentOnBehalfOf = DelegatedForEntry;
                    MessageBox.Show("Sending email to exchange share mail box and set DelegatedForEntry");
                }

                MessageBox.Show("Sending email to exchange share mail box");
            }
            else
            {
                // delegated or non default email account
                var drafts = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderDrafts);
                Msg = drafts.Items.Add(Type.Missing);


                // check if the sender email is Delegated for address
                var DelegatedForEntry = FindDelegateAddressEntry(senderEmailAddress);
                if (DelegatedForEntry != null)
                {
                    // use default email account to send on behalf for                  
                    Msg.SentOnBehalfOf = DelegatedForEntry;

                    MessageBox.Show("Sending email to OnBehalf of email account");
                }
                else
                {
                    Msg.Account = RdoSession.Accounts[senderEmailAddress];
                    MessageBox.Show("Sending email to other non-default of email account");
                }
            }


            Msg.To = receiverEmailAddress;
            Msg.Recipients.ResolveAll(true, 0);
            Msg.Subject = subject;
            Msg.Body = "Test body";
            Msg.Save();
            Msg.Send();

            MessageBox.Show("Email is sent!");

        }

        private RDOAccount FindRdoEmailAccountBySenderEmailAddress(string senderEmailAddress)
        {
            var emailAccounts = FindAllEmailAccountsBySenderEmailAddress(senderEmailAddress);

            if (emailAccounts.Count == 0)
                return null;

            if (emailAccounts.Count == 1)
                return emailAccounts[0];

            // exchange take priority first
            var account = emailAccounts.Find(a => a.AccountType == rdoAccountType.atExchange);
            if (account != null)
                return account;

            // order by email acocunt type - atPOP3 , atIMAP , atExchange , atMAPI , atLDAP , atOther 
            var orderedEmailAccounts = emailAccounts.OrderBy(a => (int)a.AccountType).ToList();

            return orderedEmailAccounts[0];
        }


        public bool SetToSenderEmailProfile(string senderEmailAddress)
        {
            // check if sender email in current profile first
            if (ValidSenderEmailAddressInEmailProfile(senderEmailAddress))
                return true;

            bool profileSwitched = false;
            foreach (string profileName in RdoSession.Profiles)
            {
                if (RdoSession.ProfileName == profileName)
                    continue;
                try
                {
                    // switch to other email profile
                    ProfileName = profileName;

                    // logon to other profile
                    SessionLogon();

                    if (!ValidSenderEmailAddressInEmailProfile(senderEmailAddress))
                        continue;

                    profileSwitched = true;
                    break;
                }
                catch (Exception ex)
                {
                    // capture exception and do nothing if fail to switch to that profile
                    //_logger.ErrorException("SetToSenderEmailProfile:", ex);
                    ProfileName = string.Empty;
                }
            }

            return profileSwitched;
        }

        /// <summary>
        /// If sender email addressis configured as Delegate for, then sent out the address entry
        /// </summary>
        /// <param name="senderEmailAddress"></param>
        /// <returns></returns>
        public RDOAddressEntry FindDelegateAddressEntry(string senderEmailAddress)
        {
            if (RdoSession.CurrentUser == null)
                return null;

            var isDelegateForEntries = RdoSession.CurrentUser.IsDelegateFor;
            if (isDelegateForEntries == null)
                return null;

            for (var i = 1; i <= isDelegateForEntries.Count; i++)
            {
                var delegateForEmailAddress = isDelegateForEntries[i].SMTPAddress;
                if (String.Compare(senderEmailAddress, delegateForEmailAddress, true, CultureInfo.InvariantCulture) == 0)
                    return isDelegateForEntries[i];
            }
            return null;
        }



        public void SendDefaultTestEmailMessage(string senderEmailAddress, string receiverEmailAddress, string subject)
        {

            if (RdoSession.ExchangeConnectionMode > 0)
            {
                var username = RdoSession.CurrentUser.Name;
                var serverName = RdoSession.ExchangeMailboxServerName;


                RdoSession.Logoff();

                RdoSession.LogonExchangeMailbox(username, serverName);

                var sharedDraftFolder = RdoSession.GetSharedDefaultFolder(senderEmailAddress, rdoDefaultFolders.olFolderDrafts);

                var isDelegateFor = RdoSession.CurrentUser.IsDelegateFor;

                var sharedBox = RdoSession.GetSharedMailbox(senderEmailAddress);


                int x = 3;

            }

            //if (FindRdoEmailAccountBySenderEmailAddress(senderEmailAddress) == null)
            //{
            //    MessageBox.Show(string.Format("Sender's Email address '{0}' cannot found", senderEmailAddress));
            //    return;
            //}

            var user = RdoSession.CurrentUser;
            if (user != null)
            {
                MessageBox.Show("Default email address = " + user.SMTPAddress);
            }



            var Drafts = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderDrafts);
            Drafts = RdoSession.GetSharedDefaultFolder(senderEmailAddress, rdoDefaultFolders.olFolderDrafts);
            var Msg = Drafts.Items.Add(Type.Missing);
            var Account = RdoSession.Accounts[senderEmailAddress];


            if (Account == null)
            {
                if (user != null)
                {
                    MessageBox.Show("Email account cannot found, use default email account.\n\nNotes: Current login user email address = " + user.SMTPAddress);
                }
            }

            Msg.Account = Account;

            Msg.SentOnBehalfOf = FindDelegateAddressEntry(senderEmailAddress);
            Msg.SenderEmailAddress = senderEmailAddress;
            Msg.SenderName = senderEmailAddress;
            Msg.To = receiverEmailAddress;
            Msg.Recipients.ResolveAll(true, 0);
            Msg.Subject = subject;
            Msg.Body = "Test body";
            Msg.Save();
            Msg.Send();

            MessageBox.Show("Email is sent!");

        }

/*
        public void SendDefaultTestEmailMessage(string senderEmailAddress, string receiverEmailAddress, string subject)
        {

            //if (FindRdoEmailAccountBySenderEmailAddress(senderEmailAddress) == null)
            //{
            //    MessageBox.Show(string.Format("Sender's Email address '{0}' cannot found", senderEmailAddress));
            //    return;
            //}

            var user = RdoSession.CurrentUser;
            if (user != null)
            {
                MessageBox.Show("Default email address = " + user.SMTPAddress);
            }



            var Drafts = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderDrafts);
            var Msg = Drafts.Items.Add(Type.Missing);
            var Account = RdoSession.Accounts[senderEmailAddress];


            if (Account == null)
            {
                if (user != null)
                {
                    MessageBox.Show("Email account cannot found, use default email account.\n\nNotes: Current login user email address = " + user.SMTPAddress);
                }
            }

            Msg.Account = Account;

            Msg.SentOnBehalfOf = FindDelegateAddressEntry(senderEmailAddress);
            Msg.SenderEmailAddress = senderEmailAddress;
            Msg.SenderName = senderEmailAddress;
            Msg.To = receiverEmailAddress;
            Msg.Recipients.ResolveAll(true, 0);
            Msg.Subject = subject;
            Msg.Body = "Test body";
            Msg.Save();
            Msg.Send();

            MessageBox.Show("Email is sent!");

        }
*/
        public string GetDefaultEmailAddress()
        {
            // need to Kario Connect to initialise so that we can get current user. Otherwise, they will return unknown
            var folder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderSentMail);

            if (RdoSession.CurrentUser != null)
                return RdoSession.CurrentUser.SMTPAddress;
            return null;
        }

        public bool IsDefaultEmailAddress(string senderEmailAddress)
        {
            var defaultEmailAddress = GetDefaultEmailAddress();
            
            if (string.IsNullOrEmpty(defaultEmailAddress))
                return false;

            return (String.Compare(senderEmailAddress, defaultEmailAddress, true, CultureInfo.InvariantCulture) == 0);
        }

        public bool IsDelegateForEmailAddress(string senderEmailAddress)
        {
            return FindDelegateAddressEntry(senderEmailAddress) != null;
        }

        public bool IsDefaultMailAccountEmailAddress(string senderEmailAddress)
        {
            var sentFolder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderSentMail);
            if (sentFolder != null && sentFolder.Items.Count > 0)
            {
                var sender = sentFolder.Items[1].Sender;
                if (sender != null)
                    return (String.Compare(senderEmailAddress, sender.SMTPAddress, true, CultureInfo.InvariantCulture) == 0);                
            }

            return false;
        }


        public bool IsSharedMailBoxAccountEmailAddress(string senderEmailAddress)
        {
            var isFound = false;
            if (RdoSession.ExchangeConnectionMode > 0)
            {

                try
                {
                    var userName = RdoSession.CurrentUser.Name;
                    var serverName = RdoSession.ExchangeMailboxServerName;

                    RdoSession.Logoff();

                    RdoSession.LogonExchangeMailbox(userName, serverName);

                    var sharedDraftFolder = RdoSession.GetSharedDefaultFolder(senderEmailAddress,
                                                                              rdoDefaultFolders.olFolderDrafts);
                    isFound = sharedDraftFolder != null;

                }
                catch (Exception ex)
                {
                    isFound = false;
                }
                finally
                {
                    // logon back to current profile
                    SessionLogon();
                }
            }
        
            return isFound;
        }

        public string GetEmailAccountSendBoxItemSenderEmail()
        {
            var sentFolder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderSentMail);
            if (sentFolder != null && sentFolder.Items.Count > 0)
            {
                var sender = sentFolder.Items[1].Sender;
                if (sender != null)
                    return sender.SMTPAddress;
            }

            return null;
        }
        public bool ValidSenderEmailAddressInEmailProfile(string senderEmailAddress)
        {
            // check if default email address in current profile
            if (IsDefaultEmailAddress(senderEmailAddress))
                return true;

            // check if it is Delegated for email address
            if (IsDelegateForEmailAddress(senderEmailAddress))
                return true;

            if (IsSharedMailBoxAccountEmailAddress(senderEmailAddress))
                return true;

            // check if sender email in current profile
            if (FindRdoEmailAccountBySenderEmailAddress(senderEmailAddress) != null)
                return true;
            
            return false;
        }

        public bool ResolveEmailAddress(string senderEmailAddress)
        {

       
            // Get the first item in the inbox, can be any other mail item
            //var folder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderSentMail);
            //var folder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderSentMail);

            var folder = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderDrafts);

            //var Msg = folder.Items.Add(Type.Missing);
            //Msg.Recipients.ResolveAll(true, 0);
            //Msg.Subject = "xxx";
            //Msg.Body = "Test body";
            //Msg.Save();
            //var xAddress = Msg.Sender.SMTPAddress;

            //var MailItem = folder.Items[1];
            //var smtpAddress = MailItem.Sender.SMTPAddress;


            //MessageBox.Show(string.Format("Sender Email address for Sent box = {0} ", smtpAddress));

            try
            {
                if (RdoSession.CurrentWindowsUser != null)
                {
                    MessageBox.Show(string.Format("1.Email address of current window users = {0}",
                                                  RdoSession.CurrentWindowsUser.SMTPAddress));

                }
                else
                {
                    MessageBox.Show("1.Window current user cannot found");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(string.Format("1. Current Windows user not supported \n\n{0}",ex.Message));
            }

            MessageBox.Show(string.Format("2.Sender Email address from Sent box = {0} ",
                                          GetEmailAccountSendBoxItemSenderEmail()));


            // cyrrent user
            if (RdoSession.CurrentUser != null)
            {
                MessageBox.Show(string.Format("3. Current user = {0}, SMTP Email address = {1}", RdoSession.CurrentUser.Name,
                                RdoSession.CurrentUser.SMTPAddress));

                MessageBox.Show(string.Format("3a. Current user = {0}, Address = {1}", RdoSession.CurrentUser.Name,
                                RdoSession.CurrentUser.Address));

            }
            else
            {
                MessageBox.Show("3. Current user cannot found");
            }


            var storeOwnerFound = false;
            var store = RdoSession.GetDefaultFolder(rdoDefaultFolders.olFolderSentMail).Store;
            if (store != null)
            {
                if (store.StoreKind == TxStoreKind.skPrimaryExchangeMailbox ||
                    store.StoreKind == TxStoreKind.skDelegateExchangeMailbox)
                {


                    RDOExchangeMailboxStore exStore = ((RDOExchangeMailboxStore)store);
                    if (exStore != null && exStore.Owner != null)
                    {
                        MessageBox.Show(string.Format("4. Sent box is exchange mail box and owner SMTP adderess is {0}",
                                                      exStore.Owner.SMTPAddress));

                        storeOwnerFound = true;
                    }

                }
            }

            if (!storeOwnerFound)
                MessageBox.Show("4. Exchange store owner email cannot found");                

            return true;


        }

   

    }
}
