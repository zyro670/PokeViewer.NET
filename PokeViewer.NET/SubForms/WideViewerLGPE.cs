using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class WideViewerLGPE : Form
    {
        private readonly SwitchSocketAsync SwitchConnection;

        public WideViewerLGPE(SwitchSocketAsync switchConnection)
        {
            InitializeComponent();
            SwitchConnection = switchConnection;
            SwitchConnection.SendAsync(SwitchCommand.Configure(SwitchConfigureParameter.controllerType, 1, true), CancellationToken.None);
        }

        public const uint LastSpawnOffset = 0x419BB180;
        public const uint LastSpawnSprite = 0x419BB184;
        private async Task ViewOverworldLGPE(CancellationToken token)
        {
            PB8 pk = new();
            var species = (Species)BitConverter.ToUInt16(await SwitchConnection.ReadBytesAsync(LastSpawnOffset, 2, token).ConfigureAwait(false), 0);
            var flag = BitConverter.ToUInt16(await SwitchConnection.ReadBytesAsync(LastSpawnSprite, 2, token).ConfigureAwait(false), 0);
            flagbox.Text = $"{flag}";
            pk.Species = (ushort)species;            
            var sprite = PokeImg(pk, pk.CanGigantamax);
            pictureBox1.Load(sprite);
            /*if (pk.Species == 0)
            {
                await Task.Delay(7_000, token).ConfigureAwait(false);
                await Click(SwitchButton.B, 1_200, token).ConfigureAwait(false);
                await Click(SwitchButton.A, 1_200, token).ConfigureAwait(false);
                await Click(SwitchButton.A, 1_200, token).ConfigureAwait(false);
            }*/
            while (!token.IsCancellationRequested)
            {
                species = (Species)BitConverter.ToUInt16(await SwitchConnection.ReadBytesAsync(LastSpawnOffset, 2, token).ConfigureAwait(false), 0);
                if (species != 0)
                    await SwitchConnection.WriteBytesAsync(new byte[] { 0x0, 0x0 }, LastSpawnOffset, token).ConfigureAwait(false);

                flag = BitConverter.ToUInt16(await SwitchConnection.ReadBytesAsync(LastSpawnSprite, 2, token).ConfigureAwait(false), 0);
                if (flag >= 2 && flag < 4)
                    break;

                flagbox.Text = $"{flag}";
                if (species != 0 && species < Species.MAX_COUNT)
                {
                    pk.Species = (ushort)species;
                    if (flag <= 1)
                        CommonEdits.SetIsShiny(pk, false);
                    if (flag > 1 && flag < 4)
                        CommonEdits.SetIsShiny(pk, true);
                    sprite = PokeImg(pk, false);
                    pictureBox1.Load(sprite);
                }
                /*if (!await IsOnOverworldTitle(token).ConfigureAwait(false))
                {
                    await Task.Delay(7_000, token).ConfigureAwait(false);
                    await Click(SwitchButton.B, 1_200, token).ConfigureAwait(false);
                    await Click(SwitchButton.A, 1_200, token).ConfigureAwait(false);
                    await Click(SwitchButton.A, 1_200, token).ConfigureAwait(false);
                }*/
            }
            //button1.Text = "Shiny found!";
            //await Click(SwitchButton.X, 1_000, token).ConfigureAwait(false);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;            
            button1.Text = "Waiting...";
            button1.Enabled = false;
            await ViewOverworldLGPE(token).ConfigureAwait(false);
        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {            
            await SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        public async Task<bool> IsOnOverworldTitle(CancellationToken token)
        {
            var data = await SwitchConnection.ReadBytesMainAsync(0x163F694, 1, token).ConfigureAwait(false);
            return data[0] == 0;
        }
    }
}
