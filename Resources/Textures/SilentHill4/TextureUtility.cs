using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SHLib.Resources.Textures.SilentHill4
{
    /// <summary>
    /// Utility class for dealing with textures.
    /// </summary>
    public class TextureUtility
    {
        public static TextureChunk loadTextureChunk(Containers.SilentHill4.Bin.Chunk chunk)
        {
            if(chunk.chunkType != Containers.SilentHill4.Bin.Chunk.ChunkTypes.Textures)
            {
                throw new Exceptions.SilentHill4.ChunkDoesNotMatchTypeException("The chunk did not match the expected type (Textures)");
            }

            MemoryStream stream = new MemoryStream(chunk.data);

            BinaryReader reader = new BinaryReader(stream);

            TextureChunk texChunk = new TextureChunk();

            texChunk.textureCount = reader.ReadInt16();
            texChunk.paletteCount = reader.ReadInt16();

            texChunk.unused = reader.ReadBytes(0xC);

            texChunk.textureInfoPointers = new uint[texChunk.textureCount];
            texChunk.paletteInfoPointers = new uint[texChunk.paletteCount];
            texChunk.textureInfos = new TextureChunk.TextureInfo[texChunk.textureCount].Select(v => new TextureChunk.TextureInfo()).ToArray();
            texChunk.paletteInfos = new TextureChunk.PaletteInfo[texChunk.paletteCount].Select(v => new TextureChunk.PaletteInfo()).ToArray();


            // Read the pointers
            for (var i=0;i< texChunk.textureCount;i++)
            {
                texChunk.textureInfoPointers[i] = reader.ReadUInt32();
            }
            for (var i = 0; i < texChunk.paletteCount; i++)
            {
                texChunk.paletteInfoPointers[i] = reader.ReadUInt32();
            }

            for (var i = 0; i < texChunk.textureCount; i++)
            {
                texChunk.textureInfos[i].width = reader.ReadInt32();
                texChunk.textureInfos[i].height = reader.ReadInt32();
                texChunk.textureInfos[i].unknown = reader.ReadBytes(0x8);
            }
            for (var i = 0; i < texChunk.paletteCount; i++)
            {
                texChunk.paletteInfos[i].unknown = reader.ReadBytes(0xc);
                texChunk.paletteInfos[i].pointer = reader.ReadInt32();
            }

            long texOffset = reader.BaseStream.Position;

            texChunk.textureHeaders = new TextureChunk.TextureHeader[texChunk.textureCount].Select(v => new TextureChunk.TextureHeader()).ToArray(); ;

            // Read texture headers
            for (int i = 0; i < texChunk.textureCount; i++)
            {

                texChunk.textureHeaders[i].empty = reader.ReadBytes(0x20);

                texChunk.textureHeaders[i].width = reader.ReadInt32();
                texChunk.textureHeaders[i].height = reader.ReadInt32();

                texChunk.textureHeaders[i].imageType = reader.ReadChars(4);

                texChunk.textureHeaders[i].imageCount = reader.ReadInt32();

                texChunk.textureHeaders[i].pitch = reader.ReadInt32();

                texChunk.textureHeaders[i].unknownBytes = reader.ReadBytes(0x1c);

                texChunk.textureHeaders[i].textureOffset = reader.ReadInt32();
                texChunk.textureHeaders[i].mipMap1Offset = reader.ReadInt32();
                texChunk.textureHeaders[i].mipMap2Offset = reader.ReadInt32();
                texChunk.textureHeaders[i].mipMap3Offset = reader.ReadInt32();
                texChunk.textureHeaders[i].mipMap4Offset = reader.ReadInt32();
                texChunk.textureHeaders[i].mipMap5Offset = reader.ReadInt32();
                texChunk.textureHeaders[i].mipMap6Offset = reader.ReadInt32();

                texChunk.textureHeaders[i].textureId = reader.ReadInt32();
            }

            // Read texture data
            for (int i = 0; i < texChunk.textureCount; i++)
            {
                if (texChunk.textureHeaders[i].imageCount == 1)
                {
                    // If we're not at the end of the chunk
                    if (i != texChunk.textureCount - 1)
                    {
                        texChunk.textureHeaders[i].mainImageData = new byte[(texChunk.textureHeaders[i].pitch) * 4];

                        // Every image's pointer needs to be incremented by a value of 0x70 * imageIndex for whatever reason
                        reader.BaseStream.Position = (texChunk.textureHeaders[i].textureOffset + texOffset) + (0x70 * i);

                        // Get the image data
                        texChunk.textureHeaders[i].mainImageData = reader.ReadBytes(texChunk.textureHeaders[i].mainImageData.Length);
                    }
                    else
                    {
                        texChunk.textureHeaders[i].mainImageData = new byte[(texChunk.textureHeaders[i].pitch) * 4];

                        // Every image's pointer needs to be incremented by a value of 0x70 * imageIndex for whatever reason
                        reader.BaseStream.Position = (texChunk.textureHeaders[i].textureOffset + texOffset) + (0x70 * i);

                        // Get the image data
                        texChunk.textureHeaders[i].mainImageData = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length - texChunk.textureHeaders[i].mainImageData.Length));
                    }
                }
                else
                {
                    // Every image's pointer needs to be incremented by a value of 0x70 * imageIndex for whatever reason
                    reader.BaseStream.Position = (texChunk.textureHeaders[i].textureOffset + texOffset) + (0x70 * i);

                    texChunk.textureHeaders[i].mainImageData = reader.ReadBytes(Convert.ToInt32(texChunk.textureHeaders[i].mipMap1Offset - texChunk.textureHeaders[i].textureOffset));
                }
            }



            return texChunk;
        }

        /// <summary>
        /// Exports a DDS texture to the filestream.
        /// </summary>
        /// <param name="file">The filestream to write to.</param>
        /// <param name="image">The image to export to DDS.</param>
        public static void exportTextureAsDDS(FileStream file, TextureChunk.TextureHeader image)
        {

            BinaryWriter imageWriter = new BinaryWriter(file);

            imageWriter.Write(0x20534444);

            // Write DDS something?
            imageWriter.Write(0x7c);

            imageWriter.Write(0x081007);

            imageWriter.Write(image.height);
            imageWriter.Write(image.width);

            imageWriter.Write(image.pitch);

            // DDS volume
            imageWriter.Write(0);

            // Mip map level
            imageWriter.Write(0);

            // Write reserved section
            imageWriter.Write(new byte[11 * 4]);

            // DDS pixel format size (always 32)
            imageWriter.Write(0x20);



            if (image.imageType[0] != 21)
            {
                // PF Flag
                imageWriter.Write(0x4);
                imageWriter.Write(image.imageType);
                imageWriter.Write(new byte[0x14]);
            }
            else
            {
                // PF Flag
                imageWriter.Write(0x41);
                imageWriter.Write(0x00);
                imageWriter.Write(0x20);
                imageWriter.Write(0xFF0000);
                imageWriter.Write(0x00FF00);
                imageWriter.Write(0x0000FF);
                imageWriter.Write(0xFF000000);
            }


            // Caps
            imageWriter.Write(0x1000);

            imageWriter.Write(new byte[0x10]);

            imageWriter.Write(image.mainImageData);

        }
    }
}
