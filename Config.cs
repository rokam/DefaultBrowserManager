using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using DefaultBrowserManager.Helper;
using DefaultBrowserManager.Model;
using DefaultBrowserManager.Util;
using Microsoft.Win32;

namespace DefaultBrowserManager
{
    /// <summary>
    /// Static class with the application configs
    /// </summary>
    public static class Config
    {
        public static Dictionary<string, Browser> Default = new Dictionary<string, Browser>();
        private static List<NavigationRule> Rules = new List<NavigationRule>();

        /// <summary>
        /// Load all configs into memory
        /// </summary>
        public static void Initialize()
        {
            List<Browser> browsers = BrowserHelper.FindBrowsers();
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE");
            key = key.CreateSubKey("DefaultBrowserManager");
            RegistryKey p = key.CreateSubKey("NavegadorPadrao");
            if (p.ValueCount == 0)
            {
                Dictionary<string, Browser> defaultBrowsers = BrowserHelper.FindDefaultBrowsers();
                foreach(string protocol in defaultBrowsers.Keys)
                {
                    if(defaultBrowsers[protocol] != null)
                        p.SetValue(protocol, defaultBrowsers[protocol].ProgID);
                }
            }
            foreach (string k in p.GetValueNames())
            {
                Default.Add(k, browsers.Find(x => x.ProgID == p.GetValue(k).ToString()));
            }
            RegistryKey r = key.OpenSubKey("Regras");
            foreach (string process in r.GetSubKeyNames())
            {
                RegistryKey tmp = r.OpenSubKey(process);
                foreach (string protocol in tmp.GetValueNames())
                {
                    Rules.Add(new NavigationRule { Process = process, Protocol = protocol, Browser = browsers.Find(x => x.ProgID == tmp.GetValue(protocol).ToString()) });
                }
            }
        }

        /// <summary>
        /// Set a default rule, when all request of a protocol should be redirected to a browser if no rule is found.
        /// </summary>
        /// <param name="protocol">Protocol, can be 'ftp', 'http', 'https', 'mailto'</param>
        /// <param name="browser">A Browser object</param>
        /// <returns>If the operation is successfull</returns>
        public static bool ChangeDefaultRule(string protocol, Browser browser)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE");
                key = key.CreateSubKey("DefaultBrowserManager");
                RegistryKey p = key.CreateSubKey("NavegadorPadrao");
                if (browser != null)
                    p.SetValue(protocol, browser.ProgID);
                else
                    p.DeleteValue(protocol);
            }
            catch
            {
                return false;
            }
            Default[protocol] = browser;
            return true;
        }

        /// <summary>
        /// Add a new Rule
        /// </summary>
        /// <param name="rule">Rule itself</param>
        /// <returns>If operation is successfull</returns>
        public static bool AddRule(NavigationRule rule)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE");
                key = key.CreateSubKey("DefaultBrowserManager");
                key = key.CreateSubKey("Regras");
                key = key.CreateSubKey(rule.Process);
                key.SetValue(rule.Protocol, rule.Browser.ProgID);
            }
            catch 
            {
                return false;
            }
            Rules.Add(rule);
            return true;
        }

        /// <summary>
        /// Remove a Rule
        /// </summary>
        /// <param name="rule">Rupe itself</param>
        /// <returns>If operation is successfull</returns>
        public static bool RemoveRule(NavigationRule rule)
        {
            if (Rules.Contains(rule))
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE");
                    key = key.CreateSubKey("DefaultBrowserManager");
                    RegistryKey p = key.CreateSubKey("Regras");
                    key = p.CreateSubKey(rule.Process);
                    key.DeleteValue(rule.Protocol);
                    if (key.ValueCount == 0 && key.SubKeyCount == 0)
                        p.DeleteSubKeyTree(rule.Process);
                }
                catch
                {
                    return false;
                }
                return Rules.Remove(rule);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get a rule list snapshot
        /// </summary>
        /// <returns>Rule array</returns>
        public static NavigationRule[] ListRules()
        {
            return Rules.ToArray();
        }

        /// <summary>
        /// Detect which browser should open given some parameters
        /// </summary>
        /// <param name="process">Process where the link was clicked</param>
        /// <param name="protocol">Protocol of the URL</param>
        /// <returns>Browser that correspond to a rule or the default browser for the protocol</returns>
        public static Browser DetectBrowser(Process process, string protocol)
        {
            NavigationRule r = Rules.Find(x => x.Process == process.ProcessName && x.Protocol == protocol);
            if (r == null)
            {
                return Default.Keys.Contains(protocol) ? Default[protocol] : BrowserHelper.FindBrowsers().First();
            }
            return r.Browser;
        }

        /// <summary>
        /// Set the application as system default browser
        /// </summary>
        /// <param name="protocols">Array of protocols</param>
        /// <returns>If the operation is successfull</returns>
        public static bool SetAsDefaultBrowser(string[] protocols)
        {
            return true;
        }

        /// <summary>
        /// Redirect the request to the corresponding browser
        /// </summary>
        /// <param name="urls">url array</param>
        /// <returns>Process of the browser</returns>
        public static Process Redirect(string[] urls)
        {
            Process p = null;
            foreach (string url in urls)
                p = Redirect(url);
            return p;
        }

        /// <summary>
        /// Redirect the request to the corresponding browser
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>Process of the browser</returns>
        public static Process Redirect(string url)
        {
            Uri uri = new Uri(url);
            return Redirect(uri);
        }

        /// <summary>
        /// Redirect the request to the corresponding browser
        /// </summary>
        /// <param name="uri">Uri object</param>
        /// <returns>Process of the browser</returns>
        public static Process Redirect(Uri uri)
        {
            return DetectBrowser(User32Util.GetForegroundWindowProcess(), uri.Scheme).OpenURL(uri.ToString());
        }
    }
}
