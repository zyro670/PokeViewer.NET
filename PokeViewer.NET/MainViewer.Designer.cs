namespace PokeViewer.NET
{
    partial class MainViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainViewer));
            Connect = new Button();
            SwitchIP = new TextBox();
            View = new Button();
            ViewBox = new TextBox();
            PokeSprite = new PictureBox();
            Specialty = new PictureBox();
            OriginIcon = new PictureBox();
            Typing1 = new PictureBox();
            HidePIDEC = new CheckBox();
            Typing2 = new PictureBox();
            LiveStats = new TextBox();
            RefreshStats = new CheckBox();
            RefreshBox = new TextBox();
            Refresh = new Label();
            TodaysDate = new Label();
            WindowCapture = new Button();
            HpLabel = new Label();
            UniqueBox = new CheckBox();
            UniqueBox2 = new CheckBox();
            InGameScreenshot = new Button();
            ToggleSwitchProtocol = new CheckBox();
            ViewerControl = new TabControl();
            ConnectionPage = new TabPage();
            TurboButton = new Button();
            EventRedeemButton = new Button();
            MoodGroup = new GroupBox();
            BackColorLabel = new Label();
            ForeColorLabel = new Label();
            AltBackCombo = new ComboBox();
            AltForeCombo = new ComboBox();
            SetMoodButton = new Button();
            DefaultMoodRadio = new RadioButton();
            AltMoodRadio = new RadioButton();
            ScreenLabel = new Label();
            TrainerPassportGroup = new GroupBox();
            TrainerPhoto = new PictureBox();
            TIDSID = new Label();
            TrainerLabel = new Label();
            ScreenOnLabel = new Label();
            ExtrasGroup = new GroupBox();
            MessageText = new TextBox();
            PingLabel = new Label();
            WideButton = new Button();
            RaidButton = new Button();
            DiscordIDText = new TextBox();
            WebhookURLText = new TextBox();
            DiscordID = new Label();
            WebHookURL = new Label();
            SaveButton = new Button();
            ScreenOffLabel = new Label();
            ConnectionGroupBox = new GroupBox();
            VersionLabel = new Label();
            ConnectionSpriteBox = new PictureBox();
            ScreenTrackBar = new TrackBar();
            ViewPage = new TabPage();
            BoxPage = new TabPage();
            PartyPage = new TabPage();
            EggPage = new TabPage();
            NPCPage = new TabPage();
            InGameScreenshotPage = new TabPage();
            MiscPage = new TabPage();
            ((System.ComponentModel.ISupportInitialize)PokeSprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Specialty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)OriginIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Typing1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Typing2).BeginInit();
            ViewerControl.SuspendLayout();
            ConnectionPage.SuspendLayout();
            MoodGroup.SuspendLayout();
            TrainerPassportGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrainerPhoto).BeginInit();
            ExtrasGroup.SuspendLayout();
            ConnectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ConnectionSpriteBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScreenTrackBar).BeginInit();
            ViewPage.SuspendLayout();
            SuspendLayout();
            // 
            // Connect
            // 
            Connect.Location = new Point(66, 57);
            Connect.Name = "Connect";
            Connect.Size = new Size(75, 25);
            Connect.TabIndex = 0;
            Connect.Text = "Connect";
            Connect.UseVisualStyleBackColor = true;
            Connect.Click += Connect_Click;
            // 
            // SwitchIP
            // 
            SwitchIP.Location = new Point(37, 26);
            SwitchIP.Name = "SwitchIP";
            SwitchIP.Size = new Size(128, 23);
            SwitchIP.TabIndex = 2;
            SwitchIP.TextAlign = HorizontalAlignment.Center;
            SwitchIP.TextChanged += CheckForIP;
            // 
            // View
            // 
            View.Location = new Point(221, 264);
            View.Name = "View";
            View.Size = new Size(47, 23);
            View.TabIndex = 3;
            View.Text = "View";
            View.UseVisualStyleBackColor = true;
            View.Click += View_Click;
            // 
            // ViewBox
            // 
            ViewBox.BackColor = SystemColors.GradientInactiveCaption;
            ViewBox.BorderStyle = BorderStyle.FixedSingle;
            ViewBox.ForeColor = SystemColors.InfoText;
            ViewBox.Location = new Point(34, 88);
            ViewBox.Multiline = true;
            ViewBox.Name = "ViewBox";
            ViewBox.ReadOnly = true;
            ViewBox.Size = new Size(182, 169);
            ViewBox.TabIndex = 4;
            ViewBox.TextAlign = HorizontalAlignment.Center;
            // 
            // PokeSprite
            // 
            PokeSprite.BackColor = SystemColors.GradientInactiveCaption;
            PokeSprite.BorderStyle = BorderStyle.FixedSingle;
            PokeSprite.Location = new Point(274, 88);
            PokeSprite.Name = "PokeSprite";
            PokeSprite.Size = new Size(182, 169);
            PokeSprite.SizeMode = PictureBoxSizeMode.CenterImage;
            PokeSprite.TabIndex = 7;
            PokeSprite.TabStop = false;
            // 
            // Specialty
            // 
            Specialty.Location = new Point(221, 295);
            Specialty.Name = "Specialty";
            Specialty.Size = new Size(47, 41);
            Specialty.SizeMode = PictureBoxSizeMode.Zoom;
            Specialty.TabIndex = 8;
            Specialty.TabStop = false;
            // 
            // OriginIcon
            // 
            OriginIcon.ImageLocation = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
            OriginIcon.Location = new Point(213, 18);
            OriginIcon.Name = "OriginIcon";
            OriginIcon.Size = new Size(64, 64);
            OriginIcon.SizeMode = PictureBoxSizeMode.AutoSize;
            OriginIcon.TabIndex = 10;
            OriginIcon.TabStop = false;
            // 
            // Typing1
            // 
            Typing1.Location = new Point(274, 294);
            Typing1.Name = "Typing1";
            Typing1.Size = new Size(182, 42);
            Typing1.SizeMode = PictureBoxSizeMode.StretchImage;
            Typing1.TabIndex = 12;
            Typing1.TabStop = false;
            // 
            // HidePIDEC
            // 
            HidePIDEC.AutoSize = true;
            HidePIDEC.Location = new Point(39, 57);
            HidePIDEC.Name = "HidePIDEC";
            HidePIDEC.Size = new Size(91, 19);
            HidePIDEC.TabIndex = 13;
            HidePIDEC.Text = "Hide PID/EC";
            HidePIDEC.UseVisualStyleBackColor = true;
            HidePIDEC.CheckedChanged += CheckForHide;
            // 
            // Typing2
            // 
            Typing2.Location = new Point(274, 351);
            Typing2.Name = "Typing2";
            Typing2.Size = new Size(182, 41);
            Typing2.SizeMode = PictureBoxSizeMode.StretchImage;
            Typing2.TabIndex = 14;
            Typing2.TabStop = false;
            // 
            // LiveStats
            // 
            LiveStats.BackColor = SystemColors.GradientInactiveCaption;
            LiveStats.Location = new Point(34, 294);
            LiveStats.Multiline = true;
            LiveStats.Name = "LiveStats";
            LiveStats.ReadOnly = true;
            LiveStats.ScrollBars = ScrollBars.Vertical;
            LiveStats.Size = new Size(182, 98);
            LiveStats.TabIndex = 15;
            LiveStats.TextAlign = HorizontalAlignment.Center;
            // 
            // RefreshStats
            // 
            RefreshStats.AutoSize = true;
            RefreshStats.Location = new Point(39, 267);
            RefreshStats.Name = "RefreshStats";
            RefreshStats.Size = new Size(72, 19);
            RefreshStats.TabIndex = 16;
            RefreshStats.Text = "LiveStats";
            RefreshStats.UseVisualStyleBackColor = true;
            RefreshStats.CheckedChanged += RefreshStats_CheckedChanged;
            // 
            // RefreshBox
            // 
            RefreshBox.Location = new Point(112, 403);
            RefreshBox.Name = "RefreshBox";
            RefreshBox.Size = new Size(71, 23);
            RefreshBox.TabIndex = 17;
            RefreshBox.Text = "2000";
            RefreshBox.TextAlign = HorizontalAlignment.Center;
            // 
            // Refresh
            // 
            Refresh.AutoSize = true;
            Refresh.Location = new Point(34, 407);
            Refresh.Name = "Refresh";
            Refresh.Size = new Size(75, 15);
            Refresh.TabIndex = 18;
            Refresh.Text = "Refresh Rate:";
            // 
            // TodaysDate
            // 
            TodaysDate.AutoSize = true;
            TodaysDate.Location = new Point(274, 407);
            TodaysDate.Name = "TodaysDate";
            TodaysDate.Size = new Size(57, 15);
            TodaysDate.TabIndex = 19;
            TodaysDate.Text = "DateTime";
            // 
            // WindowCapture
            // 
            WindowCapture.Image = (Image)resources.GetObject("WindowCapture.Image");
            WindowCapture.Location = new Point(169, 57);
            WindowCapture.Name = "WindowCapture";
            WindowCapture.Size = new Size(37, 25);
            WindowCapture.TabIndex = 20;
            WindowCapture.UseVisualStyleBackColor = true;
            WindowCapture.Click += CaptureWindow_Click;
            // 
            // HpLabel
            // 
            HpLabel.AutoSize = true;
            HpLabel.Location = new Point(126, 268);
            HpLabel.Name = "HpLabel";
            HpLabel.Size = new Size(63, 15);
            HpLabel.TabIndex = 21;
            HpLabel.Text = "          HP%";
            // 
            // UniqueBox
            // 
            UniqueBox.AutoSize = true;
            UniqueBox.Location = new Point(283, 267);
            UniqueBox.Name = "UniqueBox";
            UniqueBox.Size = new Size(15, 14);
            UniqueBox.TabIndex = 22;
            UniqueBox.UseVisualStyleBackColor = true;
            UniqueBox.Visible = false;
            // 
            // UniqueBox2
            // 
            UniqueBox2.AutoSize = true;
            UniqueBox2.Location = new Point(356, 267);
            UniqueBox2.Name = "UniqueBox2";
            UniqueBox2.Size = new Size(15, 14);
            UniqueBox2.TabIndex = 23;
            UniqueBox2.UseVisualStyleBackColor = true;
            UniqueBox2.Visible = false;
            // 
            // InGameScreenshot
            // 
            InGameScreenshot.Image = (Image)resources.GetObject("InGameScreenshot.Image");
            InGameScreenshot.Location = new Point(285, 57);
            InGameScreenshot.Name = "InGameScreenshot";
            InGameScreenshot.Size = new Size(37, 25);
            InGameScreenshot.TabIndex = 30;
            InGameScreenshot.UseVisualStyleBackColor = true;
            InGameScreenshot.Click += InGameScreenshot_Click;
            // 
            // ToggleSwitchProtocol
            // 
            ToggleSwitchProtocol.AutoSize = true;
            ToggleSwitchProtocol.Location = new Point(15, 19);
            ToggleSwitchProtocol.Name = "ToggleSwitchProtocol";
            ToggleSwitchProtocol.Size = new Size(52, 19);
            ToggleSwitchProtocol.TabIndex = 32;
            ToggleSwitchProtocol.Text = "USB?";
            ToggleSwitchProtocol.UseVisualStyleBackColor = true;
            ToggleSwitchProtocol.CheckedChanged += CheckForUSBChecked;
            // 
            // ViewerControl
            // 
            ViewerControl.Controls.Add(ConnectionPage);
            ViewerControl.Controls.Add(ViewPage);
            ViewerControl.Controls.Add(BoxPage);
            ViewerControl.Controls.Add(PartyPage);
            ViewerControl.Controls.Add(EggPage);
            ViewerControl.Controls.Add(NPCPage);
            ViewerControl.Controls.Add(InGameScreenshotPage);
            ViewerControl.Controls.Add(MiscPage);
            ViewerControl.Location = new Point(-5, 2);
            ViewerControl.Name = "ViewerControl";
            ViewerControl.SelectedIndex = 0;
            ViewerControl.Size = new Size(511, 510);
            ViewerControl.TabIndex = 33;
            ViewerControl.SelectedIndexChanged += ViewerControl_SelectedIndexChanged;
            // 
            // ConnectionPage
            // 
            ConnectionPage.BackColor = SystemColors.ControlLightLight;
            ConnectionPage.Controls.Add(TurboButton);
            ConnectionPage.Controls.Add(EventRedeemButton);
            ConnectionPage.Controls.Add(MoodGroup);
            ConnectionPage.Controls.Add(ScreenLabel);
            ConnectionPage.Controls.Add(TrainerPassportGroup);
            ConnectionPage.Controls.Add(ScreenOnLabel);
            ConnectionPage.Controls.Add(ExtrasGroup);
            ConnectionPage.Controls.Add(ScreenOffLabel);
            ConnectionPage.Controls.Add(ConnectionGroupBox);
            ConnectionPage.Controls.Add(ScreenTrackBar);
            ConnectionPage.Location = new Point(4, 24);
            ConnectionPage.Name = "ConnectionPage";
            ConnectionPage.Padding = new Padding(3);
            ConnectionPage.Size = new Size(503, 482);
            ConnectionPage.TabIndex = 0;
            ConnectionPage.Text = "Connection 🔌";
            // 
            // TurboButton
            // 
            TurboButton.Location = new Point(407, 6);
            TurboButton.Name = "TurboButton";
            TurboButton.Size = new Size(75, 23);
            TurboButton.TabIndex = 50;
            TurboButton.Text = "Turbo";
            TurboButton.UseVisualStyleBackColor = true;
            TurboButton.Click += button1_Click_1;
            // 
            // EventRedeemButton
            // 
            EventRedeemButton.Location = new Point(407, 31);
            EventRedeemButton.Name = "EventRedeemButton";
            EventRedeemButton.Size = new Size(75, 23);
            EventRedeemButton.TabIndex = 49;
            EventRedeemButton.Text = "Event";
            EventRedeemButton.UseVisualStyleBackColor = true;
            EventRedeemButton.Visible = false;
            EventRedeemButton.Click += button1_Click;
            // 
            // MoodGroup
            // 
            MoodGroup.Controls.Add(BackColorLabel);
            MoodGroup.Controls.Add(ForeColorLabel);
            MoodGroup.Controls.Add(AltBackCombo);
            MoodGroup.Controls.Add(AltForeCombo);
            MoodGroup.Controls.Add(SetMoodButton);
            MoodGroup.Controls.Add(DefaultMoodRadio);
            MoodGroup.Controls.Add(AltMoodRadio);
            MoodGroup.Location = new Point(279, 288);
            MoodGroup.Name = "MoodGroup";
            MoodGroup.Size = new Size(212, 132);
            MoodGroup.TabIndex = 48;
            MoodGroup.TabStop = false;
            MoodGroup.Text = "Mood";
            // 
            // BackColorLabel
            // 
            BackColorLabel.AutoSize = true;
            BackColorLabel.Location = new Point(6, 103);
            BackColorLabel.Name = "BackColorLabel";
            BackColorLabel.Size = new Size(61, 15);
            BackColorLabel.TabIndex = 50;
            BackColorLabel.Text = "BackColor";
            // 
            // ForeColorLabel
            // 
            ForeColorLabel.AutoSize = true;
            ForeColorLabel.Location = new Point(6, 74);
            ForeColorLabel.Name = "ForeColorLabel";
            ForeColorLabel.Size = new Size(59, 15);
            ForeColorLabel.TabIndex = 49;
            ForeColorLabel.Text = "ForeColor";
            // 
            // AltBackCombo
            // 
            AltBackCombo.FormattingEnabled = true;
            AltBackCombo.Location = new Point(71, 100);
            AltBackCombo.Name = "AltBackCombo";
            AltBackCombo.Size = new Size(132, 23);
            AltBackCombo.TabIndex = 48;
            AltBackCombo.SelectedIndexChanged += AltBackCombo_SelectedIndexChanged;
            // 
            // AltForeCombo
            // 
            AltForeCombo.FormattingEnabled = true;
            AltForeCombo.Location = new Point(71, 71);
            AltForeCombo.Name = "AltForeCombo";
            AltForeCombo.Size = new Size(132, 23);
            AltForeCombo.TabIndex = 37;
            AltForeCombo.SelectedIndexChanged += ColorComboBox_SelectedIndexChanged;
            // 
            // SetMoodButton
            // 
            SetMoodButton.Location = new Point(128, 22);
            SetMoodButton.Name = "SetMoodButton";
            SetMoodButton.Size = new Size(75, 23);
            SetMoodButton.TabIndex = 47;
            SetMoodButton.Text = "Set Mood";
            SetMoodButton.UseVisualStyleBackColor = true;
            SetMoodButton.Click += SetMoodButton_Click;
            // 
            // DefaultMoodRadio
            // 
            DefaultMoodRadio.AutoSize = true;
            DefaultMoodRadio.Location = new Point(11, 22);
            DefaultMoodRadio.Name = "DefaultMoodRadio";
            DefaultMoodRadio.Size = new Size(98, 19);
            DefaultMoodRadio.TabIndex = 45;
            DefaultMoodRadio.TabStop = true;
            DefaultMoodRadio.Text = "Default Mood";
            DefaultMoodRadio.UseVisualStyleBackColor = true;
            DefaultMoodRadio.CheckedChanged += DefaultMoodRadio_CheckedChanged;
            // 
            // AltMoodRadio
            // 
            AltMoodRadio.AutoSize = true;
            AltMoodRadio.Location = new Point(11, 46);
            AltMoodRadio.Name = "AltMoodRadio";
            AltMoodRadio.Size = new Size(108, 19);
            AltMoodRadio.TabIndex = 46;
            AltMoodRadio.TabStop = true;
            AltMoodRadio.Text = "Alternate Mood";
            AltMoodRadio.UseVisualStyleBackColor = true;
            AltMoodRadio.CheckedChanged += AltMoodRadio_CheckedChanged;
            // 
            // ScreenLabel
            // 
            ScreenLabel.AutoSize = true;
            ScreenLabel.Location = new Point(43, 28);
            ScreenLabel.Name = "ScreenLabel";
            ScreenLabel.Size = new Size(42, 15);
            ScreenLabel.TabIndex = 44;
            ScreenLabel.Text = "Screen";
            // 
            // TrainerPassportGroup
            // 
            TrainerPassportGroup.BackColor = Color.Transparent;
            TrainerPassportGroup.Controls.Add(TrainerPhoto);
            TrainerPassportGroup.Controls.Add(TIDSID);
            TrainerPassportGroup.Controls.Add(TrainerLabel);
            TrainerPassportGroup.Location = new Point(108, 195);
            TrainerPassportGroup.Name = "TrainerPassportGroup";
            TrainerPassportGroup.Size = new Size(264, 81);
            TrainerPassportGroup.TabIndex = 36;
            TrainerPassportGroup.TabStop = false;
            TrainerPassportGroup.Text = "Trainer Passport";
            // 
            // TrainerPhoto
            // 
            TrainerPhoto.Location = new Point(190, 16);
            TrainerPhoto.Name = "TrainerPhoto";
            TrainerPhoto.Size = new Size(69, 57);
            TrainerPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            TrainerPhoto.TabIndex = 39;
            TrainerPhoto.TabStop = false;
            // 
            // TIDSID
            // 
            TIDSID.AutoSize = true;
            TIDSID.Location = new Point(6, 49);
            TIDSID.Name = "TIDSID";
            TIDSID.Size = new Size(56, 15);
            TIDSID.TabIndex = 38;
            TIDSID.Text = "TID | SID: ";
            // 
            // TrainerLabel
            // 
            TrainerLabel.AutoSize = true;
            TrainerLabel.Location = new Point(6, 24);
            TrainerLabel.Name = "TrainerLabel";
            TrainerLabel.Size = new Size(45, 15);
            TrainerLabel.TabIndex = 37;
            TrainerLabel.Text = "Name: ";
            // 
            // ScreenOnLabel
            // 
            ScreenOnLabel.AutoSize = true;
            ScreenOnLabel.Location = new Point(97, 28);
            ScreenOnLabel.Name = "ScreenOnLabel";
            ScreenOnLabel.Size = new Size(25, 15);
            ScreenOnLabel.TabIndex = 43;
            ScreenOnLabel.Text = "ON";
            // 
            // ExtrasGroup
            // 
            ExtrasGroup.BackColor = Color.Transparent;
            ExtrasGroup.Controls.Add(MessageText);
            ExtrasGroup.Controls.Add(PingLabel);
            ExtrasGroup.Controls.Add(WideButton);
            ExtrasGroup.Controls.Add(RaidButton);
            ExtrasGroup.Controls.Add(DiscordIDText);
            ExtrasGroup.Controls.Add(WebhookURLText);
            ExtrasGroup.Controls.Add(DiscordID);
            ExtrasGroup.Controls.Add(ToggleSwitchProtocol);
            ExtrasGroup.Controls.Add(WebHookURL);
            ExtrasGroup.Controls.Add(SaveButton);
            ExtrasGroup.Location = new Point(7, 288);
            ExtrasGroup.Name = "ExtrasGroup";
            ExtrasGroup.Size = new Size(266, 132);
            ExtrasGroup.TabIndex = 35;
            ExtrasGroup.TabStop = false;
            ExtrasGroup.Text = "Extras";
            // 
            // MessageText
            // 
            MessageText.Location = new Point(68, 100);
            MessageText.Name = "MessageText";
            MessageText.Size = new Size(185, 23);
            MessageText.TabIndex = 51;
            // 
            // PingLabel
            // 
            PingLabel.AutoSize = true;
            PingLabel.Location = new Point(1, 103);
            PingLabel.Name = "PingLabel";
            PingLabel.Size = new Size(53, 15);
            PingLabel.TabIndex = 39;
            PingLabel.Text = "Message";
            // 
            // WideButton
            // 
            WideButton.Enabled = false;
            WideButton.Location = new Point(131, 15);
            WideButton.Name = "WideButton";
            WideButton.Size = new Size(59, 23);
            WideButton.TabIndex = 37;
            WideButton.Text = "Wide";
            WideButton.UseVisualStyleBackColor = true;
            WideButton.Click += WideButton_Click;
            // 
            // RaidButton
            // 
            RaidButton.Enabled = false;
            RaidButton.Location = new Point(195, 15);
            RaidButton.Name = "RaidButton";
            RaidButton.Size = new Size(58, 23);
            RaidButton.TabIndex = 38;
            RaidButton.Text = "Raid";
            RaidButton.UseVisualStyleBackColor = true;
            RaidButton.Click += RaidButton_Click;
            // 
            // DiscordIDText
            // 
            DiscordIDText.Location = new Point(68, 70);
            DiscordIDText.Name = "DiscordIDText";
            DiscordIDText.Size = new Size(185, 23);
            DiscordIDText.TabIndex = 4;
            DiscordIDText.TextAlign = HorizontalAlignment.Center;
            // 
            // WebhookURLText
            // 
            WebhookURLText.Location = new Point(68, 41);
            WebhookURLText.Name = "WebhookURLText";
            WebhookURLText.Size = new Size(185, 23);
            WebhookURLText.TabIndex = 3;
            WebhookURLText.TextAlign = HorizontalAlignment.Center;
            WebhookURLText.UseSystemPasswordChar = true;
            // 
            // DiscordID
            // 
            DiscordID.AutoSize = true;
            DiscordID.Location = new Point(1, 73);
            DiscordID.Name = "DiscordID";
            DiscordID.Size = new Size(61, 15);
            DiscordID.TabIndex = 2;
            DiscordID.Text = "Discord ID";
            // 
            // WebHookURL
            // 
            WebHookURL.AutoSize = true;
            WebHookURL.Location = new Point(1, 44);
            WebHookURL.Name = "WebHookURL";
            WebHookURL.Size = new Size(58, 15);
            WebHookURL.TabIndex = 1;
            WebHookURL.Text = "Webhook";
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(67, 15);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(59, 23);
            SaveButton.TabIndex = 0;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // ScreenOffLabel
            // 
            ScreenOffLabel.AutoSize = true;
            ScreenOffLabel.Location = new Point(3, 28);
            ScreenOffLabel.Name = "ScreenOffLabel";
            ScreenOffLabel.Size = new Size(28, 15);
            ScreenOffLabel.TabIndex = 42;
            ScreenOffLabel.Text = "OFF";
            // 
            // ConnectionGroupBox
            // 
            ConnectionGroupBox.BackColor = Color.Transparent;
            ConnectionGroupBox.Controls.Add(VersionLabel);
            ConnectionGroupBox.Controls.Add(ConnectionSpriteBox);
            ConnectionGroupBox.Controls.Add(Connect);
            ConnectionGroupBox.Controls.Add(SwitchIP);
            ConnectionGroupBox.Location = new Point(138, 21);
            ConnectionGroupBox.Name = "ConnectionGroupBox";
            ConnectionGroupBox.Size = new Size(200, 168);
            ConnectionGroupBox.TabIndex = 34;
            ConnectionGroupBox.TabStop = false;
            ConnectionGroupBox.Text = "Switch IP";
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.Location = new Point(159, 10);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(37, 15);
            VersionLabel.TabIndex = 36;
            VersionLabel.Text = "v1.0.0";
            // 
            // ConnectionSpriteBox
            // 
            ConnectionSpriteBox.Location = new Point(71, 90);
            ConnectionSpriteBox.Name = "ConnectionSpriteBox";
            ConnectionSpriteBox.Size = new Size(64, 64);
            ConnectionSpriteBox.TabIndex = 35;
            ConnectionSpriteBox.TabStop = false;
            // 
            // ScreenTrackBar
            // 
            ScreenTrackBar.BackColor = SystemColors.ControlLightLight;
            ScreenTrackBar.LargeChange = 1;
            ScreenTrackBar.Location = new Point(1, 3);
            ScreenTrackBar.Maximum = 1;
            ScreenTrackBar.Name = "ScreenTrackBar";
            ScreenTrackBar.Size = new Size(123, 45);
            ScreenTrackBar.TabIndex = 38;
            ScreenTrackBar.Value = 1;
            ScreenTrackBar.Scroll += ScreenTrackBar_Scroll;
            // 
            // ViewPage
            // 
            ViewPage.Controls.Add(OriginIcon);
            ViewPage.Controls.Add(InGameScreenshot);
            ViewPage.Controls.Add(Typing1);
            ViewPage.Controls.Add(Typing2);
            ViewPage.Controls.Add(View);
            ViewPage.Controls.Add(ViewBox);
            ViewPage.Controls.Add(PokeSprite);
            ViewPage.Controls.Add(Specialty);
            ViewPage.Controls.Add(UniqueBox2);
            ViewPage.Controls.Add(HidePIDEC);
            ViewPage.Controls.Add(UniqueBox);
            ViewPage.Controls.Add(LiveStats);
            ViewPage.Controls.Add(HpLabel);
            ViewPage.Controls.Add(RefreshStats);
            ViewPage.Controls.Add(WindowCapture);
            ViewPage.Controls.Add(RefreshBox);
            ViewPage.Controls.Add(TodaysDate);
            ViewPage.Controls.Add(Refresh);
            ViewPage.Location = new Point(4, 24);
            ViewPage.Name = "ViewPage";
            ViewPage.Padding = new Padding(3);
            ViewPage.Size = new Size(503, 482);
            ViewPage.TabIndex = 1;
            ViewPage.Text = "View 🔎";
            ViewPage.UseVisualStyleBackColor = true;
            // 
            // BoxPage
            // 
            BoxPage.Location = new Point(4, 24);
            BoxPage.Name = "BoxPage";
            BoxPage.Padding = new Padding(3);
            BoxPage.Size = new Size(503, 482);
            BoxPage.TabIndex = 2;
            BoxPage.Text = "Box 📦";
            BoxPage.UseVisualStyleBackColor = true;
            // 
            // PartyPage
            // 
            PartyPage.Location = new Point(4, 24);
            PartyPage.Name = "PartyPage";
            PartyPage.Size = new Size(503, 482);
            PartyPage.TabIndex = 8;
            PartyPage.Text = "Party 👨‍👩‍👦‍👦";
            PartyPage.UseVisualStyleBackColor = true;
            // 
            // EggPage
            // 
            EggPage.Location = new Point(4, 24);
            EggPage.Name = "EggPage";
            EggPage.Padding = new Padding(3);
            EggPage.Size = new Size(503, 482);
            EggPage.TabIndex = 3;
            EggPage.Text = "Egg \U0001f95a";
            EggPage.UseVisualStyleBackColor = true;
            // 
            // NPCPage
            // 
            NPCPage.Location = new Point(4, 24);
            NPCPage.Name = "NPCPage";
            NPCPage.Padding = new Padding(3);
            NPCPage.Size = new Size(503, 482);
            NPCPage.TabIndex = 5;
            NPCPage.Text = "NPC 🤖";
            NPCPage.UseVisualStyleBackColor = true;
            // 
            // InGameScreenshotPage
            // 
            InGameScreenshotPage.BorderStyle = BorderStyle.FixedSingle;
            InGameScreenshotPage.Location = new Point(4, 24);
            InGameScreenshotPage.Name = "InGameScreenshotPage";
            InGameScreenshotPage.Size = new Size(503, 482);
            InGameScreenshotPage.TabIndex = 7;
            InGameScreenshotPage.Text = "Screenshot 📷";
            InGameScreenshotPage.UseVisualStyleBackColor = true;
            // 
            // MiscPage
            // 
            MiscPage.Location = new Point(4, 24);
            MiscPage.Name = "MiscPage";
            MiscPage.Size = new Size(503, 482);
            MiscPage.TabIndex = 9;
            MiscPage.Text = "Misc 📓";
            MiscPage.UseVisualStyleBackColor = true;
            // 
            // MainViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(495, 468);
            Controls.Add(ViewerControl);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainViewer";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PokeViewer.NET";
            Load += PokeViewerForm_Load;
            ((System.ComponentModel.ISupportInitialize)PokeSprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)Specialty).EndInit();
            ((System.ComponentModel.ISupportInitialize)OriginIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)Typing1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Typing2).EndInit();
            ViewerControl.ResumeLayout(false);
            ConnectionPage.ResumeLayout(false);
            ConnectionPage.PerformLayout();
            MoodGroup.ResumeLayout(false);
            MoodGroup.PerformLayout();
            TrainerPassportGroup.ResumeLayout(false);
            TrainerPassportGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TrainerPhoto).EndInit();
            ExtrasGroup.ResumeLayout(false);
            ExtrasGroup.PerformLayout();
            ConnectionGroupBox.ResumeLayout(false);
            ConnectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ConnectionSpriteBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScreenTrackBar).EndInit();
            ViewPage.ResumeLayout(false);
            ViewPage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button Connect;
        private TextBox SwitchIP;
        private Button View;
        private TextBox ViewBox;
        private PictureBox PokeSprite;
        private PictureBox Specialty;
        private PictureBox OriginIcon;
        private PictureBox Typing1;
        private CheckBox HidePIDEC;
        private PictureBox Typing2;
        private TextBox LiveStats;
        private CheckBox RefreshStats;
        private TextBox RefreshBox;
        private new Label Refresh;
        private Label TodaysDate;
        private Button WindowCapture;
        private Label HpLabel;
        private CheckBox UniqueBox;
        private CheckBox UniqueBox2;
        private Button InGameScreenshot;
        private CheckBox ToggleSwitchProtocol;
        private TabControl ViewerControl;
        private TabPage ConnectionPage;
        private TabPage ViewPage;
        private TabPage BoxPage;
        private TabPage EggPage;
        private TabPage NPCPage;
        private GroupBox ConnectionGroupBox;
        private PictureBox ConnectionSpriteBox;
        private Label VersionLabel;
        private TabPage InGameScreenshotPage;
        private TabPage PartyPage;
        private TabPage MiscPage;
        private GroupBox ExtrasGroup;
        private Button SaveButton;
        private TextBox WebhookURLText;
        private Label DiscordID;
        private Label WebHookURL;
        private TextBox DiscordIDText;
        private Button RaidButton;
        private Button WideButton;
        private Label TrainerLabel;
        private GroupBox TrainerPassportGroup;
        private Label TIDSID;
        private PictureBox TrainerPhoto;
        private TrackBar ScreenTrackBar;
        private Label ScreenLabel;
        private Label ScreenOnLabel;
        private Label ScreenOffLabel;
        private GroupBox MoodGroup;
        private Button SetMoodButton;
        private RadioButton DefaultMoodRadio;
        private RadioButton AltMoodRadio;
        private ComboBox AltForeCombo;
        private ComboBox AltBackCombo;
        private Label BackColorLabel;
        private Label ForeColorLabel;
        private Button EventRedeemButton;
        private Button TurboButton;
        private Label PingLabel;
        private TextBox MessageText;
    }
}