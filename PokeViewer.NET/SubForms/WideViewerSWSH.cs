using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;
using static PokeViewer.NET.ViewerUtil;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.WideViewForms
{
    public partial class WideViewerSWSH : Form
    {
        private readonly ViewerState Executor;
        public int GameType;
        protected ViewerOffsets Offsets { get; } = new();
        public WideViewerSWSH(ViewerState executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
        }
        private uint DefaultOffset;

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            button1.BackColor = color.Item1;
            button1.ForeColor = color.Item2;
            checkBox1.BackColor = color.Item1;
            checkBox1.ForeColor = color.Item2;

            TextBox[] textboxes =
            {
                textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12,
            };

            PictureBox[] boxes =
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15,
                pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24,
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

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            if (Executor is not null)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }
        public async Task OverworldSaveGame(CancellationToken token)
        {
            await Click(X, 1_000, token).ConfigureAwait(false);
            await Click(R, 2_000, token).ConfigureAwait(false);
            await Click(A, 3_500, token).ConfigureAwait(false);
        }

        private async Task<uint> GetOverworldOffsets(CancellationToken token)
        {
            if (Executor is not null)
            {
                List<uint> offset = new();
                for (uint i = 0; i < 10; i++)
                {
                    uint fishing = 0x4505B640 + i * Offsets.KCoordIncrement;
                    byte[] check = await Executor.SwitchConnection.ReadBytesAsync(fishing, 4, token).ConfigureAwait(false);
                    Species species = (Species)BitConverter.ToUInt16(check.AsSpan(0, 2));
                    var form = (byte)BitConverter.ToUInt16(check.AsSpan(0x2, 2));
                    if (species == 0 || species > Species.MAX_COUNT || !PersonalTable.SWSH.IsPresentInGame((ushort)species, form))
                        continue;

                    var data = await Executor.SwitchConnection.ReadBytesAsync(fishing + 0x39, 1, token).ConfigureAwait(false);
                    if (data[0] != 255)
                        continue;

                    offset.Add(fishing);
                }
                if (offset.FirstOrDefault() == 0)
                {
                    for (uint i = 0; i < 100; i++)
                    {
                        if (offset.Count == 12)
                            break;

                        uint startingoffset = Offsets.StartingOffset + i * Offsets.KCoordIncrement;
                        byte[] check = await Executor.SwitchConnection.ReadBytesAsync(startingoffset, 4, token).ConfigureAwait(false);
                        Species species = (Species)BitConverter.ToUInt16(check.AsSpan(0, 2));
                        var form = (byte)BitConverter.ToUInt16(check.AsSpan(0x2, 2));
                        if (species == 0 || species > Species.MAX_COUNT || !PersonalTable.SWSH.IsPresentInGame((ushort)species, form))
                            continue;

                        var data = await Executor.SwitchConnection.ReadBytesAsync(startingoffset + 0x39, 1, token).ConfigureAwait(false);
                        if (data[0] != 255)
                            continue;

                        offset.Add(startingoffset);
                    }
                }
                if (offset.FirstOrDefault() == 0)
                {
                    MessageBox.Show($"No Pokemon are present, change your position!");
                    return DefaultOffset;
                }
                if (offset.FirstOrDefault() != 0)
                    DefaultOffset = offset.FirstOrDefault();
            }
            return DefaultOffset;
        }

        private async Task WideViewOverworld(CancellationToken token)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12 };
            TextBox[] outputBox = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12 };
            PictureBox[] markBoxes = { pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24 };

            for (int s = 0; s < 12; s++)
            {
                boxes[s].Image = null;
                outputBox[s].Text = string.Empty;
                markBoxes[s].Image = null;
            }

            button1.Enabled = false;
            button1.Text = "Saving...";
            await OverworldSaveGame(token).ConfigureAwait(false);
            button1.Text = "Preparing...";
            await GetOverworldOffsets(token).ConfigureAwait(false);
            button1.Text = "Viewing...";
            int d = 0;
            int aura;
            int max = 50;
            string auratype = string.Empty;
            string url = string.Empty;
            uint overworldseed;
            Species species;
            uint offset = DefaultOffset;
            if (checkBox1.Checked)
            {
                offset = Offsets.FishingOffset;
                max = 3;
            }
            List<string> spriteBox = [];
            List<string> textBox = [];
            List<string> markBox = [];
            for (uint i = 0; i < max; i++)
            {
                var newoffset = offset + i * Offsets.KCoordIncrement;
                var data = await Executor.SwitchConnection.ReadBytesAsync(newoffset, 56, token).ConfigureAwait(false);

                species = (Species)BitConverter.ToUInt16(data.AsSpan(0, 2));
                var form = (byte)BitConverter.ToUInt16(data.AsSpan(0x2, 2));
                if (species == 0 || species > Species.MAX_COUNT || !PersonalTable.SWSH.IsPresentInGame((ushort)species, form))
                    continue;
                var lastdata = await Executor.SwitchConnection.ReadBytesAsync(newoffset + 0x39, 1, token).ConfigureAwait(false);
                if (lastdata[0] != 255)
                    continue;
                PK8 pk = new();
                pk.Species = (ushort)species;
                pk.Form = (byte)BitConverter.ToUInt16(data.AsSpan(0x2, 2));
                pk.Gender = (byte)BitConverter.ToUInt16(data.AsSpan(0x10, 2));
                pk.SetNature((Nature)data[8]);
                pk.SetAbility(data[12] - 1);
                if (data[22] != 255)
                    pk.SetRibbonIndex((RibbonIndex)data[22]);
                var hasMark = HasMark(pk, out RibbonIndex mark);
                string msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                aura = BitConverter.ToUInt16(data.AsSpan(0x20, 2));
                overworldseed = BitConverter.ToUInt32(await Executor.SwitchConnection.ReadBytesAsync(newoffset + 0x18, 4, token).ConfigureAwait(false), 0);

                var shinytype = BitConverter.ToUInt16(await Executor.SwitchConnection.ReadBytesAsync(newoffset + 0x6, 2, token).ConfigureAwait(false), 0);
                if (shinytype != 1)
                    CommonEdits.SetIsShiny(pk, false);
                if (shinytype == 1)
                    CommonEdits.SetIsShiny(pk, true);
                string isShiny = pk.IsShiny ? "Shiny: Yes" : "Shiny: No";
                if (aura != 0) auratype = $"{Environment.NewLine}- Brilliant Aura."; if (aura == 0) auratype = $"";
                Move Move1 = (Move)BitConverter.ToUInt16(await Executor.SwitchConnection.ReadBytesAsync(newoffset + 0x30, 2, token).ConfigureAwait(false), 0);
                string moveset = string.Empty;
                if (Move1 != 0) moveset = $"{Environment.NewLine}- {Move1}."; if (Move1 == 0) moveset = $"";

                var formz = FormOutput(pk.Species, pk.Form, out _);
                string log = $"{species}{formz}{Environment.NewLine}Seed: {overworldseed:X8}{Environment.NewLine}{isShiny}{msg}{auratype}{moveset}";

                textBox.Add(log);
                bool canGmax = new ShowdownSet(ShowdownParsing.GetShowdownText(pk)).CanGigantamax;
                var sprite = PokeImg(pk, canGmax);
                spriteBox.Add(sprite);
                if (!string.IsNullOrEmpty(msg))
                    url = $"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.Misc/Resources/img/ribbons/ribbonmark{msg.Replace($"{Environment.NewLine}Mark: ", "").ToLower()}.png";

                if (string.IsNullOrEmpty(msg))
                    url = "empty";

                markBox.Add(url);
                d++;
            }
            for (int b = 0; b < d; b++)
            {
                if (spriteBox[b] == null)
                    continue;

                boxes[b].Load(spriteBox[b]);
                outputBox[b].Text = textBox[b];

                if (markBox[b].Contains("empty"))
                    continue;
                markBoxes[b].Load(markBox[b]);
            }

            button1.Enabled = true;
            button1.Text = "WideView";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WideViewOverworld(CancellationToken.None).ConfigureAwait(false);
        }

    }
}
