using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace SHLib.Resources.Geometry.SilentHill4
{
    public class Model
    {
        // If a chunk starts with this, it is a model chunk
        public uint magic = 0xFFFF0003;

        // Game/engine version
        // Always 4 for SH4
        public uint gameVersion = 0x04;

        // Points to the model scaling(???)
        public uint scalePointer;

        // Number of bones in a model
        public uint numBones;

        public uint unknown1;

        // Number of (some kind of) bones in a model
        public uint numBones2;

        public uint unknown3;
        public uint unknown4;

        // The number of sub parts in a model
        public uint numSubParts;

        // Pointer to the first 3D mesh in the model chunk
        public uint pointerToFirstMesh;

        public uint unknown6;

        // Pointer to the end of the last 3D mesh in the model chunk
        public uint pointerToEndOfLastMesh;

        // Number of textures in the texture chunk that the model uses
        public uint numTextures;

        public uint unknown7;

        public uint unknown8;

        public uint unknown9;

        public uint unknown10;
        public uint unknown11;
        public uint unknown12;
        public uint unknown13;

        public uint unknown14;
        public uint unknown15;
        public uint unknown16;
        public uint unknown17;

        public uint unknown18;
        public uint unknown19;
        public uint unknown20;
        public uint unknown21;

        public uint unknown22;
        public uint unknown23;
        public uint unknown24;
        public uint unknown25;

        public Matrix<float>[] boneSet1;
        public Matrix<float>[] boneSet2;

        public UnknownChunk unknownChunk = new UnknownChunk();

        // The good stuff
        public Primitive[] primitives;

        public class UnknownChunk
        {
            // Always seems to be 0xFF, but could be something else in some models
            public uint unknownMagic = 0xFF;

            public byte[] unknownChunk = new byte[0xc];

            // Whether or not the mesh uses textures
            public int usesTextures;

            public byte[] unknownChunk2 = new byte[0x10];

            // The index of the texture the part uses
            public int textureToUse;

            public byte[] unknownChunk3 = new byte[0x18];

            public UnknownFloatChunk[] unknownFloatChunks = new UnknownFloatChunk[8];

            public byte[] unknownChunk4 = new byte[0x10];

            public class UnknownFloatChunk
            {
                public float[] unknown = new float[3];
                public uint unknown2;
            }
        }
    }
}
