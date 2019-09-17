using System;
using System.Collections.Generic;
using System.Text;

namespace SHLib.Resources.Containers.SilentHill4
{
    /// <summary>
    /// Represents a Silent Hill 4 .bin file.
    /// </summary>
    class Bin
    {
        /// <summary>
        /// The number of chunks inside the .bin.
        /// </summary>
        public int chunkCount;

        /// <summary>
        /// The chunks inside the .bin.
        /// </summary>
        public Chunk[] chunks;

        /// <summary>
        /// A chunk in the .bin file.
        /// Chunks can contain different types of data, such as textures, meshes, or animations.
        /// </summary>
        public class Chunk
        {
            /// <summary>
            /// The data of the chunk.
            /// </summary>
            public byte[] data;

            public ChunkTypes chunkType;

            public enum ChunkTypes
            {
                Mesh,
                Animation,
                Textures,
                SLGT,
                SDB,
                ShadowModel,
                MonsterIDs,
                Unknown
            }
        }
    }
}
