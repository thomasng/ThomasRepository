namespace S6Db32ChangePathUtility
{
    public interface IMainFormView
    {
        MainModel Model { get; set; }

        void ShowResultLogMessage(string message, bool append);
        void UpdateRegistryProcessedStatus(int number, string mode);

        void ShowVerificationStatus();

        void VerificationCompleted(bool isSuccess);

        void ModificationCompleted(bool isSuccess);

    }
}
