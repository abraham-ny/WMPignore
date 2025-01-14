using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMPignoreList
{
    public partial class IgnoreGUI : Form
    {
        private readonly ListBox lstIgnoreList;
        private readonly Button btnAdd, btnRemove, btnClose;
        private readonly IgnoreListService ignoreListService;

        public IgnoreGUI()
        { 
            
            
            ignoreListService = new IgnoreListService();
            //this.Padding = 10;
            lstIgnoreList = new ListBox { Dock = DockStyle.Top, Margin = new Padding(10)  };
            btnAdd = new Button { Text = "Add", Dock = DockStyle.Bottom, Width = 100 };
            btnRemove = new Button { Text = "Remove", Dock = DockStyle.Bottom, Width = 100 };
            btnClose = new Button { Text = "Close", Dock = DockStyle.Bottom, Width = 100 };

            btnAdd.Click += (s, e) =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        ignoreListService.AddToIgnoreList(dialog.SelectedPath);
                        RefreshIgnoreList();
                    }
                }
            };

            btnRemove.Click += (s, e) =>
            {
                if (lstIgnoreList.SelectedItem != null)
                {
                    ignoreListService.RemoveFromIgnoreList(lstIgnoreList.SelectedItem.ToString());
                    RefreshIgnoreList();
                }
            };

            btnClose.Click += (s, e) => Close();

            Controls.Add(lstIgnoreList);
            Controls.Add(btnAdd);
            Controls.Add(btnRemove);
            Controls.Add(btnClose);

            MenuStrip mnu = new MenuStrip();
            ToolStripMenuItem file = new ToolStripMenuItem("File");
            ToolStripMenuItem help = new ToolStripMenuItem("Help");
            file.DropDownItems.Add("Open", null, openDirPicker);
            file.DropDownItems.Add("Exit", null, closeWin);
            help.DropDownItems.Add("About", null, showAbout);
            help.DropDownItems.Add("View Source", null, github);
            mnu.Items.Add(file);
            mnu.Items.Add(help);
            mnu.Dock = DockStyle.Top;
            this.MainMenuStrip = mnu;
            Controls.Add(mnu);
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Icon = new Icon("wmplist.ico");

            RefreshIgnoreList();
        }

        private void github(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/abraham-ny/WMPignore",
                    UseShellExecute = true
                });
            }catch(Exception ex)
            {
                MessageBox.Show($"Failed to open browser {ex.Message}");
            }
        }

        private void showAbout(object sender, EventArgs e)
        {
            MessageBox.Show("Windows Media Player ignore list manager. \nThis tool allows you to exclude specific folders from wmp playlist/media library even if they are subfolders of a library folder.\n(c)2025, Abraham Moruri"
                , "WMP-ignore v1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void closeWin(object sender, EventArgs e)
        {
            Close();
        }

        private void openDirPicker(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ignoreListService.AddToIgnoreList(dialog.SelectedPath);
                    RefreshIgnoreList();
                }
            }
        }

        private void IgnoreGUI_Load(object sender, EventArgs e)
        {

        }

        public void LOpenDirPicker()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ignoreListService.AddToIgnoreList(dialog.SelectedPath);
                    RefreshIgnoreList();
                }
            }
        }
        private void RefreshIgnoreList()
        {
            lstIgnoreList.Items.Clear();
            foreach (var folder in ignoreListService.LoadIgnoreList())
            {
                lstIgnoreList.Items.Add(folder);
            }
        }
    }
}
