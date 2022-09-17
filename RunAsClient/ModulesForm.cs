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
    public partial class ModulesForm : Form
    {
        public ModulesForm(string sModules, bool showTree = false)
        {
            InitializeComponent();

            list.Visible = !showTree;
            tree.Visible = showTree;

            if (showTree)
            {
                string[] modules = sModules.Split('\0')[0].Split("\r\n");
                foreach (string module in modules)
                {
                    string[] details = module.Split(';');
                    if (!string.IsNullOrWhiteSpace(details[0]))
                    {
                        TreeNode parent = tree.Nodes.Add(details[0]);
                        for (int i = 1; i < details.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(details[i]))
                                parent.Nodes.Add(details[i]);
                        }
                    }
                }
            }
            else
            {
                string[] modules = sModules.Split('\0')[0].Split("\r\n");
                foreach (string module in modules)
                    if (!string.IsNullOrWhiteSpace(module))
                        list.Items.Add(module);
            }
        }
    }
}
