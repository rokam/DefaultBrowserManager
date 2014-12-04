using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DefaultBrowserManager.Model;
using Microsoft.Win32;

namespace DefaultBrowserManager.Helper
{
    /// <summary>
    /// Static Helper class to detect and load browsers into memory.
    /// </summary>
    public class BrowserHelper
    {
        private static List<Browser> Browsers = null;

        /// <summary>
        /// Discover all installed browsers
        /// </summary>
        /// <returns>List of browsers</returns>
        public static List<Browser> FindBrowsers()
        {
            if (Browsers == null)
            {
                List<Browser> browsers = new List<Browser>();
                try
                {
                    RegistryKey masterkey = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Clients").OpenSubKey("StartMenuInternet");

                    foreach (string k in masterkey.GetSubKeyNames())
                    {
                        RegistryKey key = masterkey.OpenSubKey(k);
                        string name = key.GetValue(null).ToString();
                        string progid = k;
                        Icon icon = null;
                        try
                        {
                            string pathIcon = key.OpenSubKey("DefaultIcon").GetValue(null).ToString();
                            pathIcon = pathIcon.Substring(0, pathIcon.LastIndexOf(','));
                            icon = Icon.ExtractAssociatedIcon(pathIcon);
                        }
                        catch { }
                        List<string> protocols = new List<string>();
                        string command = key.OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue(null).ToString();
                        try
                        {
                            RegistryKey tmp = key.OpenSubKey("Capabilities").OpenSubKey("URLAssociations");
                            foreach (string value in tmp.GetValueNames())
                            {
                                progid = tmp.GetValue(value).ToString();
                                protocols.Add(value);
                            }
                        }
                        catch { }
                        if (progid != "DEFAULTBM")
                        {
                            Browser n = new Browser(name, command, icon, progid);
                            n.Protocols.AddRange(protocols);
                            browsers.Add(n);
                        }
                    }
                }
                catch { }
                Browsers = browsers.OrderBy(x=>x.Name).ToList();
            }
            return Browsers;
        }

        /// <summary>
        /// Discover the default browsers.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Browser> FindDefaultBrowsers()
        {
            Dictionary<string, Browser> browsers = new Dictionary<string, Browser>();
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations");
            foreach (string k in key.GetSubKeyNames())
            {
                try
                {
                    browsers.Add(k, Browsers.Find(x => x.ProgID == key.OpenSubKey(k).OpenSubKey("UserChoice").GetValue("Progid").ToString()));
                }
                catch { }
            }
            return browsers;
        }
    }
}
