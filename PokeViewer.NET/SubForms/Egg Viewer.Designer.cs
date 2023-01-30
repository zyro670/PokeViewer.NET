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
            this.PokeSpriteBox = new System.Windows.Forms.PictureBox();
            this.PokeStats = new System.Windows.Forms.TextBox();
            this.FetchButton = new System.Windows.Forms.Button();
            this.BallBox = new System.Windows.Forms.PictureBox();
            this.HardStopButton = new System.Windows.Forms.Button();
            this.StopOnShiny = new System.Windows.Forms.CheckBox();
            this.CheckBoxOf3 = new System.Windows.Forms.CheckBox();
            this.EatOnStart = new System.Windows.Forms.CheckBox();
            this.Item1Value = new System.Windows.Forms.TextBox();
            this.Item1Label = new System.Windows.Forms.Label();
            this.Item2Label = new System.Windows.Forms.Label();
            this.Item2Value = new System.Windows.Forms.TextBox();
            this.EatAgain = new System.Windows.Forms.CheckBox();
            this.Item3Label = new System.Windows.Forms.Label();
            this.Item3Value = new System.Windows.Forms.TextBox();
            this.DUPItem1 = new System.Windows.Forms.CheckBox();
            this.DUPItem2 = new System.Windows.Forms.CheckBox();
            this.DUPItem3 = new System.Windows.Forms.CheckBox();
            this.SandwichCount = new System.Windows.Forms.Label();
            this.ShinyFoundLabel = new System.Windows.Forms.Label();
            this.BasketCount = new System.Windows.Forms.Label();
            this.NextSanwichLabel = new System.Windows.Forms.Label();
            this.HoldIngredients = new System.Windows.Forms.CheckBox();
            this.NumberOfFillings = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.HoldTimeToFillings = new System.Windows.Forms.Label();
            this.FillingHoldTime = new System.Windows.Forms.TextBox();
            this.WebHookText = new System.Windows.Forms.TextBox();
            this.SaveValues = new System.Windows.Forms.Button();
            this.UserDiscordIDText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ScreenshotEggView = new System.Windows.Forms.Button();
            this.PingOnReset = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PokeSpriteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BallBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFillings)).BeginInit();
            this.SuspendLayout();
            // 
            // PokeSpriteBox
            // 
            this.PokeSpriteBox.Location = new System.Drawing.Point(31, 57);
            this.PokeSpriteBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PokeSpriteBox.Name = "PokeSpriteBox";
            this.PokeSpriteBox.Size = new System.Drawing.Size(126, 115);
            this.PokeSpriteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PokeSpriteBox.TabIndex = 0;
            this.PokeSpriteBox.TabStop = false;
            // 
            // PokeStats
            // 
            this.PokeStats.Location = new System.Drawing.Point(23, 182);
            this.PokeStats.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PokeStats.Multiline = true;
            this.PokeStats.Name = "PokeStats";
            this.PokeStats.Size = new System.Drawing.Size(207, 181);
            this.PokeStats.TabIndex = 1;
            this.PokeStats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FetchButton
            // 
            this.FetchButton.Location = new System.Drawing.Point(23, 375);
            this.FetchButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FetchButton.Name = "FetchButton";
            this.FetchButton.Size = new System.Drawing.Size(107, 38);
            this.FetchButton.TabIndex = 2;
            this.FetchButton.Text = "Fetch";
            this.FetchButton.UseVisualStyleBackColor = true;
            this.FetchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // BallBox
            // 
            this.BallBox.Location = new System.Drawing.Point(166, 110);
            this.BallBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BallBox.Name = "BallBox";
            this.BallBox.Size = new System.Drawing.Size(59, 62);
            this.BallBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.BallBox.TabIndex = 4;
            this.BallBox.TabStop = false;
            // 
            // HardStopButton
            // 
            this.HardStopButton.Location = new System.Drawing.Point(139, 375);
            this.HardStopButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.HardStopButton.Name = "HardStopButton";
            this.HardStopButton.Size = new System.Drawing.Size(93, 38);
            this.HardStopButton.TabIndex = 5;
            this.HardStopButton.Text = "HardStop";
            this.HardStopButton.UseVisualStyleBackColor = true;
            this.HardStopButton.Click += new System.EventHandler(this.HardStop_Click);
            // 
            // StopOnShiny
            // 
            this.StopOnShiny.AutoSize = true;
            this.StopOnShiny.Location = new System.Drawing.Point(261, 25);
            this.StopOnShiny.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StopOnShiny.Name = "StopOnShiny";
            this.StopOnShiny.Size = new System.Drawing.Size(160, 29);
            this.StopOnShiny.TabIndex = 11;
            this.StopOnShiny.Text = "Stop On Shiny?";
            this.StopOnShiny.UseVisualStyleBackColor = true;
            // 
            // CheckBoxOf3
            // 
            this.CheckBoxOf3.AutoSize = true;
            this.CheckBoxOf3.Location = new System.Drawing.Point(261, 62);
            this.CheckBoxOf3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CheckBoxOf3.Name = "CheckBoxOf3";
            this.CheckBoxOf3.Size = new System.Drawing.Size(226, 29);
            this.CheckBoxOf3.TabIndex = 12;
            this.CheckBoxOf3.Text = "3 Segment/Family of 3?";
            this.CheckBoxOf3.UseVisualStyleBackColor = true;
            // 
            // EatOnStart
            // 
            this.EatOnStart.AutoSize = true;
            this.EatOnStart.Location = new System.Drawing.Point(261, 98);
            this.EatOnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EatOnStart.Name = "EatOnStart";
            this.EatOnStart.Size = new System.Drawing.Size(140, 29);
            this.EatOnStart.TabIndex = 13;
            this.EatOnStart.Text = "Eat On Start?";
            this.EatOnStart.UseVisualStyleBackColor = true;
            // 
            // Item1Value
            // 
            this.Item1Value.Location = new System.Drawing.Point(329, 260);
            this.Item1Value.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Item1Value.Name = "Item1Value";
            this.Item1Value.Size = new System.Drawing.Size(44, 31);
            this.Item1Value.TabIndex = 14;
            this.Item1Value.Text = "0";
            this.Item1Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Item1Label
            // 
            this.Item1Label.AutoSize = true;
            this.Item1Label.Location = new System.Drawing.Point(257, 268);
            this.Item1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Item1Label.Name = "Item1Label";
            this.Item1Label.Size = new System.Drawing.Size(63, 25);
            this.Item1Label.TabIndex = 15;
            this.Item1Label.Text = "Item 1";
            // 
            // Item2Label
            // 
            this.Item2Label.AutoSize = true;
            this.Item2Label.Location = new System.Drawing.Point(257, 310);
            this.Item2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Item2Label.Name = "Item2Label";
            this.Item2Label.Size = new System.Drawing.Size(63, 25);
            this.Item2Label.TabIndex = 16;
            this.Item2Label.Text = "Item 2";
            // 
            // Item2Value
            // 
            this.Item2Value.Location = new System.Drawing.Point(329, 307);
            this.Item2Value.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Item2Value.Name = "Item2Value";
            this.Item2Value.Size = new System.Drawing.Size(44, 31);
            this.Item2Value.TabIndex = 17;
            this.Item2Value.Text = "5";
            this.Item2Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EatAgain
            // 
            this.EatAgain.AutoSize = true;
            this.EatAgain.Location = new System.Drawing.Point(261, 135);
            this.EatAgain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EatAgain.Name = "EatAgain";
            this.EatAgain.Size = new System.Drawing.Size(121, 29);
            this.EatAgain.TabIndex = 18;
            this.EatAgain.Text = "Eat Again?";
            this.EatAgain.UseVisualStyleBackColor = true;
            // 
            // Item3Label
            // 
            this.Item3Label.AutoSize = true;
            this.Item3Label.Location = new System.Drawing.Point(257, 355);
            this.Item3Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Item3Label.Name = "Item3Label";
            this.Item3Label.Size = new System.Drawing.Size(63, 25);
            this.Item3Label.TabIndex = 19;
            this.Item3Label.Text = "Item 3";
            // 
            // Item3Value
            // 
            this.Item3Value.Location = new System.Drawing.Point(329, 353);
            this.Item3Value.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Item3Value.Name = "Item3Value";
            this.Item3Value.Size = new System.Drawing.Size(44, 31);
            this.Item3Value.TabIndex = 20;
            this.Item3Value.Text = "0";
            this.Item3Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DUPItem1
            // 
            this.DUPItem1.AutoSize = true;
            this.DUPItem1.Location = new System.Drawing.Point(396, 267);
            this.DUPItem1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DUPItem1.Name = "DUPItem1";
            this.DUPItem1.Size = new System.Drawing.Size(73, 29);
            this.DUPItem1.TabIndex = 21;
            this.DUPItem1.Text = "DUP";
            this.DUPItem1.UseVisualStyleBackColor = true;
            // 
            // DUPItem2
            // 
            this.DUPItem2.AutoSize = true;
            this.DUPItem2.Checked = true;
            this.DUPItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DUPItem2.Location = new System.Drawing.Point(396, 310);
            this.DUPItem2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DUPItem2.Name = "DUPItem2";
            this.DUPItem2.Size = new System.Drawing.Size(73, 29);
            this.DUPItem2.TabIndex = 22;
            this.DUPItem2.Text = "DUP";
            this.DUPItem2.UseVisualStyleBackColor = true;
            // 
            // DUPItem3
            // 
            this.DUPItem3.AutoSize = true;
            this.DUPItem3.Checked = true;
            this.DUPItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DUPItem3.Location = new System.Drawing.Point(396, 355);
            this.DUPItem3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DUPItem3.Name = "DUPItem3";
            this.DUPItem3.Size = new System.Drawing.Size(73, 29);
            this.DUPItem3.TabIndex = 23;
            this.DUPItem3.Text = "DUP";
            this.DUPItem3.UseVisualStyleBackColor = true;
            // 
            // SandwichCount
            // 
            this.SandwichCount.AutoSize = true;
            this.SandwichCount.Location = new System.Drawing.Point(19, 428);
            this.SandwichCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SandwichCount.Name = "SandwichCount";
            this.SandwichCount.Size = new System.Drawing.Size(163, 25);
            this.SandwichCount.TabIndex = 28;
            this.SandwichCount.Text = "Sandwiches Made: ";
            // 
            // ShinyFoundLabel
            // 
            this.ShinyFoundLabel.AutoSize = true;
            this.ShinyFoundLabel.Location = new System.Drawing.Point(253, 510);
            this.ShinyFoundLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ShinyFoundLabel.Name = "ShinyFoundLabel";
            this.ShinyFoundLabel.Size = new System.Drawing.Size(132, 25);
            this.ShinyFoundLabel.TabIndex = 29;
            this.ShinyFoundLabel.Text = "Shinies Found: ";
            // 
            // BasketCount
            // 
            this.BasketCount.AutoSize = true;
            this.BasketCount.Location = new System.Drawing.Point(253, 478);
            this.BasketCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BasketCount.Name = "BasketCount";
            this.BasketCount.Size = new System.Drawing.Size(125, 25);
            this.BasketCount.TabIndex = 30;
            this.BasketCount.Text = "Basket Count: ";
            // 
            // NextSanwichLabel
            // 
            this.NextSanwichLabel.AutoSize = true;
            this.NextSanwichLabel.Location = new System.Drawing.Point(19, 460);
            this.NextSanwichLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NextSanwichLabel.Name = "NextSanwichLabel";
            this.NextSanwichLabel.Size = new System.Drawing.Size(132, 25);
            this.NextSanwichLabel.TabIndex = 31;
            this.NextSanwichLabel.Text = "Next Sandwich:";
            // 
            // HoldIngredients
            // 
            this.HoldIngredients.AutoSize = true;
            this.HoldIngredients.Location = new System.Drawing.Point(261, 172);
            this.HoldIngredients.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.HoldIngredients.Name = "HoldIngredients";
            this.HoldIngredients.Size = new System.Drawing.Size(144, 29);
            this.HoldIngredients.TabIndex = 32;
            this.HoldIngredients.Text = "Hold Fillings?";
            this.HoldIngredients.UseVisualStyleBackColor = true;
            // 
            // NumberOfFillings
            // 
            this.NumberOfFillings.Location = new System.Drawing.Point(386, 395);
            this.NumberOfFillings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NumberOfFillings.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumberOfFillings.Name = "NumberOfFillings";
            this.NumberOfFillings.Size = new System.Drawing.Size(70, 31);
            this.NumberOfFillings.TabIndex = 33;
            this.NumberOfFillings.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(256, 403);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 25);
            this.label1.TabIndex = 34;
            this.label1.Text = "Fillings Count?";
            // 
            // HoldTimeToFillings
            // 
            this.HoldTimeToFillings.AutoSize = true;
            this.HoldTimeToFillings.Location = new System.Drawing.Point(254, 443);
            this.HoldTimeToFillings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HoldTimeToFillings.Name = "HoldTimeToFillings";
            this.HoldTimeToFillings.Size = new System.Drawing.Size(162, 25);
            this.HoldTimeToFillings.TabIndex = 35;
            this.HoldTimeToFillings.Text = "Fillings HOLD Time";
            // 
            // FillingHoldTime
            // 
            this.FillingHoldTime.Location = new System.Drawing.Point(409, 440);
            this.FillingHoldTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FillingHoldTime.Name = "FillingHoldTime";
            this.FillingHoldTime.Size = new System.Drawing.Size(45, 31);
            this.FillingHoldTime.TabIndex = 36;
            // 
            // WebHookText
            // 
            this.WebHookText.Location = new System.Drawing.Point(181, 545);
            this.WebHookText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WebHookText.Name = "WebHookText";
            this.WebHookText.Size = new System.Drawing.Size(157, 31);
            this.WebHookText.TabIndex = 37;
            this.WebHookText.UseSystemPasswordChar = true;
            // 
            // SaveValues
            // 
            this.SaveValues.Location = new System.Drawing.Point(17, 540);
            this.SaveValues.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveValues.Name = "SaveValues";
            this.SaveValues.Size = new System.Drawing.Size(150, 38);
            this.SaveValues.TabIndex = 39;
            this.SaveValues.Text = "Save Fields";
            this.SaveValues.UseVisualStyleBackColor = true;
            this.SaveValues.Click += new System.EventHandler(this.SaveValues_Click);
            // 
            // UserDiscordIDText
            // 
            this.UserDiscordIDText.Location = new System.Drawing.Point(181, 590);
            this.UserDiscordIDText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UserDiscordIDText.Name = "UserDiscordIDText";
            this.UserDiscordIDText.Size = new System.Drawing.Size(157, 31);
            this.UserDiscordIDText.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(349, 550);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 25);
            this.label2.TabIndex = 41;
            this.label2.Text = "WebHook URL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(349, 593);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 25);
            this.label3.TabIndex = 42;
            this.label3.Text = "User ID To Ping";
            // 
            // ScreenshotEggView
            // 
            this.ScreenshotEggView.Location = new System.Drawing.Point(17, 495);
            this.ScreenshotEggView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScreenshotEggView.Name = "ScreenshotEggView";
            this.ScreenshotEggView.Size = new System.Drawing.Size(150, 38);
            this.ScreenshotEggView.TabIndex = 43;
            this.ScreenshotEggView.Text = "Screenshot";
            this.ScreenshotEggView.UseVisualStyleBackColor = true;
            this.ScreenshotEggView.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Liveness Check
            // 
            this.PingOnReset.AutoSize = true;
            this.PingOnReset.Location = new System.Drawing.Point(260, 207);
            this.PingOnReset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PingOnReset.Name = "PingOnReset";
            this.PingOnReset.Size = new System.Drawing.Size(144, 29);
            this.PingOnReset.TabIndex = 44;
            this.PingOnReset.Text = "Ping on Reset";
            this.PingOnReset.UseVisualStyleBackColor = true;
            this.PingOnReset.Checked = Settings.Default.LivenessPing;
            // 
            // Egg_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 652);
            this.Controls.Add(this.PingOnReset);
            this.Controls.Add(this.ScreenshotEggView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UserDiscordIDText);
            this.Controls.Add(this.SaveValues);
            this.Controls.Add(this.WebHookText);
            this.Controls.Add(this.FillingHoldTime);
            this.Controls.Add(this.HoldTimeToFillings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumberOfFillings);
            this.Controls.Add(this.HoldIngredients);
            this.Controls.Add(this.NextSanwichLabel);
            this.Controls.Add(this.BasketCount);
            this.Controls.Add(this.ShinyFoundLabel);
            this.Controls.Add(this.SandwichCount);
            this.Controls.Add(this.DUPItem3);
            this.Controls.Add(this.DUPItem2);
            this.Controls.Add(this.DUPItem1);
            this.Controls.Add(this.Item3Value);
            this.Controls.Add(this.Item3Label);
            this.Controls.Add(this.EatAgain);
            this.Controls.Add(this.Item2Value);
            this.Controls.Add(this.Item2Label);
            this.Controls.Add(this.Item1Label);
            this.Controls.Add(this.Item1Value);
            this.Controls.Add(this.EatOnStart);
            this.Controls.Add(this.CheckBoxOf3);
            this.Controls.Add(this.StopOnShiny);
            this.Controls.Add(this.HardStopButton);
            this.Controls.Add(this.BallBox);
            this.Controls.Add(this.FetchButton);
            this.Controls.Add(this.PokeStats);
            this.Controls.Add(this.PokeSpriteBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Egg_Viewer";
            this.Text = "Egg Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.PokeSpriteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BallBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFillings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox PokeSpriteBox;
        private TextBox PokeStats;
        private Button FetchButton;
        private PictureBox BallBox;
        private Button HardStopButton;
        private CheckBox StopOnShiny;
        private CheckBox CheckBoxOf3;
        private CheckBox EatOnStart;
        private TextBox Item1Value;
        private Label Item1Label;
        private Label Item2Label;
        private TextBox Item2Value;
        private CheckBox EatAgain;
        private Label Item3Label;
        private TextBox Item3Value;
        private CheckBox DUPItem1;
        private CheckBox DUPItem2;
        private CheckBox DUPItem3;
        private Label SandwichCount;
        private Label ShinyFoundLabel;
        private Label BasketCount;
        private Label NextSanwichLabel;
        private CheckBox HoldIngredients;
        private NumericUpDown NumberOfFillings;
        private Label label1;
        private Label HoldTimeToFillings;
        private TextBox FillingHoldTime;
        private TextBox WebHookText;
        private Button SaveValues;
        private TextBox UserDiscordIDText;
        private Label label2;
        private Label label3;
        private Button ScreenshotEggView;
        private CheckBox PingOnReset;
    }
}