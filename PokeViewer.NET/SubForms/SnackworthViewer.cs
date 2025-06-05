using PKHeX.Core;
using Color = System.Drawing.Color;
using static System.Buffers.Binary.BinaryPrimitives;
using Octokit;
using SysBot.Base;

namespace PokeViewer.NET.SubForms
{
    public partial class SnackworthViewer : Form
    {
        private readonly ViewerState Executor;
        private static ulong BaseBlockKeyPointer = 0;
        protected ViewerOffsets Offsets { get; } = new();
        public SnackworthViewer(ViewerState executor)
        {
            InitializeComponent();
            Executor = executor;
        }

        private async void ModifyBtn_Click(object sender, EventArgs e)
        {
            ModifyBtn.Enabled = false;
            var token = CancellationToken.None;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            if (FezandipitiCheck.Checked)
            {
                var FezandipitiCheck = await ReadEncryptedBlockBool(Blocks.KCapturedFezandipiti, token).ConfigureAwait(false);
                if (!FezandipitiCheck)
                {
                    await WriteBlock(false, Blocks.KCapturedFezandipiti, token, true).ConfigureAwait(false);
                }
            }
            if (OkidogiCheck.Checked)
            {
                var OkidogiCheck = await ReadEncryptedBlockBool(Blocks.KCapturedOkidogi, token).ConfigureAwait(false);
                if (!OkidogiCheck)
                {
                    await WriteBlock(false, Blocks.KCapturedOkidogi, token, true).ConfigureAwait(false);
                }
            }
            if (MunkidoriCheck.Checked)
            {
                var MunkidoriCheck = await ReadEncryptedBlockBool(Blocks.KCapturedMunkidori, CancellationToken.None).ConfigureAwait(false);
                if (!MunkidoriCheck)
                {
                    await WriteBlock(false, Blocks.KCapturedMunkidori, token, true).ConfigureAwait(false);
                }
            }
            if (ArticunoCheck.Checked)
            {
                var (ArticunoCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateArticuno, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (ArticunoCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateArticuno, token, ArticunoCheck).ConfigureAwait(false);
                }
            }
            if (ZapdosCheck.Checked)
            {
                var (ZapdosCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateZapdos, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (ZapdosCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateZapdos, token, ZapdosCheck).ConfigureAwait(false);
                }
            }
            if (MoltresCheck.Checked)
            {
                var (MoltresCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateMoltres, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (MoltresCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateMoltres, token, MoltresCheck).ConfigureAwait(false);
                }
            }
            if (EnteiCheck.Checked)
            {
                var (EnteiCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateEntei, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (EnteiCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateEntei, token, EnteiCheck).ConfigureAwait(false);
                }
            }
            if (SuicuneCheck.Checked)
            {
                var (SuicuneCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateSuicune, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (SuicuneCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateSuicune, token, SuicuneCheck).ConfigureAwait(false);
                }
            }
            if (RaikouCheck.Checked)
            {
                var (RaikouCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateRaikou, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (RaikouCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateRaikou, token, RaikouCheck).ConfigureAwait(false);
                }
            }
            if (HoOhCheck.Checked)
            {
                var (HoOhCheck, _)= await ReadEncryptedBlockInt32(Blocks.KLegendaryStateHoOh, 0, token).ConfigureAwait(false);
                if (HoOhCheck is 0 or 2)
                    await WriteBlock(1, Blocks.KLegendaryStateHoOh, token, HoOhCheck).ConfigureAwait(false);
            }
            if (LugiaCheck.Checked)
            {
                var (LugiaCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateLugia, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (LugiaCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateLugia, token, LugiaCheck).ConfigureAwait(false);
                }
            }
            if (RayquazaCheck.Checked)
            {
                var (RayquazaCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateRayquaza, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (RayquazaCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateRayquaza, token, RayquazaCheck).ConfigureAwait(false);
                }
            }
            if (GroudonCheck.Checked)
            {
                var (GroudonCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateGroudon, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (GroudonCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateGroudon, token, GroudonCheck).ConfigureAwait(false);
                }
            }
            if (KyogreCheck.Checked)
            {
                var (KyogreCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateKyogre, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (KyogreCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateKyogre, token, KyogreCheck).ConfigureAwait(false);
                }
            }
            if (LatiosCheck.Checked)
            {
                var (LatiosCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateLatios, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (LatiosCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateLatios, token, LatiosCheck).ConfigureAwait(false);
                }
            }
            if (LatiasCheck.Checked)
            {
                var (LatiasCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateLatias, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (LatiasCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateLatias, token, LatiasCheck).ConfigureAwait(false);
                }
            }
            if (KyuremCheck.Checked)
            {
                var (KyuremCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateKyurem, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (KyuremCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateKyurem, token, KyuremCheck).ConfigureAwait(false);
                }
            }
            if (ZekromCheck.Checked)
            {
                var (ZekromCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateZekrom, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (ZekromCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateZekrom, token, ZekromCheck).ConfigureAwait(false);
                }
            }
            if (ReshiramCheck.Checked)
            {
                var (ReshiramCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateReshiram, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (ReshiramCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateReshiram, token, ReshiramCheck).ConfigureAwait(false);
                }
            }
            if (CobalionCheck.Checked)
            {
                var (CobalionCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateCobalion, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (CobalionCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateCobalion, token, CobalionCheck).ConfigureAwait(false);
                }
            }
            if (TerrakionCheck.Checked)
            {
                var (MoltresCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateTerrakion, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (MoltresCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateTerrakion, token, MoltresCheck).ConfigureAwait(false);
                }
            }
            if (VirizionCheck.Checked)
            {
                var (VirizionCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateVirizion, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (VirizionCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateVirizion, token, VirizionCheck).ConfigureAwait(false);
                }
            }
            if (WoChienCheck.Checked)
            {
                var (WoChienCheck, _) = await ReadEncryptedBlockInt32(Blocks.KShrineStateWoChien, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (WoChienCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KShrineStateWoChien, token, WoChienCheck).ConfigureAwait(false);
                }
            }
            if (ChienPaoCheck.Checked)
            {
                var (ChienPaoCheck, _) = await ReadEncryptedBlockInt32(Blocks.KShrineStateWoChien, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (ChienPaoCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KShrineStateWoChien, token, ChienPaoCheck).ConfigureAwait(false);
                }
            }
            if (TingLuCheck.Checked)
            {
                var (TingLuCheck, _) = await ReadEncryptedBlockInt32(Blocks.KShrineStateTinglu, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (TingLuCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KShrineStateTinglu, token, TingLuCheck).ConfigureAwait(false);
                }
            }
            if (ChiYuCheck.Checked)
            {
                var (ChiYuCheck, _) = await ReadEncryptedBlockInt32(Blocks.KShrineStateChiYu, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (ChiYuCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KShrineStateChiYu, token, ChiYuCheck).ConfigureAwait(false);
                }
            }
            if (NecrozmaCheck.Checked)
            {
                var (NecrozmaCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateNecrozma, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (NecrozmaCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateNecrozma, token, NecrozmaCheck).ConfigureAwait(false);
                }
            }
            if (SolgaleoCheck.Checked)
            {
                var (SolgaleoCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateSolgaleo, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (SolgaleoCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateSolgaleo, token, SolgaleoCheck).ConfigureAwait(false);
                }
            }
            if (LunalaCheck.Checked)
            {
                var (LunalaCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateLunala, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (LunalaCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateLunala, token, LunalaCheck).ConfigureAwait(false);
                }
            }
            if (KubfuCheck.Checked)
            {
                var (KubfuCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateKubfu, 0, token).ConfigureAwait(false);
                if (KubfuCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateKubfu, token, KubfuCheck).ConfigureAwait(false);
                }
            }
            if (GlastrierCheck.Checked)
            {
                var (GlastrierCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateGlastrier, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (GlastrierCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateGlastrier, token, GlastrierCheck).ConfigureAwait(false);
                }
            }
            if (SpectrierCheck.Checked)
            {
                var (SpectrierCheck, _) = await ReadEncryptedBlockInt32(Blocks.KLegendaryStateSpectrier, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (SpectrierCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KLegendaryStateSpectrier, token, SpectrierCheck).ConfigureAwait(false);
                }
            }
            if (MeloettaCheck.Checked)
            {
                var (MeloettaCheck, _) = await ReadEncryptedBlockInt32(Blocks.KMeloettaStatus, BaseBlockKeyPointer, CancellationToken.None).ConfigureAwait(false);
                if (MeloettaCheck is 0 or 2)
                {
                    await WriteBlock(1, Blocks.KMeloettaStatus, token, MeloettaCheck).ConfigureAwait(false);
                }
            }
            ModifyBtn.Enabled = true;
            MessageBox.Show("Done.");
        }

        // Read, Decrypt, and Write Block tasks from Tera-Finder/RaidCrawler/sv-livemap.
        #region saveblocktasks
        public static byte[] DecryptBlock(uint key, byte[] block)
        {
            var rng = new SCXorShift32(key);
            for (int i = 0; i < block.Length; i++)
                block[i] = (byte)(block[i] ^ rng.Next());
            return block;
        }

        public async Task<(byte, ulong)> ReadEncryptedBlockByte(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (header[1], address);
        }

        public async Task<(byte[], ulong)> ReadEncryptedBlockHeader(DataBlock block, ulong init, CancellationToken token)
        {
            if (init == 0)
            {
                var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
                init = address;
            }

            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(init, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);

            return (header, init);
        }

        public async Task<(byte[]?, ulong)> ReadEncryptedBlockArray(DataBlock block, ulong init, CancellationToken token)
        {
            if (init == 0)
            {
                var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
                init = address;
            }

            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(init, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return (data[6..], init);
        }

        public async Task<(uint, ulong)> ReadEncryptedBlockUint(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadUInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        public async Task<(int, ulong)> ReadEncryptedBlockInt32(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        public async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data);
            return res[0] == 2;
        }

        public async Task<byte[]> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return data[6..];
        }

        public async Task<byte[]?> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            var size = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5 + (int)size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data)[5..];

            return res;
        }

        public async Task<bool> WriteBlock(object data, DataBlock block, CancellationToken token, object? toExpect = default)
        {
            if (block.IsEncrypted)
                return await WriteEncryptedBlockSafe(block, toExpect, data, token).ConfigureAwait(false);
            else
                return await WriteDecryptedBlock((byte[])data!, block, token).ConfigureAwait(false);
        }

        public async Task<bool> WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            await Executor.SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> WriteEncryptedBlockSafe(DataBlock block, object? toExpect, object toWrite, CancellationToken token)
        {
            if (toExpect == default || toWrite == default)
                return false;

            return block.Type switch
            {
                SCTypeCode.Array => await WriteEncryptedBlockArray(block, (byte[])toExpect, (byte[])toWrite, token).ConfigureAwait(false),
                SCTypeCode.Bool1 or SCTypeCode.Bool2 or SCTypeCode.Bool3 => await WriteEncryptedBlockBool(block, (bool)toExpect, (bool)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Byte or SCTypeCode.SByte => await WriteEncryptedBlockByte(block, (byte)toExpect, (byte)toWrite, token).ConfigureAwait(false),
                SCTypeCode.UInt32 or SCTypeCode.UInt64 => await WriteEncryptedBlockUint(block, (uint)toExpect, (uint)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Int32 => await WriteEncryptedBlockInt32(block, (int)toExpect, (int)toWrite, token).ConfigureAwait(false),
                _ => throw new NotSupportedException($"Block {block.Name} (Type {block.Type}) is currently not supported.")
            };
        }

        public async Task<bool> WriteEncryptedBlockUint(DataBlock block, uint valueToExpect, uint valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteUInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> WriteEncryptedBlockInt32(DataBlock block, int valueToExpect, int valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> WriteEncryptedBlockByte(DataBlock block, byte valueToExpect, byte valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = header[1];
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            header[1] = valueToInject;
            header = EncryptBlock(block.Key, header);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> WriteEncryptedBlockArray(DataBlock block, byte[] arrayToExpect, byte[] arrayToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[6..];
            if (!ram.SequenceEqual(arrayToExpect)) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            Array.ConstrainedCopy(arrayToInject, 0, data, 6, block.Size);
            data = EncryptBlock(block.Key, data);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
        {
            ulong address;
            try
            {
                address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
                address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[0] == 2;
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            data[0] = valueToInject ? (byte)2 : (byte)1;
            data = EncryptBlock(block.Key, data);
            await Executor.SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);

            return true;
        }

        public static byte[] EncryptBlock(uint key, byte[] block) => DecryptBlock(key, block);

        public async Task<ulong> SearchSaveKey(uint key, CancellationToken token)
        {
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(BaseBlockKeyPointer + 8, 16, token).ConfigureAwait(false);
            var start = BitConverter.ToUInt64(data.AsSpan()[..8]);
            var end = BitConverter.ToUInt64(data.AsSpan()[8..]);

            while (start < end)
            {
                var block_ct = (end - start) / 48;
                var mid = start + (block_ct >> 1) * 48;

                data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(mid, 4, token).ConfigureAwait(false);
                var found = BitConverter.ToUInt32(data);
                if (found == key)
                    return mid;

                if (found >= key)
                    end = mid;
                else start = mid + 48;
            }
            return start;
        }
        #endregion
    }
}
