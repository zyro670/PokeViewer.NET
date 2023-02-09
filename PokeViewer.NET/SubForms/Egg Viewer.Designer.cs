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
            this.components = new System.ComponentModel.Container();
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
            this.WebHookURL = new System.Windows.Forms.Label();
            this.DiscordUserID = new System.Windows.Forms.Label();
            this.ScreenshotEggView = new System.Windows.Forms.Button();
            this.PingOnReset = new System.Windows.Forms.CheckBox();
            this.SpeIVCheckbox = new System.Windows.Forms.CheckBox();
            this.SpDIVCheckbox = new System.Windows.Forms.CheckBox();
            this.SpAIVCheckbox = new System.Windows.Forms.CheckBox();
            this.DefIVCheckbox = new System.Windows.Forms.CheckBox();
            this.AtkIVCheckbox = new System.Windows.Forms.CheckBox();
            this.HPIVCheckbox = new System.Windows.Forms.CheckBox();
            this.HPIVNumber = new System.Windows.Forms.NumericUpDown();
            this.AtkIVNumber = new System.Windows.Forms.NumericUpDown();
            this.SpAIVNumber = new System.Windows.Forms.NumericUpDown();
            this.DefIVNumber = new System.Windows.Forms.NumericUpDown();
            this.SpeIVNumber = new System.Windows.Forms.NumericUpDown();
            this.SpDIVNumber = new System.Windows.Forms.NumericUpDown();
            this.IVBox = new System.Windows.Forms.GroupBox();
            this.genderDropdown = new System.Windows.Forms.ComboBox();
            this.genderLabel = new System.Windows.Forms.Label();
            this.RarityBox = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PokeSpriteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BallBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFillings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HPIVNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AtkIVNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAIVNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefIVNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeIVNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDIVNumber)).BeginInit();
            this.RarityBox.SuspendLayout();
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
            this.PokeStats.ReadOnly = true;
            this.PokeStats.Size = new System.Drawing.Size(146, 135);
            this.PokeStats.TabIndex = 1;
            this.PokeStats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FetchButton
            // 
            this.FetchButton.Location = new System.Drawing.Point(16, 248);
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
            this.HardStopButton.Location = new System.Drawing.Point(97, 248);
            this.HardStopButton.Name = "HardStopButton";
            this.HardStopButton.Size = new System.Drawing.Size(65, 23);
            this.HardStopButton.TabIndex = 5;
            this.HardStopButton.Text = "HardStop";
            this.HardStopButton.UseVisualStyleBackColor = true;
            this.HardStopButton.Click += new System.EventHandler(this.HardStop_Click);
            // 
            // StopOnShiny
            // 
            this.StopOnShiny.AutoSize = true;
            this.StopOnShiny.Location = new System.Drawing.Point(183, 21);
            this.StopOnShiny.Name = "StopOnShiny";
            this.StopOnShiny.Size = new System.Drawing.Size(101, 19);
            this.StopOnShiny.TabIndex = 11;
            this.StopOnShiny.Text = "Stop On Shiny";
            this.toolTip1.SetToolTip(this.StopOnShiny, "If checked, will stop on any shiny found");
            this.StopOnShiny.UseVisualStyleBackColor = true;
            // 
            // CheckBoxOf3
            // 
            this.CheckBoxOf3.AutoSize = true;
            this.CheckBoxOf3.Location = new System.Drawing.Point(183, 43);
            this.CheckBoxOf3.Name = "CheckBoxOf3";
            this.CheckBoxOf3.Size = new System.Drawing.Size(145, 19);
            this.CheckBoxOf3.TabIndex = 12;
            this.CheckBoxOf3.Text = "3 Segment/Family of 3";
            this.toolTip1.SetToolTip(this.CheckBoxOf3, "If checked, if breeding Dunsparce or Tandemaus, will filter out shinies to only t" +
        "he rare forms. Only works with Shiny stop checked.");
            this.CheckBoxOf3.UseVisualStyleBackColor = true;
            // 
            // EatOnStart
            // 
            this.EatOnStart.AutoSize = true;
            this.EatOnStart.Location = new System.Drawing.Point(183, 130);
            this.EatOnStart.Name = "EatOnStart";
            this.EatOnStart.Size = new System.Drawing.Size(88, 19);
            this.EatOnStart.TabIndex = 13;
            this.EatOnStart.Text = "Eat On Start";
            this.EatOnStart.UseVisualStyleBackColor = true;
            // 
            // Item1Value
            // 
            this.Item1Value.Location = new System.Drawing.Point(393, 232);
            this.Item1Value.Name = "Item1Value";
            this.Item1Value.Size = new System.Drawing.Size(32, 23);
            this.Item1Value.TabIndex = 14;
            this.Item1Value.Text = "0";
            this.Item1Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Item1Label
            // 
            this.Item1Label.AutoSize = true;
            this.Item1Label.Location = new System.Drawing.Point(343, 237);
            this.Item1Label.Name = "Item1Label";
            this.Item1Label.Size = new System.Drawing.Size(40, 15);
            this.Item1Label.TabIndex = 15;
            this.Item1Label.Text = "Item 1";
            // 
            // Item2Label
            // 
            this.Item2Label.AutoSize = true;
            this.Item2Label.Location = new System.Drawing.Point(343, 262);
            this.Item2Label.Name = "Item2Label";
            this.Item2Label.Size = new System.Drawing.Size(40, 15);
            this.Item2Label.TabIndex = 16;
            this.Item2Label.Text = "Item 2";
            // 
            // Item2Value
            // 
            this.Item2Value.Location = new System.Drawing.Point(393, 260);
            this.Item2Value.Name = "Item2Value";
            this.Item2Value.Size = new System.Drawing.Size(32, 23);
            this.Item2Value.TabIndex = 17;
            this.Item2Value.Text = "5";
            this.Item2Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EatAgain
            // 
            this.EatAgain.AutoSize = true;
            this.EatAgain.Location = new System.Drawing.Point(183, 152);
            this.EatAgain.Name = "EatAgain";
            this.EatAgain.Size = new System.Drawing.Size(76, 19);
            this.EatAgain.TabIndex = 18;
            this.EatAgain.Text = "Eat Again";
            this.EatAgain.UseVisualStyleBackColor = true;
            // 
            // Item3Label
            // 
            this.Item3Label.AutoSize = true;
            this.Item3Label.Location = new System.Drawing.Point(343, 289);
            this.Item3Label.Name = "Item3Label";
            this.Item3Label.Size = new System.Drawing.Size(40, 15);
            this.Item3Label.TabIndex = 19;
            this.Item3Label.Text = "Item 3";
            // 
            // Item3Value
            // 
            this.Item3Value.Location = new System.Drawing.Point(393, 288);
            this.Item3Value.Name = "Item3Value";
            this.Item3Value.Size = new System.Drawing.Size(32, 23);
            this.Item3Value.TabIndex = 20;
            this.Item3Value.Text = "0";
            this.Item3Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DUPItem1
            // 
            this.DUPItem1.AutoSize = true;
            this.DUPItem1.Location = new System.Drawing.Point(440, 236);
            this.DUPItem1.Name = "DUPItem1";
            this.DUPItem1.Size = new System.Drawing.Size(49, 19);
            this.DUPItem1.TabIndex = 21;
            this.DUPItem1.Text = "DUP";
            this.DUPItem1.UseVisualStyleBackColor = true;
            // 
            // DUPItem2
            // 
            this.DUPItem2.AutoSize = true;
            this.DUPItem2.Checked = true;
            this.DUPItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DUPItem2.Location = new System.Drawing.Point(440, 262);
            this.DUPItem2.Name = "DUPItem2";
            this.DUPItem2.Size = new System.Drawing.Size(49, 19);
            this.DUPItem2.TabIndex = 22;
            this.DUPItem2.Text = "DUP";
            this.DUPItem2.UseVisualStyleBackColor = true;
            // 
            // DUPItem3
            // 
            this.DUPItem3.AutoSize = true;
            this.DUPItem3.Checked = true;
            this.DUPItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DUPItem3.Location = new System.Drawing.Point(440, 289);
            this.DUPItem3.Name = "DUPItem3";
            this.DUPItem3.Size = new System.Drawing.Size(49, 19);
            this.DUPItem3.TabIndex = 23;
            this.DUPItem3.Text = "DUP";
            this.DUPItem3.UseVisualStyleBackColor = true;
            // 
            // SandwichCount
            // 
            this.SandwichCount.AutoSize = true;
            this.SandwichCount.Location = new System.Drawing.Point(13, 280);
            this.SandwichCount.Name = "SandwichCount";
            this.SandwichCount.Size = new System.Drawing.Size(108, 15);
            this.SandwichCount.TabIndex = 28;
            this.SandwichCount.Text = "Sandwiches Made: ";
            // 
            // ShinyFoundLabel
            // 
            this.ShinyFoundLabel.AutoSize = true;
            this.ShinyFoundLabel.Location = new System.Drawing.Point(177, 306);
            this.ShinyFoundLabel.Name = "ShinyFoundLabel";
            this.ShinyFoundLabel.Size = new System.Drawing.Size(87, 15);
            this.ShinyFoundLabel.TabIndex = 29;
            this.ShinyFoundLabel.Text = "Shinies Found: ";
            // 
            // BasketCount
            // 
            this.BasketCount.AutoSize = true;
            this.BasketCount.Location = new System.Drawing.Point(177, 287);
            this.BasketCount.Name = "BasketCount";
            this.BasketCount.Size = new System.Drawing.Size(83, 15);
            this.BasketCount.TabIndex = 30;
            this.BasketCount.Text = "Basket Count: ";
            // 
            // NextSanwichLabel
            // 
            this.NextSanwichLabel.AutoSize = true;
            this.NextSanwichLabel.Location = new System.Drawing.Point(13, 299);
            this.NextSanwichLabel.Name = "NextSanwichLabel";
            this.NextSanwichLabel.Size = new System.Drawing.Size(89, 15);
            this.NextSanwichLabel.TabIndex = 31;
            this.NextSanwichLabel.Text = "Next Sandwich:";
            // 
            // HoldIngredients
            // 
            this.HoldIngredients.AutoSize = true;
            this.HoldIngredients.Location = new System.Drawing.Point(183, 174);
            this.HoldIngredients.Name = "HoldIngredients";
            this.HoldIngredients.Size = new System.Drawing.Size(92, 19);
            this.HoldIngredients.TabIndex = 32;
            this.HoldIngredients.Text = "Hold Fillings";
            this.HoldIngredients.UseVisualStyleBackColor = true;
            // 
            // NumberOfFillings
            // 
            this.NumberOfFillings.Location = new System.Drawing.Point(270, 237);
            this.NumberOfFillings.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumberOfFillings.Name = "NumberOfFillings";
            this.NumberOfFillings.Size = new System.Drawing.Size(49, 23);
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
            this.label1.Location = new System.Drawing.Point(179, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 34;
            this.label1.Text = "Fillings Count?";
            // 
            // HoldTimeToFillings
            // 
            this.HoldTimeToFillings.AutoSize = true;
            this.HoldTimeToFillings.Location = new System.Drawing.Point(178, 266);
            this.HoldTimeToFillings.Name = "HoldTimeToFillings";
            this.HoldTimeToFillings.Size = new System.Drawing.Size(108, 15);
            this.HoldTimeToFillings.TabIndex = 35;
            this.HoldTimeToFillings.Text = "Fillings HOLD Time";
            // 
            // FillingHoldTime
            // 
            this.FillingHoldTime.Location = new System.Drawing.Point(286, 264);
            this.FillingHoldTime.Name = "FillingHoldTime";
            this.FillingHoldTime.Size = new System.Drawing.Size(33, 23);
            this.FillingHoldTime.TabIndex = 36;
            // 
            // WebHookText
            // 
            this.WebHookText.Location = new System.Drawing.Point(127, 327);
            this.WebHookText.Name = "WebHookText";
            this.WebHookText.Size = new System.Drawing.Size(111, 23);
            this.WebHookText.TabIndex = 37;
            this.WebHookText.UseSystemPasswordChar = true;
            // 
            // SaveValues
            // 
            this.SaveValues.Location = new System.Drawing.Point(12, 347);
            this.SaveValues.Name = "SaveValues";
            this.SaveValues.Size = new System.Drawing.Size(105, 23);
            this.SaveValues.TabIndex = 39;
            this.SaveValues.Text = "Save Fields";
            this.SaveValues.UseVisualStyleBackColor = true;
            this.SaveValues.Click += new System.EventHandler(this.SaveValues_Click);
            // 
            // UserDiscordIDText
            // 
            this.UserDiscordIDText.Location = new System.Drawing.Point(127, 354);
            this.UserDiscordIDText.Name = "UserDiscordIDText";
            this.UserDiscordIDText.Size = new System.Drawing.Size(111, 23);
            this.UserDiscordIDText.TabIndex = 40;
            // 
            // WebHookURL
            // 
            this.WebHookURL.AutoSize = true;
            this.WebHookURL.Location = new System.Drawing.Point(244, 330);
            this.WebHookURL.Name = "WebHookURL";
            this.WebHookURL.Size = new System.Drawing.Size(101, 15);
            this.WebHookURL.TabIndex = 41;
            this.WebHookURL.Text = "Discord Webhook";
            // 
            // DiscordUserID
            // 
            this.DiscordUserID.AutoSize = true;
            this.DiscordUserID.Location = new System.Drawing.Point(244, 356);
            this.DiscordUserID.Name = "DiscordUserID";
            this.DiscordUserID.Size = new System.Drawing.Size(87, 15);
            this.DiscordUserID.TabIndex = 42;
            this.DiscordUserID.Text = "Discord User ID";
            // 
            // ScreenshotEggView
            // 
            this.ScreenshotEggView.Location = new System.Drawing.Point(12, 320);
            this.ScreenshotEggView.Name = "ScreenshotEggView";
            this.ScreenshotEggView.Size = new System.Drawing.Size(105, 23);
            this.ScreenshotEggView.TabIndex = 43;
            this.ScreenshotEggView.Text = "Screenshot";
            this.ScreenshotEggView.UseVisualStyleBackColor = true;
            this.ScreenshotEggView.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // PingOnReset
            // 
            this.PingOnReset.AutoSize = true;
            this.PingOnReset.Location = new System.Drawing.Point(182, 195);
            this.PingOnReset.Name = "PingOnReset";
            this.PingOnReset.Size = new System.Drawing.Size(98, 19);
            this.PingOnReset.TabIndex = 44;
            this.PingOnReset.Text = "Ping on Reset";
            this.PingOnReset.UseVisualStyleBackColor = true;
            // 
            // SpeIVCheckbox
            // 
            this.SpeIVCheckbox.AutoSize = true;
            this.SpeIVCheckbox.Location = new System.Drawing.Point(364, 136);
            this.SpeIVCheckbox.Name = "SpeIVCheckbox";
            this.SpeIVCheckbox.Size = new System.Drawing.Size(45, 19);
            this.SpeIVCheckbox.TabIndex = 50;
            this.SpeIVCheckbox.Text = "Spe";
            this.SpeIVCheckbox.UseVisualStyleBackColor = true;
            // 
            // SpDIVCheckbox
            // 
            this.SpDIVCheckbox.AutoSize = true;
            this.SpDIVCheckbox.Location = new System.Drawing.Point(364, 115);
            this.SpDIVCheckbox.Name = "SpDIVCheckbox";
            this.SpDIVCheckbox.Size = new System.Drawing.Size(47, 19);
            this.SpDIVCheckbox.TabIndex = 49;
            this.SpDIVCheckbox.Text = "SpD";
            this.SpDIVCheckbox.UseVisualStyleBackColor = true;
            // 
            // SpAIVCheckbox
            // 
            this.SpAIVCheckbox.AutoSize = true;
            this.SpAIVCheckbox.Location = new System.Drawing.Point(364, 94);
            this.SpAIVCheckbox.Name = "SpAIVCheckbox";
            this.SpAIVCheckbox.Size = new System.Drawing.Size(47, 19);
            this.SpAIVCheckbox.TabIndex = 48;
            this.SpAIVCheckbox.Text = "SpA";
            this.SpAIVCheckbox.UseVisualStyleBackColor = true;
            // 
            // DefIVCheckbox
            // 
            this.DefIVCheckbox.AutoSize = true;
            this.DefIVCheckbox.Location = new System.Drawing.Point(364, 69);
            this.DefIVCheckbox.Name = "DefIVCheckbox";
            this.DefIVCheckbox.Size = new System.Drawing.Size(44, 19);
            this.DefIVCheckbox.TabIndex = 47;
            this.DefIVCheckbox.Text = "Def";
            this.DefIVCheckbox.UseVisualStyleBackColor = true;
            // 
            // AtkIVCheckbox
            // 
            this.AtkIVCheckbox.AutoSize = true;
            this.AtkIVCheckbox.Location = new System.Drawing.Point(364, 48);
            this.AtkIVCheckbox.Name = "AtkIVCheckbox";
            this.AtkIVCheckbox.Size = new System.Drawing.Size(44, 19);
            this.AtkIVCheckbox.TabIndex = 46;
            this.AtkIVCheckbox.Text = "Atk";
            this.AtkIVCheckbox.UseVisualStyleBackColor = true;
            // 
            // HPIVCheckbox
            // 
            this.HPIVCheckbox.AutoSize = true;
            this.HPIVCheckbox.Location = new System.Drawing.Point(364, 24);
            this.HPIVCheckbox.Name = "HPIVCheckbox";
            this.HPIVCheckbox.Size = new System.Drawing.Size(42, 19);
            this.HPIVCheckbox.TabIndex = 45;
            this.HPIVCheckbox.Text = "HP";
            this.HPIVCheckbox.UseVisualStyleBackColor = true;
            // 
            // HPIVNumber
            // 
            this.HPIVNumber.Location = new System.Drawing.Point(410, 20);
            this.HPIVNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.HPIVNumber.Name = "HPIVNumber";
            this.HPIVNumber.Size = new System.Drawing.Size(44, 23);
            this.HPIVNumber.TabIndex = 57;
            this.HPIVNumber.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // AtkIVNumber
            // 
            this.AtkIVNumber.Location = new System.Drawing.Point(410, 43);
            this.AtkIVNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.AtkIVNumber.Name = "AtkIVNumber";
            this.AtkIVNumber.Size = new System.Drawing.Size(44, 23);
            this.AtkIVNumber.TabIndex = 58;
            this.AtkIVNumber.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SpAIVNumber
            // 
            this.SpAIVNumber.Location = new System.Drawing.Point(410, 88);
            this.SpAIVNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SpAIVNumber.Name = "SpAIVNumber";
            this.SpAIVNumber.Size = new System.Drawing.Size(44, 23);
            this.SpAIVNumber.TabIndex = 60;
            this.SpAIVNumber.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // DefIVNumber
            // 
            this.DefIVNumber.Location = new System.Drawing.Point(410, 66);
            this.DefIVNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.DefIVNumber.Name = "DefIVNumber";
            this.DefIVNumber.Size = new System.Drawing.Size(44, 23);
            this.DefIVNumber.TabIndex = 59;
            this.DefIVNumber.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SpeIVNumber
            // 
            this.SpeIVNumber.Location = new System.Drawing.Point(410, 134);
            this.SpeIVNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SpeIVNumber.Name = "SpeIVNumber";
            this.SpeIVNumber.Size = new System.Drawing.Size(44, 23);
            this.SpeIVNumber.TabIndex = 62;
            this.SpeIVNumber.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SpDIVNumber
            // 
            this.SpDIVNumber.Location = new System.Drawing.Point(410, 111);
            this.SpDIVNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SpDIVNumber.Name = "SpDIVNumber";
            this.SpDIVNumber.Size = new System.Drawing.Size(44, 23);
            this.SpDIVNumber.TabIndex = 61;
            this.SpDIVNumber.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // IVBox
            // 
            this.IVBox.Location = new System.Drawing.Point(355, 6);
            this.IVBox.Name = "IVBox";
            this.IVBox.Size = new System.Drawing.Size(109, 164);
            this.IVBox.TabIndex = 63;
            this.IVBox.TabStop = false;
            this.IVBox.Text = "IVs";
            this.toolTip1.SetToolTip(this.IVBox, "Will stop only at pokémons matching a specific IV Spread");
            // 
            // genderDropdown
            // 
            this.genderDropdown.FormattingEnabled = true;
            this.genderDropdown.Items.AddRange(new object[] {
            "Any",
            "Male",
            "Female"});
            this.genderDropdown.Location = new System.Drawing.Point(67, 63);
            this.genderDropdown.MaxDropDownItems = 3;
            this.genderDropdown.Name = "genderDropdown";
            this.genderDropdown.Size = new System.Drawing.Size(89, 23);
            this.genderDropdown.TabIndex = 64;
            this.genderDropdown.Text = "Any";
            this.toolTip1.SetToolTip(this.genderDropdown, "Will only stop for shiny of a determined gender. Use Any for Genderless");
            // 
            // genderLabel
            // 
            this.genderLabel.AutoSize = true;
            this.genderLabel.Location = new System.Drawing.Point(16, 66);
            this.genderLabel.Name = "genderLabel";
            this.genderLabel.Size = new System.Drawing.Size(45, 15);
            this.genderLabel.TabIndex = 65;
            this.genderLabel.Text = "Gender";
            // 
            // RarityBox
            // 
            this.RarityBox.Controls.Add(this.genderLabel);
            this.RarityBox.Controls.Add(this.genderDropdown);
            this.RarityBox.Location = new System.Drawing.Point(177, 5);
            this.RarityBox.Name = "RarityBox";
            this.RarityBox.Size = new System.Drawing.Size(160, 98);
            this.RarityBox.TabIndex = 66;
            this.RarityBox.TabStop = false;
            this.RarityBox.Text = "Rarity";
            // 
            // Egg_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 396);
            this.Controls.Add(this.SpeIVNumber);
            this.Controls.Add(this.SpDIVNumber);
            this.Controls.Add(this.SpAIVNumber);
            this.Controls.Add(this.DefIVNumber);
            this.Controls.Add(this.AtkIVNumber);
            this.Controls.Add(this.HPIVNumber);
            this.Controls.Add(this.SpeIVCheckbox);
            this.Controls.Add(this.SpDIVCheckbox);
            this.Controls.Add(this.SpAIVCheckbox);
            this.Controls.Add(this.DefIVCheckbox);
            this.Controls.Add(this.AtkIVCheckbox);
            this.Controls.Add(this.HPIVCheckbox);
            this.Controls.Add(this.PingOnReset);
            this.Controls.Add(this.ScreenshotEggView);
            this.Controls.Add(this.DiscordUserID);
            this.Controls.Add(this.WebHookURL);
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
            this.Controls.Add(this.StopOnShiny);
            this.Controls.Add(this.HardStopButton);
            this.Controls.Add(this.BallBox);
            this.Controls.Add(this.FetchButton);
            this.Controls.Add(this.PokeStats);
            this.Controls.Add(this.PokeSpriteBox);
            this.Controls.Add(this.IVBox);
            this.Controls.Add(this.CheckBoxOf3);
            this.Controls.Add(this.RarityBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Egg_Viewer";
            this.Text = "Masuda Bot";
            ((System.ComponentModel.ISupportInitialize)(this.PokeSpriteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BallBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFillings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HPIVNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AtkIVNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAIVNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefIVNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeIVNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDIVNumber)).EndInit();
            this.RarityBox.ResumeLayout(false);
            this.RarityBox.PerformLayout();
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
        private Label WebHookURL;
        private Label DiscordUserID;
        private Button ScreenshotEggView;
        private CheckBox PingOnReset;
        private CheckBox SpeIVCheckbox;
        private CheckBox SpDIVCheckbox;
        private CheckBox SpAIVCheckbox;
        private CheckBox DefIVCheckbox;
        private CheckBox AtkIVCheckbox;
        private CheckBox HPIVCheckbox;
        private NumericUpDown HPIVNumber;
        private NumericUpDown AtkIVNumber;
        private NumericUpDown SpAIVNumber;
        private NumericUpDown DefIVNumber;
        private NumericUpDown SpeIVNumber;
        private NumericUpDown SpDIVNumber;
        private GroupBox IVBox;
        private ComboBox genderDropdown;
        private Label genderLabel;
        private GroupBox RarityBox;
        private ToolTip toolTip1;
    }
}