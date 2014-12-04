using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DefaultBrowserManager.Helper;
using DefaultBrowserManager.Model;

namespace DefaultBrowserManager
{
    public partial class DefaultRule : Form
    {
        public DefaultRule()
        {
            InitializeComponent();

            List<Browser> n = BrowserHelper.FindBrowsers();
            this.comboBox1.Items.AddRange(n.ToArray());
            this.comboBox2.Items.AddRange(n.ToArray());
            this.comboBox3.Items.AddRange(n.ToArray());
            this.comboBox4.Items.AddRange(n.ToArray());

            this.comboBox1.SelectedItem = Config.Default.Keys.Contains("ftp") ? (object)Config.Default["ftp"] ?? (object)"None" : (object)"None";
            this.comboBox2.SelectedItem = Config.Default.Keys.Contains("http") ? (object)Config.Default["http"] ?? (object)"None" : (object)"None";
            this.comboBox3.SelectedItem = Config.Default.Keys.Contains("https") ? (object)Config.Default["https"] ?? (object)"None" : (object)"None";
            this.comboBox4.SelectedItem = Config.Default.Keys.Contains("mailto") ? (object)Config.Default["mailto"] ?? (object)"None" : (object)"None";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.ChangeDefaultRule("ftp", (this.comboBox1.SelectedItem is string) ? null : this.comboBox1.SelectedItem as Browser);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.ChangeDefaultRule("http", (this.comboBox2.SelectedItem is string) ? null : this.comboBox1.SelectedItem as Browser);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.ChangeDefaultRule("https", (this.comboBox3.SelectedItem is string) ? null : this.comboBox1.SelectedItem as Browser);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.ChangeDefaultRule("mailto", (this.comboBox4.SelectedItem is string) ? null : this.comboBox1.SelectedItem as Browser);
        }
    }
}
