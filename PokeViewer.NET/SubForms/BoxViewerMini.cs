namespace PokeViewer.NET.WideViewForms
{
    public partial class BoxViewerMini : Form
    {
        public BoxViewerMini(PictureBox box, string stats)
        {
            InitializeComponent();
            pictureBox1.Image = box.Image;
            textBox1.Text = stats;
        }
    }
}
