using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.Misc;
using PKHeX.Drawing.PokeSprite;
using PokeViewer.NET.SubForms;
using static PokeViewer.NET.RoutineExecutor;
using static PokeViewer.NET.ViewerUtil;
using ToolTip = System.Windows.Forms.ToolTip;

namespace PokeViewer.NET
{
    public partial class BoxViewerMode : Form
    {
        private readonly int GameType;
        private ToolTip tt = new();
        private bool ReadInProgress;
        private List<string> CurrentSlotStats = [];
        private List<string> CurrentSlotSpecies = [];
        private List<string> CurrentSlotNature = [];
        private List<string> CurrentSlotAbility = [];
        private List<string> CurrentSlotIVs = [];
        private List<string> CurrentSlotScale = [];
        private List<string> CurrentSlotMark = [];
        private List<string> CurrentSlotBall = [];
        private List<PKM> PKMs = [];
        private readonly ViewerExecutor Executor;
        private ulong AbsoluteBoxOffset;
        private uint BoxOffset = 0;
        private int BoxSlotSize = 0;
        private int TotalBoxes = 0;
        private Image? EggDefault = null!;
        private Image? ShinySquare = null!;
        private Image? ShinyStar = null!;
        private SimpleTrainerInfo TrainerInfo = new();

        private string DumpFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public BoxViewerMode(int gametype, ViewerExecutor executor, (Color, Color) color, SimpleTrainerInfo trainer)
        {
            InitializeComponent();
            GameType = gametype;
            Executor = executor;
            ViewButton.Text = "View";
            if (GameType == (int)GameSelected.HOME)
                DumpCheck.Visible = false;
            TrainerInfo = trainer;
            SetColors(color);
            LoadComboBox();
            LoadResponses();
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            ViewButton.BackColor = color.Item1;
            ViewButton.ForeColor = color.Item2;
            FlexButton.BackColor = color.Item1;
            FlexButton.ForeColor = color.Item2;
            button3.BackColor = color.Item1;
            button3.ForeColor = color.Item2;
            button2.BackColor = color.Item1;
            button2.ForeColor = color.Item2;
            comboBox1.BackColor = color.Item1;
            comboBox1.ForeColor = color.Item2;
            HidePIDECCheck.BackColor = color.Item1;
            HidePIDECCheck.ForeColor = color.Item2;
            CSVCheck.BackColor = color.Item1;
            CSVCheck.ForeColor = color.Item2;
            DumpCheck.BackColor = color.Item1;
            DumpCheck.ForeColor = color.Item2;
            ViewAllCheck.BackColor = color.Item1;
            ViewAllCheck.ForeColor = color.Item2;

            PictureBox[] boxes =
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10,
                pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20,
                pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30
            };

            for (int i = 0; i < boxes.Length; i++)
                boxes[i].BackColor = color.Item1;
        }

        private async void LoadResponses()
        {
            await AnticipateResponse(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task AnticipateResponse(CancellationToken token)
        {
            using HttpClient client = new();
            var eggresponse = await client.GetStreamAsync("https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0000_000_uk_n_00000000_f_n.png", token).ConfigureAwait(false);
            EggDefault = Image.FromStream(eggresponse);

            string shinyicon = "https://raw.githubusercontent.com/zyro670/PokeTextures/2137b7024c161aad7ba832da481cff83792f5e67/icon_version/icon_";
            var square = await client.GetStreamAsync(shinyicon + "square.png", token).ConfigureAwait(false);
            ShinySquare = Image.FromStream(square);

            var star = await client.GetStreamAsync(shinyicon + "star.png", token).ConfigureAwait(false);
            ShinyStar = Image.FromStream(star);
        }

        private void LoadComboBox()
        {
            switch (GameType)
            {
                case (int)GameSelected.Sword or (int)GameSelected.Shield: TotalBoxes = 32; break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: TotalBoxes = 40; break;
                case (int)GameSelected.LegendsArceus: TotalBoxes = 32; break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: TotalBoxes = 40; break;
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: TotalBoxes = 32; break;
                case (int)GameSelected.HOME: TotalBoxes = 200; break;
            }
            comboBox1.Items.Clear();
            for (var i = 0; i < TotalBoxes; i++)
                comboBox1.Items.Add($"Box {i + 1}");
        }

        private void DisableAssets()
        {
            ViewButton.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            comboBox1.Enabled = false;
            DumpCheck.Enabled = false;
            FlexButton.Enabled = false;
        }

        private void EnableAssets()
        {
            ViewButton.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            comboBox1.Enabled = true;
            DumpCheck.Enabled = true;
            FlexButton.Enabled = true;
        }

        public async Task ReadBoxes(int boxnumber, CancellationToken token)
        {
            PictureBox[] boxes =
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10,
                pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20,
                pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30
            };

            var box = boxnumber;
            ViewButton.Text = "Reading...";
            DisableAssets();
            List<Image> images = new();
            List<Color> colors = new();
            PKM pk = new PK9();
            var folder = $"BoxViewer";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var subfolder = $"BoxViewer\\{(GameSelected)GameType}";
            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);
            var trainersubfolder = $"BoxViewer\\{(GameSelected)GameType}\\{TrainerInfo.OT}-{TrainerInfo.TID16}";
            if (!Directory.Exists(trainersubfolder))
                Directory.CreateDirectory(trainersubfolder);

            try
            {
                if (ViewAllCheck.Checked)
                {
                    CSVCheck.Checked = true;
                    CSVCheck.Enabled = false;
                    ViewAllCheck.Enabled = false;
                    CurrentBox.Visible = true;
                    if (GameType != (int)GameSelected.HOME)
                        DumpCheck.Checked = true;
                    bool allboxes = true;
                    while (allboxes == true && box != TotalBoxes)
                    {
                        try
                        {
                            CurrentBox.Text = $"{box + 1}";
                            await BoxRoutine(box, boxes, images, colors, pk, true, token).ConfigureAwait(false);
                            box++;
                        }
                        catch (Exception ex) { MessageBox.Show($"{ex}"); }
                    }
                    if (allboxes == true)
                        allboxes = false;
                    CurrentBox.Visible = false;
                    MessageBox.Show("All boxes have been dumped to PK & CSV files!");
                }
                else
                    await BoxRoutine(box, boxes, images, colors, pk, false, token).ConfigureAwait(false);
            }
            catch (Exception ex) { MessageBox.Show($"{ex}"); }
            PKMs = new();
            ViewButton.Text = "View";
            EnableAssets();
            CSVCheck.Enabled = true;
            ViewAllCheck.Enabled = true;
        }

        public async Task BoxRoutine(int box, PictureBox[] boxes, List<Image> images, List<Color> colors, PKM pk, bool dumpall, CancellationToken token)
        {
            CurrentSlotStats = new();
            CurrentSlotSpecies = new();
            CurrentSlotNature = new();
            CurrentSlotAbility = new();
            CurrentSlotIVs = new();
            CurrentSlotScale = new();
            CurrentSlotMark = new();
            CurrentSlotBall = new();
            if (GameType is (int)GameSelected.Scarlet or (int)GameSelected.Violet && AbsoluteBoxOffset == 0)
            {
                var SVptr = new long[] { 0x47350D8, 0xD8, 0x8, 0xB8, 0x30, 0x9D0, 0x0 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(SVptr, CancellationToken.None).ConfigureAwait(false);
            }
            if (GameType is (int)GameSelected.LegendsArceus && AbsoluteBoxOffset == 0)
            {
                var LAptr = new long[] { 0x42BA6B0, 0x1F0, 0x68 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(LAptr, CancellationToken.None).ConfigureAwait(false);
            }

            for (int i = 0; i < 30; i++)
            {
                UpdateProgress(100 / 30 * i, 100);
                switch (GameType)
                {
                    case (int)GameSelected.HOME:
                        {
                            pk = await ReadPKH((uint)(BoxOffset + (BoxSlotSize * i + (BoxSlotSize * 30 * box))), BoxSlotSize, token).ConfigureAwait(false);
                            PKMs.Add(pk);
                            break;
                        }
                    case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                        {
                            var boxsize = 30 * BoxSlotSize;
                            var boxStart = AbsoluteBoxOffset + (ulong)(box * boxsize);
                            var slotstart = boxStart + (ulong)(i * BoxSlotSize);
                            pk = await ReadBoxPokemon(slotstart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            PKMs.Add((PK9)pk);
                            break;
                        }
                    case (int)GameSelected.LegendsArceus:
                        {
                            var boxsize = 30 * BoxSlotSize;
                            var boxStart = AbsoluteBoxOffset + (ulong)(box * boxsize);
                            var slotstart = boxStart + (ulong)(i * BoxSlotSize);
                            pk = await ReadBoxPokemon(slotstart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            PKMs.Add((PA8)pk);
                            break;
                        }
                    case (int)GameSelected.BrilliantDiamond:
                        {
                            var sizeup = GetBDSPSlotValue(i);
                            var boxvalue = GetBDSPBoxValue(box);
                            var b1s1b = new long[] { 0x4C64DC0, 0xB8, 0x10, 0xA0, boxvalue, sizeup, 0x20 };
                            var boxStart = await Executor.SwitchConnection.PointerAll(b1s1b, token).ConfigureAwait(false);
                            pk = await ReadBoxPokemon(boxStart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            PKMs.Add((PB8)pk);
                            break;
                        }
                    case (int)GameSelected.ShiningPearl:
                        {
                            var sizeup = GetBDSPSlotValue(i);
                            var boxvalue = GetBDSPBoxValue(box);
                            var b1s1b = new long[] { 0x4E7BE98, 0xB8, 0x10, 0xA0, boxvalue, sizeup, 0x20 };
                            var boxStart = await Executor.SwitchConnection.PointerAll(b1s1b, token).ConfigureAwait(false);
                            pk = await ReadBoxPokemon(boxStart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            PKMs.Add((PB8)pk);
                            break;
                        }
                    case (int)GameSelected.Sword or (int)GameSelected.Shield:
                        {
                            pk = await ReadBoxPokemon(AbsoluteBoxOffset, (uint)(BoxOffset + (BoxSlotSize * i + (BoxSlotSize * 30 * box))), BoxSlotSize, token).ConfigureAwait(false);
                            PKMs.Add((PK8)pk);
                            break;
                        }
                    case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                        {
                            pk = await ReadBoxPokemon(AbsoluteBoxOffset, (uint)GetSlotOffset(box, i), LGPESlotSize + LGPEGapSize, token).ConfigureAwait(false);
                            PKMs.Add((PB7)pk);
                            break;
                        }
                }

                if (PKMs[i].Species is 0 or > (int)Species.MAX_COUNT)
                {
                    Image blank = null!;
                    Color ic = Color.WhiteSmoke;
                    if (!dumpall)
                    {
                        images.Add(blank);
                        colors.Add(ic);
                    }
                    CurrentSlotStats.Add($"Box {box + 1} Slot {i + 1} is empty.");
                    continue;
                }
                var img = await Sanitize(pk, token).ConfigureAwait(false);
                Color c = PKMs[i].Gender == 0 ? Color.LightBlue : PKMs[i].Gender == 1 ? Color.LightPink : Color.LightGray;
                if (!dumpall)
                {
                    images.Add(img);
                    colors.Add(c);
                }
            }
            UpdateProgress(100, 100);

            if (!dumpall)
            {
                for (int p = 0; p < images.Count; p++)
                {
                    boxes[p].Image = images[p];
                    boxes[p].BackColor = colors[p];
                }
            }

            if (DumpCheck.Checked && GameType != (int)GameSelected.HOME)
            {
                foreach (var pkm in PKMs)
                {
                    if ((Species)pkm.Species is not Species.None)
                        DumpPokemon(DumpFolder, $"BoxViewer\\{(GameSelected)GameType}\\{TrainerInfo.OT}-{TrainerInfo.TID16}\\Box{box + 1}", pkm);
                }
            }

            if (CSVCheck.Checked)
            {
                var filePath = $"BoxViewer\\{(GameSelected)GameType}\\{TrainerInfo.OT}-{TrainerInfo.TID16}\\Box{box + 1}.csv";
                string res = string.Empty;
                res += "Species" + ",";
                res += "Nature" + ",";
                res += "Ability" + ",";
                res += "IVs" + ",";
                res += "Scale" + ",";
                res += "Mark" + ",";
                res += "Ball" + ",";
                res += Environment.NewLine;
                for (int s = 0; s < CurrentSlotSpecies.Count; s++)
                {
                    res += CurrentSlotSpecies[s].ToString() + ",";
                    res += CurrentSlotNature[s].ToString().Replace("Nature: ", "") + ",";
                    res += CurrentSlotAbility[s].ToString().Replace("Ability: ", "") + ",";
                    res += CurrentSlotIVs[s].ToString().Replace("IVs: ", "") + ",";
                    res += CurrentSlotScale[s].ToString().Replace("Scale: ", "") + ",";
                    res += CurrentSlotMark[s].ToString().Replace("Mark: ", "") + ",";
                    res += CurrentSlotBall[s].ToString().Replace("Ball: ", "") + ",";
                    res += Environment.NewLine;
                }
                using StreamWriter writer = new(new FileStream(filePath, FileMode.Create, FileAccess.Write));
                writer.WriteLine(res);
            }
        }

        private async Task<Image> Sanitize(PKM pk, CancellationToken token)
        {
            using HttpClient client = new();
            string pid = string.Empty;
            if (!HidePIDECCheck.Checked)
                pid = $"PID: {pk.PID:X8}";
            string ec = string.Empty;
            if (!HidePIDECCheck.Checked)
                ec = $"{Environment.NewLine}EC: {pk.EncryptionConstant:X8}";
            string form = FormOutput(pk.Species, pk.Form, out _);
            string gender = string.Empty;
            switch (pk.Gender)
            {
                case 0: gender = " (M)"; break;
                case 1: gender = " (F)"; break;
                case 2: break;
            }

            string msg = string.Empty;
            Image? m = null;
            Image? o = null;
            if (pk is PK8 or PK9)
            {
                var info = RibbonInfo.GetRibbonInfo(pk);
                foreach (var rib in info)
                {
                    if (!rib.HasRibbon)
                        continue;

                    var mimg = RibbonSpriteUtil.GetRibbonSprite(rib.Name);
                    if (mimg is not null)
                        m = new Bitmap(mimg, new Size(mimg.Width + 100, mimg.Height + 100));

                    if (pk is PK9)
                    {
                        bool hasMark = HasMark((PK9)pk, out RibbonIndex mark);
                        msg = hasMark ? $"Mark: {mark.ToString().Replace("Mark", "")}" : "";
                    }
                    else
                    {
                        bool hasMark = HasMark((PK8)pk, out RibbonIndex mark);
                        msg = hasMark ? $"Mark: {mark.ToString().Replace("Mark", "")}" : "";
                    }
                }
            }

            if (pk is PKH)
            {
                bool hasMark = HasAffixedRibbon((IRibbonSetAffixed)pk, out RibbonIndex mark);
                msg = hasMark ? $"Mark: {mark.ToString().Replace("Mark", "")}" : "";
                string markon = "https://raw.githubusercontent.com/zyro670/PokeTextures/5141086ee706c09d6c9aca1a773a3d08143e6460/Ribbons/icon_ribbon_";
                if (hasMark)
                {
                    string val = (int)mark >= 100 ? $"{(int)mark + 1}" : (int)mark < 100 && (int)mark > 10 ? $"0{(int)mark + 1}" : $"00{(int)mark + 1}";
                    var markresponse8 = await client.GetStreamAsync(hasMark ? markon + val + ".png" : markon, token).ConfigureAwait(false);
                    m = Image.FromStream(markresponse8);
                    m = new Bitmap(m, new Size(m.Width, m.Height));
                }
            }

            o = SpriteUtil.GetBallSprite(pk.Ball);

            string alpha = string.Empty;
            if (pk is PA8)
            {
                bool isAlpha = pk is PA8 pa8 && pa8.IsAlpha;
                if (isAlpha)
                    alpha = $"αlpha - ";
            }
            bool isGmax = pk is PK8 pk8 && pk8.CanGigantamax;
            string gMax = isGmax ? "Gigantamax - " : "";

            string scale = string.Empty;
            if (pk is PK9 pk9)
                scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pk9.Scale)} ({pk9.Scale})";
            if (pk is PK8 pk82)
                scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pk82.HeightScalar)} ({pk82.HeightScalar})";
            if (pk is PB8 pb8)
                scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pb8.HeightScalar)} ({pb8.HeightScalar})";
            if (pk is PA8 pa82)
                scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pa82.HeightScalar)} ({pa82.HeightScalar})";
            if (pk is PKH pkh2)
                scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pkh2.HeightScalar)} ({pkh2.HeightScalar})";
            string ballstring = string.Empty;
            ballstring = $"Ball: {(Ball)pk.Ball}";

            string sens = string.Empty;
            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(ec))
                sens = pid + ec;
            CurrentSlotSpecies.Add($"{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{gMax}{alpha}{(Species)pk.Species}{form}{gender}{Environment.NewLine}{sens}");
            CurrentSlotNature.Add($"Nature: {(Nature)pk.Nature}");
            CurrentSlotAbility.Add($"Ability: {(Ability)pk.Ability}");
            CurrentSlotIVs.Add($"IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}");
            CurrentSlotScale.Add(scale);
            CurrentSlotMark.Add(msg);
            CurrentSlotBall.Add(ballstring);
            CurrentSlotStats.Add($"{CurrentSlotSpecies.Last()}{Environment.NewLine}{CurrentSlotNature.Last()}{Environment.NewLine}{CurrentSlotAbility.Last()}{Environment.NewLine}" +
                $"{CurrentSlotIVs.Last()}{Environment.NewLine}{CurrentSlotScale.Last()}{Environment.NewLine}{CurrentSlotBall.Last()}{Environment.NewLine}{CurrentSlotMark.Last()}");
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
            string? sprite;
            try
            {
                if ((Species)pk.Species is Species.Charmander or Species.Squirtle or Species.Bulbasaur or Species.Charmeleon or Species.Wartortle or Species.Ivysaur or Species.Milcery or Species.Hattrem)
                    isGmax = false;
                sprite = PokeImg(pk, isGmax);
            }
            catch
            {
                sprite = "https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX.Drawing.PokeSprite/Resources/img/Pokemon%20Sprite%20Overlays/starter.png";
            }
            var response = await client.GetStreamAsync(sprite, token).ConfigureAwait(false);
            Image img = Image.FromStream(response);
            var img2 = (Image)new Bitmap(img, new Size(img.Width, img.Height));

            if (pk.IsEgg)
            {
                var egg = new Bitmap(EggDefault!, new Size(EggDefault!.Width / 3, EggDefault!.Height / 3));
                img2 = ImageUtil.LayerImage(img2, egg, -5, 85);
            }

            if (pk.IsShiny)
            {
                Image? shiny = pk.ShinyXor == 0 ? ShinySquare : ShinyStar;
                shiny = new Bitmap(shiny!, new Size(shiny!.Width / 2, shiny!.Height / 2));
                img2 = ImageUtil.LayerImage(img2, shiny, 105, 5);
            }

            if (m != null)
            {
                m = new Bitmap(m, new Size(m.Width / 4 + 10, m.Height / 4 + 10));
                img2 = ImageUtil.LayerImage(img2, m, 0, 0);
            }

            if (o != null)
            {
                img2 = ImageUtil.LayerImage(img2, o, 100, 100);
            }
            return img2;
        }

        private readonly uint LGPEStart = 0x533675B0;
        private readonly int LGPESlotSize = 260;
        private readonly int LGPESlotCount = 25;
        private readonly int LGPEGapSize = 380;
        private ulong GetBoxOffset(int box) => (ulong)LGPEStart + (ulong)((LGPESlotSize + LGPEGapSize) * LGPESlotCount * box);
        private ulong GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (ulong)((LGPESlotSize + LGPEGapSize) * slot);

        private async Task<PKM> ReadPKH(uint offset, int size, CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false);
            PKM ph = new PKH(data);
            return ph;
        }

        private async Task<PKM> ReadBoxPokemon(ulong absoluteoffset, uint offset, int size, CancellationToken token)
        {
            PKM pk = new PK9();
            byte[]? data;
            switch (GameType)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet or (int)GameSelected.LegendsArceus or (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl:
                    {
                        data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(absoluteoffset, size, token).ConfigureAwait(false);
                        switch (GameType)
                        {
                            case (int)GameSelected.Scarlet or (int)GameSelected.Violet: pk = new PK9(data); break;
                            case (int)GameSelected.LegendsArceus: pk = new PA8(data); break;
                            case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: pk = new PB8(data); break;
                        }
                        break;
                    }
                case (int)GameSelected.Sword or (int)GameSelected.Shield or (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                    {
                        data = await Executor.SwitchConnection.ReadBytesAsync(offset, size, token).ConfigureAwait(false);
                        switch (GameType)
                        {
                            case (int)GameSelected.Sword or (int)GameSelected.Shield: pk = new PK8(data); break;
                            case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: pk = new PB7(data); break;
                        }
                        break;
                    }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            ForwardClick(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BackwardClick(sender, e);
        }

        private void PrepareSlots()
        {
            switch (GameType)
            {
                case (int)GameSelected.HOME:
                    {
                        BoxSlotSize = 0x2D0; BoxOffset = 0x12B90; break;
                    }
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                    {
                        BoxSlotSize = 0x158; break;
                    }
                case (int)GameSelected.LegendsArceus:
                    {
                        BoxSlotSize = 0x168; break;
                    }
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl:
                    {
                        BoxSlotSize = 0x168;
                        break;
                    }
                case (int)GameSelected.Sword or (int)GameSelected.Shield:
                    {
                        BoxSlotSize = 0x158; BoxOffset = 0x45075880; break;
                    }
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                    {
                        BoxSlotSize = 0x158; BoxOffset = 0x533675B0; break;
                    }
            }
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            ReadInProgress = true;
            tt.RemoveAll();
            tt.Dispose();
            tt = new();
            CurrentSlotStats = new();
            if (BoxSlotSize == 0 && BoxOffset == 0)
                PrepareSlots();
            var currentbox = comboBox1.SelectedIndex;
            if (currentbox == -1)
                currentbox = 0;
            await ReadBoxes(currentbox, token).ConfigureAwait(false);
            ReadInProgress = false;
        }

        private async void ForwardClick(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            ReadInProgress = true;
            CurrentSlotStats = new();
            if (BoxSlotSize == 0 && BoxOffset == 0)
                PrepareSlots();
            var currentbox = comboBox1.SelectedIndex;
            if (currentbox >= TotalBoxes - 1)
                currentbox = 0;
            else
                currentbox++;
            await ReadBoxes(currentbox, token).ConfigureAwait(false);
            comboBox1.SelectedIndex = currentbox;
            ReadInProgress = false;
        }

        private async void BackwardClick(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            ReadInProgress = true;
            CurrentSlotStats = new();
            if (BoxSlotSize == 0 && BoxOffset == 0)
                PrepareSlots();
            var currentbox = comboBox1.SelectedIndex;
            if (currentbox <= 0)
                currentbox = TotalBoxes - 1;
            else
                currentbox--;
            await ReadBoxes(currentbox, token).ConfigureAwait(false);
            comboBox1.SelectedIndex = currentbox;
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
            if (pbox is not null)
            {
                if (pbox.Image is null)
                {
                    MessageBox.Show("No data present, click view and try again.");
                    return;
                }
                var currentslot = int.Parse(pbox.Name.Replace("pictureBox", "")) - 1;
                if (pbox.Image is not null)
                {
                    using BoxViewerMini form = new(pbox, CurrentSlotStats[currentslot].ToString(), this.BackColor, this.ForeColor);
                    form.ShowDialog();
                }
            }
        }

        private void UpdateProgress(int currProgress, int maxProgress)
        {
            var value = (100 * currProgress) / maxProgress;
            if (progressBar1.InvokeRequired)
                progressBar1.Invoke(() => progressBar1.Value = value);
            else
                progressBar1.Value = value;
        }

        private void FlexButton_Click(object sender, EventArgs e)
        {
            FlexButton.Visible = false;
            DumpCheck.Visible = false;
            Rectangle bounds = Bounds;
            Bitmap bmp = new(Width, Height - 60);
            DrawToBitmap(bmp, bounds);
            Clipboard.SetImage(bmp);
            MessageBox.Show("Copied to clipboard!");
            FlexButton.Visible = true;
            DumpCheck.Visible = true;
        }

        public async Task<ulong> ReturnBoxSlot(int box, int slot)
        {
            PrepareSlots();
            if (GameType is (int)GameSelected.Scarlet or (int)GameSelected.Violet && AbsoluteBoxOffset == 0)
            {
                var SVptr = new long[] { 0x47350D8, 0xD8, 0x8, 0xB8, 0x30, 0x9D0, 0x0 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(SVptr, CancellationToken.None).ConfigureAwait(false);
            }
            if (GameType is (int)GameSelected.LegendsArceus && AbsoluteBoxOffset == 0)
            {
                var LAptr = new long[] { 0x42BA6B0, 0x1F0, 0x68 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(LAptr, CancellationToken.None).ConfigureAwait(false);
            }
            var boxsize = 30 * BoxSlotSize;
            var boxStart = AbsoluteBoxOffset + (ulong)(box * boxsize);
            var slotstart = boxStart + (ulong)(slot * BoxSlotSize);
            return slotstart;
        }

        public async Task<(string, string, PKM)> SlotAssist(int box, int slot, CancellationToken token)
        {
            PrepareSlots();
            if (GameType is (int)GameSelected.Scarlet or (int)GameSelected.Violet && AbsoluteBoxOffset == 0)
            {
                var SVptr = new long[] { 0x47350D8, 0xD8, 0x8, 0xB8, 0x30, 0x9D0, 0x0 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(SVptr, CancellationToken.None).ConfigureAwait(false);
            }
            if (GameType is (int)GameSelected.LegendsArceus && AbsoluteBoxOffset == 0)
            {
                var LAptr = new long[] { 0x42BA6B0, 0x1F0, 0x68 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(LAptr, CancellationToken.None).ConfigureAwait(false);
            }

            PKM pk;
            var boxsize = 30 * BoxSlotSize;
            var boxStart = AbsoluteBoxOffset + (ulong)(box * boxsize);
            var slotstart = boxStart + (ulong)(slot * BoxSlotSize);
            pk = await ReadBoxPokemon(slotstart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
            string ball = $"https://raw.githubusercontent.com/zyro670/PokeTextures/main/Ball/{pk.Ball}.png";
            bool isGmax = false;
            if (pk is PK8 pk8 && pk8.CanGigantamax)
                isGmax = true;
            string sprite = PokeImg(pk, isGmax);
            return (sprite, ball, pk);
        }

        public async Task<List<PKM>> BoxRoutineAssist(int box, CancellationToken token)
        {
            PrepareSlots();
            List<PKM> list = new();
            if (GameType is (int)GameSelected.Scarlet or (int)GameSelected.Violet && AbsoluteBoxOffset == 0)
            {
                var SVptr = new long[] { 0x47350D8, 0xD8, 0x8, 0xB8, 0x30, 0x9D0, 0x0 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(SVptr, CancellationToken.None).ConfigureAwait(false);
            }
            if (GameType is (int)GameSelected.LegendsArceus && AbsoluteBoxOffset == 0)
            {
                var LAptr = new long[] { 0x42BA6B0, 0x1F0, 0x68 };
                AbsoluteBoxOffset = await Executor.SwitchConnection.PointerAll(LAptr, CancellationToken.None).ConfigureAwait(false);
            }

            for (int i = 0; i < 30; i++)
            {
                PKM pk;
                switch (GameType)
                {
                    case (int)GameSelected.HOME:
                        {
                            pk = await ReadPKH((uint)(BoxOffset + (BoxSlotSize * i + (BoxSlotSize * 30 * box))), BoxSlotSize, token).ConfigureAwait(false);
                            list.Add(pk);
                            break;
                        }
                    case (int)GameSelected.Scarlet or (int)GameSelected.Violet:
                        {
                            var boxsize = 30 * BoxSlotSize;
                            var boxStart = AbsoluteBoxOffset + (ulong)(box * boxsize);
                            var slotstart = boxStart + (ulong)(i * BoxSlotSize);
                            pk = await ReadBoxPokemon(slotstart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            list.Add((PK9)pk);
                            break;
                        }
                    case (int)GameSelected.LegendsArceus:
                        {
                            var boxsize = 30 * BoxSlotSize;
                            var boxStart = AbsoluteBoxOffset + (ulong)(box * boxsize);
                            var slotstart = boxStart + (ulong)(i * BoxSlotSize);
                            pk = await ReadBoxPokemon(slotstart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            list.Add((PA8)pk);
                            break;
                        }
                    case (int)GameSelected.BrilliantDiamond:
                        {
                            var sizeup = GetBDSPSlotValue(i);
                            var boxvalue = GetBDSPBoxValue(box);
                            var b1s1b = new long[] { 0x4C64DC0, 0xB8, 0x10, 0xA0, boxvalue, sizeup, 0x20 };
                            var boxStart = await Executor.SwitchConnection.PointerAll(b1s1b, token).ConfigureAwait(false);
                            pk = await ReadBoxPokemon(boxStart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            list.Add((PB8)pk);
                            break;
                        }
                    case (int)GameSelected.ShiningPearl:
                        {
                            var sizeup = GetBDSPSlotValue(i);
                            var boxvalue = GetBDSPBoxValue(box);
                            var b1s1b = new long[] { 0x4E7BE98, 0xB8, 0x10, 0xA0, boxvalue, sizeup, 0x20 };
                            var boxStart = await Executor.SwitchConnection.PointerAll(b1s1b, token).ConfigureAwait(false);
                            pk = await ReadBoxPokemon(boxStart, BoxOffset, BoxSlotSize, token).ConfigureAwait(false);
                            list.Add((PB8)pk);
                            break;
                        }
                    case (int)GameSelected.Sword or (int)GameSelected.Shield:
                        {
                            pk = await ReadBoxPokemon(AbsoluteBoxOffset, (uint)(BoxOffset + (BoxSlotSize * i + (BoxSlotSize * 30 * box))), BoxSlotSize, token).ConfigureAwait(false);
                            list.Add((PK8)pk);
                            break;
                        }
                    case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee:
                        {
                            pk = await ReadBoxPokemon(AbsoluteBoxOffset, (uint)GetSlotOffset(box, i), LGPESlotSize + LGPEGapSize, token).ConfigureAwait(false);
                            list.Add((PB7)pk);
                            break;
                        }
                }
            }
            return list;
        }
    }
}
