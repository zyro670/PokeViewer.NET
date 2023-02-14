using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.SubForms
{
    public partial class OverworldViewSV : Form
    {
        public static RoutineExecutor Executor = new();
        private readonly SwitchSocketAsync SwitchConnection;

        public OverworldViewSV(SwitchSocketAsync switchConnection)
        {
            InitializeComponent();
            SwitchConnection = switchConnection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScanOverworld(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task ScanOverworld(CancellationToken token)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox3, pictureBox5, pictureBox7, pictureBox9, pictureBox11, pictureBox13, pictureBox15 };
            TextBox[] outputBox = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8 };
            PictureBox[] markboxes = { pictureBox2, pictureBox4, pictureBox6, pictureBox8, pictureBox10, pictureBox12, pictureBox14, pictureBox16 };
            string? url = string.Empty;
            string? sprite = string.Empty;
            string value = string.Empty;
            button1.Text = "Scanning...";
            for (int i = 0; i < 8; i++)
            {                              
                switch (i)
                {
                    case 0: value = "[[[[[[main+437D188]+28]+20]+30]+08]+30]"; break;
                    case 1: value = "[[[[[[main+437D188]+28]+50]+30]+08]+30]"; break;
                    case 2: value = "[[[[[[main+437D188]+28]+60]+30]+08]+30]"; break;
                    case 3: value = "[[[[[[main+437D188]+28]+70]+30]+08]+30]"; break;
                    case 4: value = "[[[[[[main+437D188]+28]+78]+30]+08]+30]"; break;
                    case 5: value = "[[[[[[main+437D188]+28]+80]+30]+08]+30]"; break;
                    case 6: value = "[[[[[[main+437D188]+28]+90]+30]+08]+30]"; break;
                    case 7: value = "[[[[[[main+437D188]+28]+98]+30]+08]+30]"; break;
                    case 8: value = "[[[[[[main+437D188]+28]+18]+30]+08]+30]"; break;
                    case 9: break;
                }
                var ofs = await GetPointerAddress(value, token).ConfigureAwait(false);
                var data = await SwitchConnection.ReadBytesAbsoluteAsync(ofs, 344, token).ConfigureAwait(false);

                var pk = new PK9(data);
                //var pk = await ReadOverworldPokemonSV(ofs, size).ConfigureAwait(false);
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
            }
            button1.Text = "Done";
            return;
        }

        public async Task<PK9> ReadOverworldPokemonSV(ulong offset, int size)
        {
            //offset = await ParsePointer("[[[[main+4348AC0]+10]+B8]+388]+3E0", CancellationToken.None).ConfigureAwait(false);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, CancellationToken.None).ConfigureAwait(false);
            var pk = new PK9(data);

            return pk;
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
    }
}
