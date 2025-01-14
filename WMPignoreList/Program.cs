using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMPignoreList
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            NotifyIcon trayIcon = new NotifyIcon
            {
                Icon = new Icon("wmplist.ico"),
                Visible = true,
                Text = "WMP Ignore List Manager"
            };
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Start App", null, (s, e) => OpenManager()).ToolTipText = "Launches the WMPignore foreground GUI application";
            menu.Items.Add("Exit", null, (s, e) => Application.Exit()).ToolTipText = "Closes the WMPignore application totally";
            trayIcon.ContextMenuStrip = menu;
            trayIcon.Text = "WMP Ignore List";

            // Start background service
            IgnoreListService ignoreListService = new IgnoreListService();
            ignoreListService.StartMonitoring();

            // Run the application
            Application.Run();

            // Clean up resources when the application exits
            trayIcon.Dispose();
        }
        private static void OpenManager()
        {
            // Open the GUI to manage the ignore list
            DialogResult res = new IgnoreGUI().ShowDialog();
            if (res.Equals(true))
            {
                Console.WriteLine("Window is running?");
            }
        }
    }
}
