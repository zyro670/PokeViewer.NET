using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{    
    public partial class TrainerViewer : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public static RoutineExecutor Executor = new();
        public int GameType;
        public TrainerViewer(int gameType)
        {
            InitializeComponent();
            GameType = gameType;
            this.Text = VersionString(GameType);
            SwitchConnection.Connect();
        }

        private string VersionString(int type)
        {
            string vers = string.Empty;
            switch (type)
            {
                case (int)GameSelected.SW or (int)GameSelected.SH: vers = "PokeViewer.NET - Trainer View (SWSH)"; break;
                case (int)GameSelected.BD or (int)GameSelected.SP: vers = "PokeViewer.NET - Trainer View (BDSP)"; break;
                case (int)GameSelected.LA: vers = "PokeViewer.NET - Trainer View (LA)"; break;
                case (int)GameSelected.LGP or (int)GameSelected.LGE: vers = "PokeViewer.NET - Trainer View (LGPE)"; break;
            }
            return vers;
        }
        private void SanityCheck(PKM pk, int count)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            TextBox[] outputBox = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
            string? sprite = string.Empty;
            bool isValid = false;
            switch (GameType)
            {
                case (int)GameSelected.SW or (int)GameSelected.SH: isValid = (PersonalTable.SWSH[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.BD or (int)GameSelected.SP: isValid = (PersonalTable.BDSP[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.LA: isValid = (PersonalTable.LA[pk.Species]).IsPresentInGame; break;
                case (int)GameSelected.LGP or (int)GameSelected.LGE: isValid = pk.Species < (int)Species.Mewtwo && pk.Species != (int)Species.Meltan && pk.Species != (int)Species.Melmetal; break;
            }
            if (!isValid || pk.Species <= 0 || pk.Species > (int)Species.MAX_COUNT)
            {
                outputBox[count].Text = "No Pokémon present.";
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
            outputBox[count].Text = output;
            sprite = PokeImg(pk, isGmax, GameType);
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
            if (SwitchConnection.Connected)
            {
                for (int i = 0; i < 6; i++)
                {
                    switch (GameType)
                    {
                        case (int)GameSelected.SW or (int)GameSelected.SH:
                            var pk8 = await ReadInBattlePokemonSWSH((uint)(ofs + (i * 0x7A0)), 0x158).ConfigureAwait(false); SanityCheck(pk8, i); break;
                        case (int)GameSelected.LGP or (int)GameSelected.LGE: ofs = 0x9A119D20;
                            var pb7 = await ReadInBattlePokemonLGPE((uint)(ofs + (i * 0x280)), 0x158).ConfigureAwait(false); SanityCheck(pb7, i); break;
                    }
            
                }
            }
            else
                return;
        }
    }
}
