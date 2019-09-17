using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SHLib
{
    class FileUtility
    {
        /// <summary>
        /// Opens a file with a specific game
        /// </summary>
        /// <param name="path"></param>
        /// <param name="game"></param>
        public void openFile(string path, Game game)
        {

        }

        /// <summary>
        /// Saves the current file to it's original location on the disk.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="game"></param>
        public void saveFile(FileStream file, Game game)
        {

        }
    }

    enum Game
    {
        SilentHill2,
        SilentHill3,
        SilentHill4
    }
}
