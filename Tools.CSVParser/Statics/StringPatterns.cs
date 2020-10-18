using System;
using System.Collections.Generic;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Statics
{
    class StringPatterns
    {

        public static readonly string NumericPattern = "0123456789";
        public static readonly string AlphabeticalUpperPattern = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static readonly string AlphabeticalLowerPattern = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string RestrictedCharactersPattern = "`~!@#$%^&*()-+=[]{}\\|;:'\"<>,./? ";
        public static readonly string UnrestrictedCharactersPattern = "_";

    }
}
