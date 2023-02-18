using PKHeX.Core;
using PKHeX.Drawing.Misc;
using PokeViewer.NET.SubForms;
using PokeViewer.NET.WideViewForms;
using SysBot.Base;
using System.Net.Sockets;
using static PokeViewer.NET.RoutineExecutor;
using static PokeViewer.NET.ViewerUtil;
using Octokit;
using System.Diagnostics;

namespace PokeViewer.NET
{
    public partial class MainViewer : Form
    {
        private static readonly SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        private const string ViewerVersion = "1.0.1";
        public MainViewer()
        {
            InitializeComponent();
            DisableTabsOnStart();
            VersionLabel.Text = $"v{ViewerVersion}";
            CheckLinkLabel();
        }

        private int GameType;
        private readonly string RefreshTime = Properties.Settings.Default.RefreshRate;

        private void PokeViewerForm_Load(object sender, EventArgs e)
        {
            SwitchIP.Text = Properties.Settings.Default.SwitchIP;
            View.Visible = false;
            ViewBox.Visible = false;
            PokeSprite.Visible = false;
            LiveStats.Visible = false;
            RefreshStats.Visible = false;
            HidePIDEC.Visible = false;
            HpLabel.Visible = false;
            UniqueBox.Visible = false;
            UniqueBox2.Visible = false;
            LoadOriginDefault();
            LoadDateTime(sender, e);
        }

        private async void CheckLinkLabel()
        {
            //Get all releases from GitHub
            //Source: https://octokitnet.readthedocs.io/en/latest/getting-started/
            GitHubClient client = new(new ProductHeaderValue("Poke-Viewer-NET"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("zyro670", "PokeViewer.NET");

            //Setup the versions
            Version latestGitHubVersion = new(releases[0].TagName);
            Version localVersion = new(ViewerVersion); 

            //Compare the Versions
            //Source: https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
            int versionComparison = localVersion.CompareTo(latestGitHubVersion);
            if (versionComparison < 0)
            {
                //The version on GitHub is more up to date than this local release.
                linkLabel1.Text = "An update is available!";
                linkLabel1.SetBounds(40, 160, linkLabel1.Width, linkLabel1.Height);

            }
            else if (versionComparison > 0)
            {
                //This local version is greater than the release version on GitHub.
                linkLabel1.Text = "You are on an azure-artifact build.";
                linkLabel1.SetBounds(8, 160, linkLabel1.Width, linkLabel1.Height);
            }
            else
            {
                //This local Version and the Version on GitHub are equal.
                linkLabel1.Text = "You are on the latest release.";
                linkLabel1.SetBounds(24, 160, linkLabel1.Width, linkLabel1.Height);
            }
        }

        private void LoadOriginDefault()
        {
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
            OriginIcon.ImageLocation = url;
            ConnectionSpriteBox.ImageLocation = url;
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

        private void CheckForUSBChecked(object sender, EventArgs e)
        {
            if (ToggleSwitchProtocol.Checked)
                ConnectionGroupBox.Text = "Switch Port";
            else
                ConnectionGroupBox.Text = "Switch IP";

        }

        private void LoadDateTime(object sender, EventArgs e)
        {
            TodaysDate.Text = "Met Date: " + DateTime.Today.ToString("MM/dd/yyyy");
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (!SwitchConnection.Connected)
            {
                try
                {
                    SwitchConnection.Connect();
                    Height = 507;
                    Width = 511;
                    ConnectionGroupBox.SetBounds(135, 130, ConnectionGroupBox.Width, ConnectionGroupBox.Height);
                    EnableTabs();
                    Connect.Text = "Disconnect";
                    View.Enabled = true;
                    SwitchIP.Enabled = false;
                    ViewBox.Visible = true;
                    PokeSprite.Visible = true;
                    LiveStats.Visible = true;
                    RefreshStats.Visible = true;
                    Refresh.Visible = true;
                    RefreshBox.Visible = true;
                    InGameScreenshot.Visible = true;
                    HidePIDEC.Visible = true;
                    HpLabel.Visible = true;
                    View.Visible = true;
                    ToggleSwitchProtocol.Enabled = false;
                    Window_Loaded();
                }
                catch (SocketException err)
                {
                    MessageBox.Show(err.Message);
                    MessageBox.Show($"{Environment.NewLine}Ensure IP address is correct before connecting!");
                }
            }
            else if (SwitchConnection.Connected)
            {
                SwitchConnection.Disconnect();
                SwitchConnection.Reset();
                DialogResult dialogResult = MessageBox.Show("Close Application?", "Disconnecting PokeViewer.NET", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    System.Windows.Forms.Application.Restart();
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    this.Close();
                }
            }

        }

        private void View_Click(object sender, EventArgs e)
        {
            ReadEncounter_ClickAsync(sender, e);
        }

        private async void FillPokeData(PKM pk, ulong offset, uint offset2, int size)
        {
            Specialty.Visible = false;
            var sprite = string.Empty;
            bool isValid = false;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: isValid = PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield: isValid = PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: isValid = PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LegendsArceus:
                    {
                        isValid = PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form);
                        if (!isValid)
                        {
                            if ((Species)pk.Species is Species.Decidueye or Species.Typhlosion or Species.Samurott or Species.Qwilfish or Species.Lilligant or Species.Sliggoo or Species.Goodra
                            or Species.Growlithe or Species.Arcanine or Species.Voltorb or Species.Electrode or Species.Sneasel or Species.Avalugg or Species.Zorua or Species.Zoroark or Species.Braviary)
                                isValid = true;
                        }
                        break;
                    }
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
            }
            if (!isValid || pk.Species < 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                ViewBox.Text = "No Pokémon present.";
                View.Enabled = true;
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                PokeSprite.Load(sprite);
                Typing1.Visible = false;
                Typing2.Visible = false;
                Specialty.Visible = false;
                LiveStats.Clear();
                return;
            }
            Typing1.Visible = true;
            Typing2.Visible = true;
            bool alpha = pk is PA8 pa8 ? pa8.IsAlpha : false;
            bool hasMark = false;
            bool isGmax = pk is PK8 pk8 ? pk8.CanGigantamax : false;
            string msg = "";
            if (pk is PK8)
            {
                hasMark = HasMark((PK8)pk, out RibbonIndex mark);
                msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
            }
            if (pk is PK8 && UniqueBox2.Checked)
                msg = $"{Environment.NewLine}Mark: Curry";

            string isAlpha = alpha ? $"αlpha - " : "";
            string pid = HidePIDEC.Checked ? "" : $"{Environment.NewLine}PID: {pk.PID:X8}";
            string ec = HidePIDEC.Checked ? "" : $"{Environment.NewLine}EC: {pk.EncryptionConstant:X8}";
            var form = FormOutput(pk.Species, pk.Form, out _);
            string gender = string.Empty;
            switch (pk.Gender)
            {
                case 0: gender = " (M)"; break;
                case 1: gender = " (F)"; break;
                case 2: break;
            }
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{isAlpha}{(Species)pk.Species}{form}{gender}{ec}{pid}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{msg}";
            LiveStats.Text = $"{(Move)pk.Move1} - {pk.Move1_PP}PP{Environment.NewLine}{(Move)pk.Move2} - {pk.Move2_PP}PP{Environment.NewLine}{(Move)pk.Move3} - {pk.Move3_PP}PP{Environment.NewLine}{(Move)pk.Move4} - {pk.Move4_PP}PP";
            ViewBox.Text = output;
            sprite = PokeImg(pk, isGmax);
            PokeSprite.Load(sprite);
            var imgt1 = TypeSpriteUtil.GetTypeSpriteGem(pk.PersonalInfo.Type1);
            Typing1.Image = imgt1;
            if (pk.PersonalInfo.Type1 != pk.PersonalInfo.Type2)
            {
                var imgt2 = TypeSpriteUtil.GetTypeSpriteGem(pk.PersonalInfo.Type2);
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
                var info = RibbonInfo.GetRibbonInfo(pk);
                foreach (var rib in info)
                {
                    if (!rib.HasRibbon)
                        continue;

                    var mimg = RibbonSpriteUtil.GetRibbonSprite(rib.Name);
                    if (mimg is not null)
                    {
                        Specialty.Visible = true;
                        Specialty.Image = mimg;
                    }
                }
            }
            if (pk is PK8 & UniqueBox2.Checked)
            {
                var mimg = RibbonSpriteUtil.GetRibbonSprite("RibbonMarkCurry");
                if (mimg is not null)
                {
                    Specialty.Visible = true;
                    Specialty.Image = mimg;
                }
            }
            if (isGmax)
            {
                var url = $"https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_daimax.png";
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
                int.TryParse(RefreshTime, out var refr);
                while (pk.Stat_HPCurrent != 0)
                {
                    if (!SwitchConnection.Connected)
                    {
                        System.Windows.Forms.Application.Restart();
                        Environment.Exit(0);
                    }
                    switch (GameType)
                    {
                        case (int)GameSelected.Scarlet or (int)GameSelected.Violet: pk = await ReadInBattlePokemonSV(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.Sword or (int)GameSelected.Shield: pk = await ReadInBattlePokemonSWSH(offset2, size).ConfigureAwait(false); break;
                        case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: pk = await ReadInBattlePokemonBDSP(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LegendsArceus: pk = await ReadInBattlePokemonLA(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: pk = await ReadInBattlePokemonLGPE(offset2, size).ConfigureAwait(false); break;
                    }
                    LiveStats.Text = $"{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}PP";
                    HpLabel.Text = $"HP - {(pk.Stat_HPCurrent / StartingHP) * 100}%";
                    await Task.Delay(refr, CancellationToken.None).ConfigureAwait(false); // Wait time between reads
                }
                LiveStats.Clear();
                HpLabel.Text = "          HP%";
                ViewBox.Text = "No Pokémon present.";
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                PokeSprite.Load(sprite);
                if (pk is PK9)
                {
                    PokeSprite.SizeMode = PictureBoxSizeMode.AutoSize;
                }
                Typing1.Visible = false;
                Typing1.Image = null;
                Typing2.Visible = false;
                Typing2.Image = null;
                Specialty.Visible = false;
                Specialty.Image = null;
            }
            View.Enabled = true;
        }

        public async Task<PK9> ReadInBattlePokemonSV(ulong offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PK9(data);
            if (UniqueBox.Checked)
            {
                var ptr = new long[] { 0x42F3DD8, 0xD8, 0x48, 0x18, 0xD8, 0x1E0 };
                var ofs = await SwitchConnection.PointerAll(ptr, CancellationToken.None).ConfigureAwait(false);
                data = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, size, CancellationToken.None).ConfigureAwait(false); // RaidLobbyPokemon
                pk = new PK9(data);
                return pk;
            }
            return pk;
        }

        public async Task<PK8> ReadInBattlePokemonSWSH(uint offset, int size)
        {
            var token = CancellationToken.None;
            byte[]? data = { 0 };
            PK8 pk = new();
            if (UniqueBox.Checked)
            {
                offset = 0x886A95B8;
                data = await SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false); // RaidPokemon
                pk = new PK8(data);
                return pk;
            }
            if (UniqueBox2.Checked)
            {
                IReadOnlyList<long>[] campers =
                {
                    new long[] { 0x2636120, 0x280, 0xD8, 0x78, 0x10, 0x98, 0x00 },
                    new long[] { 0x2636170, 0x2F0, 0x58, 0x130, 0x138, 0xD0 },
                    new long[] { 0x28ED668, 0x68, 0x1E8, 0x1D0, 0x128 },
                    new long[] { 0x296C030, 0x60, 0x40, 0x1B0, 0x58, 0x00 }
                };

                for (int i = 0; i < campers.Length; i++)
                {
                    var pointer = campers[i];
                    var ofs = await SwitchConnection.PointerAll(pointer, token).ConfigureAwait(false);
                    data = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, size, token).ConfigureAwait(false);
                    pk = new PK8(data);
                    if (pk.Species != 0 && pk.Species < (int)Species.MAX_COUNT)
                        return pk;
                }
            }
            data = await SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false); // WildPokemon
            pk = new PK8(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await SwitchConnection.ReadBytesAsync(0x886BC348, size, token).ConfigureAwait(false); // LegendaryPokemon
                pk = new PK8(data);
            }
            return pk;
        }

        public async Task<PA8> ReadInBattlePokemonLA(ulong offset, int size)
        {
            var token = CancellationToken.None;
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PA8(data);
            return pk;
        }

        public async Task<PB8> ReadInBattlePokemonBDSP(ulong offset, int size)
        {
            var token = CancellationToken.None;
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB8(data);
            return pk;
        }

        public async Task<PB7> ReadInBattlePokemonLGPE(uint offset, int size)
        {
            var token = CancellationToken.None;
            var data = await SwitchConnection.ReadBytesMainAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB7(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await SwitchConnection.ReadBytesAsync(0x9A118D68, size, token).ConfigureAwait(false);
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
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    ptr = new long[] { 0x43A7848, 0x348, 0x10, 0xD8, 0x28 }; break;
                case (int)GameSelected.LegendsArceus:
                    ptr = new long[] { 0x42C30E8, 0x1A9 }; break;
                case (int)GameSelected.BrilliantDiamond:
                    ptr = new long[] { 0x4C59C98, 0xB8, 0x3C }; break;
                case (int)GameSelected.ShiningPearl:
                    ptr = new long[] { 0x4E70D70, 0xB8, 0x3C }; break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield:
                    {
                        var data = await SwitchConnection.ReadBytesAsync((uint)(GameType == (int)GameVersion.SH ? 0x3F128626 : 0x3F128624), 1, token).ConfigureAwait(false);
                        return data[0] == (GameType == (int)GameSelected.Shield ? 0x40 : 0x41);
                    }
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                    {
                        var data = await SwitchConnection.ReadBytesMainAsync(0x163F694, 1, token).ConfigureAwait(false);
                        return data[0] == 0;
                    }
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

        private void SanityCheck(PKM pk)
        {
            bool isValid = false;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: isValid = PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield: isValid = PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: isValid = PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LegendsArceus: isValid = PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
            }
            if (!isValid || pk.Species < 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                ViewBox.Text = "No Pokémon present.";
                View.Enabled = true;
                var sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                PokeSprite.Load(sprite);
                Typing1.Visible = false;
                Typing2.Visible = false;
                Specialty.Visible = false;
                LiveStats.Clear();
                return;
            }
        }
        private async void ReadEncounter_ClickAsync(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            View.Enabled = false;
            if (SwitchConnection.Connected)
            {
                ViewBox.Text = "Reading encounter...";
                switch (GameType)
                {
                    case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                        {
                            var ptr = new long[] { 0x42FD3C0, 0x10, 0x2D0, 0x2A0, 0x48, 0x2E0 };
                            var ofs = await SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x158;
                            var pk = await ReadInBattlePokemonSV(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                            return;
                        }
                    case (int)GameSelected.Sword or (int)GameSelected.Shield:
                        {
                            if (UniqueBox.Checked && UniqueBox2.Checked)
                            {
                                MessageBox.Show("You have both unique boxes checked! Please select only one!");
                                System.Media.SystemSounds.Beep.Play();
                                View.Enabled = true;
                                UniqueBox.Checked = false;
                                UniqueBox2.Checked = false;
                                ViewBox.Text = "Click View!";
                                return;
                            }
                            uint ufs = 0x8FEA3648;
                            int size = 0x158;
                            var pk = await ReadInBattlePokemonSWSH(ufs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, 0, ufs, size);
                            break;
                        }
                    case (int)GameSelected.LegendsArceus:
                        {
                            var ptr = new long[] { 0x42A6F00, 0xD0, 0xB8, 0x300, 0x70, 0x60, 0x98, 0x10, 0x00 };
                            var ofs = await SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonLA(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.BrilliantDiamond:
                        {
                            var ptr = new long[] { 0x4C59EF0, 0x20, 0x98, 0x00, 0x20 };
                            var ofs = await SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.ShiningPearl:
                        {
                            var ptr = new long[] { 0x4E70FC8, 0x20, 0x98, 0x00, 0x20 };
                            var ofs = await SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            int size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                        {
                            uint ufs = 0x163EDC0;
                            int size = 0x158;
                            var pk = await ReadInBattlePokemonLGPE(ufs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, 0, ufs, size);
                            break;
                        }
                }
            }
        }

        private async void Window_Loaded()
        {
            var token = CancellationToken.None;
            int type = 0;
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/icon_version/64x64/icon_version_";
            string title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            switch (title)
            {
                case ScarletID:
                    {
                        url += "SC.png";
                        type = (int)GameSelected.Scarlet;
                        View.Enabled = false;
                        UniqueBox.Visible = true;
                        UniqueBox.Text = "Raid Lobby";
                        UniqueBox.Enabled = false;
                        break;
                    }
                case VioletID:
                    {
                        url += "VI.png";
                        type = (int)GameSelected.Violet;
                        View.Enabled = false;
                        UniqueBox.Visible = true;
                        UniqueBox.Text = "Raid Lobby";
                        UniqueBox.Enabled = false;
                        break;
                    }
                case LegendsArceusID:
                    {
                        url += "LA.png";
                        type = (int)GameSelected.LegendsArceus;
                        break;
                    }
                case ShiningPearlID:
                    {
                        url += "SP.png";
                        type = (int)GameSelected.ShiningPearl;
                        break;
                    }
                case BrilliantDiamondID:
                    {
                        url += "BD.png";
                        type = (int)GameSelected.BrilliantDiamond;
                        break;
                    }
                case SwordID:
                    {
                        url += "SW.png";
                        type = (int)GameSelected.Sword;
                        UniqueBox.Visible = true; UniqueBox2.Visible = true;
                        UniqueBox.Text = "Raid"; UniqueBox2.Text = "Curry";
                        break;
                    }
                case ShieldID:
                    {
                        url += "SH.png";
                        type = (int)GameSelected.Shield;
                        UniqueBox.Visible = true; UniqueBox2.Visible = true;
                        UniqueBox.Text = "Raid"; UniqueBox2.Text = "Curry";
                        break;
                    }
                case EeveeID:
                    {
                        url += "LGE.png"; type = (int)GameSelected.LetsGoEevee;
                        break;
                    }
                case PikachuID:
                    {
                        url += "LGP.png"; type = (int)GameSelected.LetsGoPikachu;
                        break;
                    }
            }

            OriginIcon.ImageLocation = url;
            ConnectionSpriteBox.ImageLocation = url;
            GameType = type;
            ViewBox.Text = "Click View!";
            var bg = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
            PokeSprite.ImageLocation = bg;
        }

        private void CaptureWindow_Click(object sender, EventArgs e)
        {
            Bitmap FormScreenShot = new(Width, Height);
            Graphics G = Graphics.FromImage(FormScreenShot);
            G.CopyFromScreen(Location, new Point(0, 0), Size);
            Clipboard.SetImage(FormScreenShot);
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

        private void InGameScreenshot_Click(object sender, EventArgs e)
        {
            var fn = "screenshot.jpg";
            if (!SwitchConnection.Connected)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show($"No device connected! In-Game Screenshot not possible!");
                return;
            }
            var bytes = SwitchConnection.Screengrab(CancellationToken.None).Result;
            File.WriteAllBytes(fn, bytes);
            FileStream stream = new(fn, System.IO.FileMode.Open);
            var img = Image.FromStream(stream);
            Clipboard.SetImage(img);
            using (Form form = new())
            {
                Bitmap vimg = (Bitmap)img;

                form.StartPosition = FormStartPosition.CenterScreen;

                Bitmap original = vimg;
                Bitmap resized = new(original, new Size(original.Width / 2, original.Height / 2));

                PictureBox pb = new()
                {
                    Dock = DockStyle.Fill,
                    Image = resized
                };
                form.Size = resized.Size;

                form.Controls.Add(pb);
                form.ShowDialog();
            }
            stream.Dispose();
            File.Delete(fn);
        }

        private void ViewerControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SwitchConnection.Connected)
                return;
            if (ViewerControl.SelectedTab.Text is "Raid 🎉")
            {
                if (GameType is (int)GameSelected.Scarlet or (int)GameSelected.Violet)
                {
                    RaidCodeEntry RaidForm = new(SwitchConnection);
                    RaidForm.ShowDialog();
                    return;
                }
                else
                {
                    MessageBox.Show($"Raid View currently not supported for {(GameSelected)GameType}.");
                    ViewerControl.TabPages.Remove(ViewerControl.SelectedTab);
                    return;
                }

            }

            Form form = new();
            string currentTab = ViewerControl.SelectedTab.Text;
            if (currentTab is not "Wide 🔭" && GameType is (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl or (int)GameSelected.Sword or (int)GameSelected.Shield)
            {
                ViewerControl.Height = 550;
                ViewerControl.Width = 511;
                Height = 507;
                Width = 511;
            }
            switch (currentTab)
            {
                case "Connection 🔌": return;
                case "View 🔎":
                    switch (GameType)
                    {
                        case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                            {
                                MessageBox.Show($"View currently not supported for {(GameSelected)GameType}.");
                                ViewerControl.TabPages.Remove(ViewerControl.SelectedTab);
                                return;
                            }
                        case (int)GameSelected.LegendsArceus or (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl or (int)GameSelected.Sword or (int)GameSelected.Shield or (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                            return;
                    }
                    break;                    
                case "Box 📦": form = new BoxViewerMode(GameType, SwitchConnection) { TopLevel = false }; break;
                case "Egg 🥚":
                    switch (GameType)
                    {
                        case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                            {
                                form = new Egg_Viewer(SwitchConnection) { TopLevel = false }; break;
                            }
                        case (int)GameSelected.LegendsArceus or (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl or (int)GameSelected.Sword or (int)GameSelected.Shield or (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                            {
                                MessageBox.Show($"Egg View currently not supported for {(GameSelected)GameType}.");
                                ViewerControl.TabPages.Remove(ViewerControl.SelectedTab);
                                return;
                            }
                    }
                    break;
                case "Wide 🔭":
                    switch (GameType)
                    {
                        case (int)GameSelected.Scarlet or (int)GameSelected.Violet or (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                            {
                                MessageBox.Show($"Wide View currently not supported for {(GameSelected)GameType}.");
                                ViewerControl.TabPages.Remove(ViewerControl.SelectedTab);
                                return;
                            }
                        case (int)GameSelected.LegendsArceus:
                            {
                                form = new WideViewerLA(SwitchConnection) { TopLevel = false }; 
                                ViewerControl.Height = 380;
                                ViewerControl.Width = 780;
                                Height = 400;
                                Width = 800;
                                break;
                            }
                        case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl:
                            {
                                form = new WideViewerBDSP(SwitchConnection) { TopLevel = false };
                                ViewerControl.Height = 560;
                                ViewerControl.Width = 825;
                                Height = 600;
                                Width = 850; 
                                break;
                            }
                        case (int)GameSelected.Sword or (int)GameSelected.Shield:
                            {
                                form = new WideViewerSWSH(SwitchConnection) { TopLevel = false };
                                ViewerControl.Height = 600;
                                ViewerControl.Width = 890;
                                Height = 580;
                                Width = 870; 
                                break;
                            }
                    }
                    break;
                case "NPC 🤖":
                    switch (GameType)
                    {
                        case (int)GameSelected.Scarlet or (int)GameSelected.Violet or (int)GameSelected.LegendsArceus or (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl:
                            {
                                MessageBox.Show($"NPC View currently not supported for {(GameSelected)GameType}.");
                                ViewerControl.TabPages.Remove(ViewerControl.SelectedTab);
                                return;
                            }
                        case (int)GameSelected.Sword or (int)GameSelected.Shield or (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                            {
                                form = new NPCViewer(GameType, SwitchConnection) { TopLevel = false }; break;
                            }
                    }
                    break;
            }
            int curr = ViewerControl.SelectedIndex;
            TabPage tbp = ViewerControl.TabPages[curr];
            ViewerControl.TabPages.Contains(tbp);
            if (!tbp.Controls.Contains(form))
                tbp.Controls.Add(form);
            form.WindowState = FormWindowState.Maximized;
            form.ControlBox = false;
            form.Show();
            Refresh();

        }

        private void DisableTabsOnStart()
        {
            ViewerControl.TabPages.Remove(tabPage2);
            ViewerControl.TabPages.Remove(tabPage3);
            ViewerControl.TabPages.Remove(tabPage4);
            ViewerControl.TabPages.Remove(tabPage5);
            ViewerControl.TabPages.Remove(tabPage6);
            ViewerControl.TabPages.Remove(tabPage7);

        }

        private void EnableTabs()
        {
            ViewerControl.TabPages.Add(tabPage2);
            ViewerControl.TabPages.Add(tabPage3);
            ViewerControl.TabPages.Add(tabPage4);
            ViewerControl.TabPages.Add(tabPage5);
            ViewerControl.TabPages.Add(tabPage6);
            ViewerControl.TabPages.Add(tabPage7);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/zyro670/PokeViewer.NET/releases") { UseShellExecute = true });
        }
    }
}