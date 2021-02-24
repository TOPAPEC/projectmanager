
namespace WindowsFormsApp4
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBoxUsers = new System.Windows.Forms.GroupBox();
            this.richTextBoxUsers = new System.Windows.Forms.RichTextBox();
            this.groupBoxProjects = new System.Windows.Forms.GroupBox();
            this.richTextboxProjects = new System.Windows.Forms.RichTextBox();
            this.groupBoxTasks = new System.Windows.Forms.GroupBox();
            this.richTextBoxTasks = new System.Windows.Forms.RichTextBox();
            this.groupBoxUsers.SuspendLayout();
            this.groupBoxProjects.SuspendLayout();
            this.groupBoxTasks.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(899, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyDown);
            // 
            // groupBoxUsers
            // 
            this.groupBoxUsers.Controls.Add(this.richTextBoxUsers);
            this.groupBoxUsers.Location = new System.Drawing.Point(0, 29);
            this.groupBoxUsers.Name = "groupBoxUsers";
            this.groupBoxUsers.Size = new System.Drawing.Size(300, 532);
            this.groupBoxUsers.TabIndex = 1;
            this.groupBoxUsers.TabStop = false;
            this.groupBoxUsers.Text = "Users";
            // 
            // richTextBoxUsers
            // 
            this.richTextBoxUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxUsers.Location = new System.Drawing.Point(3, 19);
            this.richTextBoxUsers.Name = "richTextBoxUsers";
            this.richTextBoxUsers.ReadOnly = true;
            this.richTextBoxUsers.Size = new System.Drawing.Size(294, 510);
            this.richTextBoxUsers.TabIndex = 0;
            this.richTextBoxUsers.Text = "";
            // 
            // groupBoxProjects
            // 
            this.groupBoxProjects.Controls.Add(this.richTextboxProjects);
            this.groupBoxProjects.Location = new System.Drawing.Point(300, 29);
            this.groupBoxProjects.Name = "groupBoxProjects";
            this.groupBoxProjects.Size = new System.Drawing.Size(299, 532);
            this.groupBoxProjects.TabIndex = 2;
            this.groupBoxProjects.TabStop = false;
            this.groupBoxProjects.Text = "Projects";
            // 
            // richTextboxProjects
            // 
            this.richTextboxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextboxProjects.Location = new System.Drawing.Point(3, 19);
            this.richTextboxProjects.Name = "richTextboxProjects";
            this.richTextboxProjects.ReadOnly = true;
            this.richTextboxProjects.Size = new System.Drawing.Size(293, 510);
            this.richTextboxProjects.TabIndex = 0;
            this.richTextboxProjects.Text = "";
            // 
            // groupBoxTasks
            // 
            this.groupBoxTasks.Controls.Add(this.richTextBoxTasks);
            this.groupBoxTasks.Location = new System.Drawing.Point(600, 29);
            this.groupBoxTasks.Name = "groupBoxTasks";
            this.groupBoxTasks.Size = new System.Drawing.Size(300, 532);
            this.groupBoxTasks.TabIndex = 3;
            this.groupBoxTasks.TabStop = false;
            this.groupBoxTasks.Text = "Tasks";
            // 
            // richTextBoxTasks
            // 
            this.richTextBoxTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxTasks.Location = new System.Drawing.Point(3, 19);
            this.richTextBoxTasks.Name = "richTextBoxTasks";
            this.richTextBoxTasks.ReadOnly = true;
            this.richTextBoxTasks.Size = new System.Drawing.Size(294, 510);
            this.richTextBoxTasks.TabIndex = 0;
            this.richTextBoxTasks.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 561);
            this.Controls.Add(this.groupBoxTasks);
            this.Controls.Add(this.groupBoxProjects);
            this.Controls.Add(this.groupBoxUsers);
            this.Controls.Add(this.textBox1);
            this.MaximumSize = new System.Drawing.Size(915, 600);
            this.MinimumSize = new System.Drawing.Size(915, 600);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxUsers.ResumeLayout(false);
            this.groupBoxProjects.ResumeLayout(false);
            this.groupBoxTasks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxUsers;
        private System.Windows.Forms.GroupBox groupBoxProjects;
        private System.Windows.Forms.GroupBox groupBoxTasks;
        private System.Windows.Forms.RichTextBox richTextBoxUsers;
        private System.Windows.Forms.RichTextBox richTextboxProjects;
        private System.Windows.Forms.RichTextBox richTextBoxTasks;
    }
}

