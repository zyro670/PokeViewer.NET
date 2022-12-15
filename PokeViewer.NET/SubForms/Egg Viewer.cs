using PKHeX.Core;
using SysBot.Base;
using static SysBot.Base.SwitchButton;
using static SysBot.Base.SwitchStick;
using static PokeViewer.NET.RoutineExecutor;
using PKHeX.Drawing.PokeSprite;
using System.Text.RegularExpressions;

namespace PokeViewer.NET.SubForms
{
    public partial class Egg_Viewer : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public Egg_Viewer()
        {
            InitializeComponent();
            SwitchConnection.Connect();
        }
        private int eggcount = 0;
        private int sandwichcount = 0;
        private const int InjectBox = 0;
        private const int InjectSlot = 0;
        private static readonly PK9 Blank = new();
        private uint EggData = 0x04386040;
        public string DumpFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            eggcount = 0;

            if (checkBox3.Checked)
            {
                await MakeSandwich(token).ConfigureAwait(false);
                await PerformEggRoutine(token).ConfigureAwait(false);
            }
            else
                await PerformEggRoutine(token).ConfigureAwait(false);
        }

        private async Task PerformEggRoutine(CancellationToken token)
        {
            PK9 pk = new();
            PK9 pkprev = new();

            if (button1.Enabled == true)
            {
                button1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                checkBox5.Enabled = false;
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
            }
            while (!token.IsCancellationRequested)
            {
                var time = Convert.ToDouble(textBox5.Text);
                DateTime currentTime = DateTime.Now;
                DateTime x30MinsLater = currentTime.AddMinutes(time);
                while (x30MinsLater > DateTime.Now)
                {
                    pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                    pkprev = pk;
                    label3.Text = "Waiting...";
                    while (pkprev.EncryptionConstant == pk.EncryptionConstant || pk == null || (Species)pk.Species == Species.None)
                    {
                        await Task.Delay(2_500, token).ConfigureAwait(false);
                        pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                    }

                    while (pk != null && (Species)pk.Species != Species.None && pkprev.EncryptionConstant != pk.EncryptionConstant)
                    {
                        eggcount++;

                        if (checkBox8.Checked && pk.IsShiny)
                            DumpPokemon(DumpFolder, "shinyeggs", pk);

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
                        string output = $"{$"Egg #{eggcount}"}{Environment.NewLine}{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}";
                        textBox1.Text = output;
                        var sprite = PokeImg(pk, false, 7);
                        pictureBox1.Load(sprite);
                        var ballsprite = SpriteUtil.GetBallSprite(pk.Ball);
                        pictureBox2.Image = ballsprite;

                        if (pk.IsShiny && (Species)pk.Species != Species.None && checkBox1.Checked)
                        {
                            if ((Species)pk.Species == Species.Dunsparce && pk.EncryptionConstant % 100 == 0 && checkBox2.Checked)
                            {
                                label3.Text = $"Shiny 3 segment!";
                                button1.Enabled = true;
                                textBox2.Enabled = true;
                                textBox3.Enabled = true;
                                textBox4.Enabled = true;
                                textBox5.Enabled = true;
                                checkBox5.Enabled = true;
                                checkBox6.Enabled = true;
                                checkBox7.Enabled = true;
                                return;
                            }

                            label3.Text = $"Match found!";
                            button1.Enabled = true;
                            textBox2.Enabled = true;
                            textBox3.Enabled = true;
                            textBox4.Enabled = true;
                            textBox5.Enabled = true;
                            checkBox5.Enabled = true;
                            checkBox6.Enabled = true;
                            checkBox7.Enabled = true;
                            return;
                        }

                        await Click(A, 0_700, token).ConfigureAwait(false);
                        await Click(A, 0_700, token).ConfigureAwait(false);

                        await GetDialogueAction(token).ConfigureAwait(false);

                        await SetBoxPokemon(Blank, InjectBox, InjectSlot, token).ConfigureAwait(false);

                        pkprev = pk;

                    }
                }
                await MakeSandwich(token).ConfigureAwait(false);
                await PerformEggRoutine(token).ConfigureAwait(false);
            }
        }
        private async Task<PK9> ReadPokemonSV(uint offset, int size, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesMainAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PK9(data);
            return pk;
        }

        public async Task SetStick(SwitchStick stick, short x, short y, int delay, CancellationToken token)
        {
            var cmd = SwitchCommand.SetStick(stick, x, y, true);
            await SwitchConnection.SendAsync(cmd, token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task MakeSandwich(CancellationToken token)
        {
            sandwichcount++;
            label7.Text = $"Sandwiches Made: {sandwichcount}";

            await Click(MINUS, 0_500, token).ConfigureAwait(false);
            await SetStick(LEFT, 0, 30000, 0, token).ConfigureAwait(false); // Face up to table
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);

            await Click(A, 1_500, token).ConfigureAwait(false);
            await Click(A, 3_500, token).ConfigureAwait(false);
            await Click(X, 1_500, token).ConfigureAwait(false);

            var ofs = await GetPointerAddress("[[[[[main+43A7550]+20]+400]+48]+F0]+02", token).ConfigureAwait(false);
            var data = new byte[] { 0x99 };
            await SwitchConnection.WriteBytesAbsoluteAsync(data, ofs, token).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                // Lettuce
                var m1 = Convert.ToInt32(textBox2.Text);

                for (int i = 0; i < m1; i++)
                {
                    if (checkBox5.Checked)
                        await Click(DUP, 0_800, token).ConfigureAwait(false);
                    else
                    await Click(DDOWN, 0_800, token).ConfigureAwait(false);
                }
            }

            await Click(A, 0_800, token).ConfigureAwait(false);
            await Click(PLUS, 0_800, token).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                // Mystica Salt
                var m2 = Convert.ToInt32(textBox3.Text);

                for (int i = 0; i < m2; i++)
                {
                    if (checkBox6.Checked)
                        await Click(DUP, 0_800, token).ConfigureAwait(false);
                    else
                        await Click(DDOWN, 0_800, token).ConfigureAwait(false);
                }
            }

            await Click(A, 0_800, token).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                // Mystica Sweet
                var m3 = Convert.ToInt32(textBox4.Text);

                for (int i = 0; i < m3; i++)
                {
                    if (checkBox7.Checked)
                        await Click(DUP, 0_800, token).ConfigureAwait(false);
                    else
                        await Click(DDOWN, 0_800, token).ConfigureAwait(false);
                }
            } 

            await Click(A, 0_800, token).ConfigureAwait(false);
            await Click(PLUS, 0_800, token).ConfigureAwait(false);
            // Set pick
            await Click(A, 8_000, token).ConfigureAwait(false);
            //Wait for bread

            await SetStick(LEFT, 0, 30000, 0_700, token).ConfigureAwait(false); // Navigate to ingredients
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);

            ofs = await GetPointerAddress("[[[[[main+43A7550]+20]+400]+48]+F0]+02", token).ConfigureAwait(false);
            var text = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 1, token).ConfigureAwait(false);
            string result = System.Text.Encoding.ASCII.GetString(text);

            for (int i = 0; i < 5; i++)
                await Click(A, 0_500, token).ConfigureAwait(false);

            while (result == "?")
            {
                await Click(A, 3_000, token).ConfigureAwait(false);
                text = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 1, token).ConfigureAwait(false);
                result = System.Text.Encoding.ASCII.GetString(text);
            }

            if (result == "t")
            {
                await Task.Delay(2_500, token).ConfigureAwait(false);
                await Click(B, 1_000, token).ConfigureAwait(false);
                await SetStick(LEFT, 0, -15000, 0, token).ConfigureAwait(false); // Face away from table
                await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
            }
        }

        public async Task SetBoxPokemon(PK9 pkm, int box, int slot, CancellationToken token)
        {
            var ofs = await GetPointerAddress("[[[main+43A77C8]+108]+9B0]", token).ConfigureAwait(false);
            pkm.ResetPartyStats();
            await SwitchConnection.WriteBytesAbsoluteAsync(pkm.EncryptedPartyData, ofs, token).ConfigureAwait(false);
        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SwitchConnection.Reset();
            this.Close();            
            Application.Restart();
        }        

        public async Task<ulong> GetPointerAddress(string pointer, CancellationToken token, bool heaprealtive = false) //Code from LiveHex
        {
            var ptr = pointer;
            if (string.IsNullOrWhiteSpace(ptr) || ptr.IndexOfAny(new char[] { '-', '/', '*' }) != -1)
                return 0;
            while (ptr.Contains("]]"))
                ptr = ptr.Replace("]]", "]+0]");
            uint finadd = 0;
            if (!ptr.EndsWith("]"))
            {
                finadd = Util.GetHexValue(ptr.Split('+').Last());
                ptr = ptr[..ptr.LastIndexOf('+')];
            }
            var jumps = ptr.Replace("main", "").Replace("[", "").Replace("]", "").Split(new[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            if (jumps.Length == 0)
                return 0;

            var initaddress = Util.GetHexValue(jumps[0].Trim());
            ulong address = BitConverter.ToUInt64(await SwitchConnection.ReadBytesMainAsync(initaddress, 0x8, token).ConfigureAwait(false), 0);
            foreach (var j in jumps)
            {
                var val = Util.GetHexValue(j.Trim());
                if (val == initaddress)
                    continue;
                address = BitConverter.ToUInt64(await SwitchConnection.ReadBytesAbsoluteAsync(address + val, 0x8, token).ConfigureAwait(false), 0);
            }
            address += finadd;
            if (heaprealtive)
            {
                ulong heap = await SwitchConnection.GetHeapBaseAsync(token);
                address -= heap;
            }
            return address;
        }

        public static void DumpPokemon(string folder, string subfolder, PKM pk)
        {
            string form = pk.Form > 0 ? $"-{pk.Form:00}" : string.Empty;
            string ballFormatted = string.Empty;
            string shinytype = string.Empty;
            string marktype = string.Empty;
            if (pk.IsShiny)
            {
                if (pk.Format >= 8 && (pk.ShinyXor == 0 || pk.FatefulEncounter || pk.Version == (int)GameVersion.GO))
                    shinytype = " ■";
                else
                    shinytype = " ★";
            }

            string IVList = pk.IV_HP + "." + pk.IV_ATK + "." + pk.IV_DEF + "." + pk.IV_SPA + "." + pk.IV_SPD + "." + pk.IV_SPE;

            string TIDFormatted = pk.Generation >= 7 ? $"{pk.TrainerID7:000000}" : $"{pk.TID:00000}";

            if (pk.Ball != (int)Ball.None)
                ballFormatted = " - " + GameInfo.Strings.balllist[pk.Ball].Split(' ')[0];

            string speciesName = SpeciesName.GetSpeciesNameGeneration(pk.Species, (int)LanguageID.English, pk.Format);
            if (pk is IGigantamax gmax && gmax.CanGigantamax)
                speciesName += "-Gmax";

            string OTInfo = string.IsNullOrEmpty(pk.OT_Name) ? "" : $" - {pk.OT_Name} - {TIDFormatted}{ballFormatted}";

            string filename = $"{pk.Species:000}{form}{shinytype} - {speciesName} - {marktype}{IVList}{OTInfo} - {pk.EncryptionConstant:X8}";
            string filetype = "";
            if (pk is PK9)
                filetype = ".pk9";

            if (!Directory.Exists(folder))
                return;
            var dir = Path.Combine(folder, subfolder);
            Directory.CreateDirectory(dir);
            var fn = Path.Combine(dir, filename + filetype);
            File.WriteAllBytes(fn, pk.DecryptedPartyData);
            LogUtil.LogInfo($"Saved file: {fn}", "Dump");
        }

        private async Task GetDialogueAction(CancellationToken token)
        {
            var ofs = await GetPointerAddress("[[[[[main+43A7550]+20]+400]+48]+F0]", token).ConfigureAwait(false);
            var text = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 4, token).ConfigureAwait(false);
            string result = System.Text.Encoding.ASCII.GetString(text);
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            result = rgx.Replace(result, "");

            while (result != "Do") // No egg
            {                
                switch (result)
                {
                    case "Th": // 1 egg
                        {
                            for (int i = 0; i < 3; i++)
                                await Click(A, 0_800, token).ConfigureAwait(false);
                            break;
                        }
                    case "": // More than 1 egg
                        {
                            for (int i = 0; i < 4; i++)
                                await Click(A, 0_800, token).ConfigureAwait(false);

                            await SetBoxPokemon(Blank, InjectBox, InjectSlot, token).ConfigureAwait(false);
                            break;
                        }
                };
                await Task.Delay(1_000, token).ConfigureAwait(false);
                text = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 4, token).ConfigureAwait(false);
                result = System.Text.Encoding.ASCII.GetString(text);
                rgx = new Regex("[^a-zA-Z0-9 -]");
                result = rgx.Replace(result, "");
            }
            await Click(A, 0_800, token).ConfigureAwait(false);
        }
    }
}
