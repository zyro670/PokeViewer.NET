using Newtonsoft.Json;
using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;
using PokeViewer.NET.Properties;
using SysBot.Base;
using System.Text;
using static PokeViewer.NET.RoutineExecutor;
using static SysBot.Base.SwitchButton;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokeViewer.NET.SubForms
{
    public partial class OutbreakViewSV : Form
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
        public List<OutbreakStash> OutbreakCache = [];
        public List<OutbreakStash> BCATOutbreakCache = [];
        private List<Image> MapSpritesP = [];
        private List<byte[]?> MapPOSP = [];
        private List<byte[]?> MapPOSObP = [];
        private List<string> MapCountP = [];
        private List<string> MapStringsP = [];
        private List<Image> MapSpritesObP = [];
        private List<string> MapCountObP = [];
        private List<string> MapStringsObP = [];
        private List<Image> MapSpritesK = [];
        private List<byte[]?> MapPOSK = [];
        private List<byte[]?> MapPOSObK = [];
        private List<string> MapCountK = [];
        private List<string> MapStringsK = [];
        private List<Image> MapSpritesObK = [];
        private List<string> MapCountObK = [];
        private List<string> MapStringsObK = [];
        private List<Image> MapSpritesB = [];
        private List<byte[]?> MapPOSB = [];
        private List<byte[]?> MapPOSObB = [];
        private List<string> MapCountB = [];
        private List<string> MapStringsB = [];
        private List<Image> MapSpritesObB = [];
        private List<string> MapCountObB = [];
        private List<string> MapStringsObB = [];
        private List<PK9> pkList = [];
        private Image PaldeaStored;
        private Image KitakamiStored;
        private Image BlueberryStored;
        private readonly string[] SpeciesList = null!;
        private readonly string[] FormsList = null!;
        private readonly string[] TypesList = null!;
        private readonly string[] GenderList = null!;

        protected ViewerOffsets Offsets { get; } = new();
        public OutbreakViewSV(ViewerExecutor executor, (Color, Color) color, bool OutbreakFormOpen)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            MapGroup.SelectedIndex = Settings.Default.MapSetting;
            SpeciesBox.DataSource = Enum.GetValues(typeof(Species)).Cast<Species>().Where(z => z != Species.MAX_COUNT).ToArray();
            LocationCombo.DataSource = Enum.GetValues(typeof(MapSource)).Cast<MapSource>().Where(z => z != MapSource.MAX_COUNT).ToArray();
            var path = "refs\\outbreakfilters.txt";
            if (File.Exists(path))
                LoadFilters(path);

            LoadOutbreakCache();
            SpeciesList = GameInfo.GetStrings(1).specieslist;
            FormsList = GameInfo.GetStrings(1).forms;
            TypesList = GameInfo.GetStrings(1).types;
            GenderList = [.. GameInfo.GenderSymbolUnicode];
            PaldeaStored = PaldeaMap.Image;
            KitakamiStored = KitakamiMap.Image;
            BlueberryStored = BlueberryMap.Image;
        }

        private enum MapSource
        {
            Paldea = 0,
            Kitakami = 1,
            Blueberry = 2,
            MAX_COUNT = 3,
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
            ViewList.BackColor = color.Item1;
            ViewList.ForeColor = color.Item2;
            ClearList.BackColor = color.Item1;
            ClearList.ForeColor = color.Item2;
            AddSpecies.BackColor = color.Item1;
            AddSpecies.ForeColor = color.Item2;
            RemoveSpecies.BackColor = color.Item1;
            RemoveSpecies.ForeColor = color.Item2;
            SearchListGroup.BackColor = color.Item1;
            SearchListGroup.ForeColor = color.Item2;
            OutbreakIcon.BackColor = color.Item1;
            OutbreakIcon.ForeColor = color.Item2;
            SpeciesBox.BackColor = color.Item1;
            SpeciesBox.ForeColor = color.Item2;
            FormCombo.BackColor = color.Item1;
            FormCombo.ForeColor = color.Item2;
            MapGroup.BackColor = color.Item1;
            MapGroup.ForeColor = color.Item2;
            PaldeaGroup.BackColor = color.Item1;
            PaldeaGroup.ForeColor = color.Item2;
            KitakamiGroup.BackColor = color.Item1;
            KitakamiGroup.ForeColor = color.Item2;
            BlueberryGroup.BackColor = color.Item1;
            BlueberryGroup.ForeColor = color.Item2;
            PaldeaEventGroup.BackColor = color.Item1;
            PaldeaEventGroup.ForeColor = color.Item2;
            KitakamiEventGroup.BackColor = color.Item1;
            KitakamiEventGroup.ForeColor = color.Item2;
            BlueberryEventGroup.BackColor = color.Item1;
            BlueberryEventGroup.ForeColor = color.Item2;
            UptimeLabel.BackColor = color.Item1;
            UptimeLabel.ForeColor = color.Item2;
            StatusLabel.BackColor = color.Item1;
            StatusLabel.ForeColor = color.Item2;
            progressBar1.BackColor = color.Item1;
            progressBar1.ForeColor = color.Item2;
            NextBtn.BackColor = color.Item1;
            NextBtn.ForeColor = color.Item2;
            PrevBtn.BackColor = color.Item1;
            PrevBtn.ForeColor = color.Item2;
            LocationCombo.BackColor = color.Item1;
            LocationCombo.ForeColor = color.Item2;
            Apply0To64.BackColor = color.Item1;
            Apply0To64.ForeColor = color.Item2;
            HardStopOutbreak.BackColor = color.Item1;
            HardStopOutbreak.ForeColor = color.Item2;
            ResetGameLabel.BackColor = color.Item1;
            ResetGameLabel.ForeColor = color.Item2;
            ResetGameNumeric.BackColor = color.Item1;
            ResetGameNumeric.ForeColor = color.Item2;
        }

        public void LoadOutbreakCache()
        {
            for (int i = 0; i < 18; i++)
                OutbreakCache.Add(new());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LocationCombo.SelectedIndex)
            {
                case 0: PaldeaMap.BringToFront(); PaldeaGroup.BringToFront(); PaldeaEventGroup.BringToFront(); break;
                case 1: KitakamiMap.BringToFront(); KitakamiGroup.BringToFront(); KitakamiEventGroup.BringToFront(); break;
                case 2: BlueberryMap.BringToFront(); BlueberryGroup.BringToFront(); BlueberryEventGroup.BringToFront(); break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (LocationCombo.SelectedIndex)
            {
                case 0: LocationCombo.SelectedIndex = 1; break;
                case 1: LocationCombo.SelectedIndex = 2; break;
                case 2: LocationCombo.SelectedIndex = 0; break;
            }
            switch (LocationCombo.SelectedIndex)
            {
                case 0: PaldeaMap.BringToFront(); PaldeaGroup.BringToFront(); PaldeaEventGroup.BringToFront(); break;
                case 1: KitakamiMap.BringToFront(); KitakamiGroup.BringToFront(); KitakamiEventGroup.BringToFront(); break;
                case 2: BlueberryMap.BringToFront(); BlueberryGroup.BringToFront(); BlueberryEventGroup.BringToFront(); break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (LocationCombo.SelectedIndex)
            {
                case 0: LocationCombo.SelectedIndex = 2; break;
                case 1: LocationCombo.SelectedIndex = 0; break;
                case 2: LocationCombo.SelectedIndex = 1; break;
            }
            switch (LocationCombo.SelectedIndex)
            {
                case 0: PaldeaMap.BringToFront(); PaldeaGroup.BringToFront(); PaldeaEventGroup.BringToFront(); break;
                case 1: KitakamiMap.BringToFront(); KitakamiGroup.BringToFront(); KitakamiEventGroup.BringToFront(); break;
                case 2: BlueberryMap.BringToFront(); BlueberryGroup.BringToFront(); BlueberryEventGroup.BringToFront(); break;
            }
        }

        private void MapGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            OutbreakCache = [];
            LoadOutbreakCache();
            CountCacheP = 0;
            CountCacheK = 0;
            CountCacheB = 0;
            CountCacheBCP = 0;
            CountCacheBCK = 0;
            Settings.Default.MapSetting = MapGroup.SelectedIndex;
            Settings.Default.Save();
        }

        private async void OutbreakScan_Click(object sender, EventArgs e)
        {
            StartTime = DateTime.Now;
            UptimeOnLoad(sender, e);
            await SearchForOutbreak(CancellationToken.None).ConfigureAwait(false);
        }
        private async Task SearchForOutbreak(CancellationToken token)
        {
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);

            List<Image> ImgP = [];
            List<Image> ImgK = [];
            List<Image> ImgB = [];
            List<Image> ImgObP = [];
            List<Image> ImgObK = [];
            List<Image> ImgObB = [];
            List<byte[]?> POSlistP = [];
            List<byte[]?> POSlistK = [];
            List<byte[]?> POSlistB = [];
            List<byte[]?> POSlistObP = [];
            List<byte[]?> POSlistObK = [];
            List<byte[]?> POSlistObB = [];
            List<uint> kolistP = [];
            List<uint> kolistK = [];
            List<uint> kolistB = [];
            List<uint> kolistObP = [];
            List<uint> kolistObK = [];
            List<uint> kolistObB = [];
            List<uint> totallistP = [];
            List<uint> totallistK = [];
            List<uint> totallistB = [];
            List<uint> totallistObP = [];
            List<uint> totallistObK = [];
            List<uint> totallistObB = [];
            List<string> stringsP = [];
            List<string> stringsK = [];
            List<string> stringsB = [];
            List<string> stringsObP = [];
            List<string> stringsObK = [];
            List<string> stringsObB = [];
            List<string> CountlistP = [];
            List<string> CountlistK = [];
            List<string> CountlistB = [];
            List<string> CountlistObP = [];
            List<string> CountlistObK = [];
            List<string> CountlistObB = [];
            List<PK9> monsP = [];
            List<PK9> monsK = [];
            List<PK9> monsB = [];
            List<PK9> monsObP = [];
            List<PK9> monsObK = [];
            List<PK9> monsObB = [];
            MapSpritesP = [];
            MapPOSP = [];
            MapCountP = [];
            MapStringsP = [];
            MapSpritesK = [];
            MapCountK = [];
            MapStringsK = [];
            MapPOSK = [];
            MapSpritesB = [];
            MapPOSB = [];
            MapCountB = [];
            MapStringsB = [];

            DisableAssets();
            int dayskip = 0;
            int resetcount = 0;
            int OutbreaktotalP = 0;
            int OutbreaktotalK = 0;
            int OutbreaktotalB = 0;
            int OutbreaktotalBCP = 0;
            int OutbreaktotalBCK = 0;
            int OutbreaktotalBCB = 0;

            while (!token.IsCancellationRequested)
            {
                dayskip++;
                resetcount++;

                if (dayskip % 2 == 0)
                    await ResetTime(token).ConfigureAwait(false);

                if (resetcount == ResetGameNumeric.Value)
                {
                    OutbreakCache = [];
                    LoadOutbreakCache();
                    CountCacheP = 0;
                    CountCacheK = 0;
                    CountCacheB = 0;
                    CountCacheBCP = 0;
                    CountCacheBCK = 0;
                    CountCacheBCB = 0;
                    resetcount = 0;
                    await ResetGame(token, false).ConfigureAwait(false);
                }

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
                        OutbreakCache = [];
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
                        OutbreakCache = [];
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
                        OutbreakCache = [];
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
                                BCATOutbreakCache = [];
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
                                BCATOutbreakCache = [];
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
                                BCATOutbreakCache = [];
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
                                if (i > OutbreaktotalBCB + 20)
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
                PaldeaGroup.Text = $"Paldea Outbreaks: {OutbreaktotalP}";
                PaldeaEventGroup.Text = $"Paldea Outbreaks: {OutbreaktotalBCP}";
                KitakamiGroup.Text = $"Kitakami Outbreaks: {OutbreaktotalK}";
                KitakamiEventGroup.Text = $"Event Outbreaks: {OutbreaktotalBCK}";
                BlueberryGroup.Text = $"Blueberry Outbreaks: {OutbreaktotalB}";
                BlueberryEventGroup.Text = $"Event Outbreaks: {OutbreaktotalBCB}";

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

                if (MapGroup.SelectedIndex is 0 or 1) EnableAssets(PaldeaMap, 0, MapSpritesP, MapPOSP, MapStringsP, MapCountP, MapSpritesObP, MapStringsObP, MapCountObP, MapPOSObP);
                if (MapGroup.SelectedIndex is 0 or 2) EnableAssets(KitakamiMap, 1, MapSpritesK, MapPOSK, MapStringsK, MapCountK, MapSpritesObK, MapStringsObK, MapCountObK, MapPOSObK);
                if (MapGroup.SelectedIndex is 0 or 3) EnableAssets(BlueberryMap, 2, MapSpritesB, MapPOSB, MapStringsB, MapCountB, MapSpritesObB, MapStringsObB, MapCountObB, MapPOSObB);

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
                        PaldeaMap.BringToFront(); PaldeaGroup.BringToFront(); PaldeaEventGroup.BringToFront();


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
                        KitakamiMap.BringToFront(); KitakamiGroup.BringToFront(); KitakamiEventGroup.BringToFront();

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
                        BlueberryMap.BringToFront(); BlueberryGroup.BringToFront(); BlueberryEventGroup.BringToFront();

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
                        PaldeaMap.BringToFront(); PaldeaGroup.BringToFront(); PaldeaEventGroup.BringToFront();

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
                        KitakamiMap.BringToFront(); KitakamiGroup.BringToFront(); KitakamiEventGroup.BringToFront();

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
                        BlueberryMap.BringToFront(); BlueberryGroup.BringToFront(); BlueberryEventGroup.BringToFront();

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
                    ImgP = [];
                    ImgObP = [];
                    POSlistP = [];
                    POSlistObP = [];
                    kolistP = [];
                    kolistObP = [];
                    totallistP = [];
                    totallistObP = [];
                    stringsP = [];
                    stringsObP = [];
                    monsP = [];
                    monsObP = [];
                    CountlistP = [];
                    CountlistObP = [];

                    ImgK = [];
                    ImgObK = [];
                    POSlistK = [];
                    POSlistObK = [];
                    kolistK = [];
                    kolistObK = [];
                    totallistK = [];
                    totallistObK = [];
                    stringsK = [];
                    stringsObK = [];
                    monsK = [];
                    monsObK = [];
                    CountlistK = [];
                    CountlistObK = [];

                    ImgB = [];
                    ImgObB = [];
                    POSlistB = [];
                    POSlistObB = [];
                    kolistB = [];
                    kolistObB = [];
                    totallistB = [];
                    totallistObB = [];
                    stringsB = [];
                    stringsObB = [];
                    monsB = [];
                    monsObB = [];
                    CountlistB = [];
                    CountlistObB = [];

                    MapSpritesP = [];
                    MapPOSP = [];
                    MapCountP = [];
                    MapStringsP = [];
                    MapSpritesK = [];
                    MapCountK = [];
                    MapStringsK = [];
                    MapPOSK = [];
                    MapSpritesB = [];
                    MapPOSB = [];
                    MapCountB = [];
                    MapStringsB = [];

                    MapSpritesObP = [];
                    MapPOSObP = [];
                    MapCountObP = [];
                    MapStringsObP = [];
                    MapSpritesObK = [];
                    MapCountObK = [];
                    MapStringsObK = [];
                    MapPOSObK = [];
                    MapSpritesObB = [];
                    MapPOSObB = [];
                    MapCountObB = [];
                    MapStringsObB = [];
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
            var (validOutbreaksB, _) = await ReadEncryptedBlockByte(Blocks.KMassOutbreakTotalBlueberry, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCP, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01MainTotalSpawns, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCK, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01DLC1TotalSpawns, 0, token).ConfigureAwait(false);
            var (validOutbreaksBCB, _) = await ReadEncryptedBlockByte(Blocks.KOutbreakBC01DLC2TotalSpawns, 0, token).ConfigureAwait(false);

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
                if (ScanForEventOutbreak.Checked)
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
            }
            if (MapGroup.SelectedIndex is 0 or 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    StatusLabel.Text = $"KOing Kitakami: {25 * i}%";
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
                if (ScanForEventOutbreak.Checked)
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
            }
            if (MapGroup.SelectedIndex is 0 or 3)
            {
                for (int i = 0; i < 5; i++)
                {
                    StatusLabel.Text = $"KOing Blueberry: {20 * i}%";
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
                if (ScanForEventOutbreak.Checked)
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
            }
            StatusLabel.Text = "Status:";
            OutbreakScan.Enabled = true;
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

        public async Task TimeSkipBwd(CancellationToken token) => await Executor.Connection.SendAsync(SwitchCommand.TimeSkipBack(true), token).ConfigureAwait(false);
        public async Task ResetTime(CancellationToken token) => await Executor.Connection.SendAsync(SwitchCommand.ResetTime(true), token).ConfigureAwait(false);

        public async Task ResetTimeNTP(CancellationToken token) => await Executor.Connection.SendAsync(ResetTimeNTP(true), token).ConfigureAwait(false);

        private static readonly Encoding Encoder = Encoding.ASCII;

        private static byte[] Encode(string command, bool crlf = true)
        {
            if (crlf)
                command += "\r\n";
            return Encoder.GetBytes(command);
        }

        public static byte[] ResetTimeNTP(bool crlf = true) => Encode("resetTimeNTP", crlf);

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

        public async Task<(byte, ulong)> ReadEncryptedBlockByte(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (header[1], address);
        }

        public async Task<(byte[], ulong)> ReadEncryptedBlockHeader(DataBlock block, ulong init, CancellationToken token)
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

        public async Task<(byte[]?, ulong)> ReadEncryptedBlockArray(DataBlock block, ulong init, CancellationToken token)
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

        public async Task<(uint, ulong)> ReadEncryptedBlockUint(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadUInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        public async Task<(int, ulong)> ReadEncryptedBlockInt32(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        public async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data);
            return res[0] == 2;
        }

        public async Task<byte[]> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return data[6..];
        }

        public async Task<byte[]?> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
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

        public async Task<bool> WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            await Executor.SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> WriteEncryptedBlockSafe(DataBlock block, object? toExpect, object toWrite, CancellationToken token)
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

        public async Task<bool> WriteEncryptedBlockUint(DataBlock block, uint valueToExpect, uint valueToInject, CancellationToken token)
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

        public async Task<bool> WriteEncryptedBlockInt32(DataBlock block, int valueToExpect, int valueToInject, CancellationToken token)
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

        public async Task<bool> WriteEncryptedBlockByte(DataBlock block, byte valueToExpect, byte valueToInject, CancellationToken token)
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

        public async Task<bool> WriteEncryptedBlockArray(DataBlock block, byte[] arrayToExpect, byte[] arrayToInject, CancellationToken token)
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

        public async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
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

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.Connection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

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
            ScanForEventOutbreak.Enabled = true;
        }

        private void DisableAssets()
        {
            OutbreakScan.Enabled = false;
            OutbreakSearch.Enabled = false;
            Apply0To64.Enabled = false;
            ScanForEventOutbreak.Enabled = false;
        }

        private void ViewList_Click(object sender, EventArgs e)
        {
            string r = string.Empty;
            foreach (var p in pkList.ToList())
            {
                r += $"{(Species)p.Species} - {p.Form}{Environment.NewLine}";
            }
            MessageBox.Show(r, "Search List");
        }

        private void SpeciesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormCombo.Items.Clear();
            FormCombo.Text = string.Empty;
            var formlist = FormConverter.GetFormList((ushort)(Species)SpeciesBox.SelectedIndex, TypesList, FormsList, GenderList, EntityContext.Gen9);
            if ((Species)SpeciesBox.SelectedIndex == Species.Minior)
                formlist = formlist.Take((formlist.Length + 1) / 2).ToArray();

            if (formlist.Length == 0 || (formlist.Length == 1 && formlist[0].Equals("")))
                FormCombo.Visible = false;
            else
            {
                FormCombo.Items.AddRange(formlist);
                FormCombo.Visible = true;
            }
        }

        private void AddSpecies_Click(object sender, EventArgs e)
        {
            PK9 pk = new()
            {
                Species = (ushort)(Species)SpeciesBox.SelectedIndex,
                Form = (byte)FormCombo.SelectedIndex,
            };
            if (!FormCombo.Visible)
                pk.Form = 0;
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
                Form = (byte)FormCombo.SelectedIndex,
            };
            if (!FormCombo.Visible)
                pk.Form = 0;
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
            pkList = [];
            MessageBox.Show("Cleared all filters", "Search List");
        }

        public Image? GeneratePaldeaMap(List<Image> small, List<byte[]?> pos, List<Image> eve, List<byte[]?> pos2)
        {
            Image result = PaldeaStored;
            for (int i = 0; i < small.Count; i++)
            {
                var newsmol = (Image)new Bitmap(small[i], new Size(125, 125));
                SpriteUtil.GetSpriteGlow(newsmol, 0xFF, 0xFF, 0xFF, out var glow, true);
                newsmol = ImageUtil.LayerImage(newsmol, ImageUtil.GetBitmap(glow, newsmol.Width, newsmol.Height, newsmol.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos[i]!, 0);
                float flty = BitConverter.ToSingle(pos[i]!, 8);
                try
                {
                    x = (fltx + 2.072021484) * 1024 / 5000;
                    y = (flty + 5255.240018) * 1024 / 5000;
                    result = ImageUtil.LayerImage(result, newsmol, (int)x - 50, (int)y - 10);
                }
                catch { return null; }
            }
            for (int i = 0; i < eve.Count; i++)
            {
                var neweve = (Image)new Bitmap(eve[i], new Size(125, 125));
                SpriteUtil.GetSpriteGlow(neweve, 0xFF, 0xFF, 0xFF, out var glow, true);
                neweve = ImageUtil.LayerImage(neweve, ImageUtil.GetBitmap(glow, neweve.Width, neweve.Height, neweve.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos2[i]!, 0);
                float flty = BitConverter.ToSingle(pos2[i]!, 8);
                try
                {
                    x = (fltx + 2.072021484) * 1024 / 5000;
                    y = (flty + 5255.240018) * 1024 / 5000;
                    result = ImageUtil.LayerImage(result, neweve, (int)x - 50, (int)y - 10);
                }
                catch { return null; }
            }
            return result;
        }

        public Image? GenerateKitakamiMap(List<Image> small, List<byte[]?> pos, List<Image> eve, List<byte[]?> pos2)
        {
            Image result = KitakamiStored;
            for (int i = 0; i < small.Count; i++)
            {
                var newsmol = (Image)new Bitmap(small[i], new Size(65, 65));
                SpriteUtil.GetSpriteGlow(newsmol, 0xFF, 0xFF, 0xFF, out var glow, true);
                newsmol = ImageUtil.LayerImage(newsmol, ImageUtil.GetBitmap(glow, newsmol.Width, newsmol.Height, newsmol.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos[i]!, 0);
                float flty = BitConverter.ToSingle(pos[i]!, 8);
                try
                {
                    x = ((fltx * 2.766970605475146) + -248.08352352566726) * 512 / 5000;
                    y = ((flty * 2.5700782642623805) + 5070.808599816581) * 512 / 5000;
                    result = ImageUtil.LayerImage(result, newsmol, (int)x - 50, (int)y - 50);
                }
                catch { return null; }
            }
            for (int i = 0; i < eve.Count; i++)
            {
                var neweve = (Image)new Bitmap(eve[i], new Size(65, 65));
                SpriteUtil.GetSpriteGlow(neweve, 0xFF, 0xFF, 0xFF, out var glow, true);
                neweve = ImageUtil.LayerImage(neweve, ImageUtil.GetBitmap(glow, neweve.Width, neweve.Height, neweve.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos2[i]!, 0);
                float flty = BitConverter.ToSingle(pos2[i]!, 8);
                try
                {
                    x = ((fltx * 2.766970605475146) + -248.08352352566726) * 512 / 5000;
                    y = ((flty * 2.5700782642623805) + 5070.808599816581) * 512 / 5000;
                    result = ImageUtil.LayerImage(result, neweve, (int)x - 50, (int)y - 50);
                }
                catch { return null; }
            }
            return result;
        }

        public Image? GenerateBlueberryMap(List<Image> small, List<byte[]?> pos, List<Image> eve, List<byte[]?> pos2)
        {
            Image result = BlueberryStored;
            for (int i = 0; i < small.Count; i++)
            {
                var newsmol = (Image)new Bitmap(small[i], new Size(65, 65));
                SpriteUtil.GetSpriteGlow(newsmol, 0xFF, 0xFF, 0xFF, out var glow, true);
                newsmol = ImageUtil.LayerImage(newsmol, ImageUtil.GetBitmap(glow, newsmol.Width, newsmol.Height, newsmol.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos[i]!, 0);
                float flty = BitConverter.ToSingle(pos[i]!, 8);
                try
                {
                    x = ((fltx * 2.766970605475146) + -248.08352352566726) * 512 / 5000;
                    y = ((flty * 2.5700782642623805) + 5070.808599816581) * 512 / 5000;
                    result = ImageUtil.LayerImage(result, newsmol, (int)x - 50, (int)y - 50);
                }
                catch { return null; }
            }
            for (int i = 0; i < eve.Count; i++)
            {
                var neweve = (Image)new Bitmap(eve[i], new Size(65, 65));
                SpriteUtil.GetSpriteGlow(neweve, 0xFF, 0xFF, 0xFF, out var glow, true);
                neweve = ImageUtil.LayerImage(neweve, ImageUtil.GetBitmap(glow, neweve.Width, neweve.Height, neweve.PixelFormat), 0, 0);
                double x, y;
                float fltx = BitConverter.ToSingle(pos2[i]!, 0);
                float flty = BitConverter.ToSingle(pos2[i]!, 8);
                try
                {
                    x = ((fltx * 2.766970605475146) + -248.08352352566726) * 512 / 5000;
                    y = ((flty * 2.5700782642623805) + 5070.808599816581) * 512 / 5000;
                    result = ImageUtil.LayerImage(result, neweve, (int)x - 50, (int)y - 50);
                }
                catch { return null; }
            }
            return result;
        }

        private void EnableAssets(PictureBox mainmap, int map, List<Image> sprite, List<byte[]?> pos, List<string> count, List<string> strings, List<Image> spriteob, List<string> countob, List<string> stringsob, List<byte[]?> pos2)
        {
            HideAssets();
            switch (map)
            {
                case 0:
                    mainmap.Image = GeneratePaldeaMap(sprite, pos, spriteob, pos2);
                    for (int i = 0; i < sprite.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                OBSprite1.Image = sprite[i]; Ob1Results.Text = count[i]; ObCount1.Text = strings[i];
                                OBSprite1.Visible = true; Ob1Results.Visible = true; ObCount1.Visible = true; break;
                            case 1:
                                OBSprite2.Image = sprite[i]; Ob2Results.Text = count[i]; ObCount2.Text = strings[i];
                                OBSprite2.Visible = true; Ob2Results.Visible = true; ObCount2.Visible = true; break;
                            case 2:
                                OBSprite3.Image = sprite[i]; Ob3Results.Text = count[i]; ObCount3.Text = strings[i];
                                OBSprite3.Visible = true; Ob3Results.Visible = true; ObCount3.Visible = true; break;
                            case 3:
                                OBSprite4.Image = sprite[i]; Ob4Results.Text = count[i]; ObCount4.Text = strings[i];
                                OBSprite4.Visible = true; Ob4Results.Visible = true; ObCount4.Visible = true; break;
                            case 4:
                                OBSprite5.Image = sprite[i]; Ob5Results.Text = count[i]; ObCount5.Text = strings[i];
                                OBSprite5.Visible = true; Ob5Results.Visible = true; ObCount5.Visible = true; break;
                            case 5:
                                OBSprite6.Image = sprite[i]; Ob6Results.Text = count[i]; ObCount6.Text = strings[i];
                                OBSprite6.Visible = true; Ob6Results.Visible = true; ObCount6.Visible = true; break;
                            case 6:
                                OBSprite7.Image = sprite[i]; Ob7Results.Text = count[i]; ObCount7.Text = strings[i];
                                OBSprite7.Visible = true; Ob7Results.Visible = true; ObCount7.Visible = true; break;
                            case 7:
                                OBSprite8.Image = sprite[i]; Ob8Results.Text = count[i]; ObCount8.Text = strings[i];
                                OBSprite8.Visible = true; Ob8Results.Visible = true; ObCount8.Visible = true; break;
                        }
                    }
                    for (int i = 0; i < spriteob.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                PaldeaEventSpr1.Image = spriteob[i]; PaldeaEventName1.Text = countob[i]; PaldeaEventCount1.Text = stringsob[i];
                                PaldeaEventSpr1.Visible = true; PaldeaEventName1.Visible = true; PaldeaEventCount1.Visible = true; break;
                            case 1:
                                PaldeaEventSpr2.Image = spriteob[i]; PaldeaEventName2.Text = countob[i]; PaldeaEventCount2.Text = stringsob[i];
                                PaldeaEventSpr2.Visible = true; PaldeaEventName2.Visible = true; PaldeaEventCount2.Visible = true; break;
                            case 2:
                                PaldeaEventSpr3.Image = spriteob[i]; PaldeaEventName3.Text = countob[i]; PaldeaEventCount3.Text = stringsob[i];
                                PaldeaEventSpr3.Visible = true; PaldeaEventName3.Visible = true; PaldeaEventCount3.Visible = true; break;
                            case 3:
                                PaldeaEventSpr4.Image = spriteob[i]; PaldeaEventName4.Text = countob[i]; PaldeaEventCount4.Text = stringsob[i];
                                PaldeaEventSpr4.Visible = true; PaldeaEventName4.Visible = true; PaldeaEventCount4.Visible = true; break;
                            case 4:
                                PaldeaEventSpr5.Image = spriteob[i]; PaldeaEventName5.Text = countob[i]; PaldeaEventCount5.Text = stringsob[i];
                                PaldeaEventSpr5.Visible = true; PaldeaEventName5.Visible = true; PaldeaEventCount5.Visible = true; break;
                        }
                    }
                    break;
                case 1:
                    mainmap.Image = GenerateKitakamiMap(sprite, pos, spriteob, pos2);
                    for (int i = 0; i < sprite.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                KSpr1.Image = sprite[i]; KitaName1.Text = count[i]; KitaCount1.Text = strings[i];
                                KSpr1.Visible = true; KitaName1.Visible = true; KitaCount1.Visible = true; break;
                            case 1:
                                KSpr2.Image = sprite[i]; KitaName2.Text = count[i]; KitaCount2.Text = strings[i];
                                KSpr2.Visible = true; KitaName2.Visible = true; KitaCount2.Visible = true; break;
                            case 2:
                                KSpr3.Image = sprite[i]; KitaName3.Text = count[i]; KitaCount3.Text = strings[i];
                                KSpr3.Visible = true; KitaName3.Visible = true; KitaCount3.Visible = true; break;
                            case 3:
                                KSpr4.Image = sprite[i]; KitaName4.Text = count[i]; KitaCount4.Text = strings[i];
                                KSpr4.Visible = true; KitaName4.Visible = true; KitaCount4.Visible = true; break;
                        }
                    }
                    for (int i = 0; i < spriteob.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                KSprEvent1.Image = spriteob[i]; Event1Results.Text = countob[i]; EventCount1.Text = stringsob[i];
                                KSprEvent1.Visible = true; Event1Results.Visible = true; EventCount1.Visible = true; break;
                            case 1:
                                KSprEvent2.Image = spriteob[i]; Event2Results.Text = countob[i]; EventCount2.Text = stringsob[i];
                                KSprEvent2.Visible = true; Event2Results.Visible = true; EventCount2.Visible = true; break;
                            case 2:
                                KSprEvent3.Image = spriteob[i]; Event3Results.Text = countob[i]; EventCount3.Text = stringsob[i];
                                KSprEvent3.Visible = true; Event3Results.Visible = true; EventCount3.Visible = true; break;
                            case 3:
                                KSprEvent4.Image = spriteob[i]; Event4Results.Text = countob[i]; EventCount4.Text = stringsob[i];
                                KSprEvent4.Visible = true; Event4Results.Visible = true; EventCount4.Visible = true; break;
                            case 4:
                                KSprEvent5.Image = spriteob[i]; Event5Results.Text = countob[i]; EventCount5.Text = stringsob[i];
                                KSprEvent5.Visible = true; Event5Results.Visible = true; EventCount5.Visible = true; break;
                        }
                    }
                    break;
                case 2:
                    mainmap.Image = GenerateBlueberryMap(sprite, pos, spriteob, pos2);
                    for (int i = 0; i < sprite.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                BSpr1.Image = sprite[i]; BlueName1.Text = count[i]; BlueCount1.Text = strings[i];
                                BSpr1.Visible = true; BlueName1.Visible = true; BlueCount1.Visible = true; break;
                            case 1:
                                BSpr2.Image = sprite[i]; BlueName2.Text = count[i]; BlueCount2.Text = strings[i];
                                BSpr2.Visible = true; BlueName2.Visible = true; BlueCount2.Visible = true; break;
                            case 2:
                                BSpr3.Image = sprite[i]; BlueName3.Text = count[i]; BlueCount3.Text = strings[i];
                                BSpr3.Visible = true; BlueName3.Visible = true; BlueCount3.Visible = true; break;
                            case 3:
                                BSpr4.Image = sprite[i]; BlueName4.Text = count[i]; BlueCount4.Text = strings[i];
                                BSpr4.Visible = true; BlueName4.Visible = true; BlueCount4.Visible = true; break;
                            case 4:
                                BSpr5.Image = sprite[i]; BlueName5.Text = count[i]; BlueCount5.Text = strings[i];
                                BSpr5.Visible = true; BlueName5.Visible = true; BlueCount5.Visible = true; break;
                        }
                    }
                    for (int i = 0; i < spriteob.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                BlueberrySpr1.Image = spriteob[i]; BlueberryName1.Text = countob[i]; BlueberryEventCount1.Text = stringsob[i];
                                BlueberrySpr1.Visible = true; BlueberryName1.Visible = true; BlueberryEventCount1.Visible = true; break;
                            case 1:
                                BlueberrySpr2.Image = spriteob[i]; BlueberryName2.Text = countob[i]; BlueberryEventCount2.Text = stringsob[i];
                                BlueberrySpr2.Visible = true; BlueberryName2.Visible = true; BlueberryEventCount2.Visible = true; break;
                            case 2:
                                BlueberrySpr3.Image = spriteob[i]; BlueberryName3.Text = countob[i]; BlueberryEventCount3.Text = stringsob[i];
                                BlueberrySpr3.Visible = true; BlueberryName3.Visible = true; BlueberryEventCount3.Visible = true; break;
                            case 3:
                                BlueberrySpr4.Image = spriteob[i]; BlueberryName4.Text = countob[i]; BlueberryEventCount4.Text = stringsob[i];
                                BlueberrySpr4.Visible = true; BlueberryName4.Visible = true; BlueberryEventCount4.Visible = true; break;
                            case 4:
                                BlueberrySpr5.Image = spriteob[i]; BlueberryName5.Text = countob[i]; BlueberryEventCount5.Text = stringsob[i];
                                BlueberrySpr5.Visible = true; BlueberryName5.Visible = true; BlueberryEventCount5.Visible = true; break;
                        }
                    }
                    break;
            }
        }

        private void HideAssets()
        {
            OBSprite1.Visible = false; Ob1Results.Visible = false; ObCount1.Visible = false;
            OBSprite2.Visible = false; Ob2Results.Visible = false; ObCount2.Visible = false;
            OBSprite3.Visible = false; Ob3Results.Visible = false; ObCount3.Visible = false;
            OBSprite4.Visible = false; Ob4Results.Visible = false; ObCount4.Visible = false;
            OBSprite5.Visible = false; Ob5Results.Visible = false; ObCount5.Visible = false;
            OBSprite6.Visible = false; Ob6Results.Visible = false; ObCount6.Visible = false;
            OBSprite7.Visible = false; Ob7Results.Visible = false; ObCount7.Visible = false;
            OBSprite8.Visible = false; Ob8Results.Visible = false; ObCount8.Visible = false;
            PaldeaEventSpr1.Visible = false; PaldeaEventName1.Visible = false; PaldeaEventCount1.Visible = false;
            PaldeaEventSpr2.Visible = false; PaldeaEventName2.Visible = false; PaldeaEventCount2.Visible = false;
            PaldeaEventSpr3.Visible = false; PaldeaEventName3.Visible = false; PaldeaEventCount3.Visible = false;
            PaldeaEventSpr4.Visible = false; PaldeaEventName4.Visible = false; PaldeaEventCount4.Visible = false;
            PaldeaEventSpr5.Visible = false; PaldeaEventName5.Visible = false; PaldeaEventCount5.Visible = false;
            KSpr1.Visible = false; KitaName1.Visible = false; KitaCount1.Visible = false;
            KSpr2.Visible = false; KitaName2.Visible = false; KitaCount2.Visible = false;
            KSpr3.Visible = false; KitaName3.Visible = false; KitaCount3.Visible = false;
            KSpr4.Visible = false; KitaName4.Visible = false; KitaCount4.Visible = false;
            KSprEvent1.Visible = false; Event1Results.Visible = false; EventCount1.Visible = false;
            KSprEvent2.Visible = false; Event2Results.Visible = false; EventCount2.Visible = false;
            KSprEvent3.Visible = false; Event3Results.Visible = false; EventCount3.Visible = false;
            KSprEvent4.Visible = false; Event4Results.Visible = false; EventCount4.Visible = false;
            KSprEvent5.Visible = false; Event5Results.Visible = false; EventCount5.Visible = false;
            BSpr1.Visible = false; BlueName1.Visible = false; BlueCount1.Visible = false;
            BSpr2.Visible = false; BlueName2.Visible = false; BlueCount2.Visible = false;
            BSpr3.Visible = false; BlueName3.Visible = false; BlueCount3.Visible = false;
            BSpr4.Visible = false; BlueName4.Visible = false; BlueCount4.Visible = false;
            BSpr5.Visible = false; BlueName5.Visible = false; BlueCount5.Visible = false;
            BlueberrySpr1.Visible = false; BlueberryName1.Visible = false; BlueberryEventCount1.Visible = false;
            BlueberrySpr2.Visible = false; BlueberryName2.Visible = false; BlueberryEventCount2.Visible = false;
            BlueberrySpr3.Visible = false; BlueberryName3.Visible = false; BlueberryEventCount3.Visible = false;
            BlueberrySpr4.Visible = false; BlueberryName4.Visible = false; BlueberryEventCount4.Visible = false;
            BlueberrySpr5.Visible = false; BlueberryName5.Visible = false; BlueberryEventCount5.Visible = false;
        }

        private void OutbreakView_Closed(object sender, FormClosedEventArgs e)
        {
            MiscViewSV.OutbreakFormOpen = false;
        }
    }
}
