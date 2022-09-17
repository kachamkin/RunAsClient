namespace RunAsClient
{
    partial class ProcessForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessForm));
            this.procGrid = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.killProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoffUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewModulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewThreadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMemoryDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.search = new System.Windows.Forms.TextBox();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.procGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // procGrid
            // 
            this.procGrid.AllowUserToAddRows = false;
            this.procGrid.AllowUserToDeleteRows = false;
            this.procGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.procGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.procGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.procGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.procGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.procGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.procGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.procGrid.EnableHeadersVisualStyles = false;
            this.procGrid.Location = new System.Drawing.Point(0, 31);
            this.procGrid.MultiSelect = false;
            this.procGrid.Name = "procGrid";
            this.procGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.procGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.procGrid.RowHeadersVisible = false;
            this.procGrid.RowTemplate.ContextMenuStrip = this.contextMenuStrip1;
            this.procGrid.RowTemplate.Height = 25;
            this.procGrid.RowTemplate.ReadOnly = true;
            this.procGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.procGrid.Size = new System.Drawing.Size(799, 420);
            this.procGrid.TabIndex = 0;
            this.procGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.procGrid_KeyDown);
            this.procGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.procGrid_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.killProcessToolStripMenuItem,
            this.logoffUserToolStripMenuItem,
            this.sendMessageToolStripMenuItem,
            this.setPriorityToolStripMenuItem,
            this.viewModulesToolStripMenuItem,
            this.viewThreadsToolStripMenuItem,
            this.viewMemoryDetailsToolStripMenuItem,
            this.toolStripSeparator1,
            this.refreshToolStripMenuItem,
            this.usersToolStripMenuItem,
            this.allToolStripMenuItem,
            this.copyAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 274);
            // 
            // killProcessToolStripMenuItem
            // 
            this.killProcessToolStripMenuItem.Name = "killProcessToolStripMenuItem";
            this.killProcessToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.killProcessToolStripMenuItem.Text = "Kill process";
            this.killProcessToolStripMenuItem.Click += new System.EventHandler(this.killProcessToolStripMenuItem_Click);
            // 
            // logoffUserToolStripMenuItem
            // 
            this.logoffUserToolStripMenuItem.Name = "logoffUserToolStripMenuItem";
            this.logoffUserToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.logoffUserToolStripMenuItem.Text = "Logoff user";
            this.logoffUserToolStripMenuItem.Click += new System.EventHandler(this.logoffUserToolStripMenuItem_Click);
            // 
            // sendMessageToolStripMenuItem
            // 
            this.sendMessageToolStripMenuItem.Name = "sendMessageToolStripMenuItem";
            this.sendMessageToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.sendMessageToolStripMenuItem.Text = "Send message";
            this.sendMessageToolStripMenuItem.Click += new System.EventHandler(this.sendMessageToolStripMenuItem_Click);
            // 
            // setPriorityToolStripMenuItem
            // 
            this.setPriorityToolStripMenuItem.Name = "setPriorityToolStripMenuItem";
            this.setPriorityToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.setPriorityToolStripMenuItem.Text = "Set priority";
            this.setPriorityToolStripMenuItem.Click += new System.EventHandler(this.setPriorityToolStripMenuItem_Click);
            // 
            // viewModulesToolStripMenuItem
            // 
            this.viewModulesToolStripMenuItem.Name = "viewModulesToolStripMenuItem";
            this.viewModulesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.viewModulesToolStripMenuItem.Text = "View modules";
            this.viewModulesToolStripMenuItem.Click += new System.EventHandler(this.viewModulesToolStripMenuItem_Click);
            // 
            // viewThreadsToolStripMenuItem
            // 
            this.viewThreadsToolStripMenuItem.Name = "viewThreadsToolStripMenuItem";
            this.viewThreadsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.viewThreadsToolStripMenuItem.Text = "View threads";
            this.viewThreadsToolStripMenuItem.Click += new System.EventHandler(this.viewThreadsToolStripMenuItem_Click);
            // 
            // viewMemoryDetailsToolStripMenuItem
            // 
            this.viewMemoryDetailsToolStripMenuItem.Name = "viewMemoryDetailsToolStripMenuItem";
            this.viewMemoryDetailsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.viewMemoryDetailsToolStripMenuItem.Text = "View memory details";
            this.viewMemoryDetailsToolStripMenuItem.Click += new System.EventHandler(this.viewMemoryDetailsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // search
            // 
            this.search.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.search.Location = new System.Drawing.Point(3, 2);
            this.search.Name = "search";
            this.search.PlaceholderText = "Search";
            this.search.Size = new System.Drawing.Size(796, 23);
            this.search.TabIndex = 4;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copyAllToolStripMenuItem.Text = "Copy all";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.copyAllToolStripMenuItem_Click);
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.search);
            this.Controls.Add(this.procGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Processes";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProcessForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.procGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView procGrid;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem killProcessToolStripMenuItem;
        private ToolStripMenuItem logoffUserToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripMenuItem sendMessageToolStripMenuItem;
        private TextBox search;
        private ToolStripMenuItem setPriorityToolStripMenuItem;
        private ToolStripMenuItem viewModulesToolStripMenuItem;
        private ToolStripMenuItem viewMemoryDetailsToolStripMenuItem;
        private ToolStripMenuItem viewThreadsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem usersToolStripMenuItem;
        private ToolStripMenuItem allToolStripMenuItem;
        private ToolStripMenuItem copyAllToolStripMenuItem;
    }
}