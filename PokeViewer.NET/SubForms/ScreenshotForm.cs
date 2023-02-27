namespace PokeViewer.NET.SubForms
{
    public partial class ScreenshotForm : Form
    {
        private readonly ViewerExecutor Executor;
        public ScreenshotForm(ViewerExecutor executor)
        {
            InitializeComponent();            
            Executor = executor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            var fn = "screenshot.jpg";
            if (!Executor.SwitchConnection.Connected)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show($"No device connected! In-Game Screenshot not possible!");
                return;
            }
            var bytes = Executor.SwitchConnection.Screengrab(token).Result;
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

            MessageBox.Show("Screenshot copied to Clipboard!");
            stream.Dispose();
            File.Delete(fn);

        }
    }
}
