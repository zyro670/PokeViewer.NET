using PKHeX.Core;
using PKHeX.Drawing.PokeSprite;
using PokeViewer.NET.Properties;
using SysBot.Base;
using System.Text;
using Newtonsoft.Json;
using static SysBot.Base.SwitchButton;
using static System.Buffers.Binary.BinaryPrimitives;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.SubForms
{
    public partial class MiscView : Form
    {
        private readonly ViewerExecutor Executor;
        private static ulong BaseBlockKeyPointer = 0;
        public ulong CountCacheP;
        public ulong CountCacheK;
        public ulong CountCacheBCP;
        public ulong CountCacheBCK;
        private DateTime StartTime;
        private System.Timers.Timer timer = new();
        public List<OutbreakStash> OutbreakCache = new();
        public List<OutbreakStash> BCATOutbreakCache = new();
        private List<Image> MapSpritesP = new();
        private List<byte[]?> MapPOSP = new();
        private List<Image> MapSpritesK = new();
        private List<byte[]?> MapPOSK = new();
        private List<PK9> pkList = new();
        protected ViewerOffsets Offsets { get; } = new();
        public MiscView(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            comboBox1.SelectedIndex = Settings.Default.MapSetting;
            SpeciesBox.DataSource = Enum.GetValues(typeof(Species)).Cast<Species>().Where(z => z != Species.MAX_COUNT).ToArray();
            var path = "outbreakfilters.txt";
            if (File.Exists(path))
                LoadFilters(path);

            OverShootUpDown.Value = Settings.Default.MiscOvershoot;
            LoadOutbreakCache();
        }

        private void LoadFilters(string data)
        {
            string contents = File.ReadAllText(data);
            string[] monlist = contents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string mons in monlist)
            {
                string[] mon = mons.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                PK9 pk = new()
                {
                    Species = Convert.ToUInt16(mon[0]),
                    Form = (byte)Convert.ToUInt16(mon[1])
                };
                pkList.Add(pk);
            }
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            OutbreakScan.BackColor = color.Item1;
            OutbreakScan.ForeColor = color.Item2;
            OpenMapPaldea.BackColor = color.Item1;
            OpenMapPaldea.ForeColor = color.Item2;
            OpenMapKitakami.BackColor = color.Item1;
            OpenMapKitakami.ForeColor = color.Item2;
            OpenMapBlueberry.BackColor = color.Item1;
            OpenMapBlueberry.ForeColor = color.Item2;
            ViewList.BackColor = color.Item1;
            ViewList.ForeColor = color.Item2;
            SaveList.BackColor = color.Item1;
            SaveList.ForeColor = color.Item2;
            ClearList.BackColor = color.Item1;
            ClearList.ForeColor = color.Item2;
            ChangeFormButton.BackColor = color.Item1;
            ChangeFormButton.ForeColor = color.Item2;
            ReadValues.BackColor = color.Item1;
            ReadValues.ForeColor = color.Item2;
            AddSpecies.BackColor = color.Item1;
            AddSpecies.ForeColor = color.Item2;
            RemoveSpecies.BackColor = color.Item1;
            RemoveSpecies.ForeColor = color.Item2;
            groupBox1.BackColor = color.Item1;
            groupBox1.ForeColor = color.Item2;
            PaldeaGroup.BackColor = color.Item1;
            PaldeaGroup.ForeColor = color.Item2;
            KitakamiGroup.BackColor = color.Item1;
            KitakamiGroup.ForeColor = color.Item2;
            groupBox4.BackColor = color.Item1;
            groupBox4.ForeColor = color.Item2;
            BlueberryGroup.BackColor = color.Item1;
            BlueberryGroup.ForeColor = color.Item2;
            DPADUpDown.BackColor = color.Item1;
            DPADUpDown.ForeColor = color.Item2;
            OverShootUpDown.BackColor = color.Item1;
            OverShootUpDown.ForeColor = color.Item2;
            SpeciesBox.BackColor = color.Item1;
            SpeciesBox.ForeColor = color.Item2;
            FormBox.BackColor = color.Item1;
            FormBox.ForeColor = color.Item2;
            V_ComboBox.BackColor = color.Item1;
            V_ComboBox.ForeColor = color.Item2;
            comboBox1.BackColor = color.Item1;
            comboBox1.ForeColor = color.Item2;
            TimeViewerButton.BackColor = color.Item1;
            TimeViewerButton.ForeColor = color.Item2;
        }

        public void LoadOutbreakCache()
        {
            for (int i = 0; i < 12; i++)
                OutbreakCache.Add(new());
        }

        private void SelectedIndex_IsChanged(object sender, EventArgs e)
        {
            OutbreakCache = new();
            LoadOutbreakCache();
            CountCacheP = 0;
            CountCacheK = 0;
            CountCacheBCP = 0;
            CountCacheBCK = 0;
            Settings.Default.MapSetting = comboBox1.SelectedIndex;
            Settings.Default.Save();
        }
        private async Task SearchForOutbreak(CancellationToken token)
        {
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);

            Settings.Default.MiscOvershoot = OverShootUpDown.Value;
            Settings.Default.Save();

            Label[] Plist = { Ob1Results, Ob2Results, Ob3Results, Ob4Results, Ob5Results, Ob6Results, Ob7Results, Ob8Results };
            PictureBox[] SprP = { OBSprite1, OBSprite2, OBSprite3, OBSprite4, OBSprite5, OBSprite6, OBSprite7, OBSprite8 };
            Label[] KList = { ObResults9, ObResults10, ObResults11, ObResults12 };
            PictureBox[] SprK = { ObSprite9, ObSprite10, ObSprite11, ObSprite12 };
            Label[] CountP = { ObCount1, ObCount2, ObCount3, ObCount4, ObCount5, ObCount6, ObCount7, ObCount8 };
            Label[] CountK = { ObCount9, ObCount10, ObCount11, ObCount12 };

            List<Image> ImgP = new();
            List<Image> ImgK = new();
            List<Image> ImgObP = new();
            List<Image> ImgObK = new();
            List<byte[]?> POSlistP = new();
            List<byte[]?> POSlistK = new();
            List<byte[]?> POSlistObP = new();
            List<byte[]?> POSlistObK = new();
            List<uint> kolistP = new();
            List<uint> kolistK = new();
            List<uint> kolistObP = new();
            List<uint> kolistObK = new();
            List<uint> totallistP = new();
            List<uint> totallistK = new();
            List<uint> totallistObP = new();
            List<uint> totallistObK = new();
            List<string> stringsP = new();
            List<string> stringsK = new();
            List<string> stringsObP = new();
            List<string> stringsObK = new();
            List<string> CountlistP = new();
            List<string> CountlistK = new();
            List<string> CountlistObP = new();
            List<string> CountlistObK = new();
            List<PK9> monsP = new();
            List<PK9> monsK = new();
            List<PK9> monsObP = new();
            List<PK9> monsObK = new();

            DisableAssets();
            int dayskip = 0;
            int OutbreaktotalP = 0;
            int OutbreaktotalK = 0;
            int OutbreaktotalBCP = 0;
            int OutbreaktotalBCK = 0;
            while (!token.IsCancellationRequested)
            {
                dayskip++;

                if (dayskip % 2 == 0)
                    await ResetTime(token).ConfigureAwait(false);

                if (dayskip % 30 == 0)
                {
                    OutbreakCache = new();
                    LoadOutbreakCache();
                    CountCacheP = 0;
                    CountCacheK = 0;
                    CountCacheBCP = 0;
                    CountCacheBCK = 0;
                    await ResetGame(token, false).ConfigureAwait(false);
                }

                if (HardStopOutbreak.Checked)
                {
                    MessageBox.Show("HardStop enabled, ending task. Uncheck if you wish to scan until match is found.");
                    break;
                }

                for (int i = 0; i < Plist.Length; i++)
                {
                    SprP[i].Image = null;
                    Plist[i].Text = string.Empty;
                }

                for (int i = 0; i < KList.Length; i++)
                {
                    SprK[i].Image = null;
                    KList[i].Text = string.Empty;
                }

                StatusLabel.Text = "Status: Saving...";
                UpdateProgress(10, 100);
                await SVSaveGameOverworld(token).ConfigureAwait(false);
                await Task.Delay(1_500, token).ConfigureAwait(false);
                StatusLabel.Text = "Status: Scanning...";

                var block = Blocks.KOutbreakSpecies1;
                var koblock = Blocks.KMassOutbreakKO1;
                var totalblock = Blocks.KMassOutbreak01TotalSpawns;
                var formblock = Blocks.KMassOutbreak01Form;
                var pos = Blocks.KMassOutbreak01CenterPos;

                if (comboBox1.SelectedIndex is 0 or 1)
                {
                    var dataP = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalPaldea, CountCacheP, token).ConfigureAwait(false);
                    if (CountCacheP == 0)
                        CountCacheP = dataP.Item2;

                    OutbreaktotalP = Convert.ToInt32(dataP.Item1);
                    if (OutbreaktotalP > 8)
                    {
                        BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                        // Rerun in case of bad pointer
                        OutbreakCache = new();
                        LoadOutbreakCache();
                        CountCacheP = 0;
                        continue;
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        UpdateProgress(8 * i, 100);
                        switch (i)
                        {
                            case 0: break;
                            case 1: block = Blocks.KOutbreakSpecies2; formblock = Blocks.KMassOutbreak02Form; koblock = Blocks.KMassOutbreakKO2; totalblock = Blocks.KMassOutbreak02TotalSpawns; pos = Blocks.KMassOutbreak02CenterPos; break;
                            case 2: block = Blocks.KOutbreakSpecies3; formblock = Blocks.KMassOutbreak03Form; koblock = Blocks.KMassOutbreakKO3; totalblock = Blocks.KMassOutbreak03TotalSpawns; pos = Blocks.KMassOutbreak03CenterPos; break;
                            case 3: block = Blocks.KOutbreakSpecies4; formblock = Blocks.KMassOutbreak04Form; koblock = Blocks.KMassOutbreakKO4; totalblock = Blocks.KMassOutbreak04TotalSpawns; pos = Blocks.KMassOutbreak04CenterPos; break;
                            case 4: block = Blocks.KOutbreakSpecies5; formblock = Blocks.KMassOutbreak05Form; koblock = Blocks.KMassOutbreakKO5; totalblock = Blocks.KMassOutbreak05TotalSpawns; pos = Blocks.KMassOutbreak05CenterPos; break;
                            case 5: block = Blocks.KOutbreakSpecies6; formblock = Blocks.KMassOutbreak06Form; koblock = Blocks.KMassOutbreakKO6; totalblock = Blocks.KMassOutbreak06TotalSpawns; pos = Blocks.KMassOutbreak06CenterPos; break;
                            case 6: block = Blocks.KOutbreakSpecies7; formblock = Blocks.KMassOutbreak07Form; koblock = Blocks.KMassOutbreakKO7; totalblock = Blocks.KMassOutbreak07TotalSpawns; pos = Blocks.KMassOutbreak07CenterPos; break;
                            case 7: block = Blocks.KOutbreakSpecies8; formblock = Blocks.KMassOutbreak08Form; koblock = Blocks.KMassOutbreakKO8; totalblock = Blocks.KMassOutbreak08TotalSpawns; pos = Blocks.KMassOutbreak08CenterPos; break;
                        }
                        if (i > OutbreaktotalP - 1)
                            continue;

                        var (kocount, lofs) = await ReadEncryptedBlockUint(koblock, OutbreakCache[i].SpeciesKOCountLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesKOCountLoaded = lofs;
                        var (totalcount, tofs) = await ReadEncryptedBlockUint(totalblock, OutbreakCache[i].SpeciesTotalCountLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesTotalCountLoaded = tofs;
                        var (species, sofs) = await ReadEncryptedBlockUint(block, OutbreakCache[i].SpeciesLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesLoaded = sofs;
                        var (form, fofs) = await ReadEncryptedBlockByte(formblock, OutbreakCache[i].SpeciesFormLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesFormLoaded = fofs;
                        var (obpos, bofs) = await ReadEncryptedBlockArray(pos, OutbreakCache[i].SpeciesCenterPOSLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesCenterPOSLoaded = bofs;

                        PK9 pk = new()
                        {
                            Species = SpeciesConverter.GetNational9((ushort)species),
                            Form = form,
                        };
                        CommonEdits.SetIsShiny(pk, false);
                        string pkform = form is 0 ? "" : $"-{form}";
                        stringsP.Add($"{(Species)pk.Species}{pkform}");
                        CountlistP.Add($"{kocount}/{totalcount}");
                        var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                        ImgP.Add(img);
                        monsP.Add(pk);
                        POSlistP.Add(obpos);
                        kolistP.Add(kocount);
                        totallistP.Add(totalcount);
                    }
                }

                if (comboBox1.SelectedIndex is 0 or 2)
                {
                    var dataK = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalKitakami, CountCacheK, token).ConfigureAwait(false);
                    //UpdateProgress(20, 100);
                    if (CountCacheK == 0)
                        CountCacheK = dataK.Item2;

                    OutbreaktotalK = Convert.ToInt32(dataK.Item1);
                    if (OutbreaktotalK > 4)
                    {
                        BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                        // Rerun in case of bad pointer
                        OutbreakCache = new();
                        LoadOutbreakCache();
                        CountCacheK = 0;
                        continue;
                    }

                    for (int i = 8; i < 12; i++)
                    {
                        UpdateProgress(8 * i, 100);
                        switch (i)
                        {
                            case 8: block = Blocks.KOutbreakSpecies9; formblock = Blocks.KMassOutbreak09Form; koblock = Blocks.KMassOutbreakKO9; totalblock = Blocks.KMassOutbreak09TotalSpawns; pos = Blocks.KMassOutbreak09CenterPos; break;
                            case 9: block = Blocks.KOutbreakSpecies10; formblock = Blocks.KMassOutbreak10Form; koblock = Blocks.KMassOutbreakKO10; totalblock = Blocks.KMassOutbreak10TotalSpawns; pos = Blocks.KMassOutbreak10CenterPos; break;
                            case 10: block = Blocks.KOutbreakSpecies11; formblock = Blocks.KMassOutbreak11Form; koblock = Blocks.KMassOutbreakKO11; totalblock = Blocks.KMassOutbreak11TotalSpawns; pos = Blocks.KMassOutbreak11CenterPos; break;
                            case 11: block = Blocks.KOutbreakSpecies12; formblock = Blocks.KMassOutbreak12Form; koblock = Blocks.KMassOutbreakKO12; totalblock = Blocks.KMassOutbreak12TotalSpawns; pos = Blocks.KMassOutbreak12CenterPos; break;
                        }
                        if (i > OutbreaktotalK + 8 - 1)
                            continue;

                        var (kocount, lofs) = await ReadEncryptedBlockUint(koblock, OutbreakCache[i].SpeciesKOCountLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesKOCountLoaded = lofs;
                        var (totalcount, tofs) = await ReadEncryptedBlockUint(totalblock, OutbreakCache[i].SpeciesTotalCountLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesTotalCountLoaded = tofs;
                        var (species, sofs) = await ReadEncryptedBlockUint(block, OutbreakCache[i].SpeciesLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesLoaded = sofs;
                        var (form, fofs) = await ReadEncryptedBlockByte(formblock, OutbreakCache[i].SpeciesFormLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesFormLoaded = fofs;
                        var (obpos, bofs) = await ReadEncryptedBlockArray(pos, OutbreakCache[i].SpeciesCenterPOSLoaded, token).ConfigureAwait(false);
                        OutbreakCache[i].SpeciesCenterPOSLoaded = bofs;

                        PK9 pk = new()
                        {
                            Species = SpeciesConverter.GetNational9((ushort)species),
                            Form = form,
                        };
                        CommonEdits.SetIsShiny(pk, false);
                        string pkform = form is 0 ? "" : $"-{form}";
                        stringsK.Add($"{(Species)pk.Species}{pkform}");
                        CountlistK.Add($"{kocount}/{totalcount}");
                        var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                        ImgK.Add(img);
                        monsK.Add(pk);
                        POSlistK.Add(obpos);
                        kolistK.Add(kocount);
                        totallistK.Add(totalcount);
                    }
                }

                if (ScanForEventOutbreak.Checked)
                {
                    var BCATObEnabled = await ReadEncryptedBlockBool(Blocks.KBCATOutbreakEnabled, token).ConfigureAwait(false);
                    if (BCATObEnabled)
                    {
                        var BCOspecies = Blocks.KOutbreakBC01MainSpecies;
                        var BCOko = Blocks.KOutbreakBC01MainNumKOed;
                        var BCOtotal = Blocks.KOutbreakBC01MainTotalSpawns;
                        var BCOform = Blocks.KOutbreakBC01MainForm;
                        var BCOcenter = Blocks.KOutbreakBC01MainCenterPos;
                        for (int i = 0; i < 20; i++)
                            BCATOutbreakCache.Add(new());

                        if (comboBox1.SelectedIndex is 0 or 1)
                        {
                            var dataBCP = await ReadEncryptedBlockByte(Blocks.KOutbreakBCMainNumActive, CountCacheBCP, token).ConfigureAwait(false);
                            if (CountCacheBCP == 0)
                                CountCacheBCP = dataBCP.Item2;

                            OutbreaktotalBCP = Convert.ToInt32(dataBCP.Item1);
                            if (OutbreaktotalBCP > 10)
                            {
                                BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                                // Rerun in case of bad pointer
                                BCATOutbreakCache = new();
                                LoadOutbreakCache();
                                CountCacheBCP = 0;
                                continue;
                            }

                            for (int i = 0; i < 10; i++)
                            {
                                switch (i)
                                {
                                    case 0: break;
                                    case 1: BCOcenter = Blocks.KOutbreakBC02MainCenterPos; break;
                                    case 2: BCOcenter = Blocks.KOutbreakBC03MainCenterPos; break;
                                    case 3: BCOcenter = Blocks.KOutbreakBC04MainCenterPos; break;
                                    case 4: BCOcenter = Blocks.KOutbreakBC05MainCenterPos; break;
                                    case 5: BCOcenter = Blocks.KOutbreakBC06MainCenterPos; break;
                                    case 6: BCOcenter = Blocks.KOutbreakBC07MainCenterPos; break;
                                    case 7: BCOcenter = Blocks.KOutbreakBC08MainCenterPos; break;
                                    case 8: BCOcenter = Blocks.KOutbreakBC09MainCenterPos; break;
                                    case 9: BCOcenter = Blocks.KOutbreakBC10MainCenterPos; break;
                                }
                                if (i > OutbreaktotalBCP)
                                    continue;

                                var (kocount, lofs) = await ReadEncryptedBlockUint(BCOko, BCATOutbreakCache[i].SpeciesKOCountLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesKOCountLoaded = lofs;
                                var (totalcount, tofs) = await ReadEncryptedBlockUint(BCOtotal, BCATOutbreakCache[i].SpeciesTotalCountLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesTotalCountLoaded = tofs;
                                var (species, sofs) = await ReadEncryptedBlockUint(BCOspecies, BCATOutbreakCache[i].SpeciesLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesLoaded = sofs;
                                var (form, fofs) = await ReadEncryptedBlockByte(BCOform, BCATOutbreakCache[i].SpeciesFormLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesFormLoaded = fofs;
                                var (obpos, bofs) = await ReadEncryptedBlockArray(BCOcenter, BCATOutbreakCache[i].SpeciesCenterPOSLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesCenterPOSLoaded = bofs;

                                PK9 pk = new()
                                {
                                    Species = SpeciesConverter.GetNational9((ushort)species),
                                    Form = form,
                                };
                                CommonEdits.SetIsShiny(pk, false);
                                string pkform = form is 0 ? "" : $"-{form}";
                                stringsObP.Add($"{(Species)pk.Species}{pkform}");
                                CountlistObP.Add($"{kocount}/{totalcount}");
                                var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                                ImgObP.Add(img);
                                monsObP.Add(pk);
                                POSlistObP.Add(obpos);
                                kolistObP.Add(kocount);
                                totallistObP.Add(totalcount);
                            }
                        }

                        if (comboBox1.SelectedIndex is 0 or 2)
                        {
                            var dataBCK = await ReadEncryptedBlockByte(Blocks.KOutbreakBCDLC1NumActive, CountCacheBCK, token).ConfigureAwait(false);
                            if (CountCacheBCK == 0)
                                CountCacheBCK = dataBCK.Item2;

                            OutbreaktotalBCK = Convert.ToInt32(dataBCK.Item1);
                            if (OutbreaktotalBCK > 10)
                            {
                                BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                                // Rerun in case of bad pointer
                                BCATOutbreakCache = new();
                                LoadOutbreakCache();
                                CountCacheBCK = 0;
                                continue;
                            }

                            BCOspecies = Blocks.KOutbreakBC01DLC1Species;
                            BCOko = Blocks.KOutbreakBC01DLC1NumKOed;
                            BCOtotal = Blocks.KOutbreakBC01DLC1TotalSpawns;
                            BCOform = Blocks.KOutbreakBC01DLC1Form;
                            BCOcenter = Blocks.KOutbreakBC01DLC1CenterPos;

                            for (int i = 10; i < 20; i++)
                            {
                                switch (i)
                                {
                                    case 10: break;
                                    case 11: BCOcenter = Blocks.KOutbreakBC02DLC1CenterPos; break;
                                    case 12: BCOcenter = Blocks.KOutbreakBC03DLC1CenterPos; break;
                                    case 13: BCOcenter = Blocks.KOutbreakBC04DLC1CenterPos; break;
                                    case 14: BCOcenter = Blocks.KOutbreakBC05DLC1CenterPos; break;
                                    case 15: BCOcenter = Blocks.KOutbreakBC06DLC1CenterPos; break;
                                    case 16: BCOcenter = Blocks.KOutbreakBC07DLC1CenterPos; break;
                                    case 17: BCOcenter = Blocks.KOutbreakBC08DLC1CenterPos; break;
                                    case 18: BCOcenter = Blocks.KOutbreakBC09DLC1CenterPos; break;
                                    case 19: BCOcenter = Blocks.KOutbreakBC10DLC1CenterPos; break;
                                }
                                if (i > OutbreaktotalBCK + 10)
                                    continue;

                                var (kocount, lofs) = await ReadEncryptedBlockUint(BCOko, BCATOutbreakCache[i].SpeciesKOCountLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesKOCountLoaded = lofs;
                                var (totalcount, tofs) = await ReadEncryptedBlockUint(BCOtotal, BCATOutbreakCache[i].SpeciesTotalCountLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesTotalCountLoaded = tofs;
                                var (species, sofs) = await ReadEncryptedBlockUint(BCOspecies, BCATOutbreakCache[i].SpeciesLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesLoaded = sofs;
                                var (form, fofs) = await ReadEncryptedBlockByte(BCOform, BCATOutbreakCache[i].SpeciesFormLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesFormLoaded = fofs;
                                var (obpos, bofs) = await ReadEncryptedBlockArray(BCOcenter, BCATOutbreakCache[i].SpeciesCenterPOSLoaded, token).ConfigureAwait(false);
                                BCATOutbreakCache[i].SpeciesCenterPOSLoaded = bofs;

                                PK9 pk = new()
                                {
                                    Species = SpeciesConverter.GetNational9((ushort)species),
                                    Form = form,
                                };
                                CommonEdits.SetIsShiny(pk, false);
                                string pkform = form is 0 ? "" : $"-{form}";
                                stringsObK.Add($"{(Species)pk.Species}{pkform}");
                                CountlistObK.Add($"{kocount}/{totalcount}");
                                var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                                ImgObK.Add(img);
                                monsObK.Add(pk);
                                POSlistObK.Add(obpos);
                                kolistObK.Add(kocount);
                                totallistObK.Add(totalcount);
                            }
                        }
                    }
                }

                UpdateProgress(100, 100);
                var textp = ScanForEventOutbreak.Checked ? OutbreaktotalBCP + OutbreaktotalP : OutbreaktotalP;
                var textk = ScanForEventOutbreak.Checked ? OutbreaktotalBCK + OutbreaktotalK : OutbreaktotalK;
                OpenMapPaldea.Text = $"Paldea: {textp}";
                OpenMapKitakami.Text = $"Kitakami: {textk}";

                for (int i = 0; i < ImgP.Count; i++)
                {
                    SprP[i].Image = ImgP[i];
                    Plist[i].Text = stringsP[i].ToString();
                }

                for (int i = 0; i < CountlistP.Count; i++)
                    CountP[i].Text = CountlistP[i];

                foreach (var pips in ImgP)
                    MapSpritesP.Add(pips);
                foreach (var pps in POSlistP)
                    MapPOSP.Add(pps);

                for (int i = 0; i < ImgK.Count; i++)
                {
                    SprK[i].Image = ImgK[i];
                    KList[i].Text = stringsK[i].ToString();
                }

                for (int i = 0; i < CountlistK.Count; i++)
                    CountK[i].Text = CountlistK[i];

                foreach (var kips in ImgK)
                    MapSpritesK.Add(kips);
                foreach (var kps in POSlistK)
                    MapPOSK.Add(kps);

                if (ScanForEventOutbreak.Checked)
                {
                    foreach (var pips in ImgObP)
                        MapSpritesP.Add(pips);
                    foreach (var pps in POSlistObP)
                        MapPOSP.Add(pps);

                    await Task.Delay(0_500, token).ConfigureAwait(false);

                    foreach (var kips in ImgObK)
                        MapSpritesK.Add(kips);
                    foreach (var kps in POSlistObK)
                        MapPOSK.Add(kps);
                }

                for (int i = 0; i < monsP.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if (p.Species == monsP[i].Species && p.Form == monsP[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        timer.Stop();
                        string msg = $"{(Species)monsP[i].Species} outbreak found!";
                        EnableAssets();
                        timer.Stop();

                        if (Apply0To77.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 77...";
                            await ResetGame(token, true).ConfigureAwait(false);
                            StatusLabel.Text = "Status:";
                        }

                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(monsP[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        return;
                    }
                }

                for (int i = 0; i < monsK.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if ((Species)p.Species == (Species)monsK[i].Species && p.Form == monsK[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        timer.Stop();
                        string msg = $"{(Species)monsK[i].Species} outbreak found!";
                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(monsK[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        EnableAssets();
                        timer.Stop();

                        if (Apply0To77.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 77...";
                            await ResetGame(token, true).ConfigureAwait(false);
                            StatusLabel.Text = "Status:";
                        }
                        return;
                    }
                }

                for (int i = 0; i < monsObP.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if (p.Species == monsObP[i].Species && p.Form == monsObP[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        timer.Stop();
                        string msg = $"{(Species)monsObP[i].Species} outbreak found!";
                        EnableAssets();
                        timer.Stop();

                        if (Apply0To77.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 77...";
                            await ResetGame(token, true).ConfigureAwait(false);
                            StatusLabel.Text = "Status:";
                        }

                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(monsObP[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        return;
                    }
                }

                for (int i = 0; i < monsObK.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if (p.Species == monsObK[i].Species && p.Form == monsObK[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        timer.Stop();
                        string msg = $"{(Species)monsObK[i].Species} outbreak found!";
                        EnableAssets();
                        timer.Stop();

                        if (Apply0To77.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 77...";
                            await ResetGame(token, true).ConfigureAwait(false);
                            StatusLabel.Text = "Status:";
                        }

                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(monsObK[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        return;
                    }
                }

                ImgP = new();
                ImgK = new();
                POSlistP = new();
                POSlistK = new();
                kolistP = new();
                totallistP = new();
                stringsP = new();
                monsP = new();
                kolistK = new();
                totallistK = new();
                stringsK = new();
                monsK = new();
                CountlistP = new();
                CountlistK = new();

                MapSpritesP = new();
                MapPOSP = new();
                MapSpritesK = new();
                MapPOSK = new();

                DaySkipTotal.Text = $"Day Skips: {dayskip}";

                if (HardStopOutbreak.Checked)
                {
                    MessageBox.Show("HardStop enabled, ending task. Uncheck if you wish to scan until match is found.");
                    {
                        EnableAssets();
                        return;
                    }
                }
                if (OutbreakSearch.Checked)
                {
                    StatusLabel.Text = "Status: Skipping...";
                    for (int i = 0; i < 24; i++)
                        await TimeSkipFwd(token).ConfigureAwait(false);
                    await Task.Delay(3_000, token).ConfigureAwait(false);
                }
                else if (!OutbreakSearch.Checked)
                    break;
            }
            EnableAssets();
            timer.Stop();
        }

        private void UpdateProgress(int currProgress, int maxProgress)
        {
            var value = (100 * currProgress) / maxProgress;
            if (progressBar1.InvokeRequired)
                progressBar1.Invoke(() => progressBar1.Value = value);
            else
                progressBar1.Value = value;
        }

        private async Task ResetGame(CancellationToken token, bool knockout)
        {
            await CloseGame(token).ConfigureAwait(false);
            await ResetTime(token).ConfigureAwait(false);
            await StartGameScreen(token).ConfigureAwait(false);
            if (knockout is true)
                await KOToSeventy_Click(token).ConfigureAwait(false);
            await Click(A, 15_000, token).ConfigureAwait(false);
        }

        public async Task CloseGame(CancellationToken token)
        {
            // Close out of the game
            await Click(B, 0_500, token).ConfigureAwait(false);
            await Click(HOME, 2_000, token).ConfigureAwait(false);
            await Click(X, 1_000, token).ConfigureAwait(false);
            await Click(A, 5_000, token).ConfigureAwait(false);
        }

        public async Task StartGameScreen(CancellationToken token)
        {
            // Open game.
            await Click(A, 1_000, token).ConfigureAwait(false);

            await Click(A, 1_000, token).ConfigureAwait(false);
            // If they have DLC on the system and can't use it, requires an UP + A to start the game.
            // Should be harmless otherwise since they'll be in loading screen.
            await Click(DUP, 0_600, token).ConfigureAwait(false);
            await Click(A, 0_600, token).ConfigureAwait(false);

            // Switch Logo and game load screen
            await Task.Delay(19_000, token).ConfigureAwait(false);
        }

        private async Task KOToSeventy_Click(CancellationToken token)
        {
            OutbreakScan.Enabled = false;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var (validOutbreaksP, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalPaldea, 0, token).ConfigureAwait(false);
            var (validOutbreaksK, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalKitakami, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCP, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01MainTotalSpawns, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCK, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01DLC1TotalSpawns, 0, token).ConfigureAwait(false);

            var OutbreaktotalP = Convert.ToInt32(validOutbreaksP);
            var OutbreaktotalK = Convert.ToInt32(validOutbreaksK);
            var OutbreaktotalBCP = Convert.ToInt32(validOutbreaksBCP);
            var OutbreaktotalBCK = Convert.ToInt32(validOutbreaksBCK);
            var koblock = Blocks.KMassOutbreakKO1;
            if (comboBox1.SelectedIndex is 0 or 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    StatusLabel.Text = $"KOing Paldea: {8 * i + 1}%";
                    switch (i)
                    {
                        case 0: break;
                        case 1: koblock = Blocks.KMassOutbreakKO2; break;
                        case 2: koblock = Blocks.KMassOutbreakKO3; break;
                        case 3: koblock = Blocks.KMassOutbreakKO4; break;
                        case 4: koblock = Blocks.KMassOutbreakKO5; break;
                        case 5: koblock = Blocks.KMassOutbreakKO6; break;
                        case 6: koblock = Blocks.KMassOutbreakKO7; break;
                        case 7: koblock = Blocks.KMassOutbreakKO8; break;
                    }
                    if (i > OutbreaktotalP - 1)
                        continue;
                    var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                    uint inj = 77;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (comboBox1.SelectedIndex is 0 or 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    StatusLabel.Text = $"KOing Kitakami: {8 * i + 1}%";
                    switch (i)
                    {
                        case 0: koblock = Blocks.KMassOutbreakKO9; break;
                        case 1: koblock = Blocks.KMassOutbreakKO10; break;
                        case 2: koblock = Blocks.KMassOutbreakKO11; break;
                        case 3: koblock = Blocks.KMassOutbreakKO12; break;
                    }
                    if (i > OutbreaktotalK - 1)
                        continue;
                    var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                    uint inj = 77;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (comboBox1.SelectedIndex is 0 or 1 && ScanForEventOutbreak.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    StatusLabel.Text = $"KOing PaldeaEvent: {10 * i}%";
                    switch (i)
                    {
                        case 0: koblock = Blocks.KOutbreakBC01MainNumKOed; break;
                        case 1: koblock = Blocks.KOutbreakBC02MainNumKOed; break;
                        case 2: koblock = Blocks.KOutbreakBC03MainNumKOed; break;
                        case 3: koblock = Blocks.KOutbreakBC04MainNumKOed; break;
                        case 4: koblock = Blocks.KOutbreakBC05MainNumKOed; break;
                        case 5: koblock = Blocks.KOutbreakBC06MainNumKOed; break;
                        case 6: koblock = Blocks.KOutbreakBC07MainNumKOed; break;
                        case 7: koblock = Blocks.KOutbreakBC08MainNumKOed; break;
                        case 8: koblock = Blocks.KOutbreakBC09MainNumKOed; break;
                        case 9: koblock = Blocks.KOutbreakBC10MainNumKOed; break;
                    }
                    if (i > OutbreaktotalBCP - 1)
                        continue;
                    var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                    uint inj = 77;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (comboBox1.SelectedIndex is 0 or 2 && ScanForEventOutbreak.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    StatusLabel.Text = $"KOing KitakamiEvent: {10 * i}%";
                    switch (i)
                    {
                        case 0: koblock = Blocks.KOutbreakBC01DLC1NumKOed; break;
                        case 1: koblock = Blocks.KOutbreakBC02DLC1NumKOed; break;
                        case 2: koblock = Blocks.KOutbreakBC03DLC1NumKOed; break;
                        case 3: koblock = Blocks.KOutbreakBC04DLC1NumKOed; break;
                        case 4: koblock = Blocks.KOutbreakBC05DLC1NumKOed; break;
                        case 5: koblock = Blocks.KOutbreakBC06DLC1NumKOed; break;
                        case 6: koblock = Blocks.KOutbreakBC07DLC1NumKOed; break;
                        case 7: koblock = Blocks.KOutbreakBC08DLC1NumKOed; break;
                        case 8: koblock = Blocks.KOutbreakBC09DLC1NumKOed; break;
                        case 9: koblock = Blocks.KOutbreakBC10DLC1NumKOed; break;
                    }
                    if (i > OutbreaktotalBCK - 1)
                        continue;
                    var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                    uint inj = 77;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            StatusLabel.Text = "Status:";
            OutbreakScan.Enabled = true;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            StartTime = DateTime.Now;
            UptimeOnLoad(sender, e);
            await SearchForOutbreak(CancellationToken.None).ConfigureAwait(false);
        }

        private void UptimeOnLoad(object sender, EventArgs e)
        {
            timer = new System.Timers.Timer { Interval = 1000 };
            timer.Elapsed += (o, args) =>
            {
                UptimeLabel.Text = $"Uptime: {StartTime - DateTime.Now:d\\.hh\\:mm\\:ss}";
            };
            timer.Start();
        }

        public class OutbreakStash
        {
            public ulong SpeciesLoaded { get; set; } = 0;
            public ulong SpeciesFormLoaded { get; set; } = 0;
            public ulong SpeciesTotalCountLoaded { get; set; } = 0;
            public ulong SpeciesKOCountLoaded { get; set; } = 0;
            public ulong SpeciesCenterPOSLoaded { get; set; } = 0;
        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.Connection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        public async Task PressAndHold(SwitchButton b, int hold, int delay, CancellationToken token)
        {
            await Executor.Connection.SendAsync(SwitchCommand.Hold(b, true), token).ConfigureAwait(false);
            await Task.Delay(hold, token).ConfigureAwait(false);
            await Executor.Connection.SendAsync(SwitchCommand.Release(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        public async Task DaySkip(CancellationToken token) => await Executor.Connection.SendAsync(SwitchCommand.DaySkip(true), token).ConfigureAwait(false);
        public async Task TimeSkipFwd(CancellationToken token) => await Executor.Connection.SendAsync(SwitchCommand.TimeSkipForward(true), token).ConfigureAwait(false);
        public async Task TimeSkipBwd(CancellationToken token) => await Executor.Connection.SendAsync(SwitchCommand.TimeSkipBack(true), token).ConfigureAwait(false);
        public async Task ResetTime(CancellationToken token) => await Executor.Connection.SendAsync(SwitchCommand.ResetTime(true), token).ConfigureAwait(false);

        private static HttpClient? _client;
        private static HttpClient Client
        {
            get
            {
                _client ??= new HttpClient();
                return _client;
            }
        }

        private static string[]? DiscordWebhooks;

        private async void SendNotifications(string results, string thumbnail)
        {
            if (string.IsNullOrEmpty(results) || string.IsNullOrEmpty(Settings.Default.WebHook))
                return;
            DiscordWebhooks = Settings.Default.WebHook.Split(',');
            if (DiscordWebhooks == null)
                return;

            var webhook = GenerateWebhook(results, thumbnail);
            var content = new StringContent(JsonConvert.SerializeObject(webhook), Encoding.UTF8, "application/json");
            foreach (var url in DiscordWebhooks)
                await Client.PostAsync(url, content).ConfigureAwait(false);
        }

        private static object GenerateWebhook(string results, string thumbnail)
        {
            var WebHook = new
            {
                username = $"PokeViewer.NET",
                content = $"<@{Settings.Default.UserDiscordID}>",
                embeds = new List<object>
                {
                    new
                    {
                        title = $"Match Found!",
                        thumbnail = new
                        {
                            url = thumbnail
                        },
                        fields = new List<object>
                        {
                            new { name = "Description               ", value = results, inline = true, },
                        },
                    }
                }
            };
            return WebHook;
        }

        // Read, Decrypt, and Write Block tasks from Tera-Finder/RaidCrawler/sv-livemap.
        #region saveblocktasks
        public static byte[] DecryptBlock(uint key, byte[] block)
        {
            var rng = new SCXorShift32(key);
            for (int i = 0; i < block.Length; i++)
                block[i] = (byte)(block[i] ^ rng.Next());
            return block;
        }

        private async Task<(byte, ulong)> ReadEncryptedBlockByte(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (header[1], address);
        }

        private async Task<(byte[], ulong)> ReadEncryptedBlockHeader(DataBlock block, ulong init, CancellationToken token)
        {
            if (init == 0)
            {
                var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
                init = address;
            }

            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(init, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);

            return (header, init);
        }

        private async Task<(byte[]?, ulong)> ReadEncryptedBlockArray(DataBlock block, ulong init, CancellationToken token)
        {
            if (init == 0)
            {
                var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
                init = address;
            }

            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(init, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return (data[6..], init);
        }

        private async Task<(uint, ulong)> ReadEncryptedBlockUint(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadUInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        private async Task<(int, ulong)> ReadEncryptedBlockInt32(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        private async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data);
            return res[0] == 2;
        }

        private async Task<byte[]> ReadBlock(DataBlock block, CancellationToken token)
        {
            return await ReadEncryptedBlock(block, token).ConfigureAwait(false);
        }

        private async Task<byte[]> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return data[6..];
        }

        private async Task<byte[]?> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            var size = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5 + (int)size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data)[5..];

            return res;
        }

        public async Task<bool> WriteBlock(object data, DataBlock block, CancellationToken token, object? toExpect = default)
        {
            if (block.IsEncrypted)
                return await WriteEncryptedBlockSafe(block, toExpect, data, token).ConfigureAwait(false);
            else
                return await WriteDecryptedBlock((byte[])data!, block, token).ConfigureAwait(false);
        }

        private async Task<bool> WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            await Executor.SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockSafe(DataBlock block, object? toExpect, object toWrite, CancellationToken token)
        {
            if (toExpect == default || toWrite == default)
                return false;

            return block.Type switch
            {
                SCTypeCode.Array => await WriteEncryptedBlockArray(block, (byte[])toExpect, (byte[])toWrite, token).ConfigureAwait(false),
                SCTypeCode.Bool1 or SCTypeCode.Bool2 or SCTypeCode.Bool3 => await WriteEncryptedBlockBool(block, (bool)toExpect, (bool)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Byte or SCTypeCode.SByte => await WriteEncryptedBlockByte(block, (byte)toExpect, (byte)toWrite, token).ConfigureAwait(false),
                SCTypeCode.UInt32 or SCTypeCode.UInt64 => await WriteEncryptedBlockUint(block, (uint)toExpect, (uint)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Int32 => await WriteEncryptedBlockInt32(block, (int)toExpect, (int)toWrite, token).ConfigureAwait(false),
                _ => throw new NotSupportedException($"Block {block.Name} (Type {block.Type}) is currently not supported.")
            };
        }

        private async Task<bool> WriteEncryptedBlockUint(DataBlock block, uint valueToExpect, uint valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteUInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockInt32(DataBlock block, int valueToExpect, int valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockByte(DataBlock block, byte valueToExpect, byte valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = header[1];
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            header[1] = valueToInject;
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockArray(DataBlock block, byte[] arrayToExpect, byte[] arrayToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[6..];
            if (!ram.SequenceEqual(arrayToExpect)) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            Array.ConstrainedCopy(arrayToInject, 0, data, 6, block.Size);
            data = EncryptBlock(block.Key, data);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[0] == 2;
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            data[0] = valueToInject ? (byte)2 : (byte)1;
            data = EncryptBlock(block.Key, data);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);

            return true;
        }

        public static byte[] EncryptBlock(uint key, byte[] block) => DecryptBlock(key, block);

        public async Task<ulong> SearchSaveKey(uint key, CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(BaseBlockKeyPointer + 8, 16, token).ConfigureAwait(false);
            var start = BitConverter.ToUInt64(data.AsSpan()[..8]);
            var end = BitConverter.ToUInt64(data.AsSpan()[8..]);

            while (start < end)
            {
                var block_ct = (end - start) / 48;
                var mid = start + (block_ct >> 1) * 48;

                data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(mid, 4, token).ConfigureAwait(false);
                var found = BitConverter.ToUInt32(data);
                if (found == key)
                    return mid;

                if (found >= key)
                    end = mid;
                else start = mid + 48;
            }
            return start;
        }
        #endregion

        private async Task SVSaveGameOverworld(CancellationToken token)
        {
            await Click(X, 2_000, token).ConfigureAwait(false);
            await Click(R, 1_800, token).ConfigureAwait(false);
            await Click(A, 5_000, token).ConfigureAwait(false);
            for (int i = 0; i < 6; i++) // Mash B
                await Click(B, 0_500, token).ConfigureAwait(false);
        }

        private void EnableAssets()
        {
            OutbreakScan.Enabled = true;
            OutbreakScan.Text = "Scan";
            OutbreakSearch.Enabled = true;
            Apply0To77.Enabled = true;
            OpenMapPaldea.Enabled = true;
            OpenMapKitakami.Enabled = true;
            ScanForEventOutbreak.Enabled = true;
        }

        private void DisableAssets()
        {
            OutbreakScan.Enabled = false;
            OutbreakSearch.Enabled = false;
            Apply0To77.Enabled = false;
            OpenMapPaldea.Enabled = false;
            OpenMapKitakami.Enabled = false;
            ScanForEventOutbreak.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string r = string.Empty;
            foreach (var p in pkList.ToList())
            {
                r += $"{(Species)p.Species} - {p.Form}{Environment.NewLine}";
            }
            MessageBox.Show(r, "Search List");
        }

        private void AddSpecies_Click(object sender, EventArgs e)
        {
            PK9 pk = new()
            {
                Species = (ushort)(Species)SpeciesBox.SelectedIndex,
                Form = (byte)FormBox.Value,
            };

            if (pk.Species == 0)
            {
                MessageBox.Show("You can't add a blank species!", "Search List");
                return;
            }

            if (pkList.Count == 0)
            {
                pkList.Add(pk);
                MessageBox.Show($"Added {(Species)pk.Species}", "Search List");
                return;
            }
            foreach (var p in pkList.ToList())
            {
                if (p.Species == pk.Species && p.Form == pk.Form)
                {
                    MessageBox.Show($"List contains {(Species)pk.Species} with Form {pk.Form}", "Search List");
                    continue;
                }

                if (p.Species != pk.Species)
                {
                    pkList.Add(pk);
                    MessageBox.Show($"Added {(Species)pk.Species}", "Search List");
                    return;
                }
            }
        }

        private void RemoveSpecies_Click(object sender, EventArgs e)
        {
            if (pkList.Count == 0)
                return;

            PK9 pk = new()
            {
                Species = (ushort)(Species)SpeciesBox.SelectedIndex,
                Form = (byte)FormBox.Value,
            };
            foreach (var p in pkList.ToList())
            {
                if (p.Species != pk.Species && p.Form != pk.Form)
                    continue;

                if (p.Species == pk.Species && p.Form == pk.Form)
                {
                    pkList.Remove(p);
                    MessageBox.Show($"Removed {(Species)p.Species}", "Search List");
                    return;
                }
            }
        }

        private void SaveList_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            foreach (var p in pkList.ToList())
            {
                output += $"{p.Species}-{p.Form},";
            }
            using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "outbreakfilters.txt"));
            sw.Write(output);
            MessageBox.Show("Done", "Search List");
        }

        private void ClearList_Click(object sender, EventArgs e)
        {
            pkList.Clear();
            pkList = new();
            MessageBox.Show("Cleared all filters", "Search List");
        }

        private void OpenMap_Click(object sender, EventArgs e)
        {
            MapViewPaldea form = new(MapSpritesP, MapPOSP);
            form.ShowDialog();
        }

        private void OpenMapKitakami_Click(object sender, EventArgs e)
        {
            MapViewKitakami form = new(MapSpritesK, MapPOSK);
            form.ShowDialog();
        }

        private void TimeViewerButton_Click(object sender, EventArgs e)
        {
            TimeViewer form = new(Executor);
            form.ShowDialog();
        }
    }
}
