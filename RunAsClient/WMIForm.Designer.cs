namespace RunAsClient
{
    partial class WMIForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WMIForm));
            this.procGrid = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.callMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.className = new System.Windows.Forms.ComboBox();
            this.search = new System.Windows.Forms.TextBox();
            this.method = new System.Windows.Forms.ComboBox();
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
            this.procGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
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
            this.procGrid.ContextMenuStrip = this.contextMenuStrip1;
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
            this.procGrid.Location = new System.Drawing.Point(1, 58);
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
            this.procGrid.RowTemplate.Height = 25;
            this.procGrid.RowTemplate.ReadOnly = true;
            this.procGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.procGrid.Size = new System.Drawing.Size(799, 394);
            this.procGrid.TabIndex = 1;
            this.procGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.procGrid_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.callMethodToolStripMenuItem,
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 70);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // callMethodToolStripMenuItem
            // 
            this.callMethodToolStripMenuItem.Name = "callMethodToolStripMenuItem";
            this.callMethodToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.callMethodToolStripMenuItem.Text = "Call method";
            this.callMethodToolStripMenuItem.Click += new System.EventHandler(this.callMethodToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // className
            // 
            this.className.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.className.Location = new System.Drawing.Point(4, 2);
            this.className.Name = "className";
            this.className.Size = new System.Drawing.Size(620, 23);
            this.className.TabIndex = 2;
            this.className.SelectedValueChanged += new System.EventHandler(this.className_SelectedValueChanged);
            this.className.KeyDown += new System.Windows.Forms.KeyEventHandler(this.className_KeyDown);
            this.className.Leave += new System.EventHandler(this.className_Leave);
            // 
            // search
            // 
            this.search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.search.Location = new System.Drawing.Point(630, 2);
            this.search.Name = "search";
            this.search.PlaceholderText = "Search";
            this.search.Size = new System.Drawing.Size(170, 23);
            this.search.TabIndex = 5;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // method
            // 
            this.method.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.method.FormattingEnabled = true;
            this.method.Location = new System.Drawing.Point(4, 31);
            this.method.Name = "method";
            this.method.Size = new System.Drawing.Size(796, 23);
            this.method.TabIndex = 6;
            // 
            // WMIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.method);
            this.Controls.Add(this.search);
            this.Controls.Add(this.className);
            this.Controls.Add(this.procGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WMIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "WMI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WMIForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.procGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView procGrid;
        private ComboBox className;
        private TextBox search;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ComboBox method;
        private ToolStripMenuItem callMethodToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
    }
}