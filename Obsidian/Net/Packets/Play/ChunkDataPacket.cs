﻿using Obsidian;
using Obsidian.ChunkData;
using Obsidian.Nbt.Tags;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Obsidian.Net.Packets.Play
{
    public class ChunkDataPacket : Packet
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public List<ChunkSection> Sections { get; }
        public List<int> Biomes { get; }
        public List<NbtTag> BlockEntities { get; }

        public int changedSectionFilter = 0b1111111111111111;

        public ChunkDataPacket() : base(0x22) { }

        public ChunkDataPacket(int chunkX, int chunkZ) : base(0x22)
        {
            this.ChunkX = chunkX;
            this.ChunkZ = chunkZ;

            this.Sections = new List<ChunkSection>();
            this.Biomes = new List<int>(16 * 16);
            this.BlockEntities = new List<NbtTag>();
        }

        protected override async Task ComposeAsync(MinecraftStream stream)
        {
            bool fullChunk = true; // changedSectionFilter == 0b1111111111111111;

            await stream.WriteIntAsync(this.ChunkX);
            await stream.WriteIntAsync(this.ChunkZ);

            await stream.WriteBooleanAsync(fullChunk);

            int mask = 0;

            await using var dataStream = new MinecraftStream();

            var chunkSectionY = 0;
            foreach (var section in this.Sections)
            {
                if (section == null)
                    throw new InvalidOperationException();

                if (fullChunk || (this.changedSectionFilter & (1 << chunkSectionY)) != 0)
                {
                    mask |= 1 << chunkSectionY;

                    await section.WriteToAsync(dataStream);
                }
                chunkSectionY++;
            }

            if (chunkSectionY != 16)
                throw new InvalidOperationException();

            if (fullChunk)
            {
                if (this.Biomes.Count != 16 * 16)
                    throw new InvalidOperationException();

                foreach (int biomeId in this.Biomes)
                    await dataStream.WriteIntAsync(biomeId);

            }
            await stream.WriteVarIntAsync(mask);

            await stream.WriteVarIntAsync((int)dataStream.Length);

            dataStream.Position = 0;
            await dataStream.CopyToAsync(stream);

            await stream.WriteVarIntAsync(this.BlockEntities.Count);

            foreach (var entity in this.BlockEntities)
                await stream.WriteNbtAsync(entity);
        }

        private int NeededBits(int value) => BitOperations.LeadingZeroCount((uint)value);
    }
}