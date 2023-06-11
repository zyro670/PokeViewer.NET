namespace PokeViewer.NET.SubForms
{
    partial class RaidCodeEntry
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
            textBox1 = new TextBox();
            EnterButton = new Button();
            AutoPaste = new Button();
            panel1 = new Panel();
            button1 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(18, 9);
            textBox1.MaxLength = 6;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 75);
            textBox1.TabIndex = 0;
            textBox1.DoubleClick += textBox1_DoubleClicked;
            // 
            // EnterButton
            // 
            EnterButton.Location = new Point(18, 87);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new Size(93, 23);
            EnterButton.TabIndex = 1;
            EnterButton.Text = "Enter";
            EnterButton.UseVisualStyleBackColor = true;
            EnterButton.Click += button1_Click;
            // 
            // AutoPaste
            // 
            AutoPaste.Location = new Point(125, 87);
            AutoPaste.Name = "AutoPaste";
            AutoPaste.Size = new Size(93, 23);
            AutoPaste.TabIndex = 3;
            AutoPaste.Text = "Auto Paste";
            AutoPaste.UseVisualStyleBackColor = true;
            AutoPaste.Click += button3_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(EnterButton);
            panel1.Controls.Add(AutoPaste);
            panel1.Location = new Point(-6, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(233, 148);
            panel1.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(80, 116);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Clear All";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // RaidCodeEntry
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Magenta;
            ClientSize = new Size(225, 143);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "RaidCodeEntry";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RaidCodeEntry";
            TransparencyKey = Color.Magenta;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox1;
        private Button EnterButton;
        private Button AutoPaste;
        private Panel panel1;
        private Button button1;
    }
}