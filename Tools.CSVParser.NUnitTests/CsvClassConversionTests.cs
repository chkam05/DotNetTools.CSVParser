using chkam05.DotNetTools.CSVParser;
using chkam05.DotNetTools.CSVParser.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.CSVParser.NUnitTests.Data;
using Tools.CSVParser.NUnitTests.Data.Models;

namespace Tools.CSVParser.NUnitTests
{
    public class CsvClassConversionTests
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
        public void ConversionWithHeadersAutoMapTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = true,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesData, parserOptions);
            var result = parser.ParseToClass<StaticTypesModelAutoMap>();

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].ID, "0");
            Assert.AreEqual(result[0].NAME, "Kamil Karpinski");
            Assert.AreEqual(result[0].ENABLED, true);
            Assert.AreEqual(result[0].BIRTHDATE.Date, new DateTime(1994, 12, 05, 0, 0, 0, DateTimeKind.Utc).Date);
            Assert.AreEqual(result[0].BALANCE, 199.99);
            Assert.AreEqual(result[0].HEIGHT, 1.83f);
            Assert.AreEqual(result[0].AGE, 26);
            Assert.AreEqual(result[0].SECONDS, 33321600);
        }

        [Test]
        public void ConversionWithHeadersAutoMapNoLetterSizeTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesData, parserOptions);
            var result = parser.ParseToClass<StaticTypesModel>();

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].Id, "0");
            Assert.AreEqual(result[0].Name, "Kamil Karpinski");
            Assert.AreEqual(result[0].Enabled, true);
            Assert.AreEqual(result[0].Birthdate.Date, new DateTime(1994, 12, 05, 0, 0, 0, DateTimeKind.Utc).Date);
            Assert.AreEqual(result[0].Balance, 199.99);
            Assert.AreEqual(result[0].Height, 1.83f);
            Assert.AreEqual(result[0].Age, 26);
            Assert.AreEqual(result[0].Seconds, 33321600);
        }

        [Test]
        public void ConversionWithHeadersManualMapTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = true
            };

            //  Create Mapping.
            var mapping = new Dictionary<string, string>()
            {
                { "ID", "Identifier" },
                { "NAME", "NameSurname" },
                { "ENABLED", "Visible" },
                { "BIRTHDATE", "Date" },
                { "BALANCE", "Money" },
                { "HEIGHT", "Size" },
                { "AGE", "CurrentAge" },
                { "SECONDS", "AgeInSeconds" }
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesData, parserOptions);
            var result = parser.ParseToClass<StaticTypesModelCustomMap>(mapping);

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].Identifier, "0");
            Assert.AreEqual(result[0].NameSurname, "Kamil Karpinski");
            Assert.AreEqual(result[0].Visible, true);
            Assert.AreEqual(result[0].Date.Date, new DateTime(1994, 12, 05, 0, 0, 0, DateTimeKind.Utc).Date);
            Assert.AreEqual(result[0].Money, 199.99);
            Assert.AreEqual(result[0].Size, 1.83f);
            Assert.AreEqual(result[0].CurrentAge, 26);
            Assert.AreEqual(result[0].AgeInSeconds, 33321600);
        }

        [Test]
        public void ConversionAutoMapTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = false
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesDataNoHeaders, parserOptions);

            try
            {
                var result = parser.ParseToClass<StaticTypesModel>();
            }
            catch(Exception exc)
            {
                Assert.AreEqual(exc.Message, "For non headers configuration, mapping is needed.");
                return;
            }

            Assert.Fail();
        }

        [Test]
        public void ConversionManualMapTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = false
            };

            //  Create Mapping.
            var mapping = new Dictionary<string, string>()
            {
                { "field_0", "Id" },
                { "field_1", "Name" },
                { "field_2", "Enabled" },
                { "field_3", "Birthdate" },
                { "field_4", "Balance" },
                { "field_5", "Height" },
                { "field_6", "Age" },
                { "field_7", "Seconds" }
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTypesDataNoHeaders, parserOptions);
            var result = parser.ParseToClass<StaticTypesModel>(mapping);

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].Id, "0");
            Assert.AreEqual(result[0].Name, "Kamil Karpinski");
            Assert.AreEqual(result[0].Enabled, true);
            Assert.AreEqual(result[0].Birthdate.Date, new DateTime(1994, 12, 05, 0, 0, 0, DateTimeKind.Utc).Date);
            Assert.AreEqual(result[0].Balance, 199.99);
            Assert.AreEqual(result[0].Height, 1.83f);
            Assert.AreEqual(result[0].Age, 26);
            Assert.AreEqual(result[0].Seconds, 33321600);
        }

        [Test]
        public void ConversionUndersizedDataTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticUndersizedData, parserOptions);
            var result = parser.ParseToClass<StaticUndersizedModel>();

            //  Validate result data.
            Assert.True(result.Count == 3);

            Assert.AreEqual(result[0].Id, "0");
            Assert.AreEqual(result[0].Name, "Kamil");
            Assert.AreEqual(result[0].Surname, null);
            Assert.AreEqual(result[0].Age, null);

            Assert.AreEqual(result[1].Id, "1");
            Assert.AreEqual(result[1].Name, "Jan");
            Assert.AreEqual(result[1].Surname, null);
            Assert.AreEqual(result[1].Age, null);
        }

        [Test]
        public void ConversionUndersizedDataNoHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = false
            };

            //  Create Mapping.
            var mapping = new Dictionary<string, string>()
            {
                { "field_0", "Id" },
                { "field_1", "Name" },
                { "field_2", "Surname" },
                { "field_3", "Age" }
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticUndersizedDataNoHeaders, parserOptions);
            var result = parser.ParseToClass<StaticUndersizedModel>(mapping);

            //  Validate result data.
            Assert.True(result.Count == 3);

            Assert.AreEqual(result[0].Id, "0");
            Assert.AreEqual(result[0].Name, "Kamil");
            Assert.AreEqual(result[0].Surname, null);
            Assert.AreEqual(result[0].Age, null);

            Assert.AreEqual(result[1].Id, "1");
            Assert.AreEqual(result[1].Name, "Jan");
            Assert.AreEqual(result[1].Surname, "Kowalski");
            Assert.AreEqual(result[1].Age, 32);
        }

        [Test]
        public void ConversionLeakyDataTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticLeakyData, parserOptions);
            var result = parser.ParseToClass<StaticUndersizedModel>();

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].Id, "");
            Assert.AreEqual(result[1].Name, "");
            Assert.AreEqual(result[2].Surname, "");
            Assert.AreEqual(result[3].Age, null);
        }

        [Test]
        public void ConversionLeakyDataNoHeadersTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = false
            };

            //  Create Mapping.
            var mapping = new Dictionary<string, string>()
            {
                { "field_0", "Id" },
                { "field_1", "Name" },
                { "field_2", "Surname" },
                { "field_3", "Age" }
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticLeakyDataNoHeaders, parserOptions);
            var result = parser.ParseToClass<StaticUndersizedModel>(mapping);

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].Id, "");
            Assert.AreEqual(result[1].Name, "");
            Assert.AreEqual(result[2].Surname, "");
            Assert.AreEqual(result[3].Age, null);
        }

        [Test]
        public void ConversionAutoDateTimeTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseAutoDateConversion = true,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticAutoConvertDateTimeData, parserOptions);
            var result = parser.ParseToClass<StaticDateTypesModel>();

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].DateTime.Date, new DateTime(1994, 12, 05).Date);
            Assert.AreEqual(result[1].DateTime, new DateTime(1997, 01, 01, 0, 5, 0));
            Assert.AreEqual(result[2].DateTime, DateTime.Now.Date.AddHours(9).AddMinutes(41));
            Assert.AreEqual(result[3].DateTime, DateTime.Now.Date.AddHours(6).AddMinutes(30).AddSeconds(15));
        }

        [Test]
        public void ConversionManualDateTimeTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DateTimePattern = "yyyy.MM.dd",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseAutoDateConversion = false,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticManualConvertDateTimeData, parserOptions);
            var result = parser.ParseToClass<StaticDateTypesModel>();

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].DateTime.Date, new DateTime(1994, 12, 05).Date);
            Assert.AreEqual(result[1].DateTime.Date, new DateTime(1997, 01, 01).Date);
            Assert.AreEqual(result[2].DateTime.Date, new DateTime(1999, 08, 27).Date);
            Assert.AreEqual(result[3].DateTime.Date, new DateTime(1995, 02, 10).Date);
        }

        [Test]
        public void ConversionTimestampDateTimeTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                DefaultFieldName = "field_",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseTimeSpanInsteadOfString = true,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.StaticTimeSpanConvertDateTimeData, parserOptions);
            var result = parser.ParseToClass<StaticDateTypesModel>();

            //  Validate result data.
            Assert.True(result.Count == 4);

            Assert.AreEqual(result[0].DateTime.Date, new DateTime(1994, 12, 05).Date);
            Assert.AreEqual(result[1].DateTime.Date, new DateTime(1997, 01, 01).Date);
            Assert.AreEqual(result[2].DateTime.Date, new DateTime(1999, 08, 27).Date);
            Assert.AreEqual(result[3].DateTime.Date, new DateTime(1995, 02, 10).Date);
        }

        [Test]
        public void ConversionNullableVariablesTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.NullableTypesData, parserOptions);
            var result = parser.ParseToClass<NullableTypesModel>();

            //  Validate result data.
            Assert.True(result.Count == 7);

            Assert.AreEqual(result[0].Name, "");
            Assert.AreEqual(result[1].Enabled, null);
            Assert.AreEqual(result[2].Birthdate, null);
            Assert.AreEqual(result[3].Balance, null);
            Assert.AreEqual(result[4].Height, null);
            Assert.AreEqual(result[5].Age, null);
            Assert.AreEqual(result[6].Seconds, null);
        }

        [Test]
        public void ConversionListVariablesTest()
        {
            //  Create Options.
            var parserOptions = new CsvParserOptions()
            {
                ColumnDelimiter = ";",
                ListDelimiter = ",",
                FloatingPointNumberDelimiter = ".",
                IncludeMappingLetterSize = false,
                UseAutoDateConversion = true,
                UseHeaders = true
            };

            //  Load CSV Data.
            var parser = CsvParser.FromString(StaticTestData.ListTypesData, parserOptions);
            var result = parser.ParseToClass<ListTypesModel>();

            //  Validate result data.
            Assert.True(result.Count == 3);

            for (int f = 0; f < 3; f++)
            {
                var count = f == 0 ? 4 : f == 1 ? 1 : 0;

                Assert.AreEqual(result[f].Id, f.ToString());
                Assert.AreEqual(result[f].Name.Count, count);
                Assert.AreEqual(result[f].Enabled.Count, count);
                Assert.AreEqual(result[f].Birthdate.Count, count);
                Assert.AreEqual(result[f].Balance.Count, count);
                Assert.AreEqual(result[f].Height.Count, count);
                Assert.AreEqual(result[f].Age.Count, count);
                Assert.AreEqual(result[f].Seconds.Count, count);
            }
        }

        #endregion TEST METHODS

    }
}
