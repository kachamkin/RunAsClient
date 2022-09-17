namespace RunAsClient
{
    partial class PriorityForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PriorityForm));
            this.priority = new System.Windows.Forms.ComboBox();
            this.butSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // priority
            // 
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.FormattingEnabled = true;
            this.priority.Items.AddRange(new object[] {
            "Idle",
            "Below normal",
            "Normal",
            "Above normal",
            "High",
            "Realtime"});
            this.priority.Location = new System.Drawing.Point(3, 5);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(231, 23);
            this.priority.TabIndex = 0;
            // 
            // butSet
            // 
            this.butSet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butSet.Location = new System.Drawing.Point(3, 32);
            this.butSet.Name = "butSet";
            this.butSet.Size = new System.Drawing.Size(231, 23);
            this.butSet.TabIndex = 1;
            this.butSet.Text = "Set";
            this.butSet.UseVisualStyleBackColor = true;
            this.butSet.Click += new System.EventHandler(this.butSet_Click);
            // 
            // PriorityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 59);
            this.Controls.Add(this.butSet);
            this.Controls.Add(this.priority);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PriorityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set priority";
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox priority;
        private Button butSet;
    }
}