using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{    
    public partial class NPCViewer : Form
    {
        private readonly SwitchSocketAsync SwitchConnection;
        public static RoutineExecutor Executor = new();
        public int GameType;
        public ToolTip tt = new();
        public List<string> CurrentSlotStats = new();
        public NPCViewer(int gametype, SwitchSocketAsync switchConnection)
        {
            InitializeComponent();
            SwitchConnection = switchConnection;
            GameType = gametype;
            this.Text = VersionString(GameType);
            SwitchConnection.Connect();
        }

        private static string VersionString(int type)
        {
            string vers = string.Empty;
            switch (type)
            {
                case (int)GameSelected.Sword or (int)GameSelected.Shield: vers = "PokeViewer.NET - Trainer View (SWSH)"; break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: vers = "PokeViewer.NET - Trainer View (BDSP)"; break;
                case (int)GameSelected.LegendsArceus: vers = "PokeViewer.NET - Trainer View (LA)"; break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: vers = "PokeViewer.NET - Trainer View (LGPE)"; break;
            }
            return vers;
        }
        private void SanityCheck(PKM pk, int count)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            string? sprite = string.Empty;
            bool isValid = false;
            switch (GameType)
            {
                case (int)GameSelected.Sword or (int)GameSelected.Shield: isValid = PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: isValid = PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LegendsArceus: isValid = PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form); break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
            }
            if (!isValid || pk.Species <= 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                CurrentSlotStats.Add("No Pokémon present.");
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
                boxes[count].Load(sprite);
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
            string output = $"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{Environment.NewLine}{(Nature)pk.Nature} Nature{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}Level: {pk.CurrentLevel}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move1]} - {pk.Move1_PP}{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move2]} - {pk.Move2_PP}{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move3]} - {pk.Move3_PP}{Environment.NewLine}{GameInfo.GetStrings(1).Move[pk.Move4]} - {pk.Move4_PP}";
            CurrentSlotStats.Add(output);
            sprite = PokeImg(pk, isGmax);
            boxes[count].Load(sprite);

            return;
        }

        public async Task<PK8> ReadInBattlePokemonSWSH(uint offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PK8(data);
            return pk;
        }

        public async Task<PB7> ReadInBattlePokemonLGPE(uint offset, int size)
        {
            var data = await SwitchConnection.ReadBytesAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PB7(data);
            return pk;
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            uint ofs = 0x886BC348;
            tt.RemoveAll();
            tt.Dispose();
            tt = new();
            if (SwitchConnection.Connected)
            {
                for (int i = 0; i < 6; i++)
                {
                    switch (GameType)
                    {
                        case (int)GameSelected.Sword or (int)GameSelected.Shield:
                            var pk8 = await ReadInBattlePokemonSWSH((uint)(ofs + (i * 0x7A0)), 0x158).ConfigureAwait(false); SanityCheck(pk8, i); break;
                        case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: ofs = 0x9A119D20;
                            var pb7 = await ReadInBattlePokemonLGPE((uint)(ofs + (i * 0x280)), 0x158).ConfigureAwait(false); SanityCheck(pb7, i); break;
                    }
            
                }
            }
            else
                return;
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
