namespace PokeViewer.NET.SubForms
{
    public partial class BoxViewerMini : Form
    {
        public BoxViewerMini(PictureBox box, string stats, Color back, Color fore)
        {
            InitializeComponent();
            pictureBox1.Image = box.Image;
            textBox1.Text = stats;
            BackColor = back;
            ForeColor = fore;
        }

        private void ClipIt()
        {
            Rectangle bounds = this.Bounds;
            Bitmap bmp = new(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
            }
            DrawToBitmap(bmp, bounds);
            Clipboard.SetImage(bmp);
            MessageBox.Show("Copied to clipboard!");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ClipIt();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ClipIt();
        }
    }
}
