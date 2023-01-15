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
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.SandwichCount = new System.Windows.Forms.Label();
            this.ShinyFoundLabel = new System.Windows.Forms.Label();
            this.BasketCount = new System.Windows.Forms.Label();
            this.NextSanwichLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PokeSpriteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BallBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PokeSpriteBox
            // 
            this.PokeSpriteBox.Location = new System.Drawing.Point(22, 34);
            this.PokeSpriteBox.Name = "PokeSpriteBox";
            this.PokeSpriteBox.Size = new System.Drawing.Size(88, 69);
            this.PokeSpriteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PokeSpriteBox.TabIndex = 0;
            this.PokeSpriteBox.TabStop = false;
            // 
            // PokeStats
            // 
            this.PokeStats.Location = new System.Drawing.Point(16, 109);
            this.PokeStats.Multiline = true;
            this.PokeStats.Name = "PokeStats";
            this.PokeStats.Size = new System.Drawing.Size(146, 110);
            this.PokeStats.TabIndex = 1;
            this.PokeStats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FetchButton
            // 
            this.FetchButton.Location = new System.Drawing.Point(16, 225);
            this.FetchButton.Name = "FetchButton";
            this.FetchButton.Size = new System.Drawing.Size(75, 23);
            this.FetchButton.TabIndex = 2;
            this.FetchButton.Text = "Fetch";
            this.FetchButton.UseVisualStyleBackColor = true;
            this.FetchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // BallBox
            // 
            this.BallBox.Location = new System.Drawing.Point(116, 66);
            this.BallBox.Name = "BallBox";
            this.BallBox.Size = new System.Drawing.Size(41, 37);
            this.BallBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.BallBox.TabIndex = 4;
            this.BallBox.TabStop = false;
            // 
            // HardStopButton
            // 
            this.HardStopButton.Location = new System.Drawing.Point(97, 225);
            this.HardStopButton.Name = "HardStopButton";
            this.HardStopButton.Size = new System.Drawing.Size(65, 23);
            this.HardStopButton.TabIndex = 5;
            this.HardStopButton.Text = "HardStop";
            this.HardStopButton.UseVisualStyleBackColor = true;
            this.HardStopButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // StopOnShiny
            // 
            this.StopOnShiny.AutoSize = true;
            this.StopOnShiny.Location = new System.Drawing.Point(180, 20);
            this.StopOnShiny.Name = "StopOnShiny";
            this.StopOnShiny.Size = new System.Drawing.Size(106, 19);
            this.StopOnShiny.TabIndex = 11;
            this.StopOnShiny.Text = "Stop On Shiny?";
            this.StopOnShiny.UseVisualStyleBackColor = true;
            // 
            // CheckBoxOf3
            // 
            this.CheckBoxOf3.AutoSize = true;
            this.CheckBoxOf3.Location = new System.Drawing.Point(180, 45);
            this.CheckBoxOf3.Name = "CheckBoxOf3";
            this.CheckBoxOf3.Size = new System.Drawing.Size(150, 19);
            this.CheckBoxOf3.TabIndex = 12;
            this.CheckBoxOf3.Text = "3 Segment/Family of 3?";
            this.CheckBoxOf3.UseVisualStyleBackColor = true;
            // 
            // EatOnStart
            // 
            this.EatOnStart.AutoSize = true;
            this.EatOnStart.Location = new System.Drawing.Point(180, 69);
            this.EatOnStart.Name = "EatOnStart";
            this.EatOnStart.Size = new System.Drawing.Size(93, 19);
            this.EatOnStart.TabIndex = 13;
            this.EatOnStart.Text = "Eat On Start?";
            this.EatOnStart.UseVisualStyleBackColor = true;
            // 
            // Item1Value
            // 
            this.Item1Value.Location = new System.Drawing.Point(230, 122);
            this.Item1Value.Name = "Item1Value";
            this.Item1Value.Size = new System.Drawing.Size(32, 23);
            this.Item1Value.TabIndex = 14;
            this.Item1Value.Text = "0";
            this.Item1Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Item1Label
            // 
            this.Item1Label.AutoSize = true;
            this.Item1Label.Location = new System.Drawing.Point(180, 125);
            this.Item1Label.Name = "Item1Label";
            this.Item1Label.Size = new System.Drawing.Size(40, 15);
            this.Item1Label.TabIndex = 15;
            this.Item1Label.Text = "Item 1";
            // 
            // Item2Label
            // 
            this.Item2Label.AutoSize = true;
            this.Item2Label.Location = new System.Drawing.Point(180, 150);
            this.Item2Label.Name = "Item2Label";
            this.Item2Label.Size = new System.Drawing.Size(40, 15);
            this.Item2Label.TabIndex = 16;
            this.Item2Label.Text = "Item 2";
            // 
            // Item2Value
            // 
            this.Item2Value.Location = new System.Drawing.Point(230, 148);
            this.Item2Value.Name = "Item2Value";
            this.Item2Value.Size = new System.Drawing.Size(32, 23);
            this.Item2Value.TabIndex = 17;
            this.Item2Value.Text = "4";
            this.Item2Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EatAgain
            // 
            this.EatAgain.AutoSize = true;
            this.EatAgain.Location = new System.Drawing.Point(180, 94);
            this.EatAgain.Name = "EatAgain";
            this.EatAgain.Size = new System.Drawing.Size(81, 19);
            this.EatAgain.TabIndex = 18;
            this.EatAgain.Text = "Eat Again?";
            this.EatAgain.UseVisualStyleBackColor = true;
            // 
            // Item3Label
            // 
            this.Item3Label.AutoSize = true;
            this.Item3Label.Location = new System.Drawing.Point(180, 177);
            this.Item3Label.Name = "Item3Label";
            this.Item3Label.Size = new System.Drawing.Size(40, 15);
            this.Item3Label.TabIndex = 19;
            this.Item3Label.Text = "Item 3";
            // 
            // Item3Value
            // 
            this.Item3Value.Location = new System.Drawing.Point(230, 176);
            this.Item3Value.Name = "Item3Value";
            this.Item3Value.Size = new System.Drawing.Size(32, 23);
            this.Item3Value.TabIndex = 20;
            this.Item3Value.Text = "1";
            this.Item3Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(277, 124);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(49, 19);
            this.checkBox5.TabIndex = 21;
            this.checkBox5.Text = "DUP";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.Location = new System.Drawing.Point(277, 150);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(49, 19);
            this.checkBox6.TabIndex = 22;
            this.checkBox6.Text = "DUP";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Checked = true;
            this.checkBox7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox7.Location = new System.Drawing.Point(277, 177);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(49, 19);
            this.checkBox7.TabIndex = 23;
            this.checkBox7.Text = "DUP";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // SandwichCount
            // 
            this.SandwichCount.AutoSize = true;
            this.SandwichCount.Location = new System.Drawing.Point(180, 222);
            this.SandwichCount.Name = "SandwichCount";
            this.SandwichCount.Size = new System.Drawing.Size(108, 15);
            this.SandwichCount.TabIndex = 28;
            this.SandwichCount.Text = "Sandwiches Made: ";
            // 
            // ShinyFoundLabel
            // 
            this.ShinyFoundLabel.AutoSize = true;
            this.ShinyFoundLabel.Location = new System.Drawing.Point(180, 257);
            this.ShinyFoundLabel.Name = "ShinyFoundLabel";
            this.ShinyFoundLabel.Size = new System.Drawing.Size(87, 15);
            this.ShinyFoundLabel.TabIndex = 29;
            this.ShinyFoundLabel.Text = "Shinies Found: ";
            // 
            // BasketCount
            // 
            this.BasketCount.AutoSize = true;
            this.BasketCount.Location = new System.Drawing.Point(180, 206);
            this.BasketCount.Name = "BasketCount";
            this.BasketCount.Size = new System.Drawing.Size(83, 15);
            this.BasketCount.TabIndex = 30;
            this.BasketCount.Text = "Basket Count: ";
            // 
            // NextSanwichLabel
            // 
            this.NextSanwichLabel.AutoSize = true;
            this.NextSanwichLabel.Location = new System.Drawing.Point(180, 239);
            this.NextSanwichLabel.Name = "NextSanwichLabel";
            this.NextSanwichLabel.Size = new System.Drawing.Size(89, 15);
            this.NextSanwichLabel.TabIndex = 31;
            this.NextSanwichLabel.Text = "Next Sandwich:";
            // 
            // Egg_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 281);
            this.Controls.Add(this.NextSanwichLabel);
            this.Controls.Add(this.BasketCount);
            this.Controls.Add(this.ShinyFoundLabel);
            this.Controls.Add(this.SandwichCount);
            this.Controls.Add(this.checkBox7);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox5);
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
            this.Name = "Egg_Viewer";
            this.Text = "Egg Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.PokeSpriteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BallBox)).EndInit();
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
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private CheckBox checkBox7;
        private Label SandwichCount;
        private Label ShinyFoundLabel;
        private Label BasketCount;
        private Label NextSanwichLabel;
    }
}