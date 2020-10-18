using chkam05.DotNetTools.CSVParser;
using chkam05.DotNetTools.CSVParser.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tools.CSVParser.NUnitTests.Data;

namespace Tools.CSVParser.NUnitTests
{
    public class CsvArrayConversionTests
    {

        //  VARIABLES

        //  METHODS

        #region SETUP METHODS

        [SetUp]
        public void Setup()
        {
        }

        #endregion SETUP METHODS

        #region TEST METHODS

        [Test]
        public void ConversionWithHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesData, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            Assert.True(result.Count == 5);
            Assert.True(result[0].Length == 8);

            Assert.AreEqual(result[0][0], "ID");
            Assert.AreEqual(result[0][1], "NAME");
            Assert.AreEqual(result[0][2], "ENABLED");
            Assert.AreEqual(result[0][3], "BIRTHDATE");
            Assert.AreEqual(result[0][4], "BALANCE");
            Assert.AreEqual(result[0][5], "HEIGHT");
            Assert.AreEqual(result[0][6], "AGE");
            Assert.AreEqual(result[0][7], "SECONDS");
        }

        [Test]
        public void ConversionWithoutHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                UseHeaders = false
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesDataNoHeaders, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Length == 8);

            Assert.AreEqual(result[0][0], "0");
            Assert.AreEqual(result[0][1], "Kamil Karpinski");
            Assert.AreEqual(result[0][2], "true");
            Assert.AreEqual(result[0][3], "1994.12.05");
            Assert.AreEqual(result[0][4], "199.99");
            Assert.AreEqual(result[0][5], "1.83");
            Assert.AreEqual(result[0][6], "26");
            Assert.AreEqual(result[0][7], "33321600");
        }

        [Test]
        public void ConversionUndersizedDataTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticUndersizedData, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Length == 4);

            Assert.AreEqual(result[0][2], "field_2");
            Assert.AreEqual(result[1][2], string.Empty);
            Assert.AreEqual(result[2][2], "Kowalski");
            Assert.AreEqual(result[3][2], "Nowak");

            Assert.AreEqual(result[0][3], "field_3");
            Assert.AreEqual(result[1][3], string.Empty);
            Assert.AreEqual(result[2][3], "32");
            Assert.AreEqual(result[3][3], string.Empty);
        }

        [Test]
        public void ConversionUndersizedDataNoHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                UseHeaders = false
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticUndersizedDataNoHeaders, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            Assert.True(result.Count == 3);
            Assert.True(result[0].Length == 4);

            Assert.AreEqual(result[0][2], string.Empty);
            Assert.AreEqual(result[1][2], "Kowalski");
            Assert.AreEqual(result[2][2], "Nowak");

            Assert.AreEqual(result[0][3], string.Empty);
            Assert.AreEqual(result[1][3], "32");
            Assert.AreEqual(result[2][3], string.Empty);
        }

        [Test]
        public void ConversionLeakyDataTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticLeakyData, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            Assert.True(result.Count == 5);
            Assert.True(result[0].Length == 4);

            Assert.AreEqual(result[1][0], string.Empty);
            Assert.AreEqual(result[2][1], string.Empty);
            Assert.AreEqual(result[3][2], string.Empty);
            Assert.AreEqual(result[4][3], string.Empty);
        }

        [Test]
        public void ConversionLeakyDataNoHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                UseHeaders = false
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticLeakyDataNoHeaders, parserOptions);
            var result = parser.ParseToStringsArray();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Length == 4);

            Assert.AreEqual(result[0][0], string.Empty);
            Assert.AreEqual(result[1][1], string.Empty);
            Assert.AreEqual(result[2][2], string.Empty);
            Assert.AreEqual(result[3][3], string.Empty);
        }

        #endregion TEST METHODS

    }
}
