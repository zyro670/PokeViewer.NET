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
            this.OriginIcon = new System.Windows.Forms.PictureBox();
            this.WindowCapture = new System.Windows.Forms.Button();
            this.DayCareView = new System.Windows.Forms.Button();
            this.InGameScreenshot = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OriginIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(27, 53);
            this.Connect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(107, 42);
            this.Connect.TabIndex = 0;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // SwitchIP
            // 
            this.SwitchIP.Location = new System.Drawing.Point(27, 10);
            this.SwitchIP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SwitchIP.Name = "SwitchIP";
            this.SwitchIP.Size = new System.Drawing.Size(181, 31);
            this.SwitchIP.TabIndex = 2;
            this.SwitchIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SwitchIP.TextChanged += new System.EventHandler(this.CheckForIP);
            // 
            // OriginIcon
            // 
            this.OriginIcon.ImageLocation = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_gene" +
    "ration_00%5Esb.png";
            this.OriginIcon.Location = new System.Drawing.Point(241, 52);
            this.OriginIcon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OriginIcon.Name = "OriginIcon";
            this.OriginIcon.Size = new System.Drawing.Size(64, 64);
            this.OriginIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OriginIcon.TabIndex = 10;
            this.OriginIcon.TabStop = false;
            // 
            // WindowCapture
            // 
            this.WindowCapture.Image = ((System.Drawing.Image)(resources.GetObject("WindowCapture.Image")));
            this.WindowCapture.Location = new System.Drawing.Point(156, 55);
            this.WindowCapture.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WindowCapture.Name = "WindowCapture";
            this.WindowCapture.Size = new System.Drawing.Size(53, 42);
            this.WindowCapture.TabIndex = 20;
            this.WindowCapture.UseVisualStyleBackColor = true;
            this.WindowCapture.Click += new System.EventHandler(this.CaptureWindow_Click);
            // 
            // DayCareView
            // 
            this.DayCareView.Location = new System.Drawing.Point(451, 7);
            this.DayCareView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DayCareView.Name = "DayCareView";
            this.DayCareView.Size = new System.Drawing.Size(107, 42);
            this.DayCareView.TabIndex = 27;
            this.DayCareView.Text = "Egg View";
            this.DayCareView.UseVisualStyleBackColor = true;
            this.DayCareView.Visible = false;
            this.DayCareView.Click += new System.EventHandler(this.DayCareView_Click);
            // 
            // InGameScreenshot
            // 
            this.InGameScreenshot.Image = ((System.Drawing.Image)(resources.GetObject("InGameScreenshot.Image")));
            this.InGameScreenshot.Location = new System.Drawing.Point(156, 107);
            this.InGameScreenshot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InGameScreenshot.Name = "InGameScreenshot";
            this.InGameScreenshot.Size = new System.Drawing.Size(53, 42);
            this.InGameScreenshot.TabIndex = 30;
            this.InGameScreenshot.UseVisualStyleBackColor = true;
            this.InGameScreenshot.Click += new System.EventHandler(this.InGameScreenshot_Click);
            // 
            // MainViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(579, 176);
            this.Controls.Add(this.InGameScreenshot);
            this.Controls.Add(this.DayCareView);
            this.Controls.Add(this.WindowCapture);
            this.Controls.Add(this.OriginIcon);
            this.Controls.Add(this.SwitchIP);
            this.Controls.Add(this.Connect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MainViewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PokeViewer.NET";
            this.Load += new System.EventHandler(this.PokeViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OriginIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button Connect;
        private TextBox SwitchIP;
        private PictureBox OriginIcon;
        private Button WindowCapture;
        private Button DayCareView;
        private Button InGameScreenshot;
    }
}