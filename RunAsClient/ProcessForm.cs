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
    struct CurRowData
    {
        public string Name; public int Id; public string User; public int Num;
    }

    public partial class ProcessForm : Form
    {
        private CurRowData curRowData;
        private readonly DataTable processList = new();
        private readonly ClientForm topForm; 
        public ProcessForm(string sProcesses, ClientForm _topForm)
        {
            
            InitializeComponent();

            processList.Columns.Add("Number", typeof(int));
            processList.Columns.Add("Image", typeof(Bitmap));
            processList.Columns.Add("Name", typeof(string));
            processList.Columns.Add("Id", typeof(int));
            processList.Columns.Add("User", typeof(string));
            processList.Columns.Add("Memory", typeof(long));
            processList.Columns.Add("Started", typeof(string));
            processList.Columns.Add("CPU", typeof(double));
            processList.Columns.Add("Priority", typeof(string));
            processList.Columns.Add("Threads", typeof(int));
            processList.Columns.Add("Modules", typeof(int));
            processList.Columns.Add("Bitness", typeof(string));
            processList.Columns.Add("Critical", typeof(string));
            processList.Columns.Add("Product", typeof(string));
            processList.Columns.Add("Company", typeof(string));
            processList.Columns.Add("Path", typeof(string));

            topForm = _topForm;
            UpdateData(sProcesses);

        }

        public void UpdateData(string sProcesses)
        {

            procGrid.DataSource = null;

            processList.Clear();

            string[] rows = sProcesses.Split('\0')[0].Split("#\r\n");
            foreach (string row in rows)
            {

                if (string.IsNullOrEmpty(row))
                    continue;

                string[] data = row.Split(";", StringSplitOptions.TrimEntries);

                DataRow dataRow = processList.Rows.Add();

                dataRow["Number"] = processList.Rows.Count;
                dataRow["Name"] = data[0];
                if (int.TryParse(data[1], out int id))
                    dataRow["Id"] = id;
                dataRow["User"] = data[2];
                if (long.TryParse(data[3], out long mem))
                    dataRow["Memory"] = mem / 1024;
                dataRow["Started"] = data[4];
                if (double.TryParse(data[5].Replace(".", ","), out double cpuUsage))
                    dataRow["CPU"] = cpuUsage;
                dataRow["Priority"] = data[6];
                if (int.TryParse(data[7], out int threads))
                    dataRow["Threads"] = threads;
                if (int.TryParse(data[8], out int modules))
                    dataRow["Modules"] = modules;
                dataRow["Bitness"] = data[9];
                dataRow["Critical"] = data[10];
                dataRow["Product"] = data[11];
                dataRow["Company"] = data[12];
                if (!string.IsNullOrWhiteSpace(data[13]))
                {
                    using MemoryStream ms = new(Convert.FromBase64String(data[13]));
                    Bitmap bmp = new(ms, false);
                    bmp.MakeTransparent(Color.Black);
                    dataRow["Image"] = bmp;
                }
                else
                    dataRow["Image"] = new Bitmap(1, 1);
                dataRow["Path"] = data[14];
            }

            procGrid.DataSource = processList;

            DataGridViewColumn memory = procGrid.Columns["Memory"];
            memory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            memory.DefaultCellStyle.Format = "N0";
            memory.HeaderText = "Memory, K";

            DataGridViewColumn cpu = procGrid.Columns["CPU"];
            cpu.DefaultCellStyle.Format = "N6";
            cpu.HeaderText = "CPU, %";

            procGrid.Columns["Started"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            procGrid.Columns["Critical"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            procGrid.Columns["Id"].DefaultCellStyle.Format = "N0";
            procGrid.Columns["Threads"].DefaultCellStyle.Format = "N0";

            DataGridViewColumn image = procGrid.Columns["Image"];
            image.HeaderText = "";
            image.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            image.Width = 40;

            DataGridViewColumn number = procGrid.Columns["Number"];
            number.HeaderText = "#";
            number.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            number.Width = 40;


            IEnumerable<DataGridViewRow> coll = procGrid.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Id"].Value.Equals(curRowData.Id) && r.Cells["Name"].Value.Equals(curRowData.Name) && r.Cells["User"].Value.Equals(curRowData.User));
            if (coll?.Count() > 0)
            {
                coll.First().Selected = true;
                try
                {
                    procGrid.FirstDisplayedScrollingRowIndex = procGrid.SelectedRows[0].Index - curRowData.Num;
                }
                catch
                {
                };
            }
        
        }

        private async void killProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            object? idVal = procGrid.Rows[rowInd].Cells["Id"].Value;
            if (idVal == null)
                return;
            string curId = ((int)idVal).ToString().Replace(" ", "");
            if (string.IsNullOrEmpty(curId))
                return;

            await Task.Run(() => topForm.SendMessage("#kill#" + procGrid.Rows[procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells["Id"].Value, "Kill process"));
            topForm.SendMessage("#proclist#", "Get processes list");
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            topForm.SendMessage("#proclist#", "Get processes list");
        }

        private async void logoffUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            object? userVal = procGrid.Rows[rowInd].Cells["User"].Value;
            if (userVal == null)
                return;
            string curUser = (string)userVal;
            if (string.IsNullOrEmpty(curUser))
                return;

            await Task.Run(() => topForm.SendMessage("#logoff#" + curUser, "Logoff user"));
            topForm.SendMessage("#proclist#", "Get processes list");
        }

        private void ProcessForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            topForm.procForm = null;
        }

        private void procGrid_MouseDown(object sender, MouseEventArgs e)
        {
            int ind = procGrid.HitTest(e.X, e.Y).RowIndex;
            if (ind == -1)
                return;

            if (e.Button == MouseButtons.Right)
            {
                procGrid.ClearSelection();
                procGrid.Rows[ind].Selected = true;  
            }

            if (procGrid.Rows[ind].Cells["Name"].Value == DBNull.Value || procGrid.Rows[ind].Cells["Id"].Value == DBNull.Value)
                return;
            
            curRowData = new() { Name = (string)procGrid.Rows[ind].Cells["Name"].Value, User = (string)procGrid.Rows[ind].Cells["User"].Value, Id = (int)procGrid.Rows[ind].Cells["Id"].Value, Num = ind - procGrid.FirstDisplayedScrollingRowIndex };

        }

        private async void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            string? curUser = (string?)procGrid.Rows[rowInd].Cells["User"].Value;
            if (string.IsNullOrEmpty(curUser))
                return;

            string message = Interaction.InputBox("Input message text here", "Send message to " + curUser);
            if (!string.IsNullOrWhiteSpace(message))
                await Task.Run(() => topForm.SendMessage("#message#" + curUser + "#" + message, "Send message to user"));
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            ((DataTable)procGrid.DataSource).DefaultView.RowFilter = string.IsNullOrWhiteSpace(search.Text) ? "" : "Name LIKE '*" + search.Text + "*' OR User LIKE '*" + search.Text + "*' OR Started LIKE '*" + search.Text + "*' OR Priority LIKE '*" + search.Text + "*' OR Bitness LIKE '*" + search.Text + "*' OR Product LIKE '*" + search.Text + "*' OR Company LIKE '*" + search.Text + "*'";
        }

        private void procGrid_KeyDown(object sender, KeyEventArgs e)
        {
            string symbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (symbols.IndexOf((char)e.KeyData) >= 0)
                search.Text += ((char)e.KeyData).ToString().ToLower();
        }

        private void setPriorityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            object? idVal = procGrid.Rows[rowInd].Cells["Id"].Value;
            if (idVal == null)
                return;
            string curId = ((int)idVal).ToString().Replace(" ", "");
            if (string.IsNullOrEmpty(curId))
                return;

            new PriorityForm(topForm, curId).ShowDialog();
        }

        private async void viewModulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            object? idVal = procGrid.Rows[rowInd].Cells["Id"].Value;
            if (idVal == null)
                return;
            string curId = ((int)idVal).ToString().Replace(" ", "");
            if (string.IsNullOrEmpty(curId))
                return;

            await Task.Run(() => topForm.SendMessage("#modules#" + curId, "View modules"));
        }

        private async void viewMemoryDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            object? idVal = procGrid.Rows[rowInd].Cells["Id"].Value;
            if (idVal == null)
                return;
            string curId = ((int)idVal).ToString().Replace(" ", "");
            if (string.IsNullOrEmpty(curId))
                return;

            await Task.Run(() => topForm.SendMessage("#memorydetails#" + curId, "View memory details"));
        }

        private async void viewThreadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowInd = procGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowInd == -1)
                return;
            object? idVal = procGrid.Rows[rowInd].Cells["Id"].Value;
            if (idVal == null)
                return;
            string curId = ((int)idVal).ToString().Replace(" ", "");
            if (string.IsNullOrEmpty(curId))
                return;

            await Task.Run(() => topForm.SendMessage("#threads#" + curId, "View threads"));
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = processList.Copy();
            var l = dt.Rows.OfType<DataRow>().GroupBy(x => x["User"]).ToArray();
            dt.Rows.Clear();
            foreach (var row in l)
                dt.Rows.Add()["User"] = row.Key;

            foreach (DataColumn c in dt.Columns)
                if (c.ColumnName != "User")
                    procGrid.Columns[c.ColumnName].Visible = false;

            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;

            procGrid.DataSource = dt;
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataColumn c in processList.Columns)
                if (c.ColumnName != "User")
                    procGrid.Columns[c.ColumnName].Visible = true;

            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[3].Visible = true;
            contextMenuStrip1.Items[4].Visible = true;
            contextMenuStrip1.Items[5].Visible = true;
            contextMenuStrip1.Items[6].Visible = true;
            contextMenuStrip1.Items[8].Visible = true;

            procGrid.DataSource = processList;
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            procGrid.MultiSelect = true;
            procGrid.SelectAll();
            Clipboard.SetDataObject(procGrid.GetClipboardContent());
            procGrid.MultiSelect = false;
            try
            {
                procGrid.Rows[curRowData.Num].Selected = true;
            }
            catch { };
        }
    }
}
