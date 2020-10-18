using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Data
{
    class DataImporter
    {

        //  METHODS

        #region FILE IMPORTER METHODS

        /// <summary> Load data as string lines from file. </summary>
        /// <param name="filePath"> File path to open. </param>
        /// <returns> String lines loaded from file. </returns>
        public static string[] FromFile(string filePath)
        {
            //  Check if selected file exists.
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File \"{filePath}\" not found.");

            try
            {
                //  Load string lines data from file.
                return File.ReadAllLines(filePath);
            }
            catch (Exception)
            {
                throw new IOException($"File \"{filePath}\" is not supported or has been corrupted.");
            }
        }

        #endregion FILE IMPORTER METHODS

        #region STREAM IMPORTER METHODS

        /// <summary> Load data as string lines from stream. </summary>
        /// <param name="stream"> Stream containing string data. </param>
        /// <returns> String lines readed from stream. </returns>
        public static string[] FromStream(Stream stream)
        {
            //  Check if stream has data inside.
            if (stream == null || stream.Length == 0)
                throw new NullReferenceException($"Attempt to read data from empty stream.");

            //  Setup output data and stream to read from it.
            var output = new List<string>();
            stream.Position = 0;

            try
            {
                using (var streamReader = new StreamReader(stream))
                {
                    //  Read line by line from stream.
                    while (streamReader.Peek() >= 0)
                        output.Add(streamReader.ReadLine());

                    //  Return read data as array.
                    return output.ToArray();
                }
            }
            catch (Exception)
            {
                throw new ArgumentException($"Stream contains inappropriate data type or has been corrupted");
            }
            
        }

        #endregion STREAM IMPORTER METHODS

    }
}
