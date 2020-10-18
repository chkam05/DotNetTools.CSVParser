using chkam05.DotNetTools.CSVParser.Data;
using chkam05.DotNetTools.CSVParser.Statics;
using chkam05.DotNetTools.CSVParser.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Tools
{
    class ClassResolver<TModelData>
        where TModelData : class
    {

        //  VARIABLES

        private readonly List<Type> AvailableTypes = new List<Type>()
        {
            typeof(bool),
            typeof(DateTime),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(List<bool>),
            typeof(List<DateTime>),
            typeof(List<double>),
            typeof(List<float>),
            typeof(List<int>),
            typeof(List<long>),
            typeof(List<string>),
            typeof(long),
            typeof(Nullable<bool>),
            typeof(Nullable<DateTime>),
            typeof(Nullable<double>),
            typeof(Nullable<float>),
            typeof(Nullable<int>),
            typeof(Nullable<long>),
            typeof(string),
        };
        private readonly BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance;

        private Dictionary<string, string> _fieldsMapping = null;
        private List<string> _headers = null;
        private CsvParserOptions _options = null;
        private Type _dataType;
        private PropertyInfo[] _properties;


        //  METHODS

        #region CLASS METHODS

        public ClassResolver(CsvParserOptions options, List<string> headers = null)
        {
            //  Setup data.
            _options = options;
            _headers = headers;
            _dataType = typeof(TModelData);
            _properties = _dataType.GetProperties(BindingFlags);

            //  Run class validation.
            ValidateClassFields();
        }

        #endregion CLASS METHODS

        #region DATA CONVERSION METHODS

        /// <summary> Set specified boolean value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetBool(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(bool))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (valueToLower == "true" || valueToLower == "1")
                property.SetValue(result, true);
            else if (valueToLower == "false" || valueToLower == "0")
                property.SetValue(result, false);
            else
                return ConversionResult.INVALID_CONVERSION;

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified DateTime value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetDateTime(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(DateTime))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            DateTime convertedData = DateTime.Now;

            try
            {
                if (_options.UseTimeSpanInsteadOfString)
                {
                    //  Convert from TimeSpan.
                    var valueWithCorrectDelimiter = valueToLower.Replace(_options.FloatingPointNumberDelimiter, ".");
                    convertedData = DateTimeTools.ConvertFromTimeStamp(double.Parse(valueWithCorrectDelimiter));
                }
                else if (_options.UseAutoDateConversion)
                {
                    //  Auto convert from string.
                    convertedData = DateTimeTools.AutoConvertFromString(valueToLower);
                }
                else
                {
                    //  Convert from string.
                    convertedData = DateTimeTools.ConvertFromString(valueToLower, _options.DateTimePattern);
                }
            }
            catch (Exception)
            {
                return ConversionResult.INVALID_CONVERSION;
            }

            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified double value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetDouble(TModelData result, string fieldName, string value)
        {
            //  Get property, lower value and convert floating point number delimiter.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueWithCorrectDelimiter = value.ToLower().Replace(_options.FloatingPointNumberDelimiter, ",");

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(double))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (double.TryParse(valueWithCorrectDelimiter, out double conversionResult))
            {
                property.SetValue(result, conversionResult);
                return ConversionResult.SUCCESS;
            }

            //  Return result.
            return ConversionResult.INVALID_CONVERSION;
        }

        /// <summary> Set specified float value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetFloat(TModelData result, string fieldName, string value)
        {
            //  Get property, lower value and convert floating point number delimiter.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueWithCorrectDelimiter = value.ToLower().Replace(_options.FloatingPointNumberDelimiter, ",");

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(float))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (float.TryParse(valueWithCorrectDelimiter, out float conversionResult))
            {
                property.SetValue(result, conversionResult);
                return ConversionResult.SUCCESS;
            }

            //  Return result.
            return ConversionResult.INVALID_CONVERSION;
        }

        /// <summary> Set specified int value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetInt(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(int))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (int.TryParse(valueToLower, out int conversionResult))
            {
                property.SetValue(result, conversionResult);
                return ConversionResult.SUCCESS;
            }

            //  Return result.
            return ConversionResult.INVALID_CONVERSION;
        }

        /// <summary> Set specified bool list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetBoolList(TModelData result, string fieldName, string values)
        {
            //  Get property, lower and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = values.ToLower();
            var separatedValues = values.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<bool>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<bool>();

            //  Convert data values.
            foreach (var value in separatedValues)
            {
                if (string.IsNullOrEmpty(value))
                    continue;
                else if (value == "true" || value == "1")
                    convertedData.Add(true);
                else if (value == "false" || value == "0")
                    convertedData.Add(false);
                else
                    return ConversionResult.INVALID_CONVERSION;
            }

            //  Set data and return result.
            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified DateTime list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetDateTimeList(TModelData result, string fieldName, string values)
        {
            //  Get property, lower and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = values.ToLower();
            var separatedValues = values.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<DateTime>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<DateTime>();

            foreach (var value in separatedValues)
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                    else if (_options.UseTimeSpanInsteadOfString)
                    {
                        //  Convert from TimeSpan.
                        var valueWithCorrectDelimiter = value.Replace(_options.FloatingPointNumberDelimiter, ".");
                        var singleConvertedData = DateTimeTools.ConvertFromTimeStamp(double.Parse(valueWithCorrectDelimiter));
                        convertedData.Add(singleConvertedData);
                    }
                    else if (_options.UseAutoDateConversion)
                    {
                        //  Auto convert from string.
                        var singleConvertedData = DateTimeTools.AutoConvertFromString(value);
                        convertedData.Add(singleConvertedData);
                    }
                    else
                    {
                        //  Convert from string.
                        var singleConvertedData = DateTimeTools.ConvertFromString(value, _options.DateTimePattern);
                        convertedData.Add(singleConvertedData);
                    }
                }
                catch (Exception)
                {
                    return ConversionResult.INVALID_CONVERSION;
                }
            }

            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified double list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetDoubleList(TModelData result, string fieldName, string values)
        {
            //  Get property, lower and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = values.ToLower();
            var separatedValues = valueToLower.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<double>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<double>();

            //  Convert data values.
            foreach (var value in separatedValues)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                //  Convert floating point number delimiter.
                var valueWithCorrectDelimiter = value.ToLower().Replace(_options.FloatingPointNumberDelimiter, ",");

                if (double.TryParse(valueWithCorrectDelimiter, out double conversionResult))
                    convertedData.Add(conversionResult);
                else
                    return ConversionResult.INVALID_CONVERSION;
            }

            //  Set data and return result.
            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified float list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetFloatList(TModelData result, string fieldName, string values)
        {
            //  Get property, lower and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = values.ToLower();
            var separatedValues = valueToLower.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<float>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<float>();

            //  Convert data values.
            foreach (var value in separatedValues)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                //  Convert floating point number delimiter.
                var valueWithCorrectDelimiter = value.ToLower().Replace(_options.FloatingPointNumberDelimiter, ",");

                if (float.TryParse(valueWithCorrectDelimiter, out float conversionResult))
                    convertedData.Add(conversionResult);
                else
                    return ConversionResult.INVALID_CONVERSION;
            }

            //  Set data and return result.
            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified int list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetIntList(TModelData result, string fieldName, string values)
        {
            //  Get property, lower and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = values.ToLower();
            var separatedValues = valueToLower.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<int>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<int>();

            //  Convert data values.
            foreach (var value in separatedValues)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                if (int.TryParse(value, out int conversionResult))
                    convertedData.Add(conversionResult);
                else
                    return ConversionResult.INVALID_CONVERSION;
            }

            //  Set data and return result.
            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified long list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetLongList(TModelData result, string fieldName, string values)
        {
            //  Get property, lower and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = values.ToLower();
            var separatedValues = valueToLower.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<long>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<long>();

            //  Convert data values.
            foreach (var value in separatedValues)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                if (long.TryParse(value, out long conversionResult))
                    convertedData.Add(conversionResult);
                else
                    return ConversionResult.INVALID_CONVERSION;
            }

            //  Set data and return result.
            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified string list values in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="values"> Values to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetStringList(TModelData result, string fieldName, string values)
        {
            //  Get property and split values.
            var property = typeof(TModelData).GetProperty(fieldName);
            var separatedValues = values.Split(new[] { _options.ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(List<string>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            var convertedData = new List<string>();

            //  Convert data values.
            foreach (var value in separatedValues)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                convertedData.Add(value);
            }

            //  Set data and return result.
            property.SetValue(result, convertedData);
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified long value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetLong(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(long))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (long.TryParse(valueToLower, out long conversionResult))
            {
                property.SetValue(result, conversionResult);
                return ConversionResult.SUCCESS;
            }

            //  Return result.
            return ConversionResult.INVALID_CONVERSION;
        }

        /// <summary> Set specified nullable boolean value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetNullableBool(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = !string.IsNullOrEmpty(value) ? value.ToLower() : null;

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(Nullable<bool>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (string.IsNullOrEmpty(valueToLower))
                property.SetValue(result, null);
            else if (valueToLower == "true" || valueToLower == "1")
                property.SetValue(result, true);
            else if (valueToLower == "false" || valueToLower == "0")
                property.SetValue(result, false);
            else
                return ConversionResult.INVALID_CONVERSION;

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified nulabble DateTime value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetNullableDateTime(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(Nullable<DateTime>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            try
            {
                if (string.IsNullOrEmpty(valueToLower))
                {
                    property.SetValue(result, null);
                    return ConversionResult.SUCCESS;
                }
                else if (_options.UseTimeSpanInsteadOfString)
                {
                    //  Convert from TimeSpan.
                    var valueWithCorrectDelimiter = valueToLower.Replace(_options.FloatingPointNumberDelimiter, ".");
                    var convertedData = DateTimeTools.ConvertFromTimeStamp(double.Parse(valueWithCorrectDelimiter));
                    property.SetValue(result, convertedData);
                    return ConversionResult.SUCCESS;
                }
                else if (_options.UseAutoDateConversion)
                {
                    //  Auto convert from string.
                    var convertedData = DateTimeTools.AutoConvertFromString(valueToLower);
                    property.SetValue(result, convertedData);
                    return ConversionResult.SUCCESS;
                }
                else
                {
                    //  Convert from string.
                    var convertedData = DateTimeTools.ConvertFromString(valueToLower, _options.DateTimePattern);
                    property.SetValue(result, convertedData);
                    return ConversionResult.SUCCESS;
                }
            }
            catch (Exception)
            {
                return ConversionResult.INVALID_CONVERSION;
            }
        }

        /// <summary> Set specified nullable double value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetNullableDouble(TModelData result, string fieldName, string value)
        {
            //  Get property, lower value and convert floating point number delimiter.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueWithCorrectDelimiter = !string.IsNullOrEmpty(value) ? value.ToLower().Replace(_options.FloatingPointNumberDelimiter, ",") : null;

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(Nullable<double>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (string.IsNullOrEmpty(valueWithCorrectDelimiter))
                property.SetValue(result, null);
            else if (double.TryParse(valueWithCorrectDelimiter, out double conversionResult))
                property.SetValue(result, conversionResult);
            else
                return ConversionResult.INVALID_CONVERSION;

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified nullable float value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetNullableFloat(TModelData result, string fieldName, string value)
        {
            //  Get property, lower value and convert floating point number delimiter.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueWithCorrectDelimiter = !string.IsNullOrEmpty(value) ? value.ToLower().Replace(_options.FloatingPointNumberDelimiter, ",") : null;

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(Nullable<float>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (string.IsNullOrEmpty(valueWithCorrectDelimiter))
                property.SetValue(result, null);
            else if (float.TryParse(valueWithCorrectDelimiter, out float conversionResult))
                property.SetValue(result, conversionResult);
            else
                return ConversionResult.INVALID_CONVERSION;

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified nullable int value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetNullableInt(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(Nullable<int>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (string.IsNullOrEmpty(valueToLower))
                property.SetValue(result, null);
            else if (int.TryParse(valueToLower, out int conversionResult))
                property.SetValue(result, conversionResult);
            else
                return ConversionResult.INVALID_CONVERSION;

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified nullable long value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetNullableLong(TModelData result, string fieldName, string value)
        {
            //  Get property and lower value.
            var property = typeof(TModelData).GetProperty(fieldName);
            var valueToLower = value.ToLower();

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(Nullable<long>))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Convert and set data.
            if (string.IsNullOrEmpty(valueToLower))
                property.SetValue(result, null);
            else if (long.TryParse(valueToLower, out long conversionResult))
                property.SetValue(result, conversionResult);
            else
                return ConversionResult.INVALID_CONVERSION;

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        /// <summary> Set specified string value in output data model. </summary>
        /// <param name="result"> Output data model. </param>
        /// <param name="fieldName"> Field name to set value. </param>
        /// <param name="value"> Value to set. </param>
        /// <returns> Conversion result state. </returns>
        private ConversionResult SetString(TModelData result, string fieldName, string value)
        {
            //  Get property.
            var property = typeof(TModelData).GetProperty(fieldName);

            //  Check if property exist.
            if (property == null)
                return ConversionResult.INVALID_PROPERTY;

            //  Check if property type is correct.
            if (property.PropertyType != typeof(string))
                return ConversionResult.INVALID_PROPERTY_TYPE;

            //  Set data.
            property.SetValue(result, value);

            //  Return result.
            return ConversionResult.SUCCESS;
        }

        #endregion DATA CONVERSION METHODS

        #region MAPPING METHODS

        /// <summary> Create mapping between CSV headers and class fields. </summary>
        private void MakeMapping()
        {
            _fieldsMapping = new Dictionary<string, string>();

            foreach (var propertyInfo in _properties)
            {
                //  Find dependency of the headers names and fields in the class.
                var headerFields = _headers.Where(f => !_options.IncludeMappingLetterSize
                    ? f.ToLower() == propertyInfo.Name.ToLower()
                    : f == propertyInfo.Name);

                //  Assign header name to the class field.
                if (headerFields != null && headerFields.Any())
                    _fieldsMapping.Add(headerFields.FirstOrDefault(), propertyInfo.Name);
            }
        }

        /// <summary> Setup custom mapping between CSV and class fields. </summary>
        /// <param name="fieldsMapping"> List of fields mapping (key: CSV, value: class). </param>
        /// <returns> ClassResolver with custom mapping. </returns>
        public ClassResolver<TModelData> UseMapping(Dictionary<string, string> fieldsMapping = null)
        {
            //  Setup mapping.
            _fieldsMapping = fieldsMapping;

            //  Validate custom mapping.
            ValidateCustomFieldsMapping();

            return this;
        }

        #endregion MAPPING METHODS

        #region PARSE METHODS

        public TModelData ParseCSVRow(List<string> csvRowFields, int rowIndex)
        {
            //  Check if mapping configuration is setted up.
            if (!_options.UseHeaders && _fieldsMapping == null)
                throw new ArgumentException("For non headers configuration, mapping is needed.");

            //  Setup mapping if manual mapping is not used.
            if (_headers != null && _fieldsMapping == null)
                MakeMapping();

            //  Create response object;
            TModelData result = (TModelData) Activator.CreateInstance(typeof(TModelData));

            foreach (var i in Enumerable.Range(0, csvRowFields.Count))
            {
                //  Check that the data field position does not exceed the length of the headers.
                if (i >= _headers.Count)
                    continue;
                var header = _headers[i];

                //  Check if the header is included in the mapping.
                if (!_fieldsMapping.ContainsKey(header))
                    continue;
                var fieldName = _fieldsMapping[header];

                //  Get property type and check if exist.
                var propertyInfo = typeof(TModelData).GetProperty(fieldName);
                if (propertyInfo == null)
                    continue;

                //  Check, convert and set data.
                var conversionResult = ConversionResult.SUCCESS;

                #region Check, convert and set data

                if (propertyInfo.PropertyType == typeof(bool))
                    conversionResult = SetBool(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(DateTime))
                    conversionResult = SetDateTime(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(double))
                    conversionResult = SetDouble(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(float))
                    conversionResult = SetFloat(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(int))
                    conversionResult = SetInt(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<bool>))
                    conversionResult = SetBoolList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<DateTime>))
                    conversionResult = SetDateTimeList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<double>))
                    conversionResult = SetDoubleList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<float>))
                    conversionResult = SetFloatList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<int>))
                    conversionResult = SetIntList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<long>))
                    conversionResult = SetLongList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(List<string>))
                    conversionResult = SetStringList(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(long))
                    conversionResult = SetLong(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(Nullable<bool>))
                    conversionResult = SetNullableBool(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(Nullable<DateTime>))
                    conversionResult = SetNullableDateTime(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(Nullable<double>))
                    conversionResult = SetNullableDouble(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(Nullable<float>))
                    conversionResult = SetNullableFloat(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(Nullable<int>))
                    conversionResult = SetNullableInt(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(Nullable<long>))
                    conversionResult = SetNullableLong(result, fieldName, csvRowFields[i]);

                else if (propertyInfo.PropertyType == typeof(string))
                    conversionResult = SetString(result, fieldName, csvRowFields[i]);

                #endregion Check, convert and set data

                //  Validate conversion.
                switch (conversionResult)
                {
                    case ConversionResult.SUCCESS:
                        break;
                    case ConversionResult.INVALID_CONVERSION:
                    case ConversionResult.INVALID_CONVERSION_SERIES:
                        throw new ArgumentException($"Invalid [{propertyInfo.PropertyType}] conversion on index {rowIndex}", $"{header}:{fieldName}");
                    case ConversionResult.INVALID_PROPERTY:
                    case ConversionResult.INVALID_PROPERTY_TYPE:
                        throw new ArgumentException($"Invalid [{propertyInfo.PropertyType}] output model field", $"{header}:{fieldName}");
                }
            }

            return result;
        }

        #endregion PARSE METHODS

        #region VALIDATION METHODS

        /// <summary> Check if class contains only supported fields for CSV data parsing. </summary>
        private void ValidateClassFields()
        {
            foreach (var propertyInfo in _properties)
                if (!AvailableTypes.Contains(propertyInfo.PropertyType))
                    throw new ArgumentException($"{_dataType.Name} class, contains illegal {propertyInfo.PropertyType.Name} data type.");
        }

        /// <summary> Check if custom mapping configuration is correct. </summary>
        private void ValidateCustomFieldsMapping()
        {
            //  Create lists of checked mapping fields.
            var usedKeysStack = new List<string>();
            var usedValuesStack = new List<string>();

            foreach (var kvp in _fieldsMapping)
            {
                //  Update data for mapping validation.
                var mapping = kvp;

                if (!_options.IncludeMappingLetterSize)
                    mapping = new KeyValuePair<string, string>(kvp.Key.ToLower(), kvp.Value.ToLower());

                #region Empty field names validation

                //  Check if fields has correct names.
                if (string.IsNullOrEmpty(mapping.Key) || string.IsNullOrWhiteSpace(mapping.Key))
                    throw new ArgumentException("Mapping configuration contains an empty CSV field name.");

                if (string.IsNullOrEmpty(mapping.Value) || string.IsNullOrWhiteSpace(mapping.Value))
                    throw new ArgumentException("Mapping configuration contains an empty class field name.");

                #endregion Empty field names validation

                #region Forbidden characters in field names validation

                //  Check if fields contains forbidden character.
                var contansCharsKey = mapping.Key.Where(c => StringPatterns.RestrictedCharactersPattern.Contains(c));
                var contansCharsValue = mapping.Value.Where(c => StringPatterns.RestrictedCharactersPattern.Contains(c));

                if (contansCharsKey != null && contansCharsKey.Any())
                    throw new ArgumentException($"Mapping configuration contains {mapping.Key} CSV field name witch character {contansCharsKey.FirstOrDefault()}.");

                if (contansCharsValue != null && contansCharsValue.Any())
                    throw new ArgumentException($"Mapping configuration contains {mapping.Value} class field name witch character {contansCharsValue.FirstOrDefault()}.");

                //  Check if fields contains number as first character.
                var firstCharKey = mapping.Key.FirstOrDefault();
                var firstCharValue = mapping.Value.FirstOrDefault();

                if (StringPatterns.NumericPattern.Contains($"{firstCharKey}"))
                    throw new ArgumentException($"Headers cannot contains {mapping.Key} CSV field witch first numeric character.");

                if (StringPatterns.NumericPattern.Contains($"{firstCharValue}"))
                    throw new ArgumentException($"Headers cannot contains {mapping.Value} class field witch first numeric character.");

                #endregion Forbidden characters in field names validation

                #region Correct field names validation

                if (_headers != null)
                {
                    var hasCSVField = _headers.Where(
                        h => (!_options.IncludeMappingLetterSize ? h.ToLower() : h) == mapping.Key);

                    if (!hasCSVField.Any())
                        throw new ArgumentException($"Mapping configuration contains field name {mapping.Key} not included in CSV fields.");
                }
                else
                {
                    var containsName = mapping.Key.IndexOf(_options.DefaultFieldName);
                    var initalLength = _options.DefaultFieldName.Length;
                    var keyEndLength = mapping.Key.Length - _options.DefaultFieldName.Length;

                    if (containsName != 0 || int.TryParse(mapping.Key.Substring(initalLength, keyEndLength), out int num))
                        throw new ArgumentException($"Mapping configuration contains field name {mapping.Key} inconsistent with default CSV field name {_options.DefaultFieldName} + field number.");
                }

                var hasClassField = _properties.Where(
                    p => (!_options.IncludeMappingLetterSize ? p.Name.ToLower() : p.Name) == mapping.Value);

                if (!hasClassField.Any())
                    throw new ArgumentException($"Mapping configuration contains field name {mapping.Value} not included in class fields.");

                #endregion Correct field names validation

                #region Duplicated field names validation

                //  Check if fields are not duplicated.
                if (usedKeysStack.Contains(mapping.Key))
                    throw new ArgumentException($"Mapping configuration contains duplicated {mapping.Key} CSV field name.");

                if (usedValuesStack.Contains(mapping.Value))
                    throw new ArgumentException($"Mapping configuration contains duplicated {mapping.Value} class field name.");

                usedKeysStack.Add(mapping.Key);
                usedValuesStack.Add(mapping.Value);

                #endregion Duplicated field names validation
            }
        }

        #endregion VALIDATION METHODS

    }
}
