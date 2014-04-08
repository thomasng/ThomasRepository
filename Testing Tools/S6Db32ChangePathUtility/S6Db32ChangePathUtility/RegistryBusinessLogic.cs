using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace S6Db32ChangePathUtility
{
    public class RegistryBusinessLogic
    {
        public ILogger Logger { get; set; }
        public List<RegistryKey> RegistryRootKeys = new List<RegistryKey>();
        public RegistryBusinessLogic()
        {
            RegistryRootKeys.Add(Registry.ClassesRoot);
            RegistryRootKeys.Add(Registry.LocalMachine);
            RegistryRootKeys.Add(Registry.CurrentUser);
            RegistryRootKeys.Add(Registry.Users);
        }


        void ShowRegistryExceptionMessage(string keyName, string exceptionMessage)
        {
            //Presenter.RecordResultLog(string.Format("Fail to access registry key [{0}], Reason = [{1}]", keyName, exceptionMessage), true);
        }

        private IEnumerable<string> GetSubKeyNames(RegistryKey root)
        {
            try
            {
                return root.GetSubKeyNames();
            }
            catch (Exception ex)
            {
                ShowRegistryExceptionMessage(root.Name, ex.Message);
            }
            return null;
        }

        private IEnumerable<string> GetValueNames(RegistryKey root)
        {
            try
            {
                return root.GetValueNames();
            }
            catch (Exception ex)
            {
                ShowRegistryExceptionMessage(root.Name, ex.Message);
            }
            return null;
        }


        #region FindAllMatchedRegistryData

        private int _numOfRegistryProcessed = 0;        

        /// <summary>
        /// Find all matching registry data
        /// </summary>
        public List<RegistryData> FindAllMatchedRegistryData(string registryValueData, bool isPartialMatch)
        {
            _numOfRegistryProcessed = 0;

            var index = 1;
            var dataList = new List<RegistryData>();
            foreach (var registryRoot in RegistryRootKeys)
            {
                Logger.RecordBlankLine();
                Logger.RecordResultLog(string.Format("{0}. Searching Registry Root - [{1}]", index, registryRoot.Name), true);

                FindAllMatchedRegistryData(registryRoot, registryValueData, isPartialMatch, dataList);

                index++;
            }

            return dataList;
        }

        /// <summary>
        /// Find all matched registry day
        /// </summary>
        public void FindAllMatchedRegistryData(RegistryKey root, string registryValueData, bool isPartialMatch, List<RegistryData> dataList)
        {
            // get sub key names
            var subKeyNames = GetSubKeyNames(root);
            if (subKeyNames == null)
                return;


            foreach (var child in subKeyNames)
            {
                if (string.IsNullOrEmpty(child))
                    continue;

                try
                {
                    using (var childKey = root.OpenSubKey(child))
                    {
                        FindAllMatchedRegistryData(childKey, registryValueData, isPartialMatch, dataList);
                    }
                }
                catch (Exception ex)
                {
                    ShowRegistryExceptionMessage(root.Name, ex.Message);
                }
            }

            // get value names
            var rootValueNames = GetValueNames(root);
            if (rootValueNames == null)
                return;

            foreach (var value in rootValueNames)
            {
                try
                {
                    _numOfRegistryProcessed++;

                    if (_numOfRegistryProcessed % 1000 == 0)
                        Logger.UpdateRegistryProcessedStatus("analysed", _numOfRegistryProcessed);

                    var registryValue = (root.GetValue(value) ?? "").ToString();


                    if (!IsTextMatched(registryValue, registryValueData,isPartialMatch))
                        continue;

                    var data = new RegistryData
                    {
                        Key = string.Format("{0}\\{1}", root, value),
                        Value = (root.GetValue(value) ?? "").ToString()
                    };

                    Logger.RecordResultLog(string.Format("Dll path found in registry key :  [{0}], value = '{1}'", data.Key, data.Value), true);

                    dataList.Add(data);


                }
                catch (Exception ex)
                {
                    ShowRegistryExceptionMessage(root.Name, ex.Message);
                }

            }
        }


        private bool IsTextMatched(string value1, string value2 , bool isPartialMatch)
        {
            if (isPartialMatch)
            {
                // partial match
                return value1.ToLower().Contains(value2.ToLower());
            }
            else
            {
                // exactly match (ignore case)
                return string.Compare(value1, value2, StringComparison.InvariantCultureIgnoreCase) == 0;
            }            
        }

        #endregion FindAllMatchedRegistryData


        #region GetFirstRegistryContainingPartialValue

        public RegistryData GetFirstRegistryContainingPartialValue(string partialText)
        {
            _numOfRegistryProcessed = 0;

            var index = 1;
            foreach (var registryRoot in RegistryRootKeys)
            {
                Logger.RecordResultLog(string.Format("{0}. Searching Registry Root - [{1}]", index, registryRoot.Name), true);

                var matchRegistryData = GetFirstRegistryContainingPartialValue(registryRoot, partialText);
                if (matchRegistryData != null)
                    return matchRegistryData;

                index++;
            }

            return null;
        }

        public RegistryData GetFirstRegistryContainingPartialValue(RegistryKey root,  string partialText)
        {
            if (root == null)
                return null;

            var subKeyNames = root.GetSubKeyNames();

            foreach (var child in subKeyNames)
            {
                if (string.IsNullOrEmpty(child))
                    continue;

                using (var childKey = root.OpenSubKey(child))
                {
                    var registryData = GetFirstRegistryContainingPartialValue(childKey, partialText);
                    if (registryData != null)
                        return registryData;
                }
            }

            foreach (var value in root.GetValueNames())
            {
                _numOfRegistryProcessed++;

                if (_numOfRegistryProcessed % 1000 == 0)
                    Logger.UpdateRegistryProcessedStatus("analysed", _numOfRegistryProcessed);


                var registryValue = (root.GetValue(value) ?? "").ToString();

                if (!registryValue.ToLower().Contains(partialText.ToLower()))
                    continue;

                var matchRegistryData = new RegistryData
                                            {
                                                Key = string.Format("{0}\\{1}", root, value),
                                                Value = registryValue
                                            };


                return matchRegistryData;
            }

            return null;
        }

        #endregion GetFirstRegistryContainingPartialValue

        private int _numOfRegistryModified = 0;

        public int SetAllRegistryValue(List<RegistryData> registerList, string dataValue)
        {
            _numOfRegistryModified = 0;

            foreach (var registeryData in registerList)
            {
                SetRegistryValue(registeryData, dataValue);
            }

            return _numOfRegistryModified;
        }


        public bool SetRegistryValue(RegistryData registeryData, string dataValue)
        {
            try
            {
                Registry.SetValue(registeryData.Key, "", dataValue);
                Logger.RecordResultLog(string.Format("Registry key [{0}] is changed from ['{1}'] to ['{2}']", registeryData.Key, registeryData.Value, dataValue));
                _numOfRegistryModified++;
            }
            catch (Exception ex)
            {

                Logger.RecordResultLogError(string.Format("Fail to change value of registry key [{0}]", registeryData.Key));
                return false;
            }
            return true;
        }


        #region GetSystemReleaseProgramPath


        /// <summary>
        /// return the System Release program path
        /// </summary>
        /// <returns></returns>
        public string GetSystemReleaseProgramPath()
        {
            var installPath = GetSystemReleaseWorkstationProgramPath();
            if (!string.IsNullOrEmpty(installPath))
                return installPath;

            return GetSystemReleaseServerProgramPath();
        }


        public string GetSystemReleaseWorkstationProgramPath()
        {
            const string WortkstationRegistry = @"SOFTWARE\MYOB\System Release\Workstation";

            var path = string.Empty;
            try
            {
                var registryKey = Registry.LocalMachine.OpenSubKey(WortkstationRegistry);
                if (registryKey != null)
                {
                    path = registryKey.GetValue("PATH") as string;
                }

            }
            catch (Exception ex)
            {
                ShowRegistryExceptionMessage(WortkstationRegistry, ex.Message);
            }

            return path;
        }


        public string GetSystemReleaseServerProgramPath()
        {
            const string ServerRegistry = @"SOFTWARE\MYOB\System Release";

            var path = string.Empty;
            try
            {
                var registryKey = Registry.LocalMachine.OpenSubKey(ServerRegistry);
                if (registryKey != null)
                {
                    path = registryKey.GetValue("PATH") as string;
                }

            }
            catch (Exception ex)
            {
                ShowRegistryExceptionMessage(ServerRegistry, ex.Message);
            }

            return path;

        }

        #endregion #region GetSystemReleaseProgramPath

    }
}
