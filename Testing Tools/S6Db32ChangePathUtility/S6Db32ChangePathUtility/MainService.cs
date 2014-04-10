using System.Collections.Generic;
using System.IO;

namespace S6Db32ChangePathUtility
{
    public class MainService
    {
        public const string S632DbDllName = "S6DB32.dll";

        public ILogger Logger { get; private set; }

        public string SrInstallPath { get; private set; }

        public string FinalSrDllFullPath { get; private set; }


        public MainService(ILogger logger)
        {
            Logger = logger;
            LoadData();
        }

        public RegistryBusinessLogic CreateRegistryBl()
        {
            return new RegistryBusinessLogic() { Logger = Logger };
        }


        public bool LoadData()
        {
            SrInstallPath = CreateRegistryBl().GetSystemReleaseProgramPath();

            if (string.IsNullOrEmpty(SrInstallPath))
            {
                Logger.RecordResultLogError("System Release install path is not found in registry! Please contact MYOB support!");
                return false;
            }

            Logger.RecordResultLog(string.Format("System Release install path : {0}", SrInstallPath));

            FinalSrDllFullPath = Path.Combine(SrInstallPath, S632DbDllName);

            return true;
        }


        public RegistryData FindFirstMatchedData()
        {
            if (string.IsNullOrEmpty(SrInstallPath))
            {
                Logger.RecordResultLogError("Cannot found System Release Installation path.");
                return null;
            }

            return CreateRegistryBl().GetFirstRegistryContainingPartialValue(S632DbDllName);
        }

        public List<RegistryData> FindAllMatchingRegistryData()
        {            
            if (string.IsNullOrEmpty(SrInstallPath))
            {
                Logger.RecordResultLogError("Cannot found System Release Installation path.");
                return new List<RegistryData>();
            }

            return CreateRegistryBl().FindAllMatchedRegistryData(S632DbDllName, true);
        }

        public bool IsValidToRun()
        {
            if (string.IsNullOrEmpty(SrInstallPath))
            {
                Logger.RecordResultLogError("Cannot found System Release Installation path.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Start the modification process
        ///  - change the matched S6Db32.dll path to new path
        /// </summary>
        /// <returns></returns>
        public bool StartRegistryModification()
        {

            if (!IsValidToRun())
            {
                 return false;
            }


            var foundList = FindAllMatchingRegistryData();

            if (foundList.Count > 0)
            {
                Logger.RecordResultLog(string.Format("  --- Therre are {0} registry keys matched!", foundList.Count), false);

                Logger.RecordBlankLine();

                Logger.RecordResultLog("Start Changing registry value....", true);

                // change all registry values
                var noOfModified = CreateRegistryBl().SetAllRegistryValue(foundList, FinalSrDllFullPath);


                if (noOfModified == foundList.Count)
                {
                    Logger.RecordResultLog("Update successfuly !!!", true);
                }
                else
                {
                    Logger.RecordResultLog("Some Update fail !!!", true);
                }
                // output status
                Logger.RecordResultLog(string.Format("No of registry updated : {0}", noOfModified), true);
                Logger.RecordResultLog(string.Format("No of registry fail to update : {0}", foundList.Count - noOfModified), true);
            }
            else
            {
                Logger.RecordResultLog(string.Format("  --- No registry value '{0}' is matched!", FinalSrDllFullPath), false);
            }

            return true;
        }
        

        
    }
}
