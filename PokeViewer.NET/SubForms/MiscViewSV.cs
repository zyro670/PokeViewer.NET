using Newtonsoft.Json;
using PKHeX.Core;
using PKHeX.Drawing.Misc;
using PKHeX.Drawing.PokeSprite;
using PokeViewer.NET.Properties;
using RaidCrawler.Core.Structures;
using SysBot.Base;
using System.Text;
using static PokeViewer.NET.RoutineExecutor;
using static SysBot.Base.SwitchButton;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokeViewer.NET.SubForms
{
    public partial class MiscViewSV : Form
    {
        private readonly ViewerExecutor Executor;
        private static ulong BaseBlockKeyPointer = 0;
        public ulong CountCacheP;
        public ulong CountCacheK;
        public ulong CountCacheB;
        public ulong CountCacheBCP;
        public ulong CountCacheBCK;
        public ulong CountCacheBCB;
        private DateTime StartTime;
        private System.Timers.Timer timer = new();
        public List<OutbreakStash> OutbreakCache = new();
        public List<OutbreakStash> BCATOutbreakCache = new();
        private List<Image> MapSpritesP = new();
        private List<byte[]?> MapPOSP = new();
        private List<byte[]?> MapPOSObP = new();
        private List<string> MapCountP = new();
        private List<string> MapStringsP = new();
        private List<Image> MapSpritesObP = new();
        private List<string> MapCountObP = new();
        private List<string> MapStringsObP = new();
        private List<Image> MapSpritesK = new();
        private List<byte[]?> MapPOSK = new();
        private List<byte[]?> MapPOSObK = new();
        private List<string> MapCountK = new();
        private List<string> MapStringsK = new();
        private List<Image> MapSpritesObK = new();
        private List<string> MapCountObK = new();
        private List<string> MapStringsObK = new();
        private List<Image> MapSpritesB = new();
        private List<byte[]?> MapPOSB = new();
        private List<byte[]?> MapPOSObB = new();
        private List<string> MapCountB = new();
        private List<string> MapStringsB = new();
        private List<Image> MapSpritesObB = new();
        private List<string> MapCountObB = new();
        private List<string> MapStringsObB = new();
        private List<PK9> pkList = new();
        protected ViewerOffsets Offsets { get; } = new();
        public MiscViewSV(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            MapGroup.SelectedIndex = Settings.Default.MapSetting;
            SpeciesBox.DataSource = Enum.GetValues(typeof(Species)).Cast<Species>().Where(z => z != Species.MAX_COUNT).ToArray();
            var path = "refs\\outbreakfilters.txt";
            if (File.Exists(path))
                LoadFilters(path);

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
            OutbreakGroup.BackColor = color.Item1;
            OutbreakGroup.ForeColor = color.Item2;
            SearchListGroup.BackColor = color.Item1;
            SearchListGroup.ForeColor = color.Item2;
            OutbreakIcon.BackColor = color.Item1;
            OutbreakIcon.ForeColor = color.Item2;
            SpeciesBox.BackColor = color.Item1;
            SpeciesBox.ForeColor = color.Item2;
            FormBox.BackColor = color.Item1;
            FormBox.ForeColor = color.Item2;
            MapGroup.BackColor = color.Item1;
            MapGroup.ForeColor = color.Item2;
            FwdButton.BackColor = color.Item1;
            FwdButton.ForeColor = color.Item2;
            BwdButton.BackColor = color.Item1;
            BwdButton.ForeColor = color.Item2;
            ResetButton.BackColor = color.Item1;
            ResetButton.ForeColor = color.Item2;
            TimeNumeric.BackColor = color.Item1;
            TimeNumeric.ForeColor = color.Item2;
            TimeGroup.BackColor = color.Item1;
            TimeGroup.ForeColor = color.Item2;
            BackColor = color.Item1;
            ForeColor = color.Item2;
            ClearFCE.BackColor = color.Item1;
            ClearFCE.ForeColor = color.Item2;
            EnterButton.BackColor = color.Item1;
            EnterButton.ForeColor = color.Item2;
            AutoPaste.BackColor = color.Item1;
            AutoPaste.ForeColor = color.Item2;
            GoButton.BackColor = color.Item1;
            GoButton.ForeColor = color.Item2;
            FCETextBox.BackColor = color.Item1;
            FCETextBox.ForeColor = color.Item2;
            groupBox2.BackColor = color.Item1;
            groupBox2.ForeColor = color.Item2;
            Results.BackColor = color.Item1;
            Results.ForeColor = color.Item2;
            RaidSensCheck.BackColor = color.Item1;
            RaidSensCheck.ForeColor = color.Item2;
            SeedToPokemonGroup.BackColor = color.Item1;
            SeedToPokemonGroup.ForeColor = color.Item2;
            SeedLabel.BackColor = color.Item1;
            SeedLabel.ForeColor = color.Item2;
            RaidNumeric.BackColor = color.Item1;
            RaidNumeric.ForeColor = color.Item2;
        }

        public void LoadOutbreakCache()
        {
            for (int i = 0; i < 18; i++)
                OutbreakCache.Add(new());
        }

        private void SelectedIndex_IsChanged(object sender, EventArgs e)
        {
            OutbreakCache = new();
            LoadOutbreakCache();
            CountCacheP = 0;
            CountCacheK = 0;
            CountCacheB = 0;
            CountCacheBCP = 0;
            CountCacheBCK = 0;
            Settings.Default.MapSetting = MapGroup.SelectedIndex;
            Settings.Default.Save();
        }
        private async Task SearchForOutbreak(CancellationToken token)
        {
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);

            List<Image> ImgP = new();
            List<Image> ImgK = new();
            List<Image> ImgB = new();
            List<Image> ImgObP = new();
            List<Image> ImgObK = new();
            List<Image> ImgObB = new();
            List<byte[]?> POSlistP = new();
            List<byte[]?> POSlistK = new();
            List<byte[]?> POSlistB = new();
            List<byte[]?> POSlistObP = new();
            List<byte[]?> POSlistObK = new();
            List<byte[]?> POSlistObB = new();
            List<uint> kolistP = new();
            List<uint> kolistK = new();
            List<uint> kolistB = new();
            List<uint> kolistObP = new();
            List<uint> kolistObK = new();
            List<uint> kolistObB = new();
            List<uint> totallistP = new();
            List<uint> totallistK = new();
            List<uint> totallistB = new();
            List<uint> totallistObP = new();
            List<uint> totallistObK = new();
            List<uint> totallistObB = new();
            List<string> stringsP = new();
            List<string> stringsK = new();
            List<string> stringsB = new();
            List<string> stringsObP = new();
            List<string> stringsObK = new();
            List<string> stringsObB = new();
            List<string> CountlistP = new();
            List<string> CountlistK = new();
            List<string> CountlistB = new();
            List<string> CountlistObP = new();
            List<string> CountlistObK = new();
            List<string> CountlistObB = new();
            List<PK9> monsP = new();
            List<PK9> monsK = new();
            List<PK9> monsB = new();
            List<PK9> monsObP = new();
            List<PK9> monsObK = new();
            List<PK9> monsObB = new();
            MapSpritesP = new();
            MapPOSP = new();
            MapCountP = new();
            MapStringsP = new();
            MapSpritesK = new();
            MapCountK = new();
            MapStringsK = new();
            MapPOSK = new();
            MapSpritesB = new();
            MapPOSB = new();
            MapCountB = new();
            MapStringsB = new();

            DisableAssets();
            int dayskip = 0;
            int OutbreaktotalP = 0;
            int OutbreaktotalK = 0;
            int OutbreaktotalB = 0;
            int OutbreaktotalBCP = 0;
            int OutbreaktotalBCK = 0;
            int OutbreaktotalBCB = 0;
            while (!token.IsCancellationRequested)
            {
                dayskip++;

                if (dayskip % 2 == 0)
                    await ResetTime(token).ConfigureAwait(false);

                if (HardStopOutbreak.Checked)
                {
                    MessageBox.Show("HardStop enabled, ending task. Uncheck if you wish to scan until match is found.");
                    break;
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

                if (MapGroup.SelectedIndex is 0 or 1)
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
                        UpdateProgress(20, 100);
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
                        string pkform = FormOutput(pk.Species, pk.Form, out _);
                        stringsP.Add($"{(Species)pk.Species}-{pkform}");
                        CountlistP.Add($"{kocount}/{totalcount}");
                        var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                        ImgP.Add(img);
                        monsP.Add(pk);
                        POSlistP.Add(obpos);
                        kolistP.Add(kocount);
                        totallistP.Add(totalcount);
                    }
                }

                if (MapGroup.SelectedIndex is 0 or 2)
                {
                    var dataK = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalKitakami, CountCacheK, token).ConfigureAwait(false);
                    UpdateProgress(30, 100);
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

                if (MapGroup.SelectedIndex is 0 or 3)
                {
                    var dataB = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalBlueberry, CountCacheB, token).ConfigureAwait(false);
                    UpdateProgress(40, 100);
                    if (CountCacheB == 0)
                        CountCacheB = dataB.Item2;

                    OutbreaktotalB = Convert.ToInt32(dataB.Item1);
                    if (OutbreaktotalB > 8)
                    {
                        BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                        // Rerun in case of bad pointer
                        OutbreakCache = new();
                        LoadOutbreakCache();
                        CountCacheB = 0;
                        continue;
                    }

                    for (int i = 12; i < 17; i++)
                    {
                        switch (i)
                        {
                            case 12: block = Blocks.KOutbreakSpecies13; formblock = Blocks.KMassOutbreak13Form; koblock = Blocks.KMassOutbreakKO13; totalblock = Blocks.KMassOutbreak13TotalSpawns; pos = Blocks.KMassOutbreak13CenterPos; break;
                            case 13: block = Blocks.KOutbreakSpecies14; formblock = Blocks.KMassOutbreak14Form; koblock = Blocks.KMassOutbreakKO14; totalblock = Blocks.KMassOutbreak14TotalSpawns; pos = Blocks.KMassOutbreak14CenterPos; break;
                            case 14: block = Blocks.KOutbreakSpecies15; formblock = Blocks.KMassOutbreak15Form; koblock = Blocks.KMassOutbreakKO15; totalblock = Blocks.KMassOutbreak15TotalSpawns; pos = Blocks.KMassOutbreak15CenterPos; break;
                            case 15: block = Blocks.KOutbreakSpecies16; formblock = Blocks.KMassOutbreak16Form; koblock = Blocks.KMassOutbreakKO16; totalblock = Blocks.KMassOutbreak16TotalSpawns; pos = Blocks.KMassOutbreak16CenterPos; break;
                            case 16: block = Blocks.KOutbreakSpecies17; formblock = Blocks.KMassOutbreak17Form; koblock = Blocks.KMassOutbreakKO17; totalblock = Blocks.KMassOutbreak17TotalSpawns; pos = Blocks.KMassOutbreak17CenterPos; break;
                        }
                        if (i > OutbreaktotalB + 12 - 1)
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
                        stringsB.Add($"{(Species)pk.Species}{pkform}");
                        CountlistB.Add($"{kocount}/{totalcount}");
                        var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                        ImgB.Add(img);
                        monsB.Add(pk);
                        POSlistB.Add(obpos);
                        kolistB.Add(kocount);
                        totallistB.Add(totalcount);
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
                        for (int i = 0; i < 30; i++)
                            BCATOutbreakCache.Add(new());

                        if (MapGroup.SelectedIndex is 0 or 1)
                        {
                            var dataBCP = await ReadEncryptedBlockByte(Blocks.KOutbreakBCMainNumActive, CountCacheBCP, token).ConfigureAwait(false);
                            UpdateProgress(50, 100);
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
                                    case 1: BCOspecies = Blocks.KOutbreakBC02MainSpecies; BCOform = Blocks.KOutbreakBC02MainForm; BCOko = Blocks.KOutbreakBC02MainNumKOed; BCOtotal = Blocks.KOutbreakBC02MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC02MainCenterPos; break;
                                    case 2: BCOspecies = Blocks.KOutbreakBC03MainSpecies; BCOform = Blocks.KOutbreakBC03MainForm; BCOko = Blocks.KOutbreakBC03MainNumKOed; BCOtotal = Blocks.KOutbreakBC03MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC03MainCenterPos; break;
                                    case 3: BCOspecies = Blocks.KOutbreakBC04MainSpecies; BCOform = Blocks.KOutbreakBC04MainForm; BCOko = Blocks.KOutbreakBC04MainNumKOed; BCOtotal = Blocks.KOutbreakBC04MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC04MainCenterPos; break;
                                    case 4: BCOspecies = Blocks.KOutbreakBC05MainSpecies; BCOform = Blocks.KOutbreakBC05MainForm; BCOko = Blocks.KOutbreakBC05MainNumKOed; BCOtotal = Blocks.KOutbreakBC05MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC05MainCenterPos; break;
                                    case 5: BCOspecies = Blocks.KOutbreakBC06MainSpecies; BCOform = Blocks.KOutbreakBC06MainForm; BCOko = Blocks.KOutbreakBC06MainNumKOed; BCOtotal = Blocks.KOutbreakBC06MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC06MainCenterPos; break;
                                    case 6: BCOspecies = Blocks.KOutbreakBC07MainSpecies; BCOform = Blocks.KOutbreakBC07MainForm; BCOko = Blocks.KOutbreakBC07MainNumKOed; BCOtotal = Blocks.KOutbreakBC07MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC07MainCenterPos; break;
                                    case 7: BCOspecies = Blocks.KOutbreakBC08MainSpecies; BCOform = Blocks.KOutbreakBC08MainForm; BCOko = Blocks.KOutbreakBC08MainNumKOed; BCOtotal = Blocks.KOutbreakBC08MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC08MainCenterPos; break;
                                    case 8: BCOspecies = Blocks.KOutbreakBC09MainSpecies; BCOform = Blocks.KOutbreakBC09MainForm; BCOko = Blocks.KOutbreakBC09MainNumKOed; BCOtotal = Blocks.KOutbreakBC09MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC09MainCenterPos; break;
                                    case 9: BCOspecies = Blocks.KOutbreakBC10MainSpecies; BCOform = Blocks.KOutbreakBC10MainForm; BCOko = Blocks.KOutbreakBC10MainNumKOed; BCOtotal = Blocks.KOutbreakBC10MainTotalSpawns; BCOcenter = Blocks.KOutbreakBC10MainCenterPos; break;
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

                        if (MapGroup.SelectedIndex is 0 or 2)
                        {
                            var dataBCK = await ReadEncryptedBlockByte(Blocks.KOutbreakBCDLC1NumActive, CountCacheBCK, token).ConfigureAwait(false);
                            UpdateProgress(60, 100);
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
                                    case 11: BCOspecies = Blocks.KOutbreakBC02DLC1Species; BCOform = Blocks.KOutbreakBC02DLC1Form; BCOko = Blocks.KOutbreakBC02DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC02DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC02DLC1CenterPos; break;
                                    case 12: BCOspecies = Blocks.KOutbreakBC03DLC1Species; BCOform = Blocks.KOutbreakBC03DLC1Form; BCOko = Blocks.KOutbreakBC03DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC03DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC03DLC1CenterPos; break;
                                    case 13: BCOspecies = Blocks.KOutbreakBC04DLC1Species; BCOform = Blocks.KOutbreakBC04DLC1Form; BCOko = Blocks.KOutbreakBC04DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC04DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC04DLC1CenterPos; break;
                                    case 14: BCOspecies = Blocks.KOutbreakBC05DLC1Species; BCOform = Blocks.KOutbreakBC05DLC1Form; BCOko = Blocks.KOutbreakBC05DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC05DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC05DLC1CenterPos; break;
                                    case 15: BCOspecies = Blocks.KOutbreakBC06DLC1Species; BCOform = Blocks.KOutbreakBC06DLC1Form; BCOko = Blocks.KOutbreakBC06DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC06DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC06DLC1CenterPos; break;
                                    case 16: BCOspecies = Blocks.KOutbreakBC07DLC1Species; BCOform = Blocks.KOutbreakBC07DLC1Form; BCOko = Blocks.KOutbreakBC07DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC07DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC07DLC1CenterPos; break;
                                    case 17: BCOspecies = Blocks.KOutbreakBC08DLC1Species; BCOform = Blocks.KOutbreakBC08DLC1Form; BCOko = Blocks.KOutbreakBC08DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC08DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC08DLC1CenterPos; break;
                                    case 18: BCOspecies = Blocks.KOutbreakBC09DLC1Species; BCOform = Blocks.KOutbreakBC09DLC1Form; BCOko = Blocks.KOutbreakBC09DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC09DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC09DLC1CenterPos; break;
                                    case 19: BCOspecies = Blocks.KOutbreakBC10DLC1Species; BCOform = Blocks.KOutbreakBC10DLC1Form; BCOko = Blocks.KOutbreakBC10DLC1NumKOed; BCOtotal = Blocks.KOutbreakBC10DLC1TotalSpawns; BCOcenter = Blocks.KOutbreakBC10DLC1CenterPos; break;
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

                        if (MapGroup.SelectedIndex is 0 or 3)
                        {
                            var dataBCB = await ReadEncryptedBlockByte(Blocks.KOutbreakBCDLC2NumActive, CountCacheBCB, token).ConfigureAwait(false);
                            UpdateProgress(70, 100);
                            if (CountCacheBCB == 0)
                                CountCacheBCB = dataBCB.Item2;

                            OutbreaktotalBCB = Convert.ToInt32(dataBCB.Item1);
                            if (OutbreaktotalBCB > 10)
                            {
                                BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
                                // Rerun in case of bad pointer
                                BCATOutbreakCache = new();
                                LoadOutbreakCache();
                                CountCacheBCB = 0;
                                continue;
                            }

                            BCOspecies = Blocks.KOutbreakBC01DLC2Species;
                            BCOko = Blocks.KOutbreakBC01DLC2NumKOed;
                            BCOtotal = Blocks.KOutbreakBC01DLC2TotalSpawns;
                            BCOform = Blocks.KOutbreakBC01DLC2Form;
                            BCOcenter = Blocks.KOutbreakBC01DLC2CenterPos;

                            for (int i = 20; i < 30; i++)
                            {
                                switch (i)
                                {
                                    case 20: break;
                                    case 21: BCOspecies = Blocks.KOutbreakBC02DLC2Species; BCOform = Blocks.KOutbreakBC02DLC2Form; BCOko = Blocks.KOutbreakBC02DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC02DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC02DLC2CenterPos; break;
                                    case 22: BCOspecies = Blocks.KOutbreakBC03DLC2Species; BCOform = Blocks.KOutbreakBC03DLC2Form; BCOko = Blocks.KOutbreakBC03DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC03DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC03DLC2CenterPos; break;
                                    case 23: BCOspecies = Blocks.KOutbreakBC04DLC2Species; BCOform = Blocks.KOutbreakBC04DLC2Form; BCOko = Blocks.KOutbreakBC04DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC04DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC04DLC2CenterPos; break;
                                    case 24: BCOspecies = Blocks.KOutbreakBC05DLC2Species; BCOform = Blocks.KOutbreakBC05DLC2Form; BCOko = Blocks.KOutbreakBC05DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC05DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC05DLC2CenterPos; break;
                                    case 25: BCOspecies = Blocks.KOutbreakBC06DLC2Species; BCOform = Blocks.KOutbreakBC06DLC2Form; BCOko = Blocks.KOutbreakBC06DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC06DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC06DLC2CenterPos; break;
                                    case 26: BCOspecies = Blocks.KOutbreakBC07DLC2Species; BCOform = Blocks.KOutbreakBC07DLC2Form; BCOko = Blocks.KOutbreakBC07DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC07DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC07DLC2CenterPos; break;
                                    case 27: BCOspecies = Blocks.KOutbreakBC08DLC2Species; BCOform = Blocks.KOutbreakBC08DLC2Form; BCOko = Blocks.KOutbreakBC08DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC08DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC08DLC2CenterPos; break;
                                    case 28: BCOspecies = Blocks.KOutbreakBC09DLC2Species; BCOform = Blocks.KOutbreakBC09DLC2Form; BCOko = Blocks.KOutbreakBC09DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC09DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC09DLC2CenterPos; break;
                                    case 29: BCOspecies = Blocks.KOutbreakBC10DLC2Species; BCOform = Blocks.KOutbreakBC10DLC2Form; BCOko = Blocks.KOutbreakBC10DLC2NumKOed; BCOtotal = Blocks.KOutbreakBC10DLC2TotalSpawns; BCOcenter = Blocks.KOutbreakBC10DLC2CenterPos; break;
                                }
                                if (i > OutbreaktotalBCB + 10)
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
                                stringsObB.Add($"{(Species)pk.Species}{pkform}");
                                CountlistObB.Add($"{kocount}/{totalcount}");
                                var img = SpriteUtil.SB8a.GetSprite(pk.Species, pk.Form, 0, 0, 0, false, Shiny.Never, EntityContext.Gen9);
                                ImgObB.Add(img);
                                monsObB.Add(pk);
                                POSlistObB.Add(obpos);
                                kolistObB.Add(kocount);
                                totallistObB.Add(totalcount);
                            }
                        }
                    }
                }

                UpdateProgress(100, 100);
                var textp = ScanForEventOutbreak.Checked ? OutbreaktotalBCP + OutbreaktotalP : OutbreaktotalP;
                var textk = ScanForEventOutbreak.Checked ? OutbreaktotalBCK + OutbreaktotalK : OutbreaktotalK;
                var textb = ScanForEventOutbreak.Checked ? OutbreaktotalBCB + OutbreaktotalB : OutbreaktotalB;
                OpenMapPaldea.Text = $"Paldea: {textp}";
                OpenMapKitakami.Text = $"Kitakami: {textk}";
                OpenMapBlueberry.Text = $"Blueberry: {textb}";

                foreach (var pips in ImgP)
                    MapSpritesP.Add(pips);
                foreach (var pps in POSlistP)
                    MapPOSP.Add(pps);
                foreach (var kps in CountlistP)
                    MapCountP.Add(kps);
                foreach (var kps in stringsP)
                    MapStringsP.Add(kps);

                foreach (var kips in ImgK)
                    MapSpritesK.Add(kips);
                foreach (var kps in POSlistK)
                    MapPOSK.Add(kps);
                foreach (var kps in CountlistK)
                    MapCountK.Add(kps);
                foreach (var kps in stringsK)
                    MapStringsK.Add(kps);

                foreach (var bips in ImgB)
                    MapSpritesB.Add(bips);
                foreach (var bps in POSlistB)
                    MapPOSB.Add(bps);
                foreach (var bps in CountlistB)
                    MapCountB.Add(bps);
                foreach (var bps in stringsB)
                    MapStringsB.Add(bps);

                if (ScanForEventOutbreak.Checked)
                {
                    foreach (var pips in ImgObP)
                        MapSpritesObP.Add(pips);
                    foreach (var pps in POSlistObP)
                        MapPOSObP.Add(pps);
                    foreach (var kps in CountlistObP)
                        MapCountObP.Add(kps);
                    foreach (var kps in stringsObP)
                        MapStringsObP.Add(kps);

                    await Task.Delay(0_500, token).ConfigureAwait(false);

                    foreach (var kips in ImgObK)
                        MapSpritesObK.Add(kips);
                    foreach (var kps in POSlistObK)
                        MapPOSObK.Add(kps);
                    foreach (var kps in CountlistObK)
                        MapCountObK.Add(kps);
                    foreach (var kps in stringsObK)
                        MapStringsObK.Add(kps);

                    await Task.Delay(0_500, token).ConfigureAwait(false);

                    foreach (var kips in ImgObB)
                        MapSpritesObB.Add(kips);
                    foreach (var kps in POSlistObB)
                        MapPOSObB.Add(kps);
                    foreach (var bps in CountlistObB)
                        MapCountObB.Add(bps);
                    foreach (var bps in stringsObB)
                        MapStringsObB.Add(bps);
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

                        if (Apply0To64.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 64...";
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

                        if (Apply0To64.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 64...";
                            await ResetGame(token, true).ConfigureAwait(false);
                            StatusLabel.Text = "Status:";
                        }
                        return;
                    }
                }

                for (int i = 0; i < monsB.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if ((Species)p.Species == (Species)monsB[i].Species && p.Form == monsB[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        timer.Stop();
                        string msg = $"{(Species)monsB[i].Species} outbreak found!";
                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(monsB[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        EnableAssets();
                        timer.Stop();

                        if (Apply0To64.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 64...";
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

                        if (Apply0To64.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 64...";
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

                        if (Apply0To64.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 64...";
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

                for (int i = 0; i < monsObB.Count; i++)
                {
                    bool hunted = false;
                    foreach (var p in pkList)
                    {
                        if (p.Species == monsObB[i].Species && p.Form == monsObB[i].Form)
                            hunted = true;
                    }
                    if (hunted is true && OutbreakSearch.Checked)
                    {
                        timer.Stop();
                        string msg = $"{(Species)monsObB[i].Species} outbreak found!";
                        EnableAssets();
                        timer.Stop();

                        if (Apply0To64.Checked)
                        {
                            StatusLabel.Text = "Status: 0 -> 64...";
                            await ResetGame(token, true).ConfigureAwait(false);
                            StatusLabel.Text = "Status:";
                        }

                        if (EnableWebhook.Checked)
                        {
                            var sprite = PokeImg(monsObB[i], false);
                            SendNotifications(msg, sprite);
                        }
                        else
                            MessageBox.Show(msg);

                        return;
                    }
                }

                if (OutbreakSearch.Checked)
                {
                    ImgP = new();
                    ImgObP = new();
                    POSlistP = new();
                    POSlistObP = new();
                    kolistP = new();
                    kolistObP = new();
                    totallistP = new();
                    totallistObP = new();
                    stringsP = new();
                    stringsObP = new();
                    monsP = new();
                    monsObP = new();
                    CountlistP = new();
                    CountlistObP = new();

                    ImgK = new();
                    ImgObK = new();
                    POSlistK = new();
                    POSlistObK = new();
                    kolistK = new();
                    kolistObK = new();
                    totallistK = new();
                    totallistObK = new();
                    stringsK = new();
                    stringsObK = new();
                    monsK = new();
                    monsObK = new();
                    CountlistK = new();
                    CountlistObK = new();

                    ImgB = new();
                    ImgObB = new();
                    POSlistB = new();
                    POSlistObB = new();
                    kolistB = new();
                    kolistObB = new();
                    totallistB = new();
                    totallistObB = new();
                    stringsB = new();
                    stringsObB = new();
                    monsB = new();
                    monsObB = new();
                    CountlistB = new();
                    CountlistObB = new();

                    MapSpritesP = new();
                    MapPOSP = new();
                    MapCountP = new();
                    MapStringsP = new();
                    MapSpritesK = new();
                    MapCountK = new();
                    MapStringsK = new();
                    MapPOSK = new();
                    MapSpritesB = new();
                    MapPOSB = new();
                    MapCountB = new();
                    MapStringsB = new();
                }

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
                    for (int i = 0; i < 1; i++)
                        await TimeSkipBwd(token).ConfigureAwait(false);
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
                await KOToSixtyFour_Click(token).ConfigureAwait(false);
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

        private async Task KOToSixtyFour_Click(CancellationToken token)
        {
            OutbreakScan.Enabled = false;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var (validOutbreaksP, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalPaldea, 0, token).ConfigureAwait(false);
            var (validOutbreaksK, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalKitakami, 0, token).ConfigureAwait(false);
            var (validOutbreaksB, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalKitakami, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCP, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01MainTotalSpawns, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCK, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01DLC1TotalSpawns, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCB, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01DLC1TotalSpawns, 0, token).ConfigureAwait(false);

            var OutbreaktotalP = Convert.ToInt32(validOutbreaksP);
            var OutbreaktotalK = Convert.ToInt32(validOutbreaksK);
            var OutbreaktotalB = Convert.ToInt32(validOutbreaksB);
            var OutbreaktotalBCP = Convert.ToInt32(validOutbreaksBCP);
            var OutbreaktotalBCK = Convert.ToInt32(validOutbreaksBCK);
            var OutbreaktotalBCB = Convert.ToInt32(validOutbreaksBCB);
            var koblock = Blocks.KMassOutbreakKO1;
            if (MapGroup.SelectedIndex is 0 or 1)
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
                    uint inj = 64;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (MapGroup.SelectedIndex is 0 or 2)
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
                    uint inj = 64;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (MapGroup.SelectedIndex is 0 or 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    StatusLabel.Text = $"KOing Blueberry: {8 * i + 1}%";
                    switch (i)
                    {
                        case 0: koblock = Blocks.KMassOutbreakKO13; break;
                        case 1: koblock = Blocks.KMassOutbreakKO14; break;
                        case 2: koblock = Blocks.KMassOutbreakKO15; break;
                        case 3: koblock = Blocks.KMassOutbreakKO16; break;
                        case 4: koblock = Blocks.KMassOutbreakKO17; break;
                    }
                    if (i > OutbreaktotalB - 1)
                        continue;
                    var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                    uint inj = 64;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (MapGroup.SelectedIndex is 0 or 1 && ScanForEventOutbreak.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    StatusLabel.Text = $"KOing PaldeaBC: {10 * i}%";
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
                    uint inj = 64;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (MapGroup.SelectedIndex is 0 or 2 && ScanForEventOutbreak.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    StatusLabel.Text = $"KOing KitaBC: {10 * i}%";
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
                    uint inj = 64;
                    await WriteBlock(inj, koblock, token, (uint)currentcount).ConfigureAwait(false);
                }
            }
            if (MapGroup.SelectedIndex is 0 or 3 && ScanForEventOutbreak.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    StatusLabel.Text = $"KOing BlueBC: {10 * i}%";
                    switch (i)
                    {
                        case 0: koblock = Blocks.KOutbreakBC01DLC2NumKOed; break;
                        case 1: koblock = Blocks.KOutbreakBC02DLC2NumKOed; break;
                        case 2: koblock = Blocks.KOutbreakBC03DLC2NumKOed; break;
                        case 3: koblock = Blocks.KOutbreakBC04DLC2NumKOed; break;
                        case 4: koblock = Blocks.KOutbreakBC05DLC2NumKOed; break;
                        case 5: koblock = Blocks.KOutbreakBC06DLC2NumKOed; break;
                        case 6: koblock = Blocks.KOutbreakBC07DLC2NumKOed; break;
                        case 7: koblock = Blocks.KOutbreakBC08DLC2NumKOed; break;
                        case 8: koblock = Blocks.KOutbreakBC09DLC2NumKOed; break;
                        case 9: koblock = Blocks.KOutbreakBC10DLC2NumKOed; break;
                    }
                    if (i > OutbreaktotalBCB - 1)
                        continue;
                    var (currentcount, _) = await ReadEncryptedBlockInt32(koblock, 0, token).ConfigureAwait(false);
                    uint inj = 64;
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
            Apply0To64.Enabled = true;
            OpenMapPaldea.Enabled = true;
            OpenMapKitakami.Enabled = true;
            OpenMapBlueberry.Enabled = true;
            ScanForEventOutbreak.Enabled = true;
        }

        private void DisableAssets()
        {
            OutbreakScan.Enabled = false;
            OutbreakSearch.Enabled = false;
            Apply0To64.Enabled = false;
            OpenMapPaldea.Enabled = false;
            OpenMapKitakami.Enabled = false;
            OpenMapBlueberry.Enabled = false;
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
                string output = string.Empty;
                foreach (var pA in pkList.ToList())
                {
                    output += $"{pA.Species}-{pA.Form},";
                }
                using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "refs", "outbreakfilters.txt"));
                sw.Write(output);
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
                    string output = string.Empty;
                    foreach (var pA in pkList.ToList())
                    {
                        output += $"{pA.Species}-{pA.Form},";
                    }
                    using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "refs", "outbreakfilters.txt"));
                    sw.Write(output);
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
                    string output = string.Empty;
                    foreach (var pA in pkList.ToList())
                    {
                        output += $"{pA.Species}-{pA.Form},";
                    }
                    using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "refs", "outbreakfilters.txt"));
                    sw.Write(output);
                    return;
                }
            }
        }

        private void ClearList_Click(object sender, EventArgs e)
        {
            pkList.Clear();
            pkList = new();
            MessageBox.Show("Cleared all filters", "Search List");
        }

        private void OpenMap_Click(object sender, EventArgs e)
        {
            MapViewPaldea form = new(MapSpritesP, MapPOSP, MapStringsP, MapCountP, MapSpritesObP, MapStringsObP, MapCountObP, MapPOSObP);
            form.ShowDialog();
        }

        private void OpenMapKitakami_Click(object sender, EventArgs e)
        {
            MapViewKitakami form = new(MapSpritesK, MapPOSK, MapStringsK, MapCountK, MapSpritesObK, MapStringsObK, MapCountObK, MapPOSObK);
            form.ShowDialog();
        }

        private void OpenMapBlueberry_Click(object sender, EventArgs e)
        {
            MapViewBlueberry form = new(MapSpritesB, MapPOSB, MapStringsB, MapCountB, MapSpritesObB, MapStringsObB, MapCountObB, MapPOSObB);
            form.ShowDialog();
            Show();
        }

        private async void FwdButton_Click(object sender, EventArgs e)
        {
            FwdButton.Enabled = false;
            BwdButton.Enabled = false;
            ResetButton.Enabled = false;

            for (int i = 0; i < TimeNumeric.Value; i++)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipForward(true), CancellationToken.None).ConfigureAwait(false);

            FwdButton.Enabled = true;
            BwdButton.Enabled = true;
            ResetButton.Enabled = true;
        }

        private async void BwdButton_Click(object sender, EventArgs e)
        {
            FwdButton.Enabled = false;
            BwdButton.Enabled = false;
            ResetButton.Enabled = false;

            for (int i = 0; i < TimeNumeric.Value; i++)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipBack(true), CancellationToken.None).ConfigureAwait(false);

            FwdButton.Enabled = true;
            BwdButton.Enabled = true;
            ResetButton.Enabled = true;
        }

        private async void ResetButton_Click(object sender, EventArgs e)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.ResetTime(true), CancellationToken.None).ConfigureAwait(false);
        }

        private async void EnterButton_Click(object sender, EventArgs e)
        {
            if (FCETextBox.Text.Length < 4 || FCETextBox.Text.Length == 5)
            {
                MessageBox.Show($"{FCETextBox.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(FCETextBox.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
            else
                MessageBox.Show("TextBox is empty. Try again after you fill it in!");
        }

        private async Task EnterRaidCode(CancellationToken token)
        {
            if (FCETextBox.Text.Length > 6)
            {
                FCETextBox.Text = FCETextBox.Text.Substring(0, 6);
            }
            var strokes = FCETextBox.Text.ToUpper().ToArray();
            var number = $"NumPad";
            string[] badVals = { "@", "I", "O", "=", "&", ";", "Z", "*", "#", "!", "?" };
            List<HidKeyboardKey> keystopress = new();
            foreach (var str in strokes)
            {
                if (badVals.Contains(str.ToString()))
                {
                    MessageBox.Show($"{str} is not a valid button. Stopping code entry.");
                    return;
                }
                foreach (HidKeyboardKey keypress in (HidKeyboardKey[])Enum.GetValues(typeof(HidKeyboardKey)))
                {
                    if (str.ToString().Equals(keypress.ToString()) || (number + str.ToString()).Equals(keypress.ToString()))
                        keystopress.Add(keypress);
                }
            }
            await Executor.SwitchConnection.SendAsync(SwitchCommand.TypeMultipleKeys(keystopress, true), token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);

        }

        private async void AutoPaste_Click(object sender, EventArgs e)
        {
            AutoPaste.Enabled = false;
            Clipboard.Clear();
            while (!Clipboard.ContainsText())
            {
                await Task.Delay(0_100);
            }
            FCETextBox.Text = Clipboard.GetText();
            if (!string.IsNullOrEmpty(FCETextBox.Text.Trim()))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);

            AutoPaste.Enabled = true;
        }

        private void ClearFCE_Click(object sender, EventArgs e)
        {
            FCETextBox.Text = string.Empty;
        }

        private async void GoButton_Click(object sender, EventArgs e)
        {
            GoButton.Enabled = false;
            RaidNumeric.Enabled = false;
            RaidSensCheck.Enabled = false;
            try
            {
                await ReadRaids(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            GoButton.Enabled = true;
            RaidNumeric.Enabled = true;
            RaidSensCheck.Enabled = true;

            RaidIcon.Load(raidimages[0]);
            Results.Text = results[0];
            TeraIcon.Image = teratype[0];
        }

        private void NumericValue_Changed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(raidimages[(int)RaidNumeric.Value - 1]))
            {
                RaidIcon.Load(raidimages[(int)RaidNumeric.Value - 1]);
                Results.Text = results[(int)RaidNumeric.Value - 1];
                TeraIcon.Image = teratype[(int)RaidNumeric.Value - 1];
                RaidDifficultyLabel.Text = $"{stars[(int)RaidNumeric.Value - 1]} ☆";
            }
            else
                MessageBox.Show("No results saved, please hit GO again.");
        }

        private (string, string) CalculateFromSeed(uint seed, PK9 pk)
        {
            var raidseed = RaidSensCheck.Checked ? "Hidden" : $"0x{seed:X8}";
            string ec = RaidSensCheck.Checked ? "Hidden" : $"{pk.EncryptionConstant:X8}";
            string pid = RaidSensCheck.Checked ? "Hidden" : $"{pk.PID:X8}";
            string form = string.Empty;
            if (pk.Form != 0)
                form = $"{pk.Form}";
            string results = $"{(Species)pk.Species}{form}{Environment.NewLine}" +
                $"Seed: {raidseed}{Environment.NewLine}" +
                $"EC: {ec}{Environment.NewLine}" +
                $"PID: {pid}{Environment.NewLine}" +
                $"IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}" +
                $"Gender: {(Gender)pk.Gender}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}" +
                $"Scale: {PokeSizeDetailedUtil.GetSizeRating(pk.Scale)} ({pk.Scale})";

            var sprite = PokeImg(pk, false);
            return (sprite, results);
        }

        public async Task<ulong> SearchSaveKeyRaid(ulong BaseBlockKeyPointer, uint key, CancellationToken token)
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

        public async Task<byte[]> ReadSaveBlockRaid(ulong BaseBlockKeyPointer, uint key, int size, CancellationToken token)
        {
            var block_ofs = await SearchSaveKeyRaid(BaseBlockKeyPointer, key, token).ConfigureAwait(false);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(block_ofs + 8, 0x8, token).ConfigureAwait(false);
            block_ofs = BitConverter.ToUInt64(data, 0);

            var block = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(block_ofs, size, token).ConfigureAwait(false);
            return DecryptBlock(key, block);
        }

        public async Task<int> GetStoryProgress(ulong BaseBlockKeyPointer, CancellationToken token)
        {
            for (int i = DifficultyFlags.Count - 1; i >= 0; i--)
            {
                // See https://github.com/Lincoln-LM/sv-live-map/pull/43
                var block = await ReadSaveBlockRaid(BaseBlockKeyPointer, DifficultyFlags[i], 1, token).ConfigureAwait(false);
                if (block[0] == 2)
                    return i + 1;
            }
            return 0;
        }

        private async Task ReadRaids(CancellationToken token)
        {
            if (RaidBlockOffsetP == 0)
                RaidBlockOffsetP = await Executor.SwitchConnection.PointerAll(Offsets.RaidBlockPointerP, token).ConfigureAwait(false);

            if (RaidBlockOffsetK == 0)
                RaidBlockOffsetK = await Executor.SwitchConnection.PointerAll(Offsets.RaidBlockPointerK, token).ConfigureAwait(false);

            if (RaidBlockOffsetB == 0)
                RaidBlockOffsetB = await Executor.SwitchConnection.PointerAll(Offsets.RaidBlockPointerB, token).ConfigureAwait(false);

            string id = await Executor.SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            var game = id switch
            {
                RaidCrawler.Core.Structures.Offsets.ScarletID => "Scarlet",
                RaidCrawler.Core.Structures.Offsets.VioletID => "Violet",
                _ => "",
            };
            container = new(game);
            container.SetGame(game);

            var BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);

            StoryProgress = await GetStoryProgress(BaseBlockKeyPointer, token).ConfigureAwait(false);
            EventProgress = Math.Min(StoryProgress, 3);

            await ReadEventRaids(BaseBlockKeyPointer, container, token).ConfigureAwait(false);

            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(RaidBlockOffsetP + RaidBlock.HEADER_SIZE, (int)RaidBlock.SIZE_BASE, token).ConfigureAwait(false);

            (int delivery, int enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0, TeraRaidMapParent.Paldea);
            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            var raids = container.Raids;
            var encounters = container.Encounters;
            var rewards = container.Rewards;
            container.ClearRaids();
            container.ClearEncounters();
            container.ClearRewards();

            data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(RaidBlockOffsetK, (int)RaidBlock.SIZE_KITAKAMI, token).ConfigureAwait(false);

            (delivery, enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0, TeraRaidMapParent.Kitakami);

            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            var allRaids = raids.Concat(container.Raids).ToList().AsReadOnly();
            var allEncounters = encounters.Concat(container.Encounters).ToList().AsReadOnly();
            var allRewards = rewards.Concat(container.Rewards).ToList().AsReadOnly();

            container.ClearRaids();
            container.ClearEncounters();
            container.ClearRewards();

            data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(RaidBlockOffsetB, (int)RaidBlock.SIZE_BLUEBERRY, token).ConfigureAwait(false);

            (delivery, enc) = container.ReadAllRaids(data, StoryProgress, EventProgress, 0, TeraRaidMapParent.Blueberry);

            if (enc > 0)
                MessageBox.Show($"Failed to find encounters for {enc} raid(s).");

            if (delivery > 0)
                MessageBox.Show($"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.");

            allRaids = allRaids.Concat(container.Raids).ToList().AsReadOnly();
            allEncounters = allEncounters.Concat(container.Encounters).ToList().AsReadOnly();
            allRewards = allRewards.Concat(container.Rewards).ToList().AsReadOnly();

            container.SetRaids(allRaids);
            container.SetEncounters(allEncounters);
            container.SetRewards(allRewards);

            for (int i = 0; i < container.Raids.Count; i++)
            {
                var raidz = container.Raids;
                int index = i;
                Raid raid = raidz[index];
                var encounter = container.Encounters[index];
                var param = encounter.GetParam();
                var blank = new PK9
                {
                    Species = encounter.Species,
                    Form = encounter.Form
                };
                Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
                var (spr, txt) = CalculateFromSeed(raid.Seed, blank);
                results.Add(txt);
                raidimages.Add(spr);
                var type = TypeSpriteUtil.GetTypeSpriteGem((byte)container.Raids[i].TeraType);
                teratype.Add(type!);
                var starcount = container.Raids[i].IsEvent ? container.Encounters[i].Stars : RaidExtensions.GetStarCount(container.Raids[i], container.Raids[i].Difficulty, StoryProgress, container.Raids[i].IsBlack);
                stars.Add(starcount);
            }
        }

        public async Task ReadEventRaids(ulong BaseBlockKeyPointer, RaidContainer container, CancellationToken token, bool force = false)
        {
            var prio_file = Path.Combine(Directory.GetCurrentDirectory(), "cache", "raid_priority_array");
            if (!force && File.Exists(prio_file))
            {
                (_, var version) = FlatbufferDumper.DumpDeliveryPriorities(File.ReadAllBytes(prio_file));
                var blk = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidPriorityLocation, "raid_priority_array.tmp", true, token).ConfigureAwait(false);
                (_, var v2) = FlatbufferDumper.DumpDeliveryPriorities(blk);
                if (version != v2)
                    force = true;

                var tmp_file = Path.Combine(Directory.GetCurrentDirectory(), "cache", "raid_priority_array.tmp");
                if (File.Exists(tmp_file))
                    File.Delete(tmp_file);

                if (v2 == 0) // raid reset
                    return;
            }

            var delivery_raid_prio = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidPriorityLocation, "raid_priority_array", force, token).ConfigureAwait(false);
            (var group_id, var priority) = FlatbufferDumper.DumpDeliveryPriorities(delivery_raid_prio);
            if (priority == 0)
                return;

            var delivery_raid_fbs = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidBinaryLocation, "raid_enemy_array", force, token).ConfigureAwait(false);
            var delivery_fixed_rewards = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidFixedRewardLocation, "fixed_reward_item_array", force, token).ConfigureAwait(false);
            var delivery_lottery_rewards = await ReadBlockDefault(BaseBlockKeyPointer, RaidCrawler.Core.Structures.Offsets.BCATRaidLotteryRewardLocation, "lottery_reward_item_array", force, token).ConfigureAwait(false);

            container.DistTeraRaids = TeraDistribution.GetAllEncounters(delivery_raid_fbs);
            container.MightTeraRaids = TeraMight.GetAllEncounters(delivery_raid_fbs);
            container.DeliveryRaidPriority = group_id;
            container.DeliveryRaidFixedRewards = FlatbufferDumper.DumpFixedRewards(delivery_fixed_rewards);
            container.DeliveryRaidLotteryRewards = FlatbufferDumper.DumpLotteryRewards(delivery_lottery_rewards);
        }

        public async Task<byte[]> ReadBlockDefault(ulong BaseBlockKeyPointer, uint key, string? cache, bool force, CancellationToken token)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "cache");
            Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, cache ?? "");
            if (force is false && cache is not null && File.Exists(path))
                return File.ReadAllBytes(path);

            var bin = await ReadSaveBlockObject(BaseBlockKeyPointer, key, token).ConfigureAwait(false);
            File.WriteAllBytes(path, bin);
            return bin;
        }

        public async Task<byte[]> ReadSaveBlockObject(ulong BaseBlockKeyPointer, uint key, CancellationToken token)
        {
            var header_ofs = await SearchSaveKeyRaid(BaseBlockKeyPointer, key, token).ConfigureAwait(false);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(header_ofs + 8, 8, token).ConfigureAwait(false);
            header_ofs = BitConverter.ToUInt64(data);

            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(header_ofs, 5, token).ConfigureAwait(false);
            header = DecryptBlock(key, header);

            var size = BitConverter.ToUInt32(header.AsSpan()[1..]);
            var obj = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(header_ofs, (int)size + 5, token).ConfigureAwait(false);
            return DecryptBlock(key, obj)[5..];
        }

        private List<string> raidimages = new();
        private List<string> results = new();
        private List<Image> teratype = new();
        private List<int> stars = new();
        private ulong RaidBlockOffsetP;
        private ulong RaidBlockOffsetK;
        private ulong RaidBlockOffsetB;
        private RaidContainer? container;
        private int StoryProgress;
        private int EventProgress;
        private readonly IReadOnlyList<uint> DifficultyFlags = new List<uint>() { 0xEC95D8EF, 0xA9428DFE, 0x9535F471, 0x6E7F8220 };

        private void Screenshot_Click(object sender, EventArgs e)
        {
            Rectangle bounds = Bounds;
            Bitmap bmp = new(this.Width, this.Height);
            DrawToBitmap(bmp, bounds);
            Bitmap CroppedImage = bmp.Clone(new(220, 170, bmp.Width - 220, bmp.Height - 170), bmp.PixelFormat);
            Clipboard.SetImage(CroppedImage);
            MessageBox.Show("Copied to clipboard!");
        }
    }
}
