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
            this.Connect = new System.Windows.Forms.Button();
            this.SwitchIP = new System.Windows.Forms.TextBox();
            this.View = new System.Windows.Forms.Button();
            this.ViewBox = new System.Windows.Forms.TextBox();
            this.PokeSprite = new System.Windows.Forms.PictureBox();
            this.Specialty = new System.Windows.Forms.PictureBox();
            this.OriginIcon = new System.Windows.Forms.PictureBox();
            this.ScreenShot = new System.Windows.Forms.CheckBox();
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
            this.WideView = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PokeSprite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Specialty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing2)).BeginInit();
            this.SuspendLayout();
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(249, 8);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(89, 23);
            this.Connect.TabIndex = 0;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // SwitchIP
            // 
            this.SwitchIP.Location = new System.Drawing.Point(12, 8);
            this.SwitchIP.Name = "SwitchIP";
            this.SwitchIP.Size = new System.Drawing.Size(125, 23);
            this.SwitchIP.TabIndex = 2;
            this.SwitchIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SwitchIP.TextChanged += new System.EventHandler(this.CheckForIP);
            // 
            // View
            // 
            this.View.Enabled = false;
            this.View.Location = new System.Drawing.Point(163, 233);
            this.View.Name = "View";
            this.View.Size = new System.Drawing.Size(47, 23);
            this.View.TabIndex = 3;
            this.View.Text = "View";
            this.View.UseVisualStyleBackColor = true;
            this.View.Visible = false;
            this.View.Click += new System.EventHandler(this.View_Click);
            // 
            // ViewBox
            // 
            this.ViewBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ViewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ViewBox.Location = new System.Drawing.Point(12, 72);
            this.ViewBox.Multiline = true;
            this.ViewBox.Name = "ViewBox";
            this.ViewBox.ReadOnly = true;
            this.ViewBox.Size = new System.Drawing.Size(161, 142);
            this.ViewBox.TabIndex = 4;
            this.ViewBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ViewBox.Visible = false;
            // 
            // PokeSprite
            // 
            this.PokeSprite.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.PokeSprite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PokeSprite.Location = new System.Drawing.Point(199, 72);
            this.PokeSprite.Name = "PokeSprite";
            this.PokeSprite.Size = new System.Drawing.Size(161, 142);
            this.PokeSprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PokeSprite.TabIndex = 7;
            this.PokeSprite.TabStop = false;
            // 
            // Specialty
            // 
            this.Specialty.Location = new System.Drawing.Point(163, 260);
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
            this.OriginIcon.Location = new System.Drawing.Point(154, 3);
            this.OriginIcon.Name = "OriginIcon";
            this.OriginIcon.Size = new System.Drawing.Size(64, 64);
            this.OriginIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OriginIcon.TabIndex = 10;
            this.OriginIcon.TabStop = false;
            // 
            // ScreenShot
            // 
            this.ScreenShot.AutoSize = true;
            this.ScreenShot.Location = new System.Drawing.Point(12, 51);
            this.ScreenShot.Name = "ScreenShot";
            this.ScreenShot.Size = new System.Drawing.Size(85, 19);
            this.ScreenShot.TabIndex = 11;
            this.ScreenShot.Text = "ScreenShot";
            this.ScreenShot.UseVisualStyleBackColor = true;
            this.ScreenShot.Visible = false;
            // 
            // Typing1
            // 
            this.Typing1.Location = new System.Drawing.Point(216, 234);
            this.Typing1.Name = "Typing1";
            this.Typing1.Size = new System.Drawing.Size(144, 27);
            this.Typing1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Typing1.TabIndex = 12;
            this.Typing1.TabStop = false;
            // 
            // HidePIDEC
            // 
            this.HidePIDEC.AutoSize = true;
            this.HidePIDEC.Location = new System.Drawing.Point(12, 33);
            this.HidePIDEC.Name = "HidePIDEC";
            this.HidePIDEC.Size = new System.Drawing.Size(91, 19);
            this.HidePIDEC.TabIndex = 13;
            this.HidePIDEC.Text = "Hide PID/EC";
            this.HidePIDEC.UseVisualStyleBackColor = true;
            this.HidePIDEC.Visible = false;
            this.HidePIDEC.CheckedChanged += new System.EventHandler(this.CheckForHide);
            // 
            // Typing2
            // 
            this.Typing2.Location = new System.Drawing.Point(217, 267);
            this.Typing2.Name = "Typing2";
            this.Typing2.Size = new System.Drawing.Size(143, 26);
            this.Typing2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Typing2.TabIndex = 14;
            this.Typing2.TabStop = false;
            // 
            // LiveStats
            // 
            this.LiveStats.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.LiveStats.Location = new System.Drawing.Point(12, 234);
            this.LiveStats.Multiline = true;
            this.LiveStats.Name = "LiveStats";
            this.LiveStats.ReadOnly = true;
            this.LiveStats.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LiveStats.Size = new System.Drawing.Size(145, 67);
            this.LiveStats.TabIndex = 15;
            this.LiveStats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RefreshStats
            // 
            this.RefreshStats.AutoSize = true;
            this.RefreshStats.Location = new System.Drawing.Point(12, 215);
            this.RefreshStats.Name = "RefreshStats";
            this.RefreshStats.Size = new System.Drawing.Size(72, 19);
            this.RefreshStats.TabIndex = 16;
            this.RefreshStats.Text = "LiveStats";
            this.RefreshStats.UseVisualStyleBackColor = true;
            this.RefreshStats.CheckedChanged += new System.EventHandler(this.RefreshStats_CheckedChanged);
            // 
            // RefreshBox
            // 
            this.RefreshBox.Location = new System.Drawing.Point(95, 303);
            this.RefreshBox.Name = "RefreshBox";
            this.RefreshBox.Size = new System.Drawing.Size(45, 23);
            this.RefreshBox.TabIndex = 17;
            this.RefreshBox.Text = "2000";
            this.RefreshBox.Visible = false;
            // 
            // Refresh
            // 
            this.Refresh.AutoSize = true;
            this.Refresh.Location = new System.Drawing.Point(20, 306);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(75, 15);
            this.Refresh.TabIndex = 18;
            this.Refresh.Text = "Refresh Rate:";
            this.Refresh.Visible = false;
            // 
            // TodaysDate
            // 
            this.TodaysDate.AutoSize = true;
            this.TodaysDate.Location = new System.Drawing.Point(225, 305);
            this.TodaysDate.Name = "TodaysDate";
            this.TodaysDate.Size = new System.Drawing.Size(57, 15);
            this.TodaysDate.TabIndex = 19;
            this.TodaysDate.Text = "DateTime";
            // 
            // WindowCapture
            // 
            this.WindowCapture.Location = new System.Drawing.Point(219, 36);
            this.WindowCapture.Name = "WindowCapture";
            this.WindowCapture.Size = new System.Drawing.Size(75, 23);
            this.WindowCapture.TabIndex = 20;
            this.WindowCapture.Text = "Capture";
            this.WindowCapture.UseVisualStyleBackColor = true;
            this.WindowCapture.Click += new System.EventHandler(this.CaptureWindow_Click);
            // 
            // HpLabel
            // 
            this.HpLabel.AutoSize = true;
            this.HpLabel.Location = new System.Drawing.Point(99, 216);
            this.HpLabel.Name = "HpLabel";
            this.HpLabel.Size = new System.Drawing.Size(63, 15);
            this.HpLabel.TabIndex = 21;
            this.HpLabel.Text = "          HP%";
            // 
            // UniqueBox
            // 
            this.UniqueBox.AutoSize = true;
            this.UniqueBox.Location = new System.Drawing.Point(217, 215);
            this.UniqueBox.Name = "UniqueBox";
            this.UniqueBox.Size = new System.Drawing.Size(84, 19);
            this.UniqueBox.TabIndex = 22;
            this.UniqueBox.Text = "UniqueBox";
            this.UniqueBox.UseVisualStyleBackColor = true;
            // 
            // UniqueBox2
            // 
            this.UniqueBox2.AutoSize = true;
            this.UniqueBox2.Location = new System.Drawing.Point(291, 215);
            this.UniqueBox2.Name = "UniqueBox2";
            this.UniqueBox2.Size = new System.Drawing.Size(90, 19);
            this.UniqueBox2.TabIndex = 23;
            this.UniqueBox2.Text = "UniqueBox2";
            this.UniqueBox2.UseVisualStyleBackColor = true;
            // 
            // WideView
            // 
            this.WideView.Enabled = false;
            this.WideView.Location = new System.Drawing.Point(294, 36);
            this.WideView.Name = "WideView";
            this.WideView.Size = new System.Drawing.Size(75, 23);
            this.WideView.TabIndex = 24;
            this.WideView.Text = "WideView";
            this.WideView.UseVisualStyleBackColor = true;
            this.WideView.Click += new System.EventHandler(this.WideView_Click);
            // 
            // MainViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(372, 330);
            this.Controls.Add(this.WideView);
            this.Controls.Add(this.UniqueBox2);
            this.Controls.Add(this.UniqueBox);
            this.Controls.Add(this.HpLabel);
            this.Controls.Add(this.WindowCapture);
            this.Controls.Add(this.TodaysDate);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.RefreshBox);
            this.Controls.Add(this.RefreshStats);
            this.Controls.Add(this.LiveStats);
            this.Controls.Add(this.HidePIDEC);
            this.Controls.Add(this.ScreenShot);
            this.Controls.Add(this.OriginIcon);
            this.Controls.Add(this.Specialty);
            this.Controls.Add(this.PokeSprite);
            this.Controls.Add(this.ViewBox);
            this.Controls.Add(this.View);
            this.Controls.Add(this.SwitchIP);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.Typing2);
            this.Controls.Add(this.Typing1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainViewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PokeViewer.NET";
            this.Load += new System.EventHandler(this.PokeViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PokeSprite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Specialty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Typing2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button Connect;
        private TextBox SwitchIP;
        private Button View;
        private TextBox ViewBox;
        private PictureBox PokeSprite;
        private PictureBox Specialty;
        private PictureBox OriginIcon;
        private CheckBox ScreenShot;
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
        private Button WideView;
    }
}