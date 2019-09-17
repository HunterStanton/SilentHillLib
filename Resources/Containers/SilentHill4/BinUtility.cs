using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SHLib.Resources.Containers.SilentHill4
{
    /// <summary>
    /// A utility class for getting Bin objects from files on the disk as well as comitting Bin objects to disk.
    /// </summary>
    class BinUtility
    {
        /// <summary>
        /// Loads a .bin file.
        /// </summary>
        /// <param name="binFile">The filestream open on the .bin file.</param>
        /// <returns>The .bin file represented as a Bin object.</returns>
        public Bin LoadBin(FileStream binFile)a
        {

            Bin bin = new Bin();

            // Create a binary reader that will be used to read the file
            BinaryReader reader = new BinaryReader(binFile);

            bin.chunkCount = reader.ReadInt32();

            bin.chunks = new Bin.Chunk[bin.chunkCount];

            for (int i = 0; i < bin.chunkCount; i++)
            {
                int offset = reader.ReadInt32();

                // Save the original position of the reader so we can reset it at the end
                long origPos = reader.BaseStream.Position;

                // Read the offset of the next file
                int nextFileOffset = reader.ReadInt32();

                // Advance the stream to the file's offset
                reader.BaseStream.Position = offset;

                if (nextFileOffset != 0)
                {
                    // Store the original position so we can return to the chunk after attempting to read it's "magic"
                    long originalPosition = reader.BaseStream.Position;

                    // Read the "magics"
                    // The second magic will always match the first if the file is texture data, because the two magics in that case aren't actually magics but the texture and palette count
                    // However this does not seem to always be the case, but it is in ~90% of scenarios, good enough
                    int magic = reader.ReadInt16();
                    int magic2 = reader.ReadInt16();

                    if (magic == magic2)
                    {
                        bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Textures;
                    }
                    else
                    {
                        switch (magic)
                        {
                            // Unknown data, looks like 3d coordinates though
                            case 0xFF11:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Unknown;
                                break;
                            // Model data always starts with 0x0003
                            case 0x0003:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Mesh;
                                break;
                            // SDB files embedded in a .bin
                            case 0x8581:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.SDB;
                                break;
                            // List of monster internal IDs
                            case 0x4554:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.MonsterIDs;
                                break;
                            // Chunk with SLGT magic, not sure what this controls
                            case 0x4C53:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.SLGT;
                                break;
                            // Check if the second magic is also 0xFF01 which indicates animation data
                            // If not just jump to unknown chunk
                            case 0x0001:
                                if (magic2 != -255)
                                {
                                    goto default;
                                }
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Animation;
                                break;
                            // No magic found
                            default:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Unknown;
                                break;
                        }
                    }
                    // Return to the beginning of the chunk
                    reader.BaseStream.Position = originalPosition;

                    bin.chunks[i].data = reader.ReadBytes(nextFileOffset - offset);

                }
                else
                {
                    // Store the original position so we can return to the chunk after attempting to read it's "magic"
                    long originalPosition = reader.BaseStream.Position;

                    // Read the "magics"
                    // The second magic will always match the first if the file is texture data, because the two magics in that case aren't actually magics but the texture and palette count
                    // However this does not seem to always be the case, but it is in ~90% of scenarios, good enough
                    int magic = reader.ReadInt16();
                    int magic2 = reader.ReadInt16();

                    if (magic == magic2)
                    {
                        bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Textures;
                    }
                    else
                    {
                        switch (magic)
                        {
                            // Unknown data, looks like 3d coordinates though
                            case 0xFF11:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Unknown;
                                break;
                            // Model data always starts with 0x0003
                            case 0x0003:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Mesh;
                                break;
                            // SDB files embedded in a .bin
                            case 0x8581:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.SDB;
                                break;
                            // List of monster internal IDs
                            case 0x4554:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.MonsterIDs;
                                break;
                            // Chunk with SLGT magic, not sure what this controls
                            case 0x4C53:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.SLGT;
                                break;
                            // Check if the second magic is also 0xFF01 which indicates animation data
                            // If not just jump to unknown chunk
                            case 0x0001:
                                if (magic2 != -255)
                                {
                                    goto default;
                                }
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Animation;
                                break;
                            // No magic found
                            default:
                                bin.chunks[i].chunkType = Bin.Chunk.ChunkTypes.Unknown;
                                break;
                        }
                    }
                    // Return to the beginning of the chunk
                    reader.BaseStream.Position = originalPosition;

                    bin.chunks[i].data = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length) - offset);
                }

                // Reset base stream position
                reader.BaseStream.Position = origPos;
            }

            // Close the reader
            reader.Close();

            return bin;
        }

        /// <summary>
        /// Builds a .bin file from a Bin object and saves that .bin file to disk.
        /// Also calculates all padding and things like that.
        /// </summary>
        /// <param name="bin">The Bin object that will be saved to disk.</param>
        /// <param name="binFile">The filestream to which the .bin file is committed.</param>
        /// <returns>Whether or not the save succeeded.</returns>
        public bool SaveBin(Bin bin, FileStream binFile)
        {
            // Create a binary writer that will write the new bin file
            BinaryWriter writer = new BinaryWriter(binFile);

            writer.Write(bin.chunkCount);

            List<byte> binBody = new List<byte>();

            // The game seems to have a couple of "safe" values for how much padding is in the header before things go wrong
            // For example, using 0x800 (which allows for 512 chunks in a bin, way more than any bin the game normally uses) stuff will load, but things will break and probably crash eventually
            // Henry's cutscene model for example looks and loads great, but his shadows go wonky and it will crash in random cutscenes, so we want to avoid this at all costs by using a set of if statements to determine what padding to give the header based the original bins

            int padding = 0x0;

            // Determine what padding to use depending on how many chunks are in the bin
            if (bin.chunkCount < 0x1F)
            {
                padding = 0x80;
            }

            if (bin.chunkCount > 0x1F && bin.chunkCount < 0x3F)
            {
                padding = 0x100;
            }

            if (bin.chunkCount > 0x3F && bin.chunkCount < 0x5F)
            {
                padding = 0x180;
            }

            if (bin.chunkCount > 0x5F && bin.chunkCount < 0x7F)
            {
                padding = 0x200;
            }

            // This one doesn't seem to be used by any existing bins, but we'll put it here anyway
            if (bin.chunkCount > 0x7F && bin.chunkCount < 0x9F)
            {
                padding = 0x280;
            }

            if (bin.chunkCount > 0x9F && bin.chunkCount < 0xBF)
            {
                padding = 0x300;
            }

            int tempLength = padding + 0x0;
            int previousLength = 0;

            // Loop through every bin chunk in the output directory and build a new bin file from it
            foreach (var chunk in bin.chunks)
            {
                // Write the offset of the current file to the bin header
                writer.Write(tempLength + previousLength);

                previousLength = previousLength + Convert.ToInt32(chunk.data.Length);


                // Append the current bin chunk to the bin body
                binBody.AddRange(chunk.data);

            }

            // Append extra bytes to pad the bin header
            writer.Write(new byte[padding - writer.BaseStream.Length]);

            // Write the bin body to the bin file
            writer.Write(binBody.ToArray());

            // Close the binary writer and file
            writer.Close();

            return true;
        }
    }
}
