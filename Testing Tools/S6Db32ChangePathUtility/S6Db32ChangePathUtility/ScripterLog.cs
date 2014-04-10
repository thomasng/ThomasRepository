using System;
using System.IO;
using System.Windows.Forms;

namespace S6Db32ChangePathUtility
{
    public class ScripterLog
    {

        /// <summary>
        /// sortable Date Time Pattern (yyyy'-'MM'-'dd'T'HH':'mm':'ss)
        /// e.g. 2002-01-03T00:00:00
        /// </summary>
        private const string SORTABLE_DATE_TIME_PATTERN = "s";

        private const string FILE_NEW_LINE = "\r\n";

        private FileInfo _logFile;
        private DateTime _upgradedTime;
        private string _headerText = string.Empty;
        private string LineText = null;

        public ScripterLog()
        {
            _upgradedTime = DateTime.Now;
            _headerText = string.Empty;
            Init(GetLogFileName());
            
        }

        private string GetLogFileName()
        {
            string appDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            return Path.Combine(appDirectory, "ResultLog.txt");
        }


        public ScripterLog(string logFile, DateTime upgradedTime, string headerText)
        {
            _upgradedTime = upgradedTime;
            _headerText = headerText;
            Init(logFile);
        }

        public string LogFileName
        {
            get { return _logFile.FullName; }
        }


        /// <summary>
        /// Initialise the encryption log file
        /// </summary>
        protected void Init(string sOutputToFile)
        {
            // allow logging
            _logFile = new FileInfo(sOutputToFile);

            // generate the file
            GenerateTextFile(string.Empty, sOutputToFile);

            // write header
            WriteFileHeader(_upgradedTime);
        }

        /// <summary>
        /// genertae/overwrite the text file
        /// </summary>
        /// <param name="scriptBlock"></param>
        /// <param name="fileName"></param>
        public static void GenerateTextFile(string scriptBlock, string fileName)
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.Write(scriptBlock);
                sw.Flush();
                sw.Close();
            }
        }


        /// <summary>
        /// Write output to the file to a line
        /// </summary>
        public void WriteLn(string text, bool bReplaceNewLine)
        {
            if (bReplaceNewLine)
                text = text.Replace("\n", FILE_NEW_LINE);

            Write(text + FILE_NEW_LINE);
        }

        // WriteLn

        /// <summary>
        /// Write output to the file
        ///  - with time stamp
        /// </summary>
        public void Write(string text)
        {
            string textWithTime = string.Format("{0}\t{1}", DateTime.Now.ToString(SORTABLE_DATE_TIME_PATTERN)
                                                , text);
            WriteRawText(textWithTime);
        }

        /// <summary>
        /// Write raw text line
        /// </summary>
        /// <param name="text"></param>
        public void WriteRawText(string text)
        {
            using (var w = File.AppendText(_logFile.FullName))
            {
                w.WriteLine(text);

                // Update the underlying file.
                w.Flush();
                // Close the writer and underlying file.
                w.Close();
            }
        }

        /// <summary>
        /// Write output to the file to a line
        /// </summary>
        public void WriteFileHeader(DateTime upgradeTime)
        {
            WriteBlankLine();
            WrtiteSectionLine();

            WriteRawText(string.IsNullOrEmpty(_headerText)
                             ? "MYOB System Release S6DB32.dll Change Path Utility"
                             : _headerText);

            WriteRawText("Modified Time : " + upgradeTime.ToString());
            WrtiteSectionLine();
            WriteBlankLine();
        }


        /// <summary>
        /// Display  section line
        ///  - output to Console windows
        /// </summary>
        public void WrtiteSectionLine()
        {
            if (string.IsNullOrEmpty(LineText))
            {
                LineText = string.Empty;
                for (int i = 0; i < 80; i++)
                    LineText += "-";
            }

            WriteRawText(LineText);
        }


        /// <summary>
        /// Display  blank line
        /// </summary>
        public void WriteBlankLine()
        {
            WriteRawText("");
        }
    }
}
