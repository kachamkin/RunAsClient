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
    public partial class ListViewEx : ListView
    {
        public ListViewEx()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x7b)
                if (m.WParam != this.Handle)
                    return;
            base.WndProc(ref m);
        }
    }
}
