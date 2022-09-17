namespace RunAsClient
{
    partial class ClientForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.host = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.process = new System.Windows.Forms.TextBox();
            this.args = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uploadArgs = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.file = new System.Windows.Forms.TextBox();
            this.butUpload = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.message = new System.Windows.Forms.TextBox();
            this.processToKill = new System.Windows.Forms.TextBox();
            this.user = new System.Windows.Forms.TextBox();
            this.action = new System.Windows.Forms.ComboBox();
            this.butAction = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // host
            // 
            this.host.Location = new System.Drawing.Point(11, 10);
            this.host.Name = "host";
            this.host.PlaceholderText = "Host";
            this.host.Size = new System.Drawing.Size(368, 23);
            this.host.TabIndex = 6;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(11, 39);
            this.port.Name = "port";
            this.port.PlaceholderText = "Port";
            this.port.Size = new System.Drawing.Size(368, 23);
            this.port.TabIndex = 7;
            this.port.TextChanged += new System.EventHandler(this.port_TextChanged);
            // 
            // process
            // 
            this.process.Location = new System.Drawing.Point(19, 38);
            this.process.Name = "process";
            this.process.PlaceholderText = "Process";
            this.process.Size = new System.Drawing.Size(327, 23);
            this.process.TabIndex = 3;
            // 
            // args
            // 
            this.args.Location = new System.Drawing.Point(18, 67);
            this.args.Name = "args";
            this.args.PlaceholderText = "Arguments";
            this.args.Size = new System.Drawing.Size(328, 23);
            this.args.TabIndex = 4;
            // 
            // send
            // 
            this.send.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.send.Location = new System.Drawing.Point(18, 121);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(328, 23);
            this.send.TabIndex = 0;
            this.send.Text = "Execute";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(5, 69);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(374, 176);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.process);
            this.tabPage1.Controls.Add(this.send);
            this.tabPage1.Controls.Add(this.args);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(366, 148);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Start process";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uploadArgs);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.file);
            this.tabPage2.Controls.Add(this.butUpload);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(366, 148);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Upload and execute";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uploadArgs
            // 
            this.uploadArgs.Location = new System.Drawing.Point(19, 67);
            this.uploadArgs.Name = "uploadArgs";
            this.uploadArgs.PlaceholderText = "Arguments";
            this.uploadArgs.Size = new System.Drawing.Size(328, 23);
            this.uploadArgs.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // file
            // 
            this.file.Location = new System.Drawing.Point(20, 38);
            this.file.Name = "file";
            this.file.PlaceholderText = "File";
            this.file.Size = new System.Drawing.Size(299, 23);
            this.file.TabIndex = 4;
            // 
            // butUpload
            // 
            this.butUpload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butUpload.Location = new System.Drawing.Point(18, 121);
            this.butUpload.Name = "butUpload";
            this.butUpload.Size = new System.Drawing.Size(328, 23);
            this.butUpload.TabIndex = 1;
            this.butUpload.Text = "Execute";
            this.butUpload.UseVisualStyleBackColor = true;
            this.butUpload.Click += new System.EventHandler(this.butUpload_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.message);
            this.tabPage3.Controls.Add(this.processToKill);
            this.tabPage3.Controls.Add(this.user);
            this.tabPage3.Controls.Add(this.action);
            this.tabPage3.Controls.Add(this.butAction);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(366, 148);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Action";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // message
            // 
            this.message.Location = new System.Drawing.Point(17, 67);
            this.message.Multiline = true;
            this.message.Name = "message";
            this.message.PlaceholderText = "Message";
            this.message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.message.Size = new System.Drawing.Size(328, 23);
            this.message.TabIndex = 6;
            this.message.Visible = false;
            // 
            // processToKill
            // 
            this.processToKill.Location = new System.Drawing.Point(17, 38);
            this.processToKill.Name = "processToKill";
            this.processToKill.PlaceholderText = "Process";
            this.processToKill.Size = new System.Drawing.Size(328, 23);
            this.processToKill.TabIndex = 5;
            this.processToKill.Visible = false;
            // 
            // user
            // 
            this.user.Location = new System.Drawing.Point(18, 38);
            this.user.Name = "user";
            this.user.PlaceholderText = "User";
            this.user.Size = new System.Drawing.Size(328, 23);
            this.user.TabIndex = 4;
            this.user.Visible = false;
            // 
            // action
            // 
            this.action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.action.FormattingEnabled = true;
            this.action.Items.AddRange(new object[] {
            "RDP",
            "Get processes list",
            "Get memory usage",
            "Get system info",
            "WMI",
            "View file system",
            "Reboot",
            "Shutdown",
            "Logoff all",
            "Logoff user",
            "Kill process",
            "Send message to all",
            "Send message to user"});
            this.action.Location = new System.Drawing.Point(18, 10);
            this.action.Name = "action";
            this.action.Size = new System.Drawing.Size(328, 23);
            this.action.TabIndex = 3;
            this.action.TextChanged += new System.EventHandler(this.action_TextChanged);
            // 
            // butAction
            // 
            this.butAction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butAction.Location = new System.Drawing.Point(18, 121);
            this.butAction.Name = "butAction";
            this.butAction.Size = new System.Drawing.Size(328, 23);
            this.butAction.TabIndex = 2;
            this.butAction.Text = "Execute";
            this.butAction.UseVisualStyleBackColor = true;
            this.butAction.Click += new System.EventHandler(this.butAction_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(392, 244);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.port);
            this.Controls.Add(this.host);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ClientForm";
            this.Text = "Run As Client";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox host;
        private TextBox port;
        private TextBox process;
        private TextBox args;
        private Button send;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button butUpload;
        private TextBox file;
        private Button button1;
        private TabPage tabPage3;
        private Button butAction;
        private ComboBox action;
        private TextBox user;
        private TextBox processToKill;
        private TextBox uploadArgs;
        private TextBox message;
    }
}