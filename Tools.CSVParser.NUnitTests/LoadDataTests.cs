using chkam05.DotNetTools.CSVParser;
using chkam05.DotNetTools.CSVParser.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools.CSVParser.NUnitTests
{
    public class LoadDataTests
    {

        //  VARIABLES

        private string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "SimpleData.csv");
        private Stream _streamData;
        private string _textData;


        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
            //  Create test data.
            _textData = @"ID;NAME;SURNAME;AGE
0;Kamil;Karpiński;26
1;Jan;Kowalski;32
2;Aleksander;Nowak;28
3;Michał;Wójcik;43";

            //  Convert data to stream.
            byte[] byteArray = Encoding.ASCII.GetBytes(_textData);
            _streamData = new MemoryStream(byteArray);
        }

        #endregion SETUP METHODS

        #region TESTS METHODS

        [Test]
        public void LoadFromFileTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                UseHeaders = true
            };

            //  Test parser.
            var parser = CsvParser.FromFile(_filePath, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            ValidateTestsResult(result);
        }

        [Test]
        public void LoadFromStreamTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                UseHeaders = true
            };

            //  Test parser.
            var parser = CsvParser.FromStream(_streamData, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            ValidateTestsResult(result);
        }

        [Test]
        public void LoadFromStringTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                UseHeaders = true
            };

            //  Test parser.
            var parser = CsvParser.FromString(_textData, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            ValidateTestsResult(result);
        }

        #endregion TESTS METHODS

        #region TESTS VALIDATION

        private void ValidateTestsResult(List<string[]> result)
        {
            //  Check data.
            Assert.True(result.Count == 5);
            Assert.True(result[0].Length == 4);
        }

        #endregion TESTS VALIDATION

    }
}
