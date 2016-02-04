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
            if (args.Length > 0 && ContainArgumentValue(args, "-c"))
            {
                var consoleService = new ConsoleService();

                var confirmToClose = true;
                // silent mode
                if (ContainArgumentValue(args, "-s"))
                {
                    confirmToClose = false;
                    consoleService.SilentMode = true;
                }


                return consoleService.StartModification(confirmToClose);
            }
            else
            {
                Application.Run(new MainForm());

                return 0;
            }


        } // ParseAndExecuteCommand


        private static bool ContainArgumentValue(string[] args, string value)
        {
            if (args.Length == 0)
                return false;

            for(int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == value)
                    return true;
            }
            return false;
        }
        
    }
}
