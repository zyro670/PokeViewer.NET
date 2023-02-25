using SysBot.Base;
using Tulpep.NotificationWindow;

namespace PokeViewer.NET.SubForms
{
    public partial class ScreenshotForm : Form
    {
        private readonly SwitchSocketAsync SwitchConnection;
        public ScreenshotForm(SwitchSocketAsync switchConnection)
        {
            InitializeComponent();
            SwitchConnection = switchConnection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            var fn = "screenshot.jpg";
            if (!SwitchConnection.Connected)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show($"No device connected! In-Game Screenshot not possible!");
                return;
            }
            var bytes = SwitchConnection.Screengrab(token).Result;
            File.WriteAllBytes(fn, bytes);
            FileStream stream = new(fn, FileMode.Open);
            var img = Image.FromStream(stream);
            Clipboard.SetImage(img);
            Bitmap vimg = (Bitmap)img;

            StartPosition = FormStartPosition.CenterScreen;
            Bitmap original = vimg;
            Bitmap resized = new(original, new Size(original.Width / 2, original.Height / 2));

            pictureBox1.Image = resized;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            PopupNotifier popup = new()
            {
                TitleText = "Screenshot copied to Clipboard!",
                ContentText = "Ready for sharing!",
                Size = new Size(300, 100),
                TitleColor = Color.Black,
                BorderColor = Color.Black,
                ShowCloseButton = true,
            };
            popup.Popup();

            stream.Dispose();
            File.Delete(fn);
        }
    }
}
