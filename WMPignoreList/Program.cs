using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
                Icon = SystemIcons.WinLogo, // Use a default system icon
                Visible = true,
                Text = "WMP Ignore List Manager"
            };
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Start App", null, (s, e) => OpenManager());
            menu.Items.Add("Exit", null, (s, e) => Application.Exit());
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
            DialogResult res = new Form1().ShowDialog();
            if (res.Equals(true))
            {
                Console.WriteLine("Window is running?");
            }
        }
    }
}
