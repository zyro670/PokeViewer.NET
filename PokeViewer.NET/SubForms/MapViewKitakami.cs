using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;

namespace PokeViewer.NET.SubForms
{
    public partial class MapViewKitakami : Form
    {
        public MapViewKitakami(List<Image> img, List<byte[]?> pos)
        {
            InitializeComponent();
            EnableAssets(img, pos);
        }

        public Image? GenerateMapSprite(List<Image> small, List<byte[]?> pos)
        {
            Image result = pictureBox1.Image;
            for (int i = 0; i < small.Count; i++)
            {
                var newsmol = (Image)new Bitmap(small[i], new Size(65, 65));
                SpriteUtil.GetSpriteGlow(newsmol, 0xFF, 0xFF, 0xFF, out var glow, true);
                newsmol = ImageUtil.LayerImage(newsmol, ImageUtil.GetBitmap(glow, newsmol.Width, newsmol.Height, newsmol.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos[i]!, 0);
                float flty = BitConverter.ToSingle(pos[i]!, 8);
                try
                {
                    x = ((fltx * 2.766970605475146) + -248.08352352566726) * 512 / 5000;
                    y = ((flty * 2.5700782642623805) + 5070.808599816581) * 512 / 5000;
                    result = ImageUtil.LayerImage(result, newsmol, (int)x - 50, (int)y - 50);
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
