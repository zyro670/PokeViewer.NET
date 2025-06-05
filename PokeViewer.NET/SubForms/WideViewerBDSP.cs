using PKHeX.Core;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class WideViewerBDSP : Form
    {
        private readonly ViewerState Executor;
        public WideViewerBDSP(ViewerState executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            Window_Loaded();
        }

        private const string ShiningPearlID = "010018E011D92000";
        private const string BrilliantDiamondID = "0100000011D90000";
        private int GameType;

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            WideView10Button.BackColor = color.Item1;
            WideView10Button.ForeColor = color.Item2;

            TextBox[] textboxes =
            {
                textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10,
            };

            PictureBox[] boxes =
            {
                WideImage1, WideImage2, WideImage3, WideImage4, WideImage5, WideImage6, WideImage7, WideImage8, WideImage9, WideImage10
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

        private async void Window_Loaded()
        {
            var token = CancellationToken.None;
            int type = 0;
            string title = await Executor.SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            switch (title)
            {
                case ShiningPearlID: type = (int)GameSelected.ShiningPearl; break;
                case BrilliantDiamondID: type = (int)GameSelected.BrilliantDiamond; break;
            }
            GameType = type;
        }
        private async void WideView10Button_Click(object sender, EventArgs e)
        {
            await UndergroundScanning(CancellationToken.None).ConfigureAwait(false);
        }
        private async Task UndergroundScanning(CancellationToken token)
        {
            WideView10Button.Enabled = false;
            WideView10Button.Text = "Viewing...";
            int i = 0;
            int d = 0;
            int ofs = 0;
            if (GameType == (int)GameSelected.BrilliantDiamond)
                ofs = 0x4C5A638;
            else if (GameType == (int)GameSelected.ShiningPearl)
                ofs = 0x4E71710;
            var sprite = string.Empty;
            List<string> spriteBox = new();
            List<string> textBox = new();

            PictureBox[] boxes = { WideImage1, WideImage2, WideImage3, WideImage4, WideImage5, WideImage6, WideImage7, WideImage8, WideImage9, WideImage10 };
            TextBox[] outputBox = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10 };
            for (int y = 0; y < 5; y++)
            {
                var ptr = new long[] { ofs, 0xB8, 0x00, 0xA8, 0x10, 0x10, 0x20 + (i * 0x10), 0x10, 0x10, 0x20 };
                PB8? pk = await ReadUntilPresentAbsolute(await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false), 0_200, 0_200, token).ConfigureAwait(false) ?? new();
                if (pk.Species != 0 && pk.Species < (int)Species.MAX_COUNT)
                {
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
                    string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}";
                    textBox.Add(output);
                    sprite = PokeImg(pk, pk.CanGigantamax);
                    spriteBox.Add(sprite);
                    d++;
                }
                i++;
            }
            i = 0;
            for (int z = 0; z < 5; z++)
            {
                var ptr = new long[] { ofs, 0xB8, 0x00, 0xA8, 0x10, 0x10, 0x28 + (i * 0x10), 0x10, 0x10, 0x20 };
                PB8? pk = await ReadUntilPresentAbsolute(await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false), 0_200, 0_200, token).ConfigureAwait(false) ?? new();
                if (pk.Species != 0 && pk.Species < (int)Species.MAX_COUNT)
                {
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
                    string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}";
                    textBox.Add(output);
                    sprite = PokeImg(pk, pk.CanGigantamax);
                    spriteBox.Add(sprite);
                    d++;
                }
                i++;
            }

            for (int b = 0; b < d; b++)
            {
                boxes[b].Load(spriteBox[b]);
                outputBox[b].Text = textBox[b];
            }

            WideView10Button.Enabled = true;
            WideView10Button.Text = "WideView";
        }
        public async Task<PB8?> ReadUntilPresentAbsolute(ulong offset, int waitms, int waitInterval, CancellationToken token, int size = 0x158)
        {
            int msWaited = 0;
            while (msWaited < waitms)
            {
                var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
                var pk = new PB8(data);
                if (pk.Species != 0 && pk.ChecksumValid)
                    return pk;

                await Task.Delay(waitInterval, token).ConfigureAwait(false);
                msWaited += waitInterval;
            }
            return null;

        }
    }
}

