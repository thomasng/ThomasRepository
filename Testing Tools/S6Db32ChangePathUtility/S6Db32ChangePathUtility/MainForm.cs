using System;
using System.Windows.Forms;

namespace S6Db32ChangePathUtility
{
    public partial class MainForm : Form, IMainFormView
    {
        public MainModel Model { get; set; }

        public MainFormPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();

            InitializePresenter();
        }


        private void InitializePresenter()
        {
            _presenter = new MainFormPresenter(this);
            _presenter.OnLoadData();


            txtFindRegistryData.ReadOnly = true;
            txtFindRegistryData.Text = Model.DllName;
            //txtFirstMatchData.Text = Model.ChangeFromValue;
            txtToData.Text = Model.ChangeToValue;
        }

        private void btnFindFirst_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            UpdateRegistryProcessedStatus(0, "processed");

            _presenter.StartAction(ActionMode.FindFirstMatch);

        }

        private void btnFindAllMatch_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            UpdateRegistryProcessedStatus(0, "processed");

            _presenter.StartAction(ActionMode.FindAllMatch);

        }


        private void EnableControls(bool enabled)
        {
            gbModify.Enabled = enabled;
            btnFindAll.Enabled = enabled;

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnStartModification_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(this, "Are you sure to modify path of S6Db32.dll in the registry now?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            EnableControls(false);

            UpdateRegistryProcessedStatus(0, "processed");

            _presenter.StartAction(ActionMode.Modification);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }



        #region IMainFormView Members


        /// <summary>
        /// show result log message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="append"></param>
        public void ShowResultLogMessage(string message, bool append)
        {

            if (append)
                txtResult.Text += Environment.NewLine + message;
            else
                txtResult.Text = message;

            if (txtResult.Text.Length > 0)
            {
                // move the cursor upto latest text
                txtResult.SelectionStart = txtResult.Text.Length - 1;
                txtResult.ScrollToCaret();
            }

            Application.DoEvents();
        }

        public void ShowVerificationStatus()
        {
            throw new NotImplementedException();
        }

        public void VerificationCompleted(bool isSuccess)
        {
            EnableControls(true);
        }

        public void ModificationCompleted(bool isSuccess)
        {
            EnableControls(true);
        }


        public void UpdateRegistryProcessedStatus(int number, string mode)
        {
            lblRegistryProcessed.Text = string.Format("No of registry item {0} : {1} ", mode, number.ToString("0,0"));
            Application.DoEvents();
        }

        #endregion

        private void MainForm_Shown(object sender, EventArgs e)
        {
            btnStartModification_Click(null, null);
        }

    }
}
