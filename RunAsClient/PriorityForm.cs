using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunAsClient
{
    public partial class PriorityForm : Form
    {
        private ClientForm topForm;
        private string procId;
        public PriorityForm(ClientForm _topForm, string _procId)
        {
            InitializeComponent();

            topForm = _topForm;
            procId = _procId;
            priority.Text = "Idle";
        }

        private void butSet_Click(object sender, EventArgs e)
        {
            topForm.SendMessage("#priority#" + procId + ";" + priority.Text, "Set priority");
            topForm.SendMessage("#proclist#", "Get processes list");
            Close();
        }
    }
}
