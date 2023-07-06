namespace PokeViewer.NET.WideViewForms
{
    partial class WideViewerLGPE
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
            pictureBox1 = new PictureBox();
            WaitButton = new Button();
            flagbox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(47, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(156, 128);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // WaitButton
            // 
            WaitButton.BackColor = Color.Transparent;
            WaitButton.Location = new Point(65, 146);
            WaitButton.Name = "WaitButton";
            WaitButton.Size = new Size(123, 23);
            WaitButton.TabIndex = 3;
            WaitButton.Text = "Wait For Shiny";
            WaitButton.UseVisualStyleBackColor = false;
            WaitButton.Click += WaitButton_Click;
            // 
            // flagbox
            // 
            flagbox.Location = new Point(111, 175);
            flagbox.Name = "flagbox";
            flagbox.ReadOnly = true;
            flagbox.Size = new Size(27, 23);
            flagbox.TabIndex = 4;
            // 
            // WideViewerLGPE
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(246, 210);
            Controls.Add(flagbox);
            Controls.Add(WaitButton);
            Controls.Add(pictureBox1);
            MaximizeBox = false;
            Name = "WideViewerLGPE";
            Text = "PokeViewer.NET - WideViewLGPE";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private Button WaitButton;
        private TextBox flagbox;
    }
}