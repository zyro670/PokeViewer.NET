using System.Windows.Forms;

namespace PokeViewer.NET.SubForms
{
    partial class MiscViewSV
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
            MiscGroup = new GroupBox();
            SnackGroup = new GroupBox();
            SnackworthBtn = new Button();
            TimeGroup = new GroupBox();
            SetSwitchTimeButton = new Button();
            CurrentTimeLabel = new Label();
            FwdButton = new Button();
            TimeNumeric = new NumericUpDown();
            BwdButton = new Button();
            ResetButton = new Button();
            OutbreakGroup = new GroupBox();
            OutbreakBtn = new Button();
            WildSpawnGroup = new GroupBox();
            WildDisable = new RadioButton();
            WildSpawnBtn = new Button();
            WildEnable = new RadioButton();
            SeedToPokemonGroup = new GroupBox();
            RaidDifficultyLabel = new Label();
            CaptureRaidButton = new Button();
            TeraIcon = new PictureBox();
            RaidIcon = new PictureBox();
            Results = new TextBox();
            RaidNumeric = new NumericUpDown();
            GoButton = new Button();
            SeedLabel = new Label();
            RaidSensCheck = new CheckBox();
            groupBox2 = new GroupBox();
            FCETextBox = new TextBox();
            ClearFCE = new Button();
            AutoPaste = new Button();
            EnterButton = new Button();
            ReadValues = new Button();
            VivForm = new Label();
            ChangeFormButton = new Button();
            V_ComboBox = new ComboBox();
            button1 = new Button();
            HoursLabel = new Label();
            MiscGroup.SuspendLayout();
            SnackGroup.SuspendLayout();
            TimeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TimeNumeric).BeginInit();
            OutbreakGroup.SuspendLayout();
            WildSpawnGroup.SuspendLayout();
            SeedToPokemonGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TeraIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RaidIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RaidNumeric).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // MiscGroup
            // 
            MiscGroup.Controls.Add(SnackGroup);
            MiscGroup.Controls.Add(TimeGroup);
            MiscGroup.Controls.Add(OutbreakGroup);
            MiscGroup.Controls.Add(WildSpawnGroup);
            MiscGroup.Controls.Add(SeedToPokemonGroup);
            MiscGroup.Controls.Add(groupBox2);
            MiscGroup.Location = new Point(15, 3);
            MiscGroup.Name = "MiscGroup";
            MiscGroup.Size = new Size(468, 435);
            MiscGroup.TabIndex = 18;
            MiscGroup.TabStop = false;
            MiscGroup.Text = "Misc Tools";
            // 
            // SnackGroup
            // 
            SnackGroup.Controls.Add(SnackworthBtn);
            SnackGroup.Location = new Point(227, 256);
            SnackGroup.Name = "SnackGroup";
            SnackGroup.Size = new Size(233, 74);
            SnackGroup.TabIndex = 88;
            SnackGroup.TabStop = false;
            SnackGroup.Text = "One Per Save";
            // 
            // SnackworthBtn
            // 
            SnackworthBtn.Location = new Point(55, 27);
            SnackworthBtn.Name = "SnackworthBtn";
            SnackworthBtn.Size = new Size(123, 30);
            SnackworthBtn.TabIndex = 82;
            SnackworthBtn.Text = "Snackworth";
            SnackworthBtn.UseVisualStyleBackColor = true;
            SnackworthBtn.Click += SnackworthBtn_Click;
            // 
            // TimeGroup
            // 
            TimeGroup.Controls.Add(HoursLabel);
            TimeGroup.Controls.Add(SetSwitchTimeButton);
            TimeGroup.Controls.Add(CurrentTimeLabel);
            TimeGroup.Controls.Add(FwdButton);
            TimeGroup.Controls.Add(TimeNumeric);
            TimeGroup.Controls.Add(BwdButton);
            TimeGroup.Controls.Add(ResetButton);
            TimeGroup.Location = new Point(8, 336);
            TimeGroup.Name = "TimeGroup";
            TimeGroup.Size = new Size(452, 93);
            TimeGroup.TabIndex = 74;
            TimeGroup.TabStop = false;
            TimeGroup.Text = "Time Conditions";
            // 
            // SetSwitchTimeButton
            // 
            SetSwitchTimeButton.Location = new Point(219, 37);
            SetSwitchTimeButton.Name = "SetSwitchTimeButton";
            SetSwitchTimeButton.Size = new Size(208, 44);
            SetSwitchTimeButton.TabIndex = 77;
            SetSwitchTimeButton.Text = "Set Switch System Time To Match Current Local Time";
            SetSwitchTimeButton.UseVisualStyleBackColor = true;
            SetSwitchTimeButton.Click += button2_Click_1;
            // 
            // CurrentTimeLabel
            // 
            CurrentTimeLabel.AutoSize = true;
            CurrentTimeLabel.Location = new Point(219, 19);
            CurrentTimeLabel.Name = "CurrentTimeLabel";
            CurrentTimeLabel.Size = new Size(110, 15);
            CurrentTimeLabel.TabIndex = 76;
            CurrentTimeLabel.Text = "Current Local Time:";
            // 
            // FwdButton
            // 
            FwdButton.Location = new Point(28, 29);
            FwdButton.Name = "FwdButton";
            FwdButton.Size = new Size(70, 23);
            FwdButton.TabIndex = 71;
            FwdButton.Text = "Forward";
            FwdButton.UseVisualStyleBackColor = true;
            FwdButton.Click += FwdButton_Click;
            // 
            // TimeNumeric
            // 
            TimeNumeric.Location = new Point(104, 31);
            TimeNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            TimeNumeric.Name = "TimeNumeric";
            TimeNumeric.Size = new Size(52, 23);
            TimeNumeric.TabIndex = 73;
            TimeNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // BwdButton
            // 
            BwdButton.Location = new Point(28, 58);
            BwdButton.Name = "BwdButton";
            BwdButton.Size = new Size(70, 23);
            BwdButton.TabIndex = 70;
            BwdButton.Text = "Backward";
            BwdButton.UseVisualStyleBackColor = true;
            BwdButton.Click += BwdButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.Location = new Point(104, 58);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(69, 23);
            ResetButton.TabIndex = 72;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // OutbreakGroup
            // 
            OutbreakGroup.Controls.Add(OutbreakBtn);
            OutbreakGroup.Location = new Point(8, 22);
            OutbreakGroup.Name = "OutbreakGroup";
            OutbreakGroup.Size = new Size(200, 85);
            OutbreakGroup.TabIndex = 87;
            OutbreakGroup.TabStop = false;
            OutbreakGroup.Text = "Outbreaks";
            // 
            // OutbreakBtn
            // 
            OutbreakBtn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OutbreakBtn.Location = new Point(49, 31);
            OutbreakBtn.Name = "OutbreakBtn";
            OutbreakBtn.Size = new Size(123, 30);
            OutbreakBtn.TabIndex = 81;
            OutbreakBtn.Text = "View";
            OutbreakBtn.UseVisualStyleBackColor = true;
            OutbreakBtn.Click += button2_Click;
            // 
            // WildSpawnGroup
            // 
            WildSpawnGroup.Controls.Add(WildDisable);
            WildSpawnGroup.Controls.Add(WildSpawnBtn);
            WildSpawnGroup.Controls.Add(WildEnable);
            WildSpawnGroup.Location = new Point(8, 256);
            WildSpawnGroup.Name = "WildSpawnGroup";
            WildSpawnGroup.Size = new Size(200, 74);
            WildSpawnGroup.TabIndex = 86;
            WildSpawnGroup.TabStop = false;
            WildSpawnGroup.Text = "Wild Spawns";
            // 
            // WildDisable
            // 
            WildDisable.AutoSize = true;
            WildDisable.Location = new Point(115, 19);
            WildDisable.Name = "WildDisable";
            WildDisable.Size = new Size(63, 19);
            WildDisable.TabIndex = 85;
            WildDisable.Text = "Disable";
            WildDisable.UseVisualStyleBackColor = true;
            // 
            // WildSpawnBtn
            // 
            WildSpawnBtn.Location = new Point(63, 44);
            WildSpawnBtn.Name = "WildSpawnBtn";
            WildSpawnBtn.Size = new Size(93, 23);
            WildSpawnBtn.TabIndex = 83;
            WildSpawnBtn.Text = "Modify";
            WildSpawnBtn.UseVisualStyleBackColor = true;
            WildSpawnBtn.Click += WildSpawnBtn_ClickAsync;
            // 
            // WildEnable
            // 
            WildEnable.AutoSize = true;
            WildEnable.Checked = true;
            WildEnable.Location = new Point(49, 19);
            WildEnable.Name = "WildEnable";
            WildEnable.Size = new Size(60, 19);
            WildEnable.TabIndex = 84;
            WildEnable.TabStop = true;
            WildEnable.Text = "Enable";
            WildEnable.UseVisualStyleBackColor = true;
            // 
            // SeedToPokemonGroup
            // 
            SeedToPokemonGroup.Controls.Add(RaidDifficultyLabel);
            SeedToPokemonGroup.Controls.Add(CaptureRaidButton);
            SeedToPokemonGroup.Controls.Add(TeraIcon);
            SeedToPokemonGroup.Controls.Add(RaidIcon);
            SeedToPokemonGroup.Controls.Add(Results);
            SeedToPokemonGroup.Controls.Add(RaidNumeric);
            SeedToPokemonGroup.Controls.Add(GoButton);
            SeedToPokemonGroup.Controls.Add(SeedLabel);
            SeedToPokemonGroup.Controls.Add(RaidSensCheck);
            SeedToPokemonGroup.Location = new Point(227, 22);
            SeedToPokemonGroup.Name = "SeedToPokemonGroup";
            SeedToPokemonGroup.Size = new Size(233, 229);
            SeedToPokemonGroup.TabIndex = 79;
            SeedToPokemonGroup.TabStop = false;
            SeedToPokemonGroup.Text = "Raid Viewer";
            // 
            // RaidDifficultyLabel
            // 
            RaidDifficultyLabel.AutoSize = true;
            RaidDifficultyLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            RaidDifficultyLabel.Location = new Point(178, 73);
            RaidDifficultyLabel.Name = "RaidDifficultyLabel";
            RaidDifficultyLabel.Size = new Size(0, 21);
            RaidDifficultyLabel.TabIndex = 14;
            // 
            // CaptureRaidButton
            // 
            CaptureRaidButton.Location = new Point(160, 43);
            CaptureRaidButton.Name = "CaptureRaidButton";
            CaptureRaidButton.Size = new Size(67, 25);
            CaptureRaidButton.TabIndex = 13;
            CaptureRaidButton.Text = "Capture";
            CaptureRaidButton.UseVisualStyleBackColor = true;
            CaptureRaidButton.Click += Screenshot_Click;
            // 
            // TeraIcon
            // 
            TeraIcon.BackColor = SystemColors.ControlLightLight;
            TeraIcon.Location = new Point(166, 97);
            TeraIcon.Name = "TeraIcon";
            TeraIcon.Size = new Size(57, 53);
            TeraIcon.SizeMode = PictureBoxSizeMode.Zoom;
            TeraIcon.TabIndex = 11;
            TeraIcon.TabStop = false;
            // 
            // RaidIcon
            // 
            RaidIcon.BackColor = SystemColors.ControlLightLight;
            RaidIcon.BackgroundImageLayout = ImageLayout.None;
            RaidIcon.Location = new Point(160, 155);
            RaidIcon.Name = "RaidIcon";
            RaidIcon.Size = new Size(69, 66);
            RaidIcon.SizeMode = PictureBoxSizeMode.Zoom;
            RaidIcon.TabIndex = 10;
            RaidIcon.TabStop = false;
            // 
            // Results
            // 
            Results.BackColor = SystemColors.ControlLightLight;
            Results.Location = new Point(6, 71);
            Results.Multiline = true;
            Results.Name = "Results";
            Results.ReadOnly = true;
            Results.Size = new Size(148, 151);
            Results.TabIndex = 7;
            Results.TextAlign = HorizontalAlignment.Center;
            // 
            // RaidNumeric
            // 
            RaidNumeric.Location = new Point(50, 17);
            RaidNumeric.Maximum = new decimal(new int[] { 119, 0, 0, 0 });
            RaidNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            RaidNumeric.Name = "RaidNumeric";
            RaidNumeric.Size = new Size(104, 23);
            RaidNumeric.TabIndex = 9;
            RaidNumeric.TextAlign = HorizontalAlignment.Right;
            RaidNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            RaidNumeric.ValueChanged += NumericValue_Changed;
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
            // RaidSensCheck
            // 
            RaidSensCheck.AutoSize = true;
            RaidSensCheck.Location = new Point(6, 46);
            RaidSensCheck.Name = "RaidSensCheck";
            RaidSensCheck.Size = new Size(129, 19);
            RaidSensCheck.TabIndex = 12;
            RaidSensCheck.Text = "Hide Sensitive Info?";
            RaidSensCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(FCETextBox);
            groupBox2.Controls.Add(ClearFCE);
            groupBox2.Controls.Add(AutoPaste);
            groupBox2.Controls.Add(EnterButton);
            groupBox2.Location = new Point(8, 122);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(200, 122);
            groupBox2.TabIndex = 80;
            groupBox2.TabStop = false;
            groupBox2.Text = "Fast Code Entry";
            // 
            // FCETextBox
            // 
            FCETextBox.Location = new Point(19, 22);
            FCETextBox.MaxLength = 6;
            FCETextBox.Multiline = true;
            FCETextBox.Name = "FCETextBox";
            FCETextBox.Size = new Size(154, 34);
            FCETextBox.TabIndex = 75;
            // 
            // ClearFCE
            // 
            ClearFCE.BackColor = Color.Transparent;
            ClearFCE.Location = new Point(60, 93);
            ClearFCE.Name = "ClearFCE";
            ClearFCE.Size = new Size(75, 23);
            ClearFCE.TabIndex = 78;
            ClearFCE.Text = "Clear All";
            ClearFCE.UseVisualStyleBackColor = false;
            ClearFCE.Click += ClearFCE_Click;
            // 
            // AutoPaste
            // 
            AutoPaste.BackColor = Color.Transparent;
            AutoPaste.Location = new Point(101, 64);
            AutoPaste.Name = "AutoPaste";
            AutoPaste.Size = new Size(93, 23);
            AutoPaste.TabIndex = 77;
            AutoPaste.Text = "Auto Paste";
            AutoPaste.UseVisualStyleBackColor = false;
            AutoPaste.Click += AutoPaste_Click;
            // 
            // EnterButton
            // 
            EnterButton.BackColor = Color.Transparent;
            EnterButton.Location = new Point(6, 64);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new Size(93, 23);
            EnterButton.TabIndex = 76;
            EnterButton.Text = "Enter";
            EnterButton.UseVisualStyleBackColor = false;
            EnterButton.Click += EnterButton_Click;
            // 
            // ReadValues
            // 
            ReadValues.Location = new Point(3, 0);
            ReadValues.Name = "ReadValues";
            ReadValues.Size = new Size(75, 23);
            ReadValues.TabIndex = 0;
            // 
            // VivForm
            // 
            VivForm.Location = new Point(3, 0);
            VivForm.Name = "VivForm";
            VivForm.Size = new Size(100, 23);
            VivForm.TabIndex = 0;
            // 
            // ChangeFormButton
            // 
            ChangeFormButton.Location = new Point(3, 0);
            ChangeFormButton.Name = "ChangeFormButton";
            ChangeFormButton.Size = new Size(75, 23);
            ChangeFormButton.TabIndex = 0;
            // 
            // V_ComboBox
            // 
            V_ComboBox.Location = new Point(3, 0);
            V_ComboBox.Name = "V_ComboBox";
            V_ComboBox.Size = new Size(121, 23);
            V_ComboBox.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(3, 0);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            // 
            // HoursLabel
            // 
            HoursLabel.AutoSize = true;
            HoursLabel.Location = new Point(156, 39);
            HoursLabel.Name = "HoursLabel";
            HoursLabel.Size = new Size(25, 15);
            HoursLabel.TabIndex = 78;
            HoursLabel.Text = "Hrs";
            // 
            // MiscViewSV
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(496, 451);
            Controls.Add(MiscGroup);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MiscViewSV";
            Text = "MiscView";
            MiscGroup.ResumeLayout(false);
            SnackGroup.ResumeLayout(false);
            TimeGroup.ResumeLayout(false);
            TimeGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TimeNumeric).EndInit();
            OutbreakGroup.ResumeLayout(false);
            WildSpawnGroup.ResumeLayout(false);
            WildSpawnGroup.PerformLayout();
            SeedToPokemonGroup.ResumeLayout(false);
            SeedToPokemonGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TeraIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RaidIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RaidNumeric).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox MiscGroup;
        private Button ChangeFormButton;
        private Label VivForm;
        private Button ReadValues;
        private ComboBox V_ComboBox;
        private Button button1;
        private GroupBox TimeGroup;
        private Button FwdButton;
        private NumericUpDown TimeNumeric;
        private Button BwdButton;
        private Button ResetButton;
        private GroupBox SeedToPokemonGroup;
        private CheckBox RaidSensCheck;
        private PictureBox TeraIcon;
        private PictureBox RaidIcon;
        private TextBox Results;
        private NumericUpDown RaidNumeric;
        private Button GoButton;
        private Label SeedLabel;
        private GroupBox groupBox2;
        private TextBox FCETextBox;
        private Button ClearFCE;
        private Button AutoPaste;
        private Button EnterButton;
        private Button CaptureRaidButton;
        private Label RaidDifficultyLabel;
        private Button OutbreakBtn;
        private Button SnackworthBtn;
        private Button WildSpawnBtn;
        private GroupBox SnackGroup;
        private GroupBox OutbreakGroup;
        private GroupBox WildSpawnGroup;
        private RadioButton WildDisable;
        private RadioButton WildEnable;
        private Button SetSwitchTimeButton;
        private Label CurrentTimeLabel;
        private Label HoursLabel;
    }
}