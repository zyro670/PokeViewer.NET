using SysBot.Base;

namespace PokeViewer.NET.SubForms
{
    public partial class TimeViewer : Form
    {
        private readonly ViewerExecutor Executor;
        public TimeViewer(ViewerExecutor executor)
        {
            InitializeComponent();
            Executor = executor;
        }

        private async void Backward_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipBack(true), CancellationToken.None).ConfigureAwait(false);
            //ulong unix = (ulong)await Executor.SwitchConnection.SendAsync(SwitchCommand.GetUnixTime(true), CancellationToken.None).ConfigureAwait(false);
            //DateTime Today = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unix).ToLocalTime();            
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            string fal = numericUpDown1.Value == 1 ? "hour" : "hours";
            MessageBox.Show($"Done. We skipped {numericUpDown1.Value} {fal} backward.");
        }

        private async void Forward_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipForward(true), CancellationToken.None).ConfigureAwait(false);
            //ulong unix = (ulong)await Executor.SwitchConnection.SendAsync(SwitchCommand.GetUnixTime(true), CancellationToken.None).ConfigureAwait(false);
            //DateTime Today = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unix).ToLocalTime();
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            string fal = numericUpDown1.Value == 1 ? "hour" : "hours";
            MessageBox.Show($"Done. We skipped {numericUpDown1.Value} {fal} forward.");
        }

        // unixTime currently fails, hide assets for now.
        private async void Read_Click(object sender, EventArgs e)
        {
            var unix = await Executor.SwitchConnection.SendAsync(SwitchCommand.GetUnixTime(true), CancellationToken.None).ConfigureAwait(false);
            DateTime Today = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unix).ToLocalTime();
        }

        private async void Reset_Click(object sender, EventArgs e)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.ResetTime(true), CancellationToken.None).ConfigureAwait(false);
            //ulong unix = (ulong)await Executor.SwitchConnection.SendAsync(SwitchCommand.GetUnixTime(true), CancellationToken.None).ConfigureAwait(false);
            //DateTime Today = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unix).ToLocalTime();
            //label2.Text = $"{Today}";
            MessageBox.Show("Done. Time has been reset.");
        }
    }
}
