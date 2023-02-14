using PKHeX.Core;
using PokeViewer.NET.WideViewForms;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET
{
    public partial class BoxViewerMode : Form
    {
        public int GameType;
        public List<string> CurrentSlotStats = new();
        public ToolTip tt = new();
        public bool ReadInProgress;
        private readonly SwitchSocketAsync SwitchConnection;
        public BoxViewerMode(int gametype, SwitchSocketAsync switchConnection)
        {
            InitializeComponent();
            SwitchConnection = switchConnection;
            GameType = gametype;
            this.Text = VersionString(GameType);
            label2.Text = $"{(GameSelected)GameType}";
            button1.Text = "View";
        }

        private static string VersionString(int type)
        {
            string vers = string.Empty;
            switch (type)
            {
                case (int)GameSelected.SW or (int)GameSelected.SH: vers = "PokeViewer.NET - Box Viewer (SWSH)"; break;
                case (int)GameSelected.BD or (int)GameSelected.SP: vers = "PokeViewer.NET - Box Viewer (BDSP)"; break;
                case (int)GameSelected.LA: vers = "PokeViewer.NET - Box Viewer (LA)"; break;
                case (int)GameSelected.LGP or (int)GameSelected.LGE: vers = "PokeViewer.NET - Box Viewer (LGPE)"; break;
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: vers = "PokeViewer.NET - Box Viewer (SV)"; break;
            }
            return vers;
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            ReadInProgress = true;
            tt.RemoveAll();
            tt.Dispose();
            tt = new();
            CurrentSlotStats = new();
            uint offset = 0x00;
            var size = 0x168;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    {
                        size = 0x158;
                        break;
                    }
                case (int)GameSelected.LA:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.BD:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.SP:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.SW or (int)GameSelected.SH:
                    {
                        size = 0x158;
                        offset = 0x45075880;
                        break;
                    }
                case (int)GameSelected.LGP or (int)GameSelected.LGE:
                    {
                        size = 0x158;
                        offset = 0x533675B0;
                        break;
                    }
            }
            var currentbox = int.Parse(textBox1.Text);
            await ReadBoxes(offset, size, currentbox, CancellationToken.None).ConfigureAwait(false);

            ReadInProgress = false;
        }

        private async Task ReadBoxes(uint offset, int size, int boxnumber, CancellationToken token)
        {
            PictureBox[] boxes =
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10,
                pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20,
                pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30
            };
            var box = boxnumber - 1;
            PKM pk = new PK9();
            button1.Text = "Reading...";
            button1.Enabled = false;
            for (int i = 0; i < 30; i++)
            {
                switch (GameType)
                {
                    case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                        {
                            var slotsize = 344;
                            var ptr = new long[] { 0x4384B18, 0x128, 0x9B0, 0x0 };
                            var b1s1 = await SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var boxsize = 30 * slotsize;
                            var boxStart = b1s1 + (ulong)(box * boxsize);
                            var slotstart = boxStart + (ulong)(i * slotsize);

                            pk = await ReadBoxPokemonSV(slotstart, size, token).ConfigureAwait(false);

                            break;
                        }
                    case (int)GameSelected.LA:
                        {
                            var slotsize = 360;
                            var ptr = new long[] { 0x42BA6B0, 0x1F0, 0x68 };
                            var b1s1 = await SwitchConnection.PointerAll(ptr, token).ConfigureAwait(false);
                            var boxsize = 30 * slotsize;
                            var boxStart = b1s1 + (ulong)(box * boxsize);
                            var slotstart = boxStart + (ulong)(i * slotsize);

                            _ = new PA8();
                            pk = await ReadBoxPokemonLA(slotstart, size, token).ConfigureAwait(false);
                            break;
                        }
                    case (int)GameSelected.BD:
                        {
                            var sizeup = GetBDSPSlotValue(i);
                            var boxvalue = GetBDSPBoxValue(box);
                            var b1s1 = new long[] { 0x4C64DC0, 0xB8, 0x10, 0xA0, boxvalue, sizeup, 0x20 };
                            var boxStart = await SwitchConnection.PointerAll(b1s1, token).ConfigureAwait(false);
                            _ = new PB8();
                            pk = await ReadBoxPokemonBDSP(boxStart, size, token).ConfigureAwait(false);
                            break;
                        }
                    case (int)GameSelected.SP:
                        {
                            var sizeup = GetBDSPSlotValue(i);
                            var boxvalue = GetBDSPBoxValue(box);
                            var b1s1 = new long[] { 0x4E7BE98, 0xB8, 0x10, 0xA0, boxvalue, sizeup, 0x20 };
                            var boxStart = await SwitchConnection.PointerAll(b1s1, token).ConfigureAwait(false);
                            _ = new PB8();
                            pk = await ReadBoxPokemonBDSP(boxStart, size, token).ConfigureAwait(false);
                            break;
                        }
                    case (int)GameSelected.SW or (int)GameSelected.SH:
                        {
                            _ = new PK8();
                            pk = await ReadBoxPokemonSWSH((uint)(offset + (size * i + (size * 30 * box))), size, token).ConfigureAwait(false);
                            break;
                        }
                    case (int)GameSelected.LGP or (int)GameSelected.LGE:
                        {
                            _ = new PB7();
                            pk = await ReadBoxPokemonLGPE((uint)GetSlotOffset(box, i), LGPESlotSize + LGPEGapSize, token).ConfigureAwait(false);
                            break;
                        }
                }
                if (pk.Species is 0 or > (int)Species.MAX_COUNT)
                {
                    boxes[i].Image = null;
                    CurrentSlotStats.Add($"Box {textBox1.Text} Slot {i} is empty.");
                    continue;
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
                string msg = string.Empty;
                if (pk is PK8)
                {
                    bool hasMark = HasMark((PK8)pk, out RibbonIndex mark);
                    msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                }
                if (pk is PK9)
                {
                    bool hasMark = HasMark((PK9)pk, out RibbonIndex mark);
                    msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                }
                string alpha = string.Empty;
                if (pk is PA8)
                {
                    bool isAlpha = pk is PA8 pa8 ? pa8.IsAlpha : false;
                    if (isAlpha)
                        alpha = $"αlpha - ";
                }
                bool isGmax = pk is PK8 pk8 ? pk8.CanGigantamax : false;
                string gMax = isGmax ? "Gigantamax - " : "";
                CurrentSlotStats.Add($"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{gMax}{alpha}{(Species)pk.Species}{form}{gender}{pid}{ec}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{msg}");
                if (pk is PK8 && isGmax)
                {
                    if (pk.Species == (int)Species.Charmander || pk.Species == (int)Species.Charmeleon || pk.Species == (int)Species.Hattrem)
                        isGmax = false;
                }
                if (pk is PB7)
                {
                    if (pk.Species == (int)Species.Eevee || pk.Species == (int)Species.Pikachu)
                        pk.Form = 0;
                }
                var sprite = PokeImg(pk, isGmax);
                boxes[i].Load(sprite);
            }
            button1.Text = "View";
            button1.Enabled = true;

        }

        private readonly uint LGPEStart = 0x533675B0;
        private readonly int LGPESlotSize = 260;
        private readonly int LGPESlotCount = 25;
        private readonly int LGPEGapSize = 380;
        private ulong GetBoxOffset(int box) => (ulong)LGPEStart + (ulong)((LGPESlotSize + LGPEGapSize) * LGPESlotCount * box);
        private ulong GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (ulong)((LGPESlotSize + LGPEGapSize) * slot);

        private async Task<PK8> ReadBoxPokemonSWSH(uint offset, int size, CancellationToken token)
        {        
            var data = await SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PK8(data);
            return pk;
        }

        private async Task<PK9> ReadBoxPokemonSV(ulong offset, int size, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PK9(data);
            return pk;
        }

        private async Task<PA8> ReadBoxPokemonLA(ulong offset, int size, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PA8(data);
            return pk;
        }

        private async Task<PB8> ReadBoxPokemonBDSP(ulong offset, int size, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB8(data);
            return pk;
        }

        private async Task<PB7> ReadBoxPokemonLGPE(uint offset, int size, CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PB7(data);
            return pk;
        }

        private void pictureBox_MouseHover(object sender, EventArgs e)
        {
            if (!ReadInProgress)
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

        private static bool HasMark(IRibbonIndex pk, out RibbonIndex result)
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

        private void button2_Click(object sender, EventArgs e)
        {
            ForwardClick(sender, e);
        }

        private async void ForwardClick(object sender, EventArgs e)
        {
            ReadInProgress = true;
            CurrentSlotStats = new();
            uint offset = 0x00;
            var size = 0x168;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    {
                        size = 0x158;
                        break;
                    }
                case (int)GameSelected.LA:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.BD:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.SP:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.SW or (int)GameSelected.SH:
                    {
                        size = 0x158;
                        offset = 0x45075880;
                        break;
                    }
                case (int)GameSelected.LGP or (int)GameSelected.LGE:
                    {
                        size = 0x158;
                        break;
                    }
            }
            var currentbox = int.Parse(textBox1.Text);
            if (currentbox == 32)
                currentbox = 1;
            else
                currentbox = currentbox + 1;
            textBox1.Text = currentbox.ToString();
            await ReadBoxes(offset, size, currentbox, CancellationToken.None).ConfigureAwait(false);

            ReadInProgress = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BackwardClick(sender, e);
        }

        private async void BackwardClick(object sender, EventArgs e)
        {
            ReadInProgress = true;
            CurrentSlotStats = new();
            uint offset = 0x00;
            var size = 0x168;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    {
                        size = 0x158;
                        break;
                    }
                case (int)GameSelected.LA:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.BD:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.SP:
                    {
                        size = 0x168;
                        break;
                    }
                case (int)GameSelected.SW or (int)GameSelected.SH:
                    {
                        size = 0x158;
                        offset = 0x45075880;
                        break;
                    }
                case (int)GameSelected.LGP or (int)GameSelected.LGE:
                    {
                        size = 0x158;
                        offset = 0x533675B0;
                        break;
                    }
            }
            var currentbox = int.Parse(textBox1.Text);
            if (currentbox == 1)
                currentbox = 32;
            else
                currentbox = currentbox - 1;
            textBox1.Text = currentbox.ToString();
            await ReadBoxes(offset, size, currentbox, CancellationToken.None).ConfigureAwait(false);

            ReadInProgress = false;
        }        

        private static uint GetBDSPSlotValue(int slot)
        {
            switch (slot)
            {
                case 0: slot = 0x20; break;
                case 1: slot = 0x28; break;
                case 2: slot = 0x30; break;
                case 3: slot = 0x38; break;
                case 4: slot = 0x40; break;
                case 5: slot = 0x48; break;
                case 6: slot = 0x50; break;
                case 7: slot = 0x58; break;
                case 8: slot = 0x60; break;
                case 9: slot = 0x68; break;
                case 10: slot = 0x70; break;
                case 11: slot = 0x78; break;
                case 12: slot = 0x80; break;
                case 13: slot = 0x88; break;
                case 14: slot = 0x90; break;
                case 15: slot = 0x98; break;
                case 16: slot = 0xA0; break;
                case 17: slot = 0xA8; break;
                case 18: slot = 0xB0; break;
                case 19: slot = 0xB8; break;
                case 20: slot = 0xC0; break;
                case 21: slot = 0xC8; break;
                case 22: slot = 0xD0; break;
                case 23: slot = 0xD8; break;
                case 24: slot = 0xE0; break;
                case 25: slot = 0xE8; break;
                case 26: slot = 0xF0; break;
                case 27: slot = 0xF8; break;
                case 28: slot = 0x100; break;
                case 29: slot = 0x108; break;
                case 30: slot = 0x110; break;
            }
            return (uint)slot;
        }

        private static uint GetBDSPBoxValue(int slot)
        {
            switch (slot)
            {
                case 0: slot = 0x20; break;
                case 1: slot = 0x28; break;
                case 2: slot = 0x30; break;
                case 3: slot = 0x38; break;
                case 4: slot = 0x40; break;
                case 5: slot = 0x48; break;
                case 6: slot = 0x50; break;
                case 7: slot = 0x58; break;
                case 8: slot = 0x60; break;
                case 9: slot = 0x68; break;
                case 10: slot = 0x70; break;
                case 11: slot = 0x78; break;
                case 12: slot = 0x80; break;
                case 13: slot = 0x88; break;
                case 14: slot = 0x90; break;
                case 15: slot = 0x98; break;
                case 16: slot = 0xA0; break;
                case 17: slot = 0xA8; break;
                case 18: slot = 0xB0; break;
                case 19: slot = 0xB8; break;
                case 20: slot = 0xC0; break;
                case 21: slot = 0xC8; break;
                case 22: slot = 0xD0; break;
                case 23: slot = 0xD8; break;
                case 24: slot = 0xE0; break;
                case 25: slot = 0xE8; break;
                case 26: slot = 0xF0; break;
                case 27: slot = 0xF8; break;
                case 28: slot = 0x100; break;
                case 29: slot = 0x108; break;
                case 30: slot = 0x110; break;
                case 31: slot = 0x118; break;
                case 32: slot = 0x120; break;
                case 33: slot = 0x128; break;
                case 34: slot = 0x130; break;
                case 35: slot = 0x138; break;
                case 36: slot = 0x140; break;
                case 37: slot = 0x148; break;
                case 38: slot = 0x150; break;
                case 39: slot = 0x158; break;
                case 40: slot = 0x160; break;
            }
            return (uint)slot;
        }

        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            PictureBox? pbox = sender as PictureBox;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (pbox.Image == null)
            {
                MessageBox.Show("No data present, click view and try again.");
                return;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var currentslot = int.Parse(pbox.Name.Replace("pictureBox", "")) - 1;
            if (pbox.Image != null)
            {
                using BoxViewerMini form = new(pbox, CurrentSlotStats[currentslot].ToString());
                form.ShowDialog();
            }            
        }

    }
}
