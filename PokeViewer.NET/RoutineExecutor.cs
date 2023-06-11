using PKHeX.Core;
using SysBot.Pokemon;

namespace PokeViewer.NET
{
    public class RoutineExecutor
    {
        public int GameType;
        public static string FormOutput(ushort species, byte form, out string[] formString)
        {
            var strings = GameInfo.GetStrings("en");
            formString = FormConverter.GetFormList(species, strings.Types, strings.forms, GameInfo.GenderSymbolASCII, typeof(EntityContext) == typeof(PKM) ? EntityContext.Gen9 : EntityContext.Gen4);
            if (formString.Length is 0)
                return string.Empty;

            formString[0] = "";
            if (form >= formString.Length)
                form = (byte)(formString.Length - 1);

            return formString[form].Contains("-") ? formString[form] : formString[form] == "" ? "" : $"-{formString[form]}";
        }

        public static string PokeImg(PKM pkm, bool canGmax)
        {
            bool md = false;
            bool fd = false;
            string[] baseLink;
            baseLink = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0001_000_mf_n_00000000_f_n.png".Split('_');

            if (Enum.IsDefined(typeof(GenderDependent), pkm.Species) && !canGmax && pkm.Form is 0)
            {
                if (pkm.Gender is 0 && pkm.Species is not (ushort)Species.Torchic)
                    md = true;
                else fd = true;
            }

            int form = pkm.Species switch
            {
                (ushort)Species.Sinistea or (ushort)Species.Polteageist or (ushort)Species.Rockruff or (ushort)Species.Mothim => 0,
                (ushort)Species.Alcremie when pkm.IsShiny || canGmax => 0,
                _ => pkm.Form,

            };

            if (pkm.Species is (ushort)Species.Sneasel)
            {
                if (pkm.Gender is 0)
                    md = true;
                else fd = true;
            }

            if (pkm.Species is (ushort)Species.Basculegion)
            {
                if (pkm.Gender is 0)
                {
                    md = true;
                    pkm.Form = 0;
                }
                else
                    pkm.Form = 1;

                string s = pkm.IsShiny ? "r" : "n";
                string g = md && pkm.Gender is not 1 ? "md" : "fd";
                return $"https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0" + $"{pkm.Species}" + "_00" + $"{pkm.Form}" + "_" + $"{g}" + "_n_00000000_f_" + $"{s}" + ".png";
            }

            baseLink[2] = pkm.Species < 10 ? $"000{pkm.Species}" : pkm.Species < 100 && pkm.Species > 9 ? $"00{pkm.Species}" : pkm.Species >= 1000 ? $"{pkm.Species}" : $"0{pkm.Species}";
            baseLink[3] = pkm.Form < 10 ? $"00{form}" : $"0{form}";
            baseLink[4] = pkm.PersonalInfo.OnlyFemale ? "fo" : pkm.PersonalInfo.OnlyMale ? "mo" : pkm.PersonalInfo.Genderless ? "uk" : fd ? "fd" : md ? "md" : "mf";
            baseLink[5] = canGmax ? "g" : "n";
            baseLink[6] = "0000000" + (pkm.Species is (ushort)Species.Alcremie && !canGmax ? pkm.Data[0xE4] : 0);
            baseLink[8] = pkm.IsShiny ? "r.png" : "n.png";
            return string.Join("_", baseLink);
        }

    }
    public enum GameSelected
    {
        LetsGoPikachu = 0,
        LetsGoEevee = 1,
        Sword = 2,
        Shield = 3,
        BrilliantDiamond = 4,
        ShiningPearl = 5,
        LegendsArceus = 6,
        Scarlet = 7,
        Violet = 8,
        HOME = 100,
    }
}
