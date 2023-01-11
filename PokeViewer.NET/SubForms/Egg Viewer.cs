using PKHeX.Core;
using SysBot.Base;
using static SysBot.Base.SwitchButton;
using static SysBot.Base.SwitchStick;
using static PokeViewer.NET.RoutineExecutor;
using PKHeX.Drawing.PokeSprite;
using System.Text.RegularExpressions;
using System.Text;

namespace PokeViewer.NET.SubForms
{
    public partial class Egg_Viewer : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        private readonly FormWindowState _WindowState;
        public Egg_Viewer()
        {
            InitializeComponent();
            SwitchConnection.Connect();
        }
        private int eggcount = 0;
        private int sandwichcount = 0;
        private int shinycount = 0;
        private const int InjectBox = 0;
        private const int InjectSlot = 0;
        private static readonly PK9 Blank = new();
        private uint EggData = 0x04386040;
        private uint PicnicMenu = 0x04416020;
        private byte[] BlankVal = { 0x01 };
        private const string TextBox = "[[[[[main+43A7550]+20]+400]+48]+F0]";
        private const string B1S1 = "[[[main+43A77C8]+108]+9B0]";
        private byte[]? TextVal = Array.Empty<byte>();

        public string DumpFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            eggcount = 0;

            await SwitchConnection.WriteBytesMainAsync(BlankVal, PicnicMenu, token).ConfigureAwait(false);

            for (int i = 0; i < 2; i++)
                await Click(A, 1_000, token).ConfigureAwait(false);

            await GrabValues(token).ConfigureAwait(false);

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
            if (button1.Enabled == true)
                DisableOptions();

            PK9 pk = new();
            PK9 pkprev = new();
            while (!token.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;
                DateTime TimeLater = currentTime.AddMinutes(30);

                while (TimeLater > DateTime.Now)
                {
                    pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                    while (pkprev.EncryptionConstant == pk.EncryptionConstant || pk == null || (Species)pk.Species == Species.None)
                    {
                        await Task.Delay(1_500, token).ConfigureAwait(false);
                        pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                    }

                    while (pk != null && (Species)pk.Species != Species.None && pkprev.EncryptionConstant != pk.EncryptionConstant)
                    {
                        eggcount++;
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

                        await Task.Delay(0_500, token).ConfigureAwait(false);
                        await Click(A, 2_500, token).ConfigureAwait(false);
                        await Click(A, 1_200, token).ConfigureAwait(false);

                        await RetrieveEgg(token).ConfigureAwait(false);

                        if (pk.IsShiny)
                        {
                            shinycount++;
                            label6.Text = $"Shinies Found: {shinycount}";
                        }

                        if (pk.IsShiny && (Species)pk.Species != Species.None && checkBox1.Checked)
                        {                            
                            if ((Species)pk.Species == Species.Dunsparce && pk.EncryptionConstant % 100 != 0 && checkBox2.Checked)
                                break;

                            if ((Species)pk.Species == Species.Dunsparce && pk.EncryptionConstant % 100 == 0 && checkBox2.Checked)
                            {
                                label3.Text = $"Shiny 3 segment!";
                                EnableOptions();
                                WindowState = _WindowState;
                                Activate();
                                MessageBox.Show("Shiny 3 Segment Dunsparce Found!");
                            }

                            await RetrieveEgg(token).ConfigureAwait(false);

                            label3.Text = $"Match found!";
                            EnableOptions();
                            WindowState = _WindowState;
                            Activate();
                            MessageBox.Show("Match found!");
                            return;
                        }

                        pkprev = pk;                        
                    }
                    for (int i = 0; i < 2; i++)
                        await Click(PLUS, 0_500, token).ConfigureAwait(false);
                    await Click(B, 1_000, token).ConfigureAwait(false);
                    label3.Text = "Waiting..";
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
            await Click(MINUS, 0_500, token).ConfigureAwait(false);
            await SetStick(LEFT, 0, 30000, 0_700, token).ConfigureAwait(false); // Face up to table
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
            await Click(A, 1_500, token).ConfigureAwait(false);
            await Click(A, 4_000, token).ConfigureAwait(false);
            await Click(X, 1_500, token).ConfigureAwait(false);

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

            sandwichcount++;
            label7.Text = $"Sandwiches Made: {sandwichcount}";
            for (int i = 0; i < 5; i++)
                await Click(A, 0_800, token).ConfigureAwait(false);

            bool inPicnic = await IsInPicnic(token).ConfigureAwait(false);

            while (!inPicnic)
            {
                await Click(A, 3_000, token).ConfigureAwait(false);
                inPicnic = await IsInPicnic(token).ConfigureAwait(false);
            }

            if (inPicnic)
            {
                await Task.Delay(2_500, token).ConfigureAwait(false);
                await SetStick(LEFT, 0, -10000, 0_500, token).ConfigureAwait(false); // Face down to basket
                await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
                await Task.Delay(1_000, token).ConfigureAwait(false);
                await SetStick(LEFT, 0, 5000, 0_200, token).ConfigureAwait(false); // Face up to basket
                await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
            }
        }

        public async Task SetBoxPokemon(PK9 pkm, int box, int slot, CancellationToken token)
        {
            var ofs = await GetPointerAddress("[[[main+43A77C8]+108]+9B0]", token).ConfigureAwait(false);
            pkm.ResetPartyStats();
            await SwitchConnection.WriteBytesAbsoluteAsync(pkm.EncryptedPartyData, ofs, token).ConfigureAwait(false);
        }

        private async Task<PK9> ReadBoxPokemonSV(ulong offset, int size, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PK9(data);
            return pk;
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

            string filename = $"{pk.Species:000}{form}{shinytype} - {speciesName} - {IVList}{OTInfo} - {pk.EncryptionConstant:X8}";
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

        private async Task GrabValues(CancellationToken token)
        {
            var ofs = await GetPointerAddress(TextBox, token).ConfigureAwait(false);
            TextVal = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 4, token).ConfigureAwait(false);
            await Click(A, 0_500, token).ConfigureAwait(false);
        }

        private async Task RetrieveEgg(CancellationToken token)
        {
            var b1s1 = await GetPointerAddress(B1S1, token).ConfigureAwait(false);
            var ofs = await GetPointerAddress(TextBox, token).ConfigureAwait(false);
            var text = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 4, token).ConfigureAwait(false);

            label3.Text = "There's an egg!";
            if (TextVal != null)
            {
                while (!text.SequenceEqual(TextVal)) // No egg
                {
                    await Click(A, 1_500, token).ConfigureAwait(false);

                    var dumpmon = await ReadBoxPokemonSV(b1s1, 344, token).ConfigureAwait(false);
                    if (dumpmon != null && (Species)dumpmon.Species != Species.None)
                    {
                        DumpPokemon(DumpFolder, "eggs", dumpmon);
                        await Task.Delay(1_000, token).ConfigureAwait(false);
                        await SetBoxPokemon(Blank, InjectBox, InjectSlot, token).ConfigureAwait(false);
                    }
                    text = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 4, token).ConfigureAwait(false);
                }
            }
        }

        private async Task<bool> IsInPicnic(CancellationToken token)
        {
            var Data = await SwitchConnection.ReadBytesMainAsync(PicnicMenu, 1, token).ConfigureAwait(false);
            return Data[0] == 0x01; // 1 when in picnic, 2 in sandwich menu, 3 when eating, 2 when done eating
        }

        private void DisableOptions()
        {
            button1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            checkBox5.Enabled = false;
            checkBox6.Enabled = false;
            checkBox7.Enabled = false;
        }

        private void EnableOptions()
        {
            button1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            checkBox5.Enabled = true;
            checkBox6.Enabled = true;
            checkBox7.Enabled = true;
        }
    }
}
