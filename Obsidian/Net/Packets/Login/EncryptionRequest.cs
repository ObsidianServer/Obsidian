﻿using System.Threading.Tasks;

namespace Obsidian.Net.Packets
{
    public class EncryptionRequest : Packet
    {
        public string ServerId { get; private set; }

        public byte[] PublicKey { get; private set; }

        public byte[] VerifyToken { get; private set; }

        public EncryptionRequest(byte[] publicKey, byte[] verifyToken) : base(0x01, new byte[0])
        {
            this.PublicKey = publicKey;

            this.VerifyToken = verifyToken;
        }

        public override async Task<byte[]> ToArrayAsync()
        {
            using (var stream = new MinecraftStream())
            {
                await stream.WriteStringAsync(this.ServerId ?? string.Empty);
                await stream.WriteVarIntAsync(this.PublicKey.Length);
                await stream.WriteAsync(this.PublicKey);
                await stream.WriteVarIntAsync(4);
                await stream.WriteAsync(this.VerifyToken);
                return stream.ToArray();
            }
        }

        public override async Task PopulateAsync()
        {
            using (var stream = new MinecraftStream(this.PacketData))
            {
                this.ServerId = await stream.ReadStringAsync() ?? string.Empty;
                var keyLength = await stream.ReadVarIntAsync();
                this.PublicKey = await stream.ReadUInt8ArrayAsync(keyLength);

                var tokenLength = await stream.ReadVarIntAsync();
                this.VerifyToken = await stream.ReadUInt8ArrayAsync(tokenLength);
            }
        }
    }
}