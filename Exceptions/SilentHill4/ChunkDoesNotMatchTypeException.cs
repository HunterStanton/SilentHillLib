using System;
using System.Collections.Generic;
using System.Text;

namespace SHLib.Exceptions.SilentHill4
{
    class ChunkDoesNotMatchTypeException : Exception
    {
        public ChunkDoesNotMatchTypeException()
        {
        }

        public ChunkDoesNotMatchTypeException(string message)
            : base(message)
        {
        }

        public ChunkDoesNotMatchTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
