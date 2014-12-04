using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DefaultBrowserManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Config.Initialize();
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--help":
                    case "-h":
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("DefaultBrowserManager.exe [option] [url]");
                        sb.AppendLine();
                        sb.AppendLine("Options:");
                        sb.AppendLine("-c, --config Opens configuration window");
                        sb.AppendLine("-h, --help This window");
                        sb.AppendLine("-r, --register Register Default Browser Manager as default browser");
                        MessageBox.Show(sb.ToString(), "Help", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        break;
                    case "-r":
                    case "--register":
                        if (Config.SetAsDefaultBrowser(args.Skip(1).ToArray()))
                        {
                            StringBuilder sb1 = new StringBuilder();
                            sb1.AppendLine("Default Browser Manager set as default browser for protocols:").AppendLine();
                            foreach (string s in args.Skip(1))
                            {
                                sb1.AppendLine(s);
                            }
                            MessageBox.Show(sb1.ToString(), "Set as default browser", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to set Default Browser Manager as default browser.", "Set as defailt browser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case "-c":
                    case "--config":
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new ConfigWindow());
                        break;
                    default:
                        Config.Redirect(args.ToArray());
                        break;
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ConfigWindow());
            }
        }
    }
}
