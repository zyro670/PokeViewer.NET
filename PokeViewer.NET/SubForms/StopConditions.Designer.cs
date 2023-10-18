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
            ResetButton = new Button();
            SPACon = new ComboBox();
            SaveButton = new Button();
            HPCon = new ComboBox();
            SPDCon = new ComboBox();
            ATKCon = new ComboBox();
            SPECon = new ComboBox();
            DEFCon = new ComboBox();
            NatureBox = new ComboBox();
            label2 = new Label();
            ScaleBox = new CheckBox();
            ShinyBox = new ComboBox();
            label1 = new Label();
            GenderBox = new ComboBox();
            ApplyIVFilter = new CheckBox();
            CheckBoxOf3 = new CheckBox();
            TargetSPE = new Label();
            TargetSPD = new Label();
            GenderFilter = new Label();
            TargetSPA = new Label();
            TargetDEF = new Label();
            TargetHP = new Label();
            TargetATK = new Label();
            HPFilter = new NumericUpDown();
            AtkFilter = new NumericUpDown();
            SpeFilter = new NumericUpDown();
            DefFilter = new NumericUpDown();
            SpdFilter = new NumericUpDown();
            SpaFilter = new NumericUpDown();
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
            StopConditionsGroup.Controls.Add(ResetButton);
            StopConditionsGroup.Controls.Add(SPACon);
            StopConditionsGroup.Controls.Add(SaveButton);
            StopConditionsGroup.Controls.Add(HPCon);
            StopConditionsGroup.Controls.Add(SPDCon);
            StopConditionsGroup.Controls.Add(ATKCon);
            StopConditionsGroup.Controls.Add(SPECon);
            StopConditionsGroup.Controls.Add(DEFCon);
            StopConditionsGroup.Controls.Add(NatureBox);
            StopConditionsGroup.Controls.Add(label2);
            StopConditionsGroup.Controls.Add(ScaleBox);
            StopConditionsGroup.Controls.Add(ShinyBox);
            StopConditionsGroup.Controls.Add(label1);
            StopConditionsGroup.Controls.Add(GenderBox);
            StopConditionsGroup.Controls.Add(ApplyIVFilter);
            StopConditionsGroup.Controls.Add(CheckBoxOf3);
            StopConditionsGroup.Controls.Add(TargetSPE);
            StopConditionsGroup.Controls.Add(TargetSPD);
            StopConditionsGroup.Controls.Add(GenderFilter);
            StopConditionsGroup.Controls.Add(TargetSPA);
            StopConditionsGroup.Controls.Add(TargetDEF);
            StopConditionsGroup.Controls.Add(TargetHP);
            StopConditionsGroup.Controls.Add(TargetATK);
            StopConditionsGroup.Controls.Add(HPFilter);
            StopConditionsGroup.Controls.Add(AtkFilter);
            StopConditionsGroup.Controls.Add(SpeFilter);
            StopConditionsGroup.Controls.Add(DefFilter);
            StopConditionsGroup.Controls.Add(SpdFilter);
            StopConditionsGroup.Controls.Add(SpaFilter);
            StopConditionsGroup.Location = new Point(21, 12);
            StopConditionsGroup.Name = "StopConditionsGroup";
            StopConditionsGroup.Size = new Size(378, 239);
            StopConditionsGroup.TabIndex = 63;
            StopConditionsGroup.TabStop = false;
            StopConditionsGroup.Text = "Stop Conditions";
            // 
            // ResetButton
            // 
            ResetButton.BackColor = Color.Transparent;
            ResetButton.Location = new Point(194, 204);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(75, 23);
            ResetButton.TabIndex = 65;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = false;
            ResetButton.Click += ResetButton_Click;
            // 
            // SPACon
            // 
            SPACon.FormattingEnabled = true;
            SPACon.Items.AddRange(new object[] { "=", "<=", ">=" });
            SPACon.Location = new Point(270, 112);
            SPACon.Name = "SPACon";
            SPACon.Size = new Size(40, 23);
            SPACon.TabIndex = 74;
            SPACon.Text = "=";
            // 
            // SaveButton
            // 
            SaveButton.BackColor = Color.Transparent;
            SaveButton.Location = new Point(113, 204);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 64;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = false;
            SaveButton.Click += SaveButton_Click;
            // 
            // HPCon
            // 
            HPCon.FormattingEnabled = true;
            HPCon.Items.AddRange(new object[] { "=", "<=", ">=" });
            HPCon.Location = new Point(270, 22);
            HPCon.Name = "HPCon";
            HPCon.Size = new Size(40, 23);
            HPCon.TabIndex = 73;
            HPCon.Text = "=";
            // 
            // SPDCon
            // 
            SPDCon.FormattingEnabled = true;
            SPDCon.Items.AddRange(new object[] { "=", "<=", ">=" });
            SPDCon.Location = new Point(270, 142);
            SPDCon.Name = "SPDCon";
            SPDCon.Size = new Size(40, 23);
            SPDCon.TabIndex = 72;
            SPDCon.Text = "=";
            // 
            // ATKCon
            // 
            ATKCon.FormattingEnabled = true;
            ATKCon.Items.AddRange(new object[] { "=", "<=", ">=" });
            ATKCon.Location = new Point(270, 52);
            ATKCon.Name = "ATKCon";
            ATKCon.Size = new Size(40, 23);
            ATKCon.TabIndex = 71;
            ATKCon.Text = "=";
            // 
            // SPECon
            // 
            SPECon.FormattingEnabled = true;
            SPECon.Items.AddRange(new object[] { "=", "<=", ">=" });
            SPECon.Location = new Point(270, 172);
            SPECon.Name = "SPECon";
            SPECon.Size = new Size(40, 23);
            SPECon.TabIndex = 70;
            SPECon.Text = "=";
            // 
            // DEFCon
            // 
            DEFCon.FormattingEnabled = true;
            DEFCon.Items.AddRange(new object[] { "=", "<=", ">=" });
            DEFCon.Location = new Point(270, 82);
            DEFCon.Name = "DEFCon";
            DEFCon.Size = new Size(40, 23);
            DEFCon.TabIndex = 69;
            DEFCon.Text = "=";
            // 
            // NatureBox
            // 
            NatureBox.FormattingEnabled = true;
            NatureBox.Items.AddRange(new object[] { "Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky", "Random" });
            NatureBox.Location = new Point(91, 83);
            NatureBox.Name = "NatureBox";
            NatureBox.Size = new Size(121, 23);
            NatureBox.TabIndex = 68;
            NatureBox.Text = "Random";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 86);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 67;
            label2.Text = "Nature";
            // 
            // ScaleBox
            // 
            ScaleBox.AutoSize = true;
            ScaleBox.Location = new Point(16, 113);
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
            ShinyBox.Location = new Point(91, 23);
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
            label1.Location = new Point(11, 26);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 64;
            label1.Text = "Shiny Target";
            // 
            // GenderBox
            // 
            GenderBox.FormattingEnabled = true;
            GenderBox.Items.AddRange(new object[] { "Male", "Female", "Genderless", "Any" });
            GenderBox.Location = new Point(91, 53);
            GenderBox.MaxDropDownItems = 4;
            GenderBox.Name = "GenderBox";
            GenderBox.Size = new Size(121, 23);
            GenderBox.TabIndex = 61;
            GenderBox.Text = "Male";
            // 
            // ApplyIVFilter
            // 
            ApplyIVFilter.AutoSize = true;
            ApplyIVFilter.Location = new Point(16, 173);
            ApplyIVFilter.Name = "ApplyIVFilter";
            ApplyIVFilter.Size = new Size(104, 19);
            ApplyIVFilter.TabIndex = 60;
            ApplyIVFilter.Text = "Apply IV Filter?";
            ApplyIVFilter.UseVisualStyleBackColor = true;
            // 
            // CheckBoxOf3
            // 
            CheckBoxOf3.AutoSize = true;
            CheckBoxOf3.Location = new Point(16, 143);
            CheckBoxOf3.Name = "CheckBoxOf3";
            CheckBoxOf3.Size = new Size(150, 19);
            CheckBoxOf3.TabIndex = 12;
            CheckBoxOf3.Text = "3 Segment/Family of 3?";
            CheckBoxOf3.UseVisualStyleBackColor = true;
            // 
            // TargetSPE
            // 
            TargetSPE.AutoSize = true;
            TargetSPE.Location = new Point(230, 175);
            TargetSPE.Name = "TargetSPE";
            TargetSPE.Size = new Size(26, 15);
            TargetSPE.TabIndex = 59;
            TargetSPE.Text = "SPE";
            // 
            // TargetSPD
            // 
            TargetSPD.AutoSize = true;
            TargetSPD.Location = new Point(230, 145);
            TargetSPD.Name = "TargetSPD";
            TargetSPD.Size = new Size(28, 15);
            TargetSPD.TabIndex = 58;
            TargetSPD.Text = "SPD";
            // 
            // GenderFilter
            // 
            GenderFilter.AutoSize = true;
            GenderFilter.Location = new Point(12, 56);
            GenderFilter.Name = "GenderFilter";
            GenderFilter.Size = new Size(45, 15);
            GenderFilter.TabIndex = 45;
            GenderFilter.Text = "Gender";
            // 
            // TargetSPA
            // 
            TargetSPA.AutoSize = true;
            TargetSPA.Location = new Point(230, 115);
            TargetSPA.Name = "TargetSPA";
            TargetSPA.Size = new Size(27, 15);
            TargetSPA.TabIndex = 57;
            TargetSPA.Text = "SPA";
            // 
            // TargetDEF
            // 
            TargetDEF.AutoSize = true;
            TargetDEF.Location = new Point(230, 85);
            TargetDEF.Name = "TargetDEF";
            TargetDEF.Size = new Size(27, 15);
            TargetDEF.TabIndex = 56;
            TargetDEF.Text = "DEF";
            // 
            // TargetHP
            // 
            TargetHP.AutoSize = true;
            TargetHP.Location = new Point(230, 25);
            TargetHP.Name = "TargetHP";
            TargetHP.Size = new Size(23, 15);
            TargetHP.TabIndex = 54;
            TargetHP.Text = "HP";
            // 
            // TargetATK
            // 
            TargetATK.AutoSize = true;
            TargetATK.Location = new Point(230, 55);
            TargetATK.Name = "TargetATK";
            TargetATK.Size = new Size(27, 15);
            TargetATK.TabIndex = 55;
            TargetATK.Text = "ATK";
            // 
            // HPFilter
            // 
            HPFilter.Location = new Point(320, 22);
            HPFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            HPFilter.Name = "HPFilter";
            HPFilter.Size = new Size(42, 23);
            HPFilter.TabIndex = 47;
            HPFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // AtkFilter
            // 
            AtkFilter.Location = new Point(320, 52);
            AtkFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            AtkFilter.Name = "AtkFilter";
            AtkFilter.Size = new Size(41, 23);
            AtkFilter.TabIndex = 48;
            AtkFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SpeFilter
            // 
            SpeFilter.Location = new Point(320, 172);
            SpeFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            SpeFilter.Name = "SpeFilter";
            SpeFilter.Size = new Size(40, 23);
            SpeFilter.TabIndex = 52;
            SpeFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // DefFilter
            // 
            DefFilter.Location = new Point(320, 82);
            DefFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            DefFilter.Name = "DefFilter";
            DefFilter.Size = new Size(40, 23);
            DefFilter.TabIndex = 49;
            DefFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SpdFilter
            // 
            SpdFilter.Location = new Point(320, 142);
            SpdFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            SpdFilter.Name = "SpdFilter";
            SpdFilter.Size = new Size(41, 23);
            SpdFilter.TabIndex = 51;
            SpdFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // SpaFilter
            // 
            SpaFilter.Location = new Point(320, 112);
            SpaFilter.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            SpaFilter.Name = "SpaFilter";
            SpaFilter.Size = new Size(42, 23);
            SpaFilter.TabIndex = 50;
            SpaFilter.Value = new decimal(new int[] { 31, 0, 0, 0 });
            // 
            // StopConditions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 263);
            ControlBox = false;
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
        private CheckBox ApplyIVFilter;
        private CheckBox CheckBoxOf3;
        private Label TargetSPD;
        private Label GenderFilter;
        private Label TargetSPA;
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
        private ComboBox ShinyBox;
        private Label label1;
        private CheckBox ScaleBox;
        private ComboBox NatureBox;
        private Label label2;
        private ComboBox ATKCon;
        private ComboBox SPECon;
        private ComboBox DEFCon;
        private ComboBox SPACon;
        private ComboBox HPCon;
        private ComboBox SPDCon;
        private Label TargetSPE;
    }
}