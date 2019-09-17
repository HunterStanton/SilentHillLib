using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using MathNet.Numerics;

namespace SHLib.Resources.Geometry.SilentHill4
{
    /// <summary>
    /// Represents a primitive for Silent Hill 4.
    /// Primitives are used to build models and contain a header that describes the primitive as well as pointers to its data.
    /// </summary>
    public class Primitive
    {
        public uint primitiveSize;

        public uint unknown;

        public uint primitiveHeaderSize;

        public uint unknownUint;

        public uint empty1;
        public uint empty2;

        public uint unknownPointer;

        public uint boneSet1Count;

        public uint boneSet1SequencePointer;

        public uint boneSet2Count;

        public uint boneSet2SequencePointer;

        public uint unknown2;
        public uint unknown3;

        public uint unknownAlwaysOne;

        public uint textureIndexPointer;

        public uint vertexConstantPointer;

        public uint headerSizeAlternative;

        public uint unknownPointer2;

        public uint unknownCount;

        public float unknownFloat1;
        public float unknownFloat2;
        public float unknownFloat3;

        public ColorChunk[] colorChunk = new ColorChunk[4];

        public short[] boneSequenceSet1;
        public short[] boneSequenceSet2;

        public byte[] unknownChunk3 = new byte[0x6c];

        public uint triStripCount;
        public uint vertexCount;

        public uint vertexSize;

        public Vertex[] vertices;
        public Triangle[] triStrips;

        public uint[] bones = new uint[0xE];

        public byte[] FFArray;

        /// <summary>
        /// Determines how the model is tinted.
        /// </summary>
        public class ColorChunk
        {
            public float red;
            public float green;
            public float blue;
            public float alpha;
        }

        /// <summary>
        /// Represents a Silent Hill 4 vertex.
        /// </summary>
        public class Vertex
        {
            public Vector3 position;
            public Vector3 unknown;

            public float texCoordU;
            public float texCoordV;

            public Vector4 unknown2;
        }

        /// <summary>
        /// Represents a Silent Hill 4 triangle.
        /// </summary>
        public class Triangle
        {
            public short index;
        }
    }
}
