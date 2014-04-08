using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace S6Db32ChangePathUtility
{
    static class Program
    {

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// The main entry point for the application.
        ///  1. For Command Prompt mode
        ///     - return 0 if database update done
        ///     - return -1 if updating database is fail
        ///  2. For GUI Mode or Help mode
        ///     - always return 0
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            int errorCode = 0;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // redirect console output to parent process;
            // must be before any calls to Console.WriteLine()
            AttachConsole(ATTACH_PARENT_PROCESS);

            // parse command line and execute
            errorCode = ParseAndExecuteCommand(args);

            // return result code
            return errorCode;
        }


        /// <summary>
        /// Parse and execute command
        /// </summary>
        /// <param name="args"></param>
        ///     
        private static int ParseAndExecuteCommand(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "-c")
            {
                return (new ConsoleService()).StartModification();
            }
            else
            {
                Application.Run(new MainForm());

                return 0;
            }


        } // ParseAndExecuteCommand


        
    }
}
