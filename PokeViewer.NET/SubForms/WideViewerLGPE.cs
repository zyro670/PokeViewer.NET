using PKHeX.Core;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;

namespace PokeViewer.NET.WideViewForms
{
    public partial class WideViewerLGPE : Form
    {
        private readonly ViewerState Executor;
        protected ViewerOffsets Offsets { get; } = new();
        public WideViewerLGPE(ViewerState executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
            Executor.SwitchConnection.SendAsync(SwitchCommand.Configure(SwitchConfigureParameter.controllerType, 1, true), CancellationToken.None);
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            WaitButton.BackColor = color.Item1;
            WaitButton.ForeColor = color.Item2;
            pictureBox1.BackColor = color.Item1;
            pictureBox1.ForeColor = color.Item2;
            flagbox.BackColor = color.Item1;
            flagbox.ForeColor = color.Item2;
        }

        private async Task ViewOverworldLGPE(CancellationToken token)
        {
            PB8 pk = new();
            var species = (Species)BitConverter.ToUInt16(await Executor.SwitchConnection.ReadBytesAsync(Offsets.LastSpawnOffset, 2, token).ConfigureAwait(false), 0);
            var flag = BitConverter.ToUInt16(await Executor.SwitchConnection.ReadBytesAsync(Offsets.LastSpawnSprite, 2, token).ConfigureAwait(false), 0);
            flagbox.Text = $"{flag}";
            pk.Species = (ushort)species;
            var sprite = PokeImg(pk, pk.CanGigantamax);
            pictureBox1.Load(sprite);
            while (!token.IsCancellationRequested)
            {
                species = (Species)BitConverter.ToUInt16(await Executor.SwitchConnection.ReadBytesAsync(Offsets.LastSpawnOffset, 2, token).ConfigureAwait(false), 0);
                if (species != 0)
                    await Executor.SwitchConnection.WriteBytesAsync(new byte[] { 0x0, 0x0 }, Offsets.LastSpawnOffset, token).ConfigureAwait(false);

                flag = BitConverter.ToUInt16(await Executor.SwitchConnection.ReadBytesAsync(Offsets.LastSpawnSprite, 2, token).ConfigureAwait(false), 0);
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
            }
            WaitButton.Text = "Shiny found!";
            await Click(SwitchButton.HOME, 1_000, token).ConfigureAwait(false);
        }

        private async void WaitButton_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            WaitButton.Text = "Waiting...";
            WaitButton.Enabled = false;
            await ViewOverworldLGPE(token).ConfigureAwait(false);
        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

    }
}
