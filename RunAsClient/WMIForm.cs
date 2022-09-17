using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace RunAsClient
{
    public partial class WMIForm : Form
    {
        public ClientForm topForm;
        private readonly DataTable processList = new();
        private int currIndex = 0;
        private int firstDisplayed = 0;

        public WMIForm(ClientForm _topForm, string _sData)
        {
            InitializeComponent();
            topForm = _topForm;
            UpdateData(_sData);
        }

        public void UpdateData(string sData)
        {
            string currMethod = method.Text;
            
            method.Items.Clear();
            procGrid.DataSource = null;

            processList.Clear();
            processList.Columns.Clear();
            processList.Columns.Add("Row", typeof(int));
            processList.DefaultView.RowFilter = "";

            string[] rows = sData.Split('\0')[0].Split("\r\n");
            if (rows.Length == 0)
                return;

            if (string.IsNullOrWhiteSpace(rows[0]))
                return;

            string[] data;
            if (rows.Length > 1)
            {
                data = rows[0].Split("#", StringSplitOptions.TrimEntries);
                foreach (string header in data)
                    if (!string.IsNullOrWhiteSpace(header))
                        method.Items.Add(header);
            }

            if (!string.IsNullOrWhiteSpace(currMethod))
                if (method.Items.Contains(currMethod))
                    method.Text = currMethod; 

            data = rows[1].Split("#", StringSplitOptions.TrimEntries);
            foreach (string header in data)
                if (!string.IsNullOrWhiteSpace(header))
                    processList.Columns.Add(header);

            if (rows.Length > 1)
            {
                for (int i = 2; i < rows.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(rows[i]))
                        continue;
                    data = rows[i].Split("#", StringSplitOptions.TrimEntries);
                    DataRow dr = processList.Rows.Add();
                    dr[0] = processList.Rows.Count;
                    for (int j = 0; j < processList.Columns.Count - 1; j++)
                        dr[j + 1] = data[j];
                }
            }

            procGrid.DataSource = processList;

            if (!string.IsNullOrWhiteSpace(search.Text))
                search_TextChanged(search, new EventArgs());

            try
            {
                procGrid.FirstDisplayedScrollingRowIndex = firstDisplayed;
                procGrid.Rows[currIndex].Selected = true;
            }
            catch { };

        }

        private void className_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                procGrid.Focus();
        }

        private void className_Leave(object sender, EventArgs e)
        {
           topForm.SendMessage("#devices#" + className.Text);
        }

        private void WMIForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            topForm.wmi = null;
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(search.Text))
            {
                ((DataTable)procGrid.DataSource).DefaultView.RowFilter = "";
                return;
            }

            string filter = "";
            for (int i = 1; i < processList.Columns.Count; i++)
            {
                if (i == 1)
                    filter = processList.Columns[i].ColumnName + " LIKE '*" + search.Text + "*'";
                else
                    filter += " OR " + processList.Columns[i].ColumnName + " LIKE '*" + search.Text + "*'";
            }
            ((DataTable)procGrid.DataSource).DefaultView.RowFilter = filter;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            topForm.SendMessage("#devices#" + className.Text);
        }

        private void callMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(className.Text) || string.IsNullOrWhiteSpace(method.Text))
            {
                MessageBox.Show("WMI class or method not specified!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string filter = Interaction.InputBox("Method \"" + method.Text + "\": input method parameters here", "Method parameters");
            if (!string.IsNullOrWhiteSpace(filter))
            {
                topForm.SendMessage("#execmethod#" + className.Text + ";" + method.Text + "#" + filter);
                topForm.SendMessage("#devices#" + className.Text);
            }
        }

        private void procGrid_MouseDown(object sender, MouseEventArgs e)
        {
            int ind = procGrid.HitTest(e.X, e.Y).RowIndex;
            if (ind == -1)
                return;

            currIndex = ind;
            firstDisplayed = procGrid.FirstDisplayedScrollingRowIndex;

            if (e.Button == MouseButtons.Right)
            {
                procGrid.ClearSelection();
                procGrid.Rows[ind].Selected = true;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(procGrid.GetClipboardContent());
        }
    }
}
