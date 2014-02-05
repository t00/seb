﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SebWindowsClient {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SEBUIStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SEBUIStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SebWindowsClient.SEBUIStrings", typeof(SEBUIStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The certificate needed to decrypt settings has not been found in the Windows certificate store. .
        /// </summary>
        internal static string certificateNotFoundInStore {
            get {
                return ResourceManager.GetString("certificateNotFoundInStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Permitted Processes Are Aleady Running.
        /// </summary>
        internal static string closeProcesses {
            get {
                return ResourceManager.GetString("closeProcesses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The processes below are already running, they need to be closed before starting the exam. Do you want to close those processes now (this may lead to loss of data!)? Otherwise SEB will quit and you can close those applications yourself before trying to start the exam again..
        /// </summary>
        internal static string closeProcessesQuestion {
            get {
                return ResourceManager.GetString("closeProcessesQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quit Safe Exam Browser.
        /// </summary>
        internal static string confirmQuitting {
            get {
                return ResourceManager.GetString("confirmQuitting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to quit SEB?.
        /// </summary>
        internal static string confirmQuittingQuestion {
            get {
                return ResourceManager.GetString("confirmQuittingQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating New Desktop Failed.
        /// </summary>
        internal static string createNewDesktopFailed {
            get {
                return ResourceManager.GetString("createNewDesktopFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SetThreadDesktop failed! Looks like the thread has hooks or windows in the current desktop..
        /// </summary>
        internal static string createNewDesktopFailedReason {
            get {
                return ResourceManager.GetString("createNewDesktopFailedReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot Decrypt Settings.
        /// </summary>
        internal static string decryptingSettingsFailed {
            get {
                return ResourceManager.GetString("decryptingSettingsFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You either entered the wrong password or these settings were saved with an incompatible SEB version..
        /// </summary>
        internal static string decryptingSettingsFailedReason {
            get {
                return ResourceManager.GetString("decryptingSettingsFailedReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Virtual Machine Detected.
        /// </summary>
        internal static string detectedVirtualMachine {
            get {
                return ResourceManager.GetString("detectedVirtualMachine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It is not allowed to run SEB on a virtual machine! SEB will quit now..
        /// </summary>
        internal static string detectedVirtualMachineForbiddenMessage {
            get {
                return ResourceManager.GetString("detectedVirtualMachineForbiddenMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the SEB administrator password used in these settings:.
        /// </summary>
        internal static string enterAdminPasswordRequired {
            get {
                return ResourceManager.GetString("enterAdminPasswordRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong password! Try again to enter the correct SEB administrator password from these settings:.
        /// </summary>
        internal static string enterAdminPasswordRequiredAgain {
            get {
                return ResourceManager.GetString("enterAdminPasswordRequiredAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only reconfigure SEB by entering the current SEB administrator password (because it was changed since installing SEB):.
        /// </summary>
        internal static string enterCurrentAdminPwdForReconfiguring {
            get {
                return ResourceManager.GetString("enterCurrentAdminPwdForReconfiguring", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong password! Try again to enter the correct current SEB administrator password:.
        /// </summary>
        internal static string enterCurrentAdminPwdForReconfiguringAgain {
            get {
                return ResourceManager.GetString("enterCurrentAdminPwdForReconfiguringAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter password used to encrypt these settings:.
        /// </summary>
        internal static string enterEncryptionPassword {
            get {
                return ResourceManager.GetString("enterEncryptionPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong password! Try again to enter the correct password used to encrypt these settings:.
        /// </summary>
        internal static string enterEncryptionPasswordAgain {
            get {
                return ResourceManager.GetString("enterEncryptionPasswordAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter password:.
        /// </summary>
        internal static string enterPassword {
            get {
                return ResourceManager.GetString("enterPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong password! Try again to enter the correct password:.
        /// </summary>
        internal static string enterPasswordAgain {
            get {
                return ResourceManager.GetString("enterPasswordAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error Decrypting Settings.
        /// </summary>
        internal static string errorDecryptingSettings {
            get {
                return ResourceManager.GetString("errorDecryptingSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SEB Windows service is stopped or the installed SEB got corrupted. Rebooting your machine or reinstalling SEB might help. Inform your exam administrator/supporter. The exam cannot be started, SEB will quit now..
        /// </summary>
        internal static string forceSebServiceMessage {
            get {
                return ResourceManager.GetString("forceSebServiceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SEB Windows Service Not Available.
        /// </summary>
        internal static string indicateMissingService {
            get {
                return ResourceManager.GetString("indicateMissingService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SEB Windows service is stopped or the installed SEB got corrupted. Rebooting your machine or reinstalling SEB might help. Inform your exam administrator/supporter. .
        /// </summary>
        internal static string indicateMissingServiceReason {
            get {
                return ResourceManager.GetString("indicateMissingServiceReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading Settings.
        /// </summary>
        internal static string loadingSettings {
            get {
                return ResourceManager.GetString("loadingSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading Settings Failed.
        /// </summary>
        internal static string loadingSettingsFailed {
            get {
                return ResourceManager.GetString("loadingSettingsFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These settings are corrupted and cannot be used..
        /// </summary>
        internal static string loadingSettingsFailedReason {
            get {
                return ResourceManager.GetString("loadingSettingsFailedReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you don&apos;t enter the right administrator password from these settings you cannot open them..
        /// </summary>
        internal static string loadingSettingsFailedWrongAdminPwd {
            get {
                return ResourceManager.GetString("loadingSettingsFailedWrongAdminPwd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading New SEB Settings Not Allowed!.
        /// </summary>
        internal static string loadingSettingsNotAllowed {
            get {
                return ResourceManager.GetString("loadingSettingsNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SEB is already running in exam mode and it is not allowed to interupt this by starting another exam. Finish the exam and quit SEB before starting another exam..
        /// </summary>
        internal static string loadingSettingsNotAllowedReason {
            get {
                return ResourceManager.GetString("loadingSettingsNotAllowedReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Locate Third Party Application.
        /// </summary>
        internal static string locatePermittedApplication {
            get {
                return ResourceManager.GetString("locatePermittedApplication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Encryption Credentials Chosen.
        /// </summary>
        internal static string noEncryptionChosen {
            get {
                return ResourceManager.GetString("noEncryptionChosen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You should either enter a password or choose a certificate to encrypt the settings file. Do you really want to save an unencrypted SEB file (not recommended for use in exams)?.
        /// </summary>
        internal static string noEncryptionChosenSaveUnencrypted {
            get {
                return ResourceManager.GetString("noEncryptionChosenSaveUnencrypted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Third Party Application Not Found.
        /// </summary>
        internal static string permittedApplicationNotFound {
            get {
                return ResourceManager.GetString("permittedApplicationNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The permitted application %s wasn&apos;t found on this system. If exam settings don&apos;t indicate a precise path, SEB can find applications installed in the Program Files or Windows System directory and those which are registered in the system..
        /// </summary>
        internal static string permittedApplicationNotFoundMessage {
            get {
                return ResourceManager.GetString("permittedApplicationNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong Password: Quitting SEB Failed.
        /// </summary>
        internal static string quittingFailed {
            get {
                return ResourceManager.GetString("quittingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only quit SEB by entering the correct quit password. Ask your exam supporter for the correct password. DO NOT RESET YOUR MACHINE! This may have undesired effects on your system&apos;s settings (see www.safeexambrowser.org/faq)..
        /// </summary>
        internal static string quittingFailedReason {
            get {
                return ResourceManager.GetString("quittingFailedReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Opening Settings for Reconfiguring Client.
        /// </summary>
        internal static string reconfiguringLocalSettings {
            get {
                return ResourceManager.GetString("reconfiguringLocalSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Opening Settings for Reconfiguring Client Failed.
        /// </summary>
        internal static string reconfiguringLocalSettingsFailed {
            get {
                return ResourceManager.GetString("reconfiguringLocalSettingsFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you don&apos;t enter the right administrator password from these settings you cannot use them..
        /// </summary>
        internal static string reconfiguringLocalSettingsFailedWrongAdminPwd {
            get {
                return ResourceManager.GetString("reconfiguringLocalSettingsFailedWrongAdminPwd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you don&apos;t enter the correct password these settings were encrypted with, you cannot use them..
        /// </summary>
        internal static string reconfiguringLocalSettingsFailedWrongPassword {
            get {
                return ResourceManager.GetString("reconfiguringLocalSettingsFailedWrongPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SEB Re-Configured.
        /// </summary>
        internal static string sebReconfigured {
            get {
                return ResourceManager.GetString("sebReconfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Local settings of SEB have been reconfigured. Do you want to start working with SEB now? (clicking &quot;No&quot; will quit SEB).
        /// </summary>
        internal static string sebReconfiguredQuestion {
            get {
                return ResourceManager.GetString("sebReconfiguredQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SEB Needs to be Restarted.
        /// </summary>
        internal static string sebReconfiguredRestartNeeded {
            get {
                return ResourceManager.GetString("sebReconfiguredRestartNeeded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Local settings of SEB have been reconfigured, but it needs to be restarted for the changed &quot;Create New Desktop&quot; setting to take effect. SEB will quit now, please restart it manually..
        /// </summary>
        internal static string sebReconfiguredRestartNeededReason {
            get {
                return ResourceManager.GetString("sebReconfiguredRestartNeededReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Opening New Settings Failed.
        /// </summary>
        internal static string settingsNotUsable {
            get {
                return ResourceManager.GetString("settingsNotUsable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These settings cannot be used. They may have been created by an newer, incompatible version of SEB or are corrupted..
        /// </summary>
        internal static string settingsNotUsableReason {
            get {
                return ResourceManager.GetString("settingsNotUsableReason", resourceCulture);
            }
        }
    }
}
