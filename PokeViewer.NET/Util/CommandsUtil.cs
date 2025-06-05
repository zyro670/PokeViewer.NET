using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using PKHeX.Core;
using PokeViewer.NET;
using PokeViewer.NET.Properties;
using SysBot.Base;
using static PokeViewer.NET.ViewerUtil;

namespace PokéViewer.NET.Util
{
    public class CommandsUtil : ModuleBase<SocketCommandContext>
    {
        private ViewerState? Executor;
        protected ViewerOffsets Offsets { get; } = new();
        private ViewerState CheckExecutor()
        {
            if (Executor is null || !Executor.SwitchConnection.Connected)
            {
                var config = GetProtocol() switch
                {
                    SwitchProtocol.USB => new SwitchConnectionConfig { Port = int.Parse(Settings.Default.SwitchIP), Protocol = SwitchProtocol.USB },
                    SwitchProtocol.WiFi => new SwitchConnectionConfig { IP = Settings.Default.SwitchIP, Port = 6000, Protocol = SwitchProtocol.WiFi },
                    _ => throw new NotImplementedException(),
                };
				var state = new ViewerExecutorBase
				{
					Connection = config,
					InitialRoutine = RoutineType.Read,
				};
				Executor = new ViewerState(state);
                Executor.SwitchConnection.Connect();
            }
            return Executor;
        }

        private static SwitchProtocol GetProtocol()
        {
            if (!Settings.Default.UseWiFiProtocol)
                return SwitchProtocol.USB;
            return SwitchProtocol.WiFi;
        }

        [Command("ping")]
        public async Task Hello()
        {
            await ReplyAsync("Pong!").ConfigureAwait(false);
        }

        [Command("addsudo")]
        public async Task AddSudoUser(ulong id)
        {
            var path = "refs\\sudo.txt";
            string contents = File.ReadAllText(path);
            if (contents.Last().Equals(','))
                File.WriteAllText(path, contents + id + ",");
            else
                File.WriteAllText(path, contents + "," + id + ",");
            await ReplyAsync($"{id} added as a Sudo User.").ConfigureAwait(false);
        }

        [Command("removesudo")]
        public async Task RemoveSudoUser(ulong id)
        {
            var path = "refs\\sudo.txt";
            string contents = File.ReadAllText(path);
            string newcontents = string.Empty;
            var sudos = contents.Split(',');
            foreach (var s in sudos)
            {
                if (s.Equals(id.ToString()))
                    newcontents = contents.Replace(id.ToString() + ',', "");
            }

            File.WriteAllText(path, newcontents);
            await ReplyAsync($"{id} removed as a Sudo User.").ConfigureAwait(false);
        }

        [Command("timeskipfwd")]
        [Alias("tsf")]
        public async Task TimeSkipFwd(int val)
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            for (int i = 0; i < val; i++)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipForward(true), token).ConfigureAwait(false);
            await ReplyAsync($"Time skipped forward {val} times.").ConfigureAwait(false);
        }

        [Command("timeskipbwd")]
        [Alias("tsb")]
        public async Task TimeSkipBwd(int val)
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            for (int i = 0; i < val; i++)
                await Executor.SwitchConnection.SendAsync(SwitchCommand.TimeSkipBack(true), token).ConfigureAwait(false);
            await ReplyAsync($"Time skipped backward {val} times.").ConfigureAwait(false);
        }

        [Command("reopen")]
        [Summary("Closes and reopens the game of the connected Switch.")]
        public async Task ReopenGame()
        {
            var token = CancellationToken.None;
            await ReplyAsync("Initiating clicks...").ConfigureAwait(false);
            await Click(SwitchButton.B, 0_500, token).ConfigureAwait(false);
            await Click(SwitchButton.HOME, 2_000, token).ConfigureAwait(false);
            await Click(SwitchButton.X, 2_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 3_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 2_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 3_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 2_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 16_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 18_000, token).ConfigureAwait(false);
            await ReplyAsync($"Game has been closed and reopened.").ConfigureAwait(false);
        }

        [Command("savegamesv")]
        [Alias("sgsv")]
        [Summary("Saves the game of the connected Switch.")]
        public async Task SaveGameSV()
        {
            var token = CancellationToken.None;
            await ReplyAsync("Initiating clicks...").ConfigureAwait(false);
            await Click(SwitchButton.B, 0_500, token).ConfigureAwait(false);
            await Click(SwitchButton.X, 2_500, token).ConfigureAwait(false);
            await Click(SwitchButton.R, 2_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 6_000, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 4_000, token).ConfigureAwait(false);
            await Click(SwitchButton.B, 1_000, token).ConfigureAwait(false);
            await ReplyAsync($"Game has been saved.").ConfigureAwait(false);
        }

        [Command("sequence")]
        [Alias("seq")]
        [Summary("Performs a set of clicks as labeled by the sequence")]
        public async Task SequenceClicks(string seq, int delay)
        {
            var token = CancellationToken.None;
            var cmds = seq.Split(',');
            var clicks = ReturnCommands(cmds);
            foreach (var c in clicks)
            {
                await ReplyAsync($"Clicking: {c}..").ConfigureAwait(false);
                await Click(c, delay, token).ConfigureAwait(false);
            }
            await ReplyAsync("Done.").ConfigureAwait(false);
        }

        [Command("peek")]
        [Summary("Take and send a screenshot from the specified Switch.")]
        public async Task Peek()
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            var bytes = await Executor.SwitchConnection.PixelPeek(token).ConfigureAwait(false) ?? [];
            if (bytes.Length == 1)
            {
                await ReplyAsync($"Failed to take a screenshot. Is the bot connected?").ConfigureAwait(false);
                return;
            }
            MemoryStream ms = new(bytes);

            var img = "cap.jpg";
            var embed = new EmbedBuilder { ImageUrl = $"attachment://{img}", Color = Discord.Color.Blue }.WithFooter(new EmbedFooterBuilder { Text = $"Captured image from bot at address {Settings.Default.SwitchIP}." });
            await Context.Channel.SendFileAsync(ms, img, "", false, embed: embed.Build());
        }

        [Command("sleep")]
        [Summary("Puts the connected Switch to sleep.")]
        public async Task Sleep()
        {
            var token = CancellationToken.None;
            await Click(SwitchButton.B, 0_500, token).ConfigureAwait(false);
            await PressAndHold(SwitchButton.HOME, 2_000, 0, token).ConfigureAwait(false);
            await Click(SwitchButton.A, 1_000, token).ConfigureAwait(false);
            await ReplyAsync($"Switch has been put to sleep. Exiting application.").ConfigureAwait(false);
            Executor!.Disconnect();
            Application.Exit();
        }

        [Command("viewtradepartner")]
        [Alias("vtp")]
        [Summary("Views current trade partner info")]
        public async Task ViewTradePartner()
        {
            var token = CancellationToken.None;
            if (Settings.Default.GameConnected is not (int)GameSelected.Scarlet or (int)GameSelected.Violet)
            {
                await ReplyAsync("Trade Partner viewing is not supported for this version.").ConfigureAwait(false);
                return;
            }
            Executor = CheckExecutor();
            var sav = new SAV9SV();
            var info = sav.MyStatus;
            var read = await Executor.SwitchConnection.PointerPeek(info.Data.Length, Offsets.TradePartnerSV, token).ConfigureAwait(false);
            read.CopyTo(info.Data);
            var nidOffset = await Executor.SwitchConnection.PointerAll(Offsets.TradePartnerNIDSV, token).ConfigureAwait(false);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(nidOffset, 8, token).ConfigureAwait(false);
            var nid = BitConverter.ToUInt64(data, 0);
            await ReplyAsync($"Trainer: {StringConverter8.GetString(read.AsSpan(8, 24))} ({sav.TrainerSID7}/{sav.TrainerTID7}) - NID: {nid}").ConfigureAwait(false);
        }

        [Command("addcustomsequence")]
        [Alias("acs", "addcs")]
        [Summary("Adds a custom sequence to a JSON file")]
        public async Task AddCustomSequence(string name, string clicks, string delays)
        {
            var path = "refs\\customsequence.json";
            var json = File.ReadAllText(path);
            var jsonData = JsonConvert.DeserializeObject<List<CustomSequence>>(json)!;

            var sequenceclicks = clicks.Split(',');
            var sc = ReturnCommands(sequenceclicks);
            var sequencedelays = delays.Split(',');
            List<SwitchButton> Clicks = [];
            List<int> Delays = [];
            foreach (var sequence in sc)
            {
                Clicks.Add(sequence);
            }
            foreach (var sd in sequencedelays)
            {
                Delays.Add(Convert.ToInt32(sd));
            }
            CustomSequence newseq = new()
            {
                Name = name,
            };

            foreach (var j in jsonData)
            {
                if (j.Name.Equals(newseq.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    await ReplyAsync("A custom sequence with this name already exists.");
                    return;
                }
            }
            newseq.Click = [.. Clicks];
            newseq.Delay = [.. Delays];
            jsonData.Add(newseq);
            json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            File.WriteAllText(path, json);
            await ReplyAsync($"Custom Sequence {name} has been added to the list.");
        }

        [Command("removecustomsequence")]
        [Alias("rcs", "removecs")]
        [Summary("Removes a custom sequence to a JSON file")]
        public async Task RemoveCustomSequence(string name)
        {
            var path = "refs\\customsequence.json";
            var json = File.ReadAllText(path);
            var jsonData = JsonConvert.DeserializeObject<List<CustomSequence>>(json)!;

            foreach (var j in jsonData)
            {
                if (j.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    jsonData.Remove(j);
                    json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                    File.WriteAllText(path, json);
                    await ReplyAsync($"Custom Sequence {name} has been removed.");
                    return;
                }
            }
            await ReplyAsync($"Sequence {name} not found.");
        }

        [Command("performcustomsequence")]
        [Alias("pcs", "performcs")]
        [Summary("Removes a custom sequence to a JSON file")]
        public async Task PerformCustomSequence(string name)
        {
            var path = "refs\\customsequence.json";
            var json = File.ReadAllText(path);
            var jsonData = JsonConvert.DeserializeObject<List<CustomSequence>>(json)!;
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            foreach (var j in jsonData)
            {
                if (j.Name.ToUpper() == name.ToUpper())
                {
                    for (int c = 0; c < j.Click.Length; c++)
                    {
                        await ReplyAsync($"Clicking: {j.Click[c]}...").ConfigureAwait(false);
                        await Click(j.Click[c], j.Delay[c], token).ConfigureAwait(false);
                    }
                    await ReplyAsync($"Custom Sequence {name} is completed.");
                    return;
                }
            }
            await ReplyAsync($"Custom Sequence {name} not found.");
        }

        [Command("sequencelist")]
        [Alias("sl")]
        [Summary("Lists all custom sequences from the JSON file")]
        public async Task ListSequences()
        {
            var embed = new EmbedBuilder();
            var path = "refs\\customsequence.json";
            var json = File.ReadAllText(path);
            var jsonData = JsonConvert.DeserializeObject<List<CustomSequence>>(json)!;
            if (jsonData.Count is 0)
            {
                await ReplyAsync("No data found in the list.").ConfigureAwait(false);
                return;
            }
            List<string> list = [];
            foreach (var j in jsonData)
            {
                string res = string.Join($",", j.Click.ToList());
                string del = string.Join($",", j.Delay.ToList());
                list.Add($"Sequence {j.Name} - Click: {res} | Delay: {del}{Environment.NewLine}");
            }

            string msg = string.Join("", list.ToList());
            embed.AddField(x =>
            {
                x.Name = "Sequence List";
                x.Value = msg;
                x.IsInline = false;
            });
            await ReplyAsync("Here's your list of sequences!", embed: embed.Build()).ConfigureAwait(false);
        }

        private async Task Click(SwitchButton b, int delay, CancellationToken token)
        {
            Executor = CheckExecutor();
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Click(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task PressAndHold(SwitchButton b, int hold, int delay, CancellationToken token)
        {
            Executor = CheckExecutor();
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Hold(b, true), token).ConfigureAwait(false);
            await Task.Delay(hold, token).ConfigureAwait(false);
            await Executor.SwitchConnection.SendAsync(SwitchCommand.Release(b, true), token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private async Task SetStick(SwitchStick stick, short y, int delay, CancellationToken token)
        {
            Executor = CheckExecutor();
            var cmd = SwitchCommand.SetStick(stick, 0, y, true);
            await Executor.SwitchConnection.SendAsync(cmd, token).ConfigureAwait(false);
            await Task.Delay(delay, token).ConfigureAwait(false);
        }

        private static List<SwitchButton> ReturnCommands(string[] cmds)
        {
            List<SwitchButton> values = [];
            for (int i = 0; i < cmds.Length; i++)
            {
                switch (cmds[i].ToUpper())
                {
                    case "A": values.Add(SwitchButton.A); break;
                    case "B": values.Add(SwitchButton.B); break;
                    case "X": values.Add(SwitchButton.X); break;
                    case "Y": values.Add(SwitchButton.Y); break;
                    case "RSTICK": values.Add(SwitchButton.RSTICK); break;
                    case "LSTICK": values.Add(SwitchButton.LSTICK); break;
                    case "R": values.Add(SwitchButton.R); break;
                    case "L": values.Add(SwitchButton.L); break;
                    case "ZL": values.Add(SwitchButton.ZL); break;
                    case "ZR": values.Add(SwitchButton.ZR); break;
                    case "PLUS": values.Add(SwitchButton.PLUS); break;
                    case "MINUS": values.Add(SwitchButton.MINUS); break;
                    case "DUP": values.Add(SwitchButton.DUP); break;
                    case "DDOWN": values.Add(SwitchButton.DDOWN); break;
                    case "DLEFT": values.Add(SwitchButton.DLEFT); break;
                    case "DRIGHT": values.Add(SwitchButton.DRIGHT); break;
                    case "HOME": values.Add(SwitchButton.HOME); break;
                    case "CAPTURE": values.Add(SwitchButton.CAPTURE); break;
                }
            }
            return values;
        }

        public class CustomSequence
        {
            public string Name { get; set; } = string.Empty;
            public SwitchButton[] Click { get; set; } = [];
            public int[] Delay { get; set; } = [];
        }

        public class HelpModule : ModuleBase<SocketCommandContext>
        {
            private readonly CommandService Service;

            public HelpModule(CommandService service)
            {
                Service = service;
            }

            [Command("help")]
            [Summary("Lists available commands.")]
            public async Task HelpAsync()
            {
                List<Embed> embeds = [];
                var builder = new EmbedBuilder
                {
                    Color = new Discord.Color(114, 137, 218),
                    Description = "These are the commands you can use:",
                };

                foreach (var module in Service.Modules)
                {
                    string? description = null;
                    HashSet<string> mentioned = [];
                    foreach (var cmd in module.Commands)
                    {
                        var name = cmd.Name;
                        if (mentioned.Contains(name))
                            continue;

                        mentioned.Add(name);
                        var result = await cmd.CheckPreconditionsAsync(Context).ConfigureAwait(false);
                        if (result.IsSuccess)
                            description += $"{cmd.Aliases[0]}\n";
                    }
                    if (string.IsNullOrWhiteSpace(description))
                        continue;

                    var moduleName = module.Name;
                    var gen = moduleName.IndexOf('`');
                    if (gen != -1)
                        moduleName = moduleName[..gen];

                    if (builder.Fields.Count == 25)
                    {
                        embeds.Add(builder.Build());
                        builder.Fields.Clear();
                        builder.Description = string.Empty;
                    }

                    builder.AddField(x =>
                    {
                        x.Name = moduleName;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }

                if (builder.Fields.Count > 0)
                    embeds.Add(builder.Build());

                await ReplyAsync("Help has arrived!", false, null, null, null, null, null, null, [.. embeds]).ConfigureAwait(false);
            }

            [Command("help")]
            [Summary("Lists information about a specific command.")]
            public async Task HelpAsync([Summary("The command you want help for")] string command)
            {
                var result = Service.Search(Context, command);

                if (!result.IsSuccess)
                {
                    await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.").ConfigureAwait(false);
                    return;
                }

                var builder = new EmbedBuilder
                {
                    Color = new Discord.Color(114, 137, 218),
                    Description = $"Here are some commands like **{command}**:",
                };

                foreach (var match in result.Commands)
                {
                    var cmd = match.Command;

                    builder.AddField(x =>
                    {
                        x.Name = string.Join(", ", cmd.Aliases);
                        x.Value = GetCommandSummary(cmd);
                        x.IsInline = false;
                    });
                }

                await ReplyAsync("Help has arrived!", false, builder.Build()).ConfigureAwait(false);
            }

            private static string GetCommandSummary(CommandInfo cmd)
            {
                return $"Summary: {cmd.Summary}\nParameters: {GetParameterSummary(cmd.Parameters)}";
            }

            private static string GetParameterSummary(IReadOnlyList<ParameterInfo> p)
            {
                if (p.Count == 0)
                    return "None";
                return $"{p.Count}\n- " + string.Join("\n- ", p.Select(GetParameterSummary));
            }

            private static string GetParameterSummary(ParameterInfo z)
            {
                var result = z.Name;
                if (!string.IsNullOrWhiteSpace(z.Summary))
                    result += $" ({z.Summary})";
                return result;
            }
        }
    }
}
