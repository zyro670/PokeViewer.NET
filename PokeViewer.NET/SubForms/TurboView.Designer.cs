namespace PokeViewer.NET.SubForms
{
    partial class TurboView
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
            checkBox1 = new CheckBox();
            comboBox1 = new ComboBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(135, 61);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Turbo";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(45, 64);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(84, 19);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "Stop Turbo";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "A", "B", "X", "Y", "RSTICK", "LSTICK", "L", "R", "ZL", "ZR", "PLUS", "MINUS", "DUP", "DDOWN", "DLEFT", "DRIGHT", "HOME", "CAPTURE" });
            comboBox1.Location = new Point(45, 32);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(165, 23);
            comboBox1.TabIndex = 2;
            comboBox1.Text = "A";
            // 
            // TurboView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(265, 131);
            Controls.Add(comboBox1);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Name = "TurboView";
            Text = "TurboView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
    }
}