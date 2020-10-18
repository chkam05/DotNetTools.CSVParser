using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.CSVParser.NUnitTests.Data.Models
{
    public class StaticTypesModelCustomMap
    {

        public string Identifier { get; set; }
        public string NameSurname { get; set; }
        public bool Visible { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
        public float Size { get; set; }
        public int CurrentAge { get; set; }
        public long AgeInSeconds { get; set; }

    }
}
