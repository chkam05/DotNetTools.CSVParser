# DotNetTools.CSVParser

.Net Tool Library written in C# language for CSV file data parsing.  
Library allows to read data from CSV File/Stream/string into multiple data types such as:

- List of string arrays
- List of dictionaries
- Class instance

## Initializing library :

Library does not contains typical initializer, but few creators, dependent on csv data source:

``` C#
//  Read from file creator.
var parser = CsvParser.FromFile("path_to_file.csv");

//  Read from stream creator.
var parser = CsvParser.FromStream(memoryStream);

//  Read from string creator.
var parser = CsvParser.FromString("csv_raw_data");
```

## Options :

Depending on how csv data will be read and to what type of data will be saved, several options are available to change. Options are optional and they are passed as a parameter into creator methods.

``` C#
var options = new CsvParserOptions() {
    //  configure options here
}

var parser = CsvParser.FromFile("path_to_file.csv", options);
```

* ```string ColumnDelimiter```  
Defines character that separates columns,  

* ```string DateTimePattern```  
Defines pattern for reading date time values,  

* ```string DefaultFieldName```  
Defines output column name if csv data doesnt have any headers (column names),  

* ```string FloatingPointNumberDelimiter```  
Defines character that separates integers from decimals numbers in floating point numbers,  

* ```bool IncludeMappingLetterSize```  
Option used in mapper to ignore case sensitivity of csv headers and class field names,  

* ```string ListDelimiter```  
Defines character that separates multiple values in csv data fields,  

* ```bool UseAutoDateConversion```  
Instead of DateTimePattern, CsvParser can automatically detect and convert value from csv data into DateTime type,

* ```bool UseTimeSpanInsteadOfString```  
Enable DateTime conversion from floating point or integer numbers (TimeSpan),

* ```bool UseHeaders```  
Read headers from csv data if they exists (first line of csv data).  

## Mapper :

Mapper is only used when csv data are converting to a class based model instance.

``` C#
//  Csv data will contain headers: (identifier; name; surname)

class DataModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

class Program
{
    public class Main()
    {
        var options = new CsvParserOptions()
        {
            ColumnDelimiter = ";",
            UseHeaders = true
        }

        var mappint = new Dictionary<string, string>()
        {
            { "identifier", "Id" },
            { "name", "FirstName" },
            { "surname", "LastName" }
        }

        var parser = CsvParser.FromFile("path_to_file.csv", options);
        List<DataModel> result = parser.ParseToClass<DataModel>(mapping);
    }
}
```

If csv data headers and class based model fields names are identical, a custom mapper is not required

``` C#
parser.ParseToClass<DataModel>();
```

but if the headers and names differ in case, just set option ```IncludeMappingLetterSize``` to *false* because it is automatically set to *true*.

## Usage :

Here are some examples of using the library in code.  

### Converting CSV data from file to list of string arrays.  

``` C#
class Program
{
    public class Main()
    {
        var parser = CsvParser.FromFile("path_to_file.csv");
        List<string[]> result = parser.ParseToStringsArray();

        //  Print headers.
        Console.WriteLine(string.Join(", ", result[0]));

        //  Print data
        for (var i = 1; i < result.Count; i++)
            Console.WriteLine(string.Join(", ", result[i]));
    }
}
```

### Converting CSV data from file to list of dicts.  

``` C#
//  Csv data will contain headers: (identifier; name; surname)

class Program
{
    public class Main()
    {
        var parser = CsvParser.FromFile("path_to_file.csv");
        var result = parser.ParseToDictList();

        //  Print headers.
        Console.WriteLine(string.Join(", ", result[0].Keys.ToList()));

        //  Print data.
        for (var i = 1; i < result.Count; i++)
        {
            //  Get row identifier and field by header name.
            Console.WriteLine($"Id = {(result[0]["identifier"])}");
            Console.WriteLine($"FirstName = {(result[0]["name"])}");
            Console.WriteLine($"LastName = {(result[0]["surname"])}");
        }
    }
}
```

### Converting CSV data from file to list of dicts without headers.  

``` C#
//  Csv data will contain 3 data columns limited by ";" character

class Program
{
    public class Main()
    {
        var options = new CsvParserOptions()
        {
            //  Define field name that will appear in the dictionary.
            DefaultFieldName = "field_";

            //  Disable reading headers from file.
            UseHeaders = false;
        };

        var parser = CsvParser.FromFile("path_to_file.csv", options);
        var result = parser.ParseToDictList();

        //  Print headers (headers will be field_0, field_1, etc.)
        Console.WriteLine(string.Join(", ", result[0].Keys.ToList()));

        //  Print data.
        for (var i = 1; i < result.Count; i++)
        {
            //  Get row identifier and field by header name.
            Console.WriteLine($"Id = {(result[0]["field_0"])}");
            Console.WriteLine($"FirstName = {(result[0]["field_1"])}");
            Console.WriteLine($"LastName = {(result[0]["field_2"])}");
        }
    }
}
```

### Advanced converting CSV data from file to list of class based data models.

``` C#
//  Csv data will contain headers: (identifier; name; surname; birthdate; height; phone_number)
//  Example: (0; Kamil; Karpiński; 1994.12.05; 1.83; 123456789)
class DataModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public double Height { get; set; }
    public long Phone { get; set; } 
}

class Program
{
    public class Main()
    {
        var options = new CsvParserOptions()
        {
            ColumnDelimiter = ";",
            FloatingPointNumberDelimiter = ".",
            IncludeMappingLetterSize = false,
            UseAutoDateConversion = true,
            UseHeaders = true
        };

        //  If IncludeMappingLetterSize is set to false,
        //  capitalization does not matter even in mapping.
        var mapping = new Dictionary<string, string>()
        {
            { "identifier", "Id" },
            { "name", "FirstName" },
            { "surname", "LastName" },
            { "birthdate", "BirthDate" },
            { "height", "Height" },
            { "phone_number", "Phone" },
        }

        var parser = CsvParser.FromFile("path_to_file.csv", options);
        List<DataModel> result = parser.ParseToClass<DataModel>(mapping);

        //  Print headers are not avaliable. But you can use mapper data.
        Console.WriteLine(string.Join(", ", mapping.Values.ToList()));

        //  Print data.
        for (var i = 1; i < result.Count; i++)
        {
            //  Get row identifier and field by header name.
            Console.WriteLine($"ID = {result.Id}");
            Console.WriteLine($"FirstName = {result.FirstName}");
            Console.WriteLine($"LastName = {result.LastName}");
            Console.WriteLine($"BirthDate = {result.BirthDate}");
            Console.WriteLine($"Height = {result.Height}");
            Console.WriteLine($"Phone = {result.Phone}");
        }
    }
}
```

For more examples and information, see Unit Tests code.
___
Copyright (c) 2020 Kamil Karpiński  
MIT License