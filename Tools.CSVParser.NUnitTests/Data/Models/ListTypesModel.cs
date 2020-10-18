using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.CSVParser.NUnitTests.Data.Models
{
    public class ListTypesModel
    {

        public string Id { get; set; }
        public List<string> Name { get; set; }
        public List<bool> Enabled { get; set; }
        public List<DateTime> Birthdate { get; set; }
        public List<double> Balance { get; set; }
        public List<float> Height { get; set; }
        public List<int> Age { get; set; }
        public List<long> Seconds { get; set; }

    }
}
