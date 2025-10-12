using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnimeStudio
{
    public sealed class NapAssetBundleIndex : NamedObject
    {
        public int AssetCount;
        public List<Asset> Assets;
        public int BundleCount;
        public List<Bundle> Bundles;
        public int BlockCount;
        public List<Block> Blocks;

        public NapAssetBundleIndex(ObjectReader reader) : base(reader)
        {
            AssetCount = reader.ReadInt32();
            Assets = new List<Asset>(AssetCount);
            for (int i = 0; i < AssetCount; i++)
                Assets.Add(new Asset(reader));

            BundleCount = reader.ReadInt32();
            Bundles = new List<Bundle>(BundleCount);
            for (int i = 0; i < BundleCount; i++)
                Bundles.Add(new Bundle(reader));

            BlockCount = reader.ReadInt32();
            Blocks = new List<Block>(BlockCount);
            for (int i = 0; i < BlockCount; i++)
                Blocks.Add(new Block(reader));
        }

        public class Asset
        {
            public uint Bundle;
            public long PathHash;
            public Asset(ObjectReader reader)
            {
                Bundle = reader.ReadUInt32();
                PathHash = reader.ReadInt64();
            }
        }

        public class Bundle
        {
            public uint BlockIndex;
            public ulong BundleHashName;
            public ulong BundleHash;
            public uint Offset;
            public uint ChildrenStartIndex;
            public uint ChildrenEndIndex;
            public uint FileSize;
            public Bundle(ObjectReader reader)
            {
                BlockIndex = reader.ReadUInt32();
                BundleHashName = reader.ReadUInt64();
                BundleHash = reader.ReadUInt64();
                Offset = reader.ReadUInt32();
                ChildrenStartIndex = reader.ReadUInt32();
                ChildrenEndIndex = reader.ReadUInt32();
                FileSize = reader.ReadUInt32();
            }
        }

        public class Block
        {
            public ulong BlockHashName;
            public string Location;
            public Block(ObjectReader reader)
            {
                BlockHashName = reader.ReadUInt64();
                Location = Encoding.UTF8.GetString(reader.ReadBytes(1));
            }
        }
    }
}
