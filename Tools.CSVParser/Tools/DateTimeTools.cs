using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Tools
{
    class DateTimeTools
    {

        //  METHODS

        #region CONVERSION FROM STRING TOOLS METHODS

        public static DateTime AutoConvertFromString(string dateTimeValue)
        {
            try
            {
                return DateTime.Parse(dateTimeValue);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid format or conversion from \"{dateTimeValue}\" string to DateTime value.");
            }
        }

        /// <summary> String conversion to DateTime using formatting. </summary>
        /// <param name="dateTimeValue"> String date time value. </param>
        /// <param name="format"> DateTime format pattern. </param>
        /// <returns> Converted DateTime from string. </returns>
        public static DateTime ConvertFromString(string dateTimeValue, string format)
        {
            try
            {
                return DateTime.ParseExact(dateTimeValue, format, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid format or conversion from \"{dateTimeValue}\" string to DateTime value.");
            }
        }

        /// <summary> TimeSpan conversion to DateTime. </summary>
        /// <param name="timeStamp"> Timespan (seconds). </param>
        /// <returns> Converted DateTime from TimeSpan. </returns>
        public static DateTime ConvertFromTimeStamp(double timeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            return dateTime;
        }

        #endregion CONVERSION FROM STRING TOOLS METHODS

    }
}
