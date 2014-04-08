using System;
using System.Text;

namespace S6Db32ChangePathUtility
{
    public enum ActionMode
    {
        FindFirstMatch,
        FindAllMatch,
        Modification
    }


    public class MainModel
    {
        public ActionMode TaskMode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // result
        public StringBuilder ResultLogText { get; set; }



        public string DllName { get; set; }
        public string ChangeFromValue { get; set; }
        public string ChangeToValue { get; set; }


        public MainModel()
        {
            ResultLogText = new StringBuilder();
        }

    }
}
