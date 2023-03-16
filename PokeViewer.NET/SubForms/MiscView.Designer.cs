namespace PokeViewer.NET.SubForms
{
    partial class MiscView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiscView));
            this.EventRaidScan = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.OverShoot = new System.Windows.Forms.NumericUpDown();
            this.OutbreakScan = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.StopOnSpecies = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.OutbreakSearch = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EnableEmbed = new System.Windows.Forms.CheckBox();
            this.OBSprite8 = new System.Windows.Forms.PictureBox();
            this.OBSprite7 = new System.Windows.Forms.PictureBox();
            this.OBSprite6 = new System.Windows.Forms.PictureBox();
            this.OBSprite5 = new System.Windows.Forms.PictureBox();
            this.OBSprite4 = new System.Windows.Forms.PictureBox();
            this.OBSprite3 = new System.Windows.Forms.PictureBox();
            this.OBSprite2 = new System.Windows.Forms.PictureBox();
            this.OBSprite1 = new System.Windows.Forms.PictureBox();
            this.OutbreakSprite = new System.Windows.Forms.PictureBox();
            this.TotalOutbreaks = new System.Windows.Forms.Label();
            this.Ob8Results = new System.Windows.Forms.Label();
            this.Ob7Results = new System.Windows.Forms.Label();
            this.Ob6Results = new System.Windows.Forms.Label();
            this.Ob5Results = new System.Windows.Forms.Label();
            this.Ob4Results = new System.Windows.Forms.Label();
            this.Ob3Results = new System.Windows.Forms.Label();
            this.Ob2Results = new System.Windows.Forms.Label();
            this.Ob1Results = new System.Windows.Forms.Label();
            this.HardStopOutbreak = new System.Windows.Forms.CheckBox();
            this.RaidEventScan = new System.Windows.Forms.GroupBox();
            this.GuaranteedIVsText = new System.Windows.Forms.Label();
            this.GuaranteedIVs = new System.Windows.Forms.NumericUpDown();
            this.HardStopEventScan = new System.Windows.Forms.CheckBox();
            this.OverShootDelay = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.OverShoot)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutbreakSprite)).BeginInit();
            this.RaidEventScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GuaranteedIVs)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventRaidScan
            // 
            this.EventRaidScan.Location = new System.Drawing.Point(6, 20);
            this.EventRaidScan.Name = "EventRaidScan";
            this.EventRaidScan.Size = new System.Drawing.Size(75, 23);
            this.EventRaidScan.TabIndex = 0;
            this.EventRaidScan.Text = "Scan Event";
            this.EventRaidScan.UseVisualStyleBackColor = true;
            this.EventRaidScan.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 49);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(162, 63);
            this.textBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(175, 78);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Conditions";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OverShoot
            // 
            this.OverShoot.Location = new System.Drawing.Point(55, 67);
            this.OverShoot.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.OverShoot.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.OverShoot.Name = "OverShoot";
            this.OverShoot.Size = new System.Drawing.Size(59, 23);
            this.OverShoot.TabIndex = 3;
            this.OverShoot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OverShoot.Value = new decimal(new int[] {
            920,
            0,
            0,
            0});
            // 
            // OutbreakScan
            // 
            this.OutbreakScan.Location = new System.Drawing.Point(14, 22);
            this.OutbreakScan.Name = "OutbreakScan";
            this.OutbreakScan.Size = new System.Drawing.Size(115, 23);
            this.OutbreakScan.TabIndex = 6;
            this.OutbreakScan.Text = "Outbreak Scan";
            this.OutbreakScan.UseVisualStyleBackColor = true;
            this.OutbreakScan.Click += new System.EventHandler(this.button5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(58, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "①";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(58, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "②";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(58, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 21);
            this.label3.TabIndex = 9;
            this.label3.Text = "③";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(58, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 21);
            this.label4.TabIndex = 10;
            this.label4.Text = "④";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(241, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "⑤";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(241, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 21);
            this.label6.TabIndex = 12;
            this.label6.Text = "⑥";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(241, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 21);
            this.label7.TabIndex = 13;
            this.label7.Text = "⑦";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(241, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 21);
            this.label8.TabIndex = 14;
            this.label8.Text = "⑧";
            // 
            // StopOnSpecies
            // 
            this.StopOnSpecies.Location = new System.Drawing.Point(115, 242);
            this.StopOnSpecies.Name = "StopOnSpecies";
            this.StopOnSpecies.Size = new System.Drawing.Size(198, 23);
            this.StopOnSpecies.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 245);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "Stop On Species";
            // 
            // OutbreakSearch
            // 
            this.OutbreakSearch.AutoSize = true;
            this.OutbreakSearch.Location = new System.Drawing.Point(199, 43);
            this.OutbreakSearch.Name = "OutbreakSearch";
            this.OutbreakSearch.Size = new System.Drawing.Size(138, 19);
            this.OutbreakSearch.TabIndex = 17;
            this.OutbreakSearch.Text = "Search For Outbreak?";
            this.OutbreakSearch.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EnableEmbed);
            this.groupBox1.Controls.Add(this.OBSprite8);
            this.groupBox1.Controls.Add(this.OBSprite7);
            this.groupBox1.Controls.Add(this.OBSprite6);
            this.groupBox1.Controls.Add(this.OBSprite5);
            this.groupBox1.Controls.Add(this.OBSprite4);
            this.groupBox1.Controls.Add(this.OBSprite3);
            this.groupBox1.Controls.Add(this.OBSprite2);
            this.groupBox1.Controls.Add(this.OBSprite1);
            this.groupBox1.Controls.Add(this.OutbreakSprite);
            this.groupBox1.Controls.Add(this.TotalOutbreaks);
            this.groupBox1.Controls.Add(this.Ob8Results);
            this.groupBox1.Controls.Add(this.Ob7Results);
            this.groupBox1.Controls.Add(this.Ob6Results);
            this.groupBox1.Controls.Add(this.Ob5Results);
            this.groupBox1.Controls.Add(this.Ob4Results);
            this.groupBox1.Controls.Add(this.Ob3Results);
            this.groupBox1.Controls.Add(this.Ob2Results);
            this.groupBox1.Controls.Add(this.Ob1Results);
            this.groupBox1.Controls.Add(this.HardStopOutbreak);
            this.groupBox1.Controls.Add(this.OutbreakSearch);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.OutbreakScan);
            this.groupBox1.Controls.Add(this.StopOnSpecies);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 280);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Outbreak Conditions";
            // 
            // EnableEmbed
            // 
            this.EnableEmbed.AutoSize = true;
            this.EnableEmbed.Location = new System.Drawing.Point(319, 244);
            this.EnableEmbed.Name = "EnableEmbed";
            this.EnableEmbed.Size = new System.Drawing.Size(106, 19);
            this.EnableEmbed.TabIndex = 37;
            this.EnableEmbed.Text = "Enable Embed?";
            this.EnableEmbed.UseVisualStyleBackColor = true;
            // 
            // OBSprite8
            // 
            this.OBSprite8.Location = new System.Drawing.Point(199, 196);
            this.OBSprite8.Name = "OBSprite8";
            this.OBSprite8.Size = new System.Drawing.Size(45, 40);
            this.OBSprite8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite8.TabIndex = 36;
            this.OBSprite8.TabStop = false;
            // 
            // OBSprite7
            // 
            this.OBSprite7.Location = new System.Drawing.Point(199, 154);
            this.OBSprite7.Name = "OBSprite7";
            this.OBSprite7.Size = new System.Drawing.Size(45, 40);
            this.OBSprite7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite7.TabIndex = 35;
            this.OBSprite7.TabStop = false;
            // 
            // OBSprite6
            // 
            this.OBSprite6.Location = new System.Drawing.Point(199, 110);
            this.OBSprite6.Name = "OBSprite6";
            this.OBSprite6.Size = new System.Drawing.Size(45, 40);
            this.OBSprite6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite6.TabIndex = 34;
            this.OBSprite6.TabStop = false;
            // 
            // OBSprite5
            // 
            this.OBSprite5.Location = new System.Drawing.Point(199, 68);
            this.OBSprite5.Name = "OBSprite5";
            this.OBSprite5.Size = new System.Drawing.Size(45, 40);
            this.OBSprite5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite5.TabIndex = 33;
            this.OBSprite5.TabStop = false;
            // 
            // OBSprite4
            // 
            this.OBSprite4.Location = new System.Drawing.Point(16, 196);
            this.OBSprite4.Name = "OBSprite4";
            this.OBSprite4.Size = new System.Drawing.Size(45, 40);
            this.OBSprite4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite4.TabIndex = 32;
            this.OBSprite4.TabStop = false;
            // 
            // OBSprite3
            // 
            this.OBSprite3.Location = new System.Drawing.Point(16, 154);
            this.OBSprite3.Name = "OBSprite3";
            this.OBSprite3.Size = new System.Drawing.Size(45, 40);
            this.OBSprite3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite3.TabIndex = 31;
            this.OBSprite3.TabStop = false;
            // 
            // OBSprite2
            // 
            this.OBSprite2.Location = new System.Drawing.Point(16, 110);
            this.OBSprite2.Name = "OBSprite2";
            this.OBSprite2.Size = new System.Drawing.Size(45, 40);
            this.OBSprite2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite2.TabIndex = 30;
            this.OBSprite2.TabStop = false;
            // 
            // OBSprite1
            // 
            this.OBSprite1.Location = new System.Drawing.Point(16, 68);
            this.OBSprite1.Name = "OBSprite1";
            this.OBSprite1.Size = new System.Drawing.Size(45, 40);
            this.OBSprite1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OBSprite1.TabIndex = 29;
            this.OBSprite1.TabStop = false;
            // 
            // OutbreakSprite
            // 
            this.OutbreakSprite.Image = ((System.Drawing.Image)(resources.GetObject("OutbreakSprite.Image")));
            this.OutbreakSprite.Location = new System.Drawing.Point(361, 12);
            this.OutbreakSprite.Name = "OutbreakSprite";
            this.OutbreakSprite.Size = new System.Drawing.Size(64, 51);
            this.OutbreakSprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OutbreakSprite.TabIndex = 28;
            this.OutbreakSprite.TabStop = false;
            // 
            // TotalOutbreaks
            // 
            this.TotalOutbreaks.AutoSize = true;
            this.TotalOutbreaks.Location = new System.Drawing.Point(11, 48);
            this.TotalOutbreaks.Name = "TotalOutbreaks";
            this.TotalOutbreaks.Size = new System.Drawing.Size(92, 15);
            this.TotalOutbreaks.TabIndex = 27;
            this.TotalOutbreaks.Text = "Total Outbreaks:";
            // 
            // Ob8Results
            // 
            this.Ob8Results.AutoSize = true;
            this.Ob8Results.Location = new System.Drawing.Point(265, 203);
            this.Ob8Results.Name = "Ob8Results";
            this.Ob8Results.Size = new System.Drawing.Size(117, 15);
            this.Ob8Results.TabIndex = 26;
            this.Ob8Results.Text = "No Outbreak Present";
            // 
            // Ob7Results
            // 
            this.Ob7Results.AutoSize = true;
            this.Ob7Results.Location = new System.Drawing.Point(265, 162);
            this.Ob7Results.Name = "Ob7Results";
            this.Ob7Results.Size = new System.Drawing.Size(117, 15);
            this.Ob7Results.TabIndex = 25;
            this.Ob7Results.Text = "No Outbreak Present";
            // 
            // Ob6Results
            // 
            this.Ob6Results.AutoSize = true;
            this.Ob6Results.Location = new System.Drawing.Point(265, 120);
            this.Ob6Results.Name = "Ob6Results";
            this.Ob6Results.Size = new System.Drawing.Size(117, 15);
            this.Ob6Results.TabIndex = 24;
            this.Ob6Results.Text = "No Outbreak Present";
            // 
            // Ob5Results
            // 
            this.Ob5Results.AutoSize = true;
            this.Ob5Results.Location = new System.Drawing.Point(265, 81);
            this.Ob5Results.Name = "Ob5Results";
            this.Ob5Results.Size = new System.Drawing.Size(117, 15);
            this.Ob5Results.TabIndex = 23;
            this.Ob5Results.Text = "No Outbreak Present";
            // 
            // Ob4Results
            // 
            this.Ob4Results.AutoSize = true;
            this.Ob4Results.Location = new System.Drawing.Point(82, 204);
            this.Ob4Results.Name = "Ob4Results";
            this.Ob4Results.Size = new System.Drawing.Size(117, 15);
            this.Ob4Results.TabIndex = 22;
            this.Ob4Results.Text = "No Outbreak Present";
            // 
            // Ob3Results
            // 
            this.Ob3Results.AutoSize = true;
            this.Ob3Results.Location = new System.Drawing.Point(82, 161);
            this.Ob3Results.Name = "Ob3Results";
            this.Ob3Results.Size = new System.Drawing.Size(117, 15);
            this.Ob3Results.TabIndex = 21;
            this.Ob3Results.Text = "No Outbreak Present";
            // 
            // Ob2Results
            // 
            this.Ob2Results.AutoSize = true;
            this.Ob2Results.Location = new System.Drawing.Point(82, 119);
            this.Ob2Results.Name = "Ob2Results";
            this.Ob2Results.Size = new System.Drawing.Size(117, 15);
            this.Ob2Results.TabIndex = 20;
            this.Ob2Results.Text = "No Outbreak Present";
            // 
            // Ob1Results
            // 
            this.Ob1Results.AutoSize = true;
            this.Ob1Results.Location = new System.Drawing.Point(83, 81);
            this.Ob1Results.Name = "Ob1Results";
            this.Ob1Results.Size = new System.Drawing.Size(117, 15);
            this.Ob1Results.TabIndex = 19;
            this.Ob1Results.Text = "No Outbreak Present";
            // 
            // HardStopOutbreak
            // 
            this.HardStopOutbreak.AutoSize = true;
            this.HardStopOutbreak.Location = new System.Drawing.Point(199, 25);
            this.HardStopOutbreak.Name = "HardStopOutbreak";
            this.HardStopOutbreak.Size = new System.Drawing.Size(121, 19);
            this.HardStopOutbreak.TabIndex = 18;
            this.HardStopOutbreak.Text = "Stop After 1 Scan?";
            this.HardStopOutbreak.UseVisualStyleBackColor = true;
            // 
            // RaidEventScan
            // 
            this.RaidEventScan.Controls.Add(this.GuaranteedIVsText);
            this.RaidEventScan.Controls.Add(this.GuaranteedIVs);
            this.RaidEventScan.Controls.Add(this.HardStopEventScan);
            this.RaidEventScan.Controls.Add(this.EventRaidScan);
            this.RaidEventScan.Controls.Add(this.button2);
            this.RaidEventScan.Controls.Add(this.textBox1);
            this.RaidEventScan.Location = new System.Drawing.Point(12, 15);
            this.RaidEventScan.Name = "RaidEventScan";
            this.RaidEventScan.Size = new System.Drawing.Size(257, 127);
            this.RaidEventScan.TabIndex = 19;
            this.RaidEventScan.TabStop = false;
            this.RaidEventScan.Text = "Raid Event Scanner";
            // 
            // GuaranteedIVsText
            // 
            this.GuaranteedIVsText.AutoSize = true;
            this.GuaranteedIVsText.Location = new System.Drawing.Point(169, 25);
            this.GuaranteedIVsText.Name = "GuaranteedIVsText";
            this.GuaranteedIVsText.Size = new System.Drawing.Size(86, 15);
            this.GuaranteedIVsText.TabIndex = 8;
            this.GuaranteedIVsText.Text = "Guaranteed IVs";
            // 
            // GuaranteedIVs
            // 
            this.GuaranteedIVs.Location = new System.Drawing.Point(177, 50);
            this.GuaranteedIVs.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.GuaranteedIVs.Name = "GuaranteedIVs";
            this.GuaranteedIVs.Size = new System.Drawing.Size(59, 23);
            this.GuaranteedIVs.TabIndex = 7;
            this.GuaranteedIVs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GuaranteedIVs.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // HardStopEventScan
            // 
            this.HardStopEventScan.AutoSize = true;
            this.HardStopEventScan.Location = new System.Drawing.Point(87, 23);
            this.HardStopEventScan.Name = "HardStopEventScan";
            this.HardStopEventScan.Size = new System.Drawing.Size(81, 19);
            this.HardStopEventScan.TabIndex = 6;
            this.HardStopEventScan.Text = "HardStop?";
            this.HardStopEventScan.UseVisualStyleBackColor = true;
            // 
            // OverShootDelay
            // 
            this.OverShootDelay.AutoSize = true;
            this.OverShootDelay.Location = new System.Drawing.Point(45, 49);
            this.OverShootDelay.Name = "OverShootDelay";
            this.OverShootDelay.Size = new System.Drawing.Size(80, 15);
            this.OverShootDelay.TabIndex = 5;
            this.OverShootDelay.Text = "OverShootMS";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OverShoot);
            this.groupBox2.Controls.Add(this.OverShootDelay);
            this.groupBox2.Location = new System.Drawing.Point(279, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 127);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Misc";
            // 
            // MiscView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 442);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RaidEventScan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MiscView";
            this.Text = "MiscView";
            ((System.ComponentModel.ISupportInitialize)(this.OverShoot)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBSprite1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutbreakSprite)).EndInit();
            this.RaidEventScan.ResumeLayout(false);
            this.RaidEventScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GuaranteedIVs)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button EventRaidScan;
        private TextBox textBox1;
        private Button button2;
        private NumericUpDown OverShoot;
        private Button OutbreakScan;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox StopOnSpecies;
        private Label label9;
        private CheckBox OutbreakSearch;
        private GroupBox groupBox1;
        private GroupBox RaidEventScan;
        private Label OverShootDelay;
        private CheckBox HardStopOutbreak;
        private CheckBox HardStopEventScan;
        private Label Ob8Results;
        private Label Ob7Results;
        private Label Ob6Results;
        private Label Ob5Results;
        private Label Ob4Results;
        private Label Ob3Results;
        private Label Ob2Results;
        private Label Ob1Results;
        private Label TotalOutbreaks;
        private PictureBox OutbreakSprite;
        private Label GuaranteedIVsText;
        private NumericUpDown GuaranteedIVs;
        private PictureBox OBSprite1;
        private PictureBox OBSprite8;
        private PictureBox OBSprite7;
        private PictureBox OBSprite6;
        private PictureBox OBSprite5;
        private PictureBox OBSprite4;
        private PictureBox OBSprite3;
        private PictureBox OBSprite2;
        private GroupBox groupBox2;
        private CheckBox EnableEmbed;
    }
}