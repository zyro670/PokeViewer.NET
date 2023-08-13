using PKHeX.Core;
using SysBot.Base;
using static SysBot.Base.SwitchButton;

namespace PokeViewer.NET.SubForms
{
    public partial class EventCodeEntry : Form
    {
        private readonly ViewerExecutor Executor;
        protected ViewerOffsets ViewerOffsets { get; } = new();
        public EventCodeEntry(ViewerExecutor executor, (Color, Color) color)
        {
            InitializeComponent();
            Executor = executor;
            SetColors(color);
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            RedeemButton.BackColor = color.Item1;
            RedeemButton.ForeColor = color.Item2;
            ClearButton.BackColor = color.Item1;
            ClearButton.ForeColor = color.Item2;
            textBox1.BackColor = color.Item1;
            textBox1.ForeColor = color.Item2;
        }

        private async Task EnterCode(CancellationToken token)
        {
            var strokes = textBox1.Text.ToUpper().Trim().ToArray();
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

            for (int i = 0; i < keystopress.Count; i++)
            {
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TypeKey(keystopress[i], true), token).ConfigureAwait(false);
                await Task.Delay(0_050, token).ConfigureAwait(false);
            }
            await Click(PLUS, 0_500, token).ConfigureAwait(false);
            await Click(PLUS, 0_500, token).ConfigureAwait(false);

        }

        private async void RedeemButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 16)
            {
                MessageBox.Show($"{textBox1.Text} is not a valid code entry. Please try again.");
            }

            if (!string.IsNullOrEmpty(textBox1.Text))
                await EnterCode(CancellationToken.None).ConfigureAwait(false);
            else
                MessageBox.Show("TextBox is empty. Try again after you fill it in!");
        }

        private new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }
    }
}
