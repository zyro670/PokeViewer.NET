using PKHeX.Core;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class NPCViewer : Form
    {
        private readonly ViewerState Executor;
        public int GameType;
        public ToolTip tt = new();
        public List<string> CurrentSlotStats = new();
        public NPCViewer(int gametype, ViewerState executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            GameType = gametype;
            Text = VersionString(GameType);
            SetColors(color);
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            button1.BackColor = color.Item1;
            button1.ForeColor = color.Item2;
            textBox1.BackColor = color.Item1;
            textBox1.ForeColor = color.Item2;
        }

        private static string VersionString(int type)
        {
            string vers = string.Empty;
            switch (type)
            {
                case (int)GameSelected.Sword or (int)GameSelected.Shield: vers = "PokeViewer.NET - NPC View (SWSH)"; break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: vers = "PokeViewer.NET - NPC View (BDSP)"; break;
                case (int)GameSelected.LegendsArceus: vers = "PokeViewer.NET - NPC View (LA)"; break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: vers = "PokeViewer.NET - NPC View (LGPE)"; break;
            }
            return vers;
        }
        private void SanityCheck(PKM pk, int count)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            bool isValid = false;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: isValid = PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield: isValid = PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: isValid = PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LegendsArceus: isValid = PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
            }
            string? sprite;
            if (!isValid || pk.Species <= 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                CurrentSlotStats.Add("No Pokémon present.");
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                boxes[count].Load(sprite);
                boxes[count].SizeMode = PictureBoxSizeMode.CenterImage;
                return;
            }

            var form = FormOutput(pk.Species, pk.Form, out _);
            bool isGmax = pk is PK8 pk8 ? pk8.CanGigantamax : false;
            string gender = string.Empty;
            switch (pk.Gender)
            {
                case 0: gender = " (M)"; break;
                case 1: gender = " (F)"; break;
                case 2: break;
            }
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{Environment.NewLine}{(Nature)pk.Nature} Nature | Ability: {(Ability)pk.Ability} | IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP} | {GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP} | {GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}";
            CurrentSlotStats.Add(output);
            sprite = PokeImg(pk, isGmax);
            boxes[count].Load(sprite);
            boxes[count].SizeMode = PictureBoxSizeMode.Zoom;

            return;
        }

        public async Task<PK8> ReadInBattlePokemonSWSH(uint offset, int size)
        {
            var data = await Executor.SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PK8(data);
            return pk;
        }

        public async Task<PB7> ReadInBattlePokemonLGPE(uint offset, int size)
        {
            var data = await Executor.SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PB7(data);
            return pk;
        }

        public async Task<PB8> ReadInBattlePokemonBDSP(ulong offset, int size)
        {
            var token = CancellationToken.None;
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB8(data);
            return pk;
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            uint ofs = 0x886BC348;
            var token = CancellationToken.None;
            tt.RemoveAll();
            tt.Dispose();
            tt = new();
            for (int i = 0; i < 6; i++)
            {
                switch (GameType)
                {
                    case (int)GameSelected.Sword or (int)GameSelected.Shield:
                        var pk8 = await ReadInBattlePokemonSWSH((uint)(ofs + (i * 0x7A0)), 0x158).ConfigureAwait(false); SanityCheck(pk8, i); break;
                    case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                        ofs = 0x9A119D20;
                        var pb7 = await ReadInBattlePokemonLGPE((uint)(ofs + (i * 0x280)), 0x158).ConfigureAwait(false); SanityCheck(pb7, i); break;
                    case (int)GameSelected.BrilliantDiamond:
                        {
                            var val = 0x20;
                            switch (i)
                            {
                                case 0: val = 0x20; break;
                                case 1: val = 0x28; break;
                                case 2: val = 0x30; break;
                                case 3: val = 0x38; break;
                                case 4: val = 0x40; break;
                                case 5: val = 0x48; break;
                            }
                            var ptr = new long[] { 0x4C64DC0, 0xB8, 0x10, 0x800, 0x58, 0x28, 0x10, val, 0x20, 0x18, 0x20 };
                            var ufs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pb8 = await ReadInBattlePokemonBDSP(ufs, size).ConfigureAwait(false);
                            SanityCheck(pb8, i);
                            break;
                        }
                    case (int)GameSelected.ShiningPearl:
                        {
                            var val = 0x20;
                            switch (i)
                            {
                                case 0: val = 0x20; break;
                                case 1: val = 0x28; break;
                                case 2: val = 0x30; break;
                                case 3: val = 0x38; break;
                                case 4: val = 0x40; break;
                                case 5: val = 0x48; break;
                            }
                            var ptr = new long[] { 0x4E7BE98, 0xB8, 0x10, 0x800, 0x58, 0x28, 0x10, val, 0x20, 0x18, 0x20 };
                            var ufs = await Executor.SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var size = 0x168;
                            var pb8 = await ReadInBattlePokemonBDSP(ufs, size).ConfigureAwait(false);
                            SanityCheck(pb8, i);
                            break;
                        }
                }
                var res = $"{CurrentSlotStats[i]}{Environment.NewLine}----------{Environment.NewLine}";
                textBox1.Text += res;

            }
        }

        private void pictureBox_MouseHover(object sender, EventArgs e)
        {
            PictureBox? pbox = sender as PictureBox;
            tt = new();
            if (pbox is not null)
            {
                if (pbox.Image == null)
                {
                    tt.SetToolTip(pbox, null);
                    return;
                }
                var currentslot = int.Parse(pbox.Name.Replace("pictureBox", "")) - 1;
                tt.SetToolTip(pbox, CurrentSlotStats[currentslot]);
            }
        }
    }
}
