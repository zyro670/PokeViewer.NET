using PKHeX.Core;
using SysBot.Base;
using static SysBot.Base.SwitchButton;
using static SysBot.Base.SwitchStick;
using static PokeViewer.NET.RoutineExecutor;
using PKHeX.Drawing.PokeSprite;
using Newtonsoft.Json;
using System.Text;
using PokeViewer.NET.Properties;

namespace PokeViewer.NET.SubForms
{
    public partial class Egg_Viewer : Form
    {
        private readonly ViewerExecutor Executor;
        public Egg_Viewer(ViewerExecutor executor)
        {
            InitializeComponent();
            Executor = executor;
            WebHookText.Text = Settings.Default.WebHook;
            UserDiscordIDText.Text = Settings.Default.UserDiscordID;
        }
        private int eggcount = 0;
        private int sandwichcount = 0;
        private int starcount = 0;
        private int squarecount = 0;
        private PK9 prevShiny = new();
        private readonly uint EggData = 0x044C12D8;
        private readonly uint PicnicMenu = 0x04551020;
        private readonly byte[] BlankVal = { 0x01 };
        private int[] IVFilters = Array.Empty<int>();
        private IReadOnlyList<long> OverworldPointer { get; } = new long[] { 0x44E2FC8, 0x348, 0x10, 0xD8, 0x28 };
        private ulong OverworldOffset;
        private DateTime StartTime;

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            StartTime = DateTime.Now;
            UptimeOnLoad(sender, e);
            await PerformEggRoutine(token).ConfigureAwait(false);
        }

        private void UptimeOnLoad(object sender, EventArgs e)
        {
            var timer = new System.Timers.Timer { Interval = 1000 };
            timer.Elapsed += (o, args) =>
            {
                UptimeLabel.Text = $"Uptime: {StartTime - DateTime.Now:d\\.hh\\:mm\\:ss}";
            };
            timer.Start();
        }

        private async Task PerformEggRoutine(CancellationToken token)
        {
            if (!string.IsNullOrEmpty(Settings.Default.WebHook))
                WebHookText.Text = Settings.Default.WebHook;

            if (ScreenOffBox.Checked)
                await SetScreen(ScreenState.Off, token).ConfigureAwait(false);

            IVFilters = GrabIvFilters();

            if (FetchButton.Enabled == true)
                DisableOptions();

            eggcount = 0;
            await Executor.SwitchConnection.WriteBytesMainAsync(BlankVal, PicnicMenu, token).ConfigureAwait(false);

            if (EatOnStart.Checked)
            {
                await MakeSandwich(token).ConfigureAwait(false);
                await WaitForEggs(token).ConfigureAwait(false);
            }
            else
                await WaitForEggs(token).ConfigureAwait(false);
        }

        private async Task WaitForEggs(CancellationToken token)
        {           
            PK9 pkprev = new();
            while (!token.IsCancellationRequested)
            {
                var wait = TimeSpan.FromMinutes(30);
                var endTime = DateTime.Now + wait;
                var ctr = 0;
                var waiting = 0;                
                while (DateTime.Now < endTime)
                {
                    NextSanwichLabel.Text = $"Next Sandwich: {endTime:hh\\:mm\\:ss}";
                    var pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                    while (pk == prevShiny || pk == null || pkprev.EncryptionConstant == pk.EncryptionConstant || (Species)pk.Species == Species.None)
                    {
                        waiting++;
                        await Task.Delay(1_500, token).ConfigureAwait(false);
                        pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                        if (waiting == 120)
                        {
                            await ReopenPicnic(token).ConfigureAwait(false);
                            await MakeSandwich(token).ConfigureAwait(false);
                            await ReopenPicnic(token).ConfigureAwait(false);
                            wait = TimeSpan.FromMinutes(30);
                            endTime = DateTime.Now + wait;
                            waiting = 0;
                            ctr = 0;
                        }
                    }

                    pk = await ReadPokemonSV(EggData, 344, token).ConfigureAwait(false);
                    while (pk != null && (Species)pk.Species != Species.None && pkprev.EncryptionConstant != pk.EncryptionConstant)
                    {
                        waiting = 0;
                        ctr++;
                        eggcount++;
                        BasketCount.Text = $"Basket Count: {ctr}";
                        string pid = $"{Environment.NewLine}PID: {pk.PID:X8}";
                        string ec = $"{Environment.NewLine}EC: {pk.EncryptionConstant:X8}";
                        var form = FormOutput(pk.Species, pk.Form, out _);
                        string gender = string.Empty;
                        switch (pk.Gender)
                        {
                            case 0: gender = " (M)"; break;
                            case 1: gender = " (F)"; break;
                            case 2: break;
                        }
                        string sensitiveinfo = HidePIDEC.Checked ? "" : $"{pid}{ec}";
                        string output = $"{$"Egg #{eggcount}"}{Environment.NewLine}{(pk.ShinyXor == 0 ? "■ - " : pk.ShinyXor <= 16 ? "★ - " : "")}{(Species)pk.Species}{form}{gender}{sensitiveinfo}{Environment.NewLine}Nature: {(Nature)pk.Nature}{Environment.NewLine}Ability: {(Ability)pk.Ability}{Environment.NewLine}IVs: {pk.IV_HP}/{pk.IV_ATK}/{pk.IV_DEF}/{pk.IV_SPA}/{pk.IV_SPD}/{pk.IV_SPE}{Environment.NewLine}Scale: {PokeSizeDetailedUtil.GetSizeRating(pk.Scale)}";
                        PokeStats.Text = output;
                        LogUtil.LogText(output);
                        var sprite = PokeImg(pk, false);
                        PokeSpriteBox.Load(sprite);
                        var ballsprite = SpriteUtil.GetBallSprite(pk.Ball);
                        BallBox.Image = ballsprite;

                        await Task.Delay(0_500, token).ConfigureAwait(false);

                        bool match = ValidateEncounter(pk);
                        if (!match && pk.IsShiny)
                            SendNotifications(output, sprite, match);
                        if (match)
                        {
                            prevShiny = pk;
                            await Click(HOME, 0_500, token).ConfigureAwait(false);
                            SendNotifications(output, sprite, match);
                            EnableOptions();
                            Activate();
                            MessageBox.Show("Match found! Claim your egg before closing the picnic!");
                            if (ScreenOffBox.Checked)
                                await SetScreen(ScreenState.On, token).ConfigureAwait(false);
                            return;
                        }

                        pkprev = pk;                        
                    }
                    if (ctr == 10)
                    {
                        BasketCount.Text = $"Resetting..";
                        await ReopenPicnic(token).ConfigureAwait(false);
                        ctr = 0;
                        waiting = 0;
                        BasketCount.Text = $"Basket Count: {ctr}";
                    }
                }
                await MakeSandwich(token).ConfigureAwait(false);
            }
        }

        private bool ValidateEncounter(PK9 pk)
        {
            if (pk.IsShiny)
            {
                if (pk.ShinyXor == 0)
                    squarecount++;
                else
                    starcount++;
                ShinyFoundLabel.Text = $"Shinies Found: {squarecount + starcount}";
                SquareStarCount.Text = $"■ - {squarecount} | ★ - {starcount}";
            }

            if (!pk.IVs.SequenceEqual(IVFilters) && !Settings.Default.IgnoreIVFilter)
                return false; // ivs != iv filters and ignore filter is false

            if (pk.Gender != Settings.Default.GenderFilter && Settings.Default.GenderFilter != 3)
                return false; // gender != gender filter when gender is not Any

            if (Settings.Default.MinMaxOnly && pk.Scale > 0 && pk.Scale < 255) // Mini/Jumbo Only
                return false;

            if (!pk.IsShiny && Settings.Default.ShinyFilter is not 0 or 1)
                return false;

            if (pk.IsShiny && Settings.Default.ShinyFilter is not 0 or 1)
            {
                if (Settings.Default.ShinyFilter is 4 && pk.ShinyXor != 0) // SquareOnly
                    return false;

                if (Settings.Default.ShinyFilter is 3 && pk.ShinyXor > 0 && pk.ShinyXor > 16) // StarOnly
                    return false;

                if ((Species)pk.Species is Species.Dunsparce or Species.Tandemaus && pk.EncryptionConstant % 100 != 0 && Settings.Default.SegmentOrFamily)
                    return false;

                if ((Species)pk.Species is Species.Dunsparce or Species.Tandemaus && pk.EncryptionConstant % 100 == 0 && Settings.Default.SegmentOrFamily)
                    return true;
            }
            return true;
        }

        private async Task<PK9> ReadPokemonSV(uint offset, int size, CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesMainAsync(offset, size, token).ConfigureAwait(false);
            var pk = new PK9(data);
            return pk;
        }

        private async Task SetStick(SwitchStick stick, short x, short y, int delay, CancellationToken token)
        {
            var cmd = SwitchCommand.SetStick(stick, x, y, true);
            await Executor.SwitchConnection.SendAsync(cmd, token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task ReopenPicnic(CancellationToken token)
        {
            await Task.Delay(0_500, token).ConfigureAwait(false);
            await Click(Y, 1_500, token).ConfigureAwait(false);
            var overworldWaitCycles = 0;
            var hasReset = false;
            OverworldOffset = await Executor.SwitchConnection.PointerAll(OverworldPointer, token).ConfigureAwait(false);
            while (!await IsOnOverworld(OverworldOffset, token).ConfigureAwait(false))
            {
                await Click(A, 1_000, token).ConfigureAwait(false);
                overworldWaitCycles++;

                if (overworldWaitCycles == 10)
                {
                    for (int i = 0; i < 5; i++)
                        await Click(B, 0_500, token).ConfigureAwait(false);

                    await Click(Y, 1_500, token).ConfigureAwait(false);
                    await Click(A, 1_000, token).ConfigureAwait(false);

                    for (int i = 0; i < 4; i++)
                        await Click(B, 0_500, token).ConfigureAwait(false); 
                }
            }
            for (int i = 0; i < 10; i++)
                await Click(A, 0_500, token).ConfigureAwait(false); 
            await Click(X, 3_000, token).ConfigureAwait(false);
            if (hasReset) 
            {
                await Click(DRIGHT, 0_500, token).ConfigureAwait(false);
                await Click(DDOWN, 0_500, token).ConfigureAwait(false);
                await Click(DDOWN, 0_500, token).ConfigureAwait(false);
            }
            await Click(A, 7_000, token).ConfigureAwait(false); 
        }

        private async Task MakeSandwich(CancellationToken token)
        {
            await Click(MINUS, 0_500, token).ConfigureAwait(false);
            await SetStick(LEFT, 0, 30000, 0_700, token).ConfigureAwait(false); // Face up to table
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
            await Click(A, 1_500, token).ConfigureAwait(false);
            await Click(A, 5_000, token).ConfigureAwait(false);
            await Click(X, 1_500, token).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(Item1Value.Text))
            {
                // Lettuce
                var m1 = Convert.ToInt32(Item1Value.Text);

                for (int i = 0; i < m1; i++)
                {
                    if (checkBox5.Checked)
                        await Click(DUP, 0_800, token).ConfigureAwait(false);
                    else
                    await Click(DDOWN, 0_800, token).ConfigureAwait(false);
                }
            }

            await Click(A, 0_800, token).ConfigureAwait(false);
            await Click(PLUS, 0_800, token).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(Item2Value.Text))
            {
                // Mystica Salt
                var m2 = Convert.ToInt32(Item2Value.Text);

                for (int i = 0; i < m2; i++)
                {
                    if (checkBox6.Checked)
                        await Click(DUP, 0_800, token).ConfigureAwait(false);
                    else
                        await Click(DDOWN, 0_800, token).ConfigureAwait(false);
                }
            }

            await Click(A, 0_800, token).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(Item3Value.Text))
            {
                // Mystica Sweet
                var m3 = Convert.ToInt32(Item3Value.Text);

                for (int i = 0; i < m3; i++)
                {
                    if (checkBox7.Checked)
                        await Click(DUP, 0_800, token).ConfigureAwait(false);
                    else
                        await Click(DDOWN, 0_800, token).ConfigureAwait(false);
                }
            } 

            await Click(A, 0_800, token).ConfigureAwait(false);
            await Click(PLUS, 0_800, token).ConfigureAwait(false);
            // Set pick
            await Click(A, 8_000, token).ConfigureAwait(false);
            //Wait for bread

            var fillingtime = Convert.ToInt32(FillingHoldTime.Text);
            await SetStick(LEFT, 0, 30000, 0_000 + fillingtime, token).ConfigureAwait(false); // Navigate to ingredients
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
            await Task.Delay(0_500, token).ConfigureAwait(false);

            if (HoldIngredients.Checked)
            {
                for (int i = 0; i < NumberOfFillings.Value; i++) // Amount of ingredients to drop
                {
                    await Hold(A, 0_800, token).ConfigureAwait(false);

                    await SetStick(LEFT, 0, -30000, 0_000 + fillingtime, token).ConfigureAwait(false); // Navigate to ingredients
                    await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
                    await Task.Delay(0_500, token).ConfigureAwait(false);
                    await Release(A, 0_800, token).ConfigureAwait(false);

                    await SetStick(LEFT, 0, 30000, 0_000 + fillingtime, token).ConfigureAwait(false); // Navigate to ingredients
                    await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
                    await Task.Delay(0_500, token).ConfigureAwait(false);
                }
            }

            for (int i = 0; i < 12; i++) // If everything is properly positioned
                await Click(A, 0_800, token).ConfigureAwait(false);

            // Sandwich failsafe
            for (int i = 0; i < 5; i++) //Attempt this several times to ensure it goes through
                await SetStick(LEFT, 0, 30000, 1_000, token).ConfigureAwait(false); // Scroll to the absolute top
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);

            while (await PicnicState(token).ConfigureAwait(false) == 2) // Until we start eating the sandwich
            {
                await SetStick(LEFT, 0, -5000, 0_300, token).ConfigureAwait(false); // Scroll down slightly and press A a few times; repeat until un-stuck
                await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);

                for (int i = 0; i < 6; i++)
                    await Click(A, 0_800, token).ConfigureAwait(false);
            }

            while (await PicnicState(token).ConfigureAwait(false) == 3)  // eating the sandwich
                await Task.Delay(1_000, token).ConfigureAwait(false);

            sandwichcount++;
            SandwichCount.Text = $"Sandwiches Made: {sandwichcount}";

            while (!await IsInPicnic(token).ConfigureAwait(false)) // Acknowledge the sandwich and return to the picnic            
                await Click(A, 5_000, token).ConfigureAwait(false); // Wait a long time to give the flag a chance to update and avoid sandwich re-entry    

            await Task.Delay(2_500, token).ConfigureAwait(false);
            await SetStick(LEFT, 0, -10000, 0_500, token).ConfigureAwait(false); // Face down to basket
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);
            await Task.Delay(1_000, token).ConfigureAwait(false);
            await SetStick(LEFT, 0, 5000, 0_200, token).ConfigureAwait(false); // Face up to basket
            await SetStick(LEFT, 0, 0, 0, token).ConfigureAwait(false);

        }

        private new async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task Hold(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Hold(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task Release(SwitchButton b, int delay, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Release(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will restart the application. Do you wish to continue?", "Hard Stop Initiated", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Executor.Disconnect();
                Application.Restart();
            }

            else if (dialogResult == DialogResult.No)
                return;
        }

        private async Task<int> PicnicState(CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesMainAsync(PicnicMenu, 1, token).ConfigureAwait(false);
            return data[0]; // 1 when in picnic, 2 in sandwich menu, 3 when eating, 2 when done eating
        }

        private async Task<bool> IsInPicnic(CancellationToken token)
        {
            var Data = await Executor.SwitchConnection.ReadBytesMainAsync(PicnicMenu, 1, token).ConfigureAwait(false);
            return Data[0] == 0x01; // 1 when in picnic, 2 in sandwich menu, 3 when eating, 2 when done eating
        }

        private async Task<bool> IsOnOverworld(ulong offset, CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(offset, 1, token).ConfigureAwait(false);
            return data[0] == 0x11;
        }

        private async Task SetScreen(ScreenState state, CancellationToken token)
        {
            await Executor.SwitchConnection.SendAsync(SwitchCommand.SetScreen(state, true), token).ConfigureAwait(false);
        }

        private void DisableOptions()
        {
            FetchButton.Enabled = false;
            Item1Value.Enabled = false;
            Item2Value.Enabled = false;
            Item3Value.Enabled = false;
            checkBox5.Enabled = false;
            checkBox6.Enabled = false;
            checkBox7.Enabled = false;
            FillingHoldTime.Enabled = false;
            NumberOfFillings.Enabled = false;
            StopConditionsButton.Enabled = false;
            EatOnStart.Enabled = false;
            EatAgain.Enabled = false;
            HoldIngredients.Enabled = false;
            ScreenOffBox.Enabled = false;
        }

        private void EnableOptions()
        {
            FetchButton.Enabled = true;
            Item1Value.Enabled = true;
            Item2Value.Enabled = true;
            Item3Value.Enabled = true;
            checkBox5.Enabled = true;
            checkBox6.Enabled = true;
            checkBox7.Enabled = true;
            FillingHoldTime.Enabled = true;
            NumberOfFillings.Enabled = true;
            StopConditionsButton.Enabled = true;
            EatOnStart.Enabled = true;
            EatAgain.Enabled = true;
            HoldIngredients.Enabled = true;
            ScreenOffBox.Enabled = true;
        }

        private static HttpClient? _client;
        private static HttpClient Client
        {
            get
            {
                _client ??= new HttpClient();
                return _client;
            }
        }

        private static string[]? DiscordWebhooks;

        private async void SendNotifications(string results, string thumbnail, bool pinguser)
        {
            if (string.IsNullOrEmpty(results) || string.IsNullOrEmpty(WebHookText.Text))
                return;
            DiscordWebhooks = WebHookText.Text.Split(',');
            if (DiscordWebhooks == null)
                return;
            var webhook = GenerateWebhook(results, thumbnail, pinguser);
            var content = new StringContent(JsonConvert.SerializeObject(webhook), Encoding.UTF8, "application/json");
            foreach (var url in DiscordWebhooks)
                await Client.PostAsync(url, content).ConfigureAwait(false);
        }

        private static object GenerateWebhook(string results, string thumbnail, bool pinguser)
        {
            string userContent = pinguser ? $"<@{Settings.Default.UserDiscordID}>" : "";
            string title = pinguser ? $"Match Found!" : "Unwanted match..";
            var WebHook = new
            {
                username = $"EggViewer.NET",
                content = userContent,
                embeds = new List<object>
                {
                    new
                    {
                        title = title,                        
                        thumbnail = new
                        {
                            url = thumbnail
                        },
                        fields = new List<object>
                        {
                            new { name = "Description               ", value = results, inline = true, },
                        },
                    }
                }
            };
            return WebHook;
        }

        private void SaveHookURL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(WebHookText.Text) || string.IsNullOrEmpty(UserDiscordIDText.Text))
            {
                MessageBox.Show("Please fill the fields before attempting to save.");
                return;
            }

            if (!string.IsNullOrEmpty(WebHookText.Text))
            {
                Settings.Default.WebHook = WebHookText.Text;
                Settings.Default.Save();
            }
            if (!string.IsNullOrEmpty(UserDiscordIDText.Text))
            {
                Settings.Default.UserDiscordID = UserDiscordIDText.Text;
                Settings.Default.Save();
            }

            MessageBox.Show("Done.");
        }

        private void StopConditionsButton_Click(object sender, EventArgs e)
        {
            using StopConditions miniform = new();
            miniform.ShowDialog();
        }

        private static int[] GrabIvFilters()
        {
            int[] ivsequence = Array.Empty<int>();
            int filters = Settings.Default.PresetIVS;
            switch (filters)
            {
                case 0: ivsequence = new[] { Settings.Default.HPFilter, Settings.Default.AtkFilter, Settings.Default.DefFilter, Settings.Default.SpaFilter, Settings.Default.SpdFilter, Settings.Default.SpeFilter }; break;
                case 1: ivsequence = new[] { 31, 31, 31, 31, 31, 31 }; break;
                case 2: ivsequence = new[] { 31, 0, 31, 31, 31, 0 }; break;
                case 3: ivsequence = new[] { 31, 0, 31, 31, 31, 31 }; break;
                case 4: ivsequence = new[] { 31, 31, 31, 31, 31, 0 }; break;
            }
            return ivsequence;
        }
    }
}
