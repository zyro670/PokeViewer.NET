using SysBot.Base;

namespace PokeViewer.NET.SubForms
{
    public partial class TurboView : Form
    {
        private readonly ViewerExecutor Executor;
        public TurboView(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            checkBox1.BackColor = color.Item1;
            checkBox1.ForeColor = color.Item2;
            button1.BackColor = color.Item1;
            button1.ForeColor = color.Item2;
            comboBox1.BackColor = color.Item1;
            comboBox1.ForeColor = color.Item2;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            if (!checkBox1.Checked)
                button1.Enabled = false;
            while (!checkBox1.Checked)
            {
                await Click((SwitchButton)comboBox1.SelectedIndex, 0_100, token).ConfigureAwait(false);
            }
            MessageBox.Show("Turbo has been stopped!");
            button1.Enabled = true;
            checkBox1.Checked = false;
        }

        private new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }
    }
}
