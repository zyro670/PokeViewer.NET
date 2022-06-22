using PKHeX.Core;
using PokeViewer.NET.Properties;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class WideViewerSWSH : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public int GameType;
        public WideViewerSWSH()
        {
            InitializeComponent();
            SwitchConnection.Connect();            
        }

        private uint StartingOffset = 0x4505B880;
        private uint KCoordIncrement = 192;
        private uint FishingOffset = 0x4505B640; // Not in any wild area
        private uint DefaultOffset;

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        public async Task OverworldSaveGame(CancellationToken token)
        {
            await Click(SwitchButton.X, 1_000, token).ConfigureAwait(false);
            await Click(SwitchButton.R, 2_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 3_500, token).ConfigureAwait(false);
        }

        private async Task<uint> GetOverworldOffsets(CancellationToken token)
        {
            List<uint> offset = new List<uint>();
            for (uint i = 0; i < 10; i++)
            {
                uint fishing = 0x4505B640 + i * KCoordIncrement;
                byte[] check = await SwitchConnection.ReadBytesAsync(fishing, 2, token).ConfigureAwait(false);
                Species species = (Species)BitConverter.ToUInt16(check.Slice(0, 2), 0);
                if (species == 0 || species > Species.MAX_COUNT || !((PersonalInfoSWSH)PersonalTable.SWSH[(int)species]).IsPresentInGame)
                    continue;

                var data = await SwitchConnection.ReadBytesAsync(fishing + 0x39, 1, token).ConfigureAwait(false);
                if (data[0] != 255)
                    continue;

                offset.Add(fishing);
            }
            if (offset.FirstOrDefault() == 0)
            {
                for (uint i = 0; i < 100; i++)
                {
                    if (offset.Count == 12)
                    {
                        break;
                    }

                    uint startingoffset = StartingOffset + i * KCoordIncrement;
                    byte[] check = await SwitchConnection.ReadBytesAsync(startingoffset, 2, token).ConfigureAwait(false);
                    Species species = (Species)BitConverter.ToUInt16(check.Slice(0, 2), 0);
                    if (species == 0 || species > Species.MAX_COUNT || !((PersonalInfoSWSH)PersonalTable.SWSH[(int)species]).IsPresentInGame)
                        continue;

                    var data = await SwitchConnection.ReadBytesAsync(startingoffset + 0x39, 1, token).ConfigureAwait(false);
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
            {
                DefaultOffset = offset.FirstOrDefault();
                MessageBox.Show($"Starting offset set to 0x{DefaultOffset:X8}");
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
                offset = FishingOffset;
                max = 3;
            }
            List<string> spriteBox = new();
            List<string> textBox = new();
            List<string> markBox = new();            
            for (uint i = 0; i < max; i++)
            {
                var newoffset = offset + i * KCoordIncrement;
                var data = await SwitchConnection.ReadBytesAsync(newoffset, 56, token).ConfigureAwait(false);

                species = (Species)BitConverter.ToUInt16(data.Slice(0, 2), 0);

                if (species == 0 || species > Species.MAX_COUNT || !((PersonalInfoSWSH)PersonalTable.SWSH[(int)species]).IsPresentInGame)
                    continue;
                var lastdata = await SwitchConnection.ReadBytesAsync(newoffset + 0x39, 1, token).ConfigureAwait(false);
                if (lastdata[0] != 255)
                    continue;
                PK8 pk = new();
                pk.Species = (int)species;
                pk.Form = BitConverter.ToUInt16(data.Slice(0x2, 2), 0);
                pk.Gender = BitConverter.ToUInt16(data.Slice(0x10, 2), 0);
                pk.SetNature(data[8]);
                pk.SetAbility(data[12] - 1);
                if (data[22] != 255)
                    pk.SetRibbonIndex((RibbonIndex)data[22]);                
                var hasMark = HasMark(pk, out RibbonIndex mark);
                string msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                aura = BitConverter.ToUInt16(data.Slice(0x20, 2), 0);
                overworldseed = BitConverter.ToUInt32(await SwitchConnection.ReadBytesAsync(newoffset + 0x18, 4, token).ConfigureAwait(false), 0);                                

                var shinytype = BitConverter.ToUInt16(await SwitchConnection.ReadBytesAsync(newoffset + 0x6, 2, token).ConfigureAwait(false), 0);
                if (shinytype != 1)
                CommonEdits.SetIsShiny(pk, false);
                if (shinytype == 1)
                    CommonEdits.SetIsShiny(pk, true);
                string isShiny = pk.IsShiny ? "Shiny: Yes" : "Shiny: No";
                if (aura != 0) auratype = $"{Environment.NewLine}- Brilliant Aura."; if (aura == 0) auratype = $"";
                Move Move1 = (Move)BitConverter.ToUInt16(await SwitchConnection.ReadBytesAsync(newoffset + 0x30, 2, token).ConfigureAwait(false), 0);
                string moveset = string.Empty;
                if (Move1 != 0) moveset = $"{Environment.NewLine}- {Move1}."; if (Move1 == 0) moveset = $"";

                var formz = FormOutput(pk.Species, pk.Form, out _);
                string log = $"{species}{formz}{Environment.NewLine}Seed: {overworldseed:X8}{Environment.NewLine}{isShiny}{msg}{auratype}{moveset}";

                textBox.Add(log);
                bool canGmax = new ShowdownSet(ShowdownParsing.GetShowdownText(pk)).CanGigantamax;
                var sprite = PokeImg(pk, canGmax, false);
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

        private void button1_Click(object sender, EventArgs e)
        {
            WideViewOverworld(CancellationToken.None).ConfigureAwait(false);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "Preparing...";
            button2.Enabled = false;
            button2.Text = "Refreshing...";
            var token = CancellationToken.None;
            await OverworldSaveGame(token).ConfigureAwait(false);
            await GetOverworldOffsets(token).ConfigureAwait(false);
            button1.Enabled = true;
            button1.Text = "Ready!";
            button2.Enabled = true;
            button2.Text = "Refresh Offsets";
        }
    }
}
