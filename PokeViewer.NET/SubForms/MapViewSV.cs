using PKHeX.Drawing.PokeSprite;
using PKHeX.Drawing;

namespace PokeViewer.NET.SubForms
{
    public partial class MapViewSV : Form
    {
        public MapViewSV(List<Image> img, List<byte[]?> pos)
        {
            InitializeComponent();
            EnableAssets(img, pos);
        }

        public Image? GenerateMapSprite(List<Image> small, List<byte[]?> pos)
        {
            Image result = pictureBox1.Image;
            for (int i = 0; i < small.Count; i++)
            {
                var newsmol = (Image)new Bitmap(small[i], new Size(125, 125));
                SpriteUtil.GetSpriteGlow(newsmol, 0xFF, 0xFF, 0xFF, out var glow, true);
                newsmol = ImageUtil.LayerImage(newsmol, ImageUtil.GetBitmap(glow, newsmol.Width, newsmol.Height, newsmol.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos[i]!, 0);
                float flty = BitConverter.ToSingle(pos[i]!, 8);
                try
                {
                    x = (fltx + 2.072021484) * 1024 / 5000;
                    y = (flty + 5255.240018) * 1024 / 5000;
                    result = ImageUtil.LayerImage(result, newsmol, (int)x - 50, (int)y - 10);
                }
                catch { return null; }
            }
            return result;
        }

        private void EnableAssets(List<Image> sprite, List<byte[]?> pos)
        {
            pictureBox1.Image = GenerateMapSprite(sprite, pos);
        }
    }
}
