namespace PokeViewer.NET.SubForms
{
    partial class EventCodeEntry
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
            RedeemButton = new Button();
            textBox1 = new TextBox();
            ClearButton = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // RedeemButton
            // 
            RedeemButton.Location = new Point(15, 55);
            RedeemButton.Name = "RedeemButton";
            RedeemButton.Size = new Size(98, 23);
            RedeemButton.TabIndex = 0;
            RedeemButton.Text = "Redeem!";
            RedeemButton.UseVisualStyleBackColor = true;
            RedeemButton.Click += RedeemButton_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(15, 12);
            textBox1.MaxLength = 16;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(233, 36);
            textBox1.TabIndex = 1;
            // 
            // ClearButton
            // 
            ClearButton.Location = new Point(150, 55);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(98, 23);
            ClearButton.TabIndex = 2;
            ClearButton.Text = "Clear Text";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(113, 50);
            label1.Name = "label1";
            label1.Size = new Size(39, 30);
            label1.TabIndex = 3;
            label1.Text = "🎁";
            // 
            // EventCodeEntry
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(260, 92);
            Controls.Add(ClearButton);
            Controls.Add(textBox1);
            Controls.Add(RedeemButton);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "EventCodeEntry";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Code Entry";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button RedeemButton;
        private TextBox textBox1;
        private Button ClearButton;
        private Label label1;
    }
}