namespace PokeViewer.NET.SubForms
{
    partial class EggViewerConditions
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
            this.StopConditions = new System.Windows.Forms.GroupBox();
            this.ScaleBox = new System.Windows.Forms.CheckBox();
            this.ShinyBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PresetIVs = new System.Windows.Forms.Label();
            this.PresetIVBox = new System.Windows.Forms.ComboBox();
            this.GenderBox = new System.Windows.Forms.ComboBox();
            this.IgnoreIVFilter = new System.Windows.Forms.CheckBox();
            this.CheckBoxOf3 = new System.Windows.Forms.CheckBox();
            this.TargetSPE = new System.Windows.Forms.Label();
            this.TargetSPD = new System.Windows.Forms.Label();
            this.GenderFilter = new System.Windows.Forms.Label();
            this.TargetSPA = new System.Windows.Forms.Label();
            this.TargetIVs = new System.Windows.Forms.Label();
            this.TargetDEF = new System.Windows.Forms.Label();
            this.TargetHP = new System.Windows.Forms.Label();
            this.TargetATK = new System.Windows.Forms.Label();
            this.HPFilter = new System.Windows.Forms.NumericUpDown();
            this.AtkFilter = new System.Windows.Forms.NumericUpDown();
            this.SpeFilter = new System.Windows.Forms.NumericUpDown();
            this.DefFilter = new System.Windows.Forms.NumericUpDown();
            this.SpdFilter = new System.Windows.Forms.NumericUpDown();
            this.SpaFilter = new System.Windows.Forms.NumericUpDown();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.StopConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HPFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AtkFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpdFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpaFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // StopConditions
            // 
            this.StopConditions.Controls.Add(this.ScaleBox);
            this.StopConditions.Controls.Add(this.ShinyBox);
            this.StopConditions.Controls.Add(this.label1);
            this.StopConditions.Controls.Add(this.PresetIVs);
            this.StopConditions.Controls.Add(this.PresetIVBox);
            this.StopConditions.Controls.Add(this.GenderBox);
            this.StopConditions.Controls.Add(this.IgnoreIVFilter);
            this.StopConditions.Controls.Add(this.CheckBoxOf3);
            this.StopConditions.Controls.Add(this.TargetSPE);
            this.StopConditions.Controls.Add(this.TargetSPD);
            this.StopConditions.Controls.Add(this.GenderFilter);
            this.StopConditions.Controls.Add(this.TargetSPA);
            this.StopConditions.Controls.Add(this.TargetIVs);
            this.StopConditions.Controls.Add(this.TargetDEF);
            this.StopConditions.Controls.Add(this.TargetHP);
            this.StopConditions.Controls.Add(this.TargetATK);
            this.StopConditions.Controls.Add(this.HPFilter);
            this.StopConditions.Controls.Add(this.AtkFilter);
            this.StopConditions.Controls.Add(this.SpeFilter);
            this.StopConditions.Controls.Add(this.DefFilter);
            this.StopConditions.Controls.Add(this.SpdFilter);
            this.StopConditions.Controls.Add(this.SpaFilter);
            this.StopConditions.Location = new System.Drawing.Point(35, 12);
            this.StopConditions.Name = "StopConditions";
            this.StopConditions.Size = new System.Drawing.Size(346, 244);
            this.StopConditions.TabIndex = 63;
            this.StopConditions.TabStop = false;
            this.StopConditions.Text = "Stop Conditions";
            // 
            // ScaleBox
            // 
            this.ScaleBox.AutoSize = true;
            this.ScaleBox.Location = new System.Drawing.Point(23, 48);
            this.ScaleBox.Name = "ScaleBox";
            this.ScaleBox.Size = new System.Drawing.Size(124, 19);
            this.ScaleBox.TabIndex = 66;
            this.ScaleBox.Text = "Mini/Jumbo Only?";
            this.ScaleBox.UseVisualStyleBackColor = true;
            // 
            // ShinyBox
            // 
            this.ShinyBox.FormattingEnabled = true;
            this.ShinyBox.Items.AddRange(new object[] {
            "DisableOption",
            "NonShiny",
            "AnyShiny",
            "StarOnly",
            "SquareOnly"});
            this.ShinyBox.Location = new System.Drawing.Point(150, 18);
            this.ShinyBox.MaxDropDownItems = 5;
            this.ShinyBox.Name = "ShinyBox";
            this.ShinyBox.Size = new System.Drawing.Size(121, 23);
            this.ShinyBox.TabIndex = 65;
            this.ShinyBox.Text = "DisableOption";
            this.ShinyBox.SelectedIndexChanged += new System.EventHandler(this.ShinyBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 64;
            this.label1.Text = "Shiny Target";
            // 
            // PresetIVs
            // 
            this.PresetIVs.AutoSize = true;
            this.PresetIVs.Location = new System.Drawing.Point(66, 186);
            this.PresetIVs.Name = "PresetIVs";
            this.PresetIVs.Size = new System.Drawing.Size(60, 15);
            this.PresetIVs.TabIndex = 63;
            this.PresetIVs.Text = "Preset IVs:";
            // 
            // PresetIVBox
            // 
            this.PresetIVBox.FormattingEnabled = true;
            this.PresetIVBox.Items.AddRange(new object[] {
            "None Selected",
            "6 IV",
            "0 ATK 0 SPE",
            "0 ATK 5 IV",
            "0 SPE 5 IV"});
            this.PresetIVBox.Location = new System.Drawing.Point(130, 183);
            this.PresetIVBox.Name = "PresetIVBox";
            this.PresetIVBox.Size = new System.Drawing.Size(141, 23);
            this.PresetIVBox.TabIndex = 62;
            this.PresetIVBox.Text = "None Selected";
            this.PresetIVBox.SelectedIndexChanged += new System.EventHandler(this.PresetIVBox_SelectedIndexChanged);
            // 
            // GenderBox
            // 
            this.GenderBox.FormattingEnabled = true;
            this.GenderBox.Items.AddRange(new object[] {
            "Male",
            "Female",
            "Genderless",
            "Any"});
            this.GenderBox.Location = new System.Drawing.Point(219, 47);
            this.GenderBox.MaxDropDownItems = 4;
            this.GenderBox.Name = "GenderBox";
            this.GenderBox.Size = new System.Drawing.Size(106, 23);
            this.GenderBox.TabIndex = 61;
            this.GenderBox.Text = "Male";
            // 
            // IgnoreIVFilter
            // 
            this.IgnoreIVFilter.AutoSize = true;
            this.IgnoreIVFilter.Location = new System.Drawing.Point(112, 215);
            this.IgnoreIVFilter.Name = "IgnoreIVFilter";
            this.IgnoreIVFilter.Size = new System.Drawing.Size(107, 19);
            this.IgnoreIVFilter.TabIndex = 60;
            this.IgnoreIVFilter.Text = "Ignore IV Filter?";
            this.IgnoreIVFilter.UseVisualStyleBackColor = true;
            // 
            // CheckBoxOf3
            // 
            this.CheckBoxOf3.AutoSize = true;
            this.CheckBoxOf3.Location = new System.Drawing.Point(88, 76);
            this.CheckBoxOf3.Name = "CheckBoxOf3";
            this.CheckBoxOf3.Size = new System.Drawing.Size(150, 19);
            this.CheckBoxOf3.TabIndex = 12;
            this.CheckBoxOf3.Text = "3 Segment/Family of 3?";
            this.CheckBoxOf3.UseVisualStyleBackColor = true;
            // 
            // TargetSPE
            // 
            this.TargetSPE.AutoSize = true;
            this.TargetSPE.Location = new System.Drawing.Point(203, 154);
            this.TargetSPE.Name = "TargetSPE";
            this.TargetSPE.Size = new System.Drawing.Size(26, 15);
            this.TargetSPE.TabIndex = 59;
            this.TargetSPE.Text = "SPE";
            // 
            // TargetSPD
            // 
            this.TargetSPD.AutoSize = true;
            this.TargetSPD.Location = new System.Drawing.Point(130, 154);
            this.TargetSPD.Name = "TargetSPD";
            this.TargetSPD.Size = new System.Drawing.Size(28, 15);
            this.TargetSPD.TabIndex = 58;
            this.TargetSPD.Text = "SPD";
            // 
            // GenderFilter
            // 
            this.GenderFilter.AutoSize = true;
            this.GenderFilter.Location = new System.Drawing.Point(168, 49);
            this.GenderFilter.Name = "GenderFilter";
            this.GenderFilter.Size = new System.Drawing.Size(45, 15);
            this.GenderFilter.TabIndex = 45;
            this.GenderFilter.Text = "Gender";
            // 
            // TargetSPA
            // 
            this.TargetSPA.AutoSize = true;
            this.TargetSPA.Location = new System.Drawing.Point(57, 154);
            this.TargetSPA.Name = "TargetSPA";
            this.TargetSPA.Size = new System.Drawing.Size(27, 15);
            this.TargetSPA.TabIndex = 57;
            this.TargetSPA.Text = "SPA";
            // 
            // TargetIVs
            // 
            this.TargetIVs.AutoSize = true;
            this.TargetIVs.Location = new System.Drawing.Point(140, 102);
            this.TargetIVs.Name = "TargetIVs";
            this.TargetIVs.Size = new System.Drawing.Size(46, 15);
            this.TargetIVs.TabIndex = 53;
            this.TargetIVs.Text = "IV Filter";
            // 
            // TargetDEF
            // 
            this.TargetDEF.AutoSize = true;
            this.TargetDEF.Location = new System.Drawing.Point(202, 125);
            this.TargetDEF.Name = "TargetDEF";
            this.TargetDEF.Size = new System.Drawing.Size(27, 15);
            this.TargetDEF.TabIndex = 56;
            this.TargetDEF.Text = "DEF";
            // 
            // TargetHP
            // 
            this.TargetHP.AutoSize = true;
            this.TargetHP.Location = new System.Drawing.Point(59, 125);
            this.TargetHP.Name = "TargetHP";
            this.TargetHP.Size = new System.Drawing.Size(23, 15);
            this.TargetHP.TabIndex = 54;
            this.TargetHP.Text = "HP";
            // 
            // TargetATK
            // 
            this.TargetATK.AutoSize = true;
            this.TargetATK.Location = new System.Drawing.Point(130, 125);
            this.TargetATK.Name = "TargetATK";
            this.TargetATK.Size = new System.Drawing.Size(27, 15);
            this.TargetATK.TabIndex = 55;
            this.TargetATK.Text = "ATK";
            // 
            // HPFilter
            // 
            this.HPFilter.Location = new System.Drawing.Point(85, 123);
            this.HPFilter.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.HPFilter.Name = "HPFilter";
            this.HPFilter.Size = new System.Drawing.Size(42, 23);
            this.HPFilter.TabIndex = 47;
            this.HPFilter.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // AtkFilter
            // 
            this.AtkFilter.Location = new System.Drawing.Point(158, 122);
            this.AtkFilter.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.AtkFilter.Name = "AtkFilter";
            this.AtkFilter.Size = new System.Drawing.Size(41, 23);
            this.AtkFilter.TabIndex = 48;
            this.AtkFilter.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SpeFilter
            // 
            this.SpeFilter.Location = new System.Drawing.Point(232, 151);
            this.SpeFilter.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SpeFilter.Name = "SpeFilter";
            this.SpeFilter.Size = new System.Drawing.Size(40, 23);
            this.SpeFilter.TabIndex = 52;
            this.SpeFilter.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // DefFilter
            // 
            this.DefFilter.Location = new System.Drawing.Point(232, 122);
            this.DefFilter.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.DefFilter.Name = "DefFilter";
            this.DefFilter.Size = new System.Drawing.Size(40, 23);
            this.DefFilter.TabIndex = 49;
            this.DefFilter.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SpdFilter
            // 
            this.SpdFilter.Location = new System.Drawing.Point(158, 151);
            this.SpdFilter.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SpdFilter.Name = "SpdFilter";
            this.SpdFilter.Size = new System.Drawing.Size(41, 23);
            this.SpdFilter.TabIndex = 51;
            this.SpdFilter.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SpaFilter
            // 
            this.SpaFilter.Location = new System.Drawing.Point(85, 152);
            this.SpaFilter.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SpaFilter.Name = "SpaFilter";
            this.SpaFilter.Size = new System.Drawing.Size(42, 23);
            this.SpaFilter.TabIndex = 50;
            this.SpaFilter.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(122, 270);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 64;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(203, 270);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 65;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // EggViewerConditions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 310);
            this.ControlBox = false;
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.StopConditions);
            this.Name = "EggViewerConditions";
            this.Text = "EggViewerConditions";
            this.StopConditions.ResumeLayout(false);
            this.StopConditions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HPFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AtkFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpdFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpaFilter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox StopConditions;
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