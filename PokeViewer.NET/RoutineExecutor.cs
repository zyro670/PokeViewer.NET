using PKHeX.Core;
using SysBot.Pokemon;
using System.Net;

namespace PokeViewer.NET
{
    public class RoutineExecutor
    {
        public int GameType;

        public static byte[] DownloadRemoteImageFile(string uri)
        {
            byte[] content;
#pragma warning disable SYSLIB0014 // Type or member is obsolete
            var request = (HttpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

            using (var response = request.GetResponse())
            using (var reader = new BinaryReader(response.GetResponseStream()))
            {
                content = reader.ReadBytes(100000);
            }

            return content;
        }

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
            string newbase = string.Empty;
            string dimensions = "128x128";
            baseLink = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0001_000_mf_n_00000000_f_n.png".Split('_');

            string pkmform = string.Empty;
            if (pkm.Form != 0)
                pkmform = $"-{pkm.Form}";

            if ((Species)pkm.Species >= Species.Sprigatito || (Species)pkm.Species == Species.Wooper && pkm.Form != 0 || (Species)pkm.Species == Species.Tauros && pkm.Form != 0)
            {
                if (pkm.IsShiny)
                    newbase = $"https://raw.githubusercontent.com/zyro670/PokeTextures/main/Placeholder_Sprites/scaled_up_sprites/Shiny/" + $"{pkm.Species}{pkmform}" + ".png";
                if (!pkm.IsShiny)
                    newbase = $"https://raw.githubusercontent.com/zyro670/PokeTextures/main/Placeholder_Sprites/scaled_up_sprites/" + $"{pkm.Species}{pkmform}" + ".png";

                if ((Species)pkm.Species is Species.Oinkologne && (Gender)pkm.Gender is Gender.Female && !pkm.IsShiny)
                    newbase = $"https://raw.githubusercontent.com/zyro670/PokeTextures/main/Placeholder_Sprites/scaled_up_sprites/" + $"{pkm.Species}f" + ".png";

                return newbase;
            }

            if ((Species)pkm.Species == Species.Enamorus)
            {
                if (!pkm.IsShiny)
                    newbase = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0905_000_fd_n_00000000_f_n.png";
                else
                    newbase = "https://raw.githubusercontent.com/zyro670/HomeImages/master/128x128/poke_capture_0905_000_fd_n_00000000_f_r.png";
                return newbase;
            }

            if (pkm.Species == (ushort)Species.Sneasel)
            {
                if (pkm.Form is 0)
                {
                    if (pkm.Gender is (int)Gender.Male)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_md_n_00000000_f_n.png";
                    if (pkm.Gender is (int)Gender.Male && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_md_n_00000000_f_r.png";
                    if (pkm.Gender is (int)Gender.Female)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_fd_n_00000000_f_n.png";
                    if (pkm.Gender is (int)Gender.Female && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_000_fd_n_00000000_f_r.png";
                }
                if (pkm.Form is 1)
                {
                    if (pkm.Gender is (int)Gender.Male)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_md_n_00000000_f_n.png";
                    if (pkm.Gender is (int)Gender.Male && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_md_n_00000000_f_r.png";
                    if (pkm.Gender is (int)Gender.Female)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_fd_n_00000000_f_n.png";
                    if (pkm.Gender is (int)Gender.Female && pkm.IsShiny)
                        newbase = $"https://raw.githubusercontent.com/zyro670/HomeImages/master/" + dimensions + "/poke_capture_0215_001_fd_n_00000000_f_r.png";
                }
                return newbase;
            }

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

            baseLink[2] = pkm.Species < 10 ? $"000{pkm.Species}" : pkm.Species < 100 && pkm.Species > 9 ? $"00{pkm.Species}" : $"0{pkm.Species}";
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
    }
}
