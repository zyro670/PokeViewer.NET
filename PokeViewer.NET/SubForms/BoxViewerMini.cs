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
    }
}
