using Patagames.Ocr;
using Patagames.Ocr.Enums;
using PKHeX.Core;
using SysBot.Base;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.SubForms
{
    public partial class RaidCodeEntry : Form
    {
        private readonly ViewerExecutor Executor;
        public RaidCodeEntry(ViewerExecutor executor)
        {
            InitializeComponent();
            Executor = executor;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 4 || textBox1.Text.Length == 5)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
            else
                MessageBox.Show("TextBox is empty. Try again after you fill it in!");
        }

        private async Task EnterRaidCode(CancellationToken token)
        {
            if (textBox1.Text.Length > 6)
            {
                textBox1.Text = textBox1.Text.Substring(0, 6);
            }
            var strokes = textBox1.Text.ToUpper().ToArray();
            var number = $"NumPad";
            string[] badVals = { "@", "I", "O", "=", "&", ";", "Z", "*", "#", "!", "?" };
            List<HidKeyboardKey> keystopress = new();
            foreach (var str in strokes)
            {
                if (badVals.Contains(str.ToString()))
                {
                    MessageBox.Show($"{str} is not a valid button. Stopping code entry.");
                    return;
                }
                foreach (HidKeyboardKey keypress in (HidKeyboardKey[])Enum.GetValues(typeof(HidKeyboardKey)))
                {
                    if (str.ToString().Equals(keypress.ToString()) || (number + str.ToString()).Equals(keypress.ToString()))
                        keystopress.Add(keypress);
                }
            }
            await Executor.SwitchConnection.SendAsync(SwitchCommand.TypeMultipleKeys(keystopress, true), token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);

        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            Label[] labels = { label1, label2, label3, label4, label5, label6 };
            for (int i = 0; i < labels.Length; i++)
                labels[i].Text = string.Empty;
        }

        private async void textBox1_DoubleClicked(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
            if (textBox1.Text.Length < 4 || textBox1.Text.Length == 5)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            AutoPaste.Enabled = false;
            Clipboard.Clear();
            while (!Clipboard.ContainsText())
            {
                await Task.Delay(0_100);
            }
            textBox1.Text = Clipboard.GetText();
            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);

            AutoPaste.Enabled = true;
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (bmpCrop);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            Label[] labels = { label1, label2, label3, label4, label5, label6 };
            var api = OcrApi.Create();
            api.Init(Languages.English);
            string outputFileName = "screen.jpg";
            Bitmap bmp = new(ScreenToTextPanel.Width, ScreenToTextPanel.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(PointToScreen(ScreenToTextPanel.Location), new Point(0, 0), ScreenToTextPanel.Size);
            if (checkBox1.Checked)
                BitmapInvertColors(bmp);
            bmp = (Bitmap)resizeImage(bmp, new Size(499, 199));
            //bmp = MakeGrayscale3(bmp);
            bmp = sharpen(bmp);
            bmp.Save(outputFileName, ImageFormat.Jpeg);


            // WIP for splitting image into 6 parts for better recognition
            /*for (int r = 0; r < 6; r++)
            {
                Rectangle rect = new(79 * r, 0, bmp.Width / 6, bmp.Height);
                var testimg = cropImage(bmp, rect);
                testimg.Save($"screensplit{r}.jpg", ImageFormat.Jpeg);
                string testText = api.GetTextFromImage((Bitmap)testimg);
                labels[r].Text = testText;
                //File.Delete($"screensplit{r}.jpg");
            }*/

            string plainText = api.GetTextFromImage(bmp);

            if (plainText.ToUpper().Contains('I'))
                plainText = plainText.Replace('I', '1');
            if (plainText.ToUpper().Contains('O'))
                plainText = plainText.Replace('O', '0');
            if (plainText.ToUpper().Contains(')'))
                plainText = plainText.Replace(')', 'J');
            if (plainText.Length > 6)
                plainText = plainText[..6];

            textBox1.Text = plainText;
            api.Dispose();
            bmp.Dispose();
            //File.Delete(outputFileName);

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
        }

        public static void BitmapInvertColors(Bitmap bitmapImage)
        {
            var bitmapRead = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            var bitmapLength = bitmapRead.Stride * bitmapRead.Height;
            var bitmapBGRA = new byte[bitmapLength];
            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, bitmapLength);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < bitmapLength; i += 4)
            {
                bitmapBGRA[i] = (byte)(255 - bitmapBGRA[i]);
                bitmapBGRA[i + 1] = (byte)(255 - bitmapBGRA[i + 1]);
                bitmapBGRA[i + 2] = (byte)(255 - bitmapBGRA[i + 2]);
            }

            var bitmapWrite = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, bitmapLength);
            bitmapImage.UnlockBits(bitmapWrite);
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new(
                   new float[][]
                   {
             new float[] {.3f, .3f, .3f, 0, 0},
             new float[] {.59f, .59f, .59f, 0, 0},
             new float[] {.11f, .11f, .11f, 0, 0},
             new float[] {0, 0, 0, 1, 0},
             new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                using ImageAttributes attributes = new();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return newBitmap;
        }

        public static Bitmap sharpen(Bitmap image)
        {
            Bitmap sharpenImage = new(image.Width, image.Height);

            int filterWidth = 3;
            int filterHeight = 3;
            int w = image.Width;
            int h = image.Height;

            double[,] filter = new double[filterWidth, filterHeight];

            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];
            Color imageColor = Color.Red;
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    // Color must be read per filter entry, not per image pixel.
                    imageColor = image.GetPixel(x, y);

                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + w) % w;
                            int imageY = (y - filterHeight / 2 + filterY + h) % h;

                            // Get the color here - once per fiter entry and image pixel.
                            imageColor = image.GetPixel(imageX, imageY);

                            red += imageColor.R * filter[filterX, filterY];
                            green += imageColor.G * filter[filterX, filterY];
                            blue += imageColor.B * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }
            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    sharpenImage.SetPixel(i, j, result[i, j]);
                }
            }
            return sharpenImage;
        }        
    }
}
