using System;
using System.Collections.Generic;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Data
{
    public class CsvParserOptions
    {

        //  VARIABLES

        /// <summary> Character that defines colums splits. </summary>
        public string ColumnDelimiter { get; set; } = ";";

        /// <summary> Default CSV DateTime pattern. </summary>
        public string DateTimePattern { get; set; } = "yyyy.MM.dd HH:mm:ss";

        /// <summary> Field name when headers are not in use. </summary>
        public string DefaultFieldName { get; set; } = "Field_";

        /// <summary> Character that defines when floating point number begins it's decimal numbers. </summary>
        public string FloatingPointNumberDelimiter { get; set; } = ".";

        /// <summary> Uppercase and lowercase matters in fields mapping. </summary>
        public bool IncludeMappingLetterSize { get; set; } = true;

        /// <summary> Character that defines list splits. </summary>
        public string ListDelimiter { get; set; } = ",";

        /// <summary> Use Automatic date time conversion from CSV file. </summary>
        public bool UseAutoDateConversion { get; set; } = true;

        /// <summary> Use TimeSpan as date time format in CSV file. </summary>
        public bool UseTimeSpanInsteadOfString { get; set; } = false;

        /// <summary> Use first line of data as data headers. </summary>
        public bool UseHeaders { get; set; } = true;
        
    }
}
