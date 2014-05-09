using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.DiagnosticsUtils;

namespace SebWindowsClient.ConfigurationUtils
{
    public class XULRunnerConfig
    {
        public Prefs prefs = new Prefs();
        public string seb_url;
        public bool seb_mainWindow_titlebar_enabled;
        public bool seb_trusted_content;
        public bool seb_pattern_regex;
        public string seb_blacklist_pattern;
        public string seb_whitelist_pattern;
        public bool seb_locked;
        public string seb_lock_keycode;
        public string seb_lock_modifiers;
        public bool seb_unlock_enabled;
        public string seb_unlock_keycode;
        public string seb_unlock_modifiers;
        public string seb_shutdown_keycode;
        public string seb_shutdown_modifiers;
        public string seb_load;
        public string seb_load_referrer_instring;
        public string seb_load_keycode;
        public string seb_load_modifiers;
        public string seb_reload_keycode;
        public string seb_reload_modifiers;
        public int seb_net_max_times;
        public int seb_net_timeout;
        public int seb_restart_mode;
        public string seb_restart_keycode;
        public string seb_restart_modifiers;
        public bool seb_popupWindows_titlebar_enabled;
        public int seb_openwin_width;
        public int seb_openwin_height;
        public string seb_showall_keycode;
        public bool seb_distinct_popup;
        public bool seb_removeProfile;
    }

    public class Prefs
    {
        public string general_useragent_override;
    }
    /// <summary>
    /// JSON Serialization and Deserialization Assistant Class
    /// </summary>
    public class SEBXulRunnerSettings
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static void XULRunnerConfigSerialize(XULRunnerConfig objXULRunnerConfig, string path)
        {
            //string json = "{\"prefs\":{\"general.useragent.override\":\"SEB\"},\"seb.url\":\"http://www.safeexambrowser.org\",\"seb.mainWindow.titlebar.enabled\":false,\"seb.trusted.content\":true,\"seb.pattern.regex\":false,\"seb.blacklist.pattern\":\"\",\"seb.whitelist.pattern\":\"\",\"seb.locked\":true,\"seb.lock.keycode\":\"VK_F2\",\"seb.lock.modifiers\":\"controlshift\",\"seb.unlock.enabled\":false,\"seb.unlock.keycode\":\"VK_F3\",\"seb.unlock.modifiers\":\"controlshift\",\"seb.shutdown.keycode\":\"VK_F4\",\"seb.shutdown.modifiers\":\"controlshift\",\"seb.load\":\"\",\"seb.load.referrer.instring\":\"\",\"seb.load.keycode\":\"VK_F6\",\"seb.load.modifiers\":\"controlshift\",\"seb.reload.keycode\":\"VK_F5\",\"seb.reload.modifiers\":\"\",\"seb.net.max.times\":3,\"seb.net.timeout\":10000,\"seb.restart.mode\":2,\"seb.restart.keycode\":\"VK_F9\",\"seb.restart.modifiers\":\"controlshift\",\"seb.popupWindows.titlebar.enabled\":false,\"seb.openwin.width\":800,\"seb.openwin.height\":600,\"seb.showall.keycode\":\"VK_F1\",\"seb.distinct.popup\":false,\"seb.removeProfile\":false}";
            //Serialise 
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string js = serializer.Serialize(objXULRunnerConfig);
            js = js.Replace("_", ".");
            //Write to config.json
            File.Delete(path);
            FileStream fs = File.Open(path,FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(js);
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static XULRunnerConfig XULRunnerConfigDeserialize(string path)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            FileStream fs = File.OpenRead(path);
            fs.Position = 0;
            StreamReader sr = new StreamReader(fs);
            string sXULRunnerConfig = sr.ReadToEnd();
            sr.Close();
            fs.Close();

            sXULRunnerConfig = sXULRunnerConfig.Replace("\n", String.Empty);
            sXULRunnerConfig = sXULRunnerConfig.Replace("\r", String.Empty);
            sXULRunnerConfig = sXULRunnerConfig.Replace(" ", String.Empty);
            sXULRunnerConfig = sXULRunnerConfig.Replace("\t", String.Empty);
            sXULRunnerConfig = sXULRunnerConfig.Replace(".", "_");

            XULRunnerConfig objXULRunnerConfig = (XULRunnerConfig)serializer.Deserialize(sXULRunnerConfig, typeof(XULRunnerConfig));

            return objXULRunnerConfig;
        }
        /// <summary>
        /// JSON Serialization of Settings Dictionary
        /// </summary>
        public static string XULRunnerConfigDictionarySerialize(Dictionary<string, object> xulRunnerSettings)
        {
            // Add current Browser Exam Key
            if ((bool)xulRunnerSettings[SEBSettings.KeySendBrowserExamKey])
            {
                string browserExamKey = SEBProtectionController.ComputeBrowserExamKey();
                xulRunnerSettings[SEBSettings.KeyBrowserExamKey] = browserExamKey;
            }
            Logger.AddInformation("Socket: " + xulRunnerSettings[SEBSettings.KeyBrowserMessagingSocket].ToString(),null,null);
            // Serialise 
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonSettings = serializer.Serialize(xulRunnerSettings);
            // Convert to Base64 String
            byte[] bytesJson = Encoding.UTF8.GetBytes(jsonSettings);
            string base64Json = Convert.ToBase64String(bytesJson);
            //// remove the two chars "==" from the end of the string
            //string base64Json = base64String.Substring(0, base64String.Length - 2);

            return base64Json;
        }

    }
}
