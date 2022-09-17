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
    public partial class PassForm : Form
    {
        private ClientForm topForm;

        public PassForm(ClientForm _topForm)
        {
            InitializeComponent();
            topForm = _topForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            topForm.password = new();
            foreach (char c in pass.Text)
                topForm.password.AppendChar(c);
            Close();
        }

        private void PassForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                topForm.password = new();
                foreach (char c in pass.Text)
                    topForm.password.AppendChar(c);
                Close();
            }
        }

        private void pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                topForm.password = new();
                foreach (char c in pass.Text)
                    topForm.password.AppendChar(c);
                Close();
            }
        }
    }
}
