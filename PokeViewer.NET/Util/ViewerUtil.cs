using PKHeX.Core;
using SysBot.Base;

namespace PokeViewer.NET
{
    public partial class ViewerUtil
    {
        public const string HOMEID = "010015F008C54000";
        public const string VioletID = "01008F6008C5E000";
        public const string ScarletID = "0100A3D008C5C000";
        public const string LegendsArceusID = "01001F5010DFA000";
        public const string ShiningPearlID = "010018E011D92000";
        public const string BrilliantDiamondID = "0100000011D90000";
        public const string SwordID = "0100ABF008968000";
        public const string ShieldID = "01008DB008C2C000";
        public const string EeveeID = "0100187003A36000";
        public const string PikachuID = "010003F003A34000";

        delegate void ChangeButtonStateCallback(Button sender, bool State);
        delegate void TextboxSetTextCallback(TextBox sender, string Text);

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

            string TIDFormatted = pk.Generation >= 7 ? $"{pk.TrainerTID7:000000}" : $"{pk.TID16:00000}";

            if (pk.Ball != (int)Ball.None)
                ballFormatted = " - " + GameInfo.Strings.balllist[pk.Ball].Split(' ')[0];

            string speciesName = SpeciesName.GetSpeciesNameGeneration(pk.Species, (int)LanguageID.English, pk.Format);
            if (pk is IGigantamax gmax && gmax.CanGigantamax)
                speciesName += "-Gmax";

            string OTInfo = string.IsNullOrEmpty(pk.OT_Name) ? "" : $" - {pk.OT_Name} - {TIDFormatted}{ballFormatted}";
            OTInfo = string.Concat(OTInfo.Split(Path.GetInvalidFileNameChars())).Trim();

            if (pk is PK8)
            {
                bool hasMark = HasMark((PK8)pk, out RibbonIndex mark);
                if (hasMark)
                    marktype = hasMark ? $"{mark.ToString().Replace("Mark", "")}Mark - " : "";
            }

            if (pk is PK9)
            {
                bool hasMark = HasMark((PK9)pk, out RibbonIndex mark);
                if (hasMark)
                    marktype = hasMark ? $"{mark.ToString().Replace("Mark", "")}Mark - " : "";
            }

            string filename = $"{pk.Species:000}{form}{shinytype} - {speciesName} - {marktype}{IVList}{OTInfo} - {pk.EncryptionConstant:X8}";
            string filetype = "";
            if (pk is PK8)
                filetype = ".pk8";
            if (pk is PB8)
                filetype = ".pb8";
            if (pk is PA8)
                filetype = ".pa8";
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

        public static bool HasMark(IRibbonIndex pk, out RibbonIndex result)
        {
            result = default;
            for (var mark = RibbonIndex.MarkLunchtime; mark <= RibbonIndex.MarkTitan; mark++)
            {
                if (pk.GetRibbon((int)mark))
                {
                    result = mark;
                    return true;
                }
            }
            return false;
        }

        public static bool HasAffixedRibbon(IRibbonSetAffixed pk, out RibbonIndex result)
        {
            result = default;
            for (var mark = RibbonIndex.ChampionKalos; mark <= RibbonIndex.MarkTitan; mark++)
            {
                if (pk.AffixedRibbon == ((int)mark))
                {
                    result = mark;
                    return true;
                }
            }
            return false;
        }

        public enum RoutineType
        {
            None,
            Read,
        }
    }

    public class ViewerOffsets
    {
        public IReadOnlyList<long> BlockKeyPointer = new List<long>() { 0x4617648, 0xD8, 0x0, 0x0, 0x30, 0x0 };
        public IReadOnlyList<long> RaidBlockPointerP { get; } = new long[] { 0x4617648, 0x1C0, 0x88, 0x40 };
        public IReadOnlyList<long> RaidBlockPointerK { get; } = new long[] { 0x4617648, 0x1C0, 0x88, 0xCD8 };
        public IReadOnlyList<long> OverworldPointer { get; } = new long[] { 0x461CB18, 0x160, 0xE8, 0x28 };
        public IReadOnlyList<long> MyStatusPointerSV { get; } = new long[] { 0x4617648, 0x1C0, 0x0, 0x40 };
        public IReadOnlyList<long> MyStatusPointerLA { get; } = new long[] { 0x42BA6B0, 0x218, 0x68 };
        public IReadOnlyList<long> MyStatusTrainerPointerBD { get; } = new long[] { 0x4C64DC0, 0xB8, 0x10, 0xE0, 0x0 };
        public IReadOnlyList<long> MyStatusTIDPointerBD { get; } = new long[] { 0x4C64DC0, 0xB8, 0x10, 0xE8 };
        public IReadOnlyList<long> MyStatusTrainerPointerSP { get; } = new long[] { 0x4E7BE98, 0xB8, 0x10, 0xE0, 0x0 };
        public IReadOnlyList<long> MyStatusTIDPointerSP { get; } = new long[] { 0x4E7BE98, 0xB8, 0x10, 0xE8 };

        public readonly uint EggData = 0x04623BA8;
        public readonly uint PicnicMenu = 0x046B4020;

        public readonly uint StartingOffset = 0x4505B880;
        public readonly uint KCoordIncrement = 192;
        public readonly uint FishingOffset = 0x4505B640; // Not in any wild area
        public readonly uint LastSpawnOffset = 0x419BB180;
        public readonly uint LastSpawnSprite = 0x419BB184;
        public readonly uint TrainerDataOffsetSWSH = 0x45068F18;
        public readonly int TrainerDataLengthSWSH = 0x110;
        public readonly uint TrainerDataLGPE = 0x53582030;
        public readonly int TrainerSizeLGPE = 0x168;
    }

    public class DataBlock
    {
        public string? Name { get; set; }
        public uint Key { get; set; }
        public SCTypeCode Type { get; set; }
        public SCTypeCode SubType { get; set; }
        public IReadOnlyList<long>? Pointer { get; set; }
        public bool IsEncrypted { get; set; }
        public int Size { get; set; }
    }

    public static class Blocks
    {
        public static DataBlock KMassOutbreakTotalPaldea = new()
        {
            Name = "KMassOutbreakTotalPaldea",
            Key = 0x6C375C8A,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakTotalKitakami = new()
        {
            Name = "KMassOutbreakTotalKitakami",
            Key = 0xBD7C2A04,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBCMainNumActive = new()
        {
            Name = "KOutbreakBCMainNumActive",
            Key = 0x7478FD9A,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBCDLC1NumActive = new()
        {
            Name = "KOutbreakBCDLC1NumActive",
            Key = 0x0D326604,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBCDLC2NumActive = new()
        {
            Name = "KOutbreakBCDLC2NumActive",
            Key = 0x1B4ECAC3,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KBCATOutbreakEnabled = new()
        {
            Name = "KBCATOutbreakEnabled",
            Key = 0x61552076,
            Type = SCTypeCode.Bool1,
            IsEncrypted = true,
            Size = 1,
        };
        #region Outbreak1
        public static DataBlock KOutbreakSpecies1 = new()
        {
            Name = "KOutbreakSpecies1",
            Key = 0x76A2F996,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak01Form = new()
        {
            Name = "KMassOutbreak01Form",
            Key = 0x29B4615D,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO1 = new()
        {
            Name = "KMassOutbreak01NumKOed",
            Key = 0x4B16FBC2,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak01TotalSpawns = new()
        {
            Name = "KMassOutbreak01TotalSpawns",
            Key = 0xB7DC495A,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak01CenterPos = new()
        {
            Name = "KMassOutbreak01CenterPos",
            Key = 0x2ED42F4D,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak2
        public static DataBlock KOutbreakSpecies2 = new()
        {
            Name = "KOutbreakSpecies2",
            Key = 0x76A0BCF3,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak02Form = new()
        {
            Name = "KMassOutbreak02Form",
            Key = 0x29B84368,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO2 = new()
        {
            Name = "KMassOutbreak02NumKOed",
            Key = 0x4B14BF1F,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak02TotalSpawns = new()
        {
            Name = "KMassOutbreak02TotalSpawns",
            Key = 0xB7DA0CB7,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak02CenterPos = new()
        {
            Name = "KMassOutbreak02CenterPos",
            Key = 0x2ED5F198,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak3
        public static DataBlock KOutbreakSpecies3 = new()
        {
            Name = "KOutbreakSpecies3",
            Key = 0x76A97E38,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak03Form = new()
        {
            Name = "KMassOutbreak03Form",
            Key = 0x29AF8223,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO3 = new()
        {
            Name = "KMassOutbreak03NumKOed",
            Key = 0x4B1CA6E4,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak03TotalSpawns = new()
        {
            Name = "KMassOutbreak03TotalSpawns",
            Key = 0xB7E1F47C,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak03CenterPos = new()
        {
            Name = "KMassOutbreak03CenterPos",
            Key = 0x2ECE09D3,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak4
        public static DataBlock KOutbreakSpecies4 = new()
        {
            Name = "KOutbreakSpecies4",
            Key = 0x76A6E26D,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak04Form = new()
        {
            Name = "KMassOutbreak04Form",
            Key = 0x29B22B86,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO4 = new()
        {
            Name = "KMassOutbreak04NumKOed",
            Key = 0x4B1A77D9,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak04TotalSpawns = new()
        {
            Name = "KMassOutbreak04TotalSpawns",
            Key = 0xB7DFC571,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak04CenterPos = new()
        {
            Name = "KMassOutbreak04CenterPos",
            Key = 0x2ED04676,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak5
        public static DataBlock KOutbreakSpecies5 = new()
        {
            Name = "KOutbreakSpecies5",
            Key = 0x76986F3A,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak05Form = new()
        {
            Name = "KMassOutbreak05Form",
            Key = 0x29A9D701,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO5 = new()
        {
            Name = "KMassOutbreak05NumKOed",
            Key = 0x4B23391E,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak05TotalSpawns = new()
        {
            Name = "KMassOutbreak05TotalSpawns",
            Key = 0xB7E886B6,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak05CenterPos = new()
        {
            Name = "KMassOutbreak05CenterPos",
            Key = 0x2EC78531,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak6
        public static DataBlock KOutbreakSpecies6 = new()
        {
            Name = "KOutbreakSpecies6",
            Key = 0x76947F97,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak06Form = new()
        {
            Name = "KMassOutbreak06Form",
            Key = 0x29AB994C,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO6 = new()
        {
            Name = "KMassOutbreak06NumKOed",
            Key = 0x4B208FBB,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak06TotalSpawns = new()
        {
            Name = "KMassOutbreak06TotalSpawns",
            Key = 0xB7E49713,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak06CenterPos = new()
        {
            Name = "KMassOutbreak06CenterPos",
            Key = 0x2ECB673C,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak7
        public static DataBlock KOutbreakSpecies7 = new()
        {
            Name = "KOutbreakSpecies7",
            Key = 0x769D40DC,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak07Form = new()
        {
            Name = "KMassOutbreak07Form",
            Key = 0x29A344C7,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO7 = new()
        {
            Name = "KMassOutbreak07NumKOed",
            Key = 0x4B28E440,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak07TotalSpawns = new()
        {
            Name = "KMassOutbreak07TotalSpawns",
            Key = 0xB7EE31D8,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak07CenterPos = new()
        {
            Name = "KMassOutbreak07CenterPos",
            Key = 0x2EC1CC77,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak8
        public static DataBlock KOutbreakSpecies8 = new()
        {
            Name = "KOutbreakSpecies8",
            Key = 0x769B11D1,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak08Form = new()
        {
            Name = "KMassOutbreak08Form",
            Key = 0x29A5EE2A,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO8 = new()
        {
            Name = "KMassOutbreak08NumKOed",
            Key = 0xB29D7978,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak08TotalSpawns = new()
        {
            Name = "KMassOutbreak08TotalSpawns",
            Key = 0xB7EABC8D,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak08CenterPos = new()
        {
            Name = "KMassOutbreak08CenterPos",
            Key = 0x2EC5BC1A,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak9
        public static DataBlock KOutbreakSpecies9 = new()
        {
            Name = "KOutbreakSpecies9",
            Key = 0x37E55F64,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak09Form = new()
        {
            Name = "KMassOutbreak09Form",
            Key = 0x69A930AB,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO9 = new()
        {
            Name = "KMassOutbreak09NumKOed",
            Key = 0xB29D7978,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak09TotalSpawns = new()
        {
            Name = "KMassOutbreak09TotalSpawns",
            Key = 0x9E0CEC77,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak09CenterPos = new()
        {
            Name = "KMassOutbreak09CenterPos",
            Key = 0x411A0C07,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak10
        public static DataBlock KOutbreakSpecies10 = new()
        {
            Name = "KOutbreakSpecies10",
            Key = 0x37E33059,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak10Form = new()
        {
            Name = "KMassOutbreak10Form",
            Key = 0x69AD204E,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO10 = new()
        {
            Name = "KMassOutbreak10NumKOed",
            Key = 0xB29ADDAD,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak10TotalSpawns = new()
        {
            Name = "KMassOutbreak10TotalSpawns",
            Key = 0x9E10DC1A,
            Type = SCTypeCode.Int32,
            Size = 4,
        };
        public static DataBlock KMassOutbreak10CenterPos = new()
        {
            Name = "KMassOutbreak10CenterPos",
            Key = 0x411CB56A,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak11
        public static DataBlock KOutbreakSpecies11 = new()
        {
            Name = "KOutbreakSpecies11",
            Key = 0x37DFB442,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak11Form = new()
        {
            Name = "KMassOutbreak11Form",
            Key = 0x69AEE965,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO11 = new()
        {
            Name = "KMassOutbreak11NumKOed",
            Key = 0xB298A7D6,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak11TotalSpawns = new()
        {
            Name = "KMassOutbreak11TotalSpawns",
            Key = 0x9E12A531,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak11CenterPos = new()
        {
            Name = "KMassOutbreak11CenterPos",
            Key = 0x411EEB41,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region Outbreak12
        public static DataBlock KOutbreakSpecies12 = new()
        {
            Name = "KOutbreakSpecies12",
            Key = 0x37DD779F,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak12Form = new()
        {
            Name = "KMassOutbreak12Form",
            Key = 0x69B2CB70,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KMassOutbreakKO12 = new()
        {
            Name = "KMassOutbreakKO12",
            Key = 0xB294B833,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KMassOutbreak12TotalSpawns = new()
        {
            Name = "KMassOutbreak12TotalSpawns",
            Key = 0x9E16873C,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KMassOutbreak12CenterPos = new()
        {
            Name = "KMassOutbreak12CenterPos",
            Key = 0x4122608C,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC01MainSpecies
        public static DataBlock KOutbreakBC01MainSpecies = new()
        {
            Name = "KOutbreakBC01MainSpecies",
            Key = 0x84AB44A6,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC01MainForm = new()
        {
            Name = "KOutbreakBC01MainForm",
            Key = 0xD82BDDAD,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC01MainNumKOed = new()
        {
            Name = "KOutbreakBC01MainNumKOed",
            Key = 0x65AC15F2,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC01MainTotalSpawns = new()
        {
            Name = "KOutbreakBC01MainTotalSpawns",
            Key = 0x71862A2A,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC01MainCenterPos = new()
        {
            Name = "KOutbreakBC01MainCenterPos",
            Key = 0x71DB2C9D,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC02MainSpecies
        public static DataBlock KOutbreakBC02MainSpecies = new()
        {
            Name = "KOutbreakBC02MainSpecies",
            Key = 0x84A7C1C3,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC02MainForm = new()
        {
            Name = "KOutbreakBC02MainForm",
            Key = 0xD82E7978,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC02MainNumKOed = new()
        {
            Name = "KOutbreakBC02MainNumKOed",
            Key = 0x65A8930F,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC02MainTotalSpawns = new()
        {
            Name = "KOutbreakBC02MainTotalSpawns",
            Key = 0x71862A2A,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC02MainCenterPos = new()
        {
            Name = "KOutbreakBC02MainCenterPos",
            Key = 0x71DD5BA8,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC03MainSpecies
        public static DataBlock KOutbreakBC03MainSpecies = new()
        {
            Name = "KOutbreakBC03MainSpecies",
            Key = 0x84B15C88,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC03MainForm = new()
        {
            Name = "KOutbreakBC03MainForm",
            Key = 0xD825B833,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC03MainNumKOed = new()
        {
            Name = "KOutbreakBC03MainNumKOed",
            Key = 0x65B22DD4,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC03MainTotalSpawns = new()
        {
            Name = "KOutbreakBC03MainTotalSpawns",
            Key = 0x718BD54C,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC03MainCenterPos = new()
        {
            Name = "KOutbreakBC03MainCenterPos",
            Key = 0x71D49A63,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC04MainSpecies
        public static DataBlock KOutbreakBC04MainSpecies = new()
        {
            Name = "KOutbreakBC04MainSpecies",
            Key = 0x84AD7A7D,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC04MainForm = new()
        {
            Name = "KOutbreakBC04MainForm",
            Key = 0xD829A7D6,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC04MainNumKOed = new()
        {
            Name = "KOutbreakBC04MainNumKOed",
            Key = 0x65AE4BC9,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC04MainTotalSpawns = new()
        {
            Name = "KOutbreakBC04MainTotalSpawns",
            Key = 0x718A1301,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC04MainCenterPos = new()
        {
            Name = "KOutbreakBC04MainCenterPos",
            Key = 0x71D743C6,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC05MainSpecies
        public static DataBlock KOutbreakBC05MainSpecies = new()
        {
            Name = "KOutbreakBC05MainSpecies",
            Key = 0x849F074A,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC05MainForm = new()
        {
            Name = "KOutbreakBC05MainForm",
            Key = 0xD8200D11,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC05MainNumKOed = new()
        {
            Name = "KOutbreakBC05MainNumKOed",
            Key = 0x65B70D0E,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC05MainTotalSpawns = new()
        {
            Name = "KOutbreakBC05MainTotalSpawns",
            Key = 0x71926786,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC05MainCenterPos = new()
        {
            Name = "KOutbreakBC05MainCenterPos",
            Key = 0x71CEEF41,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC06MainSpecies
        public static DataBlock KOutbreakBC06MainSpecies = new()
        {
            Name = "KOutbreakBC06MainSpecies",
            Key = 0x849D3767,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC06MainForm = new()
        {
            Name = "KOutbreakBC06MainForm",
            Key = 0xD823EF1C,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC06MainNumKOed = new()
        {
            Name = "KOutbreakBC06MainNumKOed",
            Key = 0x65B4D06B,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC06MainTotalSpawns = new()
        {
            Name = "KOutbreakBC06MainTotalSpawns",
            Key = 0x718FBE23,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC06MainCenterPos = new()
        {
            Name = "KOutbreakBC06MainCenterPos",
            Key = 0x71D2648C,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC07MainSpecies
        public static DataBlock KOutbreakBC07MainSpecies = new()
        {
            Name = "KOutbreakBC07MainSpecies",
            Key = 0x84A58BEC,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC07MainForm = new()
        {
            Name = "KOutbreakBC07MainForm",
            Key = 0xD81B2DD7,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC07MainNumKOed = new()
        {
            Name = "KOutbreakBC07MainNumKOed",
            Key = 0x65BCB830,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC07MainTotalSpawns = new()
        {
            Name = "KOutbreakBC07MainTotalSpawns",
            Key = 0x71987F68,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC07MainCenterPos = new()
        {
            Name = "KOutbreakBC07MainCenterPos",
            Key = 0x71CA1007,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC08MainSpecies
        public static DataBlock KOutbreakBC08MainSpecies = new()
        {
            Name = "KOutbreakBC08MainSpecies",
            Key = 0x84A2F021,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC08MainForm = new()
        {
            Name = "KOutbreakBC08MainForm",
            Key = 0xD81D6A7A,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC08MainNumKOed = new()
        {
            Name = "KOutbreakBC08MainNumKOed",
            Key = 0x65BA8925,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC08MainTotalSpawns = new()
        {
            Name = "KOutbreakBC08MainTotalSpawns",
            Key = 0x71949D5D,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC08MainCenterPos = new()
        {
            Name = "KOutbreakBC08MainCenterPos",
            Key = 0x71CCB96A,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC09MainSpecies
        public static DataBlock KOutbreakBC09MainSpecies = new()
        {
            Name = "KOutbreakBC09MainSpecies",
            Key = 0x84C2791E,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC09MainForm = new()
        {
            Name = "KOutbreakBC09MainForm",
            Key = 0xD842A565,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC09MainNumKOed = new()
        {
            Name = "KOutbreakBC09MainNumKOed",
            Key = 0x65954E3A,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC09MainTotalSpawns = new()
        {
            Name = "KOutbreakBC09MainTotalSpawns",
            Key = 0x719E3822,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC09MainCenterPos = new()
        {
            Name = "KOutbreakBC09MainCenterPos",
            Key = 0x71F18795,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC10MainSpecies
        public static DataBlock KOutbreakBC10MainSpecies = new()
        {
            Name = "KOutbreakBC10MainSpecies",
            Key = 0x84BFCFBB,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC10MainForm = new()
        {
            Name = "KOutbreakBC10MainForm",
            Key = 0xD8468770,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC10MainNumKOed = new()
        {
            Name = "KOutbreakBC10MainNumKOed",
            Key = 0x65915E97,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC10MainTotalSpawns = new()
        {
            Name = "KOutbreakBC10MainTotalSpawns",
            Key = 0x719A487F,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC10MainCenterPos = new()
        {
            Name = "KOutbreakBC10MainCenterPos",
            Key = 0x71F42360,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC01DLC1Species
        public static DataBlock KOutbreakBC01DLC1Species = new()
        {
            Name = "KOutbreakBC01DLC1Species",
            Key = 0x0F4D3B64,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC01DLC1Form = new()
        {
            Name = "KOutbreakBC01DLC1Form",
            Key = 0x41110CAB,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC01DLC1NumKOed = new()
        {
            Name = "KOutbreakBC01DLC1NumKOed",
            Key = 0xAA733578,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC01DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC01DLC1TotalSpawns",
            Key = 0x95EC433C,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC01DLC1CenterPos = new()
        {
            Name = "KOutbreakBC01DLC1CenterPos",
            Key = 0xB3C20007,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC02DLC1Species
        public static DataBlock KOutbreakBC02DLC1Species = new()
        {
            Name = "KOutbreakBC02DLC1Species",
            Key = 0x0F4B0C59,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC02DLC1Form = new()
        {
            Name = "KOutbreakBC02DLC1Form",
            Key = 0x4114FC4E,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC02DLC1NumKOed = new()
        {
            Name = "KOutbreakBC02DLC1NumKOed",
            Key = 0xAA7099AD,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC02DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC02DLC1TotalSpawns",
            Key = 0x95E86131,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC02DLC1CenterPos = new()
        {
            Name = "KOutbreakBC02DLC1CenterPos",
            Key = 0xB3C4A96A,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC03DLC1Species
        public static DataBlock KOutbreakBC03DLC1Species = new()
        {
            Name = "KOutbreakBC03DLC1Species",
            Key = 0x0F479042,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC03DLC1Form = new()
        {
            Name = "KOutbreakBC03DLC1Form",
            Key = 0x4116C565,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC03DLC1NumKOed = new()
        {
            Name = "KOutbreakBC03DLC1NumKOed",
            Key = 0xAA6E63D6,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC03DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC03DLC1TotalSpawns",
            Key = 0x95E6981A,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC03DLC1CenterPos = new()
        {
            Name = "KOutbreakBC03DLC1CenterPos",
            Key = 0xB3C6DF41,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC04DLC1Species
        public static DataBlock KOutbreakBC04DLC1Species = new()
        {
            Name = "KOutbreakBC04DLC1Species",
            Key = 0x0F45539F,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC04DLC1Form = new()
        {
            Name = "KOutbreakBC04DLC1Form",
            Key = 0x411AA770,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC04DLC1NumKOed = new()
        {
            Name = "KOutbreakBC04DLC1NumKOed",
            Key = 0xAA6A7433,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC04DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC04DLC1TotalSpawns",
            Key = 0x95E2A877,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC04DLC1CenterPos = new()
        {
            Name = "KOutbreakBC04DLC1CenterPos",
            Key = 0xB3CA548C,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC05DLC1Species
        public static DataBlock KOutbreakBC05DLC1Species = new()
        {
            Name = "KOutbreakBC05DLC1Species",
            Key = 0x0F5978C0,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC05DLC1Form = new()
        {
            Name = "KOutbreakBC05DLC1Form",
            Key = 0x4106824F,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC05DLC1NumKOed = new()
        {
            Name = "KOutbreakBC05DLC1NumKOed",
            Key = 0xAA68AB1C,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC05DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC05DLC1TotalSpawns",
            Key = 0x95F6CD98,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC05DLC1CenterPos = new()
        {
            Name = "KOutbreakBC05DLC1CenterPos",
            Key = 0xB3CC8A63,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC06DLC1Species
        public static DataBlock KOutbreakBC06DLC1Species = new()
        {
            Name = "KOutbreakBC06DLC1Species",
            Key = 0x0F560375,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC06DLC1Form = new()
        {
            Name = "KOutbreakBC06DLC1Form",
            Key = 0x41085232,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC06DLC1NumKOed = new()
        {
            Name = "KOutbreakBC06DLC1NumKOed",
            Key = 0xAA64C911,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC06DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC06DLC1TotalSpawns",
            Key = 0x95F50B4D,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC06DLC1CenterPos = new()
        {
            Name = "KOutbreakBC06DLC1CenterPos",
            Key = 0xB3CF33C6,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC07DLC1Species
        public static DataBlock KOutbreakBC07DLC1Species = new()
        {
            Name = "KOutbreakBC07DLC1Species",
            Key = 0x0F53CD9E,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC07DLC1Form = new()
        {
            Name = "KOutbreakBC07DLC1Form",
            Key = 0x410C3B09,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC07DLC1NumKOed = new()
        {
            Name = "KOutbreakBC07DLC1NumKOed",
            Key = 0xAA62267A,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC07DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC07DLC1TotalSpawns",
            Key = 0x95F12276,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC07DLC1CenterPos = new()
        {
            Name = "KOutbreakBC07DLC1CenterPos",
            Key = 0xB3D31C9D,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC08DLC1Species
        public static DataBlock KOutbreakBC08DLC1Species = new()
        {
            Name = "KOutbreakBC08DLC1Species",
            Key = 0x0F51243B,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC08DLC1Form = new()
        {
            Name = "KOutbreakBC08DLC1Form",
            Key = 0x410E6A14,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC08DLC1NumKOed = new()
        {
            Name = "KOutbreakBC08DLC1NumKOed",
            Key = 0xAA5FE9D7,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC08DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC08DLC1TotalSpawns",
            Key = 0x95EEE5D3,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC08DLC1CenterPos = new()
        {
            Name = "KOutbreakBC08DLC1CenterPos",
            Key = 0xB3D54BA8,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC09DLC1Species
        public static DataBlock KOutbreakBC09DLC1Species = new()
        {
            Name = "KOutbreakBC09DLC1Species",
            Key = 0x0F36E06C,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC09DLC1Form = new()
        {
            Name = "KOutbreakBC09DLC1Form",
            Key = 0x40F9D833,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC09DLC1NumKOed = new()
        {
            Name = "KOutbreakBC09DLC1NumKOed",
            Key = 0xAA8B4370,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC09DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC09DLC1TotalSpawns",
            Key = 0x960377B4,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC09DLC1CenterPos = new()
        {
            Name = "KOutbreakBC09DLC1CenterPos",
            Key = 0xB3D8C7BF,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC10DLC1Species
        public static DataBlock KOutbreakBC10DLC1Species = new()
        {
            Name = "KOutbreakBC10DLC1Species",
            Key = 0x0F3444A1,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC10DLC1Form = new()
        {
            Name = "KOutbreakBC10DLC1Form",
            Key = 0x40FDC7D6,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC10DLC1NumKOed = new()
        {
            Name = "KOutbreakBC10DLC1NumKOed",
            Key = 0xAA876165,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC10DLC1TotalSpawns = new()
        {
            Name = "KOutbreakBC10DLC1TotalSpawns",
            Key = 0x95FF95A9,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC10DLC1CenterPos = new()
        {
            Name = "KOutbreakBC10DLC1CenterPos",
            Key = 0xB3DB0462,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC01DLC2Species
        public static DataBlock KOutbreakBC01DLC2Species = new()
        {
            Name = "KOutbreakBC01DLC2Species",
            Key = 0x03B50A2B,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC01DLC2Form = new()
        {
            Name = "KOutbreakBC01DLC2Form",
            Key = 0x9F47C0A8,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC01DLC2NumKOed = new()
        {
            Name = "KOutbreakBC01DLC2NumKOed",
            Key = 0x6CB77613,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC01DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC01DLC2TotalSpawns",
            Key = 0xCDB0C887,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC01DLC2CenterPos = new()
        {
            Name = "KOutbreakBC01DLC2CenterPos",
            Key = 0xE623D9F6,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC02DLC2Species
        public static DataBlock KOutbreakBC02DLC2Species = new()
        {
            Name = "KOutbreakBC02DLC2Species",
            Key = 0x03B8F9CE,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC02DLC2Form = new()
        {
            Name = "KOutbreakBC02DLC2Form",
            Key = 0x9F45919D,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC02DLC2NumKOed = new()
        {
            Name = "KOutbreakBC02DLC2NumKOed",
            Key = 0x6CBB65B6,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC02DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC02DLC2TotalSpawns",
            Key = 0xCDB371EA,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC02DLC2CenterPos = new()
        {
            Name = "KOutbreakBC02DLC2CenterPos",
            Key = 0xE6219D53,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC03DLC2Species
        public static DataBlock KOutbreakBC03DLC2Species = new()
        {
            Name = "KOutbreakBC03DLC2Species",
            Key = 0x03BAC2E5,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC03DLC2Form = new()
        {
            Name = "KOutbreakBC03DLC2Form",
            Key = 0x9F41A8C6,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC03DLC2NumKOed = new()
        {
            Name = "KOutbreakBC03DLC2NumKOed",
            Key = 0x6CBD9B8D,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC03DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC03DLC2TotalSpawns",
            Key = 0xCDB5A7C1,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC03DLC2CenterPos = new()
        {
            Name = "KOutbreakBC03DLC2CenterPos",
            Key = 0xE6298518,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC04DLC2Species
        public static DataBlock KOutbreakBC04DLC2Species = new()
        {
            Name = "KOutbreakBC04DLC2Species",
            Key = 0x03BEA4F0,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC04DLC2Form = new()
        {
            Name = "KOutbreakBC04DLC2Form",
            Key = 0x9F3EFF63,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC04DLC2NumKOed = new()
        {
            Name = "KOutbreakBC04DLC2NumKOed",
            Key = 0x6CC110D8,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC04DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC04DLC2TotalSpawns",
            Key = 0xCDB91D0C,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC04DLC2CenterPos = new()
        {
            Name = "KOutbreakBC04DLC2CenterPos",
            Key = 0xE627C2CD,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC05DLC2Species
        public static DataBlock KOutbreakBC05DLC2Species = new()
        {
            Name = "KOutbreakBC05DLC2Species",
            Key = 0x03AA7FCF,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC05DLC2Form = new()
        {
            Name = "KOutbreakBC05DLC2Form",
            Key = 0x9F3CC98C,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC05DLC2NumKOed = new()
        {
            Name = "KOutbreakBC05DLC2NumKOed",
            Key = 0x6CACEBB7,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC05DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC05DLC2TotalSpawns",
            Key = 0xCDBB52E3,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC05DLC2CenterPos = new()
        {
            Name = "KOutbreakBC05DLC2CenterPos",
            Key = 0xE6194F9A,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC06DLC2Species
        public static DataBlock KOutbreakBC06DLC2Species = new()
        {
            Name = "KOutbreakBC06DLC2Species",
            Key = 0x03AC4FB2,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC06DLC2Form = new()
        {
            Name = "KOutbreakBC06DLC2Form",
            Key = 0x9F395441,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC06DLC2NumKOed = new()
        {
            Name = "KOutbreakBC06DLC2NumKOed",
            Key = 0x6CAF285A,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC06DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC06DLC2TotalSpawns",
            Key = 0xCDBDFC46,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC06DLC2CenterPos = new()
        {
            Name = "KOutbreakBC06DLC2CenterPos",
            Key = 0xE6155FF7,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC07DLC2Species
        public static DataBlock KOutbreakBC07DLC2Species = new()
        {
            Name = "KOutbreakBC07DLC2Species",
            Key = 0x03B03889,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC07DLC2Form = new()
        {
            Name = "KOutbreakBC07DLC2Form",
            Key = 0x9F371E6A,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC07DLC2NumKOed = new()
        {
            Name = "KOutbreakBC07DLC2NumKOed",
            Key = 0x6CB2A471,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC07DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC07DLC2TotalSpawns",
            Key = 0xCDC1E51D,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC07DLC2CenterPos = new()
        {
            Name = "KOutbreakBC07DLC2CenterPos",
            Key = 0xE61EFABC,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC08DLC2Species
        public static DataBlock KOutbreakBC08DLC2Species = new()
        {
            Name = "KOutbreakBC08DLC2Species",
            Key = 0x03B26794,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC08DLC2Form = new()
        {
            Name = "KOutbreakBC08DLC2Form",
            Key = 0x9F347507,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC08DLC2NumKOed = new()
        {
            Name = "KOutbreakBC08DLC2NumKOed",
            Key = 0x6CB4D37C,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC08DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC08DLC2TotalSpawns",
            Key = 0xCDC41428,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC08DLC2CenterPos = new()
        {
            Name = "KOutbreakBC08DLC2CenterPos",
            Key = 0xE61B18B1,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC09DLC2Species
        public static DataBlock KOutbreakBC09DLC2Species = new()
        {
            Name = "KOutbreakBC09DLC2Species",
            Key = 0x039DD5B3,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC09DLC2Form = new()
        {
            Name = "KOutbreakBC09DLC2Form",
            Key = 0x9F5E8860,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC09DLC2NumKOed = new()
        {
            Name = "KOutbreakBC09DLC2NumKOed",
            Key = 0x6CCF840B,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC09DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC09DLC2TotalSpawns",
            Key = 0xCDC7903F,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC09DLC2CenterPos = new()
        {
            Name = "KOutbreakBC09DLC2CenterPos",
            Key = 0xE63BE7EE,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
        #region KOutbreakBC10DLC2Species
        public static DataBlock KOutbreakBC10DLC2Species = new()
        {
            Name = "KOutbreakBC10DLC2Species",
            Key = 0x03A1C556,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC10DLC2Form = new()
        {
            Name = "KOutbreakBC10DLC2Form",
            Key = 0x9F5BEC95,
            Type = SCTypeCode.Byte,
            IsEncrypted = true,
            Size = 1,
        };
        public static DataBlock KOutbreakBC10DLC2NumKOed = new()
        {
            Name = "KOutbreakBC10DLC2NumKOed",
            Key = 0x6CD1C0AE,
            Type = SCTypeCode.UInt32,
            IsEncrypted = true,
            Size = 4,
        };
        public static DataBlock KOutbreakBC10DLC2TotalSpawns = new()
        {
            Name = "KOutbreakBC10DLC2TotalSpawns",
            Key = 0xCDC9CCE2,
            Type = SCTypeCode.Int32,
        };
        public static DataBlock KOutbreakBC10DLC2CenterPos = new()
        {
            Name = "KOutbreakBC10DLC2CenterPos",
            Key = 0xE637F84B,
            Type = SCTypeCode.Array,
            IsEncrypted = true,
            Size = 12,
        };
        #endregion
    }

}
