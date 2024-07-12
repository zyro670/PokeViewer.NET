namespace PokeViewer.NET.SubForms
{
    partial class DiscordUtilForm
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
            PrefixBox = new ComboBox();
            BotPrefixLabel = new Label();
            BotTokenLabel = new Label();
            BotToken = new TextBox();
            MessageText = new TextBox();
            PingLabel = new Label();
            DiscordIDText = new TextBox();
            WebhookURLText = new TextBox();
            DiscordID = new Label();
            WebHookURL = new Label();
            UseWiFiProtocol = new CheckBox();
            SaveButton = new Button();
            SuspendLayout();
            // 
            // PrefixBox
            // 
            PrefixBox.FormattingEnabled = true;
            PrefixBox.Items.AddRange(new object[] { "$", "!", "%", "^", "&", "*", ".", ",", ";", "-", "_" });
            PrefixBox.Location = new Point(356, 102);
            PrefixBox.Name = "PrefixBox";
            PrefixBox.Size = new Size(35, 23);
            PrefixBox.TabIndex = 63;
            PrefixBox.Text = "$";
            PrefixBox.SelectedIndexChanged += Prefix_CheckedChanged;
            // 
            // BotPrefixLabel
            // 
            BotPrefixLabel.AutoSize = true;
            BotPrefixLabel.Location = new Point(296, 105);
            BotPrefixLabel.Name = "BotPrefixLabel";
            BotPrefixLabel.Size = new Size(58, 15);
            BotPrefixLabel.TabIndex = 62;
            BotPrefixLabel.Text = "Bot Prefix";
            // 
            // BotTokenLabel
            // 
            BotTokenLabel.AutoSize = true;
            BotTokenLabel.Location = new Point(37, 105);
            BotTokenLabel.Name = "BotTokenLabel";
            BotTokenLabel.Size = new Size(59, 15);
            BotTokenLabel.TabIndex = 59;
            BotTokenLabel.Text = "Bot Token";
            // 
            // BotToken
            // 
            BotToken.Location = new Point(104, 102);
            BotToken.Name = "BotToken";
            BotToken.Size = new Size(185, 23);
            BotToken.TabIndex = 60;
            // 
            // MessageText
            // 
            MessageText.Location = new Point(515, 58);
            MessageText.Multiline = true;
            MessageText.Name = "MessageText";
            MessageText.Size = new Size(185, 150);
            MessageText.TabIndex = 61;
            // 
            // PingLabel
            // 
            PingLabel.AutoSize = true;
            PingLabel.Location = new Point(545, 31);
            PingLabel.Name = "PingLabel";
            PingLabel.Size = new Size(123, 15);
            PingLabel.TabIndex = 58;
            PingLabel.Text = "Discord Ping Message";
            // 
            // DiscordIDText
            // 
            DiscordIDText.Location = new Point(103, 131);
            DiscordIDText.Name = "DiscordIDText";
            DiscordIDText.Size = new Size(185, 23);
            DiscordIDText.TabIndex = 57;
            DiscordIDText.TextAlign = HorizontalAlignment.Center;
            // 
            // WebhookURLText
            // 
            WebhookURLText.Location = new Point(104, 73);
            WebhookURLText.Name = "WebhookURLText";
            WebhookURLText.Size = new Size(405, 23);
            WebhookURLText.TabIndex = 56;
            WebhookURLText.TextAlign = HorizontalAlignment.Center;
            // 
            // DiscordID
            // 
            DiscordID.AutoSize = true;
            DiscordID.Location = new Point(36, 134);
            DiscordID.Name = "DiscordID";
            DiscordID.Size = new Size(61, 15);
            DiscordID.TabIndex = 55;
            DiscordID.Text = "Discord ID";
            // 
            // WebHookURL
            // 
            WebHookURL.AutoSize = true;
            WebHookURL.Location = new Point(37, 77);
            WebHookURL.Name = "WebHookURL";
            WebHookURL.Size = new Size(58, 15);
            WebHookURL.TabIndex = 54;
            WebHookURL.Text = "Webhook";
            // 
            // UseWiFiProtocol
            // 
            UseWiFiProtocol.AutoSize = true;
            UseWiFiProtocol.Location = new Point(38, 41);
            UseWiFiProtocol.Name = "UseWiFiProtocol";
            UseWiFiProtocol.Size = new Size(126, 19);
            UseWiFiProtocol.TabIndex = 65;
            UseWiFiProtocol.Text = "Connect with USB?";
            UseWiFiProtocol.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(261, 172);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(184, 36);
            SaveButton.TabIndex = 64;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // DiscordUtilForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(726, 229);
            Controls.Add(UseWiFiProtocol);
            Controls.Add(SaveButton);
            Controls.Add(PrefixBox);
            Controls.Add(BotPrefixLabel);
            Controls.Add(BotTokenLabel);
            Controls.Add(BotToken);
            Controls.Add(MessageText);
            Controls.Add(PingLabel);
            Controls.Add(DiscordIDText);
            Controls.Add(WebhookURLText);
            Controls.Add(DiscordID);
            Controls.Add(WebHookURL);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DiscordUtilForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox PrefixBox;
        private Label BotPrefixLabel;
        private Label BotTokenLabel;
        private TextBox BotToken;
        private TextBox MessageText;
        private Label PingLabel;
        private TextBox DiscordIDText;
        private TextBox WebhookURLText;
        private Label DiscordID;
        private Label WebHookURL;
        private CheckBox UseWiFiProtocol;
        private Button SaveButton;
    }
}