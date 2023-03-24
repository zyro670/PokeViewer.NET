using System.Drawing.Drawing2D;

namespace PokeViewer.NET.SubForms
{
    public partial class MapViewSV : Form
    {
        public MapViewSV(Image img, byte[]? pos)
        {
            InitializeComponent();
            EnableAssets((Bitmap)img, pos);
        }

        public Bitmap Superimpose(Bitmap largeBmp, Bitmap smallBmp, byte[]? pos)
        {
            Graphics g = Graphics.FromImage(largeBmp);
            g.CompositingMode = CompositingMode.SourceOver;
            smallBmp.MakeTransparent();
            float X = BitConverter.ToSingle(pos!, 0);
            float Y = BitConverter.ToSingle(pos!, 4);
            int x = largeBmp.Width * (int)X / 5000;
            int y = largeBmp.Height * (int)Y / 5000;
            g.DrawImage(smallBmp, new Point(x, y));
            return largeBmp;
        }

        private void EnableAssets(Bitmap sprite, byte[]? pos)
        {            
            var img = Superimpose((Bitmap)pictureBox1.Image!, sprite!, pos);
            Point point = new(img.Width, img.Height);
            var map = new Bitmap(pictureBox1.Image!, new Size(pictureBox1.Width, pictureBox1.Height));
            Graphics.FromImage(map).DrawImage(img, point);
            pictureBox1.Image = map;
        }
    }
}
