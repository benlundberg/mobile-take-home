using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GuestlogixTestXF.Core
{
    /// <summary>
    /// My custom class for serializing a csv file to a C# class.
    /// I wanted to make it as general as possible.
    /// </summary>
    public class CsvConverter
    {
        /// <summary>
        /// Serialize a stream of CSV file to a list of object of T.
        /// </summary>
        public static IEnumerable<T> Serialize<T>(Stream stream)
        {
            try
            {
                // String to hold property names
                string propertyNames = null;

                // String to hold property values
                string propertyValues = null;

                using (var read = new StreamReader(stream))
                {
                    // The names of the headers will be on first line
                    propertyNames = read.ReadLine();

                    // Here we read the whole file
                    propertyValues = read.ReadToEnd();
                }

                // Remove proprety names
                propertyValues = propertyValues.Replace(propertyNames, "");

                // Remove and split the strings
                var headers = propertyNames.Split(
                    new[] { "\r\n", "\r", "\n", "," },
                    StringSplitOptions.None);

                // Replace line break with comma seperator
                propertyValues = propertyValues.Replace("\r\n", ",");
				propertyValues = propertyValues.Replace("\n", ",");

                // Use regex to trim the csv string
                var regex = new Regex("(?:^|,)(\\\"(?:[^\\\"]+|\\\"\\\")*\\\"|[^,]*)");
                var collection = regex.Matches(propertyValues);
                var properties = new string[collection.Count];
                var i = 0;
                foreach (Match match in collection)
                {
                    properties[i++] = match.Groups[0].Value.Trim('"').Trim(',').Trim('"').Trim();
                }

                // We will pack the content in a dictionary so it will be easy to create object of T
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                // Index to know when one object is completed
                int index = 0;

                foreach (var property in properties)
                {
                    if (index == 0)
                    {
                        list.Add(new Dictionary<string, object>());
                    }

                    list.Last().Add(headers[index], property);

                    index++;

                    if (index >= headers.Length)
                    {
                        index = 0;
                    }
                }

                // We take out the property info from object of T
                PropertyInfo[] propertyInfos = typeof(T).GetProperties();

                List<T> retList = new List<T>();

                // Here we will create a list of the object
                foreach (var item in list)
                {
                    // Create new object
                    var obj = Activator.CreateInstance<T>();

                    foreach (var value in item)
                    {
                        var propertyName = value.Key;
                        var propertyValue = value.Value;

                        if (propertyValue?.ToString() == "\\N")
                        {
                            propertyValue = null;
                        }

                        /* 
                        Here we set the value from the dictionary
                        We both check the name and compare with the property name from csv file.
                        But there is also a csv attribute to compare with. Sometimes the name is not allowed
                        in C#. For example 2DitigCode where 2D is not allowed
                         */
                        obj.GetType().GetProperties()?
                            .FirstOrDefault(x => x.Name.ToLower().Trim() == propertyName.ToLower().Trim() || x.GetCustomAttribute<CsvAttribute>()?.CsvName?.Trim() == propertyName?.Trim())?
                            .SetValue(obj, propertyValue);
                    }

                    retList.Add(obj);
                }

                return retList;
            }
            catch (Exception ex)
            {
                ex.Print();
                throw;
            }
        }
    }
}
