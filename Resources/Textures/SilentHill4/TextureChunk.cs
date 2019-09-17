using System;
using System.Collections.Generic;
using System.Text;

namespace SHLib.Resources.Textures.SilentHill4
{
    /// <summary>
    /// Represents a texture chunk in a .bin file.
    /// </summary>
    public class TextureChunk
    {
        // Which chunk this is in the bin file
        // This is important to store for reimporting because the game uses some sort of lookup table to determine which chunk loads when a mesh is loaded
        public uint chunkIndex;

        public int textureCount;
        public int paletteCount;

        public byte[] unused = new byte[0xc];

        public uint[] textureInfoPointers;
        public uint[] paletteInfoPointers;

        public TextureInfo[] textureInfos;
        public PaletteInfo[] paletteInfos;

        public TextureHeader[] textureHeaders;

        public class TextureInfo
        {
            public int width;
            public int height;

            // TODO: Find out what this is, always seems to be 0 on PC texture chunks
            public byte[] unknown = new byte[0x8];
        }

        public class PaletteInfo
        {
            public byte[] unknown = new byte[0xc];

            // I'm not sure what this pointer is. The first one always points to the offset of the first palette info for some reason, then the ones following are incremented by 0x60 every time
            public int pointer;
        }

        public class TextureHeader
        {
            // Always seems to be empty on PC, but might be used on PS2/XBox
            public byte[] empty = new byte[0x20];

            public int width;
            public int height;

            // DXT1, DXT3, DXT5, etc. 
            // Also can be just 0x15 for A8R8G8B8 textures
            public char[] imageType = new char[4];

            // The count of the image itself + number of mipmaps
            public int imageCount;

            // The pitch of the DDS texture
            public int pitch;

            public byte[] unknownBytes = new byte[0x1c];

            public int textureOffset;
            public int mipMap1Offset;
            public int mipMap2Offset;
            public int mipMap3Offset;
            public int mipMap4Offset;
            public int mipMap5Offset;
            public int mipMap6Offset;

            // Not sure what this is to be honest, but I'm guessing it's some sort of identifier or something, so I'll call it that
            public int textureId;

            public byte[] mainImageData;
            public byte[] mipMap1Data;
            public byte[] mipMap2Data;
            public byte[] mipMap3Data;
            public byte[] mipMap4Data;
            public byte[] mipMap5Data;
            public byte[] mipMap6Data;
        }



    }
}
