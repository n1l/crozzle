using System;
using System.Collections.Generic;
using System.IO;
using CrozzleEngine.Exceptions;

namespace CrozzleEngine.Interfaces
{
    /// <summary>
    /// Abstract file loader
    /// </summary>
    public abstract class FileLoader
    {
        private const string FILE_NOT_FOUND_PATTERN_MSG = "ERROR: File not found: path - {0}";

        /// <summary>
        /// Model
        /// </summary>
        protected List<string> file;

        /// <summary>
        /// Load file and create sample list of string model
        /// </summary>
        /// <param name="filePath">path to file</param>
        protected void LoadFile(string filePath)
        {
            file = new List<string>();

            if (!File.Exists(filePath))
            {
                throw new ModelLoadException(string.Format(FILE_NOT_FOUND_PATTERN_MSG, filePath));
            }

            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                while (!reader.EndOfStream)
                {
                    file.Add(reader.ReadLine());
                }
            }
        }
    }
}
