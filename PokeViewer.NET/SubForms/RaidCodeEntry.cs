using PKHeX.Core;
using PKHeX.Drawing.Misc;
using SysBot.Base;
using RaidCrawler.Core.Structures;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.SubForms
{
    public partial class RaidCodeEntry : Form
    {
        private readonly ViewerExecutor Executor;
        protected ViewerOffsets ViewerOffsets { get; } = new();
        public RaidCodeEntry(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            button1.BackColor = color.Item1;
            button1.ForeColor = color.Item2;
            EnterButton.BackColor = color.Item1;
            EnterButton.ForeColor = color.Item2;
            AutoPaste.BackColor = color.Item1;
            AutoPaste.ForeColor = color.Item2;
            GoButton.BackColor = color.Item1;
            GoButton.ForeColor = color.Item2;
            textBox1.BackColor = color.Item1;
            textBox1.ForeColor = color.Item2;
            Results.BackColor = color.Item1;
            Results.ForeColor = color.Item2;
            checkBox1.BackColor = color.Item1;
            checkBox1.ForeColor = color.Item2;
            SeedToPokemonGroup.BackColor = color.Item1;
            SeedToPokemonGroup.ForeColor = color.Item2;
            SeedLabel.BackColor = color.Item1;
            SeedLabel.ForeColor = color.Item2;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 4 || textBox1.Text.Length == 5)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
            else
                MessageBox.Show("TextBox is empty. Try again after you fill it in!");
        }

        private async Task EnterRaidCode(CancellationToken token)
        {
            if (textBox1.Text.Length > 6)
            {
                textBox1.Text = textBox1.Text.Substring(0, 6);
            }
            var strokes = textBox1.Text.ToUpper().ToArray();
            var number = $"NumPad";
            string[] badVals = { "@", "I", "O", "=", "&", ";", "Z", "*", "#", "!", "?" };
            List<HidKeyboardKey> keystopress = new();
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

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async void textBox1_DoubleClicked(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
            if (textBox1.Text.Length < 4 || textBox1.Text.Length == 5)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            AutoPaste.Enabled = false;
            Clipboard.Clear();
            while (!Clipboard.ContainsText())
            {
                await Task.Delay(0_100);
            }
            textBox1.Text = Clipboard.GetText();
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);

            AutoPaste.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }

        private async void GoButton_Click(object sender, EventArgs e)
        {
            GoButton.Enabled = false;
            numericUpDown1.Enabled = false;
            checkBox1.Enabled = false;
            try
            {
                await ReadRaids(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            GoButton.Enabled = true;
            numericUpDown1.Enabled = true;
            checkBox1.Enabled = true;
        }

        private (string, string) CalculateFromSeed(uint seed, PK9 pk)
        {
            var raidseed = checkBox1.Checked ? "Hidden" : $"0x{seed:X8}";
            string ec = checkBox1.Checked ? "Hidden" : $"{pk.EncryptionConstant:X8}";
            string pid = checkBox1.Checked ? "Hidden" : $"{pk.PID:X8}";
            string results = $"Seed: {raidseed}{Environment.NewLine}" +
                $"EC: {ec}{Environment.NewLine}" +
                $"PID: {pid}{Environment.NewLine}" +
                $"IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}" +
                $"Gender: {(Gender)pk.Gender} | Nature: {(Nature)pk.Nature}{Environment.NewLine}" +
                $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pk.Scale)} ({pk.Scale})";

            var sprite = RoutineExecutor.PokeImg(pk, false);
            return (sprite, results);
        }

        private ulong TeraRaidBlockOffset;
        private RaidContainer? container;
        private int StoryProgress;
        private int EventProgress;
        private readonly IReadOnlyList<uint> DifficultyFlags = new List<uint>() { 0xEC95D8EF, 0xA9428DFE, 0x9535F471, 0x6E7F8220 };

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

        private static byte[] DecryptBlock(uint key, byte[] block)
        {
            var rng = new SCXorShift32(key);
            for (int i = 0; i < block.Length; i++)
                block[i] = (byte)(block[i] ^ rng.Next());
            return block;
        }

        public static (PK9, uint) IsSeedReturned(ITeraRaid enc, Raid raid)
        {
            var param = enc.GetParam();
            var blank = new PK9
            {
                Species = enc.Species,
                Form = enc.Form
            };
            Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);

            return (blank, raid.Seed);
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
            if (TeraRaidBlockOffset == 0)
                TeraRaidBlockOffset = await Executor.SwitchConnection.PointerAll(ViewerOffsets.TeraRaidBlockPointer, token).ConfigureAwait(false);

            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(TeraRaidBlockOffset + RaidBlock.HEADER_SIZE, (int)(RaidBlock.SIZE - RaidBlock.HEADER_SIZE), token).ConfigureAwait(false);

            string id = await Executor.SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            var game = id switch
            {
                Offsets.ScarletID => "Scarlet",
                Offsets.VioletID => "Violet",
                _ => "",
            };
            container = new(game);
            container.SetGame(game);

            var BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(ViewerOffsets.BlockKeyPointer, token).ConfigureAwait(false);

            StoryProgress = await GetStoryProgress(BaseBlockKeyPointer, token).ConfigureAwait(false);
            EventProgress = Math.Min(StoryProgress, 3);

            await ReadEventRaids(BaseBlockKeyPointer, container, token).ConfigureAwait(false);

            (int delivery, int enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0);
            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            var raids = container.Raids;
            int index = (int)numericUpDown1.Value;
            Raid raid = raids[index];
            var encounter = container.Encounters[index];
            var param = encounter.GetParam();
            var blank = new PK9
            {
                Species = encounter.Species,
                Form = encounter.Form
            };
            Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
            var (spr, txt) = CalculateFromSeed(raid.Seed, blank);
            Results.Text = txt;
            pictureBox1.Load(spr);
            var tera = blank.TeraType;
            pictureBox2.Image = TypeSpriteUtil.GetTypeSpriteGem((byte)tera);
        }

        public async Task ReadEventRaids(ulong BaseBlockKeyPointer, RaidContainer container, CancellationToken token, bool force = false)
        {
            var prio_file = Path.Combine(Directory.GetCurrentDirectory(), "cache", "raid_priority_array");
            if (!force && File.Exists(prio_file))
            {
                (_, var version) = FlatbufferDumper.DumpDeliveryPriorities(File.ReadAllBytes(prio_file));
                var blk = await ReadBlockDefault(BaseBlockKeyPointer, Offsets.BCATRaidPriorityLocation, "raid_priority_array.tmp", true, token).ConfigureAwait(false);
                (_, var v2) = FlatbufferDumper.DumpDeliveryPriorities(blk);
                if (version != v2)
                    force = true;

                var tmp_file = Path.Combine(Directory.GetCurrentDirectory(), "cache", "raid_priority_array.tmp");
                if (File.Exists(tmp_file))
                    File.Delete(tmp_file);

                if (v2 == 0) // raid reset
                    return;
            }

            var delivery_raid_prio = await ReadBlockDefault(BaseBlockKeyPointer, Offsets.BCATRaidPriorityLocation, "raid_priority_array", force, token).ConfigureAwait(false);
            (var group_id, var priority) = FlatbufferDumper.DumpDeliveryPriorities(delivery_raid_prio);
            if (priority == 0)
                return;

            var delivery_raid_fbs = await ReadBlockDefault(BaseBlockKeyPointer, Offsets.BCATRaidBinaryLocation, "raid_enemy_array", force, token).ConfigureAwait(false);
            var delivery_fixed_rewards = await ReadBlockDefault(BaseBlockKeyPointer, Offsets.BCATRaidFixedRewardLocation, "fixed_reward_item_array", force, token).ConfigureAwait(false);
            var delivery_lottery_rewards = await ReadBlockDefault(BaseBlockKeyPointer, Offsets.BCATRaidLotteryRewardLocation, "lottery_reward_item_array", force, token).ConfigureAwait(false);

            container.DistTeraRaids = TeraDistribution.GetAllEncounters(delivery_raid_fbs);
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

        private void Screenshot_Click(object sender, EventArgs e)
        {
            Rectangle bounds = this.Bounds;
            Bitmap bmp = new(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
            }
            DrawToBitmap(bmp, bounds);
            Clipboard.SetImage(bmp);
            MessageBox.Show("Copied to clipboard!");
        }
    }
}
