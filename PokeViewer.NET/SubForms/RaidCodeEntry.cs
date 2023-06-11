using PKHeX.Core;
using SysBot.Base;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.SubForms
{
    public partial class RaidCodeEntry : Form
    {
        private readonly ViewerExecutor Executor;
        public RaidCodeEntry(ViewerExecutor executor)
        {
            InitializeComponent();
            Executor = executor;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 4 || textBox1.Text.Length == 5)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
            else
                MessageBox.Show("TextBox is empty. Try again after you fill it in!");
        }

        private async Task EnterRaidCode(CancellationToken token)
        {
            if (textBox1.Text.Length > 6)
            {
                textBox1.Text = textBox1.Text.Substring(0, 6);
            }
            var strokes = textBox1.Text.ToUpper().ToArray();
            var number = $"NumPad";
            string[] badVals = { "@", "I", "O", "=", "&", ";", "Z", "*", "#", "!", "?" };
            List<HidKeyboardKey> keystopress = new();
            foreach (var str in strokes)
            {
                if (badVals.Contains(str.ToString()))
                {
                    MessageBox.Show($"{str} is not a valid button. Stopping code entry.");
                    return;
                }
                foreach (HidKeyboardKey keypress in (HidKeyboardKey[])Enum.GetValues(typeof(HidKeyboardKey)))
                {
                    if (str.ToString().Equals(keypress.ToString()) || (number + str.ToString()).Equals(keypress.ToString()))
                        keystopress.Add(keypress);
                }
            }
            await Executor.SwitchConnection.SendAsync(SwitchCommand.TypeMultipleKeys(keystopress, true), token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);

        }

        public new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async void textBox1_DoubleClicked(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
            if (textBox1.Text.Length < 4 || textBox1.Text.Length == 5)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            AutoPaste.Enabled = false;
            Clipboard.Clear();
            while (!Clipboard.ContainsText())
            {
                await Task.Delay(0_100);
            }
            textBox1.Text = Clipboard.GetText();
            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterRaidCode(CancellationToken.None).ConfigureAwait(false);

            AutoPaste.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }
    }
}
