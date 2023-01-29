using NLog;
using PokeViewer.NET.SubForms;
using SysBot.Base;
using System.Net.Sockets;

namespace PokeViewer.NET
{
    public partial class MainViewer : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public static RoutineExecutor Executor = new();
        private readonly ILogger _logger;

        public MainViewer()
        {
            InitializeComponent();
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug("Initializing MainViewer");
        }

        delegate void ChangeButtonStateCallback(Button sender, bool State);
        delegate void TextboxSetTextCallback(TextBox sender, string Text);
        private const string VioletID = "01008F6008C5E000";
        private const string ScarletID = "0100A3D008C5C000";
        private const string LegendsArceusID = "01001F5010DFA000";
        private const string ShiningPearlID = "010018E011D92000";
        private const string BrilliantDiamondID = "0100000011D90000";
        private const string SwordID = "0100ABF008968000";
        private const string ShieldID = "01008DB008C2C000";
        private const string EeveeID = "0100187003A36000";
        private const string PikachuID = "010003F003A34000";
        private int GameType;
        private string RefreshTime = Properties.Settings.Default.RefreshRate;

        private void PokeViewerForm_Load(object sender, EventArgs e)
        {
            _logger.Debug("Loading MainViewer Form");
            SwitchIP.Text = Properties.Settings.Default.SwitchIP;
            LoadOriginDefault(sender, e);
        }

        private void LoadOriginDefault(object send, EventArgs e)
        {
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
            OriginIcon.ImageLocation = url;
        }

        private void CheckForIP(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text != "192.168.0.0")
            {
                Properties.Settings.Default.SwitchIP = textBox.Text;
                Config.IP = SwitchIP.Text;
            }
            Properties.Settings.Default.Save();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            _logger.Debug("Connect clicked");
            if (!SwitchConnection.Connected)
            {
                _logger.Debug("Wasn't Connected to Switch. Connecting...");
                try
                {
                    SwitchConnection.Connect();
                    Connect.Text = "Disconnect";
                    SwitchIP.Enabled = false;
                    InGameScreenshot.Visible = true;
                    DayCareView.Visible = true;
                    Window_Loaded();
                }
                catch (SocketException err)
                {
                    _logger.Warn(err, "Failed to connect to switch");
                    MessageBox.Show(err.Message);
                    MessageBox.Show($"{Environment.NewLine}Ensure IP address is correct before connecting!");
                }
            }
            else if (SwitchConnection.Connected)
            {
                SwitchConnection.Disconnect();
                SwitchConnection.Reset();
                Connect.Text = "Connect";
                SwitchIP.Enabled = true;
                InGameScreenshot.Visible = false;
                DayCareView.Visible = false;
                string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
                OriginIcon.ImageLocation = url;
                SwitchConnection.Reset();
                this.Close();
                Application.Restart();
            }
            
        }

        private void Window_Loaded()
        {
            var token = CancellationToken.None;
            int type = 0;
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/icon_version/64x64/icon_version_";
            string title = SwitchConnection.GetTitleID(token).Result;
            switch (title)
            {
                case ScarletID:
                    {
                        url = url + "SC.png";
                        type = (int)GameSelected.Scarlet;
                        DayCareView.Visible = true;
                        break;
                    }
                case VioletID:
                    {
                        url = url + "VI.png";
                        type = (int)GameSelected.Violet;
                        DayCareView.Visible = true;
                        break;
                    }
            }

            OriginIcon.ImageLocation = url;            
            GameType = type;
        }

        private void CaptureWindow_Click(object sender, EventArgs e)
        {
            _logger.Debug("Screenshotting PokeViewer");
            Bitmap FormScreenShot = new(Width, Height);
            Graphics G = Graphics.FromImage(FormScreenShot);
            G.CopyFromScreen(Location, new Point(0, 0), Size);
            Clipboard.SetImage(FormScreenShot);
        }

        private void InGameScreenshot_Click(object sender, EventArgs e)
        {
            _logger.Debug("Screenshotting Switch");
            var fn = "screenshot.jpg";
            if (!SwitchConnection.Connected)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show($"No device connected! In-Game Screenshot not possible!");
                return;
            }
            var bytes = SwitchConnection.Screengrab(CancellationToken.None).Result;
            File.WriteAllBytes(fn, bytes);
            FileStream stream = new(fn, FileMode.Open);
            var img = Image.FromStream(stream);
            Clipboard.SetImage(img);
            using (Form form = new Form())
            {
                Bitmap vimg = (Bitmap)img;

                form.StartPosition = FormStartPosition.CenterScreen;

                Bitmap original = vimg;
                Bitmap resized = new Bitmap(original, new Size(original.Width / 2, original.Height / 2));

                PictureBox pb = new PictureBox();
                pb.Dock = DockStyle.Fill;
                pb.Image = resized;
                form.Size = resized.Size;

                form.Controls.Add(pb);
                form.ShowDialog();
            }
            stream.Dispose();
            File.Delete(fn);
        }

        private void DayCareView_Click(object sender, EventArgs e)
        {
            _logger.Debug("Clicked EggViewer");
            using Egg_Viewer WideForm = new();
            WideForm.ShowDialog();
        }
    }
}