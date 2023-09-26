using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokeViewer.NET.SubForms
{
    public partial class VivillonEditor : Form
    {
        private readonly ViewerExecutor Executor;
        public VivillonEditor(ViewerExecutor executor)
        {
            InitializeComponent();
            Executor = executor;
        }
        private static ulong BaseBlockKeyPointer = 0;
        protected ViewerOffsets Offsets { get; } = new();
        private int V_Form;

        public class DataBlock
        {
            public string? Name { get; set; }
            public uint Key { get; set; }
            public SCTypeCode Type { get; set; }
            public SCTypeCode SubType { get; set; }
            public IReadOnlyList<long>? Pointer { get; set; }
            public bool IsEncrypted { get; set; }
            public int Size { get; set; }
        }

        public static class Blocks
        {
            public static DataBlock KGOVivillonFormEnabled = new()
            {
                Name = "KGOVivillonFormEnabled",
                Key = 0x0C125D5C,
                Type = SCTypeCode.Bool1,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KGOTransfer = new()
            {
                Name = "KGOTransfer",
                Key = 0x7EE0A576,
                Type = SCTypeCode.Object,
                IsEncrypted = true,
                Size = 0x3400,
            };
            public static DataBlock FSYS_GO_LINK_ENABLED = new()
            {
                Name = "FSYS_GO_LINK_ENABLED",
                Key = 0x3ABC21E3,
                Type = SCTypeCode.Bool1,
                IsEncrypted = true,
                Size = 1,
            };

            public static DataBlock KGOVivillonForm = new()
            {
                Name = "KGOVivillonForm",
                Key = 0x22F70BCF,
                Type = SCTypeCode.Byte,
                IsEncrypted = true,
                Size = 1,
            };
            public static DataBlock KGOLastConnected = new()
            {
                Name = "KGOLastConnected",
                Key = 0x867F0240,
                Type = SCTypeCode.UInt64,
                IsEncrypted = true,
                Size = 8,
            };
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

        private async Task<(byte, ulong)> ReadEncryptedBlockByte(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (header[1], address);
        }

        private async Task<(byte[], ulong)> ReadEncryptedBlockHeader(DataBlock block, ulong init, CancellationToken token)
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

        private async Task<(byte[]?, ulong)> ReadEncryptedBlockArray(DataBlock block, ulong init, CancellationToken token)
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

        private async Task<(uint, ulong)> ReadEncryptedBlockUint(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadUInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        private async Task<(int, ulong)> ReadEncryptedBlockInt32(DataBlock block, ulong init, CancellationToken token)
        {
            var (header, address) = await ReadEncryptedBlockHeader(block, init, token).ConfigureAwait(false);
            return (ReadInt32LittleEndian(header.AsSpan()[1..]), address);
        }

        private async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = DecryptBlock(block.Key, data);
            return res[0] == 2;
        }

        private async Task<byte[]> ReadBlock(DataBlock block, CancellationToken token)
        {
            return await ReadEncryptedBlock(block, token).ConfigureAwait(false);
        }

        private async Task<byte[]> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            var address = await SearchSaveKey(block.Key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            var data = await Executor.SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = DecryptBlock(block.Key, data);

            return data[6..];
        }

        private async Task<byte[]?> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
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

        private async Task<bool> WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            await Executor.SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> WriteEncryptedBlockSafe(DataBlock block, object? toExpect, object toWrite, CancellationToken token)
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

        private async Task<bool> WriteEncryptedBlockUint(DataBlock block, uint valueToExpect, uint valueToInject, CancellationToken token)
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

        private async Task<bool> WriteEncryptedBlockInt32(DataBlock block, int valueToExpect, int valueToInject, CancellationToken token)
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

        private async Task<bool> WriteEncryptedBlockByte(DataBlock block, byte valueToExpect, byte valueToInject, CancellationToken token)
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

        private async Task<bool> WriteEncryptedBlockArray(DataBlock block, byte[] arrayToExpect, byte[] arrayToInject, CancellationToken token)
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

        private async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
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

        private async void button1_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var forcevivform = await ReadEncryptedBlockBool(Blocks.KGOVivillonFormEnabled, token).ConfigureAwait(false);
            var (vivform, _) = await ReadEncryptedBlockByte(Blocks.KGOVivillonForm, 0, token).ConfigureAwait(false);
            var (epochtime, _) = await ReadEncryptedBlockUint(Blocks.KGOLastConnected, 0, token).ConfigureAwait(false);

            MessageBox.Show($"KGOVivillonFormEnabled: {forcevivform}{Environment.NewLine}" +
                $"KGOVivillonForm: {(VivForms)vivform}{Environment.NewLine}KGOLastConnected TimeStamp: {FromUnixTime(epochtime)}");
        }

        private void V_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = comboBox1.SelectedIndex;
            switch (selection)
            {
                case 0: V_Form = (int)VivForms.IcySnow; break; // Icy Snow
                case 1: V_Form = (int)VivForms.Polar; break; // Polar
                case 2: V_Form = (int)VivForms.Tundra; break; // Tundra
                case 3: V_Form = (int)VivForms.Continental; break; // Continental
                case 4: V_Form = (int)VivForms.Garden; break; // Garden
                case 5: V_Form = (int)VivForms.Elegant; break; // Elegant
                case 6: V_Form = (int)VivForms.Meadow; break; // Meadow
                case 7: V_Form = (int)VivForms.Modern; break; // Modern
                case 8: V_Form = (int)VivForms.Marine; break; // Marine
                case 9: V_Form = (int)VivForms.Archipelago; break; // Archipelago
                case 10: V_Form = (int)VivForms.HighPlains; break; // High-Plains
                case 11: V_Form = (int)VivForms.Sandstorm; break;// Sandstorm
                case 12: V_Form = (int)VivForms.River; break; // River
                case 13: V_Form = (int)VivForms.Monsoon; break; // Monsoon
                case 14: V_Form = (int)VivForms.Savanna; break; // Savanna
                case 15: V_Form = (int)VivForms.Sun; break; // Sun
                case 16: V_Form = (int)VivForms.Ocean; break; // Ocean
                case 17: V_Form = (int)VivForms.Jungle; break; // Jungle
                case 18: V_Form = (int)VivForms.Fancy; break; // Fancy
            }
        }

        private static readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var token = CancellationToken.None;
            BaseBlockKeyPointer = await Executor.SwitchConnection.PointerAll(Offsets.BlockKeyPointer, token).ConfigureAwait(false);
            var (vivform, _) = await ReadEncryptedBlockByte(Blocks.KGOVivillonForm, 0, token).ConfigureAwait(false);
            var (epochtime, _) = await ReadEncryptedBlockUint(Blocks.KGOLastConnected, 0, token).ConfigureAwait(false);

            var inj = (byte)V_Form;
            if (inj != vivform)
                await WriteBlock(inj, Blocks.KGOVivillonForm, token, vivform).ConfigureAwait(false);
            var (newform, _) = await ReadEncryptedBlockByte(Blocks.KGOVivillonForm, 0, token).ConfigureAwait(false);

            TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
            uint currentEpoch = (uint)t.TotalSeconds;

            await WriteBlock(currentEpoch, Blocks.KGOLastConnected, token, epochtime).ConfigureAwait(false);
            var (newtime, _) = await ReadEncryptedBlockUint(Blocks.KGOLastConnected, 0, token).ConfigureAwait(false);

            await WriteBlock(true, Blocks.KGOVivillonFormEnabled, token, false).ConfigureAwait(false);

            string vivmsg = inj != vivform ? $"Vivillon form has been changed from {(VivForms)vivform} to {(VivForms)newform}." : "Modified form is the same as the current form.";
            MessageBox.Show($"{vivmsg}{Environment.NewLine}KGOLastConnected TimeStamp changed from {FromUnixTime(epochtime)} to {FromUnixTime(newtime)}");
        }

        private enum VivForms
        {
            IcySnow = 0,
            Polar = 1,
            Tundra = 2,
            Continental = 3,
            Garden = 4,
            Elegant = 5,
            Meadow = 6,
            Modern = 7,
            Marine = 8,
            Archipelago = 9,
            HighPlains = 10,
            Sandstorm = 11,
            River = 12,
            Monsoon = 13,
            Savanna = 14,
            Sun = 15,
            Ocean = 16,
            Jungle = 17,
            Fancy = 18,
        }
    }
}
