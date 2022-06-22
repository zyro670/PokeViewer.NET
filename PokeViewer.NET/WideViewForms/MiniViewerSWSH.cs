using PKHeX.Core;
using PKHeX.Drawing.Misc;
using PKHeX.Drawing.PokeSprite;
using PokeViewer.NET.Properties;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class MiniViewerSWSH : Form
    {
        private readonly static SwitchConnectionConfig Config = new() { Protocol = SwitchProtocol.WiFi, IP = Settings.Default.SwitchIP, Port = 6000 };
        public SwitchSocketAsync SwitchConnection = new(Config);
        public MiniViewerSWSH()
        {
            InitializeComponent();
            SwitchConnection.Connect();
            LiveViewer(CancellationToken.None);
        }

        private const string SwordID = "0100ABF008968000";
        private const string ShieldID = "01008DB008C2C000";
        private int GameType;

        private async void LiveViewer(CancellationToken token)
        {
            string title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            switch (title)
            {
                case SwordID: GameType = (int)GameSelected.SW; break;
                case ShieldID: GameType = (int)GameSelected.SH; break;
            }
            string isShiny = string.Empty;         
            PK8 pk = new();
            while (!token.IsCancellationRequested)
            {                
                var data = await SwitchConnection.ReadBytesAsync(0x4c42a628, 88, token).ConfigureAwait(false);
                pk.Species = BitConverter.ToUInt16(data.Slice(0, 2), 0);
                if (pk.Species == 0)
                    continue;
                pk.Form = BitConverter.ToUInt16(data.Slice(2, 4), 0);
                var shinytype = BitConverter.ToUInt16(data.Slice(8, 2), 0);
                if (shinytype != 1)
                {
                    CommonEdits.SetIsShiny(pk, false);
                    isShiny = "Shiny: No";
                }
                if (shinytype == 1)
                {
                    CommonEdits.SetIsShiny(pk, true);
                    isShiny = "Shiny: Yes";
                }
                var markVal = BitConverter.ToUInt16(data.Slice(72, 2), 0);
                if (markVal != 255)
                    pk.SetRibbonIndex((RibbonIndex)markVal);
                var hasMark = HasMark(pk, out RibbonIndex mark);
                string msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : string.Empty;
                var overworldseed = BitConverter.ToUInt32(data.Slice(80, 4), 0);
                var formz = FormOutput(pk.Species, pk.Form, out _);
                bool canGmax = new ShowdownSet(ShowdownParsing.GetShowdownText(pk)).CanGigantamax;
                var sprite = PokeImg(pk, canGmax, false);
                pictureBox1.Load(sprite);
                textBox1.Text = $"{(Species)pk.Species}{formz}{Environment.NewLine}Seed: {overworldseed:X8}{Environment.NewLine}{isShiny}{msg}";
                if (hasMark)
                {
                    var info = RibbonInfo.GetRibbonInfo(pk);
                    foreach (var rib in info)
                    {
                        if (!rib.HasRibbon)
                            continue;

                        var mimg = RibbonSpriteUtil.GetRibbonSprite(rib.Name);
                        if (mimg is not null)
                        {
                            pictureBox2.Image = mimg;
                        }
                    }
                }
                else if (!hasMark)
                {
                    pictureBox2.Image = null;
                }
                pk = new();
            }
        }

        public async Task<bool> IsOnOverworldTitle(CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesAsync((uint)(GameType == (int)GameVersion.SH ? 0x3F128626 : 0x3F128624), 1, token).ConfigureAwait(false);
            return data[0] == (GameType == (int)GameSelected.SH ? 0x40 : 0x41);
        }

        public static bool HasMark(IRibbonIndex pk, out RibbonIndex result)
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
    }
}
