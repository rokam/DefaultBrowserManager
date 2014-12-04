using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefaultBrowserManager.Model
{
    public class Browser
    {
        public string Name { get; private set; }
        public string Command { get; private set; }
        public Icon Icon { get; private set; }
        public string ProgID { get; private set; }
        public List<string> Protocols 
        {
            get
            {
                return this.m_Protocols;
            }
        }
        private List<string> m_Protocols = new List<string>();

        public Browser(string name, string command, Icon icon, string progID)
        {
            this.Name = name;
            this.Command = command;
            this.Icon = icon;
            this.ProgID = progID;
        }

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
