namespace PokeViewer.NET.SubForms
{
    partial class StopConditions
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
            StopConditionsGroup = new GroupBox();
            ScaleBox = new CheckBox();
            ShinyBox = new ComboBox();
            label1 = new Label();
            PresetIVs = new Label();
            PresetIVBox = new ComboBox();
            GenderBox = new ComboBox();
            IgnoreIVFilter = new CheckBox();
            CheckBoxOf3 = new CheckBox();
            TargetSPE = new Label();
            TargetSPD = new Label();
            GenderFilter = new Label();
            TargetSPA = new Label();
            TargetIVs = new Label();
            TargetDEF = new Label();
            TargetHP = new Label();
            TargetATK = new Label();
            HPFilter = new NumericUpDown();
            AtkFilter = new NumericUpDown();
            SpeFilter = new NumericUpDown();
            DefFilter = new NumericUpDown();
            SpdFilter = new NumericUpDown();
            SpaFilter = new NumericUpDown();
            SaveButton = new Button();
            ResetButton = new Button();
            StopConditionsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HPFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AtkFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SpeFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DefFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SpdFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SpaFilter).BeginInit();
            SuspendLayout();
            // 
            // StopConditionsGroup
            // 
            StopConditionsGroup.Controls.Add(ScaleBox);
            StopConditionsGroup.Controls.Add(ShinyBox);
            StopConditionsGroup.Controls.Add(label1);
            StopConditionsGroup.Controls.Add(PresetIVs);
            StopConditionsGroup.Controls.Add(PresetIVBox);
            StopConditionsGroup.Controls.Add(GenderBox);
            StopConditionsGroup.Controls.Add(IgnoreIVFilter);
            StopConditionsGroup.Controls.Add(CheckBoxOf3);
            StopConditionsGroup.Controls.Add(TargetSPE);
            StopConditionsGroup.Controls.Add(TargetSPD);
            StopConditionsGroup.Controls.Add(GenderFilter);
            StopConditionsGroup.Controls.Add(TargetSPA);
            StopConditionsGroup.Controls.Add(TargetIVs);
            StopConditionsGroup.Controls.Add(TargetDEF);
            StopConditionsGroup.Controls.Add(TargetHP);
            StopConditionsGroup.Controls.Add(TargetATK);
            StopConditionsGroup.Controls.Add(HPFilter);
            StopConditionsGroup.Controls.Add(AtkFilter);
            StopConditionsGroup.Controls.Add(SpeFilter);
            StopConditionsGroup.Controls.Add(DefFilter);
            StopConditionsGroup.Controls.Add(SpdFilter);
            StopConditionsGroup.Controls.Add(SpaFilter);
            StopConditionsGroup.Location = new Point(35, 12);
            StopConditionsGroup.Name = "StopConditionsGroup";
            StopConditionsGroup.Size = new Size(346, 244);
            StopConditionsGroup.TabIndex = 63;
            StopConditionsGroup.TabStop = false;
            StopConditionsGroup.Text = "Stop Conditions";
            // 
            // ScaleBox
            // 
            ScaleBox.AutoSize = true;
            ScaleBox.Location = new Point(23, 48);
            ScaleBox.Name = "ScaleBox";
            ScaleBox.Size = new Size(124, 19);
            ScaleBox.TabIndex = 66;
            ScaleBox.Text = "Mini/Jumbo Only?";
            ScaleBox.UseVisualStyleBackColor = true;
            // 
            // ShinyBox
            // 
            ShinyBox.FormattingEnabled = true;
            ShinyBox.Items.AddRange(new object[] { "DisableOption", "NonShiny", "AnyShiny", "StarOnly", "SquareOnly" });
            ShinyBox.Location = new Point(150, 18);
            ShinyBox.MaxDropDownItems = 5;
            ShinyBox.Name = "ShinyBox";
            ShinyBox.Size = new Size(121, 23);
            ShinyBox.TabIndex = 65;
            ShinyBox.Text = "DisableOption";
            ShinyBox.SelectedIndexChanged += ShinyBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(75, 21);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 64;
            label1.Text = "Shiny Target";
            // 
            // PresetIVs
            // 
            PresetIVs.AutoSize = true;
            PresetIVs.Location = new Point(66, 186);
            PresetIVs.Name = "PresetIVs";
            PresetIVs.Size = new Size(60, 15);
            PresetIVs.TabIndex = 63;
            PresetIVs.Text = "Preset IVs:";
            // 
            // PresetIVBox
            // 
            PresetIVBox.FormattingEnabled = true;
            PresetIVBox.Items.AddRange(new object[] { "None Selected", "6 IV", "0 ATK 0 SPE", "0 ATK 5 IV", "0 SPE 5 IV" });
            PresetIVBox.Location = new Point(130, 183);
            PresetIVBox.Name = "PresetIVBox";
            PresetIVBox.Size = new Size(141, 23);
            PresetIVBox.TabIndex = 62;
            PresetIVBox.Text = "None Selected";
            PresetIVBox.SelectedIndexChanged += PresetIVBox_SelectedIndexChanged;
            // 
            // GenderBox
            // 
            GenderBox.FormattingEnabled = true;
            GenderBox.Items.AddRange(new object[] { "Male", "Female", "Genderless", "Any" });
            GenderBox.Location = new Point(219, 47);
            GenderBox.MaxDropDownItems = 4;
            GenderBox.Name = "GenderBox";
            GenderBox.Size = new Size(106, 23);
            GenderBox.TabIndex = 61;
            GenderBox.Text = "Male";
            // 
            // IgnoreIVFilter
            // 
            IgnoreIVFilter.AutoSize = true;
            IgnoreIVFilter.Location = new Point(112, 215);
            IgnoreIVFilter.Name = "IgnoreIVFilter";
            IgnoreIVFilter.Size = new Size(107, 19);
            IgnoreIVFilter.TabIndex = 60;
            IgnoreIVFilter.Text = "Ignore IV Filter?";
            IgnoreIVFilter.UseVisualStyleBackColor = true;
            // 
            // CheckBoxOf3
            // 
            CheckBoxOf3.AutoSize = true;
            CheckBoxOf3.Location = new Point(88, 76);
            CheckBoxOf3.Name = "CheckBoxOf3";
            CheckBoxOf3.Size = new Size(150, 19);
            CheckBoxOf3.TabIndex = 12;
            CheckBoxOf3.Text = "3 Segment/Family of 3?";
            CheckBoxOf3.UseVisualStyleBackColor = true;
            // 
            // TargetSPE
            // 
            TargetSPE.AutoSize = true;
            TargetSPE.Location = new Point(203, 154);
            TargetSPE.Name = "TargetSPE";
            TargetSPE.Size = new Size(26, 15);
            TargetSPE.TabIndex = 59;
            TargetSPE.Text = "SPE";
            // 
            // TargetSPD
            // 
            TargetSPD.AutoSize = true;
            TargetSPD.Location = new Point(130, 154);
            TargetSPD.Name = "TargetSPD";
            TargetSPD.Size = new Size(28, 15);
            TargetSPD.TabIndex = 58;
            TargetSPD.Text = "SPD";
            // 
            // GenderFilter
            // 
            GenderFilter.AutoSize = true;
            GenderFilter.Location = new Point(168, 49);
            GenderFilter.Name = "GenderFilter";
            GenderFilter.Size = new Size(45, 15);
            GenderFilter.TabIndex = 45;
            GenderFilter.Text = "Gender";
            // 
            // TargetSPA
            // 
            TargetSPA.AutoSize = true;
            TargetSPA.Location = new Point(57, 154);
            TargetSPA.Name = "TargetSPA";
            TargetSPA.Size = new Size(27, 15);
            TargetSPA.TabIndex = 57;
            TargetSPA.Text = "SPA";
            // 
            // TargetIVs
            // 
            TargetIVs.AutoSize = true;
            TargetIVs.Location = new Point(140, 102);
            TargetIVs.Name = "TargetIVs";
            TargetIVs.Size = new Size(46, 15);
            TargetIVs.TabIndex = 53;
            TargetIVs.Text = "IV Filter";
            // 
            // TargetDEF
            // 
            TargetDEF.AutoSize = true;
            TargetDEF.Location = new Point(202, 125);
            TargetDEF.Name = "TargetDEF";
            TargetDEF.Size = new Size(27, 15);
            TargetDEF.TabIndex = 56;
            TargetDEF.Text = "DEF";
            // 
            // TargetHP
            // 
            TargetHP.AutoSize = true;
            TargetHP.Location = new Point(59, 125);
            TargetHP.Name = "TargetHP";
            TargetHP.Size = new Size(23, 15);
            TargetHP.TabIndex = 54;
            TargetHP.Text = "HP";
            // 
            // TargetATK
            // 
            TargetATK.AutoSize = true;
            TargetATK.Location = new Point(130, 125);
            TargetATK.Name = "TargetATK";
            TargetATK.Size = new Size(27, 15);
            TargetATK.TabIndex = 55;
            TargetATK.Text = "ATK";
            // 
            // HPFilter
            // 
            HPFilter.Location = new Point(85, 123);
            HPFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            HPFilter.Name = "HPFilter";
            HPFilter.Size = new Size(42, 23);
            HPFilter.TabIndex = 47;
            HPFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // AtkFilter
            // 
            AtkFilter.Location = new Point(158, 122);
            AtkFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            AtkFilter.Name = "AtkFilter";
            AtkFilter.Size = new Size(41, 23);
            AtkFilter.TabIndex = 48;
            AtkFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SpeFilter
            // 
            SpeFilter.Location = new Point(232, 151);
            SpeFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            SpeFilter.Name = "SpeFilter";
            SpeFilter.Size = new Size(40, 23);
            SpeFilter.TabIndex = 52;
            SpeFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // DefFilter
            // 
            DefFilter.Location = new Point(232, 122);
            DefFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            DefFilter.Name = "DefFilter";
            DefFilter.Size = new Size(40, 23);
            DefFilter.TabIndex = 49;
            DefFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SpdFilter
            // 
            SpdFilter.Location = new Point(158, 151);
            SpdFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            SpdFilter.Name = "SpdFilter";
            SpdFilter.Size = new Size(41, 23);
            SpdFilter.TabIndex = 51;
            SpdFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SpaFilter
            // 
            SpaFilter.Location = new Point(85, 152);
            SpaFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            SpaFilter.Name = "SpaFilter";
            SpaFilter.Size = new Size(42, 23);
            SpaFilter.TabIndex = 50;
            SpaFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SaveButton
            // 
            SaveButton.BackColor = Color.Transparent;
            SaveButton.Location = new Point(122, 270);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 64;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = false;
            SaveButton.Click += SaveButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.BackColor = Color.Transparent;
            ResetButton.Location = new Point(203, 270);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(75, 23);
            ResetButton.TabIndex = 65;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = false;
            ResetButton.Click += ResetButton_Click;
            // 
            // StopConditions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 310);
            ControlBox = false;
            Controls.Add(ResetButton);
            Controls.Add(SaveButton);
            Controls.Add(StopConditionsGroup);
            Name = "StopConditions";
            Text = "Stop Conditions";
            StopConditionsGroup.ResumeLayout(false);
            StopConditionsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HPFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)AtkFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)SpeFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)DefFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)SpdFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)SpaFilter).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox StopConditionsGroup;
        private CheckBox IgnoreIVFilter;
        private CheckBox CheckBoxOf3;
        private Label TargetSPE;
        private Label TargetSPD;
        private Label GenderFilter;
        private Label TargetSPA;
        private Label TargetIVs;
        private Label TargetDEF;
        private Label TargetHP;
        private Label TargetATK;
        private NumericUpDown HPFilter;
        private NumericUpDown AtkFilter;
        private NumericUpDown SpeFilter;
        private NumericUpDown DefFilter;
        private NumericUpDown SpdFilter;
        private NumericUpDown SpaFilter;
        private Button SaveButton;
        private Button ResetButton;
        private ComboBox GenderBox;
        private Label PresetIVs;
        private ComboBox PresetIVBox;
        private ComboBox ShinyBox;
        private Label label1;
        private CheckBox ScaleBox;
    }
}