using System.Threading.Tasks;

namespace Obsidian.Net.Packets
{
    public class Handshake : Packet
    {
        public ProtocolVersion Version;
        
        public string ServerAddress;

        public ushort ServerPort;

        public ClientState NextState;

        public Handshake(byte[] data) : base(0x00, data)
        {
    
        }

        public Handshake() : base(0x00, null){}

        public override async Task PopulateAsync()
        {
            using(var stream = new MinecraftStream(this.PacketData))
            {
                this.Version = (ProtocolVersion)await stream.ReadVarIntAsync();
                this.ServerAddress = await stream.ReadStringAsync();
                this.ServerPort = await stream.ReadUnsignedShortAsync();
                this.NextState = (ClientState)await stream.ReadVarIntAsync();
            }
        }

        public override async Task<byte[]> ToArrayAsync()
        {
            using (var stream = new MinecraftStream())
            {
                await stream.WriteVarIntAsync((int)this.Version);
                //TODO: add string length check
                await stream.WriteStringAsync(this.ServerAddress);
                await stream.WriteUnsignedShortAsync(this.ServerPort);
                await stream.WriteVarIntAsync((int)this.NextState);

                return stream.ToArray();
            }
        }

    }
}