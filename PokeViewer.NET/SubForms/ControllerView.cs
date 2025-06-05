using SysBot.Base;

namespace PokeViewer.NET.SubForms
{
    public partial class ControllerView : Form
    {
        private readonly ViewerState Executor;
        public ControllerView(ViewerState executor, (Color, Color) color)
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

        private async void button2_Click(object sender, EventArgs e)
        {
            var cmds = textBox1.Text.Split(',');
            var clicks = ReturnCommands(cmds);
            foreach (var c in clicks)
            {
                await Click((SwitchButton)c, (int)numericUpDown1.Value, CancellationToken.None).ConfigureAwait(false);
            }
        }

        private static List<int> ReturnCommands(string[] cmds)
        {
            List<int> values = [];
            for (int i = 0; i < cmds.Length; i++)
            {
                switch (cmds[i].ToUpper())
                {
                    case "A": values.Add(0); break;
                    case "B": values.Add(1); break;
                    case "X": values.Add(2); break;
                    case "Y": values.Add(3); break;
                    case "RSTICK": values.Add(4); break;
                    case "LSTICK": values.Add(5); break;
                    case "R": values.Add(6); break;
                    case "L": values.Add(7); break;
                    case "ZL": values.Add(8); break;
                    case "ZR": values.Add(9); break;
                    case "PLUS": values.Add(10); break;
                    case "MINUS": values.Add(11); break;
                    case "DUP": values.Add(12); break;
                    case "DDOWN": values.Add(13); break;
                    case "DLEFT": values.Add(14); break;
                    case "DRIGHT": values.Add(15); break;
                    case "HOME": values.Add(16); break;
                    case "CAPTURE": values.Add(17); break;
                }
            }
            return values;
        }
    }
}
