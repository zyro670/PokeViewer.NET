using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Octokit;
using PKHeX.Core;
using PKHeX.Drawing.Misc;
using PokeViewer.NET.Properties;
using PokeViewer.NET.SubForms;
using PokeViewer.NET.WideViewForms;
using SysBot.Base;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Sockets;
using System.Reflection;
using static PokéViewer.NET.Util.CommandsUtil;
using static PokeViewer.NET.RoutineExecutor;
using static PokeViewer.NET.ViewerUtil;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using MethodInvoker = System.Windows.Forms.MethodInvoker;

namespace PokeViewer.NET
{
    public partial class MainViewer : Form
    {
        public ViewerState Executor = null!;
        private const string ViewerVersion = "3.3.0";
        private readonly bool[] FormLoaded = new bool[8];
        private int GameType;
        private SimpleTrainerInfo Trainer = new();
        private readonly string[] trainerStrings = new string[3];
        private readonly string RefreshTime = Settings.Default.RefreshRate;
        private readonly List<Color> UIColors = [];
        protected ViewerOffsets Offsets { get; } = new();
        public DiscordSocketClient client;
        public CommandService commands;
        public IServiceProvider services;
        public MainViewer()
        {
            InitializeComponent();
            GetAllColors();
            LoadComboBox();
            MoodChecker();
            CheckTrainerIcon();
            CheckForSudo();
            CheckForOutbreakFilter();
            CreateCustomSequenceJson();
            DisableTabsOnStart();
            VersionLabel.Text = $"v{ViewerVersion}";
            CheckReleaseLabel();
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
            });
            commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                DefaultRunMode = Discord.Commands.RunMode.Async,
                CaseSensitiveCommands = false,
            });
            var map = new ServiceCollection();
            services = map.BuildServiceProvider();
        }

        private static void CheckForOutbreakFilter()
        {
            var path = "outbreakfilters.txt";
            var newpath = "refs\\outbreakfilters.txt";
            if (File.Exists(path))
                File.Move(path, newpath);
        }

        private static void CheckForSudo()
        {
            var path = "refs\\sudo.txt";
            if (!File.Exists(path))
                File.WriteAllText(path, $"{Settings.Default.UserDiscordID},");
        }

        private static void CreateCustomSequenceJson()
        {
            var path = "refs\\customsequence.json";
            if (!File.Exists(path))
            {
                List<CustomSequence> _data =
            [
                new CustomSequence()
                {
                    Name = "Template",
                    Click = [],
                    Delay = [],
                }
            ];

                string json = JsonConvert.SerializeObject(_data.ToArray());
                File.WriteAllText(path, json);
            }
        }
       
        private void MoodChecker()
        {
            SetMoodOnStart();
            var colors = CheckForColors(Settings.Default.DarkMode);
            SetColorsConnection(colors.Item1, colors.Item2);
        }

        private void PokeViewerForm_Load(object sender, EventArgs e)
        {
            SwitchIP.Text = Settings.Default.SwitchIP;
            LoadOriginDefault();
            LoadDateTime(sender, e);
        }

        private static async void CheckReleaseLabel()
        {
            try
            {
                GitHubClient client = new(new ProductHeaderValue("PokeViewer.NET"));
                Release releases = await client.Repository.Release.GetLatest("zyro670", "PokeViewer.NET");
                Version latestGitHubVersion = new(releases.TagName);
                Version localVersion = new(ViewerVersion);
                int versionComparison = localVersion.CompareTo(latestGitHubVersion);
                if (versionComparison < 0)
                {
                    DialogResult dialogResult = MessageBox.Show("A new PokeViewer.NET release is available. Download?", "An update is available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        HttpClient httpClient = new();
                        var file = await httpClient.GetByteArrayAsync($"https://github.com/zyro670/PokeViewer.NET/releases/download/{releases.TagName}/v{releases.TagName}.zip");
                        File.WriteAllBytes($"PokeViewer.NET-{releases.TagName}.zip", file);
                        ZipFile.ExtractToDirectory($"PokeViewer.NET-{releases.TagName}.zip", AppDomain.CurrentDomain.BaseDirectory);
                        await Task.Delay(1_000, CancellationToken.None).ConfigureAwait(false);
                        File.Delete($"PokeViewer.NET-{releases.TagName}.zip");
                        MessageBox.Show($"Download complete. Please replace the PokeViewer.exe with the new one found in the v{releases.TagName} folder after you click Ok.", "Update Message");
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to check version of PokeViewer.NET, please try again later.\n{ex}");
            }
        }

        private void LoadOriginDefault()
        {
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
            OriginIcon.ImageLocation = url;
            ConnectionSpriteBox.ImageLocation = url;
        }

        private void CheckTrainerIcon()
        {
            var folder = "refs";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var icon = "refs\\icon.png";
            if (File.Exists(icon))
            {
                var oldImage = TrainerPhoto.Image;
                TrainerPhoto.Image = new Bitmap(icon);
                oldImage?.Dispose();
            }
            else
                TrainerPhoto.ImageLocation = "https://i.imgur.com/rNSKg5n.png";

        }

        private void CheckForIP(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text != "192.168.0.0")
                Settings.Default.SwitchIP = textBox.Text;

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
            if (!Settings.Default.UseWiFiProtocol)
                ConnectionGroupBox.Text = "Switch Port";
            else
                ConnectionGroupBox.Text = "Switch IP";
        }

        private void LoadDateTime(object sender, EventArgs e)
        {
            TodaysDate.Text = "Met Date: " + DateTime.Today.ToString("MM/dd/yyyy");
        }

        private static SwitchProtocol GetProtocol()
        {
            if (!Settings.Default.UseWiFiProtocol)
                return SwitchProtocol.USB;
            return SwitchProtocol.WiFi;
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            var token = new CancellationToken();
            if (Executor is null || !Executor.SwitchConnection.Connected)
            {
                try
                {
                    var config = GetProtocol() switch
                    {
                        SwitchProtocol.USB => new SwitchConnectionConfig { Port = int.Parse(Settings.Default.SwitchIP), Protocol = SwitchProtocol.USB },
                        SwitchProtocol.WiFi => new SwitchConnectionConfig { IP = Settings.Default.SwitchIP, Port = 6000, Protocol = SwitchProtocol.WiFi },
                        _ => throw new NotImplementedException(),
                    };
                    var state = new ViewerExecutorBase
                    {
                        Connection = config,
                        InitialRoutine = RoutineType.Read,
                    };
                    Executor = new ViewerState(state);
                    await Invoke(async () =>
                    {
                        Connect.Text = "Disconnect";
                        SwitchIP.Enabled = false;
                        ScreenTrackBar.Enabled = true;
                        Executor.Connection.Connect();
                        await Window_Loaded(token).ConfigureAwait(false);
                    });
                    if (!string.IsNullOrEmpty(Settings.Default.BotToken))
                    {
                        await RegisterCommandsAsync().ConfigureAwait(false);
                        await client.LoginAsync(TokenType.Bot, Settings.Default.BotToken, true).ConfigureAwait(false);
                        await client.StartAsync().ConfigureAwait(false);
                    }
                }
                catch (SocketException err)
                {
                    string port = Settings.Default.UseWiFiProtocol ? "Port" : "IP address";
                    MessageBox.Show($"{err.Message}{Environment.NewLine}Ensure {port} is correct before attempting to connect!");
                }
            }
            else
            {
                await Executor.SwitchConnection.SendAsync(SwitchCommand.DetachController(true), token).ConfigureAwait(false);
                Executor.Disconnect();
                await client.StopAsync().ConfigureAwait(false);
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
            bool isValid = false;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: isValid = PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield: isValid = PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: isValid = PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LegendsArceus: isValid = PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: isValid = pk.Species < (int)Species.Mew && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
            }
            string? sprite;
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
            bool alpha = pk is PA8 pa8 && pa8.IsAlpha;
            bool hasMark = false;
            bool isGmax = pk is PK8 pk8 && pk8.CanGigantamax;
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
            var scaleS = (IScaledSize)pk;
            string scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(scaleS.HeightScalar)} ({scaleS.HeightScalar})";

            switch (pk.Gender)
            {
                case 0: gender = " (M)"; break;
                case 1: gender = " (F)"; break;
                case 2: break;
            }
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{isAlpha}{(Species)pk.Species}{form}{gender}{ec}{pid}{Environment.NewLine}Nature: {pk.Nature}{Environment.NewLine}Ability: {GameInfo.GetStrings(1).Ability[pk.Ability]}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}{scale}{msg}";
            LiveStats.Text = $"{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}PP";
            ViewBox.Text = output;
            sprite = PokeImg(pk, isGmax);
            PokeSprite.Load(sprite);
            var type1spr = $"https://raw.githubusercontent.com/zyro670/PokeTextures/844a15d0f5ac62bebb265d015537e94fc7b8738c/Typings/grptxt_icon_type_gem_text_" + $"{pk.PersonalInfo.Type1}" + ".png";
            Typing1.Load(type1spr);
            if (pk.PersonalInfo.Type1 != pk.PersonalInfo.Type2)
            {
                var type2spr = $"https://raw.githubusercontent.com/zyro670/PokeTextures/844a15d0f5ac62bebb265d015537e94fc7b8738c/Typings/grptxt_icon_type_gem_text_" + $"{pk.PersonalInfo.Type2}" + ".png";
                Typing2.Load(type2spr);
            }
            else
                Typing2.Load(type1spr);
            if (alpha)
            {
                var url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_alpha.png";
                Image img = null!;
                using var client = new HttpClient();
                using var response = await client.GetStreamAsync(url, token).ConfigureAwait(false);
                img = Image.FromStream(response);
                Specialty.Visible = true;
                Specialty.Image = img;
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
                Image img = null!;
                using var client = new HttpClient();
                using var response = await client.GetStreamAsync(url, token).ConfigureAwait(false);
                img = Image.FromStream(response);
                Specialty.Visible = true;
                Specialty.Image = img;
            }
            if (RefreshStats.Checked)
            {
                var StartingHP = pk.Stat_HPCurrent;
                _ = int.TryParse(RefreshTime, out int refr);
                while (pk.Stat_HPCurrent != 0)
                {
                    if (!Executor.SwitchConnection.Connected)
                    {
                        System.Windows.Forms.Application.Restart();
                        Environment.Exit(0);
                    }
                    switch (GameType)
                    {
                        /*case (int)GameSelected.Scarlet or (int)GameSelected.Violet: pk = await ReadInBattlePokemonSV(offset, size).ConfigureAwait(false); break; one day*/
                        case (int)GameSelected.Sword or (int)GameSelected.Shield: pk = await ReadInBattlePokemonSWSH(offset2, size).ConfigureAwait(false); break;
                        case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: pk = await ReadInBattlePokemonBDSP(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LegendsArceus: pk = await ReadInBattlePokemonLA(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: pk = await ReadInBattlePokemonLGPE(offset2, size).ConfigureAwait(false); break;
                    }
                    LiveStats.Text = $"{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP}PP{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}PP";
                    HpLabel.Text = $"HP - {pk.Stat_HPCurrent / StartingHP * 100}%";
                    await Task.Delay(refr, token).ConfigureAwait(false);
                }
                LiveStats.Clear();
                HpLabel.Text = "          HP%";
                ViewBox.Text = "No Pokémon present.";
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                PokeSprite.Load(sprite);
                Typing1.Visible = false;
                Typing1.Image = null;
                Typing2.Visible = false;
                Typing2.Image = null;
                Specialty.Visible = false;
                Specialty.Image = null;
            }
            View.Enabled = true;
        }

        /*public async Task<PK9> ReadInBattlePokemonSV(ulong offset, int size)
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
        }*/

        public async Task<PK8> ReadInBattlePokemonSWSH(uint offset, int size)
        {
            var token = CancellationToken.None;
            byte[]? data;
            PK8 pk;
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
                [
                    [0x2636120, 0x280, 0xD8, 0x78, 0x10, 0x98, 0x00],
                    [0x2636170, 0x2F0, 0x58, 0x130, 0x138, 0xD0],
                    [0x28ED668, 0x68, 0x1E8, 0x1D0, 0x128],
                    [0x296C030, 0x60, 0x40, 0x1B0, 0x58, 0x00]
                ];

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
                            /*var ptr = new long[] { 0x42FD3C0, 0x10, 0x2D0, 0x2A0, 0x48, 0x2E0 };
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x158;
                            var pk = await ReadInBattlePokemonSV(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);*/
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
                        }
                        ; break;
                    case (int)GameSelected.BrilliantDiamond:
                        {
                            var ptr = new long[] { 0x4C59EF0, 0x20, 0x98, 0x00, 0x20 };
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }
                        ; break;
                    case (int)GameSelected.ShiningPearl:
                        {
                            var ptr = new long[] { 0x4E70FC8, 0x20, 0x98, 0x00, 0x20 };
                            var ofs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            int size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }
                        ; break;
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

        private async Task Window_Loaded(CancellationToken token)
        {
            int type = 0;
            string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/icon_version/64x64/icon_version_";
            string title = await Executor.SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            (string, string) strings = (string.Empty, string.Empty);
            switch (title)
            {
                case HOMEID:
                    {
                        url += "HOME.png";
                        type = (int)GameSelected.HOME;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                        });
                        break;
                    }
                case ScarletID:
                    {
                        url += "SC3.png";
                        type = (int)GameSelected.Scarlet;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(PartyPage);
                            ViewerControl.TabPages.Add(EggPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            EventRedeemButton.Visible = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVSV(token).ConfigureAwait(false);
                        break;
                    }
                case VioletID:
                    {
                        url += "VI3.png";
                        type = (int)GameSelected.Violet;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(PartyPage);
                            ViewerControl.TabPages.Add(EggPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            EventRedeemButton.Visible = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVSV(token).ConfigureAwait(false);
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
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVLA(token).ConfigureAwait(false);
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
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVBDSP(type, token).ConfigureAwait(false);
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
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVBDSP(type, token).ConfigureAwait(false);
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
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVSWSH(token).ConfigureAwait(false);
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
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                            ControllerButton.Enabled = true;
                        });
                        strings = await GetFakeTrainerSAVSWSH(token).ConfigureAwait(false);
                        break;
                    }
                case EeveeID:
                    {
                        url += "LGE.png"; type = (int)GameSelected.LetsGoEevee;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                        });
                        strings = GetFakeTrainerSAVLGPE();
                        break;
                    }
                case PikachuID:
                    {
                        url += "LGP.png"; type = (int)GameSelected.LetsGoPikachu;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ViewerControl.TabPages.Add(ViewPage);
                            ViewerControl.TabPages.Add(BoxPage);
                            ViewerControl.TabPages.Add(NPCPage);
                            ViewerControl.TabPages.Add(InGameScreenshotPage);
                            ViewerControl.TabPages.Add(MiscPage);
                            WideButton.Enabled = true;
                        });
                        strings = GetFakeTrainerSAVLGPE();
                        break;
                    }
            }
            BeginInvoke((MethodInvoker)delegate ()
            {
                OriginIcon.ImageLocation = url;
                ConnectionSpriteBox.ImageLocation = url;
                GameType = type;
                Settings.Default.GameConnected = GameType;
                Settings.Default.Save();
                ViewBox.Text = "Click View!";
                var bg = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                PokeSprite.ImageLocation = bg;
                TrainerLabel.Text = strings.Item1;
                richTIDSID.Text = strings.Item2;
                trainerStrings[0] = Trainer.OT!;
                trainerStrings[1] = $"{Trainer.TID16}";
                trainerStrings[2] = $"{Trainer.SID16}";
            });
        }

        private void CaptureWindow_Click(object sender, EventArgs e)
        {
            Rectangle bounds = ViewPage.Bounds;
            Bitmap bmp = new(ViewPage.Width, ViewPage.Height + 25);
            DrawToBitmap(bmp, bounds);
            Clipboard.SetImage(bmp);
            MessageBox.Show("Copied to clipboard!");
        }

        private void RefreshStats_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chextBox = (CheckBox)sender;
            if (chextBox.Checked)
                Settings.Default.RefreshStats = true;
            else
                Settings.Default.RefreshStats = false;

            Settings.Default.Save();
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
            var bytes = Executor.SwitchConnection.PixelPeek(token).Result;
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

            Form form = new();
            string currentTab = ViewerControl.SelectedTab!.Text;
            int selectedInt = 0;
            bool mode = CheckForMood();
            var colors = CheckForColors(mode);
            switch (currentTab)
            {
                case "Connection 🔌": selectedInt = 0; break;
                case "View 🔎": selectedInt = 1; break;
                case "Box 📦": selectedInt = 2; break;
                case "Party 👨‍👩‍👦‍👦": selectedInt = 3; break;
                case "Egg 🥚": selectedInt = 4; break;
                case "NPC 🤖": selectedInt = 5; break;
                case "Screenshot 📷": selectedInt = 6; break;
                case "Misc 📓": selectedInt = 7; break;
            }

            if (FormLoaded[selectedInt] is true)
            {
                return;
            }

            switch (currentTab)
            {
                case "Connection 🔌": FormLoaded[0] = true; return;
                case "View 🔎": FormLoaded[1] = true; return;
                case "Box 📦": form = new BoxViewerMode(GameType, Executor, colors, trainerStrings) { TopLevel = false }; FormLoaded[2] = true; break;
                case "Party 👨‍👩‍👦‍👦": form = new PartyViewer(GameType, Executor, colors) { TopLevel = false }; FormLoaded[3] = true; break;
                case "Egg 🥚": form = new Egg_Viewer(GameType, Executor, colors) { TopLevel = false }; FormLoaded[4] = true; break;
                case "NPC 🤖": form = new NPCViewer(GameType, Executor, colors) { TopLevel = false }; FormLoaded[5] = true; break;
                case "Screenshot 📷": form = new ScreenshotForm(Executor, colors) { TopLevel = false }; FormLoaded[6] = true; break;
                case "Misc 📓": form = new MiscView(Executor, GameType, colors) { TopLevel = false }; FormLoaded[7] = true; break;
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

        private void SetColorsConnection(Color back, Color fore)
        {
            this.BackColor = back;
            this.ForeColor = fore;
            ConnectionGroupBox.BackColor = back;
            ConnectionGroupBox.ForeColor = fore;
            ScreenTrackBar.BackColor = back;
            ScreenTrackBar.ForeColor = fore;
            WideButton.BackColor = back;
            WideButton.ForeColor = fore;
            Connect.BackColor = back;
            Connect.ForeColor = fore;
            ConnectionPage.BackColor = back;
            ConnectionPage.ForeColor = fore;
            View.BackColor = back;
            View.ForeColor = fore;
            WindowCapture.BackColor = back;
            WindowCapture.ForeColor = fore;
            InGameScreenshot.BackColor = back;
            InGameScreenshot.ForeColor = fore;
            SetMoodButton.BackColor = back;
            SetMoodButton.ForeColor = fore;
            TrainerPassportGroup.BackColor = back;
            TrainerPassportGroup.ForeColor = fore;
            ExtrasGroup.BackColor = back;
            ExtrasGroup.ForeColor = fore;
            SwitchIP.BackColor = back;
            SwitchIP.ForeColor = fore;
            AltForeCombo.BackColor = back;
            AltForeCombo.ForeColor = fore;
            AltBackCombo.BackColor = back;
            AltBackCombo.ForeColor = fore;
            EventRedeemButton.BackColor = back;
            EventRedeemButton.ForeColor = fore;
            richTIDSID.BackColor = back;
            richTIDSID.ForeColor = fore;
        }

        private static bool CheckForMood()
        {
            if (Settings.Default.DarkMode == true)
                return true;
            else
                return false;
        }

        private static (Color, Color) CheckForColors(bool mode)
        {
            if (mode is true)
                return (Settings.Default.DefaultBackColor, Settings.Default.DefaultForeColor);
            else
                return (Color.White, Color.Navy);
        }

        private void DisableTabsOnStart()
        {
            ViewerControl.TabPages.Remove(ViewPage);
            ViewerControl.TabPages.Remove(BoxPage);
            ViewerControl.TabPages.Remove(PartyPage);
            ViewerControl.TabPages.Remove(EggPage);
            ViewerControl.TabPages.Remove(NPCPage);
            ViewerControl.TabPages.Remove(InGameScreenshotPage);
            ViewerControl.TabPages.Remove(MiscPage);
        }

        private void WideButton_Click(object sender, EventArgs e)
        {
            Form form = new();
            var colors = CheckForColors(Settings.Default.DarkMode);
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    {
                        form = new WideViewerSV(Executor, colors);
                        break;
                    }
                case (int)GameSelected.LegendsArceus:
                    {
                        form = new WideViewerLA(Executor, colors);
                        break;
                    }
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl:
                    {
                        form = new WideViewerBDSP(Executor, colors);
                        break;
                    }
                case (int)GameSelected.Sword or (int)GameSelected.Shield:
                    {
                        form = new WideViewerSWSH(Executor, colors);
                        break;
                    }
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                    {
                        form = new WideViewerLGPE(Executor, colors);
                        break;
                    }
            }
            form.ShowDialog();
        }

        private void ScreenTrackBar_Scroll(object sender, EventArgs e)
        {
            if (ScreenTrackBar.Value != 1)
                SetScreen(ScreenState.Off, CancellationToken.None).ConfigureAwait(false);
            else
                SetScreen(ScreenState.On, CancellationToken.None).ConfigureAwait(false);
        }

        private async Task SetScreen(ScreenState state, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.SetScreen(state, true), token).ConfigureAwait(false);
        }

        private async void SetMoodButton_Click(object sender, EventArgs e)
        {
            bool darkmode = false;
            if (AltMoodRadio.Checked)
                darkmode = true;

            else if (!AltMoodRadio.Checked)
                darkmode = false;

            Settings.Default.DarkMode = darkmode;
            Settings.Default.DefaultForeVal = AltForeCombo.SelectedIndex;
            Settings.Default.DefaultBackVal = AltBackCombo.SelectedIndex;
            Settings.Default.Save();

            var owner = new Form { Visible = false };
            var handle = owner.Handle;
            owner.BeginInvoke((MethodInvoker)delegate
            {
                MessageBox.Show(owner, text: "Mood has been set! Application restarting...", "Timed Message");
            });
            await Task.Delay(TimeSpan.FromSeconds(2));
            owner.Dispose();

            System.Windows.Forms.Application.Restart();
        }

        private void SetMoodOnStart()
        {
            if (Settings.Default.DarkMode == true)
            {
                AltMoodRadio.Checked = true;
            }
            else
            {
                DefaultMoodRadio.Checked = true;
                Settings.Default.DefaultForeColor = Color.Navy;
                Settings.Default.DefaultBackColor = Color.White;
            }
        }

        #region TrainerSav
        // via SysBot.NET

        public class TrainerInfo
        {
            public string? OT { get; set; }
            public ushort? TID16 { get; set; }
            public ushort? SID16 { get; set; }
        }

        public async Task<(string, string)> GetFakeTrainerSAVSWSH(CancellationToken token)
        {
            var sav = new SAV8SWSH();
            var info = sav.MyStatus;
            var read = await Executor.SwitchConnection.ReadBytesAsync(Offsets.TrainerDataOffsetSWSH, Offsets.TrainerDataLengthSWSH, token).ConfigureAwait(false);
            read.CopyTo(info.Data);
            var newInfo = new TrainerInfo
            {
                OT = sav.OT,
                TID16 = sav.TID16,
                SID16 = sav.SID16
            };
            return ($"Name: {newInfo.OT}", $"TID: {sav.TrainerTID7} ({sav.TID16})\nSID: {sav.TrainerSID7} ({sav.SID16})");
        }
        public async Task<(string, string)> GetFakeTrainerSAVSV(CancellationToken token)
        {
            var sav = new SAV9SV();
            var info = sav.MyStatus;
            var read = await Executor.SwitchConnection.PointerPeek(info.Data.Length, Offsets.MyStatusPointerSV, token).ConfigureAwait(false);
            read.CopyTo(info.Data);
            var newInfo = new TrainerInfo
            {
                OT = sav.OT,
                TID16 = sav.TID16,
                SID16 = sav.SID16
            };
            return ($"Name: {newInfo.OT}", $"TID: {sav.TrainerTID7} ({sav.TID16})\nSID: {sav.TrainerSID7} ({sav.SID16})");
        }
        public async Task<(string, string)> GetFakeTrainerSAVLA(CancellationToken token)
        {
            var sav = new SAV8LA();
            var info = sav.MyStatus;
            var read = await Executor.SwitchConnection.PointerPeek(info.Data.Length, Offsets.MyStatusPointerLA, token).ConfigureAwait(false);
            read.CopyTo(info.Data);
            var newInfo = new TrainerInfo
            {
                OT = sav.OT,
                TID16 = sav.TID16,
                SID16 = sav.SID16
            };
            return ($"Name: {newInfo.OT}", $"TID: {sav.TrainerTID7} ({sav.TID16})\nSID: {sav.TrainerSID7} ({sav.SID16})");
        }
        public const int MaxByteLengthStringObject = 0x14 + 0x1A;
        public async Task<(string, string)> GetFakeTrainerSAVBDSP(int type, CancellationToken token)
        {
            var sav = new SAV8BS();
            var info = sav.MyStatus;
            var name = await Executor.SwitchConnection.PointerPeek(MaxByteLengthStringObject, type == (int)GameSelected.BrilliantDiamond ? Offsets.MyStatusTrainerPointerBD : Offsets.MyStatusTrainerPointerSP, token).ConfigureAwait(false);
            info.OT = ReadStringFromRAMObject(name);
            var offset = await Executor.SwitchConnection.PointerAll(type == (int)GameSelected.BrilliantDiamond ? Offsets.MyStatusTIDPointerBD : Offsets.MyStatusTIDPointerSP, token).ConfigureAwait(false);
            var tid = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, 2, token).ConfigureAwait(false);
            var sid = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset + 2, 2, token).ConfigureAwait(false);
            info.TID16 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(tid);
            info.SID16 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(sid);
            var newInfo = new TrainerInfo
            {
                OT = sav.OT,
                TID16 = sav.TID16,
                SID16 = sav.SID16
            };
            return ($"Name: {newInfo.OT}", $"TID: {sav.TrainerTID7} ({sav.TID16})\nSID: {sav.TrainerSID7} ({sav.SID16})");
        }
        public (string, string) GetFakeTrainerSAVLGPE()
        {
            var sav = new SAV7b();
            Span<byte> bytes = sav.Blocks.Status.Data;
            byte[] data = ReadLGPEData;
            data.CopyTo(bytes);
            var newInfo = new TrainerInfo
            {
                OT = sav.OT,
                TID16 = sav.TID16,
                SID16 = sav.SID16
            };
            return ($"Name: {newInfo.OT}", $"TID: {sav.TrainerTID7} ({sav.TID16})\nSID: {sav.TrainerSID7} ({sav.SID16})");
        }

        private byte[] ReadLGPEData => Executor.SwitchConnection.ReadBytesAsync(Offsets.TrainerDataLGPE, Offsets.TrainerSizeLGPE, CancellationToken.None).Result;

        public static string ReadStringFromRAMObject(byte[] obj)
        {
            // 0x10 typeinfo/monitor, 0x4 len, char[len]
            const int ofs_len = 0x10;
            const int ofs_chars = 0x14;
            Debug.Assert(obj.Length >= ofs_chars);

            // Detect string length, but be cautious about its correctness (protect against bad data)
            int maxCharCount = (obj.Length - ofs_chars) / 2;
            int length = BitConverter.ToInt32(obj, ofs_len);
            if (length < 0 || length > maxCharCount)
                length = maxCharCount;

            return StringConverter8.GetString(obj.AsSpan(ofs_chars, length * 2));
        }
        #endregion

        private void LoadComboBox()
        {
            AltForeCombo.Items.Clear();
            AltBackCombo.Items.Clear();
            for (var i = 0; i < UIColors.Count; i++)
            {
                AltForeCombo.Items.Add($"{UIColors[i].ToString().Replace("Color", "").Replace("[", "").Replace("]", "").Trim()}");
                AltBackCombo.Items.Add($"{UIColors[i].ToString().Replace("Color", "").Replace("[", "").Replace("]", "").Trim()}");
            }
            AltForeCombo.SelectedIndex = Settings.Default.DefaultForeVal;
            AltBackCombo.SelectedIndex = Settings.Default.DefaultBackVal;
        }

        private void GetAllColors()
        {
            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            for (int i = 28; i < 97; i++)
            {
                UIColors.Add(Color.FromKnownColor(colors[i]));
            }
        }

        private void DefaultMoodRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (DefaultMoodRadio.Checked)
            {
                AltForeCombo.Enabled = false;
                AltBackCombo.Enabled = false;
            }
        }

        private void AltMoodRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (AltMoodRadio.Checked)
            {
                AltForeCombo.Enabled = true;
                AltBackCombo.Enabled = true;
            }
        }

        private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = AltForeCombo.SelectedIndex;
            Settings.Default.DefaultForeColor = UIColors[selection];
        }

        private void AltBackCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = AltBackCombo.SelectedIndex;
            Settings.Default.DefaultBackColor = UIColors[selection];
        }

        private void EventRedeem_Click(object sender, EventArgs e)
        {
            var colors = CheckForColors(Settings.Default.DarkMode);
            EventCodeEntry form = new(Executor, colors);
            form.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var colors = CheckForColors(Settings.Default.DarkMode);
            ControllerView form = new(Executor, colors);
            form.ShowDialog();
        }

        private void DiscordUtilButton_Click(object sender, EventArgs e)
        {
            var colors = CheckForColors(Settings.Default.DarkMode);
            DiscordUtilForm form = new(colors);
            form.ShowDialog();
        }

        public async Task RegisterCommandsAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();

            await commands.AddModulesAsync(assembly, services).ConfigureAwait(false);

            client.MessageReceived += HandleCommandAsync;
            client.ReactionAdded += HandleReactionAsync;
        }

        public static bool CanUseCommandSudo(ulong uid)
        {
            var path = "refs\\sudo.txt";
            string contents = File.ReadAllText(path);
            var sudos = contents.Split(',');
            foreach (var s in sudos)
            {
                if (s.Equals(uid.ToString()))
                    return true;
            }
            return false;
        }

        private async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            if (reaction.Emote.Name == "👍")
            {
                await message.GetOrDownloadAsync().Result.GetReactionUsersAsync(new Discord.Emoji("👍"), 1000).FlattenAsync();
            }
            if (reaction.Emote.Name == "👎")
            {
                await message.GetOrDownloadAsync().Result.GetReactionUsersAsync(new Discord.Emoji("👎"), 1000).FlattenAsync();
            }
        }
        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (s is not SocketUserMessage msg)
                return;

            int pos = 0;
            if (msg.HasStringPrefix(Settings.Default.BotPrefixString, ref pos))
            {
                bool handled = await TryHandleCommand(msg, pos).ConfigureAwait(false);
                if (handled)
                    return;
            }
            return;
        }

        private async Task<bool> TryHandleCommand(SocketUserMessage msg, int pos)
        {
            var context = new SocketCommandContext(client, msg);
            if (!CanUseCommandSudo(msg.Author.Id))
            {
                await msg.Channel.SendMessageAsync("You do not have permission to use commands.").ConfigureAwait(false);
                return false;
            }
            else
            {
                var result = await commands.ExecuteAsync(context, pos, services).ConfigureAwait(false);
                if (!result.IsSuccess)
                    await msg.Channel.SendMessageAsync(result.ErrorReason).ConfigureAwait(false);

                if (result.Error == CommandError.UnknownCommand)
                    return false;
            }
            return true;
        }
    }
}