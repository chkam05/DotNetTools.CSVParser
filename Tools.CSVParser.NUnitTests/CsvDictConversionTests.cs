using chkam05.DotNetTools.CSVParser;
using chkam05.DotNetTools.CSVParser.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.CSVParser.NUnitTests.Data;

namespace Tools.CSVParser.NUnitTests
{
    public class CsvDictConversionTests
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
            var result = parser.ParseToDictList();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Keys.Count == 8);

            Assert.True(result[0].ContainsKey("ID"));
            Assert.True(result[0].ContainsKey("NAME"));
            Assert.True(result[0].ContainsKey("ENABLED"));
            Assert.True(result[0].ContainsKey("BIRTHDATE"));
            Assert.True(result[0].ContainsKey("BALANCE"));
            Assert.True(result[0].ContainsKey("HEIGHT"));
            Assert.True(result[0].ContainsKey("AGE"));
            Assert.True(result[0].ContainsKey("SECONDS"));
        }

        [Test]
        public void ConversionWithoutHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                UseHeaders = false
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesDataNoHeaders, parserOptions);
            var result = parser.ParseToDictList();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Keys.Count == 8);

            for (int f = 0; f < result[0].Keys.Count; f++)
                Assert.True(result[0].ContainsKey($"field_{f}"));

            Assert.AreEqual(result[0]["field_0"], "0");
            Assert.AreEqual(result[0]["field_1"], "Kamil Karpinski");
            Assert.AreEqual(result[0]["field_2"], "true");
            Assert.AreEqual(result[0]["field_3"], "1994.12.05");
            Assert.AreEqual(result[0]["field_4"], "199.99");
            Assert.AreEqual(result[0]["field_5"], "1.83");
            Assert.AreEqual(result[0]["field_6"], "26");
            Assert.AreEqual(result[0]["field_7"], "33321600");
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
            var result = parser.ParseToDictList();

            //  Validate result data.
            Assert.True(result.Count == 3);
            Assert.True(result[0].Keys.Count == 4);

            Assert.True(result[0].ContainsKey("field_2"));
            Assert.True(result[0].ContainsKey("field_3"));

            Assert.AreEqual(result[0]["field_2"], string.Empty);
            Assert.AreEqual(result[1]["field_2"], "Kowalski");
            Assert.AreEqual(result[2]["field_2"], "Nowak");

            Assert.AreEqual(result[0]["field_3"], string.Empty);
            Assert.AreEqual(result[1]["field_3"], "32");
            Assert.AreEqual(result[2]["field_3"], string.Empty);
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
            var result = parser.ParseToDictList();

            //  Validate result data.
            Assert.True(result.Count == 3);
            Assert.True(result[0].Keys.Count == 4);

            Assert.True(result[0].ContainsKey("field_0"));
            Assert.True(result[0].ContainsKey("field_1"));
            Assert.True(result[0].ContainsKey("field_2"));
            Assert.True(result[0].ContainsKey("field_3"));

            Assert.AreEqual(result[0]["field_0"], "0");
            Assert.AreEqual(result[0]["field_1"], "Kamil");
            Assert.AreEqual(result[0]["field_2"], string.Empty);
            Assert.AreEqual(result[0]["field_3"], string.Empty);
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
            var result = parser.ParseToDictList();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Keys.Count == 4);

            Assert.AreEqual(result[0]["ID"], string.Empty);
            Assert.AreEqual(result[1]["NAME"], string.Empty);
            Assert.AreEqual(result[2]["SURNAME"], string.Empty);
            Assert.AreEqual(result[3]["AGE"], string.Empty);
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
            var result = parser.ParseToDictList();

            //  Validate result data.
            Assert.True(result.Count == 4);
            Assert.True(result[0].Keys.Count == 4);

            Assert.True(result[0].ContainsKey("field_0"));
            Assert.True(result[0].ContainsKey("field_1"));
            Assert.True(result[0].ContainsKey("field_2"));
            Assert.True(result[0].ContainsKey("field_3"));

            Assert.AreEqual(result[0]["field_0"], string.Empty);
            Assert.AreEqual(result[0]["field_1"], "Kamil");
            Assert.AreEqual(result[0]["field_2"], "Karpiński");
            Assert.AreEqual(result[0]["field_3"], "26");
        }

        #endregion TEST METHODS

    }
}
