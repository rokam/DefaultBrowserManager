using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DefaultBrowserManager.Model
{
    public class NavigationRule
    {
        public string Process { get; set; }
        public string Protocol { get; set; }
        public Browser Browser { get; set; }

        public override string ToString()
        {
            return this.Process;
        }

        public bool IsValid()
        {
            return Browser != null && Process != null && !String.Empty.Equals(Process) && Protocol != null && !String.Empty.Equals(Protocol);
        }
    }
}
