using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DefaultBrowserManager.Model
{
    /// <summary>
    /// Navigation Rule class
    /// </summary>
    public class NavigationRule
    {
        /// <summary>
        /// Process name
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// Protocol
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Browser
        /// </summary>
        public Browser Browser { get; set; }

        public override string ToString()
        {
            return this.Process;
        }

        /// <summary>
        /// Check if all necessary fields have been selected
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Browser != null && Process != null && !String.Empty.Equals(Process) && Protocol != null && !String.Empty.Equals(Protocol);
        }
    }
}
