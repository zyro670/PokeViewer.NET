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
            panel3 = new Panel();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            checkBox1 = new CheckBox();
            EnterButton = new Button();
            ClearAll = new Button();
            AutoPaste = new Button();
            button4 = new Button();
            panel4 = new Panel();
            panel5 = new Panel();
            label7 = new Label();
            ScreenToTextPanel = new Panel();
            panel1 = new Panel();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
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
            // panel3
            // 
            panel3.BackColor = Color.LightGray;
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(checkBox1);
            panel3.Controls.Add(EnterButton);
            panel3.Controls.Add(ClearAll);
            panel3.Controls.Add(AutoPaste);
            panel3.Controls.Add(button4);
            panel3.Location = new Point(-1, 108);
            panel3.Name = "panel3";
            panel3.Size = new Size(234, 103);
            panel3.TabIndex = 8;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(197, 3);
            label6.Name = "label6";
            label6.Size = new Size(0, 15);
            label6.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(163, 3);
            label5.Name = "label5";
            label5.Size = new Size(0, 15);
            label5.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(129, 3);
            label4.Name = "label4";
            label4.Size = new Size(0, 15);
            label4.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(93, 3);
            label3.Name = "label3";
            label3.Size = new Size(0, 15);
            label3.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(58, 3);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 3);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 6;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(69, 77);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(88, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Invert Color";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // EnterButton
            // 
            EnterButton.Location = new Point(13, 19);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new Size(93, 23);
            EnterButton.TabIndex = 1;
            EnterButton.Text = "Enter";
            EnterButton.UseVisualStyleBackColor = true;
            EnterButton.Click += button1_Click;
            // 
            // ClearAll
            // 
            ClearAll.Location = new Point(120, 50);
            ClearAll.Name = "ClearAll";
            ClearAll.Size = new Size(93, 23);
            ClearAll.TabIndex = 2;
            ClearAll.Text = "Clear All";
            ClearAll.UseVisualStyleBackColor = true;
            ClearAll.Click += button2_Click;
            // 
            // AutoPaste
            // 
            AutoPaste.Location = new Point(11, 50);
            AutoPaste.Name = "AutoPaste";
            AutoPaste.Size = new Size(93, 23);
            AutoPaste.TabIndex = 3;
            AutoPaste.Text = "Auto Paste";
            AutoPaste.UseVisualStyleBackColor = true;
            AutoPaste.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(120, 19);
            button4.Name = "button4";
            button4.Size = new Size(93, 23);
            button4.TabIndex = 4;
            button4.Text = "Img To Text";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.LightGray;
            panel4.Location = new Point(191, 88);
            panel4.Name = "panel4";
            panel4.Size = new Size(39, 28);
            panel4.TabIndex = 9;
            // 
            // panel5
            // 
            panel5.BackColor = Color.LightGray;
            panel5.Controls.Add(label7);
            panel5.Location = new Point(-10, 87);
            panel5.Name = "panel5";
            panel5.Size = new Size(141, 29);
            panel5.TabIndex = 10;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(45, 4);
            label7.Name = "label7";
            label7.Size = new Size(80, 15);
            label7.TabIndex = 12;
            label7.Text = "Img Finder ->";
            // 
            // ScreenToTextPanel
            // 
            ScreenToTextPanel.Location = new Point(128, 90);
            ScreenToTextPanel.Name = "ScreenToTextPanel";
            ScreenToTextPanel.Size = new Size(65, 18);
            ScreenToTextPanel.TabIndex = 11;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(-6, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(239, 92);
            panel1.TabIndex = 6;
            // 
            // RaidCodeEntry
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Magenta;
            ClientSize = new Size(219, 208);
            Controls.Add(ScreenToTextPanel);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(panel5);
            Controls.Add(panel4);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "RaidCodeEntry";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RaidCodeEntry";
            TransparencyKey = Color.Magenta;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox1;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private CheckBox checkBox1;
        private Button EnterButton;
        private Button ClearAll;
        private Button AutoPaste;
        private Button button4;
        private Panel ScreenToTextPanel;
        private Panel panel1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label label7;
    }
}