using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Linq;

// -------------------------------------------------------------
//     Viktor Tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------

namespace SebWindowsClient.Configuration
{
    public sealed class SEBConfigurationSection : ConfigurationSection
    {
        //private const int _APP_HOURS_IN_CACHE = 6;
        private static volatile SEBConfigurationSection _SEBSettings = null;
        private static object _syncRoot = new Object();

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current instance of the configuration file.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static SEBConfigurationSection Instance
        {
            get
            {
                if (_SEBSettings == null)
                {
                    lock (_syncRoot)
                    {
                        if (_SEBSettings == null) _SEBSettings = Load();
                    }

                }
                return _SEBSettings;
            }
        }

        ///// ------------------------------------------------------------------------------------
        ///// <summary>
        ///// SebStarterIni section.
        ///// </summary>
        ///// ------------------------------------------------------------------------------------
        //[ConfigurationProperty("SebStarterIni")]
        //public String SebStarterIni
        //{
        //    get { return (String)this["SebStarterIni"]; }
        //    set { this["SebStarterIni"] = value; }
        //}

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// RegistryValues section.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("RegistryValues")]
        public SEBRegistryValuesConfigurationElement RegistryValues
        {
            get { return (SEBRegistryValuesConfigurationElement)this["RegistryValues"]; }
            set { this["RegistryValues"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// SecurityOptions sections.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("SecurityOptions")]
        public SEBSecurityOptionsConfigurationElement SecurityOptions
        {
            get { return (SEBSecurityOptionsConfigurationElement)this["SecurityOptions"]; }
            set { this["SecurityOptions"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// OnlineExam section.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("OnlineExam")]
        public SEBOnlineExamConfigurationElement OnlineExam
        {
            get { return (SEBOnlineExamConfigurationElement)this["OnlineExam"]; }
            set { this["cache"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// OtherOptions section.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("OtherOptions")]
        public SEBOtherOptionsConfigurationElement OtherOptions
        {
            get { return (SEBOtherOptionsConfigurationElement)this["OtherOptions"]; }
            set { this["urls"] = value; }
        }

        public static void encodeConfiguration()
        {
            System.Configuration.Configuration config;
            ConfigurationSection configSection;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configSection = config.GetSection("SEBSettingsGroup");
            //configSection = (SEBConfigurationSection)ConfigurationManager.GetSection("SEBSettingsGroup");

            if (configSection != null)
            {
                if (!(configSection.ElementInformation.IsLocked))
                {
                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save(ConfigurationSaveMode.Full);
                }
            }
        }

        public static void decodeConfiguration()
        {
            System.Configuration.Configuration config;
            ConfigurationSection configSection;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configSection = config.GetSection("OnlineExam");
            if (configSection != null)
            {
                if (!(configSection.ElementInformation.IsLocked))
                {
                    configSection.SectionInformation.UnprotectSection();
                    config.Save(ConfigurationSaveMode.Full);
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Load the configuration from the cache if possible, otherwise from the configuration file.
        /// </summary>
        /// <returns>The VSD Configuration settings.</returns>
        /// ------------------------------------------------------------------------------------
        private static SEBConfigurationSection Load()
        {
            if (_SEBSettings == null)
            {

                if (_SEBSettings == null)
                {
                    try
                    {
                        _SEBSettings = (SEBConfigurationSection)ConfigurationManager.GetSection("SEBSettingsGroup/SEBSettings");
                        // used only for UnitTesting
                        if (_SEBSettings == null)
                        {
                            ConfigurationFileMap fileMap = new ConfigurationFileMap(@"F:\SebWindowsClient\App.config"); //Path to your config file
                            System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
                            _SEBSettings = (SEBConfigurationSection)configuration.GetSection("SEBSettingsGroup/SEBSettings");
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString(); ;
                        throw new SEBConfigurationException("Cannot load SEBSettingsGroup/SEBSettings, check config!");
                    }
                 }
            }
            return _SEBSettings;
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBRegistryValuesConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableSwitchUser")]
        public int EnableSwitchUser
        {
            get { return (int)this["EnableSwitchUser"]; }
            set { this["EnableSwitchUser"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableLockThisComputer")]
        public int EnableLockThisComputer
        {
            get { return (int)this["EnableLockThisComputer"]; }
            set { this["EnableLockThisComputer"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableChangeAPassword")]
        public int EnableChangeAPassword
        {
            get { return (int)this["EnableChangeAPassword"]; }
            set { this["EnableChangeAPassword"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableStartTaskManager")]
        public int EnableStartTaskManager
        {
            get { return (int)this["EnableStartTaskManager"]; }
            set { this["EnableStartTaskManager"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableLogOff")]
        public int EnableLogOff
        {
            get { return (int)this["EnableLogOff"]; }
            set { this["EnableLogOff"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableShutDown")]
        public int EnableShutDown
        {
            get { return (int)this["EnableShutDown"]; }
            set { this["EnableShutDown"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableEaseOfAccess")]
        public int EnableEaseOfAccess
        {
            get { return (int)this["EnableEaseOfAccess"]; }
            set { this["EnableEaseOfAccess"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EnableVmWareClientShade")]
        public int EnableVmWareClientShade
        {
            get { return (int)this["EnableVmWareClientShade"]; }
            set { this["EnableVmWareClientShade"] = value; }
        }
     }

    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBSecurityOptionsConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("AllowVirtualMachine")]
        public int AllowVirtualMachine
        {
            get { return (int)this["AllowVirtualMachine"]; }
            set { this["AllowVirtualMachine"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("ForceWindowsService")]
        public int ForceWindowsService
        {
            get { return (int)this["ForceWindowsService"]; }
            set { this["ForceWindowsService"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("CreateNewDesktop")]
        public int CreateNewDesktop
        {
            get { return (int)this["CreateNewDesktop"]; }
            set { this["CreateNewDesktop"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("ShowSebApplicationChooser")]
        public int ShowSebApplicationChooser
        {
            get { return (int)this["ShowSebApplicationChooser"]; }
            set { this["ShowSebApplicationChooser"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("HookMessages")]
        public int HookMessages
        {
            get { return (int)this["HookMessages"]; }
            set { this["HookMessages"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("EditRegistry")]
        public int EditRegistry
        {
            get { return (int)this["EditRegistry"]; }
            set { this["EditRegistry"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("MonitorProcesses")]
        public int MonitorProcesses
        {
            get { return (int)this["MonitorProcesses"]; }
            set { this["MonitorProcesses"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("ShutdownAfterAutostartProcessTerminates")]
        public int ShutdownAfterAutostartProcessTerminates
        {
            get { return (int)this["ShutdownAfterAutostartProcessTerminates"]; }
            set { this["ShutdownAfterAutostartProcessTerminates"] = value; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    [ConfigurationCollection(typeof(SEBPermittedApplicationConfigurationElement))]
    public sealed class SEBPermittedApplicationsConfigurationElementCollection : ConfigurationElementCollection
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override ConfigurationElement CreateNewElement()
        {
            return new SEBPermittedApplicationConfigurationElement();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SEBPermittedApplicationConfigurationElement)element).Key;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        public new string this[string key]
        {
            get { return ((SEBPermittedApplicationConfigurationElement)this.BaseGet(key)).Path; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBPermittedApplicationConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("path", IsRequired = true)]
        //    [RegexStringValidator(@"[a-zA-z]:\\[\w.]+\S*")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    [ConfigurationCollection(typeof(SEBBrowserConfigurationElement))]
    public sealed class SEBBrowserConfigurationElementCollection : ConfigurationElementCollection
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override ConfigurationElement CreateNewElement()
        {
            return new SEBBrowserConfigurationElement();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SEBBrowserConfigurationElement)element).Key;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        public new string this[string key]
        {
            get { return ((SEBBrowserConfigurationElement)this.BaseGet(key)).Path; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBBrowserConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("path", IsRequired = true)]
        //    [RegexStringValidator(@"[a-zA-z]:\\[\w.]+\S*")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }

    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    [ConfigurationCollection(typeof(SEBExamUrlConfigurationElement))]
    public sealed class SEBExamUrlsConfigurationElementCollection : ConfigurationElementCollection
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override ConfigurationElement CreateNewElement()
        {
            return new SEBExamUrlConfigurationElement();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SEBExamUrlConfigurationElement)element).Key;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        public new string this[string key]
        {
            get { return ((SEBExamUrlConfigurationElement)this.BaseGet(key)).Path; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBExamUrlConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("path", IsRequired = true)]
        //    [RegexStringValidator(@"[a-zA-z]:\\[\w.]+\S*")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBAutostartProcessConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("path", IsRequired = true)]
        //    [RegexStringValidator(@"[a-zA-z]:\\[\w.]+\S*")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }


    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBOnlineExamConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("SebBrowser")]
        public SEBBrowserConfigurationElementCollection SebBrowser
        {
            get { return (SEBBrowserConfigurationElementCollection)this["SebBrowser"]; }
            set { this["SebBrowser"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("AutostartProcess")]
        public SEBAutostartProcessConfigurationElement AutostartProcess
        {
            get { return (SEBAutostartProcessConfigurationElement)this["AutostartProcess"]; }
            set { this["AutostartProcess"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("ExamUrl")]
        public SEBExamUrlsConfigurationElementCollection ExamUrl
        {
            get { return (SEBExamUrlsConfigurationElementCollection)this["ExamUrl"]; }
            set { this["ExamUrl"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("PermittedApplications")]
        public SEBPermittedApplicationsConfigurationElementCollection PermittedApplications
        {
            get { return (SEBPermittedApplicationsConfigurationElementCollection)this["PermittedApplications"]; }
            set { this["PermittedApplications"] = value; }
        }

     }

    /// ------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ------------------------------------------------------------------------------------
    public sealed class SEBOtherOptionsConfigurationElement : ConfigurationElement
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("WriteLogFileSebStarterLog")]
        public SEBOtherOptionConfigurationElement WriteLogFileSebStarterLog
        {
            get { return (SEBOtherOptionConfigurationElement)this["WriteLogFileSebStarterLog"]; }
            set { this["WriteLogFileSebStarterLog"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("HookDll")]
        public SEBOtherOptionConfigurationElement HookDll
        {
            get { return (SEBOtherOptionConfigurationElement)this["HookDll"]; }
            set { this["HookDll"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("Win9xKillExplorer")]
        public SEBOtherOptionConfigurationElement Win9xKillExplorer
        {
            get { return (SEBOtherOptionConfigurationElement)this["Win9xKillExplorer"]; }
            set { this["Win9xKillExplorer"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("Win9xScreenSaverRunning")]
        public SEBOtherOptionConfigurationElement Win9xScreenSaverRunning
        {
            get { return (SEBOtherOptionConfigurationElement)this["Win9xScreenSaverRunning"]; }
            set { this["Win9xScreenSaverRunning"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("StrongKillProcessesBefore")]
        public SEBOtherOptionConfigurationElement StrongKillProcessesBefore
        {
            get { return (SEBOtherOptionConfigurationElement)this["StrongKillProcessesBefore"]; }
            set { this["StrongKillProcessesBefore"] = value; }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [ConfigurationProperty("StrongKillProcessesAfter")]
        public SEBOtherOptionConfigurationElement StrongKillProcessesAfter
        {
            get { return (SEBOtherOptionConfigurationElement)this["StrongKillProcessesAfter"]; }
            set { this["StrongKillProcessesAfter"] = value; }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public sealed class SEBOtherOptionConfigurationElement : ConfigurationElement
        {
            public List<string> Values { get; private set; }

            protected override void DeserializeElement(XmlReader reader, bool b)
            {
                string extensions = reader.ReadElementContentAs(typeof(string), null) as string;
                if (extensions != null) this.Values = extensions.Split(',', ';').ToList();
            }
        }

    }
}