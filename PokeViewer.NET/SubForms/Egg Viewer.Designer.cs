using PokeViewer.NET.Properties;

namespace PokeViewer.NET.SubForms
{
    partial class Egg_Viewer
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
            PokeSpriteBox = new PictureBox();
            PokeStats = new TextBox();
            FetchButton = new Button();
            BallBox = new PictureBox();
            HardStopButton = new Button();
            EatOnStart = new CheckBox();
            Item1Value = new TextBox();
            Item1Label = new Label();
            Item2Label = new Label();
            Item2Value = new TextBox();
            EatAgain = new CheckBox();
            Item3Label = new Label();
            Item3Value = new TextBox();
            checkBox5 = new CheckBox();
            checkBox6 = new CheckBox();
            checkBox7 = new CheckBox();
            SandwichCount = new Label();
            ShinyFoundLabel = new Label();
            BasketCount = new Label();
            NextSanwichLabel = new Label();
            HoldIngredients = new CheckBox();
            NumberOfFillings = new NumericUpDown();
            label1 = new Label();
            HoldTimeToFillings = new Label();
            FillingHoldTime = new TextBox();
            StopConditionsButton = new Button();
            DisplayPartyCheck = new CheckBox();
            UptimeLabel = new Label();
            SquareStarCount = new Label();
            HidePIDEC = new CheckBox();
            ScreenshotButton = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            pictureBox6 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)PokeSpriteBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BallBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumberOfFillings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            SuspendLayout();
            // 
            // PokeSpriteBox
            // 
            PokeSpriteBox.BackColor = Color.WhiteSmoke;
            PokeSpriteBox.BorderStyle = BorderStyle.FixedSingle;
            PokeSpriteBox.Location = new Point(91, 39);
            PokeSpriteBox.Name = "PokeSpriteBox";
            PokeSpriteBox.Size = new Size(94, 72);
            PokeSpriteBox.SizeMode = PictureBoxSizeMode.Zoom;
            PokeSpriteBox.TabIndex = 0;
            PokeSpriteBox.TabStop = false;
            // 
            // PokeStats
            // 
            PokeStats.Location = new Point(91, 117);
            PokeStats.Multiline = true;
            PokeStats.Name = "PokeStats";
            PokeStats.ReadOnly = true;
            PokeStats.Size = new Size(146, 124);
            PokeStats.TabIndex = 1;
            PokeStats.TextAlign = HorizontalAlignment.Center;
            // 
            // FetchButton
            // 
            FetchButton.BackColor = Color.Transparent;
            FetchButton.Location = new Point(91, 250);
            FetchButton.Name = "FetchButton";
            FetchButton.Size = new Size(75, 23);
            FetchButton.TabIndex = 2;
            FetchButton.Text = "Fetch";
            FetchButton.UseVisualStyleBackColor = false;
            FetchButton.Click += button1_Click;
            // 
            // BallBox
            // 
            BallBox.BackColor = Color.WhiteSmoke;
            BallBox.BorderStyle = BorderStyle.FixedSingle;
            BallBox.Location = new Point(191, 74);
            BallBox.Name = "BallBox";
            BallBox.Size = new Size(46, 37);
            BallBox.SizeMode = PictureBoxSizeMode.CenterImage;
            BallBox.TabIndex = 4;
            BallBox.TabStop = false;
            // 
            // HardStopButton
            // 
            HardStopButton.BackColor = Color.Transparent;
            HardStopButton.Location = new Point(172, 250);
            HardStopButton.Name = "HardStopButton";
            HardStopButton.Size = new Size(65, 23);
            HardStopButton.TabIndex = 5;
            HardStopButton.Text = "HardStop";
            HardStopButton.UseVisualStyleBackColor = false;
            HardStopButton.Click += HardStopButton_Click;
            // 
            // EatOnStart
            // 
            EatOnStart.AutoSize = true;
            EatOnStart.Location = new Point(258, 54);
            EatOnStart.Name = "EatOnStart";
            EatOnStart.Size = new Size(93, 19);
            EatOnStart.TabIndex = 13;
            EatOnStart.Text = "Eat On Start?";
            EatOnStart.UseVisualStyleBackColor = true;
            // 
            // Item1Value
            // 
            Item1Value.BackColor = SystemColors.Control;
            Item1Value.Location = new Point(305, 140);
            Item1Value.Name = "Item1Value";
            Item1Value.Size = new Size(32, 23);
            Item1Value.TabIndex = 14;
            Item1Value.Text = "0";
            Item1Value.TextAlign = HorizontalAlignment.Center;
            // 
            // Item1Label
            // 
            Item1Label.AutoSize = true;
            Item1Label.Location = new Point(255, 145);
            Item1Label.Name = "Item1Label";
            Item1Label.Size = new Size(40, 15);
            Item1Label.TabIndex = 15;
            Item1Label.Text = "Item 1";
            // 
            // Item2Label
            // 
            Item2Label.AutoSize = true;
            Item2Label.Location = new Point(255, 170);
            Item2Label.Name = "Item2Label";
            Item2Label.Size = new Size(40, 15);
            Item2Label.TabIndex = 16;
            Item2Label.Text = "Item 2";
            // 
            // Item2Value
            // 
            Item2Value.BackColor = SystemColors.Control;
            Item2Value.Location = new Point(305, 168);
            Item2Value.Name = "Item2Value";
            Item2Value.Size = new Size(32, 23);
            Item2Value.TabIndex = 17;
            Item2Value.Text = "4";
            Item2Value.TextAlign = HorizontalAlignment.Center;
            // 
            // EatAgain
            // 
            EatAgain.AutoSize = true;
            EatAgain.BackColor = Color.Transparent;
            EatAgain.Location = new Point(258, 76);
            EatAgain.Name = "EatAgain";
            EatAgain.Size = new Size(81, 19);
            EatAgain.TabIndex = 18;
            EatAgain.Text = "Eat Again?";
            EatAgain.UseVisualStyleBackColor = false;
            // 
            // Item3Label
            // 
            Item3Label.AutoSize = true;
            Item3Label.Location = new Point(255, 197);
            Item3Label.Name = "Item3Label";
            Item3Label.Size = new Size(40, 15);
            Item3Label.TabIndex = 19;
            Item3Label.Text = "Item 3";
            // 
            // Item3Value
            // 
            Item3Value.BackColor = SystemColors.Control;
            Item3Value.Location = new Point(305, 196);
            Item3Value.Name = "Item3Value";
            Item3Value.Size = new Size(32, 23);
            Item3Value.TabIndex = 20;
            Item3Value.Text = "1";
            Item3Value.TextAlign = HorizontalAlignment.Center;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(352, 144);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(49, 19);
            checkBox5.TabIndex = 21;
            checkBox5.Text = "DUP";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Checked = true;
            checkBox6.CheckState = CheckState.Checked;
            checkBox6.Location = new Point(352, 170);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(49, 19);
            checkBox6.TabIndex = 22;
            checkBox6.Text = "DUP";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Checked = true;
            checkBox7.CheckState = CheckState.Checked;
            checkBox7.Location = new Point(352, 197);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(49, 19);
            checkBox7.TabIndex = 23;
            checkBox7.Text = "DUP";
            checkBox7.UseVisualStyleBackColor = true;
            // 
            // SandwichCount
            // 
            SandwichCount.AutoSize = true;
            SandwichCount.Location = new Point(88, 281);
            SandwichCount.Name = "SandwichCount";
            SandwichCount.Size = new Size(108, 15);
            SandwichCount.TabIndex = 28;
            SandwichCount.Text = "Sandwiches Made: ";
            // 
            // ShinyFoundLabel
            // 
            ShinyFoundLabel.AutoSize = true;
            ShinyFoundLabel.Location = new Point(252, 300);
            ShinyFoundLabel.Name = "ShinyFoundLabel";
            ShinyFoundLabel.Size = new Size(87, 15);
            ShinyFoundLabel.TabIndex = 29;
            ShinyFoundLabel.Text = "Shinies Found: ";
            // 
            // BasketCount
            // 
            BasketCount.AutoSize = true;
            BasketCount.Location = new Point(252, 281);
            BasketCount.Name = "BasketCount";
            BasketCount.Size = new Size(83, 15);
            BasketCount.TabIndex = 30;
            BasketCount.Text = "Basket Count: ";
            // 
            // NextSanwichLabel
            // 
            NextSanwichLabel.AutoSize = true;
            NextSanwichLabel.Location = new Point(88, 300);
            NextSanwichLabel.Name = "NextSanwichLabel";
            NextSanwichLabel.Size = new Size(89, 15);
            NextSanwichLabel.TabIndex = 31;
            NextSanwichLabel.Text = "Next Sandwich:";
            // 
            // HoldIngredients
            // 
            HoldIngredients.AutoSize = true;
            HoldIngredients.Location = new Point(258, 98);
            HoldIngredients.Name = "HoldIngredients";
            HoldIngredients.Size = new Size(97, 19);
            HoldIngredients.TabIndex = 32;
            HoldIngredients.Text = "Hold Fillings?";
            HoldIngredients.UseVisualStyleBackColor = true;
            // 
            // NumberOfFillings
            // 
            NumberOfFillings.BackColor = SystemColors.Control;
            NumberOfFillings.Location = new Point(345, 228);
            NumberOfFillings.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            NumberOfFillings.Name = "NumberOfFillings";
            NumberOfFillings.Size = new Size(49, 23);
            NumberOfFillings.TabIndex = 33;
            NumberOfFillings.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(254, 233);
            label1.Name = "label1";
            label1.Size = new Size(85, 15);
            label1.TabIndex = 34;
            label1.Text = "Fillings Count?";
            // 
            // HoldTimeToFillings
            // 
            HoldTimeToFillings.AutoSize = true;
            HoldTimeToFillings.Location = new Point(253, 260);
            HoldTimeToFillings.Name = "HoldTimeToFillings";
            HoldTimeToFillings.Size = new Size(108, 15);
            HoldTimeToFillings.TabIndex = 35;
            HoldTimeToFillings.Text = "Fillings HOLD Time";
            // 
            // FillingHoldTime
            // 
            FillingHoldTime.BackColor = SystemColors.Control;
            FillingHoldTime.Location = new Point(361, 258);
            FillingHoldTime.Name = "FillingHoldTime";
            FillingHoldTime.Size = new Size(33, 23);
            FillingHoldTime.TabIndex = 36;
            FillingHoldTime.Text = "690";
            // 
            // StopConditionsButton
            // 
            StopConditionsButton.BackColor = Color.Transparent;
            StopConditionsButton.Location = new Point(87, 339);
            StopConditionsButton.Name = "StopConditionsButton";
            StopConditionsButton.Size = new Size(105, 23);
            StopConditionsButton.TabIndex = 63;
            StopConditionsButton.Text = "Stop Conditions";
            StopConditionsButton.UseVisualStyleBackColor = false;
            StopConditionsButton.Click += StopConditionsButton_Click;
            // 
            // DisplayPartyCheck
            // 
            DisplayPartyCheck.AutoSize = true;
            DisplayPartyCheck.Location = new Point(258, 119);
            DisplayPartyCheck.Name = "DisplayPartyCheck";
            DisplayPartyCheck.Size = new Size(99, 19);
            DisplayPartyCheck.TabIndex = 64;
            DisplayPartyCheck.Text = "Display Party?";
            DisplayPartyCheck.UseVisualStyleBackColor = true;
            // 
            // UptimeLabel
            // 
            UptimeLabel.AutoSize = true;
            UptimeLabel.Location = new Point(88, 320);
            UptimeLabel.Name = "UptimeLabel";
            UptimeLabel.Size = new Size(52, 15);
            UptimeLabel.TabIndex = 65;
            UptimeLabel.Text = "Uptime: ";
            // 
            // SquareStarCount
            // 
            SquareStarCount.AutoSize = true;
            SquareStarCount.Location = new Point(252, 320);
            SquareStarCount.Name = "SquareStarCount";
            SquareStarCount.Size = new Size(70, 15);
            SquareStarCount.TabIndex = 66;
            SquareStarCount.Text = "■ - 0 | ★ - 0";
            // 
            // HidePIDEC
            // 
            HidePIDEC.AutoSize = true;
            HidePIDEC.Location = new Point(258, 33);
            HidePIDEC.Name = "HidePIDEC";
            HidePIDEC.Size = new Size(129, 19);
            HidePIDEC.TabIndex = 67;
            HidePIDEC.Text = "Hide Sensitive Info?";
            HidePIDEC.UseVisualStyleBackColor = true;
            // 
            // ScreenshotButton
            // 
            ScreenshotButton.BackColor = Color.Transparent;
            ScreenshotButton.Location = new Point(254, 339);
            ScreenshotButton.Name = "ScreenshotButton";
            ScreenshotButton.Size = new Size(105, 23);
            ScreenshotButton.TabIndex = 68;
            ScreenshotButton.Text = "Screenshot";
            ScreenshotButton.UseVisualStyleBackColor = false;
            ScreenshotButton.Click += ScreenshotButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(72, 368);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.TabIndex = 69;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(126, 368);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(50, 50);
            pictureBox2.TabIndex = 70;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Location = new Point(180, 368);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(50, 50);
            pictureBox3.TabIndex = 71;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(234, 368);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(50, 50);
            pictureBox4.TabIndex = 72;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Location = new Point(288, 368);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(50, 50);
            pictureBox5.TabIndex = 73;
            pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            pictureBox6.Location = new Point(343, 368);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(50, 50);
            pictureBox6.TabIndex = 74;
            pictureBox6.TabStop = false;
            // 
            // Egg_Viewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(426, 404);
            Controls.Add(pictureBox6);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(ScreenshotButton);
            Controls.Add(HidePIDEC);
            Controls.Add(SquareStarCount);
            Controls.Add(UptimeLabel);
            Controls.Add(DisplayPartyCheck);
            Controls.Add(StopConditionsButton);
            Controls.Add(FillingHoldTime);
            Controls.Add(HoldTimeToFillings);
            Controls.Add(label1);
            Controls.Add(NumberOfFillings);
            Controls.Add(HoldIngredients);
            Controls.Add(NextSanwichLabel);
            Controls.Add(BasketCount);
            Controls.Add(ShinyFoundLabel);
            Controls.Add(SandwichCount);
            Controls.Add(checkBox7);
            Controls.Add(checkBox6);
            Controls.Add(checkBox5);
            Controls.Add(Item3Value);
            Controls.Add(Item3Label);
            Controls.Add(EatAgain);
            Controls.Add(Item2Value);
            Controls.Add(Item2Label);
            Controls.Add(Item1Label);
            Controls.Add(Item1Value);
            Controls.Add(EatOnStart);
            Controls.Add(HardStopButton);
            Controls.Add(BallBox);
            Controls.Add(FetchButton);
            Controls.Add(PokeStats);
            Controls.Add(PokeSpriteBox);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Egg_Viewer";
            Text = "Egg Viewer";
            ((System.ComponentModel.ISupportInitialize)PokeSpriteBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)BallBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumberOfFillings).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox PokeSpriteBox;
        private TextBox PokeStats;
        private Button FetchButton;
        private PictureBox BallBox;
        private Button HardStopButton;
        private CheckBox EatOnStart;
        private TextBox Item1Value;
        private Label Item1Label;
        private Label Item2Label;
        private TextBox Item2Value;
        private CheckBox EatAgain;
        private Label Item3Label;
        private TextBox Item3Value;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private CheckBox checkBox7;
        private Label SandwichCount;
        private Label ShinyFoundLabel;
        private Label BasketCount;
        private Label NextSanwichLabel;
        private CheckBox HoldIngredients;
        private NumericUpDown NumberOfFillings;
        private Label label1;
        private Label HoldTimeToFillings;
        private TextBox FillingHoldTime;
        private Button StopConditionsButton;
        private CheckBox DisplayPartyCheck;
        private Label UptimeLabel;
        private Label SquareStarCount;
        private CheckBox HidePIDEC;
        private Button ScreenshotButton;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private PictureBox pictureBox6;
    }
}