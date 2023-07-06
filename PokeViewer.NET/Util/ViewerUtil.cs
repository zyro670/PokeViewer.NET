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

            if (pk is PK8)
            {
                bool hasMark = HasMark((PK8)pk, out RibbonIndex mark);
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
        public IReadOnlyList<long> BlockKeyPointer = new List<long>() { 0x44B71A8, 0xD8, 0x0, 0x0, 0x30, 0x0 };
        public IReadOnlyList<long> OverworldPointer { get; } = new long[] { 0x44E5068, 0x348, 0x10, 0xD8, 0x28 };
        public IReadOnlyList<long> MyStatusPointerSV { get; } = new long[] { 0x44C1C18, 0x100, 0x40 };
        public IReadOnlyList<long> MyStatusPointerLA { get; } = new long[] { 0x42BA6B0, 0x218, 0x68 };
        public IReadOnlyList<long> MyStatusTrainerPointerBD { get; } = new long[] { 0x4E36C58, 0xB8, 0x10, 0xE0, 0x0 };
        public IReadOnlyList<long> MyStatusTIDPointerBD { get; } = new long[] { 0x4E36C58, 0xB8, 0x10, 0xE8 };
        public IReadOnlyList<long> MyStatusTrainerPointerSP { get; } = new long[] { 0x4E36C58, 0xB8, 0x10, 0xE0, 0x0 };
        public IReadOnlyList<long> MyStatusTIDPointerSP { get; } = new long[] { 0x4E36C58, 0xB8, 0x10, 0xE8 };
        public readonly uint EggData = 0x044C3348;
        public readonly uint PicnicMenu = 0x04553020;
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
}
