using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;
using static PokeViewer.NET.ViewerUtil;
using static System.Buffers.Binary.BinaryPrimitives;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.SubForms
{
    public partial class WideViewerSV : Form
    {
        private readonly ViewerExecutor Executor;
        private static ulong BaseBlockKeyPointer = 0;
        protected ViewerOffsets Offsets { get; } = new();

        public WideViewerSV(ViewerExecutor executor, (Color, Color) color)
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

            TextBox[] textboxes =
            {
                textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15
            };

            PictureBox[] boxes =
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15,
                pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30
            };

            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].BackColor = color.Item1;
            }

            for (int i = 0; i < textboxes.Length; i++)
            {
                textboxes[i].BackColor = color.Item1;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            await ScanOverworld(CancellationToken.None).ConfigureAwait(false);
        }

        public async Task SVSaveGameOverworld(CancellationToken token)
        {
            await Click(X, 2_000, token).ConfigureAwait(false);
            await Click(R, 1_800, token).ConfigureAwait(false);
            await Click(A, 5_000, token).ConfigureAwait(false);
            await Click(B, 1_000, token).ConfigureAwait(false);
        }

        private new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task ScanOverworld(CancellationToken token)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox3, pictureBox5, pictureBox7, pictureBox9, pictureBox11, pictureBox13, pictureBox15, pictureBox17, pictureBox19, pictureBox21, pictureBox23, pictureBox25, pictureBox27, pictureBox29 };
            TextBox[] outputBox = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15 };
            PictureBox[] markboxes = { pictureBox2, pictureBox4, pictureBox6, pictureBox8, pictureBox10, pictureBox12, pictureBox14, pictureBox16, pictureBox18, pictureBox20, pictureBox22, pictureBox24, pictureBox26, pictureBox28, pictureBox30 };
            string? url;
            string? sprite;

            for (int i = 0; i < 15; i++)
            {
                boxes[i].Image = null;
                markboxes[i].Image = null;
                outputBox[i].Text = string.Empty;
            }

            await SVSaveGameOverworld(token).ConfigureAwait(false);
            button1.Text = "Scanning...";
            var test = await ReadBlock(Blocks.Overworld, token).ConfigureAwait(false);
            for (int i = 0; i < 15; i++)
            {
                var data = test.Slice(0 + (i * 0x1D4), 0x157);
                var pk = new PK9(data);

                bool isValid = PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form);
                if (!isValid || pk == null || pk.Species < 0 || pk.Species > (int)Species.MAX_COUNT)
                {
                    outputBox[i].Text = "No Pokémon present.";
                    sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                    boxes[i].Load(sprite);
                    button1.Text = "Done";
                    return;
                }
                string pid = $"{Environment.NewLine}PID: {pk.PID:X8}";
                string ec = $"{Environment.NewLine}EC: {pk.EncryptionConstant:X8}";
                var form = FormOutput(pk.Species, pk.Form, out _);
                string gender = string.Empty;
                switch (pk.Gender)
                {
                    case 0: gender = " (M)"; break;
                    case 1: gender = " (F)"; break;
                    case 2: break;
                }
                var hasMark = HasMark(pk, out RibbonIndex mark);
                string msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{msg}";
                outputBox[i].Text = output;
                sprite = PokeImg(pk, false);
                boxes[i].Load(sprite);

                if (!string.IsNullOrEmpty(msg))
                {
                    url = $"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.Misc/Resources/img/ribbons/ribbonmark{msg.Replace($"{Environment.NewLine}Mark: ", "").ToLower()}.png";
                    markboxes[i].Load(url);
                }
                if (pk.IsShiny)
                {
                    MessageBox.Show($"Shiny {(Species)pk.Species} is in the overworld!");
                }
            }
            await Click(B, 1_000, token).ConfigureAwait(false);
            button1.Text = "Done";
            return;
        }

        private async Task<byte[]> ReadBlock(DataBlock block, CancellationToken token)
        {
            return await ReadEncryptedBlockObject(block, token).ConfigureAwait(false);
        }

        private async Task<byte[]> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
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
            public static DataBlock Overworld = new()
            {
                Name = "Overworld",
                Key = 0x173304D8,
                Type = SCTypeCode.Object,
                IsEncrypted = true,
                Size = 2490,
            };
        }

        private byte[] DecryptBlock(uint key, byte[] block)
        {
            var rng = new SCXorShift32(key);
            for (int i = 0; i < block.Length; i++)
                block[i] = (byte)(block[i] ^ rng.Next());
            return block;
        }
    }
}
