using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SHLib
{
    class FileUtility
    {

        /// <summary>
        /// Opens a file with a specific game.
        /// </summary>
        /// <param name="path">The path to the file on disk that will be opened.</param>
        /// <param name="game">The game of the file.</param>
        /// <returns>The object that represents the opened file, or null if the operation did not succeed.</returns>
        public object openFile(string path, Game game)
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            switch (game)
            {
                case Game.SilentHill2:
                    goto default;
                case Game.SilentHill3:
                    goto default;
                case Game.SilentHill4:
                    return Resources.Containers.SilentHill4.BinUtility.LoadBin(stream);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Saves the current file to it's original location on the disk.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="stream"></param>
        /// <param name="game"></param>
        /// <returns>Whether or not the save operation succeeded.</returns>
        public bool saveFile(object file, FileStream stream, Game game)
        {
            switch (game)
            {
                case Game.SilentHill2:
                    goto default;
                case Game.SilentHill3:
                    goto default;
                case Game.SilentHill4:
                    return Resources.Containers.SilentHill4.BinUtility.SaveBin((Resources.Containers.SilentHill4.Bin)file, stream);
                default:
                    return false;
            }
        }
    }

    enum Game
    {
        SilentHill2,
        SilentHill3,
        SilentHill4
    }
}
