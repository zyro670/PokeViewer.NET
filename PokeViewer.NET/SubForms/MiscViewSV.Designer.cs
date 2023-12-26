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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiscViewSV));
            OutbreakScan = new Button();
            label9 = new Label();
            OutbreakSearch = new CheckBox();
            OutbreakGroup = new GroupBox();
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
            TimeGroup = new GroupBox();
            FwdButton = new Button();
            TimeNumeric = new NumericUpDown();
            BwdButton = new Button();
            ResetButton = new Button();
            OpenMapBlueberry = new Button();
            MapGroup = new ComboBox();
            OpenMapKitakami = new Button();
            UptimeLabel = new Label();
            DaySkipTotal = new Label();
            StatusLabel = new Label();
            Apply0To64 = new CheckBox();
            EnableWebhook = new CheckBox();
            HardStopOutbreak = new CheckBox();
            progressBar1 = new ProgressBar();
            OpenMapPaldea = new Button();
            ScanForEventOutbreak = new CheckBox();
            SearchListGroup = new GroupBox();
            FormCombo = new ComboBox();
            OutbreakIcon = new PictureBox();
            ClearList = new Button();
            ViewList = new Button();
            AddSpecies = new Button();
            SpeciesBox = new ComboBox();
            RemoveSpecies = new Button();
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
            OutbreakGroup.SuspendLayout();
            SeedToPokemonGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TeraIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RaidIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RaidNumeric).BeginInit();
            TimeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TimeNumeric).BeginInit();
            SearchListGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)OutbreakIcon).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // OutbreakScan
            // 
            OutbreakScan.BackColor = Color.Transparent;
            OutbreakScan.Location = new Point(6, 122);
            OutbreakScan.Name = "OutbreakScan";
            OutbreakScan.Size = new Size(135, 47);
            OutbreakScan.TabIndex = 6;
            OutbreakScan.Text = "Scan";
            OutbreakScan.UseVisualStyleBackColor = false;
            OutbreakScan.Click += button5_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(2, 50);
            label9.Name = "label9";
            label9.Size = new Size(46, 15);
            label9.TabIndex = 16;
            label9.Text = "Species";
            // 
            // OutbreakSearch
            // 
            OutbreakSearch.AutoSize = true;
            OutbreakSearch.Location = new Point(7, 69);
            OutbreakSearch.Name = "OutbreakSearch";
            OutbreakSearch.Size = new Size(66, 19);
            OutbreakSearch.TabIndex = 17;
            OutbreakSearch.Text = "Search?";
            OutbreakSearch.UseVisualStyleBackColor = true;
            // 
            // OutbreakGroup
            // 
            OutbreakGroup.Controls.Add(SeedToPokemonGroup);
            OutbreakGroup.Controls.Add(TimeGroup);
            OutbreakGroup.Controls.Add(OpenMapBlueberry);
            OutbreakGroup.Controls.Add(MapGroup);
            OutbreakGroup.Controls.Add(OpenMapKitakami);
            OutbreakGroup.Controls.Add(UptimeLabel);
            OutbreakGroup.Controls.Add(DaySkipTotal);
            OutbreakGroup.Controls.Add(StatusLabel);
            OutbreakGroup.Controls.Add(Apply0To64);
            OutbreakGroup.Controls.Add(EnableWebhook);
            OutbreakGroup.Controls.Add(HardStopOutbreak);
            OutbreakGroup.Controls.Add(OutbreakSearch);
            OutbreakGroup.Controls.Add(OutbreakScan);
            OutbreakGroup.Controls.Add(progressBar1);
            OutbreakGroup.Controls.Add(OpenMapPaldea);
            OutbreakGroup.Controls.Add(ScanForEventOutbreak);
            OutbreakGroup.Controls.Add(SearchListGroup);
            OutbreakGroup.Controls.Add(groupBox2);
            OutbreakGroup.Location = new Point(15, 3);
            OutbreakGroup.Name = "OutbreakGroup";
            OutbreakGroup.Size = new Size(468, 432);
            OutbreakGroup.TabIndex = 18;
            OutbreakGroup.TabStop = false;
            OutbreakGroup.Text = "Outbreak Conditions";
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
            SeedToPokemonGroup.Location = new Point(225, 198);
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
            // TimeGroup
            // 
            TimeGroup.Controls.Add(FwdButton);
            TimeGroup.Controls.Add(TimeNumeric);
            TimeGroup.Controls.Add(BwdButton);
            TimeGroup.Controls.Add(ResetButton);
            TimeGroup.Location = new Point(6, 198);
            TimeGroup.Name = "TimeGroup";
            TimeGroup.Size = new Size(200, 100);
            TimeGroup.TabIndex = 74;
            TimeGroup.TabStop = false;
            TimeGroup.Text = "Time Conditions";
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
            TimeNumeric.Size = new Size(69, 23);
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
            // OpenMapBlueberry
            // 
            OpenMapBlueberry.Location = new Point(357, 122);
            OpenMapBlueberry.Name = "OpenMapBlueberry";
            OpenMapBlueberry.Size = new Size(99, 47);
            OpenMapBlueberry.TabIndex = 66;
            OpenMapBlueberry.Text = "Blueberry";
            OpenMapBlueberry.UseVisualStyleBackColor = true;
            OpenMapBlueberry.Click += OpenMapBlueberry_Click;
            // 
            // MapGroup
            // 
            MapGroup.FormattingEnabled = true;
            MapGroup.Items.AddRange(new object[] { "All", "Paldea", "Kitakami", "Blueberry" });
            MapGroup.Location = new Point(7, 95);
            MapGroup.Name = "MapGroup";
            MapGroup.Size = new Size(98, 23);
            MapGroup.TabIndex = 61;
            MapGroup.Text = "All";
            MapGroup.SelectedIndexChanged += SelectedIndex_IsChanged;
            // 
            // OpenMapKitakami
            // 
            OpenMapKitakami.Location = new Point(252, 122);
            OpenMapKitakami.Name = "OpenMapKitakami";
            OpenMapKitakami.Size = new Size(99, 47);
            OpenMapKitakami.TabIndex = 60;
            OpenMapKitakami.Text = "Kitakami";
            OpenMapKitakami.UseVisualStyleBackColor = true;
            OpenMapKitakami.Click += OpenMapKitakami_Click;
            // 
            // UptimeLabel
            // 
            UptimeLabel.AutoSize = true;
            UptimeLabel.Location = new Point(110, 58);
            UptimeLabel.Name = "UptimeLabel";
            UptimeLabel.Size = new Size(49, 15);
            UptimeLabel.TabIndex = 41;
            UptimeLabel.Text = "Uptime:";
            // 
            // DaySkipTotal
            // 
            DaySkipTotal.AutoSize = true;
            DaySkipTotal.Location = new Point(110, 38);
            DaySkipTotal.Name = "DaySkipTotal";
            DaySkipTotal.Size = new Size(60, 15);
            DaySkipTotal.TabIndex = 40;
            DaySkipTotal.Text = "Day Skips:";
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Location = new Point(110, 19);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(42, 15);
            StatusLabel.TabIndex = 39;
            StatusLabel.Text = "Status:";
            // 
            // Apply0To64
            // 
            Apply0To64.AutoSize = true;
            Apply0To64.Location = new Point(7, 19);
            Apply0To64.Name = "Apply0To64";
            Apply0To64.Size = new Size(94, 19);
            Apply0To64.TabIndex = 40;
            Apply0To64.Text = "Apply 0 - 64?";
            Apply0To64.UseVisualStyleBackColor = true;
            // 
            // EnableWebhook
            // 
            EnableWebhook.AutoSize = true;
            EnableWebhook.Location = new Point(7, 44);
            EnableWebhook.Name = "EnableWebhook";
            EnableWebhook.Size = new Size(82, 19);
            EnableWebhook.TabIndex = 37;
            EnableWebhook.Text = "Webhook?";
            EnableWebhook.UseVisualStyleBackColor = true;
            // 
            // HardStopOutbreak
            // 
            HardStopOutbreak.AutoSize = true;
            HardStopOutbreak.Location = new Point(113, 76);
            HardStopOutbreak.Name = "HardStopOutbreak";
            HardStopOutbreak.Size = new Size(81, 19);
            HardStopOutbreak.TabIndex = 18;
            HardStopOutbreak.Text = "HardStop?";
            HardStopOutbreak.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(6, 177);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(450, 15);
            progressBar1.TabIndex = 39;
            // 
            // OpenMapPaldea
            // 
            OpenMapPaldea.BackColor = Color.Transparent;
            OpenMapPaldea.Location = new Point(147, 122);
            OpenMapPaldea.Name = "OpenMapPaldea";
            OpenMapPaldea.Size = new Size(99, 47);
            OpenMapPaldea.TabIndex = 41;
            OpenMapPaldea.Text = "Paldea";
            OpenMapPaldea.UseVisualStyleBackColor = false;
            OpenMapPaldea.Click += OpenMap_Click;
            // 
            // ScanForEventOutbreak
            // 
            ScanForEventOutbreak.AutoSize = true;
            ScanForEventOutbreak.Location = new Point(113, 99);
            ScanForEventOutbreak.Name = "ScanForEventOutbreak";
            ScanForEventOutbreak.Size = new Size(79, 19);
            ScanForEventOutbreak.TabIndex = 67;
            ScanForEventOutbreak.Text = "Event OB?";
            ScanForEventOutbreak.UseVisualStyleBackColor = true;
            // 
            // SearchListGroup
            // 
            SearchListGroup.Controls.Add(FormCombo);
            SearchListGroup.Controls.Add(OutbreakIcon);
            SearchListGroup.Controls.Add(ClearList);
            SearchListGroup.Controls.Add(ViewList);
            SearchListGroup.Controls.Add(AddSpecies);
            SearchListGroup.Controls.Add(SpeciesBox);
            SearchListGroup.Controls.Add(RemoveSpecies);
            SearchListGroup.Controls.Add(label9);
            SearchListGroup.Location = new Point(218, 12);
            SearchListGroup.Name = "SearchListGroup";
            SearchListGroup.Size = new Size(238, 104);
            SearchListGroup.TabIndex = 22;
            SearchListGroup.TabStop = false;
            SearchListGroup.Text = "Search List";
            // 
            // FormCombo
            // 
            FormCombo.FormattingEnabled = true;
            FormCombo.Location = new Point(142, 47);
            FormCombo.Name = "FormCombo";
            FormCombo.Size = new Size(93, 23);
            FormCombo.TabIndex = 50;
            // 
            // OutbreakIcon
            // 
            OutbreakIcon.Image = (Image)resources.GetObject("OutbreakIcon.Image");
            OutbreakIcon.Location = new Point(188, 10);
            OutbreakIcon.Name = "OutbreakIcon";
            OutbreakIcon.Size = new Size(44, 34);
            OutbreakIcon.SizeMode = PictureBoxSizeMode.Zoom;
            OutbreakIcon.TabIndex = 49;
            OutbreakIcon.TabStop = false;
            // 
            // ClearList
            // 
            ClearList.BackColor = Color.Transparent;
            ClearList.Location = new Point(101, 18);
            ClearList.Name = "ClearList";
            ClearList.Size = new Size(74, 23);
            ClearList.TabIndex = 48;
            ClearList.Text = "Clear All";
            ClearList.UseVisualStyleBackColor = false;
            ClearList.Click += ClearList_Click;
            // 
            // ViewList
            // 
            ViewList.BackColor = Color.Transparent;
            ViewList.Location = new Point(7, 18);
            ViewList.Name = "ViewList";
            ViewList.Size = new Size(86, 23);
            ViewList.TabIndex = 42;
            ViewList.Text = "View";
            ViewList.UseVisualStyleBackColor = false;
            ViewList.Click += button1_Click;
            // 
            // AddSpecies
            // 
            AddSpecies.BackColor = Color.Transparent;
            AddSpecies.Location = new Point(7, 75);
            AddSpecies.Name = "AddSpecies";
            AddSpecies.Size = new Size(107, 23);
            AddSpecies.TabIndex = 44;
            AddSpecies.Text = "Add";
            AddSpecies.UseVisualStyleBackColor = false;
            AddSpecies.Click += AddSpecies_Click;
            // 
            // SpeciesBox
            // 
            SpeciesBox.FormattingEnabled = true;
            SpeciesBox.Location = new Point(49, 47);
            SpeciesBox.Name = "SpeciesBox";
            SpeciesBox.Size = new Size(91, 23);
            SpeciesBox.TabIndex = 45;
            SpeciesBox.SelectedIndexChanged += SpeciesBox_IsChanged;
            // 
            // RemoveSpecies
            // 
            RemoveSpecies.BackColor = Color.Transparent;
            RemoveSpecies.Location = new Point(120, 75);
            RemoveSpecies.Name = "RemoveSpecies";
            RemoveSpecies.Size = new Size(112, 23);
            RemoveSpecies.TabIndex = 46;
            RemoveSpecies.Text = "Remove";
            RemoveSpecies.UseVisualStyleBackColor = false;
            RemoveSpecies.Click += RemoveSpecies_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(FCETextBox);
            groupBox2.Controls.Add(ClearFCE);
            groupBox2.Controls.Add(AutoPaste);
            groupBox2.Controls.Add(EnterButton);
            groupBox2.Location = new Point(6, 304);
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
            // MiscViewSV
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(482, 442);
            Controls.Add(OutbreakGroup);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MiscViewSV";
            Text = "MiscView";
            OutbreakGroup.ResumeLayout(false);
            OutbreakGroup.PerformLayout();
            SeedToPokemonGroup.ResumeLayout(false);
            SeedToPokemonGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TeraIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RaidIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RaidNumeric).EndInit();
            TimeGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)TimeNumeric).EndInit();
            SearchListGroup.ResumeLayout(false);
            SearchListGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)OutbreakIcon).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button OutbreakScan;
        private Label label9;
        private CheckBox OutbreakSearch;
        private GroupBox OutbreakGroup;
        private CheckBox HardStopOutbreak;
        private CheckBox EnableWebhook;
        private Button ChangeFormButton;
        private Label VivForm;
        private Button ReadValues;
        private ComboBox V_ComboBox;
        private ProgressBar progressBar1;
        private Label StatusLabel;
        private Label DaySkipTotal;
        private Label UptimeLabel;
        private CheckBox Apply0To64;
        private Button OpenMapPaldea;
        private Button ViewList;
        private Button AddSpecies;
        private ComboBox SpeciesBox;
        private Button RemoveSpecies;
        private Button ClearList;
        private Button OpenMapKitakami;
        private ComboBox MapGroup;
        private Button button1;
        private Button OpenMapBlueberry;
        private CheckBox ScanForEventOutbreak;
        private GroupBox SearchListGroup;
        private PictureBox OutbreakIcon;
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
        private ComboBox FormCombo;
    }
}