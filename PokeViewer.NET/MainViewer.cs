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
using PokeViewer.NET.Properties;

namespace PokeViewer.NET
{
    public partial class MainViewer : Form
    {
        public ViewerExecutor Executor = null!;
        private const string ViewerVersion = "1.2.0";
        private const int AzureBuildID = 371;
        private bool[] FormLoaded = new bool[7];
        private int GameType;
        private readonly string RefreshTime = Settings.Default.RefreshRate;
        public MainViewer()
        {
            InitializeComponent();
            DisableTabsOnStart();
            VersionLabel.Text = $"v{ViewerVersion}";
            CheckReleaseLabel();
        }

        private void PokeViewerForm_Load(object sender, EventArgs e)
        {
            SwitchIP.Text = Settings.Default.SwitchIP;
            LoadOriginDefault();
            LoadDateTime(sender, e);
        }

        private static void GetVersionsOnStart(int azurematch)
        {
            if (azurematch < 0)
            {
                DialogResult dialogResult = MessageBox.Show("A new azure-artifact build is available. Go to artifacts page?", "An update is available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo("https://dev.azure.com/angelosalas670/zyro670/_build?definitionId=5") { UseShellExecute = true });

                else if (dialogResult == DialogResult.No)
                    return;
            }
        }

        private static async Task CheckAzureLabel()
        {
            int azurematch;
            string latestazure = "https://dev.azure.com/angelosalas670/zyro670/_apis/build/builds?definitions=5&$top=1&api-version=5.0-preview.5";
            HttpClient client = new();
            var content = await client.GetStringAsync(latestazure);
            int buildId = int.Parse(content.Substring(140, 3));
            azurematch = AzureBuildID.CompareTo(buildId);
            GetVersionsOnStart(azurematch);
        }

        private async void CheckReleaseLabel()
        {
            GitHubClient client = new(new ProductHeaderValue("PokeViewer.NET"));
            Release releases = await client.Repository.Release.GetLatest("zyro670", "PokeViewer.NET");

            Version latestGitHubVersion = new(releases.TagName);
            Version localVersion = new(ViewerVersion);

            int versionComparison = localVersion.CompareTo(latestGitHubVersion);
            if (versionComparison < 0)
            {
                //The version on GitHub is more up to date than this local release.
                DialogResult dialogResult = MessageBox.Show("A new PokeViewer.NET release is available. Go to Releases page?", "An update is available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo("https://github.com/zyro670/PokeViewer.NET/releases") { UseShellExecute = true });
            }
            await CheckAzureLabel().ConfigureAwait(false);
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
                Settings.Default.SwitchIP = textBox.Text;
            }
            Settings.Default.Save();
        }

        private void CheckForHide(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                Settings.Default.HidePIDEC = HidePIDEC.Checked;
                HidePIDEC.Checked = true;
            }
            else
            {
                Settings.Default.HidePIDEC = false;
                HidePIDEC.Checked = false;
            }
            Settings.Default.Save();
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

        private SwitchProtocol GetProtocol()
        {
            if (ToggleSwitchProtocol.Checked)
                return SwitchProtocol.USB;
            return SwitchProtocol.WiFi;
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            var token = new CancellationToken();
            if (Executor is null || !Executor.Connection.Connected)
            {
                try
                {
                    var config = GetProtocol() switch
                    {
                        SwitchProtocol.USB => new SwitchConnectionConfig { Port = int.Parse(Settings.Default.SwitchIP), Protocol = SwitchProtocol.USB },
                        SwitchProtocol.WiFi => new SwitchConnectionConfig { IP = Settings.Default.SwitchIP, Port = 6000, Protocol = SwitchProtocol.WiFi },
                        _ => throw new ArgumentOutOfRangeException(),
                    };
                    var state = new ViewerState
                    {
                        Connection = config,
                        InitialRoutine = RoutineType.Read,
                    };
                    Executor = new ViewerExecutor(state);
                    await Executor.RunAsync(token).ConfigureAwait(false);
                    Height = 507;
                    Width = 511;
                    ConnectionGroupBox.SetBounds(135, 130, ConnectionGroupBox.Width, ConnectionGroupBox.Height);
                    Connect.Text = "Disconnect";
                    await Executor.Connect(token).ConfigureAwait(false);
                    Window_Loaded();
                }
                catch (SocketException err)
                {
                    string port = ToggleSwitchProtocol.Checked ? "Port" : "IP address";
                    MessageBox.Show($"{err.Message}{Environment.NewLine}Ensure {port} is correct before attempting to connect!");
                }
            }
            else if (Executor is not null)
            {
                Executor.Connection.Disconnect();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void View_Click(object sender, EventArgs e)
        {
            ReadEncounter_ClickAsync(sender, e);
        }

        private async void FillPokeData(PKM pk, ulong offset, uint offset2, int size)
        {
            var token = CancellationToken.None;
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
                    if (!Executor.SwitchConnection.Connected)
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
                    await Task.Delay(refr, token).ConfigureAwait(false); // Wait time between reads
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
            var token = CancellationToken.None;
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PK9(data);
            if (UniqueBox.Checked)
            {
                var ptr = new long[] { 0x42F3DD8, 0xD8, 0x48, 0x18, 0xD8, 0x1E0 };
                var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(ofs, size, token).ConfigureAwait(false); // RaidLobbyPokemon
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
                data = await Executor.SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false); // RaidPokemon
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
                    var ofs = await Executor.SwitchConnection.PointerAll(pointer, token).ConfigureAwait(false);
                    data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(ofs, size, token).ConfigureAwait(false);
                    pk = new PK8(data);
                    if (pk.Species != 0 && pk.Species < (int)Species.MAX_COUNT)
                        return pk;
                }
            }
            data = await Executor.SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false); // WildPokemon
            pk = new PK8(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await Executor.SwitchConnection.ReadBytesAsync(0x886BC348, size, token).ConfigureAwait(false); // LegendaryPokemon
                pk = new PK8(data);
            }
            return pk;
        }

        public async Task<PA8> ReadInBattlePokemonLA(ulong offset, int size)
        {
            var token = CancellationToken.None;
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PA8(data);
            return pk;
        }

        public async Task<PB8> ReadInBattlePokemonBDSP(ulong offset, int size)
        {
            var token = CancellationToken.None;
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB8(data);
            return pk;
        }

        public async Task<PB7> ReadInBattlePokemonLGPE(uint offset, int size)
        {
            var token = CancellationToken.None;
            var data = await Executor.SwitchConnection.ReadBytesMainAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB7(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await Executor.SwitchConnection.ReadBytesAsync(0x9A118D68, size, token).ConfigureAwait(false);
                pk = new PB7(data);
            }
            return pk;
        }

        protected async Task<(bool, ulong)> ValidatePointerAll(IEnumerable<long> jumps, CancellationToken token)
        {
            var solved = await Executor.SwitchConnection.PointerAll(jumps, token).ConfigureAwait(false);
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
                        var data = await Executor.SwitchConnection.ReadBytesAsync((uint)(GameType == (int)GameVersion.SH ? 0x3F128626 : 0x3F128624), 1, token).ConfigureAwait(false);
                        return data[0] == (GameType == (int)GameSelected.Shield ? 0x40 : 0x41);
                    }
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                    {
                        var data = await Executor.SwitchConnection.ReadBytesMainAsync(0x163F694, 1, token).ConfigureAwait(false);
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
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, 1, token).ConfigureAwait(false);
            return data[0] == 1;
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
            if (Executor.SwitchConnection.Connected)
            {
                ViewBox.Text = "Reading encounter...";
                switch (GameType)
                {
                    case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                        {
                            var ptr = new long[] { 0x42FD3C0, 0x10, 0x2D0, 0x2A0, 0x48, 0x2E0 };
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
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
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonLA(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.BrilliantDiamond:
                        {
                            var ptr = new long[] { 0x4C59EF0, 0x20, 0x98, 0x00, 0x20 };
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.ShiningPearl:
                        {
                            var ptr = new long[] { 0x4E70FC8, 0x20, 0x98, 0x00, 0x20 };
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
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
            string title = await Executor.SwitchConnection.GetTitleID(token).ConfigureAwait(false);
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
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(EggPage);
                            ViewerControl.TabPages.Add(RaidPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
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
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(EggPage);
                            ViewerControl.TabPages.Add(RaidPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case LegendsArceusID:
                    {
                        url += "LA.png";
                        type = (int)GameSelected.LegendsArceus;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case ShiningPearlID:
                    {
                        url += "SP.png";
                        type = (int)GameSelected.ShiningPearl;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case BrilliantDiamondID:
                    {
                        url += "BD.png";
                        type = (int)GameSelected.BrilliantDiamond;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case SwordID:
                    {
                        url += "SW.png";
                        type = (int)GameSelected.Sword;
                        UniqueBox.Visible = true; UniqueBox2.Visible = true;
                        UniqueBox.Text = "Raid"; UniqueBox2.Text = "Curry";
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case ShieldID:
                    {
                        url += "SH.png";
                        type = (int)GameSelected.Shield;
                        UniqueBox.Visible = true; UniqueBox2.Visible = true;
                        UniqueBox.Text = "Raid"; UniqueBox2.Text = "Curry";
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case EeveeID:
                    {
                        url += "LGE.png"; type = (int)GameSelected.LetsGoEevee;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
                        break;
                    }
                case PikachuID:
                    {
                        url += "LGP.png"; type = (int)GameSelected.LetsGoPikachu;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(WidePage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                        });
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
            if (Executor is null)
                return;

            if (ViewerControl.SelectedTab.Text is "Raid 🎉")
            {
                RaidCodeEntry RaidForm = new(Executor);
                RaidForm.ShowDialog();
                return;
            }

            int selectedInt = 0;
            string currentTab = ViewerControl.SelectedTab.Text;
            switch (currentTab)
            {
                case "Connection 🔌": selectedInt = 0; break;
                case "View 🔎": selectedInt = 1; break;
                case "Box 📦": selectedInt = 2; break;
                case "Egg 🥚": selectedInt = 3; break;
                case "Wide 🔭": selectedInt = 4; break;
                case "NPC 🤖": selectedInt = 5; break;
                case "Screenshot 📷": selectedInt = 6; break;
            }

            if (FormLoaded[selectedInt])
                return;

            if (currentTab is not "Wide 🔭" && GameType is (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl or (int)GameSelected.Sword or (int)GameSelected.Shield or (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee)
            {
                ViewerControl.Height = 550;
                ViewerControl.Width = 511;
                Height = 507;
                Width = 511;
            }

            Form form = new();
            switch (currentTab)
            {
                case "Connection 🔌": FormLoaded[0] = true; return;
                case "View 🔎": FormLoaded[1] = true; return;
                case "Box 📦": form = new BoxViewerMode(GameType, Executor) { TopLevel = false }; FormLoaded[2] = true; break;
                case "Egg 🥚": form = new Egg_Viewer(Executor) { TopLevel = false }; FormLoaded[3] = true; break;
                case "Wide 🔭":
                    FormLoaded[4] = true;
                    switch (GameType)
                    {
                        case (int)GameSelected.LegendsArceus:
                            {
                                form = new WideViewerLA(Executor) { TopLevel = false };
                                ViewerControl.Height = 380;
                                ViewerControl.Width = 780;
                                Height = 400;
                                Width = 800;
                                break;
                            }
                        case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl:
                            {
                                form = new WideViewerBDSP(Executor) { TopLevel = false };
                                ViewerControl.Height = 560;
                                ViewerControl.Width = 825;
                                Height = 600;
                                Width = 850;
                                break;
                            }
                        case (int)GameSelected.Sword or (int)GameSelected.Shield:
                            {
                                form = new WideViewerSWSH(Executor) { TopLevel = false };
                                ViewerControl.Height = 600;
                                ViewerControl.Width = 890;
                                Height = 580;
                                Width = 870;
                                break;
                            }
                        case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                            {
                                form = new WideViewerLGPE(Executor) { TopLevel = false };
                                ViewerControl.Height = 400;
                                ViewerControl.Width = 475;
                                Height = 300;
                                Width = 400;
                                break;
                            }
                    }
                    break;
                case "NPC 🤖": form = new NPCViewer(GameType, Executor) { TopLevel = false }; FormLoaded[5] = true; break;
                case "Screenshot 📷": form = new ScreenshotForm(Executor) { TopLevel = false }; FormLoaded[6] = true; break;
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
            ViewerControl.TabPages.Remove(ViewPage);
            ViewerControl.TabPages.Remove(BoxPage);
            ViewerControl.TabPages.Remove(EggPage);
            ViewerControl.TabPages.Remove(WidePage);
            ViewerControl.TabPages.Remove(NPCPage);
            ViewerControl.TabPages.Remove(RaidPage);
            ViewerControl.TabPages.Remove(InGameScreenshotPage);

        }

    }
}