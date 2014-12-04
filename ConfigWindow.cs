using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DefaultBrowserManager.Model;
using DefaultBrowserManager.Helper;

namespace DefaultBrowserManager
{
    public partial class ConfigWindow : Form
    {
        CheckBoxState state = CheckBoxState.UncheckedNormal;

        public ConfigWindow()
        {
            InitializeComponent();
            List<Browser> navegadores = BrowserHelper.FindBrowsers();
            this.comboBox1.Items.AddRange(navegadores.ToArray());
            this.comboBox1.SelectedItem = navegadores.First();
            this.comboBox3.Items.AddRange(Process.GetProcesses().OrderBy(x => x.ProcessName).Select(x => x.ProcessName).Distinct().ToArray());
            this.comboBox3.SelectedIndex = 0;
            NavigationRule[] regras = Config.ListRules();
            foreach (NavigationRule r in regras)
            {
                AddItemToList(r);
            }
        }

        private void AddItemToList(NavigationRule r)
        {
            ListViewItem i = new ListViewItem();
            i.Tag = r;
            i.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = r.Process });
            i.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = r.Protocol });
            i.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = r.Browser.ToString() });
            listView1.Items.Add(i);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Icon icone = ((Browser)this.comboBox1.SelectedItem).Icon;
            this.pictureBox1.Image = icone != null ? icone.ToBitmap() : null;
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.LeftAndRightPadding | TextFormatFlags.VerticalCenter;

            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                Point p = new Point(e.Bounds.Width / 2 - 8, e.Bounds.Height / 2 - 8);
                CheckBoxRenderer.DrawCheckBox(e.Graphics, p, state);
                e.DrawText(flags);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0 && state != CheckBoxState.UncheckedDisabled)
            {
                bool c = state != CheckBoxState.CheckedNormal;
                foreach (ListViewItem i in listView1.Items)
                    i.Checked = c;
            }
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SetHeaderCheckState();
        }

        private void SetHeaderCheckState()
        {
            bool c = true;
            bool u = false;
            foreach (ListViewItem i in listView1.Items)
            {
                u = u || i.Checked;
                c = c && i.Checked;
            }
            state = listView1.Items.Count == 0 ? CheckBoxState.UncheckedDisabled : u && !c ? CheckBoxState.MixedNormal : c ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
            listView1.Invalidate(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListViewItem[] selected = this.listView1.CheckedItems.Cast<ListViewItem>().ToArray();
            foreach (ListViewItem i in selected)
            {
                this.listView1.Items.Remove(i);
                Config.RemoveRule(i.Tag as NavigationRule);
            }
            SetHeaderCheckState();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationRule r = new NavigationRule();
            r.Browser = this.comboBox1.SelectedItem as Browser;
            r.Process = this.comboBox3.SelectedItem as string;
            r.Protocol = this.comboBox2.SelectedItem as string;

            if (!r.IsValid())
            {
                MessageBox.Show("Please select all fields.", "Error adding rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Config.AddRule(r))
            {
                AddItemToList(r);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new DefaultRule().ShowDialog();
        }
    }
}