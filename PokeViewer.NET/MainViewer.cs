using PKHeX.Core;
using SysBot.Base;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace PokeViewer.NET
{
    public partial class MainViewer : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public MainViewer()
        {
            InitializeComponent();
        }

        delegate void ChangeButtonStateCallback(Button sender, bool State);
        delegate void TextboxSetTextCallback(TextBox sender, string Text);
        private const string LegendsArceusID = "01001F5010DFA000";
        private const string ShiningPearlID = "010018E011D92000";
        private const string BrilliantDiamondID = "0100000011D90000";
        private const string SwordID = "0100ABF008968000";
        private const string ShieldID = "01008DB008C2C000";
        private const string EeveeID = "0100187003A36000";
        private const string PikachuID = "010003F003A34000";
        private int GameType;
        private string RefreshTime = Properties.Settings.Default.RefreshRate;
        private bool ReadOverworld = false;
        private void ChangeButtonState(Button sender, bool State)
        {
            if (sender.InvokeRequired)
            {
                ChangeButtonStateCallback d = new(ChangeButtonState);
                sender.Invoke(d, sender, State);
            }
            else
            {
                sender.Enabled = State;
            }
        }

        private void TextboxSetText(TextBox sender, string Text)
        {
            if (sender.InvokeRequired)
            {
                TextboxSetTextCallback d = new(TextboxSetText);
                sender.Invoke(d, sender, Text);
            }
            else
            {
                sender.Text = Text;
            }
        }

        private void PokeViewerForm_Load(object sender, EventArgs e)
        {
            SwitchIP.Text = Properties.Settings.Default.SwitchIP;
            this.View.Visible = false;
            this.ViewBox.Visible = false;
            this.PokeSprite.Visible = false;
            this.LiveStats.Visible = false;
            this.RefreshStats.Visible = false;
            this.HidePIDEC.Visible = false;
            this.ScreenShot.Visible = false;
            this.HpLabel.Visible = false;
            LoadOriginDefault(sender, e);
            LoadDateTime(sender, e);
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

        private void CheckForHide(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                Properties.Settings.Default.HidePIDEC = HidePIDEC.Checked;
                HidePIDEC.Checked = true;
            }
            else
            {
                Properties.Settings.Default.HidePIDEC = false;
                HidePIDEC.Checked = false;
            }
            Properties.Settings.Default.Save();
        }

        private void LoadDateTime(object sender, EventArgs e)
        {
            this.TodaysDate.Text = "Met Date: " + DateTime.Today.ToString("MM/dd/yyyy");
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                SwitchConnection.Connect();                
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ChangeButtonState(Program.Viewer.Connect, false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ChangeButtonState(Program.Viewer.Disconnect, true);
                ChangeButtonState(Program.Viewer.View, true);
                this.ViewBox.Visible = true;
                this.PokeSprite.Visible = true;
                this.LiveStats.Visible = true;
                this.RefreshStats.Visible = true;
                this.Refresh.Visible = true;
                this.RefreshBox.Visible = true;
                this.ScreenShot.Visible = true;
                this.HidePIDEC.Visible = true;
                this.HpLabel.Visible = true;
                this.View.Visible = true;
                Window_Loaded();
            }
            catch (SocketException err)
            {
                if (err.Message.Contains("failed to respond"))
                {
                    MessageBox.Show(err.Message);
                }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ChangeButtonState(Program.Viewer.Connect, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ChangeButtonState(Program.Viewer.Disconnect, false);
                ChangeButtonState(Program.Viewer.View, false);
            }
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            SwitchConnection.Disconnect();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ChangeButtonState(Program.Viewer.Connect, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            ChangeButtonState(Program.Viewer.Disconnect, false);
            ChangeButtonState(Program.Viewer.View, false);
            this.ReadOverworld = false;
            this.ViewBox.Visible = false;
            this.PokeSprite.Visible = false;
            this.LiveStats.Visible = false;
            this.RefreshStats.Visible = false;
            this.Refresh.Visible = false;
            this.RefreshBox.Visible = false;
            this.ScreenShot.Visible = false;
            this.HidePIDEC.Visible = false;
            this.View.Visible = false;
            this.Typing1.Visible = false;
            this.Typing2.Visible = false;
            this.Specialty.Visible = false;
            this.HpLabel.Visible = false;
            this.LiveStats.Clear();
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
            OriginIcon.ImageLocation = url;
        }

        private void View_Click(object sender, EventArgs e)
        {
            if (this.ScreenShot.Checked)
            {
                var fn = "screenshot.jpg";
                if (!SwitchConnection.Connected)
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show($"Not connected to Switch, screenshot not possible!");
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
            if (this.ScreenShot.Checked == true)
                this.RefreshStats.Checked = false;

            ReadEncounter_ClickAsync(sender, e);

            this.ScreenShot.Checked = false;
        }

        private async void FillPokeData(PKM pk, ulong offset, uint offset2, int size)
        {
            var sprite = string.Empty;
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                TextboxSetText(Program.Viewer.ViewBox, "No Pokémon present.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ChangeButtonState(Program.Viewer.View, true);
                sprite = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0000_000_uk_n_00000000_f_n.png";
                PokeSprite.Load(sprite);
                Typing1.Visible = false;
                Typing2.Visible = false;
                Specialty.Visible = false;
                this.LiveStats.Clear();
                return;
            }
            Typing1.Visible = true;
            Typing2.Visible = true;
            bool alpha = pk is PA8 pa8 ? pa8.IsAlpha: false;
            bool hasMark = false;
            bool isGmax = pk is PK8 pk8 ? pk8.CanGigantamax : false;
            string msg = "";
            if (pk is PK8)
            {
                hasMark = HasMark((PK8)pk, out RibbonIndex mark);
                msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";

            }
            string isAlpha = alpha ? $"αlpha - " : "";
            string pid = HidePIDEC.Checked ? "" : $"{Environment.NewLine}PID: {pk.PID:X8}";
            string ec = HidePIDEC.Checked ? "" : $"{Environment.NewLine}EC: {pk.EncryptionConstant:X8}";
            var form = FormOutput(pk.Species, pk.Form, out _);
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{isAlpha}{(Species)pk.Species}{form}{pid}{ec}{Environment.NewLine}Gender: {(Gender)pk.Gender}{Environment.NewLine}Nature: {GameInfo.GetStrings(1).Natures[pk.Nature]}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{msg}";
            LiveStats.Text = $"{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}PP";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            TextboxSetText(Program.Viewer.ViewBox, output);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            sprite = PokeImg(pk, false, false);
            PokeSprite.Load(sprite);
            string typing1 = pk.PersonalInfo.Type1 >= 10 ? $"{pk.PersonalInfo.Type1}" : $"0{pk.PersonalInfo.Type1}";
            string typing2 = pk.PersonalInfo.Type2 >= 10 ? $"{pk.PersonalInfo.Type2}" : $"0{pk.PersonalInfo.Type2}";
            var type1 = $"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.Misc/Resources/img/types/type_icon_{typing1}.png";
            var imgtype1 = DownloadRemoteImageFile(type1);
            Image imgt1;
            using (var ms1 = new MemoryStream(imgtype1))
            {
                imgt1 = Image.FromStream(ms1);
            }
            Typing1.Image = imgt1;
            if (pk.PersonalInfo.Type1 != pk.PersonalInfo.Type2)
            {
                var type2 = $"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.Misc/Resources/img/types/type_icon_{typing2}.png";
                var imgtype2 = DownloadRemoteImageFile(type2);
                Image imgt2;
                using (var ms2 = new MemoryStream(imgtype2))
                {
                    imgt2 = Image.FromStream(ms2);
                }
                Typing2.Image = imgt2;
            }
            if (alpha)
            {
                var url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_alpha.png";
                var img = DownloadRemoteImageFile(url);
                Image original;
                using (var ms = new MemoryStream(img))
                {
                    original = Image.FromStream(ms);
                }
                Specialty.Visible = true;
                Specialty.Image = original;
            }
            if (hasMark)
            {
                var url = $"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.Misc/Resources/img/ribbons/ribbonmark{msg.Replace($"{Environment.NewLine}Mark: ", "").ToLower()}.png";
                var img = DownloadRemoteImageFile(url);
                Image original;
                using (var ms = new MemoryStream(img))
                {
                    original = Image.FromStream(ms);
                }
                Specialty.Visible = true;
                Specialty.Image = original;
            }
            if (isGmax)
            {
                var url = $"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/dyna.png";
                var img = DownloadRemoteImageFile(url);
                Image original;
                using (var ms = new MemoryStream(img))
                {
                    original = Image.FromStream(ms);
                }
                Specialty.Visible = true;
                Specialty.Image = original;
            }
            if (RefreshStats.Checked)
            {
                var StartingHP = pk.Stat_HPCurrent;

                int refr = 0;
                int.TryParse(RefreshTime, out refr);
                ReadOverworld = true;
                while (!await IsOnOverworldTitle(CancellationToken.None).ConfigureAwait(false))
                {                    
                    if (ReadOverworld == false || pk.Stat_HPCurrent == 0)
                        break;

                    switch (GameType)
                    {
                        case (int)GameSelected.SW or (int)GameSelected.SH: pk = await ReadInBattlePokemonSWSH(offset2, size).ConfigureAwait(false); break;
                        case (int)GameSelected.BD or (int)GameSelected.SP: pk = await ReadInBattlePokemonBDSP(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LA: pk = await ReadInBattlePokemonLA(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LGP or (int)GameSelected.LGE: pk = await ReadInBattlePokemonLGPE(offset2, size).ConfigureAwait(false); break;
                    }
                    LiveStats.Text = $"{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}PP";
                    HpLabel.Text = $"HP - {(pk.Stat_HPCurrent / StartingHP) * 100}%";
                    await Task.Delay(refr, CancellationToken.None).ConfigureAwait(false); // Wait time between reads to account for battle turns
                }
                this.LiveStats.Clear();
                this.HpLabel.Text = "          HP%";
                TextboxSetText(Program.Viewer.ViewBox, "No Pokémon present.");
                sprite = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0000_000_uk_n_00000000_f_n.png";
                PokeSprite.Load(sprite);
                Typing1.Visible = false;
                Typing2.Visible = false;
                Specialty.Visible = false;
            }
        }

        public async Task<PK8> ReadInBattlePokemonSWSH(uint offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false); // WildPokemon
            var pk = new PK8(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await SwitchConnection.ReadBytesAsync(0x886A95B8, size, CancellationToken.None).ConfigureAwait(false); // RaidPokemon
                pk = new PK8(data);
                if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
                {
                    data = await SwitchConnection.ReadBytesAsync(0x886BC348, size, CancellationToken.None).ConfigureAwait(false); // LegendaryPokemon
                    pk = new PK8(data);
                }
            }
            return pk;
        }

        public async Task<PA8> ReadInBattlePokemonLA(ulong offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PA8(data);
            return pk;
        }

        public async Task<PB8> ReadInBattlePokemonBDSP(ulong offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PB8(data);
            return pk;
        }

        public async Task<PB7> ReadInBattlePokemonLGPE(uint offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PB7(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await SwitchConnection.ReadBytesMainAsync(0x163EDC0, size, CancellationToken.None).ConfigureAwait(false);
                pk = new PB7(data);
            }
            return pk;
        }

        protected async Task<(bool, ulong)> ValidatePointerAll(IEnumerable<long> jumps, CancellationToken token)
        {
            var solved = await SwitchConnection.PointerAll(jumps, token).ConfigureAwait(false);
            return (solved != 0, solved);
        }

        public async Task<bool> IsOnOverworldTitle(CancellationToken token)
        {
            var ptr = new long[] { 0 };
            if (GameType == (int)GameSelected.LA)
                ptr = new long[] { 0x42C30E8, 0x1A9 };
            if (GameType == (int)GameSelected.BD)
                ptr = new long[] { 0x4C59C98, 0xB8, 0x3C };
            if (GameType == (int)GameSelected.SP)
                ptr = new long[] { 0x4E70D70, 0xB8, 0x3C };
            if (GameType is (int)GameSelected.SW or (int)GameSelected.SH)
            {
                var data = await SwitchConnection.ReadBytesAsync((uint)(GameType == (int)GameVersion.SH ? 0x3F128626 : 0x3F128624), 1, token).ConfigureAwait(false);
                return data[0] == (GameType == (int)GameSelected.SH ? 0x40 : 0x41);
            }
            if (GameType == (int)GameSelected.LGP || GameType == (int)GameSelected.LGE)
            {
                var data = await SwitchConnection.ReadBytesMainAsync(0x163F694, 1, token).ConfigureAwait(false);
                return data[0] == 0;
            }
            var (valid, offset) = await ValidatePointerAll(ptr, token).ConfigureAwait(false);
            if (!valid)
                return false;
            return await IsOnOverworld(offset, token).ConfigureAwait(false);
        }

        public async Task<bool> IsOnOverworld(ulong offset, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, 1, token).ConfigureAwait(false);
            return data[0] == 1;
        }

        public static bool HasMark(IRibbonIndex pk, out RibbonIndex result)
        {
            result = default;
            for (var mark = RibbonIndex.MarkLunchtime; mark <= RibbonIndex.MarkSlump; mark++)
            {
                if (pk.GetRibbon((int)mark))
                {
                    result = mark;
                    return true;
                }
            }
            return false;
        }

        private async void ReadEncounter_ClickAsync(object sender, EventArgs e)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ChangeButtonState(Program.Viewer.View, false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (SwitchConnection.Connected)
            {
                TextboxSetText(Program.Viewer.ViewBox, "Reading encounter...");
                await Task.Delay(0_500).ConfigureAwait(false); // Give state reads time to complete                

                ulong ofs = 0;
                int size = 0;
                switch (GameType)
                {
                    case (int)GameSelected.SW or (int)GameSelected.SH:
                        {
                            uint ufs = 0x8FEA3648;
                            size = 0x158;
                            var pk =await ReadInBattlePokemonSWSH(ufs, size).ConfigureAwait(false);
                            FillPokeData(pk, 0, ufs, size);
                            break;
                        }
                    case (int)GameSelected.LA:
                        {
                            ofs = await ParsePointer("[[[[[main+42a6f00]+D0]+B8]+300]+70]+60]+98]+10]", CancellationToken.None).ConfigureAwait(false);
                            size = 0x168;
                            var pk = await ReadInBattlePokemonLA(ofs, size).ConfigureAwait(false);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.BD:
                        {
                            ofs = await ParsePointer("[[[[main+4C59EF0]+20]+98]]+20", CancellationToken.None).ConfigureAwait(false);
                            size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.SP:
                        {
                            ofs = await ParsePointer("[[[[main+4E70FC8]+20]+98]]+20", CancellationToken.None).ConfigureAwait(false);
                            size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.LGP or (int)GameSelected.LGE:
                        {
                            uint ufs = 0x9A118D68;
                            size = 0x158;
                            var pk = await ReadInBattlePokemonLGPE(ufs, size).ConfigureAwait(false);
                            FillPokeData(pk, ofs, ufs, size); 
                            break;
                        }
                }
            }
            ChangeButtonState(Program.Viewer.View, true);
        }

        private static byte[] DownloadRemoteImageFile(string uri)
        {
            byte[] content;
#pragma warning disable SYSLIB0014 // Type or member is obsolete
            var request = (HttpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

            using (var response = request.GetResponse())
            using (var reader = new BinaryReader(response.GetResponseStream()))
            {
                content = reader.ReadBytes(100000);
            }

            return content;
        }

        public static string FormOutput(int species, int form, out string[] formString)
        {
            var strings = GameInfo.GetStrings("en");
            formString = FormConverter.GetFormList(species, strings.Types, strings.forms, GameInfo.GenderSymbolASCII, typeof(PK8) == typeof(PK8) ? 8 : 4);
            if (formString.Length == 0)
                return string.Empty;

            formString[0] = "";
            if (form >= formString.Length)
                form = formString.Length - 1;
            return formString[form].Contains("-") ? formString[form] : formString[form] == "" ? "" : $"-{formString[form]}";
        }

        public static string PokeImg(PKM pkm, bool canGmax, bool fullSize)
        {
            bool md = false;
            bool fd = false;
            string[] baseLink;
            string newbase = string.Empty;
            string dimensions = "128x128";
            if (fullSize)
                baseLink = "https://raw.githubusercontent.com/zyro670/HomeImages/master/512x512/poke_capture_0001_000_mf_n_00000000_f_n.png".Split('_');
            else baseLink = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0001_000_mf_n_00000000_f_n.png".Split('_');
            if (fullSize)
                dimensions = "512x512";
            if (pkm.Species == (int)Species.Sneasel)
            {
                if (pkm.Form == 0)
                {
                    if (pkm.Gender == (int)Gender.Male)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_md_n_00000000_f_n.png";
                    if (pkm.Gender == (int)Gender.Male && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_md_n_00000000_f_r.png";
                    if (pkm.Gender == (int)Gender.Female)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_fd_n_00000000_f_n.png";
                    if (pkm.Gender == (int)Gender.Female && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_fd_n_00000000_f_r.png";
                }
                if (pkm.Form == 1)
                {
                    if (pkm.Gender == (int)Gender.Male)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_md_n_00000000_f_n.png";
                    if (pkm.Gender == (int)Gender.Male && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_md_n_00000000_f_r.png";
                    if (pkm.Gender == (int)Gender.Female)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_fd_n_00000000_f_n.png";
                    if (pkm.Gender == (int)Gender.Female && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_fd_n_00000000_f_r.png";
                }
                return newbase;
            }

            if (Enum.IsDefined(typeof(GenderDependent), pkm.Species) && !canGmax && pkm.Form == 0)
            {
                if (pkm.Gender == 0 && pkm.Species != (int)Species.Torchic)
                    md = true;
                else fd = true;
            }

            int form = pkm.Species switch
            {
                (int)Species.Sinistea or (int)Species.Polteageist or (int)Species.Rockruff or (int)Species.Mothim => 0,
                (int)Species.Alcremie when pkm.IsShiny || canGmax => 0,
                _ => pkm.Form,

            };

            baseLink[2] = pkm.Species < 10 ? $"000{pkm.Species}" : pkm.Species < 100 && pkm.Species > 9 ? $"00{pkm.Species}" : $"0{pkm.Species}";
            baseLink[3] = pkm.Form < 10 ? $"00{form}" : $"0{form}";
            baseLink[4] = pkm.PersonalInfo.OnlyFemale ? "fo" : pkm.PersonalInfo.OnlyMale ? "mo" : pkm.PersonalInfo.Genderless ? "uk" : fd ? "fd" : md ? "md" : "mf";
            baseLink[5] = canGmax ? "g" : "n";
            baseLink[6] = "0000000" + (pkm.Species == (int)Species.Alcremie && !canGmax ? pkm.Data[0xE4] : 0);
            baseLink[8] = pkm.IsShiny ? "r.png" : "n.png";
            return string.Join("_", baseLink);
        }

        private async void Window_Loaded()
        {
            var token = CancellationToken.None;
            string game = string.Empty;
            int type = 0;
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/icon_version/64x64/icon_version_";
            string title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            switch (title)
            {
                case LegendsArceusID: game = "Legends: Arceus"; url = url + "LA.png"; type = (int)GameSelected.LA;  break;
                case ShiningPearlID: game = "Shining Pearl"; url = url + "SP.png"; type = (int)GameSelected.SP; break;
                case BrilliantDiamondID: game = "Brilliant Diamond"; url = url + "BD.png"; type = (int)GameSelected.BD; break;
                case SwordID: game = "Sword"; url = url + "SW.png"; type = (int)GameSelected.SW; break;
                case ShieldID: game = "Shield"; url = url + "SH.png"; type = (int)GameSelected.SH; break;
                case EeveeID: game = "Let's Go Eevee"; url = url + "LGE.png"; ; type = (int)GameSelected.LGE; break;
                case PikachuID: game = "Let's Go Pikachu"; url = url + "LGP.png"; type = (int)GameSelected.LGP; break;
            }
            //if (title is not SwordID or ShieldID or BrilliantDiamondID or ShiningPearlID or LegendsArceusID)
            //  throw new Exception($"{title} is not a valid Pokemon title. Is the right game open?");

            OriginIcon.ImageLocation = url;            
            GameType = type;
            ViewBox.Text = "Click View!";
            var bg = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
            PokeSprite.ImageLocation = bg;
        }

        enum GameSelected
        {
            LGP = 0,
            LGE = 1,
            SW = 2,
            SH = 3,
            BD = 4,
            SP = 5,
            LA = 6,
            Scarlet = 7,
            Violet = 8,
        }

        public async Task<string> GetTitleID(CancellationToken token)
        {
            var bytes = await SwitchConnection.ReadRaw(SwitchCommand.GetTitleID(), 17, token).ConfigureAwait(false);
            return Encoding.ASCII.GetString(bytes).Trim();
        }

        public async Task<ulong> ParsePointer(string pointer, CancellationToken token, bool heaprealtive = false) //Code from LiveHex
        {
            var ptr = pointer;
            if (string.IsNullOrWhiteSpace(ptr) || ptr.IndexOfAny(new char[] { '-', '/', '*' }) != -1)
                return 0;
            while (ptr.Contains("]]"))
                ptr = ptr.Replace("]]", "]+0]");
            uint? finadd = null;
            if (!ptr.EndsWith("]"))
            {
                finadd = Util.GetHexValue(ptr.Split('+').Last());
                ptr = ptr.Substring(0, ptr.LastIndexOf('+'));
            }
            var jumps = ptr.Replace("main", "").Replace("[", "").Replace("]", "").Split(new[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            if (jumps.Length == 0)
                return 0;

            var initaddress = Util.GetHexValue(jumps[0].Trim());
            ulong address = BitConverter.ToUInt64(await SwitchConnection.ReadBytesMainAsync(initaddress, 0x8, token).ConfigureAwait(false), 0);
            foreach (var j in jumps)
            {
                var val = Util.GetHexValue(j.Trim());
                if (val == initaddress)
                    continue;
                address = BitConverter.ToUInt64(await SwitchConnection.ReadBytesAbsoluteAsync(address + val, 0x8, token).ConfigureAwait(false), 0);
            }
            if (finadd != null) address += (ulong)finadd;
            if (heaprealtive)
            {
                ulong heap = await SwitchConnection.GetHeapBaseAsync(token);
                address -= heap;
            }
            return address;
        }

        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        }

        private void CaptureWindow_Click(object sender, EventArgs e)
        {
            this.WindowCapture.Visible = false;
            Bitmap FormScreenShot = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(FormScreenShot);
            G.CopyFromScreen(this.Location, new Point(0, 0), this.Size);
            Clipboard.SetImage(FormScreenShot);
            this.WindowCapture.Visible = true;
        }

        private void RefreshStats_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chextBox = (CheckBox)sender;
            if (chextBox.Checked)            
                Properties.Settings.Default.RefreshStats = true;            
            else
                Properties.Settings.Default.RefreshStats = false;

            Properties.Settings.Default.Save();
        }
    }
}