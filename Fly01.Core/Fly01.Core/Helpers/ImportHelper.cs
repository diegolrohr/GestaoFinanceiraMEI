using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Fly01.Core.Helpers
{
    public static class ImportHelper
    {
        public static string appViewModelName;
        public static string appEntitiesName;

        public static string RenameColumns(HttpPostedFileBase file, string entity)
        {
            var columns = new List<string>();
            var ci1 = new CultureInfo("pt-BR");
            StreamReader reader;

            using (reader = new StreamReader(file.InputStream, Encoding.GetEncoding(ci1.TextInfo.ANSICodePage)))
            {
                var row = reader.ReadLine();
                if (row != null) columns.AddRange(row.Split(';'));

                var properties = GetDisplayEntityColumns(entity);

                var newColumns = new List<string>();
                foreach (var column in columns)
                {
                    try
                    {
                        var property = properties.Single(x => x.GetCustomAttribute<DisplayAttribute>().Name == column);

                        if (property != null)
                        {
                            newColumns.Add(property
                                .GetCustomAttributes<JsonPropertyAttribute>()
                                .Select(p => p.PropertyName)
                                .First());
                        }
                    }
                    catch
                    {
                        newColumns.Add(column);
                    }
                }

                var newFirstRow = string.Join(";", newColumns);
                var newContent = newFirstRow + Environment.NewLine;

                while (!reader.EndOfStream)
                {
                    row = reader.ReadLine();
                    newContent += row + Environment.NewLine;
                }

                return newContent;
            }
        }

        public static IEnumerable<PropertyInfo> GetDisplayEntityColumns(string entity, bool onlyWithJsonProperty = true)
        {
            var entityType = Type.GetType(appViewModelName + entity + "VM," + appEntitiesName);

            if (entityType == null) return null;

            return entityType.GetProperties().Where(x => (onlyWithJsonProperty ? Attribute.IsDefined(x, typeof(DisplayAttribute)) : true) &&
                                                         Attribute.IsDefined(x, typeof(JsonPropertyAttribute)));
        }

        public static string EncodeBase64(string content)
        {
            var newContentBytes = Encoding.ASCII.GetBytes(content);
            var text = Convert.ToBase64String(newContentBytes);
            return text;
        }

        public static string DecodeBase64(string content)
        {
            var data = Convert.FromBase64String(content);
            var text = Encoding.UTF8.GetString(data);
            return text;
        }

        public static DataTable ConvertToDataTable(string data, char delimiter)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var dataTable = new DataTable();
            var endOfFirstLine = data.IndexOf("\n", StringComparison.Ordinal);
            var dataHeader = data.Substring(0, endOfFirstLine).Split(delimiter);
            var startContent = ++endOfFirstLine;
            var endContent = data.Length - startContent;
            var dataContent = data.Substring(startContent, endContent);

            foreach (var header in dataHeader)
            {
                dataTable.Columns.Add(header);
            }

            var sr = new StringReader(dataContent);

            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();
                if (line == null) continue;

                var lineData = line.Split(delimiter);
                var dataRow = dataTable.NewRow();

                for (var i = 0; i < dataHeader.Length; i++)
                {
                    dataRow[i] = lineData[i];
                }

                dataTable.Rows.Add(dataRow);
            }

            sr.Close();
            return dataTable;
        }
    }
}