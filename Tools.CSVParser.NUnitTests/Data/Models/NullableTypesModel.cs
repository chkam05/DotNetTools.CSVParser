using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.CSVParser.NUnitTests.Data.Models
{
    public class NullableTypesModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? Birthdate { get; set; }
        public double? Balance { get; set; }
        public float? Height { get; set; }
        public int? Age { get; set; }
        public long? Seconds { get; set; }

    }
}
