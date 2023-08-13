using PKHeX.Core;
using PKHeX.Drawing.PokeSprite;
using PokeViewer.NET.Properties;
using SysBot.Base;
using System.Text;
using Newtonsoft.Json;
using static SysBot.Base.SwitchButton;
using static System.Buffers.Binary.BinaryPrimitives;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.SubForms
{
    public partial class MiscView : Form
    {
        private readonly ViewerExecutor Executor;
        private byte[]? CenterPOS = Array.Empty<byte>();
        private int V_Form;
        private static ulong BaseBlockKeyPointer = 0;
        public ulong CountCache;
        private DateTime StartTime;
        private System.Timers.Timer timer = new();
        public List<OutbreakStash> OutbreakCache = new();
        private List<Image> MapSprites = new();
        private List<byte[]?> MapPOS = new();
        private List<PK9> pkList = new();
        protected ViewerOffsets Offsets { get; } = new();
        public MiscView(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            SpeciesBox.DataSource = Enum.GetValues(typeof(Species)).Cast<Species>().Where(z => z != Species.MAX_COUNT).ToArray();
            var path = "outbreakfilters.txt";
            if (File.Exists(path))
                LoadFilters(path);

            OverShootUpDown.Value = Settings.Default.MiscOvershoot;
            LoadOutbreakCache();
        }

        private void LoadFilters(string data)
        {
            string contents = File.ReadAllText(data);
            string[] monlist = contents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string mons in monlist)
            {
                string[] mon = mons.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                PK9 pk = new()
                {
                    Species = Convert.ToUInt16(mon[0]),
                    Form = (byte)Convert.ToUInt16(mon[1])
                };
                pkList.Add(pk);
            }
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            OutbreakScan.BackColor = color.Item1;
            OutbreakScan.ForeColor = color.Item2;
            OpenMap.BackColor = color.Item1;
            OpenMap.ForeColor = color.Item2;
            ViewList.BackColor = color.Item1;
            ViewList.ForeColor = color.Item2;
            SaveList.BackColor = color.Item1;
            SaveList.ForeColor = color.Item2;
            ClearList.BackColor = color.Item1;
            ClearList.ForeColor = color.Item2;
            ChangeFormButton.BackColor = color.Item1;
            ChangeFormButton.ForeColor = color.Item2;
            ReadValues.BackColor = color.Item1;
            ReadValues.ForeColor = color.Item2;
            AddSpecies.BackColor = color.Item1;
            AddSpecies.ForeColor = color.Item2;
            RemoveSpecies.BackColor = color.Item1;
            RemoveSpecies.ForeColor = color.Item2;
            groupBox1.BackColor = color.Item1;
            groupBox1.ForeColor = color.Item2;
            groupBox2.BackColor = color.Item1;
            groupBox2.ForeColor = color.Item2;
            groupBox3.BackColor = color.Item1;
            groupBox3.ForeColor = color.Item2;
            groupBox4.BackColor = color.Item1;
            groupBox4.ForeColor = color.Item2;
            DPADUpDown.BackColor = color.Item1;
            DPADUpDown.ForeColor = color.Item2;
            OverShootUpDown.BackColor = color.Item1;
            OverShootUpDown.ForeColor = color.Item2;
            SpeciesBox.BackColor = color.Item1;
            SpeciesBox.ForeColor = color.Item2;
            FormBox.BackColor = color.Item1;
            FormBox.ForeColor = color.Item2;
            V_ComboBox.BackColor = color.Item1;
            V_ComboBox.ForeColor = color.Item2;
        }

        public void LoadOutbreakCache()
        {
            for (int i = 0; i < 8; i++)
                OutbreakCache.Add(new());
        }

        private async Task SearchForOutbreak(CancellationToken token)
        {
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);

            Settings.Default.MiscOvershoot = OverShootUpDown.Value;
            Settings.Default.Save();

            Label[] list = { Ob1Results, Ob2Results, Ob3Results, Ob4Results, Ob5Results, Ob6Results, Ob7Results, Ob8Results };
            PictureBox[] spritelist = { OBSprite1, OBSprite2, OBSprite3, OBSprite4, OBSprite5, OBSprite6, OBSprite7, OBSprite8 };
            List<Image> imagestrings = new();
            List<byte[]?> POSlist = new();
            List<byte[]?> dummylist = new();
            List<uint> kolist = new();
            List<uint> totallist = new();
            List<string> strings = new();
            List<PK9> mons = new();
            List<PK9> prevmons = new();
            DisableAssets();
            CenterPOS = Array.Empty<byte>();
            int dayskip = 0;
            int failskip = 0;
            while (!token.IsCancellationRequested)
            {
                if (dayskip % 10 == 0)
                {
                    OutbreakCache = new();
                    LoadOutbreakCache();
                    CountCache = 0;
                }

                if (HardStopOutbreak.Checked)
                {
                    MessageBox.Show("HardStop enabled, ending task. Uncheck if you wish to scan until match is found.");
                    break;
                }

                for (int i = 0; i < list.Length; i++)
                {
                    spritelist[i].Image = null;
                    list[i].Text = string.Empty;
                }
                TotalOutbreaks.Text = string.Empty;

                DaySkipStats.Text = "Status: Saving...";
                UpdateProgress(10, 100);
                await SVSaveGameOverworld(token).ConfigureAwait(false);
                DaySkipStats.Text = "Status: Scanning...";
                var data = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalEnabled, CountCache, token).ConfigureAwait(false);
                UpdateProgress(20, 100);
                if (CountCache == 0)
                    CountCache = data.Item2;
                var Outbreaktotal = Convert.ToInt32(data.Item1);
                if (Outbreaktotal > 8)
                {
                    BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                    // Rerun in case of bad pointer
                    OutbreakCache = new();
                    LoadOutbreakCache();
                    CountCache = 0;
                    continue;
                }

                TotalOutbreaks.Text = $"Active Outbreaks: {Outbreaktotal}";
                var block = Blocks.KOutbreakSpecies1;
                var koblock = Blocks.KMassOutbreakKO1;
                var totalblock = Blocks.KMassOutbreak01TotalSpawns;
                var formblock = Blocks.KMassOutbreak01Form;
                var pos = Blocks.KMassOutbreak01CenterPos;
                for (int i = 0; i < 8; i++)
                {
                    UpdateProgress(20 + (10 * i), 100);
                    switch (i)
                    {
                        case 0: break;
                        case 1: block = Blocks.KOutbreakSpecies2; formblock = Blocks.KMassOutbreak02Form; koblock = Blocks.KMassOutbreakKO2; totalblock = Blocks.KMassOutbreak02TotalSpawns; pos = Blocks.KMassOutbreak02CenterPos; break;
                        case 2: block = Blocks.KOutbreakSpecies3; formblock = Blocks.KMassOutbreak03Form; koblock = Blocks.KMassOutbreakKO3; totalblock = Blocks.KMassOutbreak03TotalSpawns; pos = Blocks.KMassOutbreak03CenterPos; break;
                        case 3: block = Blocks.KOutbreakSpecies4; formblock = Blocks.KMassOutbreak04Form; koblock = Blocks.KMassOutbreakKO4; totalblock = Blocks.KMassOutbreak04TotalSpawns; pos = Blocks.KMassOutbreak04CenterPos; break;
                        case 4: block = Blocks.KOutbreakSpecies5; formblock = Blocks.KMassOutbreak05Form; koblock = Blocks.KMassOutbreakKO5; totalblock = Blocks.KMassOutbreak05TotalSpawns; pos = Blocks.KMassOutbreak05CenterPos; break;
                        case 5: block = Blocks.KOutbreakSpecies6; formblock = Blocks.KMassOutbreak06Form; koblock = Blocks.KMassOutbreakKO6; totalblock = Blocks.KMassOutbreak06TotalSpawns; pos = Blocks.KMassOutbreak06CenterPos; break;
                        case 6: block = Blocks.KOutbreakSpecies7; formblock = Blocks.KMassOutbreak07Form; koblock = Blocks.KMassOutbreakKO7; totalblock = Blocks.KMassOutbreak07TotalSpawns; pos = Blocks.KMassOutbreak07CenterPos; break;
                        case 7: block = Blocks.KOutbreakSpecies8; formblock = Blocks.KMassOutbreak08Form; koblock = Blocks.KMassOutbreakKO8; totalblock = Blocks.KMassOutbreak08TotalSpawns; pos = Blocks.KMassOutbreak08CenterPos; break;
                    }
                    if (i > Outbreaktotal - 1)
                        continue;

                    var (kocount, lofs) = await ReadEncryptedBlockUint(koblock, OutbreakCache[i].SpeciesKOCountLoaded, token).ConfigureAwait(false);
                    OutbreakCache[i].SpeciesKOCountLoaded = lofs;
                    var (totalcount, tofs) = await ReadEncryptedBlockUint(totalblock, OutbreakCache[i].SpeciesTotalCountLoaded, token).ConfigureAwait(false);
                    OutbreakCache[i].SpeciesTotalCountLoaded = tofs;
                    var (species, sofs) = await ReadEncryptedBlockUint(block, OutbreakCache[i].SpeciesLoaded, token).ConfigureAwait(false);
                    OutbreakCache[i].SpeciesLoaded = sofs;
                    var (form, fofs) = await ReadEncryptedBlockByte(formblock, OutbreakCache[i].SpeciesFormLoaded, token).ConfigureAwait(false);
                    OutbreakCache[i].SpeciesFormLoaded = fofs;
                    var (obpos, bofs) = await ReadEncryptedBlockArray(pos, OutbreakCache[i].SpeciesCenterPOSLoaded, token).ConfigureAwait(false);
                    OutbreakCache[i].SpeciesCenterPOSLoaded = bofs;
                    var (obdos, dofs) = await ReadEncryptedBlockArray(pos, OutbreakCache[i].SpeciesDummyPOSLoaded, token).ConfigureAwait(false);
                    OutbreakCache[i].SpeciesDummyPOSLoaded = dofs;

                    PK9 pk = new()
                    {
                        Species = SpeciesConverter.GetNational9((ushort)species),
                        Form = form,
                    };
                    CommonEdits.SetIsShiny(pk, false);
                    string pkform = form is 0 ? "" : $"-{form}";
                    strings.Add($"{(Species)pk.Species}{pkform}{Environment.NewLine}Count: {kocount}/{totalcount}");
                    var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                    imagestrings.Add(img);
                    mons.Add(pk);
                    POSlist.Add(obpos);
                    dummylist.Add(obdos);
                    kolist.Add(kocount);
                    totallist.Add(totalcount);
                }

                UpdateProgress(100, 100);

                if (prevmons.ToList().SequenceEqual(mons.ToList()))
                {
                    DaySkipStats.Text = "Status: Skip fail!";
                    failskip++;
                    continue;
                }

                prevmons = mons;

                for (int i = 0; i < imagestrings.Count; i++)
                {
                    spritelist[i].Image = imagestrings[i];
                    list[i].Text = strings[i].ToString();
                }

                DaySkipStats.Text = "Status:";

                foreach (var ips in imagestrings)
                    MapSprites.Add(ips);
                foreach (var pps in POSlist)
                    MapPOS.Add(pps);

                for (int i = 0; i < mons.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if (p.Species == mons[i].Species && p.Form == mons[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        CollideButton.Enabled = true;
                        timer.Stop();
                        CenterPOS = POSlist[i];

                        string msg = $"{(Species)mons[i].Species} outbreak found!";
                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(mons[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        EnableAssets();
                        timer.Stop();

                        if (Apply0To61.Checked)
                        {
                            DaySkipStats.Text = "Status: 0 -> 61...";
                            await PrepareZeroToSixtyOne(token).ConfigureAwait(false);
                            DaySkipStats.Text = "Status:";
                        }
                        return;
                    }
                }

                imagestrings = new();
                POSlist = new();
                strings = new();
                mons = new();
                kolist = new();
                dummylist = new();
                totallist = new();
                MapSprites = new();
                MapPOS = new();
                dayskip++;
                DaySkipTotal.Text = $"Day Skips: {dayskip - failskip}/{dayskip}";

                if (HardStopOutbreak.Checked)
                {
                    MessageBox.Show("HardStop enabled, ending task. Uncheck if you wish to scan until match is found.");
                    {
                        EnableAssets();
                        return;
                    }
                }
                if (OutbreakSearch.Checked)
                {
                    DaySkipStats.Text = "Status: Skipping...";
                    await DaySkip(token).ConfigureAwait(false);
                    await Task.Delay(2_000, token).ConfigureAwait(false);
                }
                else if (!OutbreakSearch.Checked)
                    break;
            }
            EnableAssets();
            timer.Stop();
        }

        private void UpdateProgress(int currProgress, int maxProgress)
        {
            var value = (100 * currProgress) / maxProgress;
            if (progressBar1.InvokeRequired)
                progressBar1.Invoke(() => progressBar1.Value = value);
            else
                progressBar1.Value = value;
        }

        private async Task PrepareZeroToSixtyOne(CancellationToken token)
        {
            await CloseGame(token).ConfigureAwait(false);
            await StartGameScreen(token).ConfigureAwait(false);
            await KOToSixty_Click(token).ConfigureAwait(false);
            await Click(A, 15_000, token).ConfigureAwait(false);
        }

        public async Task CloseGame(CancellationToken token)
        {
            // Close out of the game
            await Click(B, 0_500, token).ConfigureAwait(false);
            await Click(HOME, 2_000, token).ConfigureAwait(false);
            await Click(X, 1_000, token).ConfigureAwait(false);
            await Click(A, 5_000, token).ConfigureAwait(false);
        }

        public async Task StartGameScreen(CancellationToken token)
        {
            // Open game.
            await Click(A, 1_000, token).ConfigureAwait(false);

            await Click(A, 1_000, token).ConfigureAwait(false);
            // If they have DLC on the system and can't use it, requires an UP + A to start the game.
            // Should be harmless otherwise since they'll be in loading screen.
            await Click(DUP, 0_600, token).ConfigureAwait(false);
            await Click(A, 0_600, token).ConfigureAwait(false);

            // Switch Logo and game load screen
            await Task.Delay(19_000, token).ConfigureAwait(false);
        }

        private async Task KOToSixty_Click(CancellationToken token)
        {
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var (validOutbreaks, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalEnabled, 0, token).ConfigureAwait(false);
            var Outbreaktotal = Convert.ToInt32(validOutbreaks);
            var koblock = Blocks.KMassOutbreakKO1;
            for (int i = 0; i < 8; i++)
            {
                TotalOutbreaks.Text = $"KO-ing...: {12.5 * i + 1}%";
                switch (i)
                {
                    case 0: break;
                    case 1: koblock = Blocks.KMassOutbreakKO2; break;
                    case 2: koblock = Blocks.KMassOutbreakKO3; break;
                    case 3: koblock = Blocks.KMassOutbreakKO4; break;
                    case 4: koblock = Blocks.KMassOutbreakKO5; break;
                    case 5: koblock = Blocks.KMassOutbreakKO6; break;
                    case 6: koblock = Blocks.KMassOutbreakKO7; break;
                    case 7: koblock = Blocks.KMassOutbreakKO8; break;
                }
                if (i > Outbreaktotal - 1)
                    continue;
                var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                uint inj = 61;
                await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
            }
            TotalOutbreaks.Text = "Done.";
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            StartTime = DateTime.Now;
            UptimeOnLoad(sender, e);
            await SearchForOutbreak(CancellationToken.None).ConfigureAwait(false);
        }

        private void UptimeOnLoad(object sender, EventArgs e)
        {
            timer = new System.Timers.Timer { Interval = 1000 };
            timer.Elapsed += (o, args) =>
            {
                UptimeLabel.Text = $"Uptime: {StartTime - DateTime.Now:d\\.hh\\:mm\\:ss}";
            };
            timer.Start();
        }

        private async void CollideButton_Click(object sender, EventArgs e)
        {
            if (CenterPOS is not null)
            {
                float Y = BitConverter.ToSingle(CenterPOS, 4);
                Y += 40;
                WriteSingleLittleEndian(CenterPOS.AsSpan()[4..], Y);

                for (int i = 0; i < 15; i++)
                    await Executor.SwitchConnection.PointerPoke(CenterPOS, CollisionPointer, CancellationToken.None).ConfigureAwait(false);
            }

            if (CenterPOS is null)
            {
                MessageBox.Show("No valid coordinates present. Try again after finding a desired outbreak.");
                CollideButton.Enabled = false;
            }

        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var (vivform, _) = await ReadEncryptedBlockByte(Blocks.KGOVivillonForm, 0, token).ConfigureAwait(false);
            var (epochtime, _) = await ReadEncryptedBlockUint(Blocks.KGOLastConnected, 0, token).ConfigureAwait(false);

            var inj = (byte)V_Form;
            if (inj != vivform)
                await WriteBlock(inj, Blocks.KGOVivillonForm, token, vivform).ConfigureAwait(false);
            var (newform, _) = await ReadEncryptedBlockByte(Blocks.KGOVivillonForm, 0, token).ConfigureAwait(false);

            TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
            uint currentEpoch = (uint)t.TotalSeconds;

            await WriteBlock(currentEpoch, Blocks.KGOLastConnected, token, epochtime).ConfigureAwait(false);
            var (newtime, _) = await ReadEncryptedBlockUint(Blocks.KGOLastConnected, 0, token).ConfigureAwait(false);

            await WriteBlock(true, Blocks.KGOVivillonFormEnabled, token, false).ConfigureAwait(false);

            string vivmsg = inj != vivform ? $"Vivillon form has been changed from {(VivForms)vivform} to {(VivForms)newform}." : "Modified form is the same as the current form.";
            MessageBox.Show($"{vivmsg}{Environment.NewLine}KGOLastConnected TimeStamp changed from {FromUnixTime(epochtime)} to {FromUnixTime(newtime)}");
        }

        private async void ReadValues_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var forcevivform = await ReadEncryptedBlockBool(Blocks.KGOVivillonFormEnabled, token).ConfigureAwait(false);
            var (vivform, _) = await ReadEncryptedBlockByte(Blocks.KGOVivillonForm, 0, token).ConfigureAwait(false);
            var (epochtime, _) = await ReadEncryptedBlockUint(Blocks.KGOLastConnected, 0, token).ConfigureAwait(false);

            MessageBox.Show($"KGOVivillonFormEnabled: {forcevivform}{Environment.NewLine}" +
                $"KGOVivillonForm: {(VivForms)vivform}{Environment.NewLine}KGOLastConnected TimeStamp: {FromUnixTime(epochtime)}");
        }

        public class OutbreakStash
        {
            public ulong SpeciesLoaded { get; set; } = 0;
            public ulong SpeciesFormLoaded { get; set; } = 0;
            public ulong SpeciesTotalCountLoaded { get; set; } = 0;
            public ulong SpeciesKOCountLoaded { get; set; } = 0;
            public ulong SpeciesCenterPOSLoaded { get; set; } = 0;
            public ulong SpeciesDummyPOSLoaded { get; set; } = 0;
        }

        public class OutbreakData
        {
            public byte[]? CenterPOS { get; set; } = Array.Empty<byte>();
            public byte[]? DummyPOS { get; set; } = Array.Empty<byte>();
            public ushort Species { get; set; } = 0;
            public byte Form { get; set; } = (byte)0;
            public uint KoCount { get; set; } = 0;
            public uint TotalCount { get; set; } = 0;
        }

        public class DataBlock
        {
            public string? Name { get; set; }
            public uint Key { get; set; }
            public SCTypeCode Type { get; set; }
            public SCTypeCode SubType { get; set; }
            public IReadOnlyList<long>? Pointer { get; set; }
            public bool IsEncrypted { get; set; }
            public int Size { get; set; }
        }

        public static class Blocks
        {
            public static DataBlock KMassOutbreakTotalEnabled = new()
            {
                Name = "KMassOutbreakTotalEnabled",
                Key = 0x6C375C8A,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            #region Outbreak1
            public static DataBlock KOutbreakSpecies1 = new()
            {
                Name = "KOutbreakSpecies1",
                Key = 0x76A2F996,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak01Form = new()
            {
                Name = "KMassOutbreak01Form",
                Key = 0x29B4615D,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO1 = new()
            {
                Name = "KMassOutbreak01NumKOed",
                Key = 0x4B16FBC2,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak01TotalSpawns = new()
            {
                Name = "KMassOutbreak01TotalSpawns",
                Key = 0xB7DC495A,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak01CenterPos = new()
            {
                Name = "KMassOutbreak01CenterPos",
                Key = 0x2ED42F4D,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak01DummyPos = new()
            {
                Name = "KMassOutbreak01DummyPos",
                Key = 0x4A13BE7C,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak2
            public static DataBlock KOutbreakSpecies2 = new()
            {
                Name = "KOutbreakSpecies2",
                Key = 0x76A0BCF3,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak02Form = new()
            {
                Name = "KMassOutbreak02Form",
                Key = 0x29B84368,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO2 = new()
            {
                Name = "KMassOutbreak02NumKOed",
                Key = 0x4B14BF1F,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak02TotalSpawns = new()
            {
                Name = "KMassOutbreak02TotalSpawns",
                Key = 0xB7DA0CB7,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak02CenterPos = new()
            {
                Name = "KMassOutbreak02CenterPos",
                Key = 0x2ED5F198,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak02DummyPos = new()
            {
                Name = "KMassOutbreak02DummyPos",
                Key = 0x4A118F71,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak3
            public static DataBlock KOutbreakSpecies3 = new()
            {
                Name = "KOutbreakSpecies3",
                Key = 0x76A97E38,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak03Form = new()
            {
                Name = "KMassOutbreak03Form",
                Key = 0x29AF8223,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO3 = new()
            {
                Name = "KMassOutbreak03NumKOed",
                Key = 0x4B1CA6E4,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak03TotalSpawns = new()
            {
                Name = "KMassOutbreak03TotalSpawns",
                Key = 0xB7E1F47C,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak03CenterPos = new()
            {
                Name = "KMassOutbreak03CenterPos",
                Key = 0x2ECE09D3,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak03DummyPos = new()
            {
                Name = "KMassOutbreak03DummyPos",
                Key = 0x4A0E135A,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak4
            public static DataBlock KOutbreakSpecies4 = new()
            {
                Name = "KOutbreakSpecies4",
                Key = 0x76A6E26D,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak04Form = new()
            {
                Name = "KMassOutbreak04Form",
                Key = 0x29B22B86,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO4 = new()
            {
                Name = "KMassOutbreak04NumKOed",
                Key = 0x4B1A77D9,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak04TotalSpawns = new()
            {
                Name = "KMassOutbreak04TotalSpawns",
                Key = 0xB7DFC571,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak04CenterPos = new()
            {
                Name = "KMassOutbreak04CenterPos",
                Key = 0x2ED04676,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak04DummyPos = new()
            {
                Name = "KMassOutbreak04DummyPos",
                Key = 0x4A0BD6B7,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak5
            public static DataBlock KOutbreakSpecies5 = new()
            {
                Name = "KOutbreakSpecies5",
                Key = 0x76986F3A,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak05Form = new()
            {
                Name = "KMassOutbreak05Form",
                Key = 0x29A9D701,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO5 = new()
            {
                Name = "KMassOutbreak05NumKOed",
                Key = 0x4B23391E,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak05TotalSpawns = new()
            {
                Name = "KMassOutbreak05TotalSpawns",
                Key = 0xB7E886B6,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak05CenterPos = new()
            {
                Name = "KMassOutbreak05CenterPos",
                Key = 0x2EC78531,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak05DummyPos = new()
            {
                Name = "KMassOutbreak05DummyPos",
                Key = 0x4A1FFBD8,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak6
            public static DataBlock KOutbreakSpecies6 = new()
            {
                Name = "KOutbreakSpecies6",
                Key = 0x76947F97,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak06Form = new()
            {
                Name = "KMassOutbreak06Form",
                Key = 0x29AB994C,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO6 = new()
            {
                Name = "KMassOutbreak06NumKOed",
                Key = 0x4B208FBB,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak06TotalSpawns = new()
            {
                Name = "KMassOutbreak06TotalSpawns",
                Key = 0xB7E49713,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak06CenterPos = new()
            {
                Name = "KMassOutbreak06CenterPos",
                Key = 0x2ECB673C,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak06DummyPos = new()
            {
                Name = "KMassOutbreak06DummyPos",
                Key = 0x4A1C868D,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak7
            public static DataBlock KOutbreakSpecies7 = new()
            {
                Name = "KOutbreakSpecies7",
                Key = 0x769D40DC,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak07Form = new()
            {
                Name = "KMassOutbreak07Form",
                Key = 0x29A344C7,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO7 = new()
            {
                Name = "KMassOutbreak07NumKOed",
                Key = 0x4B28E440,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak07TotalSpawns = new()
            {
                Name = "KMassOutbreak07TotalSpawns",
                Key = 0xB7EE31D8,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak07CenterPos = new()
            {
                Name = "KMassOutbreak07CenterPos",
                Key = 0x2EC1CC77,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak07DummyPos = new()
            {
                Name = "KMassOutbreak07DummyPos",
                Key = 0x4A1A50B6,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Outbreak8
            public static DataBlock KOutbreakSpecies8 = new()
            {
                Name = "KOutbreakSpecies8",
                Key = 0x769B11D1,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak08Form = new()
            {
                Name = "KMassOutbreak08Form",
                Key = 0x29A5EE2A,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KMassOutbreakKO8 = new()
            {
                Name = "KMassOutbreak08NumKOed",
                Key = 0x4B256EF5,
                Type = SCTypeCode.UInt32,
                IsEncrypted = true,
                Size = 4,
            };
            public static DataBlock KMassOutbreak08TotalSpawns = new()
            {
                Name = "KMassOutbreak08TotalSpawns",
                Key = 0xB7EABC8D,
                Type = SCTypeCode.Int32,
            };
            public static DataBlock KMassOutbreak08CenterPos = new()
            {
                Name = "KMassOutbreak08CenterPos",
                Key = 0x2EC5BC1A,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            public static DataBlock KMassOutbreak08DummyPos = new()
            {
                Name = "KMassOutbreak08DummyPos",
                Key = 0x4A166113,
                Type = SCTypeCode.Array,
                IsEncrypted = true,
                Size = 12,
            };
            #endregion
            #region Vivillon
            public static DataBlock KGOVivillonFormEnabled = new()
            {
                Name = "KGOVivillonFormEnabled",
                Key = 0x0C125D5C,
                Type = SCTypeCode.Bool1,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KGOTransfer = new()
            {
                Name = "KGOTransfer",
                Key = 0x7EE0A576,
                Type = SCTypeCode.Object,
                IsEncrypted = true,
                Size = 0x3400,
            };
            public static DataBlock FSYS_GO_LINK_ENABLED = new()
            {
                Name = "FSYS_GO_LINK_ENABLED",
                Key = 0x3ABC21E3,
                Type = SCTypeCode.Bool1,
                IsEncrypted = true,
                Size = 1,
            };

            public static DataBlock KGOVivillonForm = new()
            {
                Name = "KGOVivillonForm",
                Key = 0x22F70BCF,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KGOLastConnected = new()
            {
                Name = "KGOLastConnected",
                Key = 0x867F0240,
                Type = SCTypeCode.UInt64,
                IsEncrypted = true,
                Size = 8,
            };
            #endregion
        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.Connection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        public async Task PressAndHold(SwitchButton b, int hold, int delay, CancellationToken token)
        {
            await Executor.Connection.SendAsync(SwitchCommand.Hold(b, true), token).ConfigureAwait(false);
            await Task.Delay(hold, token).ConfigureAwait(false);
            await Executor.Connection.SendAsync(SwitchCommand.Release(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task DaySkip(CancellationToken token)
        {
            for (int i = 0; i < 2; i++)
                await Click(B, 0_150, token).ConfigureAwait(false);
            await Click(HOME, 2_000, token).ConfigureAwait(false); // Back to title screen

            for (int i = 0; i < 2; i++)
                await Click(DRIGHT, 0_150, token).ConfigureAwait(false);
            await Click(DDOWN, 0_150, token).ConfigureAwait(false);
            await Click(DRIGHT, 0_150, token).ConfigureAwait(false);
            await Click(A, 1_250, token).ConfigureAwait(false); // Enter settings

            await PressAndHold(DDOWN, 2_000, 0_250, token).ConfigureAwait(false); // Scroll to system settings
            await Click(A, 1_250, token).ConfigureAwait(false);

            if (OverShootButton.Checked)
            {
                await PressAndHold(DDOWN, (int)OverShootUpDown.Value, 0_100, token).ConfigureAwait(false);
                await Click(DUP, 0_500, token).ConfigureAwait(false);
            }
            else if (DDOWNButton.Checked)
            {
                for (int i = 0; i < DPADUpDown.Value; i++)
                    await Click(DDOWN, 0_100, token).ConfigureAwait(false);
            }

            await Click(A, 1_250, token).ConfigureAwait(false);
            for (int i = 0; i < 2; i++)
                await Click(DDOWN, 0_250, token).ConfigureAwait(false);
            await Click(A, 0_500, token).ConfigureAwait(false);

            for (int i = 0; i < 8; i++) // Mash DRIGHT to confirm
                await Click(DRIGHT, 0_200, token).ConfigureAwait(false);

            await Click(A, 0_200, token).ConfigureAwait(false); // Confirm date/time change
            await Click(HOME, 1_000, token).ConfigureAwait(false);
            await Click(A, 2_000, token).ConfigureAwait(false); // Back to title screen
            /*for (int i = 0; i < 4; i++)
                await Click(DUP, 0_250, token).ConfigureAwait(false);
            await Click(A, 1_000, token).ConfigureAwait(false); // Back to title screen
            await Click(B, 1_000, token).ConfigureAwait(false);*/
        }

        public async Task SetStick(SwitchStick stick, short x, short y, int hold, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.SetStick(stick, x, y, true), token).ConfigureAwait(false);
            await Task.Delay(hold, token).ConfigureAwait(false);
            await Executor.SwitchConnection.SendAsync(SwitchCommand.SetStick(stick, 0, 0, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private static HttpClient? _client;
        private static HttpClient Client
        {
            get
            {
                _client ??= new HttpClient();
                return _client;
            }
        }

        private static string[]? DiscordWebhooks;

        private async void SendNotifications(string results, string thumbnail)
        {
            if (string.IsNullOrEmpty(results) || string.IsNullOrEmpty(Settings.Default.WebHook))
                return;
            DiscordWebhooks = Settings.Default.WebHook.Split(',');
            if (DiscordWebhooks == null)
                return;

            var webhook = GenerateWebhook(results, thumbnail);
            var content = new StringContent(JsonConvert.SerializeObject(webhook), Encoding.UTF8, "application/json");
            foreach (var url in DiscordWebhooks)
                await Client.PostAsync(url, content).ConfigureAwait(false);
        }

        private static object GenerateWebhook(string results, string thumbnail)
        {
            var WebHook = new
            {
                username = $"PokeViewer.NET",
                content = $"<@{Settings.Default.UserDiscordID}>",
                embeds = new List<object>
                {
                    new
                    {
                        title = $"Match Found!",
                        thumbnail = new
                        {
                            url = thumbnail
                        },
                        fields = new List<object>
                        {
                            new { name = "Description               ", value = results, inline = true, },
                        },
                    }
                }
            };
            return WebHook;
        }

        // Read, Decrypt, and Write Block tasks from Tera-Finder/RaidCrawler/sv-livemap.
        #region saveblocktasks
        public static byte[] DecryptBlock(uint key, byte[] block)
        {
            var rng = new SCXorShift32(key);
            for (int i = 0; i < block.Length; i++)
                block[i] = (byte)(block[i] ^ rng.Next());
            return block;
        }

        private async Task<(byte, ulong)> ReadEncryptedBlockByte(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (header[1], address);
        }

        private async Task<(byte[], ulong)> ReadEncryptedBlockHeader(DataBlock block, ulong init, CancellationToken token)
        {
            if (init == 0)
            {
                var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
                init = address;
            }

            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(init, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);

            return (header, init);
        }

        private async Task<(byte[]?, ulong)> ReadEncryptedBlockArray(DataBlock block, ulong init, CancellationToken token)
        {
            if (init == 0)
            {
                var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
                init = address;
            }

            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(init, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return (data[6..], init);
        }

        private async Task<(uint, ulong)> ReadEncryptedBlockUint(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadUInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        private async Task<(int, ulong)> ReadEncryptedBlockInt32(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        private async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data);
            return res[0] == 2;
        }

        private async Task<byte[]> ReadBlock(DataBlock block, CancellationToken token)
        {
            return await ReadEncryptedBlock(block, token).ConfigureAwait(false);
        }

        private async Task<byte[]> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return data[6..];
        }

        private async Task<byte[]?> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            var size = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5 + (int)size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data)[5..];

            return res;
        }

        public async Task<bool> WriteBlock(object data, DataBlock block, CancellationToken token, object? toExpect = default)
        {
            if (block.IsEncrypted)
                return await WriteEncryptedBlockSafe(block, toExpect, data, token).ConfigureAwait(false);
            else
                return await WriteDecryptedBlock((byte[])data!, block, token).ConfigureAwait(false);
        }

        private async Task<bool> WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            await Executor.SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockSafe(DataBlock block, object? toExpect, object toWrite, CancellationToken token)
        {
            if (toExpect == default || toWrite == default)
                return false;

            return block.Type switch
            {
                SCTypeCode.Array => await WriteEncryptedBlockArray(block, (byte[])toExpect, (byte[])toWrite, token).ConfigureAwait(false),
                SCTypeCode.Bool1 or SCTypeCode.Bool2 or SCTypeCode.Bool3 => await WriteEncryptedBlockBool(block, (bool)toExpect, (bool)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Byte or SCTypeCode.SByte => await WriteEncryptedBlockByte(block, (byte)toExpect, (byte)toWrite, token).ConfigureAwait(false),
                SCTypeCode.UInt32 or SCTypeCode.UInt64 => await WriteEncryptedBlockUint(block, (uint)toExpect, (uint)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Int32 => await WriteEncryptedBlockInt32(block, (int)toExpect, (int)toWrite, token).ConfigureAwait(false),
                _ => throw new NotSupportedException($"Block {block.Name} (Type {block.Type}) is currently not supported.")
            };
        }

        private async Task<bool> WriteEncryptedBlockUint(DataBlock block, uint valueToExpect, uint valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteUInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockInt32(DataBlock block, int valueToExpect, int valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockByte(DataBlock block, byte valueToExpect, byte valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = header[1];
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            header[1] = valueToInject;
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockArray(DataBlock block, byte[] arrayToExpect, byte[] arrayToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[6..];
            if (!ram.SequenceEqual(arrayToExpect)) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            Array.ConstrainedCopy(arrayToInject, 0, data, 6, block.Size);
            data = EncryptBlock(block.Key, data);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[0] == 2;
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            data[0] = valueToInject ? (byte)2 : (byte)1;
            data = EncryptBlock(block.Key, data);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);

            return true;
        }

        public static byte[] EncryptBlock(uint key, byte[] block) => DecryptBlock(key, block);

        public async Task<ulong> SearchSaveKey(uint key, CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(BaseBlockKeyPointer + 8, 16, token).ConfigureAwait(false);
            var start = BitConverter.ToUInt64(data.AsSpan()[..8]);
            var end = BitConverter.ToUInt64(data.AsSpan()[8..]);

            while (start < end)
            {
                var block_ct = (end - start) / 48;
                var mid = start + (block_ct >> 1) * 48;

                data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(mid, 4, token).ConfigureAwait(false);
                var found = BitConverter.ToUInt32(data);
                if (found == key)
                    return mid;

                if (found >= key)
                    end = mid;
                else start = mid + 48;
            }
            return start;
        }
        #endregion

        private async Task SVSaveGameOverworld(CancellationToken token)
        {
            await Click(X, 2_000, token).ConfigureAwait(false);
            await Click(R, 1_800, token).ConfigureAwait(false);
            await Click(A, 5_000, token).ConfigureAwait(false);
            for (int i = 0; i < 10; i++) // Mash B
                await Click(B, 0_500, token).ConfigureAwait(false);
        }

        private void EnableAssets()
        {
            OutbreakScan.Enabled = true;
            OutbreakScan.Text = "Scan";
            OutbreakSearch.Enabled = true;
            Apply0To61.Enabled = true;
            OpenMap.Enabled = true;
        }

        private void DisableAssets()
        {
            OutbreakScan.Enabled = false;
            OutbreakSearch.Enabled = false;
            Apply0To61.Enabled = false;
            OpenMap.Enabled = false;
        }

        private IReadOnlyList<long> CollisionPointer { get; } = new long[] { 0x44CCA90, 0xAD8, 0x160, 0x60, 0x100 };

        private static readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        private void V_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = V_ComboBox.SelectedIndex;
            switch (selection)
            {
                case 0: V_Form = (int)VivForms.IcySnow; break; // Icy Snow
                case 1: V_Form = (int)VivForms.Polar; break; // Polar
                case 2: V_Form = (int)VivForms.Tundra; break; // Tundra
                case 3: V_Form = (int)VivForms.Continental; break; // Continental
                case 4: V_Form = (int)VivForms.Garden; break; // Garden
                case 5: V_Form = (int)VivForms.Elegant; break; // Elegant
                case 6: V_Form = (int)VivForms.Meadow; break; // Meadow
                case 7: V_Form = (int)VivForms.Modern; break; // Modern
                case 8: V_Form = (int)VivForms.Marine; break; // Marine
                case 9: V_Form = (int)VivForms.Archipelago; break; // Archipelago
                case 10: V_Form = (int)VivForms.HighPlains; break; // High-Plains
                case 11: V_Form = (int)VivForms.Sandstorm; break;// Sandstorm
                case 12: V_Form = (int)VivForms.River; break; // River
                case 13: V_Form = (int)VivForms.Monsoon; break; // Monsoon
                case 14: V_Form = (int)VivForms.Savanna; break; // Savanna
                case 15: V_Form = (int)VivForms.Sun; break; // Sun
                case 16: V_Form = (int)VivForms.Ocean; break; // Ocean
                case 17: V_Form = (int)VivForms.Jungle; break; // Jungle
                case 18: V_Form = (int)VivForms.Fancy; break; // Fancy
            }
        }

        private void OpenMap_Click(object sender, EventArgs e)
        {
            MapViewSV form = new(MapSprites, MapPOS);
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string r = string.Empty;
            foreach (var p in pkList.ToList())
            {
                r += $"{(Species)p.Species} - {p.Form}{Environment.NewLine}";
            }
            MessageBox.Show(r, "Search List");
        }

        private void AddSpecies_Click(object sender, EventArgs e)
        {
            PK9 pk = new()
            {
                Species = (ushort)(Species)SpeciesBox.SelectedIndex,
                Form = (byte)FormBox.Value,
            };

            if (pk.Species == 0)
            {
                MessageBox.Show("You can't add a blank species!", "Search List");
                return;
            }

            if (pkList.Count == 0)
            {
                pkList.Add(pk);
                MessageBox.Show($"Added {(Species)pk.Species}", "Search List");
                return;
            }
            foreach (var p in pkList.ToList())
            {
                if (p.Species == pk.Species && p.Form == pk.Form)
                {
                    MessageBox.Show($"List contains {(Species)pk.Species} with Form {pk.Form}", "Search List");
                    continue;
                }

                if (p.Species != pk.Species)
                {
                    pkList.Add(pk);
                    MessageBox.Show($"Added {(Species)pk.Species}", "Search List");
                    return;
                }
            }
        }

        private void RemoveSpecies_Click(object sender, EventArgs e)
        {
            if (pkList.Count == 0)
                return;

            PK9 pk = new()
            {
                Species = (ushort)(Species)SpeciesBox.SelectedIndex,
                Form = (byte)FormBox.Value,
            };
            foreach (var p in pkList.ToList())
            {
                if (p.Species != pk.Species && p.Form != pk.Form)
                    continue;

                if (p.Species == pk.Species && p.Form == pk.Form)
                {
                    pkList.Remove(p);
                    MessageBox.Show($"Removed {(Species)p.Species}", "Search List");
                    return;
                }
            }
        }

        private void SaveList_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            foreach (var p in pkList.ToList())
            {
                output += $"{p.Species}-{p.Form},";
            }
            using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "outbreakfilters.txt"));
            sw.Write(output);
            MessageBox.Show("Done", "Search List");
        }

        private void ClearList_Click(object sender, EventArgs e)
        {
            pkList.Clear();
            pkList = new();
            MessageBox.Show("Cleared all filters", "Search List");
        }

        private enum VivForms
        {
            IcySnow = 0,
            Polar = 1,
            Tundra = 2,
            Continental = 3,
            Garden = 4,
            Elegant = 5,
            Meadow = 6,
            Modern = 7,
            Marine = 8,
            Archipelago = 9,
            HighPlains = 10,
            Sandstorm = 11,
            River = 12,
            Monsoon = 13,
            Savanna = 14,
            Sun = 15,
            Ocean = 16,
            Jungle = 17,
            Fancy = 18,
        }
    }
}
