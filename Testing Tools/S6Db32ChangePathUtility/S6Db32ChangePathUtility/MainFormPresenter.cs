using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace S6Db32ChangePathUtility
{
    public class MainFormPresenter : ILogger
    {

        public IMainFormView CurrentView { get; set; }

        public ScripterLog _scripterLog = new ScripterLog();

        public MainFormPresenter(IMainFormView view)
        {
            CurrentView = view;
            if (view != null && view.Model == null)
                CurrentView.Model = new MainModel();            
        }

        private MainModel ViewModel
        {
            get { return CurrentView.Model; }
        }


        public void OnLoadData()
        {
            ViewModel.DllName = MainService.S632DbDllName;            

            // set default install path
            var mainService = new MainService(this);
            ViewModel.ChangeToValue = mainService.FinalSrDllFullPath;

            ViewModel.ChangeFromValue = string.Empty;
        }


        /// <summary>
        /// verify and found the first full poath of dll name
        /// </summary>
        /// <returns></returns>
        public bool FindFirstMatch()
        {
            var mainService = new MainService(this);
            var firstData = mainService.FindFirstMatchedData();       

            if (firstData != null)
            {
                ViewModel.ChangeFromValue =  firstData.Value;

                RecordResultLog(string.Format("  --- Registry value '{0}' found!", ViewModel.DllName), false);
                RecordResultLog(string.Format("  --- First location : '{0}'", firstData.Key), false);
                RecordResultLog(string.Format("  --- Value of path : '{0}'", firstData.Value), false);
            }
            else
            {
                ViewModel.ChangeFromValue =  string.Empty;

                RecordResultLog(string.Format("  --- Registry value '{0}' is NOT found!", ViewModel.DllName), false);
            }

            return true;
        }

        /// <summary>
        /// verify and found all match of dll
        /// </summary>
        /// <returns></returns>
        public bool FindAllMatch()
        {
            RecordResultLog("Operation : Find All matching only mode (no modification)");

            var mainService = new MainService(this);
            var registryDataList = mainService.FindAllMatchingRegistryData();

            RecordResultLog(string.Format("Number of matched registry item found : {0}", registryDataList.Count), false);

            return true;
        }


        public bool DoModification()
        {
            RecordResultLog("Operation : Start modification mode");

            var mainService = new MainService(this);
            return mainService.StartRegistryModification();            
        }


        #region Background worker functions


        /// <summary>
        /// Start 
        /// </summary>
        /// <returns></returns>
        public void StartAction(ActionMode actionMode)
        {
            if (_backgroundWorker == null)
                _backgroundWorker  = CreateBackGroundWorker();

            if (_backgroundWorker.IsBusy)
            {
                return;
            }


            ViewModel.TaskMode = actionMode;

            CurrentView.ShowResultLogMessage(string.Empty, false);

            // Start the BackgroundWorker
            _backgroundWorker.RunWorkerAsync();
        }


        private BackgroundWorker _backgroundWorker;


        private BackgroundWorker CreateBackGroundWorker()
        {
            var backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            return backgroundWorker;
        }


        void BackgroundWorker_RunWorkerCompleted(object sender,
                                                 RunWorkerCompletedEventArgs e)
        {
            if (ViewModel.TaskMode == ActionMode.FindFirstMatch)
                CurrentView.VerificationCompleted((bool)e.Result);

            else if (ViewModel.TaskMode == ActionMode.FindAllMatch)
                CurrentView.VerificationCompleted((bool)e.Result);

            else
                CurrentView.ModificationCompleted((bool)e.Result);

            _backgroundWorker = null;
        }

        void BackgroundWorker_ProgressChanged(object sender,
                                              ProgressChangedEventArgs e)
        {
            // This method runs in the UI thread.
            // Report progress using the value e.ProgressPercentage and e.UserState

            var progressStatus = e.UserState as ProgressStatus;

            // append message to result log
            if (progressStatus.ProgressMode == "Log")
            {
                CurrentView.ShowResultLogMessage(progressStatus.MessageText, true);
            }
            else if (progressStatus.ProgressMode == "ProcessedNumber")
            {
                CurrentView.UpdateRegistryProcessedStatus(progressStatus.NumProcessed, progressStatus.ProcessedText);
            }
        }

        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isSuccess = false;
            ViewModel.StartTime = DateTime.Now;

            ClearResultLog();

            RecordTitle();

            if (ViewModel.TaskMode == ActionMode.FindFirstMatch)
                isSuccess = FindFirstMatch();
            else if (ViewModel.TaskMode == ActionMode.FindAllMatch)
                isSuccess = FindAllMatch();
            else if (ViewModel.TaskMode == ActionMode.Modification)
            {
                isSuccess = DoModification();
            }

            e.Result = isSuccess;

            ViewModel.EndTime = DateTime.Now;

            RecordFooter();
        }

        #endregion Background worker functions



        #region Result Log Functions

        private void SendResultLog(string message)
        {

            if (_backgroundWorker != null)
            {
                var status = new ProgressStatus
                {
                    ProgressMode = "Log",
                    MessageText = message
                };

                _backgroundWorker.ReportProgress(100, status);
            }
            else
            {
                // same UI thread, just show the result directly
                CurrentView.ShowResultLogMessage(message, true);
            }

            // log message tyo log file
            _scripterLog.WriteLn(message, true);
        }

        public void ClearResultLog()
        {
            ViewModel.ResultLogText = new StringBuilder();
            SendResultLog(string.Empty);
        }

        public void RecordResultLogSeparationLine()
        {
            RecordResultLog("------------------------------------ ------------------------------------ ");
        }

        public void RecordResultLog(string text, bool isHighLight)
        {
            RecordResultLog(isHighLight ? "****  " + text : text);
        }

        public void RecordResultLog(string text)
        {
            ViewModel.ResultLogText.AppendLine(text);
            SendResultLog(text);
        }

        public void UpdateRegistryProcessedStatus(string mode, int procssedNumber)
        {
            if (_backgroundWorker != null)
            {
                var status = new ProgressStatus
                {
                    ProgressMode = "ProcessedNumber",
                    NumProcessed = procssedNumber,
                    ProcessedText = mode
                };


                _backgroundWorker.ReportProgress(100, status);
            }
        }

        

        public void RecordResultLogError(string text)
        {
            RecordResultLog("+++ Error : " + text);
        }

        public void RecordResultLogError(Exception ex)
        {
            RecordResultLogError(ex.Message);
            if (ex.InnerException != null)
                RecordResultLogError(ex.InnerException.Message);
        }

        public void RecordBlankLine()
        {
            RecordResultLog(string.Empty);
        }

        public void RecordTitle()
        {
            RecordBlankLine();
            RecordResultLog("****************************************************************");
            RecordResultLog("Run in Gui mode");
            RecordResultLog(string.Format("Start Time : {0} ", ViewModel.StartTime), false);
            RecordResultLog(string.Format("Machine Name : {0} ", Environment.MachineName), false);
            RecordBlankLine();
        }

        public void RecordFooter()
        {
            RecordBlankLine();
            RecordResultLog("***************************");
            RecordResultLog(string.Format("End Time : {0} ", ViewModel.EndTime), false);
            RecordResultLog(string.Format("Processed Time : {0} seconds", (ViewModel.EndTime - ViewModel.StartTime).TotalSeconds), false);
            RecordResultLog("****************************************************************");
            RecordBlankLine();
        }

        #endregion Result Log Functions

    }
}
