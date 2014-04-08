using System;

namespace S6Db32ChangePathUtility
{
    public interface ILogger
    {
        void RecordResultLogSeparationLine();

        void RecordResultLog(string text, bool isHighLight);

        void RecordResultLog(string text);

        void RecordResultLogError(string text);

        void RecordResultLogError(Exception ex);

        void RecordBlankLine();

        void UpdateRegistryProcessedStatus(string mode, int procssedNumber);
    }
}
