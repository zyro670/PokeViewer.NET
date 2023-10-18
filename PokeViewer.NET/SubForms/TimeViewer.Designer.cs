namespace PokeViewer.NET.SubForms
{
    partial class TimeViewer
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            numericUpDown1 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 41);
            button1.Name = "button1";
            button1.Size = new Size(70, 23);
            button1.TabIndex = 0;
            button1.Text = "Backward";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Backward_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 12);
            button2.Name = "button2";
            button2.Size = new Size(70, 23);
            button2.TabIndex = 1;
            button2.Text = "Forward";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Forward_Click;
            // 
            // button3
            // 
            button3.Location = new Point(88, 41);
            button3.Name = "button3";
            button3.Size = new Size(69, 23);
            button3.TabIndex = 2;
            button3.Text = "Reset";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Reset_Click;
            // 
            // button4
            // 
            button4.Location = new Point(108, 55);
            button4.Name = "button4";
            button4.Size = new Size(92, 23);
            button4.TabIndex = 3;
            button4.Text = "Read Current";
            button4.UseVisualStyleBackColor = true;
            button4.Visible = false;
            button4.Click += Read_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(88, 14);
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(69, 23);
            numericUpDown1.TabIndex = 6;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // TimeViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(168, 80);
            Controls.Add(numericUpDown1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TimeViewer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TimeViewer";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private NumericUpDown numericUpDown1;
    }
}