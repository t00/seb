﻿//
//  SEBXulRunnerSettings.cs
//  SafeExamBrowser
//
//  Copyright (c) 2010-2015 Viktor Tomas, Dirk Bauer, Daniel R. Schneider, Pascal Wyss,
//  ETH Zurich, Educational Development and Technology (LET),
//  based on the original idea of Safe Exam Browser
//  by Stefan Schneider, University of Giessen
//  Project concept: Thomas Piendl, Daniel R. Schneider,
//  Dirk Bauer, Kai Reuter, Tobias Halbherr, Karsten Burger, Marco Lehre,
//  Brigitte Schmucki, Oliver Rahs. French localization: Nicolas Dunand
//
//  ``The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License at
//  http://www.mozilla.org/MPL/
//
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations
//  under the License.
//
//  The Original Code is Safe Exam Browser for Windows.
//
//  The Initial Developers of the Original Code are Viktor Tomas, 
//  Dirk Bauer, Daniel R. Schneider, Pascal Wyss.
//  Portions created by Viktor Tomas, Dirk Bauer, Daniel R. Schneider, Pascal Wyss
//  are Copyright (c) 2010-2014 Viktor Tomas, Dirk Bauer, Daniel R. Schneider, 
//  Pascal Wyss, ETH Zurich, Educational Development and Technology (LET), 
//  based on the original idea of Safe Exam Browser
//  by Stefan Schneider, University of Giessen. All Rights Reserved.
//
//  Contributor(s): ______________________________________.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SebShared.CryptographyUtils;
using SebShared;
using SebShared.DiagnosticUtils;
using SebWindowsClient.XULRunnerCommunication;

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
        public static string XULRunnerConfigDictionarySerialize(IDictionary<string, object> xulRunnerSettings)
        {
            // Add current Browser Exam Key
            if ((bool)xulRunnerSettings[SebSettings.KeySendBrowserExamKey])
            {
				var binDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                var browserExamKey = SebProtectionController.ComputeBrowserExamKey(SebInstance.Settings, binDir);
                xulRunnerSettings[SebSettings.KeyBrowserExamKey] = browserExamKey;
            }

            // Eventually update setting 
            if (SebInstance.Settings.Get<bool>(SebSettings.KeyRestartExamUseStartURL)) 
            {
                xulRunnerSettings[SebSettings.KeyRestartExamURL] = xulRunnerSettings[SebSettings.KeyStartURL];
            }

			// Enable messaging socket
			xulRunnerSettings[SebSettings.KeyBrowserMessagingSocket] = SEBXULRunnerWebSocketServer.ServerAddress;

            // Check if URL filter is enabled and send according keys to XULRunner seb only if it is
            if ((bool)xulRunnerSettings[SebSettings.KeyURLFilterEnable] == false)
            {
                xulRunnerSettings[SebSettings.KeyUrlFilterBlacklist] = "";
                xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist] = "";
            }
            else
            {
                // URL filter is enabled: Set trusted content flag to same value (what actually doesn't make sense, but it's implemented wrong in seb winctrl.jsm)
                xulRunnerSettings[SebSettings.KeyUrlFilterTrustedContent] = (bool)xulRunnerSettings[SebSettings.KeyURLFilterEnableContentFilter];

                //add the starturl to the whitelist if not yet added
                if (!xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist].ToString().Contains(xulRunnerSettings[SebSettings.KeyStartURL].ToString()))
                    if (!String.IsNullOrWhiteSpace(xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist].ToString()))
                    {
                        xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist] += @";";
                    }
                    xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist] += xulRunnerSettings[SebSettings.KeyStartURL].ToString();

                //Add the socket address if content filter is enabled
                if ((bool)xulRunnerSettings[SebSettings.KeyURLFilterEnableContentFilter] == true)
                {
                    if (!String.IsNullOrWhiteSpace(xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist].ToString()))
                    {
                        xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist] += @";";
                    }
                    //Add the Socket address with http protocoll instead of ws protocoll for the injected iframe
                    xulRunnerSettings[SebSettings.KeyUrlFilterWhitelist] += String.Format("http://{0}", SEBXULRunnerWebSocketServer.ServerAddress.Substring(5));
                }
            }

			Logger.AddInformation("Socket: " + SEBXULRunnerWebSocketServer.ServerAddress);

            // Expand environment variables in paths which XULRunner seb is processing
            string downloadDirectoryWin = (string)xulRunnerSettings[SebSettings.KeyDownloadDirectoryWin];
            downloadDirectoryWin = Environment.ExpandEnvironmentVariables(downloadDirectoryWin);
            //downloadDirectoryWin = downloadDirectoryWin.Replace(@"\", @"\\");
            xulRunnerSettings[SebSettings.KeyDownloadDirectoryWin] = downloadDirectoryWin;

            // Add proper browser user agent string to XULRunner seb settings

            if((int) xulRunnerSettings[SebSettings.KeyBrowserUserAgentTouchMode] == 2)
            {
                var custom = xulRunnerSettings[SebSettings.KeyBrowserUserAgentDesktopModeCustom];
                if(string.IsNullOrEmpty((string) custom))
                {
                    xulRunnerSettings.Remove(SebSettings.KeyBrowserUserAgent);
                }
                else
                {
                    xulRunnerSettings[SebSettings.KeyBrowserUserAgent] = xulRunnerSettings[SebSettings.KeyBrowserUserAgentTouchModeCustom];
                }
            }
            else
            {
                if((bool) xulRunnerSettings[SebSettings.KeyTouchOptimized])
                {
                    // Set correct task bar height according to display dpi
                    xulRunnerSettings[SebSettings.KeyTaskBarHeight] = (int) Math.Round((int) xulRunnerSettings[SebSettings.KeyTaskBarHeight]*1.7);

                    if((int) xulRunnerSettings[SebSettings.KeyBrowserUserAgentTouchMode] == 0)
                    {
                        xulRunnerSettings[SebSettings.KeyBrowserUserAgent] = SebConstants.BROWSER_USERAGENT_TOUCH;
                    }
                    else if((int) xulRunnerSettings[SebSettings.KeyBrowserUserAgentTouchMode] == 1)
                    {
                        xulRunnerSettings[SebSettings.KeyBrowserUserAgent] = SebConstants.BROWSER_USERAGENT_TOUCH_IPAD;
                    }
                }
                else
                {
                    if((int) xulRunnerSettings[SebSettings.KeyBrowserUserAgentDesktopMode] == 0)
                    {
                        xulRunnerSettings[SebSettings.KeyBrowserUserAgent] = SebConstants.BROWSER_USERAGENT_DESKTOP;
                    }
                }
                xulRunnerSettings[SebSettings.KeyBrowserUserAgent] += "  " + SebConstants.BROWSER_USERAGENT_SEB + " " + Application.ProductVersion;
            }

            // Set onscreen keyboard settings flag when touch optimized is enabled
            xulRunnerSettings[SebSettings.KeyBrowserScreenKeyboard] = (bool)xulRunnerSettings[SebSettings.KeyTouchOptimized];

			// Set predefined header name
			xulRunnerSettings[SebSettings.KeyBrowserURLHeader] = SebConstants.SEB_REQUEST_HEADER;

			// Set mime type allowed to open
			xulRunnerSettings[SebSettings.KeySettingsMimeType] = SebConstants.SEB_MIME_TYPE;

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
