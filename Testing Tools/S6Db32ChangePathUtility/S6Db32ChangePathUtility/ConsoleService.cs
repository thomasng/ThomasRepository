using System;


namespace S6Db32ChangePathUtility
{
    public class ConsoleService : ILogger
    {
        private DateTime _startTime;
        private DateTime _endTime;

        public ScripterLog _scripterLog = new ScripterLog();

        public ConsoleService()
        {
        }

        public int StartModification(bool confirmToClose)
        {
            _startTime = DateTime.Now;

            ClearResultLog();

            RecordTitle();

            var mainService = new MainService(this);
            var result = mainService.StartRegistryModification();

            _endTime = DateTime.Now;

            RecordFooter();

            if (confirmToClose)
            {
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }

            return result ? 0 : -1;
        }

        #region ILogger Members

        public void ClearResultLog()
        {                        
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
            Console.WriteLine(text);

            // write to log file
            _scripterLog.WriteLn(text, true);
        }

        public void UpdateRegistryProcessedStatus(string mode, int procssedNumber)
        {
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
            RecordResultLog("Run in Console commad line mode");
            RecordResultLog(string.Format("Start Time : {0} ", _startTime), false);
            RecordResultLog(string.Format("Machine Name : {0} ", Environment.MachineName), false);
            RecordBlankLine();
        }

        public void RecordFooter()
        {
            RecordBlankLine();
            RecordResultLog("***************************");
            RecordResultLog(string.Format("End Time : {0} ", _endTime), false);
            RecordResultLog(string.Format("Processed Time : {0} seconds", (_endTime - _startTime).TotalSeconds), false);
            RecordResultLog("****************************************************************");
            RecordBlankLine();
        }

        #endregion


    }
}
