// Discord Integration heavily reliant on SysCord.cs via SysBot.NET modified for this project
using Discord;
using Discord.Commands;
using Discord.Rest;
using Newtonsoft.Json;
using PKHeX.Core;
using PokeViewer.NET.Properties;
using SysBot.Base;
using static PokeViewer.NET.RoutineExecutor;
using static PokeViewer.NET.ViewerUtil;

namespace PokeViewer.NET.CommandsUtil
{
    public class CommandsUtil : ModuleBase<SocketCommandContext>
    {
        private ViewerExecutor? Executor = null;
        protected ViewerOffsets Offsets { get; } = new();
        private ViewerExecutor CheckExecutor()
        {
            if (Executor is null || !Executor.SwitchConnection.Connected)
            {
                var config = GetProtocol() switch
                {
                    SwitchProtocol.USB => new SwitchConnectionConfig { Port = int.Parse(Settings.Default.SwitchIP), Protocol = SwitchProtocol.USB },
                    SwitchProtocol.WiFi => new SwitchConnectionConfig { IP = Settings.Default.SwitchIP, Port = 6000, Protocol = SwitchProtocol.WiFi },
                    _ => throw new NotImplementedException(),
                };
                var state = new ViewerState
                {
                    Connection = config,
                    InitialRoutine = RoutineType.Read,
                };
                Executor = new ViewerExecutor(state);
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
            var bytes = await Executor.SwitchConnection.Screengrab(token).ConfigureAwait(false) ?? Array.Empty<byte>();
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

        [Command("dumpboxslot")]
        [Alias("dbs")]
        [Summary("Dumps the desired box slot to specified file type")]
        public async Task DumpBoxSlot(int box, int slot)
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            BoxViewerMode? BoxAssist = new(Settings.Default.GameConnected, Executor, default, new SimpleTrainerInfo());
            if (Settings.Default.GameConnected == (int)GameSelected.HOME)
            {
                await ReplyAsync("HOME dumping is not supported.").ConfigureAwait(false);
                return;
            }
            var result = await BoxAssist.SlotAssist(box - 1, slot - 1, token).ConfigureAwait(false);
            if (result.Item3.Species != (ushort)Species.None)
                await SendDumpedPKMAsync(Context.Channel, result.Item3).ConfigureAwait(false);
            else
                await ReplyAsync("No data present.").ConfigureAwait(false);
        }

        [Command("clearboxslot")]
        [Alias("cbs")]
        [Summary("Clears the desired box slot to specified file type")]
        public async Task ClearBoxSlot(int box, int slot)
        {
            Executor = CheckExecutor();
            BoxViewerMode? BoxAssist = new(Settings.Default.GameConnected, Executor, default, new SimpleTrainerInfo());
            if (Settings.Default.GameConnected == (int)GameSelected.HOME)
            {
                await ReplyAsync("HOME clearing is not supported.").ConfigureAwait(false);
                return;
            }
            PKM pk = new PK9();
            switch (Settings.Default.GameConnected)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: pk = new PK9(); break;
                case (int)GameSelected.LegendsArceus: pk = new PA8(); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: pk = new PB8(); break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield: pk = new PK8(); break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: pk = new PB7(); break;
            }
            var offset = await BoxAssist.ReturnBoxSlot(box - 1, slot - 1).ConfigureAwait(false);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(pk.EncryptedBoxData, offset, CancellationToken.None).ConfigureAwait(false);
            await ReplyAsync("Done.").ConfigureAwait(false);
        }

        [Command("clearbox")]
        [Alias("cb")]
        [Summary("Clears the desired box slot to specified file type")]
        public async Task ClearBox(int box)
        {
            Executor = CheckExecutor();
            BoxViewerMode? BoxAssist = new(Settings.Default.GameConnected, Executor, default, new SimpleTrainerInfo());
            if (Settings.Default.GameConnected == (int)GameSelected.HOME)
            {
                await ReplyAsync("HOME clearing is not supported.").ConfigureAwait(false);
                return;
            }
            PKM pk = new PK9();
            switch (Settings.Default.GameConnected)
            {
                case (int)GameSelected.Scarlet or (int)GameSelected.Violet: pk = new PK9(); break;
                case (int)GameSelected.LegendsArceus: pk = new PA8(); break;
                case (int)GameSelected.BrilliantDiamond or (int)GameSelected.ShiningPearl: pk = new PB8(); break;
                case (int)GameSelected.Sword or (int)GameSelected.Shield: pk = new PK8(); break;
                case (int)GameSelected.LetsGoPikachu or (int)GameSelected.LetsGoEevee: pk = new PB7(); break;
            }
            for (int i = 0; i < 30; i++)
            {
                var offset = await BoxAssist.ReturnBoxSlot(box - 1, i).ConfigureAwait(false);
                await Executor.SwitchConnection.WriteBytesAbsoluteAsync(pk.EncryptedBoxData, offset, CancellationToken.None).ConfigureAwait(false);
                await Task.Delay(0_100, CancellationToken.None).ConfigureAwait(false);
            }
            await ReplyAsync("Done.").ConfigureAwait(false);
        }

        [Command("readboxslot")]
        [Alias("rbs")]
        [Summary("Reads the specified box slot")]
        public async Task ReadSlot(int box, int slot)
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            BoxViewerMode? BoxAssist = new(Settings.Default.GameConnected, Executor, default, new SimpleTrainerInfo());
            var result = await BoxAssist.SlotAssist(box - 1, slot - 1, token).ConfigureAwait(false);
            if (result.Item3.Species != (ushort)Species.None)
            {
                var form = FormOutput(result.Item3.Species, result.Item3.Form, out _);
                string gender = string.Empty;
                var scaleS = (IScaledSize)result.Item3;
                string scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(scaleS.HeightScalar)} ({scaleS.HeightScalar})";
                switch (result.Item3.Gender)
                {
                    case 0: gender = " (M)"; break;
                    case 1: gender = " (F)"; break;
                    case 2: break;
                }
                string msg = "";
                bool hasMark = false;
                if (result.Item3 is PK8 or PK9)
                {
                    if (result.Item3 is PK8)
                    {
                        hasMark = HasMark((PK8)result.Item3, out RibbonIndex mark);
                        msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                    }
                    if (result.Item3 is PK9)
                    {
                        hasMark = HasMark((PK9)result.Item3, out RibbonIndex mark);
                        msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                    }
                }
                string egg = string.Empty;
                if (result.Item3.IsEgg)
                    egg = "\n**This Pokémon is an egg!**";
                string output = $"Nature: {(Nature)result.Item3.Nature}{Environment.NewLine}Ability: {GameInfo.GetStrings(1).Ability[result.Item3.Ability]}{Environment.NewLine}IVs: {result.Item3.IV_HP}/{result.Item3.IV_ATK}/{result.Item3.IV_DEF}/{result.Item3.IV_SPA}/{result.Item3.IV_SPD}/{result.Item3.IV_SPE}{Environment.NewLine}Ball: {(Ball)result.Item3.Ball}{Environment.NewLine}{scale}{msg}{egg}";
                var author = new EmbedAuthorBuilder
                {
                    IconUrl = result.Item2,
                    Name = $"Box {box} Slot {slot}"
                };
                var embed = new EmbedBuilder
                {
                    Title = $"{(result.Item3.ShinyXor == 0 ? "■ - " : result.Item3.ShinyXor <= 16 ? "★ - " : "")}{(Species)result.Item3.Species}{form}{gender}",
                    Color = Discord.Color.Blue,
                    ThumbnailUrl = result.Item1,
                    Description = output,
                }.WithAuthor(author);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
                await ReplyAsync($"No data present.").ConfigureAwait(false);
        }

        [Command("readbox")]
        [Alias("rb")]
        [Summary("Reads the specified box")]
        public async Task ReadBox(int box)
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            BoxViewerMode? BoxAssist = new(Settings.Default.GameConnected, Executor, default, new SimpleTrainerInfo());
            List<PKM> list = await BoxAssist.BoxRoutineAssist(box - 1, token).ConfigureAwait(false);

            var embed = new EmbedBuilder
            {
                Title = $"Displaying Box {box}",
                Color = Discord.Color.Blue
            };

            if (list.Count > 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    if (list[i].Species == (ushort)Species.None)
                        continue;

                    var form = FormOutput(list[i].Species, list[i].Form, out _);
                    string gender = string.Empty;
                    var scaleS = (IScaledSize)list[i];
                    string scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(scaleS.HeightScalar)} ({scaleS.HeightScalar})";

                    switch (list[i].Gender)
                    {
                        case 0: gender = " (M)"; break;
                        case 1: gender = " (F)"; break;
                        case 2: break;
                    }

                    string msg = "";
                    if (list[i] is PK8 or PK9)
                    {
                        bool hasMark = false;
                        if (list[i] is PK8)
                        {
                            HasMark((PK8)list[i], out RibbonIndex mark);
                            msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                        }
                        if (list[i] is PK9)
                        {
                            HasMark((PK9)list[i], out RibbonIndex mark);
                            msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                        }
                    }
                    string egg = string.Empty;
                    if (list[i].IsEgg)
                        egg = "\n**This Pokémon is an egg!**";
                    string output = $"{(list[i].ShinyXor == 0 ? "■ - " : list[i].ShinyXor <= 16 ? "★ - " : "")}{(Species)list[i].Species}{form}{gender}{Environment.NewLine}Nature: {(Nature)list[i].Nature}{Environment.NewLine}Ability: {GameInfo.GetStrings(1).Ability[list[i].Ability]}{Environment.NewLine}IVs: {list[i].IV_HP}/{list[i].IV_ATK}/{list[i].IV_DEF}/{list[i].IV_SPA}/{list[i].IV_SPD}/{list[i].IV_SPE}{Environment.NewLine}Ball: {(Ball)list[i].Ball}{Environment.NewLine}{scale}{msg}{egg}";

                    embed.AddField($"Slot {i + 1}", output, true);
                }
                await Context.Channel.SendMessageAsync("", false, embed.Build());

                if (list.Count >= 25)
                {
                    embed = new EmbedBuilder
                    {
                        Title = $"Displaying Box {box}",
                        Color = Discord.Color.Blue
                    };

                    for (int i = 25; i < list.Count; i++)
                    {
                        if (list[i].Species == (ushort)Species.None)
                            continue;

                        var form = FormOutput(list[i].Species, list[i].Form, out _);
                        string gender = string.Empty;
                        var scaleS = (IScaledSize)list[i];
                        string scale = $"Scale: {PokeSizeDetailedUtil.GetSizeRating(scaleS.HeightScalar)} ({scaleS.HeightScalar})";

                        switch (list[i].Gender)
                        {
                            case 0: gender = " (M)"; break;
                            case 1: gender = " (F)"; break;
                            case 2: break;
                        }

                        string msg = "";
                        if (list[i] is PK8 or PK9)
                        {
                            bool hasMark = false;
                            if (list[i] is PK8)
                            {
                                HasMark((PK8)list[i], out RibbonIndex mark);
                                msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                            }
                            if (list[i] is PK9)
                            {
                                HasMark((PK9)list[i], out RibbonIndex mark);
                                msg = hasMark ? $"{Environment.NewLine}Mark: {mark.ToString().Replace("Mark", "")}" : "";
                            }
                        }

                        string egg = string.Empty;
                        if (list[i].IsEgg)
                            egg = "\n**This Pokémon is an egg!**";
                        string output = $"{(list[i].ShinyXor == 0 ? "■ - " : list[i].ShinyXor <= 16 ? "★ - " : "")}{(Species)list[i].Species}{form}{gender}{Environment.NewLine}Nature: {(Nature)list[i].Nature}{Environment.NewLine}Ability: {GameInfo.GetStrings(1).Ability[list[i].Ability]}{Environment.NewLine}IVs: {list[i].IV_HP}/{list[i].IV_ATK}/{list[i].IV_DEF}/{list[i].IV_SPA}/{list[i].IV_SPD}/{list[i].IV_SPE}{Environment.NewLine}Ball: {(Ball)list[i].Ball}{Environment.NewLine}{scale}{msg}{egg}";

                        embed.AddField($"Slot {i + 1}", output, true);
                    }
                    if (embed.Fields.Count > 0)
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            else
                await ReplyAsync($"No data present.").ConfigureAwait(false);

        }

        [Command("injectboxslot", RunMode = RunMode.Async)]
        [Alias("ibs")]
        [Summary("Reads the specified box slot")]
        public async Task InjectBoxSlot(int box, int slot)
        {
            Executor = CheckExecutor();
            var token = CancellationToken.None;
            BoxViewerMode? BoxAssist = new(Settings.Default.GameConnected, Executor, default, new SimpleTrainerInfo());
            if (Settings.Default.GameConnected == (int)GameSelected.HOME)
            {
                await ReplyAsync("HOME injection is not supported.").ConfigureAwait(false);
                return;
            }
            var result = await BoxAssist.SlotAssist(box - 1, slot - 1, token).ConfigureAwait(false);

            if (result.Item3.Species != (ushort)Species.None)
            {
                RestUserMessage msg = await Context.Channel.SendMessageAsync($"Box {box} Slot {slot} contains a {(Species)result.Item3.Species}. Dump and continue with injection?").ConfigureAwait(false);
                IEmote[] reaction = { new Emoji("👍") };
                await msg.AddReactionsAsync(reaction).ConfigureAwait(false);
                for (int i = 0; i < 8; i++)
                {
                    await Task.Delay(1_000).ConfigureAwait(false);
                    await msg.UpdateAsync().ConfigureAwait(false);
                    if (msg.Reactions.Count > 1)
                        break;
                }
                if (msg.Reactions.Count < 1)
                {
                    await ReplyAsync("You did not reply before the timeout. Stopping execution.");
                    return;
                }
                await msg.ModifyAsync(x => x.Content = "Done.").ConfigureAwait(false);
                await SendDumpedPKMAsync(Context.Channel, result.Item3).ConfigureAwait(false);
            }

            var attachments = Context.Message.Attachments;
            foreach (var att in attachments)
            {
                string filetype = att.Filename[^3..];
                string url = att.Url;
                HttpClient client = new();
                var bytes = await client.GetByteArrayAsync(url).ConfigureAwait(false);
                EntityContext context = new();
                switch (filetype)
                {
                    case "PK9": context = EntityContext.Gen9; break;
                    case "PA8": context = EntityContext.Gen8a; break;
                    case "PB8": context = EntityContext.Gen8b; break;
                    case "PK8": context = EntityContext.Gen8; break;
                    case "PB7": context = EntityContext.Gen7b; break;
                }
                var pkm = EntityFormat.GetFromBytes(bytes, context);
                if (pkm == null)
                    await ReplyAsync($"The attachment failed to download.").ConfigureAwait(false);
                if (pkm != null)
                {
                    var la = new LegalityAnalysis(pkm!);
                    if (!la.Valid)
                    {
                        await ReplyAsync($"The attached {(Species)pkm.Species} is illegal.").ConfigureAwait(false);
                        RestUserMessage msg = await Context.Channel.SendMessageAsync($"The attached {(Species)pkm.Species} is illegal. Continue with injection?").ConfigureAwait(false);
                        IEmote[] reaction = { new Emoji("👍") };
                        await msg.AddReactionsAsync(reaction).ConfigureAwait(false);
                        for (int i = 0; i < 8; i++)
                        {
                            await Task.Delay(1_000).ConfigureAwait(false);
                            await msg.UpdateAsync().ConfigureAwait(false);
                            if (msg.Reactions.Count > 1)
                                break;
                        }
                        if (msg.Reactions.Count < 1)
                        {
                            await ReplyAsync("You did not reply before the timeout. Stopping execution.");
                            return;
                        }
                    }
                    var offset = await BoxAssist.ReturnBoxSlot(box - 1, slot - 1).ConfigureAwait(false);
                    await Executor.SwitchConnection.WriteBytesAbsoluteAsync(pkm.EncryptedBoxData, offset, CancellationToken.None).ConfigureAwait(false);
                    await ReplyAsync($"Done.").ConfigureAwait(false);
                    return;
                }
            }
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
            read.CopyTo(info.Data, 0);
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
            List<SwitchButton> Clicks = new();
            List<int> Delays = new();
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
                if (j.Name.ToUpper() == newseq.Name.ToUpper())
                {
                    await ReplyAsync("A custom sequence with this name already exists.");
                    return;
                }
            }
            newseq.Click = Clicks.ToArray();
            newseq.Delay = Delays.ToArray();
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
                if (j.Name.ToUpper() == name.ToUpper())
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
            List<string> list = new();
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
            List<SwitchButton> values = new();
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

        #region viaReusableActions
        private static async Task SendDumpedPKMAsync(IMessageChannel channel, PKM pkm)
        {
            string form = pkm.Form > 0 ? $"-{pkm.Form:00}" : string.Empty;
            string ballFormatted = string.Empty;
            string shinytype = string.Empty;
            string marktype = string.Empty;
            if (pkm.IsShiny)
            {
                if (pkm.Format >= 8 && (pkm.ShinyXor == 0 || pkm.FatefulEncounter || pkm.Version == (int)GameVersion.GO))
                    shinytype = " ■";
                else
                    shinytype = " ★";
            }

            string IVList = pkm.IV_HP + "." + pkm.IV_ATK + "." + pkm.IV_DEF + "." + pkm.IV_SPA + "." + pkm.IV_SPD + "." + pkm.IV_SPE;

            string TIDFormatted = pkm.Generation >= 7 ? $"{pkm.TrainerTID7:000000}" : $"{pkm.TID16:00000}";

            if (pkm.Ball != (int)Ball.None)
                ballFormatted = " - " + GameInfo.Strings.balllist[pkm.Ball].Split(' ')[0];

            string speciesName = SpeciesName.GetSpeciesNameGeneration(pkm.Species, (int)LanguageID.English, pkm.Format);
            if (pkm is IGigantamax gmax && gmax.CanGigantamax)
                speciesName += "-Gmax";

            string OTInfo = string.IsNullOrEmpty(pkm.OT_Name) ? "" : $" - {pkm.OT_Name} - {TIDFormatted}{ballFormatted}";

            if (pkm is PK9)
            {
                bool hasMark = HasMark((PK9)pkm, out RibbonIndex mark);
                if (hasMark)
                    marktype = hasMark ? $"{mark.ToString().Replace("Mark", "")}Mark - " : "";
            }
            if (pkm is PK8)
            {
                bool hasMark = HasMark((PK8)pkm, out RibbonIndex mark);
                if (hasMark)
                    marktype = hasMark ? $"{mark.ToString().Replace("Mark", "")}Mark - " : "";
            }
            string filename = $"{pkm.Species:000}{form}{shinytype} - {speciesName} - {marktype}{IVList}{OTInfo} - {pkm.EncryptionConstant:X8}";
            string filetype = "";
            if (pkm is PK8)
                filetype = ".pkm8";
            if (pkm is PB8)
                filetype = ".pb8";
            if (pkm is PA8)
                filetype = ".pa8";
            if (pkm is PK9)
                filetype = ".pk9";
            var tmp = Path.Combine(Path.GetTempPath(), filename + filetype);
            File.WriteAllBytes(tmp, pkm.DecryptedPartyData);
            await channel.SendFileAsync(tmp, "Here is your dumped file!").ConfigureAwait(false);
            File.Delete(tmp);
        }
        #endregion

        public class CustomSequence
        {
            public string Name { get; set; } = string.Empty;
            public SwitchButton[] Click { get; set; } = Array.Empty<SwitchButton>();
            public int[] Delay { get; set; } = Array.Empty<int>();
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
                List<Embed> embeds = new();
                var builder = new EmbedBuilder
                {
                    Color = new Discord.Color(114, 137, 218),
                    Description = "These are the commands you can use:",
                };

                foreach (var module in Service.Modules)
                {
                    string? description = null;
                    HashSet<string> mentioned = new();
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

                await ReplyAsync("Help has arrived!", false, null, null, null, null, null, null, embeds.ToArray()).ConfigureAwait(false);
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
