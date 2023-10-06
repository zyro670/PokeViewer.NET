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
            button1 = new Button();
            SeedToPokemonGroup = new GroupBox();
            Screenshot = new Button();
            checkBox1 = new CheckBox();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            Results = new TextBox();
            numericUpDown1 = new NumericUpDown();
            GoButton = new Button();
            SeedLabel = new Label();
            SeedToPokemonGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(29, 12);
            textBox1.MaxLength = 6;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 75);
            textBox1.TabIndex = 0;
            textBox1.DoubleClick += textBox1_DoubleClicked;
            // 
            // EnterButton
            // 
            EnterButton.BackColor = Color.Transparent;
            EnterButton.Location = new Point(29, 90);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new Size(93, 23);
            EnterButton.TabIndex = 1;
            EnterButton.Text = "Enter";
            EnterButton.UseVisualStyleBackColor = false;
            EnterButton.Click += button1_Click;
            // 
            // AutoPaste
            // 
            AutoPaste.BackColor = Color.Transparent;
            AutoPaste.Location = new Point(136, 90);
            AutoPaste.Name = "AutoPaste";
            AutoPaste.Size = new Size(93, 23);
            AutoPaste.TabIndex = 3;
            AutoPaste.Text = "Auto Paste";
            AutoPaste.UseVisualStyleBackColor = false;
            AutoPaste.Click += button3_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.Location = new Point(91, 119);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Clear All";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // SeedToPokemonGroup
            // 
            SeedToPokemonGroup.Controls.Add(Screenshot);
            SeedToPokemonGroup.Controls.Add(checkBox1);
            SeedToPokemonGroup.Controls.Add(pictureBox2);
            SeedToPokemonGroup.Controls.Add(pictureBox1);
            SeedToPokemonGroup.Controls.Add(Results);
            SeedToPokemonGroup.Controls.Add(numericUpDown1);
            SeedToPokemonGroup.Controls.Add(GoButton);
            SeedToPokemonGroup.Controls.Add(SeedLabel);
            SeedToPokemonGroup.Location = new Point(13, 148);
            SeedToPokemonGroup.Name = "SeedToPokemonGroup";
            SeedToPokemonGroup.Size = new Size(233, 193);
            SeedToPokemonGroup.TabIndex = 7;
            SeedToPokemonGroup.TabStop = false;
            SeedToPokemonGroup.Text = "Raid Viewer";
            // 
            // Screenshot
            // 
            Screenshot.BackColor = Color.Transparent;
            Screenshot.Location = new Point(143, 164);
            Screenshot.Name = "Screenshot";
            Screenshot.Size = new Size(86, 23);
            Screenshot.TabIndex = 13;
            Screenshot.Text = "Screenshot";
            Screenshot.UseVisualStyleBackColor = false;
            Screenshot.Click += Screenshot_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 167);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(129, 19);
            checkBox1.TabIndex = 12;
            checkBox1.Text = "Hide Sensitive Info?";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.Control;
            pictureBox2.Location = new Point(8, 50);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(56, 49);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 11;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.Control;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Location = new Point(168, 50);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(57, 49);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // Results
            // 
            Results.Location = new Point(6, 48);
            Results.Multiline = true;
            Results.Name = "Results";
            Results.ReadOnly = true;
            Results.Size = new Size(221, 114);
            Results.TabIndex = 7;
            Results.TextAlign = HorizontalAlignment.Center;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(50, 17);
            numericUpDown1.Maximum = new decimal(new int[] { 95, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(104, 23);
            numericUpDown1.TabIndex = 9;
            numericUpDown1.TextAlign = HorizontalAlignment.Right;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.ValueChanged += NumericValue_Changed;
            // 
            // GoButton
            // 
            GoButton.BackColor = Color.Transparent;
            GoButton.Location = new Point(160, 15);
            GoButton.Name = "GoButton";
            GoButton.Size = new Size(67, 25);
            GoButton.TabIndex = 8;
            GoButton.Text = "Go";
            GoButton.UseVisualStyleBackColor = false;
            GoButton.Click += GoButton_Click;
            // 
            // SeedLabel
            // 
            SeedLabel.AutoSize = true;
            SeedLabel.Location = new Point(6, 21);
            SeedLabel.Name = "SeedLabel";
            SeedLabel.Size = new Size(38, 15);
            SeedLabel.TabIndex = 6;
            SeedLabel.Text = "Den #";
            // 
            // RaidCodeEntry
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(258, 345);
            Controls.Add(SeedToPokemonGroup);
            Controls.Add(button1);
            Controls.Add(EnterButton);
            Controls.Add(AutoPaste);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "RaidCodeEntry";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RaidUtil - SV";
            SeedToPokemonGroup.ResumeLayout(false);
            SeedToPokemonGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button EnterButton;
        private Button AutoPaste;
        private Button button1;
        private GroupBox SeedToPokemonGroup;
        private TextBox Results;
        private Label SeedLabel;
        private Button GoButton;
        private NumericUpDown numericUpDown1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button Screenshot;
        private CheckBox checkBox1;
    }
}