using chkam05.DotNetTools.CSVParser.Data;
using chkam05.DotNetTools.CSVParser.Tools;
using chkam05.DotNetTools.CSVParser.Statics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chkam05.DotNetTools.CSVParser
{
    public class CsvParser : ICsvParser
    {

        //  VARIABLES

        private CsvParserOptions _options;
        private string[] _rawData;


        //  METHODS

        #region CLASS METHODS

        /// <summary> CSVParser class constructor. </summary>
        /// <param name="rawData"> Loaded string lines. </param>
        /// <param name="options"> Parser options. </param>
        internal CsvParser(string[] rawData, CsvParserOptions options = null)
        {
            //  Setup data.
            _rawData = rawData != null ? rawData : new string[0];

            //  Options validation.
            OptionsValidation(options);

            //  Setup parser options.
            if (options == null)
                _options = new CsvParserOptions();
            else
                _options = options;
        }

        #endregion CLASS METHODS

        #region CREATE METHODS

        /// <summary> Create CSV Parser with data loaded as string lines from string based file. </summary>
        /// <param name="filePath"> File path to open. </param>
        /// <param name="options"> Parser options. </param>
        /// <returns> Interface for CSV Parser with loaded data. </returns>
        public static ICsvParser FromFile(string filePath, CsvParserOptions options = null)
        {
            var lines = DataImporter.FromFile(filePath);
            return new CsvParser(lines, options);
        }

        /// <summary> Create CSV Parser with data loaded as string lines from stream. </summary>
        /// <param name="stream"> Stream containing string data. </param>
        /// <param name="options"> Parser options. </param>
        /// <returns> Interface for CSV Parser with loaded data. </returns>
        public static ICsvParser FromStream(Stream stream, CsvParserOptions options = null)
        {
            var lines = DataImporter.FromStream(stream);
            return new CsvParser(lines, options);
        }

        /// <summary> Create CSV Parser with data loaded as string lines from string. </summary>
        /// <param name="data"> String data with new line characters. </param>
        /// <param name="options"> Parser options. </param>
        /// <returns> Interface for CSV Parser with loaded data. </returns>
        public static ICsvParser FromString(string data, CsvParserOptions options = null)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException($"Unable to load null or empty data.");

            var lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return new CsvParser(lines, options);
        }

        #endregion CREATE METHODS

        #region PARSE FROM CSV TO CLASS METHODS

        /// <summary> Parse CSV data to List (rows) of class instances. </summary>
        /// <typeparam name="TDataModel"> Type of class to hold CSV data. </typeparam>
        /// <param name="fieldsMapping"> Optional mapping dictionary where key is CSV header and value is class field name. </param>
        /// <returns> List of class instances with CSV data. </returns>
        public List<TDataModel> ParseToClass<TDataModel>(Dictionary<string, string> fieldsMapping = null)
            where TDataModel : class
        {
            //  Create empty result data.
            var result = new List<TDataModel>();

            //  Get headers.
            var columnsCount = ResolveColumnsCount();
            var headers = _options.UseHeaders ?
                ResolveHeaders() : (from i in Enumerable.Range(0, columnsCount) select $"{_options.DefaultFieldName}{i}").ToList();

            //  Create ClassResolver instance with options and headers.
            var classResolver = new ClassResolver<TDataModel>(_options, headers);

            //  Setup mapping.
            if (fieldsMapping != null)
                classResolver.UseMapping(fieldsMapping);

            foreach (var rowIndex in Enumerable.Range(0, _rawData.Length))
            {
                //  Ommit first line of data if headers option is in use.
                if (rowIndex == 0 && _options.UseHeaders)
                    continue;

                //  Split line into fields.
                var splittedData = _rawData[rowIndex]
                    .Split(new[] { _options.ColumnDelimiter }, StringSplitOptions.None)
                    .ToList();

                //  Replace unexpected data.
                splittedData = StringListTools.ReplaceItems(splittedData, null, "");

                //  Add single data row.
                result.Add(classResolver.ParseCSVRow(splittedData, rowIndex));
            }

            return result;
        }

        #endregion PARSE FROM CSV TO CLASS METHODS

        #region PARSE FROM CSV TO STRINGS ARRAY

        /// <summary> Parse CSV data to List (rows) of strings fields (cols). </summary>
        /// <returns> List of strings with CSV data. </returns>
        public List<string[]> ParseToStringsArray()
        {
            //  Create empty result data.
            var result = new List<string[]>();

            //  Get columns count.
            var columnsCount = ResolveColumnsCount();
            
            //  Get and add headers.
            if (_options.UseHeaders)
            {
                var headers = ResolveHeaders();

                if (columnsCount > headers.Count)
                {
                    var previousCount = headers.Count;
                    headers.AddRange(from i in Enumerable.Range(0, columnsCount - headers.Count) select $"{_options.DefaultFieldName}{previousCount + i}");
                }
                result.Add(headers.ToArray());
            }

            foreach (var rowIndex in Enumerable.Range(0, _rawData.Length))
            {
                //  Ommit first line of data if headers option is in use.
                if (rowIndex == 0 && _options.UseHeaders)
                    continue;

                //  Split line into fields.
                var splittedData = _rawData[rowIndex]
                    .Split(new[] { _options.ColumnDelimiter }, StringSplitOptions.None)
                    .ToList();

                //  Replace unexpected data.
                splittedData = StringListTools.ReplaceItems(splittedData, null, "");

                //  Expand size of columns if necessary.
                if (columnsCount > splittedData.Count)
                    splittedData = StringListTools.ExpandList(splittedData, columnsCount, "");

                //  Add single data row.
                result.Add(splittedData.ToArray());
            }

            return result;
        }

        #endregion PARSE FROM CSV TO STRINGS ARRAY

        #region PARSE FROM CSV TO DICTIONARY LIST

        /// <summary> Parse CSV data to List (rows) of dictionaries (cols) contains column name and value. </summary>
        /// <returns> List od dictionaries with CSV data. </returns>
        public List<Dictionary<string, string>> ParseToDictList()
        {
            //  Create empty result data.
            var result = new List<Dictionary<string, string>>();

            //  Get headers.
            var columnsCount = ResolveColumnsCount();
            var headers = _options.UseHeaders ?
                ResolveHeaders() : (from i in Enumerable.Range(0, ResolveColumnsCount()) select $"{_options.DefaultFieldName}{i}").ToList();

            //  Update headers.
            if (columnsCount > headers.Count)
            {
                var previousCount = headers.Count;
                headers.AddRange(from i in Enumerable.Range(0, columnsCount - headers.Count) select $"{_options.DefaultFieldName}{previousCount + i}");
            }

            foreach (var rowIndex in Enumerable.Range(0, _rawData.Length))
            {
                //  Ommit first line of data if headers option is in use.
                if (rowIndex == 0 && _options.UseHeaders)
                    continue;

                //  Create single data row.
                var rowResult = new Dictionary<string, string>();

                //  Split line into fields.
                var splittedData = _rawData[rowIndex]
                    .Split(new[] { _options.ColumnDelimiter }, StringSplitOptions.None)
                    .ToList();

                //  Replace unexpected data.
                splittedData = StringListTools.ReplaceItems(splittedData, null, "");

                //  Expand size of columns if necessary.
                if (columnsCount > splittedData.Count)
                    splittedData = StringListTools.ExpandList(splittedData, columnsCount, "");

                //  Fill single data row with data
                foreach (var fieldIndex in Enumerable.Range(0, columnsCount))
                    rowResult.Add(headers[fieldIndex], splittedData[fieldIndex]);

                //  Add single data row.
                result.Add(rowResult);
            }

            return result;
        }

        #endregion PARSE FROM CSV TO DICTIONARY LIST

        #region TOOL METHODS

        /// <summary> Convert and get first line of CSV data as list of headers. </summary>
        /// <returns> List of headers. </returns>
        private List<string> ResolveHeaders()
        {
            //  Validate ability to resolve headers.
            if (_rawData.Length < 1)
                throw new ArgumentException("Headers cannot be resolved due to lack of required data length, one line.");

            //  Resolve headers.
            var headers = _rawData[0].Split(new[] { _options.ColumnDelimiter }, StringSplitOptions.None);

            //  Validate headers.
            foreach (var header in headers)
            {
                //  Check if header is null, empty, or whitespace.
                if (string.IsNullOrEmpty(header) || string.IsNullOrWhiteSpace(header))
                    throw new ArgumentException("Headers cannot contains an empty header.");

                var firstChar = header.FirstOrDefault();
                var contansChars = header.Where(c => StringPatterns.RestrictedCharactersPattern.Contains(c));

                //  Check if header contains forbidden character.
                if (contansChars != null && contansChars.Any())
                    throw new ArgumentException($"Headers cannot contains header {header} witch character {contansChars.FirstOrDefault()}.");

                //  Check if header contains number as first character.
                if (StringPatterns.NumericPattern.Contains($"{contansChars}"))
                    throw new ArgumentException($"Headers cannot contains header {header} witch first numeric character.");
            }

            //  Return headers array as headers list avalible for dynamic update.
            return new List<string>(headers);
        }

        /// <summary> Get number of columns from CSV data. </summary>
        /// <returns> Number of columns. </returns>
        private int ResolveColumnsCount()
        {
            if (_rawData.Length > 0)
                return _rawData.Max(r => r.Split(new[] { _options.ColumnDelimiter }, StringSplitOptions.None).Count());
            return 0;
        }

        #endregion TOOL METHODS

        #region VALIDATION METHODS

        /// <summary> Walidacja opcji. </summary>
        public void OptionsValidation(CsvParserOptions options = null)
        {
            if (options == null)
                return;

            if (options.ColumnDelimiter == options.ListDelimiter)
                throw new ArgumentException("Invalid options, Colunm delimiter cannot be the same as List delimiter.");
            if (options.ListDelimiter == options.FloatingPointNumberDelimiter)
                throw new ArgumentException("Invalid options, List delimiter cannot be the same as Floating Point Number delimiter.");
            if (options.ColumnDelimiter == options.FloatingPointNumberDelimiter)
                throw new ArgumentException("Invalid options, Colunm delimiter cannot be the same as Floating Point Number delimiter.");
        }

        #endregion VALIDATION METHODS

    }
}
