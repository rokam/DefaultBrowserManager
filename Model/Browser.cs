using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefaultBrowserManager.Model
{
    /// <summary>
    /// Browser class
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Browser's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Browser's command line
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Browser's icon
        /// </summary>
        public Icon Icon { get; private set; }

        /// <summary>
        /// Browser's progid
        /// </summary>
        public string ProgID { get; private set; }

        /// <summary>
        /// Browser's registered protocols
        /// </summary>
        public List<string> Protocols 
        {
            get
            {
                return this.m_Protocols;
            }
        }
        private List<string> m_Protocols = new List<string>();

        /// <summary>
        /// Browser constructor
        /// </summary>
        /// <param name="name">Browser's name</param>
        /// <param name="command">Browser's command line</param>
        /// <param name="progID">Browser's progid</param>
        public Browser(string name, string command, string progID)
        {
            this.Name = name;
            this.Command = command;
            this.Icon = null;
            this.ProgID = progID;
        }

        /// <summary>
        /// Browser constructor
        /// </summary>
        /// <param name="name">Browser's name</param>
        /// <param name="command">Browser's command line</param>
        /// <param name="icon">Browser's icon</param>
        /// <param name="progID">Browser's progid</param>
        public Browser(string name, string command, Icon icon, string progID)
        {
            this.Name = name;
            this.Command = command;
            this.Icon = icon;
            this.ProgID = progID;
        }

        /// <summary>
        /// Open URL into browser
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Browser's Process</returns>
        public Process OpenURL(string url)
        {
            ProcessStartInfo info = new ProcessStartInfo(this.Command, url);
            return Process.Start(info);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
