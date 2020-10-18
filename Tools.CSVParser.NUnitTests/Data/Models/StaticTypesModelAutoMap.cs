using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.CSVParser.NUnitTests.Data.Models
{
    public class StaticTypesModelAutoMap
    {

        public string ID { get; set; }
        public string NAME { get; set; }
        public bool ENABLED { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public double BALANCE { get; set; }
        public float HEIGHT { get; set; }
        public int AGE { get; set; }
        public long SECONDS { get; set; }

    }
}
