using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMPignoreList
{
    public partial class Form1 : Form
    {
        private readonly ListBox lstIgnoreList;
        private readonly Button btnAdd, btnRemove, btnClose;
        private readonly IgnoreListService ignoreListService;

        public Form1()
        {
            ignoreListService = new IgnoreListService();
            //this.Padding = 10;
            lstIgnoreList = new ListBox { Dock = DockStyle.Top, Height = 200 };
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

            RefreshIgnoreList();
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
