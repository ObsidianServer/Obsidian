﻿using System.Threading.Tasks;

namespace Obsidian.Net.Packets
{
    public class KeepAlive : Packet
    {
        public KeepAlive(long id) : base(0x21, new byte[0])
        {
            this.KeepAliveId = id;
        }

        public KeepAlive(byte[] data) : base(0x21, data) { }

        public long KeepAliveId { get; set; }

        public override async Task PopulateAsync()
        {
            using (var stream = new MinecraftStream(this.PacketData))
            {
                this.KeepAliveId = await stream.ReadLongAsync();
            }
        }

        public override async Task<byte[]> ToArrayAsync()
        {
            using (var stream = new MinecraftStream())
            {
                await stream.WriteLongAsync(this.KeepAliveId);
                return stream.ToArray();
            }
        }
    }
}