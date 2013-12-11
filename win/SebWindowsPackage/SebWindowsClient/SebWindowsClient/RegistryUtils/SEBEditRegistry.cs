// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.ConfigurationUtils;

namespace SebWindowsClient.RegistryUtils
{
    public class SEBEditRegistry
    {
        private const string REGISTRY_VALUE_ENABLED = "1";
        private const string REGISTRY_VALUE_DISABLED = "0";


        private Dictionary<string, int> _resetRegValues;

        public Dictionary<string, int> ResetRegValues
        {
            get { return _resetRegValues; }
            set { _resetRegValues = value; }
        }

        public void addResetRegValues(string key, int value)
        {
            _resetRegValues.Add(key, value);
        }

        private RegistryKey openedHKLMPolicySystem = null;
        private RegistryKey openedHKCUPolicySystem = null;
        private RegistryKey openedHKCUPolicyExplorer = null;
        private RegistryKey openedHKLMUtilmanExe = null;
        private RegistryKey openedHKCUVmWareClient = null;

        public SEBEditRegistry()
        {
            _resetRegValues = new Dictionary<string, int>();
        }


        /// <summary>
        /// Opens or creates registry keys and sets the regisrty key values.
        /// </summary>
        /// <returns></returns>
        public bool EditRegistry()
        {
            try
	        {
		        // Open the Windows Registry Key
		        // HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System
                this.openedHKLMPolicySystem = OpenRegistryKey(SEBGlobalConstants.HKLM, SEBGlobalConstants.KEY_POLICIES_SYSTEM, true);
                if (this.openedHKLMPolicySystem != null)
		        {
                    Logger.AddInformation("HandleOpenRegistryKey(HKLM, System) succeeded", this, null);
		        }
		        else
		        {
                    Logger.AddError("HandleOpenRegistryKey(HKLM, System) failed.", this, null);
			        return false;
		        }

		        // Open the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System
                this.openedHKCUPolicySystem = OpenRegistryKey(SEBGlobalConstants.HKCU, SEBGlobalConstants.KEY_POLICIES_SYSTEM, true);
                if (this.openedHKCUPolicySystem != null)
		        {
			        Logger.AddInformation("HandleOpenRegistryKey(HKCU, System) succeeded", this, null);
		        }
		        else
		        {
			        Logger.AddError("HandleOpenRegistryKey(HKCU, System) failed.", this, null);
			        return false;
		        }

		        // Open the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer
                this.openedHKCUPolicyExplorer = OpenRegistryKey(SEBGlobalConstants.HKCU, SEBGlobalConstants.KEY_POLICIES_EXPLORER, true);
                if (this.openedHKCUPolicyExplorer != null)
		        {
			        Logger.AddInformation("HandleOpenRegistryKey(HKCU, Explorer) succeeded", this, null);
		        }
		        else
		        {
			        Logger.AddError("HandleOpenRegistryKey(HKCU, Explorer) failed.", this, null);
			        return false;
		        }


		        // Open the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\VMware, Inc.\VMware VDM\Client
                this.openedHKCUVmWareClient = OpenRegistryKey(SEBGlobalConstants.HKCU, SEBGlobalConstants.KEY_VM_WARE_CLIENT, true);
                if (this.openedHKCUVmWareClient != null)
		        {
			        Logger.AddInformation("HandleOpenRegistryKey(HKCU, VmWareClient) succeeded", this, null);
		        }
		        else
		        {
			        Logger.AddError("HandleOpenRegistryKey(HKCU, VmWareClient) failed.", this, null);
			        return false;
		        }


		        // Open the Windows Registry Key
		        // HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Utilman.exe
                this.openedHKLMUtilmanExe = OpenRegistryKey(SEBGlobalConstants.HKLM, SEBGlobalConstants.KEY_UTILMAN_EXE, true);
                if (this.openedHKLMUtilmanExe != null)
		        {
			        Logger.AddInformation("HandleOpenRegistryKey(HKLM, UtilmanExe) succeeded", this, null);
		        }
		        else
		        {
			        Logger.AddError("HandleOpenRegistryKey(HKLM, UtilmanExe) failed", this, null);
			        return false;
		        }


		        // Set the Windows Registry Key
		        // HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System\HideFastUserSwitching
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableSwitchUser)[SEBSettings.KeyInsideSebEnableSwitchUser])
		        {
                    if (SetRegistryKeyValue(this.openedHKLMPolicySystem, SEBGlobalConstants.VAL_HIDE_FAST_USER_SWITCHING, REGISTRY_VALUE_DISABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(HideFastUserSwitching) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(HideFastUserSwitching) failed", this, null);
				        return false;
			        }
		        } 

		        // Set the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableLockWorkstation
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableLockThisComputer)[SEBSettings.KeyInsideSebEnableLockThisComputer]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKCUPolicySystem, SEBGlobalConstants.VAL_DISABLE_LOCK_WORKSTATION, REGISTRY_VALUE_DISABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(DisableLockWorkstation) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(DisableLockWorkstation) failed", this, null);
				        return false;
			        }
		        } 

		        // Set the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableChangePassword
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableChangeAPassword)[SEBSettings.KeyInsideSebEnableChangeAPassword]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKCUPolicySystem, SEBGlobalConstants.VAL_DISABLE_CHANGE_PASSWORD, REGISTRY_VALUE_DISABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(DisableChangePassword) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(DisableChangePassword) failed", this, null);
				        return false;
			        }
		        } 

		        // Set the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableStartTaskManager)[SEBSettings.KeyInsideSebEnableStartTaskManager]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKCUPolicySystem, SEBGlobalConstants.VAL_DISABLE_TASK_MANAGER, REGISTRY_VALUE_DISABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(DisableTaskMgr) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(DisableTaskMgr) failed", this, null);
				        return false;
			        }
		        } 


		        // Set the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoLogoff
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableLogOff)[SEBSettings.KeyInsideSebEnableLogOff]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKCUPolicyExplorer, SEBGlobalConstants.VAL_NO_LOG_OFF, REGISTRY_VALUE_DISABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(NoLogoff) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(NoLogoff) failed", this, null);
				        return false;
			        }
		        } 

		        // Set the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoClose
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableShutDown)[SEBSettings.KeyInsideSebEnableShutDown]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKCUPolicyExplorer, SEBGlobalConstants.VAL_NO_CLOSE, REGISTRY_VALUE_DISABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(NoClose) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(NoClose) failed", this, null);
				        return false;
			        }
		        } 

		        // Set the Windows Registry Key
		        // HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Utilman.exe\Debugger
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableEaseOfAccess)[SEBSettings.KeyInsideSebEnableEaseOfAccess]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKLMUtilmanExe, SEBGlobalConstants.VAL_ENABLE_EASE_OF_ACCESS, REGISTRY_VALUE_ENABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(EnableEaseOfAccess) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(EnableEaseOfAccess) failed", this, null);
				        return false;
			        }
		        } 

		        // Set the Windows Registry Key
		        // HKEY_CURRENT_USER\Software\VMware, Inc.\VMware VDM\Client\EnableShade
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableVmWareClientShade)[SEBSettings.KeyInsideSebEnableVmWareClientShade]) 
		        {
                    if (SetRegistryKeyValue(this.openedHKCUVmWareClient, SEBGlobalConstants.VAL_ENABLE_SHADE, REGISTRY_VALUE_ENABLED))
			        {
				        Logger.AddInformation("SetRegistryKeyValue(EnableShade) succeeded", this, null);
			        }
			        else
			        {
				        Logger.AddError("SetRegistryKeyValue(EnableShade) failed", this, null);
				        return false;
			        }
		        }

            }
            catch (Exception ex)
            {
                Logger.AddError("Error ocurred by editing registry.", this, ex, ex.Message);
                return false;
            }

	        return true;
        }

        /// <summary>
        /// Opens or creates registry key.
        /// </summary>
        /// <returns></returns>
        private RegistryKey OpenRegistryKey(String hKey, String subKey, bool bCreate)
        {
            RegistryKey regKey = null;

            try
	        {
                if (hKey.CompareTo(SEBGlobalConstants.HKLM) == 0)
                {
                    regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subKey, true);
                }
                else if (hKey.CompareTo(SEBGlobalConstants.HKCU) == 0)
                {
                    regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, true);
                }

		        if (regKey == null)
                {
					if (bCreate)
					{
                        if (hKey.CompareTo(SEBGlobalConstants.HKLM) == 0)
                        {
                            Microsoft.Win32.Registry.LocalMachine.CreateSubKey(subKey);
                            regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subKey, true);
                        }
                        else if (hKey.CompareTo(SEBGlobalConstants.HKCU) == 0)
                        {
                            Microsoft.Win32.Registry.CurrentUser.CreateSubKey(subKey);
                            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, true);
                        }
					}
		        }

            }  catch (UnauthorizedAccessException uex) {
                Logger.AddError("Not enough rights for editing registry.", this, uex, uex.Message);
            } catch (Exception ex) {
                Logger.AddError("Error openning key.", this, ex, ex.Message);
            }

            return regKey;
        }

        /// <summary>
        /// Sets registry key value.
        /// </summary>
        /// <returns></returns>
        private bool SetRegistryKeyValue(RegistryKey regKey, String valueName, String value)
        {
            _resetRegValues.Add(valueName, 0);
            try
	        {
                string oldValue = (string)regKey.GetValue(valueName);

                if (oldValue != null && oldValue.CompareTo(value) == 0)
		        {
			        //is already set. don't touch this key value in the ResetRegistry function
 		        }
		        else
		        {
                    regKey.SetValue(valueName, value, RegistryValueKind.String);
                    _resetRegValues[valueName] = 1;
		        }

                return true;
            }
            catch (UnauthorizedAccessException uex)
            {
                Logger.AddError("Not enough rights for editing registry.", this, uex, uex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logger.AddError("Error setting key value.", this, ex, ex.Message);
                return false;
            }

        } // end HandleSetRegistryKeyValue()


        /// <summary>
        /// Resets registry keys values.
        /// </summary>
        /// <returns></returns>
        public bool ResetRegistry()
        {
	        try
	        {

                if (this.openedHKLMPolicySystem != null)
                {
                    if (_resetRegValues[SEBGlobalConstants.VAL_HIDE_FAST_USER_SWITCHING] == 1) this.openedHKLMPolicySystem.DeleteValue(SEBGlobalConstants.VAL_HIDE_FAST_USER_SWITCHING);
                }
                if (this.openedHKCUPolicySystem != null)
                {
                    if (_resetRegValues[SEBGlobalConstants.VAL_DISABLE_LOCK_WORKSTATION] == 1) this.openedHKCUPolicySystem.DeleteValue(SEBGlobalConstants.VAL_DISABLE_LOCK_WORKSTATION);
                    if (_resetRegValues[SEBGlobalConstants.VAL_DISABLE_CHANGE_PASSWORD] == 1) this.openedHKCUPolicySystem.DeleteValue(SEBGlobalConstants.VAL_DISABLE_CHANGE_PASSWORD);
                    if (_resetRegValues[SEBGlobalConstants.VAL_DISABLE_TASK_MANAGER] == 1) this.openedHKCUPolicySystem.DeleteValue(SEBGlobalConstants.VAL_DISABLE_TASK_MANAGER);
                }
                if (this.openedHKCUPolicyExplorer != null)
                {
                    if (_resetRegValues[SEBGlobalConstants.VAL_NO_LOG_OFF] == 1) this.openedHKCUPolicyExplorer.DeleteValue(SEBGlobalConstants.VAL_NO_LOG_OFF);
                    if (_resetRegValues[SEBGlobalConstants.VAL_NO_CLOSE] == 1) this.openedHKCUPolicyExplorer.DeleteValue(SEBGlobalConstants.VAL_NO_CLOSE);
                }
                if (this.openedHKLMUtilmanExe != null)
                {
                    if (_resetRegValues[SEBGlobalConstants.VAL_ENABLE_EASE_OF_ACCESS] == 1) this.openedHKLMUtilmanExe.DeleteValue(SEBGlobalConstants.VAL_ENABLE_EASE_OF_ACCESS);
                }
                if (this.openedHKLMPolicySystem != null)
                {
                    if (_resetRegValues[SEBGlobalConstants.VAL_ENABLE_SHADE] == 1) this.openedHKLMPolicySystem.DeleteValue(SEBGlobalConstants.VAL_ENABLE_SHADE);
                }

                _resetRegValues.Clear();
	        }

            catch (Exception ex)
            {
                Logger.AddError("Error reseting registry.", this, ex, ex.Message);
                return false;
            }

	        return true;
        }


    }
}
