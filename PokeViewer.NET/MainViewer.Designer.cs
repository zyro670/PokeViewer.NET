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
            this.Connect = new System.Windows.Forms.Button();
            this.SwitchIP = new System.Windows.Forms.TextBox();
            this.View = new System.Windows.Forms.Button();
            this.ViewBox = new System.Windows.Forms.TextBox();
            this.PokeSprite = new System.Windows.Forms.PictureBox();
            this.Specialty = new System.Windows.Forms.PictureBox();
            this.OriginIcon = new System.Windows.Forms.PictureBox();
            this.Typing1 = new System.Windows.Forms.PictureBox();
            this.HidePIDEC = new System.Windows.Forms.CheckBox();
            this.Typing2 = new System.Windows.Forms.PictureBox();
            this.LiveStats = new System.Windows.Forms.TextBox();
            this.RefreshStats = new System.Windows.Forms.CheckBox();
            this.RefreshBox = new System.Windows.Forms.TextBox();
            this.Refresh = new System.Windows.Forms.Label();
            this.TodaysDate = new System.Windows.Forms.Label();
            this.WindowCapture = new System.Windows.Forms.Button();
            this.HpLabel = new System.Windows.Forms.Label();
            this.UniqueBox = new System.Windows.Forms.CheckBox();
            this.UniqueBox2 = new System.Windows.Forms.CheckBox();
            this.InGameScreenshot = new System.Windows.Forms.Button();
            this.ToggleSwitchProtocol = new System.Windows.Forms.CheckBox();
            this.ViewerControl = new System.Windows.Forms.TabControl();
            this.ConnectionPage = new System.Windows.Forms.TabPage();
            this.ConnectionGroupBox = new System.Windows.Forms.GroupBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ConnectionSpriteBox = new System.Windows.Forms.PictureBox();
            this.ViewPage = new System.Windows.Forms.TabPage();
            this.BoxPage = new System.Windows.Forms.TabPage();
            this.EggPage = new System.Windows.Forms.TabPage();
            this.WidePage = new System.Windows.Forms.TabPage();
            this.NPCPage = new System.Windows.Forms.TabPage();
            this.RaidPage = new System.Windows.Forms.TabPage();
            this.InGameScreenshotPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.PokeSprite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Specialty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing2)).BeginInit();
            this.ViewerControl.SuspendLayout();
            this.ConnectionPage.SuspendLayout();
            this.ConnectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionSpriteBox)).BeginInit();
            this.ViewPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(37, 57);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 25);
            this.Connect.TabIndex = 0;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // SwitchIP
            // 
            this.SwitchIP.Location = new System.Drawing.Point(37, 26);
            this.SwitchIP.Name = "SwitchIP";
            this.SwitchIP.Size = new System.Drawing.Size(128, 23);
            this.SwitchIP.TabIndex = 2;
            this.SwitchIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SwitchIP.TextChanged += new System.EventHandler(this.CheckForIP);
            // 
            // View
            // 
            this.View.Location = new System.Drawing.Point(219, 264);
            this.View.Name = "View";
            this.View.Size = new System.Drawing.Size(47, 23);
            this.View.TabIndex = 3;
            this.View.Text = "View";
            this.View.UseVisualStyleBackColor = true;
            this.View.Click += new System.EventHandler(this.View_Click);
            // 
            // ViewBox
            // 
            this.ViewBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ViewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ViewBox.Location = new System.Drawing.Point(37, 88);
            this.ViewBox.Multiline = true;
            this.ViewBox.Name = "ViewBox";
            this.ViewBox.ReadOnly = true;
            this.ViewBox.Size = new System.Drawing.Size(182, 169);
            this.ViewBox.TabIndex = 4;
            this.ViewBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PokeSprite
            // 
            this.PokeSprite.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.PokeSprite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PokeSprite.Location = new System.Drawing.Point(263, 88);
            this.PokeSprite.Name = "PokeSprite";
            this.PokeSprite.Size = new System.Drawing.Size(182, 169);
            this.PokeSprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PokeSprite.TabIndex = 7;
            this.PokeSprite.TabStop = false;
            // 
            // Specialty
            // 
            this.Specialty.Location = new System.Drawing.Point(219, 295);
            this.Specialty.Name = "Specialty";
            this.Specialty.Size = new System.Drawing.Size(47, 41);
            this.Specialty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Specialty.TabIndex = 8;
            this.Specialty.TabStop = false;
            // 
            // OriginIcon
            // 
            this.OriginIcon.ImageLocation = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_gene" +
    "ration_00%5Esb.png";
            this.OriginIcon.Location = new System.Drawing.Point(210, 18);
            this.OriginIcon.Name = "OriginIcon";
            this.OriginIcon.Size = new System.Drawing.Size(64, 64);
            this.OriginIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OriginIcon.TabIndex = 10;
            this.OriginIcon.TabStop = false;
            // 
            // Typing1
            // 
            this.Typing1.Location = new System.Drawing.Point(376, 295);
            this.Typing1.Name = "Typing1";
            this.Typing1.Size = new System.Drawing.Size(69, 56);
            this.Typing1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Typing1.TabIndex = 12;
            this.Typing1.TabStop = false;
            // 
            // HidePIDEC
            // 
            this.HidePIDEC.AutoSize = true;
            this.HidePIDEC.Location = new System.Drawing.Point(37, 57);
            this.HidePIDEC.Name = "HidePIDEC";
            this.HidePIDEC.Size = new System.Drawing.Size(91, 19);
            this.HidePIDEC.TabIndex = 13;
            this.HidePIDEC.Text = "Hide PID/EC";
            this.HidePIDEC.UseVisualStyleBackColor = true;
            this.HidePIDEC.CheckedChanged += new System.EventHandler(this.CheckForHide);
            // 
            // Typing2
            // 
            this.Typing2.Location = new System.Drawing.Point(281, 295);
            this.Typing2.Name = "Typing2";
            this.Typing2.Size = new System.Drawing.Size(69, 56);
            this.Typing2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Typing2.TabIndex = 14;
            this.Typing2.TabStop = false;
            // 
            // LiveStats
            // 
            this.LiveStats.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.LiveStats.Location = new System.Drawing.Point(37, 294);
            this.LiveStats.Multiline = true;
            this.LiveStats.Name = "LiveStats";
            this.LiveStats.ReadOnly = true;
            this.LiveStats.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LiveStats.Size = new System.Drawing.Size(171, 98);
            this.LiveStats.TabIndex = 15;
            this.LiveStats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RefreshStats
            // 
            this.RefreshStats.AutoSize = true;
            this.RefreshStats.Location = new System.Drawing.Point(37, 267);
            this.RefreshStats.Name = "RefreshStats";
            this.RefreshStats.Size = new System.Drawing.Size(72, 19);
            this.RefreshStats.TabIndex = 16;
            this.RefreshStats.Text = "LiveStats";
            this.RefreshStats.UseVisualStyleBackColor = true;
            this.RefreshStats.CheckedChanged += new System.EventHandler(this.RefreshStats_CheckedChanged);
            // 
            // RefreshBox
            // 
            this.RefreshBox.Location = new System.Drawing.Point(120, 403);
            this.RefreshBox.Name = "RefreshBox";
            this.RefreshBox.Size = new System.Drawing.Size(71, 23);
            this.RefreshBox.TabIndex = 17;
            this.RefreshBox.Text = "2000";
            this.RefreshBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Refresh
            // 
            this.Refresh.AutoSize = true;
            this.Refresh.Location = new System.Drawing.Point(42, 407);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(75, 15);
            this.Refresh.TabIndex = 18;
            this.Refresh.Text = "Refresh Rate:";
            // 
            // TodaysDate
            // 
            this.TodaysDate.AutoSize = true;
            this.TodaysDate.Location = new System.Drawing.Point(281, 407);
            this.TodaysDate.Name = "TodaysDate";
            this.TodaysDate.Size = new System.Drawing.Size(57, 15);
            this.TodaysDate.TabIndex = 19;
            this.TodaysDate.Text = "DateTime";
            // 
            // WindowCapture
            // 
            this.WindowCapture.Image = ((System.Drawing.Image)(resources.GetObject("WindowCapture.Image")));
            this.WindowCapture.Location = new System.Drawing.Point(167, 57);
            this.WindowCapture.Name = "WindowCapture";
            this.WindowCapture.Size = new System.Drawing.Size(37, 25);
            this.WindowCapture.TabIndex = 20;
            this.WindowCapture.UseVisualStyleBackColor = true;
            this.WindowCapture.Click += new System.EventHandler(this.CaptureWindow_Click);
            // 
            // HpLabel
            // 
            this.HpLabel.AutoSize = true;
            this.HpLabel.Location = new System.Drawing.Point(124, 268);
            this.HpLabel.Name = "HpLabel";
            this.HpLabel.Size = new System.Drawing.Size(63, 15);
            this.HpLabel.TabIndex = 21;
            this.HpLabel.Text = "          HP%";
            // 
            // UniqueBox
            // 
            this.UniqueBox.AutoSize = true;
            this.UniqueBox.Location = new System.Drawing.Point(281, 264);
            this.UniqueBox.Name = "UniqueBox";
            this.UniqueBox.Size = new System.Drawing.Size(15, 14);
            this.UniqueBox.TabIndex = 22;
            this.UniqueBox.UseVisualStyleBackColor = true;
            this.UniqueBox.Visible = false;
            // 
            // UniqueBox2
            // 
            this.UniqueBox2.AutoSize = true;
            this.UniqueBox2.Location = new System.Drawing.Point(355, 264);
            this.UniqueBox2.Name = "UniqueBox2";
            this.UniqueBox2.Size = new System.Drawing.Size(15, 14);
            this.UniqueBox2.TabIndex = 23;
            this.UniqueBox2.UseVisualStyleBackColor = true;
            this.UniqueBox2.Visible = false;
            // 
            // InGameScreenshot
            // 
            this.InGameScreenshot.Image = ((System.Drawing.Image)(resources.GetObject("InGameScreenshot.Image")));
            this.InGameScreenshot.Location = new System.Drawing.Point(280, 57);
            this.InGameScreenshot.Name = "InGameScreenshot";
            this.InGameScreenshot.Size = new System.Drawing.Size(37, 25);
            this.InGameScreenshot.TabIndex = 30;
            this.InGameScreenshot.UseVisualStyleBackColor = true;
            this.InGameScreenshot.Click += new System.EventHandler(this.InGameScreenshot_Click);
            // 
            // ToggleSwitchProtocol
            // 
            this.ToggleSwitchProtocol.AutoSize = true;
            this.ToggleSwitchProtocol.Location = new System.Drawing.Point(118, 61);
            this.ToggleSwitchProtocol.Name = "ToggleSwitchProtocol";
            this.ToggleSwitchProtocol.Size = new System.Drawing.Size(52, 19);
            this.ToggleSwitchProtocol.TabIndex = 32;
            this.ToggleSwitchProtocol.Text = "USB?";
            this.ToggleSwitchProtocol.UseVisualStyleBackColor = true;
            this.ToggleSwitchProtocol.CheckedChanged += new System.EventHandler(this.CheckForUSBChecked);
            // 
            // ViewerControl
            // 
            this.ViewerControl.Controls.Add(this.ConnectionPage);
            this.ViewerControl.Controls.Add(this.ViewPage);
            this.ViewerControl.Controls.Add(this.BoxPage);
            this.ViewerControl.Controls.Add(this.EggPage);
            this.ViewerControl.Controls.Add(this.WidePage);
            this.ViewerControl.Controls.Add(this.NPCPage);
            this.ViewerControl.Controls.Add(this.RaidPage);
            this.ViewerControl.Controls.Add(this.InGameScreenshotPage);
            this.ViewerControl.Location = new System.Drawing.Point(0, 2);
            this.ViewerControl.Name = "ViewerControl";
            this.ViewerControl.SelectedIndex = 0;
            this.ViewerControl.Size = new System.Drawing.Size(511, 510);
            this.ViewerControl.TabIndex = 33;
            this.ViewerControl.SelectedIndexChanged += new System.EventHandler(this.ViewerControl_SelectedIndexChanged);
            // 
            // ConnectionPage
            // 
            this.ConnectionPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ConnectionPage.Controls.Add(this.ConnectionGroupBox);
            this.ConnectionPage.Location = new System.Drawing.Point(4, 24);
            this.ConnectionPage.Name = "ConnectionPage";
            this.ConnectionPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionPage.Size = new System.Drawing.Size(503, 482);
            this.ConnectionPage.TabIndex = 0;
            this.ConnectionPage.Text = "Connection 🔌";
            // 
            // ConnectionGroupBox
            // 
            this.ConnectionGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ConnectionGroupBox.Controls.Add(this.VersionLabel);
            this.ConnectionGroupBox.Controls.Add(this.ConnectionSpriteBox);
            this.ConnectionGroupBox.Controls.Add(this.Connect);
            this.ConnectionGroupBox.Controls.Add(this.ToggleSwitchProtocol);
            this.ConnectionGroupBox.Controls.Add(this.SwitchIP);
            this.ConnectionGroupBox.Location = new System.Drawing.Point(40, 21);
            this.ConnectionGroupBox.Name = "ConnectionGroupBox";
            this.ConnectionGroupBox.Size = new System.Drawing.Size(200, 173);
            this.ConnectionGroupBox.TabIndex = 34;
            this.ConnectionGroupBox.TabStop = false;
            this.ConnectionGroupBox.Text = "Switch IP";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(159, 10);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(37, 15);
            this.VersionLabel.TabIndex = 36;
            this.VersionLabel.Text = "v1.0.0";
            // 
            // ConnectionSpriteBox
            // 
            this.ConnectionSpriteBox.Location = new System.Drawing.Point(70, 90);
            this.ConnectionSpriteBox.Name = "ConnectionSpriteBox";
            this.ConnectionSpriteBox.Size = new System.Drawing.Size(64, 64);
            this.ConnectionSpriteBox.TabIndex = 35;
            this.ConnectionSpriteBox.TabStop = false;
            // 
            // ViewPage
            // 
            this.ViewPage.Controls.Add(this.OriginIcon);
            this.ViewPage.Controls.Add(this.InGameScreenshot);
            this.ViewPage.Controls.Add(this.Typing1);
            this.ViewPage.Controls.Add(this.Typing2);
            this.ViewPage.Controls.Add(this.View);
            this.ViewPage.Controls.Add(this.ViewBox);
            this.ViewPage.Controls.Add(this.PokeSprite);
            this.ViewPage.Controls.Add(this.Specialty);
            this.ViewPage.Controls.Add(this.UniqueBox2);
            this.ViewPage.Controls.Add(this.HidePIDEC);
            this.ViewPage.Controls.Add(this.UniqueBox);
            this.ViewPage.Controls.Add(this.LiveStats);
            this.ViewPage.Controls.Add(this.HpLabel);
            this.ViewPage.Controls.Add(this.RefreshStats);
            this.ViewPage.Controls.Add(this.WindowCapture);
            this.ViewPage.Controls.Add(this.RefreshBox);
            this.ViewPage.Controls.Add(this.TodaysDate);
            this.ViewPage.Controls.Add(this.Refresh);
            this.ViewPage.Location = new System.Drawing.Point(4, 24);
            this.ViewPage.Name = "ViewPage";
            this.ViewPage.Padding = new System.Windows.Forms.Padding(3);
            this.ViewPage.Size = new System.Drawing.Size(503, 482);
            this.ViewPage.TabIndex = 1;
            this.ViewPage.Text = "View 🔎";
            this.ViewPage.UseVisualStyleBackColor = true;
            // 
            // BoxPage
            // 
            this.BoxPage.Location = new System.Drawing.Point(4, 24);
            this.BoxPage.Name = "BoxPage";
            this.BoxPage.Padding = new System.Windows.Forms.Padding(3);
            this.BoxPage.Size = new System.Drawing.Size(503, 482);
            this.BoxPage.TabIndex = 2;
            this.BoxPage.Text = "Box 📦";
            this.BoxPage.UseVisualStyleBackColor = true;
            // 
            // EggPage
            // 
            this.EggPage.Location = new System.Drawing.Point(4, 24);
            this.EggPage.Name = "EggPage";
            this.EggPage.Padding = new System.Windows.Forms.Padding(3);
            this.EggPage.Size = new System.Drawing.Size(503, 482);
            this.EggPage.TabIndex = 3;
            this.EggPage.Text = "Egg 🥚";
            this.EggPage.UseVisualStyleBackColor = true;
            // 
            // WidePage
            // 
            this.WidePage.Location = new System.Drawing.Point(4, 24);
            this.WidePage.Name = "WidePage";
            this.WidePage.Padding = new System.Windows.Forms.Padding(3);
            this.WidePage.Size = new System.Drawing.Size(503, 482);
            this.WidePage.TabIndex = 4;
            this.WidePage.Text = "Wide 🔭";
            this.WidePage.UseVisualStyleBackColor = true;
            // 
            // NPCPage
            // 
            this.NPCPage.Location = new System.Drawing.Point(4, 24);
            this.NPCPage.Name = "NPCPage";
            this.NPCPage.Padding = new System.Windows.Forms.Padding(3);
            this.NPCPage.Size = new System.Drawing.Size(503, 482);
            this.NPCPage.TabIndex = 5;
            this.NPCPage.Text = "NPC 🤖";
            this.NPCPage.UseVisualStyleBackColor = true;
            // 
            // RaidPage
            // 
            this.RaidPage.Location = new System.Drawing.Point(4, 24);
            this.RaidPage.Name = "RaidPage";
            this.RaidPage.Padding = new System.Windows.Forms.Padding(3);
            this.RaidPage.Size = new System.Drawing.Size(503, 482);
            this.RaidPage.TabIndex = 6;
            this.RaidPage.Text = "Raid 🎉";
            this.RaidPage.UseVisualStyleBackColor = true;
            // 
            // InGameScreenshotPage
            // 
            this.InGameScreenshotPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InGameScreenshotPage.Location = new System.Drawing.Point(4, 24);
            this.InGameScreenshotPage.Name = "InGameScreenshotPage";
            this.InGameScreenshotPage.Size = new System.Drawing.Size(503, 482);
            this.InGameScreenshotPage.TabIndex = 7;
            this.InGameScreenshotPage.Text = "Screenshot 📷";
            this.InGameScreenshotPage.UseVisualStyleBackColor = true;
            // 
            // MainViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(290, 240);
            this.Controls.Add(this.ViewerControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainViewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PokeViewer.NET";
            this.Load += new System.EventHandler(this.PokeViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PokeSprite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Specialty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing2)).EndInit();
            this.ViewerControl.ResumeLayout(false);
            this.ConnectionPage.ResumeLayout(false);
            this.ConnectionGroupBox.ResumeLayout(false);
            this.ConnectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionSpriteBox)).EndInit();
            this.ViewPage.ResumeLayout(false);
            this.ViewPage.PerformLayout();
            this.ResumeLayout(false);

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
        private TabPage WidePage;
        private TabPage NPCPage;
        private TabPage RaidPage;
        private GroupBox ConnectionGroupBox;
        private PictureBox ConnectionSpriteBox;
        private Label VersionLabel;
        private TabPage InGameScreenshotPage;
    }
}