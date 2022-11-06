using PKHeX.Core;
using SysBot.Base;
using SysBot.Pokemon;
using System.Net;

namespace PokeViewer.NET
{
    public class RoutineExecutor
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Properties.Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
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
            formString = FormConverter.GetFormList(species, strings.Types, strings.forms, GameInfo.GenderSymbolASCII, typeof(EntityContext) == typeof(PK8) ? EntityContext.Gen8 : EntityContext.Gen4);
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
    enum GameSelected
    {
        LGP = 0,
        LGE = 1,
        SW = 2,
        SH = 3,
        BD = 4,
        SP = 5,
        LA = 6,
        Scarlet = 7,
        Violet = 8,
    }    
}
