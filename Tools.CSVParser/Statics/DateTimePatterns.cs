using System;
using System.Collections.Generic;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Statics
{
    class DateTimePatterns
    {

        public static readonly List<string> Dates = new List<string>()
        {
            "dd MM yyyy",
            "MM dd yyyy",
            "dd M yyyy",
            "M dd yyyy",
            "d MM yyyy",
            "MM d yyyy",
            "d M yyyy",
            "M d yyyy"
        };

        public static readonly List<string> Times = new List<string>()
        {
            "HH mm",
            "HH mm ss",
            "HH mm s",
            "H mm",
            "H mm ss",
            "H mm s",
            "HH m",
            "HH m ss",
            "HH m s",
            "H m",
            "H m ss",
            "H m s"
        };

        public static readonly List<string> Separators = new List<string>()
        {
            " ",
            "-",
            ".",
            ":",
            "_",
            "/"
        };

    }
}
