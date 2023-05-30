using PKHeX.Core;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class WideViewerLA : Form
    {
        private readonly ViewerExecutor Executor;
        public WideViewerLA(ViewerExecutor executor)
        {
            InitializeComponent();
            Executor = executor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadEncounter_ClickAsync(sender, e);
        }

        private async void ReadEncounter_ClickAsync(object sender, EventArgs e)
        {
            List<PA8> viewing = new();
            var pk = new PA8();
            int size = 0x168;
            uint value = 0x00;
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0: value = 0x90; break;
                    case 1: value = 0xF0; break;
                    case 2: value = 0x150; break;
                    case 3: value = 0x1B0; break;
                    case 4: value = 0x210; break;
                }
                var ptr = new long[] { 0x42A6F00, 0x98, value, 0x10, 0x58, 0x00 };
                var ofs = await Executor.SwitchConnection.PointerAll(ptr, CancellationToken.None).ConfigureAwait(false);
                pk = await ReadInBattlePokemonLA(ofs, size).ConfigureAwait(false);
                LASanityCheck(pk, i);
            }
        }

        private async void LASanityCheck(PA8 pk, int count)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            TextBox[] outputBox = { textBox1, textBox2, textBox3, textBox4, textBox5 };
            PictureBox[] alphaboxes = { pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10 };
            string? sprite = string.Empty;
            bool isValid = PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form);
            if (!isValid || pk.Species < 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                outputBox[count].Text = "No Pokémon present.";
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                boxes[count].Load(sprite);
                return;
            }

            string isAlpha = pk.IsAlpha ? $"αlpha - " : "";
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
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{isAlpha}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}";
            outputBox[count].Text = output;
            sprite = PokeImg(pk, false);
            boxes[count].Load(sprite);
            if (pk.IsAlpha)
            {
                var url = "https://raw.githubusercontent.com/zyro670/PokeTextures/main/OriginMarks/icon_alpha.png";
                Image img = null!;
                using (HttpClient client = new())
                {
                    using var response = await client.GetStreamAsync(url, CancellationToken.None).ConfigureAwait(false);
                    img = Image.FromStream(response);
                }
                alphaboxes[count].Image = img;
            }
            else if (!pk.IsAlpha)
                alphaboxes[count].Image = null;
            return;
        }

        public async Task<PA8> ReadInBattlePokemonLA(ulong offset, int size)
        {
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PA8(data);
            return pk;
        }

    }
}

