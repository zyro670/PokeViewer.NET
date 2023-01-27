using PKHeX.Core;
using PKHeX.Drawing.Misc;
using PokeViewer.NET.SubForms;
using PokeViewer.NET.WideViewForms;
using SysBot.Base;
using System.Net.Sockets;
using System.Text;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET
{
    public partial class MainViewer : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public static RoutineExecutor Executor = new();

        public MainViewer()
        {
            InitializeComponent();
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
            TodaysDate.Text = "Met Date: " + DateTime.Today.ToString("MM/dd/yyyy");
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (!SwitchConnection.Connected)
            {
                try
                {
                    SwitchConnection.Connect();
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
                    WideView.Visible = true;
                    WideView.Enabled = true;
                    BoxViewer.Visible = true;
                    BoxViewer.Enabled = true;
                    TrainerView.Visible = true;
                    DayCareView.Visible = true;
                    OverworldView.Visible = true;
                    Raids.Visible = true;
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
                Connect.Text = "Connect";
                SwitchIP.Enabled = true;
                View.Enabled = false;
                ViewBox.Visible = false;
                PokeSprite.Visible = false;
                LiveStats.Visible = false;
                RefreshStats.Visible = false;
                Refresh.Visible = false;
                RefreshBox.Visible = false;
                InGameScreenshot.Visible = false;
                HidePIDEC.Visible = false;
                View.Visible = false;
                Typing1.Visible = false;
                Typing2.Visible = false;
                Specialty.Visible = false;
                HpLabel.Visible = false;
                UniqueBox.Visible = false;
                UniqueBox2.Visible = false;
                WideView.Visible = false;
                WideView.Enabled = false;
                BoxViewer.Visible = false;
                TrainerView.Visible = false;
                DayCareView.Visible = false;
                Raids.Visible = false;
                LiveStats.Clear();
                string url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_generation_00%5Esb.png";
                OriginIcon.ImageLocation = url;
                OverworldView.Visible = false;
                SwitchConnection.Reset();
                this.Close();
                Application.Restart();
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
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: isValid = (PersonalTable.SV[pk.Species]).IsPresentInGame;  break;
                case (int)GameSelected.SW or (int)GameSelected.SH: isValid = (PersonalTable.SWSH[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.BD or (int)GameSelected.SP: isValid = (PersonalTable.BDSP[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.LA:
                    {
                        isValid = (PersonalTable.LA[pk.Species]).IsPresentInGame;
                        if (!isValid)
                        {
                            if ((Species)pk.Species is Species.Decidueye or Species.Typhlosion or Species.Samurott or Species.Qwilfish or Species.Lilligant or Species.Sliggoo or Species.Goodra
                            or Species.Growlithe or Species.Arcanine or Species.Voltorb or Species.Electrode or Species.Sneasel or Species.Avalugg or Species.Zorua or Species.Zoroark or Species.Braviary)
                            isValid = true;
                        }
                        break;
                    }
                case (int)GameSelected.LGP or (int)GameSelected.LGE: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
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
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{isAlpha}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{msg}";
            LiveStats.Text = $"{(Move)pk.Move1} - {pk.Move1_PP}PP{Environment.NewLine}{(Move)pk.Move2} - {pk.Move2_PP}PP{Environment.NewLine}{(Move)pk.Move3} - {pk.Move3_PP}PP{Environment.NewLine}{(Move)pk.Move4} - {pk.Move4_PP}PP";
            ViewBox.Text = output;
            sprite = PokeImg(pk, isGmax);
            PokeSprite.Load(sprite);
            var imgt1 = TypeSpriteUtil.GetTypeSpriteWide(pk.PersonalInfo.Type1);
            Typing1.Image = imgt1;
            if (pk.PersonalInfo.Type1 != pk.PersonalInfo.Type2)
            {
                var imgt2 = TypeSpriteUtil.GetTypeSpriteWide(pk.PersonalInfo.Type2);
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
                        Application.Restart();
                        Environment.Exit(0);
                    }
                    switch (GameType)
                    {
                        case (int)GameSelected.Scarlet or (int)GameSelected.Violet: pk = await ReadInBattlePokemonSV(offset2, size).ConfigureAwait(false); break;
                        case (int)GameSelected.SW or (int)GameSelected.SH: pk = await ReadInBattlePokemonSWSH(offset2, size).ConfigureAwait(false); break;
                        case (int)GameSelected.BD or (int)GameSelected.SP: pk = await ReadInBattlePokemonBDSP(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LA: pk = await ReadInBattlePokemonLA(offset, size).ConfigureAwait(false); break;
                        case (int)GameSelected.LGP or (int)GameSelected.LGE: pk = await ReadInBattlePokemonLGPE(offset2, size).ConfigureAwait(false); break;
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
                offset = await GetPointerAddress("[[[[[[main+42F3DD8]+D8]+48]+18]+D8]+1E0]", CancellationToken.None).ConfigureAwait(false);
                data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, CancellationToken.None).ConfigureAwait(false); // RaidLobbyPokemon
                pk = new PK9(data);
                return pk;
            }
            return pk;
        }

        public async Task<PK8> ReadInBattlePokemonSWSH(uint offset, int size)
        {
            byte[]? data = { 0 };
            PK8 pk = new();
            if (UniqueBox.Checked)
            {
                offset = 0x886A95B8;
                data = await SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false); // RaidPokemon
                pk = new PK8(data);
                return pk;
            }
            if (UniqueBox2.Checked)
            {
                string[] campers =
                {
                    "[[[[[[main+2636120]+280]+D8]+78]+10]+98]",
                    "[[[[[main+2636170]+2F0]+58]+130]+138]+D0",
                    "[[[[main+28ED668]+68]+1E8]+1D0]+128",
                    "[[[[[main+296C030]+60]+40]+1B0]+58]"
                };
                for (int i = 0; i < campers.Length; i++)
                {
                    var pointer = campers[i];
                    var ofs = await GetPointerAddress(pointer, CancellationToken.None).ConfigureAwait(false);
                    data = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, size, CancellationToken.None).ConfigureAwait(false);
                    pk = new PK8(data);
                    if (pk.Species != 0 && pk.Species < (int)Species.MAX_COUNT)
                        return pk;
                }
            }
            data = await SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false); // WildPokemon
            pk = new PK8(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await SwitchConnection.ReadBytesAsync(0x886BC348, size, CancellationToken.None).ConfigureAwait(false); // LegendaryPokemon
                pk = new PK8(data);
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
            var data = await SwitchConnection.ReadBytesMainAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PB7(data);
            if (pk.Species == 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                data = await SwitchConnection.ReadBytesAsync(0x9A118D68, size, CancellationToken.None).ConfigureAwait(false);
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
                // Add SV overworld title here
                /*case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                ptr = new long[] { 0x42C30E8, 0x1A9 }; break;*/
                case (int)GameSelected.LA:
                ptr = new long[] { 0x42C30E8, 0x1A9 }; break;
                case (int)GameSelected.BD:
                ptr = new long[] { 0x4C59C98, 0xB8, 0x3C }; break;
                case (int)GameSelected.SP:
                ptr = new long[] { 0x4E70D70, 0xB8, 0x3C }; break;
                case (int)GameSelected.SW or (int)GameSelected.SH:
                {
                    var data = await SwitchConnection.ReadBytesAsync((uint)(GameType == (int)GameVersion.SH ? 0x3F128626 : 0x3F128624), 1, token).ConfigureAwait(false);
                    return data[0] == (GameType == (int)GameSelected.SH ? 0x40 : 0x41);
                }
                case (int)GameSelected.LGP or (int)GameSelected.LGE:
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
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: isValid = (PersonalTable.SV[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.SW or (int)GameSelected.SH: isValid = (PersonalTable.SWSH[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.BD or (int)GameSelected.SP: isValid = (PersonalTable.BDSP[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.LA: isValid = (PersonalTable.LA[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.LGP or (int)GameSelected.LGE: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
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
            View.Enabled = false;
            if (SwitchConnection.Connected)
            {
                ViewBox.Text = "Reading encounter...";
                switch (GameType)
                {
                    case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                        {
                            var ofs = await GetPointerAddress("[[[[[main+42FD3C0]+10]+2D0]+2A0]+48]+2E0", CancellationToken.None).ConfigureAwait(false);
                            var size = 0x158;
                            var pk = await ReadInBattlePokemonSV(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                            return;
                        }
                    case (int)GameSelected.SW or (int)GameSelected.SH:
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
                    case (int)GameSelected.LA:
                        {
                            var ofs = await GetPointerAddress("[[[[[main+42a6f00]+D0]+B8]+300]+70]+60]+98]+10]", CancellationToken.None).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonLA(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.BD:
                        {
                            var ofs = await GetPointerAddress("[[[[main+4C59EF0]+20]+98]]+20", CancellationToken.None).ConfigureAwait(false);
                            var size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.SP:
                        {
                            var ofs = await GetPointerAddress("[[[[main+4E70FC8]+20]+98]]+20", CancellationToken.None).ConfigureAwait(false);
                            int size = 0x168;
                            var pk = await ReadInBattlePokemonBDSP(ofs, size).ConfigureAwait(false);
                            SanityCheck(pk);
                            FillPokeData(pk, ofs, 0, size);
                        }; break;
                    case (int)GameSelected.LGP or (int)GameSelected.LGE:
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
                        WideView.Visible = false;
                        WideView.Text = "Raid View";
                        View.Enabled = false;
                        TrainerView.Visible = false;
                        DayCareView.Visible = false;
                        UniqueBox.Visible = true;
                        UniqueBox.Text = "Raid Lobby";
                        UniqueBox.Enabled = false;
                        OverworldView.Visible = true;
                        OverworldView.Enabled = false;
                        DayCareView.Visible = true;
                        break;
                    }
                case VioletID:
                    {
                        url = url + "VI.png";
                        type = (int)GameSelected.Violet;
                        WideView.Visible = false;
                        WideView.Text = "Raid View";
                        View.Enabled= false;
                        TrainerView.Visible = false;
                        DayCareView.Visible = false;
                        UniqueBox.Visible = true;
                        UniqueBox.Text = "Raid Lobby";
                        UniqueBox.Enabled = false;
                        OverworldView.Visible = true;
                        OverworldView.Enabled = false;
                        DayCareView.Visible = true;
                        break;
                    }
                case LegendsArceusID:
                    {
                        url = url + "LA.png";
                        type = (int)GameSelected.LA; 
                        TrainerView.Visible = false; 
                        DayCareView.Visible = false;
                        OverworldView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
                case ShiningPearlID:
                    {
                        url = url + "SP.png"; 
                        type = (int)GameSelected.SP; 
                        TrainerView.Visible = false;
                        OverworldView.Visible = false;
                        DayCareView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
                case BrilliantDiamondID:
                    {
                        url = url + "BD.png"; 
                        type = (int)GameSelected.BD; 
                        TrainerView.Visible = false;
                        OverworldView.Visible = false;
                        DayCareView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
                case SwordID:
                    {
                        url = url + "SW.png"; 
                        type = (int)GameSelected.SW; 
                        UniqueBox.Visible = true; UniqueBox2.Visible = true; 
                        UniqueBox.Text = "Raid"; UniqueBox2.Text = "Curry";
                        OverworldView.Visible = false;
                        DayCareView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
                case ShieldID:
                    {
                        url = url + "SH.png"; 
                        type = (int)GameSelected.SH; 
                        UniqueBox.Visible = true; UniqueBox2.Visible = true; 
                        UniqueBox.Text = "Raid"; UniqueBox2.Text = "Curry";
                        OverworldView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
                case EeveeID:
                    {
                        url = url + "LGE.png"; type = (int)GameSelected.LGE; 
                        WideView.Enabled = false;
                        DayCareView.Visible = false;
                        OverworldView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
                case PikachuID:
                    {
                        url = url + "LGP.png"; type = (int)GameSelected.LGP; 
                        WideView.Enabled = false;
                        DayCareView.Visible = false;
                        OverworldView.Visible = false;
                        Raids.Visible = false;
                        break;
                    }
            }

            OriginIcon.ImageLocation = url;            
            GameType = type;
            ViewBox.Text = "Click View!";
            var bg = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
            PokeSprite.ImageLocation = bg;
        }

        public async Task<string> GetTitleID(CancellationToken token)
        {
            var bytes = await SwitchConnection.ReadRaw(SwitchCommand.GetTitleID(), 17, token).ConfigureAwait(false);
            return Encoding.ASCII.GetString(bytes).Trim();
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

        private void WideView_Click(object sender, EventArgs e)
        {
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    {
                        MessageBox.Show("Wide View currently not supported");
                        break;
                    }
                case (int)GameSelected.LA:
                    {
                        using WideViewerLA WideForm = new();
                        WideForm.ShowDialog();
                        break;
                    }
                case (int)GameSelected.BD or (int)GameSelected.SP:
                    {
                        using WideViewerBDSP WideForm = new();
                        WideForm.ShowDialog();
                        break;
                    }
                case (int)GameSelected.SW or (int)GameSelected.SH:
                    {
                        WideView.Text = "Preparing...";
                        WideView.Enabled = false;
                        using WideViewerSWSH WideForm = new();
                        WideForm.ShowDialog();
                        WideView.Text = "WideView";
                        WideView.Enabled = true;
                        break;
                    }
            }
        }

        public async Task<ulong> GetPointerAddress(string pointer, CancellationToken token, bool heaprealtive = false) //Code from LiveHex
        {
            var ptr = pointer;
            if (string.IsNullOrWhiteSpace(ptr) || ptr.IndexOfAny(new char[] { '-', '/', '*' }) != -1)
                return 0;
            while (ptr.Contains("]]"))
                ptr = ptr.Replace("]]", "]+0]");
            uint finadd = 0;
            if (!ptr.EndsWith("]"))
            {
                finadd = Util.GetHexValue(ptr.Split('+').Last());
                ptr = ptr[..ptr.LastIndexOf('+')];
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
            address += finadd;
            if (heaprealtive)
            {
                ulong heap = await SwitchConnection.GetHeapBaseAsync(token);
                address -= heap;
            }
            return address;
        }

        private void BoxView_Click(object sender, EventArgs e)
        {
            using BoxViewerMode BoxForm = new(GameType);
            BoxForm.ShowDialog();
        }

        private void BattleView_Click(object sender, EventArgs e)
        {
            using TrainerViewer Form = new(GameType);
            Form.ShowDialog();
        }

        private void OverworldView_Click(object sender, EventArgs e)
        {
            using OverworldViewSV WideForm = new();
            WideForm.ShowDialog();
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
            using Egg_Viewer WideForm = new();
            WideForm.ShowDialog();
        }

        private void Raids_Click(object sender, EventArgs e)
        {
            using RaidCodeEntry WideForm = new();
            WideForm.ShowDialog();
        }
    }
}