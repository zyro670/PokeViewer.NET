using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;

namespace PokeViewer.NET.SubForms
{
    public partial class MapViewPaldea : Form
    {
        public MapViewPaldea(List<Image> imgP, List<byte[]?> posP, List<string> count, List<string> strings, List<Image> spriteob, List<string> countob, List<string> stringsob, List<byte[]?> pos2)
        {
            InitializeComponent();
            EnableAssets(imgP, posP, count, strings, spriteob, countob, stringsob, pos2);
            if (Properties.Settings.Default.GameConnected == (int)GameSelected.Scarlet)
                MiraiPic.Visible = false;
            else
                KoraiPic.Visible = false;
        }

        public Image? GenerateMapSprite(List<Image> small, List<byte[]?> pos, List<Image> eve, List<byte[]?> pos2)
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
                    x = (fltx + 2.072021484) * 1024 / 5000;
                    y = (flty + 5255.240018) * 1024 / 5000;
                    result = ImageUtil.LayerImage(result, newsmol, (int)x - 50, (int)y - 10);
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
                    x = (fltx + 2.072021484) * 1024 / 5000;
                    y = (flty + 5255.240018) * 1024 / 5000;
                    result = ImageUtil.LayerImage(result, neweve, (int)x - 50, (int)y - 10);
                }
                catch { return null; }
            }
            return result;
        }

        private void EnableAssets(List<Image> sprite, List<byte[]?> pos, List<string> count, List<string> strings, List<Image> spriteob, List<string> countob, List<string> stringsob, List<byte[]?> pos2)
        {
            pictureBox1.Image = GenerateMapSprite(sprite, pos, spriteob, pos2);
            for (int i = 0; i < sprite.Count; i++)
            {
                switch (i)
                {
                    case 0: OBSprite1.Image = sprite[i]; Ob1Results.Text = count[i]; ObCount1.Text = strings[i]; break;
                    case 1: OBSprite2.Image = sprite[i]; Ob2Results.Text = count[i]; ObCount2.Text = strings[i]; break;
                    case 2: OBSprite3.Image = sprite[i]; Ob3Results.Text = count[i]; ObCount3.Text = strings[i]; break;
                    case 3: OBSprite4.Image = sprite[i]; Ob4Results.Text = count[i]; ObCount4.Text = strings[i]; break;
                    case 4: OBSprite5.Image = sprite[i]; Ob5Results.Text = count[i]; ObCount5.Text = strings[i]; break;
                    case 5: OBSprite6.Image = sprite[i]; Ob6Results.Text = count[i]; ObCount6.Text = strings[i]; break;
                    case 6: OBSprite7.Image = sprite[i]; Ob7Results.Text = count[i]; ObCount7.Text = strings[i]; break;
                    case 7: OBSprite8.Image = sprite[i]; Ob8Results.Text = count[i]; ObCount8.Text = strings[i]; break;
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
