using chkam05.DotNetTools.CSVParser.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace chkam05.DotNetTools.CSVParser
{
    public interface ICsvParser
    {

        #region PARSE FROM CSV METHODS

        /// <summary> Parse CSV data to List (rows) of class instances. </summary>
        /// <typeparam name="TDataModel"> Type of class to hold CSV data. </typeparam>
        /// <param name="fieldsMapping"> Optional mapping list where key is CSV header and value is class field name. </param>
        /// <returns> List of class instances with CSV data. </returns>
        List<TDataModel> ParseToClass<TDataModel>(Dictionary<string, string> fieldsMapping = null)
            where TDataModel : class;

        /// <summary> Parse CSV data to List (rows) of strings fields (cols). </summary>
        /// <returns> List of strings with CSV data. </returns>
        List<string[]> ParseToStringsArray();

        /// <summary> Parse CSV data to List (rows) of dictionaries (cols) contains column name and value. </summary>
        /// <returns> List od dictionaries with CSV data. </returns>
        List<Dictionary<string,string>> ParseToDictList();

        #endregion PARSE FROM CSV METHODS

    }
}
