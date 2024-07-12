using Newtonsoft.Json.Linq;
using Octokit;
using PKHeX.Core;
using PKHeX.Drawing.Misc;
using RaidCrawler.Core.Structures;
using SysBot.Base;
using System;
using System.Text;
using static PokeViewer.NET.RoutineExecutor;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.SubForms
{
    public partial class MiscViewSV : Form
    {
        private readonly ViewerExecutor Executor;
        private (Color, Color) FormColor;
        OutbreakViewSV? OutbreakForm;
        public static bool OutbreakFormOpen = false;

        protected ViewerOffsets Offsets { get; } = new();
        public MiscViewSV(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            FormColor = color;
            UptimeOnLoad();
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            ChangeFormButton.BackColor = color.Item1;
            ChangeFormButton.ForeColor = color.Item2;
            ReadValues.BackColor = color.Item1;
            ReadValues.ForeColor = color.Item2;
            MiscGroup.BackColor = color.Item1;
            MiscGroup.ForeColor = color.Item2;
            OutbreakBtn.BackColor = color.Item1;
            OutbreakBtn.ForeColor = color.Item2;
            FwdButton.BackColor = color.Item1;
            FwdButton.ForeColor = color.Item2;
            BwdButton.BackColor = color.Item1;
            BwdButton.ForeColor = color.Item2;
            ResetButton.BackColor = color.Item1;
            ResetButton.ForeColor = color.Item2;
            TimeNumeric.BackColor = color.Item1;
            TimeNumeric.ForeColor = color.Item2;
            TimeGroup.BackColor = color.Item1;
            TimeGroup.ForeColor = color.Item2;
            BackColor = color.Item1;
            ForeColor = color.Item2;
            ClearFCE.BackColor = color.Item1;
            ClearFCE.ForeColor = color.Item2;
            EnterButton.BackColor = color.Item1;
            EnterButton.ForeColor = color.Item2;
            AutoPaste.BackColor = color.Item1;
            AutoPaste.ForeColor = color.Item2;
            GoButton.BackColor = color.Item1;
            GoButton.ForeColor = color.Item2;
            FCETextBox.BackColor = color.Item1;
            FCETextBox.ForeColor = color.Item2;
            groupBox2.BackColor = color.Item1;
            groupBox2.ForeColor = color.Item2;
            Results.BackColor = color.Item1;
            Results.ForeColor = color.Item2;
            RaidSensCheck.BackColor = color.Item1;
            RaidSensCheck.ForeColor = color.Item2;
            SeedToPokemonGroup.BackColor = color.Item1;
            SeedToPokemonGroup.ForeColor = color.Item2;
            SeedLabel.BackColor = color.Item1;
            SeedLabel.ForeColor = color.Item2;
            RaidNumeric.BackColor = color.Item1;
            RaidNumeric.ForeColor = color.Item2;
            OutbreakGroup.BackColor = color.Item1;
            OutbreakGroup.ForeColor = color.Item2;
            WildSpawnGroup.BackColor = color.Item1;
            WildSpawnGroup.ForeColor = color.Item2;
            SnackGroup.BackColor = color.Item1;
            SnackGroup.ForeColor = color.Item2;
            SetSwitchTimeButton.BackColor = color.Item1;
            SetSwitchTimeButton.ForeColor = color.Item2;
            CurrentTimeLabel.BackColor = color.Item1;
            CurrentTimeLabel.ForeColor = color.Item2;
            HoursLabel.BackColor = color.Item1;
            HoursLabel.ForeColor = color.Item2;
        }

        private void UptimeOnLoad()
        {
            var timer = new System.Timers.Timer { Interval = 1000 };
            timer.Elapsed += (o, args) =>
            {
                CurrentTimeLabel.Text = $"Current Local Time: {DateTime.Now}";
            };
            timer.Start();
        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.Connection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async void FwdButton_Click(object sender, EventArgs e)
        {
            FwdButton.Enabled = false;
            BwdButton.Enabled = false;
            ResetButton.Enabled = false;

            for (int i = 0; i < TimeNumeric.Value; i++)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipForward(true), CancellationToken.None).ConfigureAwait(false);

            FwdButton.Enabled = true;
            BwdButton.Enabled = true;
            ResetButton.Enabled = true;
        }

        private async void BwdButton_Click(object sender, EventArgs e)
        {
            FwdButton.Enabled = false;
            BwdButton.Enabled = false;
            ResetButton.Enabled = false;

            for (int i = 0; i < TimeNumeric.Value; i++)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipBack(true), CancellationToken.None).ConfigureAwait(false);

            FwdButton.Enabled = true;
            BwdButton.Enabled = true;
            ResetButton.Enabled = true;
        }

        private async void ResetButton_Click(object sender, EventArgs e)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.ResetTime(true), CancellationToken.None).ConfigureAwait(false);
        }

        private async void EnterButton_Click(object sender, EventArgs e)
        {
            if (FCETextBox.Text.Length < 4 || FCETextBox.Text.Length == 5)
            {
                MessageBox.Show($"{FCETextBox.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(FCETextBox.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
            else
                MessageBox.Show("TextBox is empty. Try again after you fill it in!");
        }

        private async Task EnterRaidCode(CancellationToken token)
        {
            if (FCETextBox.Text.Length > 6)
            {
                FCETextBox.Text = FCETextBox.Text.Substring(0, 6);
            }
            var strokes = FCETextBox.Text.ToUpper().ToArray();
            var number = $"NumPad";
            string[] badVals = ["@", "I", "O", "=", "&", ";", "Z", "*", "#", "!", "?"];
            List<HidKeyboardKey> keystopress = [];
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

        private async void AutoPaste_Click(object sender, EventArgs e)
        {
            AutoPaste.Enabled = false;
            Clipboard.Clear();
            while (!Clipboard.ContainsText())
            {
                await Task.Delay(0_100);
            }
            FCETextBox.Text = Clipboard.GetText();
            if (!string.IsNullOrEmpty(FCETextBox.Text.Trim()))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);

            AutoPaste.Enabled = true;
        }

        private void ClearFCE_Click(object sender, EventArgs e)
        {
            FCETextBox.Text = string.Empty;
        }

        private async void GoButton_Click(object sender, EventArgs e)
        {
            GoButton.Enabled = false;
            RaidNumeric.Enabled = false;
            RaidSensCheck.Enabled = false;
            try
            {
                await ReadRaids(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            GoButton.Enabled = true;
            RaidNumeric.Enabled = true;
            RaidSensCheck.Enabled = true;

            RaidIcon.Load(raidimages[0]);
            Results.Text = results[0];
            TeraIcon.Image = teratype[0];
        }

        private void NumericValue_Changed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(raidimages[(int)RaidNumeric.Value - 1]))
            {
                RaidIcon.Load(raidimages[(int)RaidNumeric.Value - 1]);
                Results.Text = results[(int)RaidNumeric.Value - 1];
                TeraIcon.Image = teratype[(int)RaidNumeric.Value - 1];
                RaidDifficultyLabel.Text = $"{stars[(int)RaidNumeric.Value - 1]} ☆";
            }
            else
                MessageBox.Show("No results saved, please hit GO again.");
        }

        private (string, string) CalculateFromSeed(uint seed, PK9 pk)
        {
            var raidseed = RaidSensCheck.Checked ? "Hidden" : $"0x{seed:X8}";
            string ec = RaidSensCheck.Checked ? "Hidden" : $"{pk.EncryptionConstant:X8}";
            string pid = RaidSensCheck.Checked ? "Hidden" : $"{pk.PID:X8}";
            string form = string.Empty;
            if (pk.Form != 0)
                form = $"{pk.Form}";
            string results = $"{(Species)pk.Species}{form}{Environment.NewLine}" +
                $"Seed: {raidseed}{Environment.NewLine}" +
                $"EC: {ec}{Environment.NewLine}" +
                $"PID: {pid}{Environment.NewLine}" +
                $"IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}" +
                $"Gender: {(Gender)pk.Gender}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}" +
                $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pk.Scale)} ({pk.Scale})";

            var sprite = PokeImg(pk, false);
            return (sprite, results);
        }

        public async Task<ulong> SearchSaveKeyRaid(ulong BaseBlockKeyPointer, uint key, CancellationToken token)
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

        public async Task<byte[]> ReadSaveBlockRaid(ulong BaseBlockKeyPointer, uint key, int size, CancellationToken token)
        {
            var block_ofs = await SearchSaveKeyRaid(BaseBlockKeyPointer, key, token).ConfigureAwait(false);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(block_ofs + 8, 0x8, token).ConfigureAwait(false);
            block_ofs = BitConverter.ToUInt64(data, 0);

            var block = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(block_ofs, size, token).ConfigureAwait(false);
            return DecryptBlock(key, block);
        }

        public static byte[] DecryptBlock(uint key, byte[] block)
        {
            var rng = new SCXorShift32(key);
            for (int i = 0; i < block.Length; i++)
                block[i] = (byte)(block[i] ^ rng.Next());
            return block;
        }

        public async Task<int> GetStoryProgress(ulong BaseBlockKeyPointer, CancellationToken token)
        {
            for (int i = DifficultyFlags.Count - 1; i >= 0; i--)
            {
                // See https://github.com/Lincoln-LM/sv-live-map/pull/43
                var block = await ReadSaveBlockRaid(BaseBlockKeyPointer, DifficultyFlags[i], 1, token).ConfigureAwait(false);
                if (block[0] == 2)
                    return i + 1;
            }
            return 0;
        }

        private async Task ReadRaids(CancellationToken token)
        {
            if (RaidBlockOffsetP == 0)
                RaidBlockOffsetP = await Executor.SwitchConnection.PointerAll(Offsets.RaidBlockPointerP, token).ConfigureAwait(false);

            if (RaidBlockOffsetK == 0)
                RaidBlockOffsetK = await Executor.SwitchConnection.PointerAll(Offsets.RaidBlockPointerK, token).ConfigureAwait(false);

            if (RaidBlockOffsetB == 0)
                RaidBlockOffsetB = await Executor.SwitchConnection.PointerAll(Offsets.RaidBlockPointerB, token).ConfigureAwait(false);

            string id = await Executor.SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            var game = id switch
            {
                RaidCrawler.Core.Structures.Offsets.ScarletID => "Scarlet",
                RaidCrawler.Core.Structures.Offsets.VioletID => "Violet",
                _ => "",
            };
            container = new(game);
            container.SetGame(game);

            var BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);

            StoryProgress = await GetStoryProgress(BaseBlockKeyPointer, token).ConfigureAwait(false);
            EventProgress = Math.Min(StoryProgress, 3);

            await ReadEventRaids(BaseBlockKeyPointer, container, token).ConfigureAwait(false);

            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(RaidBlockOffsetP + RaidBlock.HEADER_SIZE, (int)RaidBlock.SIZE_BASE, token).ConfigureAwait(false);

            (int delivery, int enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0, TeraRaidMapParent.Paldea);
            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            var raids = container.Raids;
            var encounters = container.Encounters;
            var rewards = container.Rewards;
            container.ClearRaids();
            container.ClearEncounters();
            container.ClearRewards();

            data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(RaidBlockOffsetK, (int)RaidBlock.SIZE_KITAKAMI, token).ConfigureAwait(false);

            (delivery, enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0, TeraRaidMapParent.Kitakami);

            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            var allRaids = raids.Concat(container.Raids).ToList().AsReadOnly();
            var allEncounters = encounters.Concat(container.Encounters).ToList().AsReadOnly();
            var allRewards = rewards.Concat(container.Rewards).ToList().AsReadOnly();

            container.ClearRaids();
            container.ClearEncounters();
            container.ClearRewards();

            data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(RaidBlockOffsetB, (int)RaidBlock.SIZE_BLUEBERRY, token).ConfigureAwait(false);

            (delivery, enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0, TeraRaidMapParent.Blueberry);

            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            allRaids = allRaids.Concat(container.Raids).ToList().AsReadOnly();
            allEncounters = allEncounters.Concat(container.Encounters).ToList().AsReadOnly();
            allRewards = allRewards.Concat(container.Rewards).ToList().AsReadOnly();

            container.SetRaids(allRaids);
            container.SetEncounters(allEncounters);
            container.SetRewards(allRewards);

            for (int i = 0; i < container.Raids.Count; i++)
            {
                var raidz = container.Raids;
                int index = i;
                Raid raid = raidz[index];
                var encounter = container.Encounters[index];
                var param = encounter.GetParam();
                var blank = new PK9
                {
                    Species = encounter.Species,
                    Form = encounter.Form
                };
                Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
                var (spr, txt) = CalculateFromSeed(raid.Seed, blank);
                results.Add(txt);
                raidimages.Add(spr);
                var type = TypeSpriteUtil.GetTypeSpriteGem((byte)container.Raids[i].TeraType);
                teratype.Add(type!);
                var starcount = container.Raids[i].IsEvent ? container.Encounters[i].Stars : RaidExtensions.GetStarCount(container.Raids[i], container.Raids[i].Difficulty, StoryProgress, container.Raids[i].IsBlack);
                stars.Add(starcount);
            }
        }

        public async Task ReadEventRaids(ulong BaseBlockKeyPointer, RaidContainer container, CancellationToken token, bool force = false)
        {
            var prio_file = Path.Combine(Directory.GetCurrentDirectory(), "cache", "raid_priority_array");
            if (!force && File.Exists(prio_file))
            {
                (_, var version) = FlatbufferDumper.DumpDeliveryPriorities(File.ReadAllBytes(prio_file));
                var blk = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidPriorityLocation, "raid_priority_array.tmp", true, token).ConfigureAwait(false);
                (_, var v2) = FlatbufferDumper.DumpDeliveryPriorities(blk);
                if (version != v2)
                    force = true;

                var tmp_file = Path.Combine(Directory.GetCurrentDirectory(), "cache", "raid_priority_array.tmp");
                if (File.Exists(tmp_file))
                    File.Delete(tmp_file);

                if (v2 == 0) // raid reset
                    return;
            }

            var delivery_raid_prio = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidPriorityLocation, "raid_priority_array", force, token).ConfigureAwait(false);
            (var group_id, var priority) = FlatbufferDumper.DumpDeliveryPriorities(delivery_raid_prio);
            if (priority == 0)
                return;

            var delivery_raid_fbs = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidBinaryLocation, "raid_enemy_array", force, token).ConfigureAwait(false);
            var delivery_fixed_rewards = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidFixedRewardLocation, "fixed_reward_item_array", force, token).ConfigureAwait(false);
            var delivery_lottery_rewards = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidLotteryRewardLocation, "lottery_reward_item_array", force, token).ConfigureAwait(false);

            container.DistTeraRaids = TeraDistribution.GetAllEncounters(delivery_raid_fbs);
            container.MightTeraRaids = TeraMight.GetAllEncounters(delivery_raid_fbs);
            container.DeliveryRaidPriority = group_id;
            container.DeliveryRaidFixedRewards = FlatbufferDumper.DumpFixedRewards(delivery_fixed_rewards);
            container.DeliveryRaidLotteryRewards = FlatbufferDumper.DumpLotteryRewards(delivery_lottery_rewards);
        }

        public async Task<byte[]> ReadBlockDefault(ulong BaseBlockKeyPointer, uint key, string? cache, bool force, CancellationToken token)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "cache");
            Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, cache ?? "");
            if (force is false && cache is not null && File.Exists(path))
                return File.ReadAllBytes(path);

            var bin = await ReadSaveBlockObject(BaseBlockKeyPointer, key, token).ConfigureAwait(false);
            File.WriteAllBytes(path, bin);
            return bin;
        }

        public async Task<byte[]> ReadSaveBlockObject(ulong BaseBlockKeyPointer, uint key, CancellationToken token)
        {
            var header_ofs = await SearchSaveKeyRaid(BaseBlockKeyPointer, key, token).ConfigureAwait(false);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(header_ofs + 8, 8, token).ConfigureAwait(false);
            header_ofs = BitConverter.ToUInt64(data);

            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(header_ofs, 5, token).ConfigureAwait(false);
            header = DecryptBlock(key, header);

            var size = BitConverter.ToUInt32(header.AsSpan()[1..]);
            var obj = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(header_ofs, (int)size + 5, token).ConfigureAwait(false);
            return DecryptBlock(key, obj)[5..];
        }

        private List<string> raidimages = [];
        private List<string> results = [];
        private List<Image> teratype = [];
        private List<int> stars = [];
        private ulong RaidBlockOffsetP;
        private ulong RaidBlockOffsetK;
        private ulong RaidBlockOffsetB;
        private RaidContainer? container;
        private int StoryProgress;
        private int EventProgress;
        private readonly IReadOnlyList<uint> DifficultyFlags = new List<uint>() { 0xEC95D8EF, 0xA9428DFE, 0x9535F471, 0x6E7F8220 };

        private void Screenshot_Click(object sender, EventArgs e)
        {
            Rectangle bounds = Bounds;
            Bitmap bmp = new(this.Width, this.Height);
            DrawToBitmap(bmp, bounds);
            Bitmap CroppedImage = bmp.Clone(new(220, 170, bmp.Width - 220, bmp.Height - 170), bmp.PixelFormat);
            Clipboard.SetImage(CroppedImage);
            MessageBox.Show("Copied to clipboard!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!OutbreakFormOpen)
            {
                OutbreakFormOpen = true;
                OutbreakForm = new(Executor, FormColor, OutbreakFormOpen);
                OutbreakForm!.Show();
            }
            else
                OutbreakForm!.Focus();
        }

        private async void WildSpawnBtn_ClickAsync(object sender, EventArgs e)
        {
            var wildstatus = await ReadEncryptedBlockBool(Blocks.KWildSpawnsEnabled, CancellationToken.None).ConfigureAwait(false);
            if (WildEnable.Checked)
            {
                if (wildstatus)
                {
                    wildstatus = await ReadEncryptedBlockBool(Blocks.KWildSpawnsEnabled, CancellationToken.None).ConfigureAwait(false);
                    MessageBox.Show($"Wild Spawns Status: {wildstatus}");
                }
                else
                {
                    await WriteEncryptedBlockBool(Blocks.KWildSpawnsEnabled, false, true, CancellationToken.None).ConfigureAwait(false);
                    wildstatus = await ReadEncryptedBlockBool(Blocks.KWildSpawnsEnabled, CancellationToken.None).ConfigureAwait(false);
                    MessageBox.Show($"Wild Spawns Status: {wildstatus}");
                }
            }
            if (WildDisable.Checked)
            {
                if (wildstatus)
                {
                    await WriteEncryptedBlockBool(Blocks.KWildSpawnsEnabled, true, false, CancellationToken.None).ConfigureAwait(false);
                    wildstatus = await ReadEncryptedBlockBool(Blocks.KWildSpawnsEnabled, CancellationToken.None).ConfigureAwait(false);
                    MessageBox.Show($"Wild Spawns Status: {wildstatus}");
                }
                else
                {
                    wildstatus = await ReadEncryptedBlockBool(Blocks.KWildSpawnsEnabled, CancellationToken.None).ConfigureAwait(false);
                    MessageBox.Show($"Wild Spawns Status: {wildstatus}");
                }
            }
        }

        private void SnackworthBtn_Click(object sender, EventArgs e)
        {
            SnackworthViewer form = new(Executor);
            form.ShowDialog();
        }

        public async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data);
            return res[0] == 2;
        }

        public async Task<ulong> SearchSaveKey(uint key, CancellationToken token)
        {
            var BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
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

        public async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
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

        private async void button2_Click_1(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            await SetDateTime((ulong)unixTimeSeconds, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task SetDateTime(ulong date, CancellationToken token)
        {
            var command = Encoding.ASCII.GetBytes($"setCurrentTime {date}{(true ? "\r\n" : "")}");
            await Executor.SwitchConnection.SendAsync(command, token).ConfigureAwait(false);
        }
    }
}
