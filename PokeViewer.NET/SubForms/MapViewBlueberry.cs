using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;

namespace PokeViewer.NET.SubForms
{
    public partial class MapViewBlueberry : Form
    {
        public MapViewBlueberry(List<Image> img, List<byte[]?> pos, List<string> count, List<string> strings, List<Image> spriteob, List<string> countob, List<string> stringsob, List<byte[]?> pos2)
        {
            InitializeComponent();
            EnableAssets(img, pos, count, strings, spriteob, countob, stringsob, pos2);
        }

        public Image? GenerateMapSprite(List<Image> small, List<byte[]?> pos, List<Image> eve, List<byte[]?> pos2)
        {
            Image result = BBPictureBox.Image;
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
            for (int i = 0; i < eve.Count; i++)
            {
                var neweve = (Image)new Bitmap(eve[i], new Size(65, 65));
                SpriteUtil.GetSpriteGlow(neweve, 0xFF, 0xFF, 0xFF, out var glow, true);
                neweve = ImageUtil.LayerImage(neweve, ImageUtil.GetBitmap(glow, neweve.Width, neweve.Height, neweve.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos2[i]!, 0);
                float flty = BitConverter.ToSingle(pos2[i]!, 8);
                try
                {
                    x = ((fltx * 2.766970605475146) + -248.08352352566726) * 512 / 5000;
                    y = ((flty * 2.5700782642623805) + 5070.808599816581) * 512 / 5000;
                    result = ImageUtil.LayerImage(result, neweve, (int)x - 50, (int)y - 50);
                }
                catch { return null; }
            }
            return result;
        }

        private void EnableAssets(List<Image> sprite, List<byte[]?> pos, List<string> count, List<string> strings, List<Image> spriteob, List<string> countob, List<string> stringsob, List<byte[]?> pos2)
        {
            BBPictureBox.Image = GenerateMapSprite(sprite, pos, spriteob, pos2);
            for (int i = 0; i < sprite.Count; i++)
            {
                switch (i)
                {
                    case 0: ObSprite13.Image = sprite[i]; ObResults13.Text = count[i]; ObCount13.Text = strings[i]; break;
                    case 1: ObSprite14.Image = sprite[i]; ObResults14.Text = count[i]; ObCount14.Text = strings[i]; break;
                    case 2: ObSprite15.Image = sprite[i]; ObResults15.Text = count[i]; ObCount15.Text = strings[i]; break;
                    case 3: ObSprite16.Image = sprite[i]; ObResults16.Text = count[i]; ObCount16.Text = strings[i]; break;
                    case 4: ObSprite17.Image = sprite[i]; ObResults17.Text = count[i]; ObCount17.Text = strings[i]; break;
                }
            }
            for (int i = 0; i < spriteob.Count; i++)
            {
                switch (i)
                {
                    case 0: EventSpr1.Image = spriteob[i]; EventCount1.Text = countob[i]; EventName1.Text = stringsob[i]; break;
                    case 1: EventSpr2.Image = spriteob[i]; EventCount2.Text = countob[i]; EventName2.Text = stringsob[i]; break;
                    case 2: EventSpr3.Image = spriteob[i]; EventCount3.Text = countob[i]; EventName3.Text = stringsob[i]; break;
                    case 3: EventSpr4.Image = spriteob[i]; EventCount4.Text = countob[i]; EventName4.Text = stringsob[i]; break;
                    case 4: EventSpr5.Image = spriteob[i]; EventCount5.Text = countob[i]; EventName5.Text = stringsob[i]; break;
                }
            }
        }
    }
}
