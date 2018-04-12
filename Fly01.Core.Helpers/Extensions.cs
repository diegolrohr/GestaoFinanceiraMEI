using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Fly01.Core.Helpers
{
    public static class Extensions
    {
        public static readonly string DATE_YYYYMMDD = "yyyyMMdd";
        public static readonly string DATE_YYYYMMDD_HHMMSS = "yyyyMMddHHmmss";
        public static readonly string DATE_DDMMYYYY = "ddMMyyyy";
        public static readonly string DATE_DD_MM_YYYY = "dd-MM-yyyy";
        public static readonly string DATE_YYYY_MM_DD = "yyyy-MM-dd";

        public enum DateFormat
        {
            YYYYMMDD,
            DDMMYYYY,
            DD_MM_YYYY,
            YYYY_MM_DD,
            YYYYMMDD_HHMMSS
        }

        //public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, TEnum selectedValue)
        //{
        //    var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        //    var items = from value in values
        //                                        select new SelectListItem()
        //                                        {
        //                                            Text = value.ToString(),
        //                                            Value = value.ToString(),
        //                                            Selected = (value.Equals(selectedValue))
        //                                        };
        //    return htmlHelper.DropDownListFor(expression, items);
        //}

        public static string toExtenso(this double value, ExtensoHelper.TipoValorExtenso tipoValorExtenso = ExtensoHelper.TipoValorExtenso.Monetario)
        {
            return ExtensoHelper.toExtenso(value, tipoValorExtenso);
        }


        private static string GetDateFormat(DateFormat dateFormatIn)
        {
            switch (dateFormatIn)
            {
                default:
                    return DATE_YYYYMMDD;
                case DateFormat.DDMMYYYY:
                    return DATE_DDMMYYYY;
                case DateFormat.DD_MM_YYYY:
                    return DATE_DD_MM_YYYY;
                case DateFormat.YYYY_MM_DD:
                    return DATE_YYYY_MM_DD;
                case DateFormat.YYYYMMDD_HHMMSS:
                    return DATE_YYYYMMDD_HHMMSS;
            }
        }

        public static string toExtenso(this double value)
        {
            return ExtensoHelper.toExtenso(value, ExtensoHelper.TipoValorExtenso.Monetario);
        }

        public static string FormataData(this string data)
        {
            string retorno = "";
            if (data != "" && data != null)
            {
                retorno = data.Substring(0, 2) + "/" + data.Substring(2, 2) + "/" + data.Substring(4, 4);
            }
            return retorno;
        }

        public static string FormatarDataYYYYMMDD(this string data)
        {
            string retorno = "";
            if (data != "")
            {
                retorno = data.Substring(4, 4) + data.Substring(2, 2) + data.Substring(0, 2);
            }
            return retorno;
        }

        public static bool IsDate(this string dateString)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(dateString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsCurrency(this string value)
        {
            try
            {
                if (value.Contains(",") || value.Contains("."))
                {
                    double.Parse(value);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public static DateTime ToDateTime(this string dateString, DateFormat dateFormatIn = DateFormat.YYYYMMDD)
        {
            try
            {
                return DateTime.ParseExact(dateString, GetDateFormat(dateFormatIn), CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return DateTime.MinValue;
            }
        }

        public static string ToStringDate(this string dateString, string dateFormatOut = "dd/MM/yyyy", DateFormat dateformatIn = DateFormat.YYYYMMDD)
        {
            return ToDateTime(dateString, dateformatIn).ToString(dateFormatOut);
        }

        public static string ToStringDateFormat(this string dateString, DateFormat dateFormatOut = DateFormat.YYYYMMDD, DateFormat dateformatIn = DateFormat.YYYY_MM_DD)
        {
            return ToDateTime(dateString, dateformatIn).ToString(GetDateFormat(dateFormatOut));
        }

        public static string ToDateString(this DateTime dateTime, string dateFormatOut = "dd/MM/yyyy")
        {
            return dateTime.ToString(dateFormatOut);
        }

        public static string MaxWords(this string str, int qty, string separator = " ")
        {
            StringBuilder sb = new StringBuilder();
            string[] arr = Regex.Split(str, separator);
            if (arr.Length <= qty)
            {
                foreach (string s in arr)
                    sb.AppendFormat("{0}{1}", s, separator);
            }
            else
            {
                int words = 0;
                foreach (string s in arr)
                {
                    sb.AppendFormat("{0}{1}", s, separator);
                    words++;
                    if (words >= qty)
                        break;
                }
            }
            return sb.ToString();
        }

        public static long GetValue(this DateTime dateTime)
        {
            TimeSpan ts = dateTime.Date - new DateTime(1970, 1, 1).Date;
            return (long)ts.TotalDays;
        }

        public static void AddParam(this List<KeyValuePair<string, string>> list, string key, string value)
        {
            AddParam(list, key, value, true);
        }

        public static void AddParam(this List<KeyValuePair<string, string>> list, string key, string value, bool removeOnExists)
        {
            if (list != null)
            {
                if (removeOnExists)
                {
                    list.Remove(key);
                    list.Add(new KeyValuePair<string, string>(key, value));
                }
                else if (!list.Any(x => x.Key.ToLower().Equals(key.ToLower())))
                {
                    list.Add(new KeyValuePair<string, string>(key, value));
                }
            }
        }

        public static void AddParam(this Dictionary<string, string> list, string key, string value)
        {
            AddParam(list, key, value, true);
        }

        public static void AddParam(this Dictionary<string, string> list, string key, string value, bool removeOnExists)
        {
            if (list != null)
            {
                if (removeOnExists) list.Remove(key);
                list.Add(key, value);
            }
        }

        public static void AddParamAndUpdate(this Dictionary<string, string> list, string key, string value)
        {
            var combinedSearch = list.Any(x => x.Key.Equals("$filter"));

            if (!key.Equals("$filter"))
                list.AddParam(key, value);
            else
            {
                var oldValue = list.Any(x => x.Key == "$filter") ? list.FirstOrDefault(x => x.Key == "$filter").Value + " and " : string.Empty;
                
                list.AddParam("$filter", string.Concat(oldValue, " ", value));
            }
        }

        public static string ReturnFilteredValue(string key, string value, bool mustCompareAsEqual = false, bool dateTimeRange = false)
        {
            if (dateTimeRange && mustCompareAsEqual)
                return string.Format("{0} lt {1}T23:59:59.99Z and {0} gt {1}T00:00:00.00Z or {0} eq {1}", key, value);
            else if (mustCompareAsEqual)
                return string.Format("{0} eq {1}", key, value);
            else
                return string.Format("contains({0}, '{1}') ", key, value);
        }

        public static string ReturnProcessFilter(Dictionary<string, string> filters)
        {
            var result = string.Empty;

            filters.ToList().ForEach(i => result += string.Format("{0} {1}", i.Key, i.Value));

            return result;
        }

        public static void Remove(this List<KeyValuePair<string, string>> list, string key)
        {
            if (list != null)
                list.RemoveAll(x => x.Key.ToLower().Equals(key.ToLower()));
        }

        public static Dictionary<string, string> ToDictionary(this List<KeyValuePair<string, string>> list)
        {
            return list.ToDictionary(x => x.Key, x => x.Value);
        }

        public static string EncodeText(this string s)
        {
            return EncodeText(s, Encoding.Default);
        }

        public static string EncodeText(this string s, Encoding enc)
        {
            if (!string.IsNullOrEmpty(s))
                return HttpUtility.UrlEncode(s, enc);
            return s;
        }

        public static string DecodeText(this string s)
        {
            return DecodeText(s, Encoding.Default);
        }

        public static string DecodeText(this string s, Encoding enc)
        {
            if (!string.IsNullOrEmpty(s))
                return HttpUtility.UrlDecode(s, enc);
            return s;
        }

        public static string ToCleanUrl(this string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                url = url.DecodeText().ToLower().Replace("http://", "").Replace("https://", "").Replace("/manager", "").Trim();
                while (url.EndsWith("/") || url.EndsWith(@"\"))
                {
                    url = url.Remove(url.Length - 1);
                }
            }

            return url;
        }

        public static string ToQueryString(this NameValueCollection collection)
        {
            var array = (from key in collection.AllKeys
                         from value in collection.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();
            return string.Join("&", array);
        }

        public static List<int> ToArrayIds(string[] ids)
        {
            List<int> result = new List<int>();

            foreach (var item in ids)
            {
                result.Add(Convert.ToInt32(item));
            }

            return result;
        }

        public static DateTime ToClientTimeZone(this DateTime data, string timeZoneId = "E. South America Standard Time")
        {
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(data, clientTimeZone);
        }

        public static DateTime ToUniversalTimeZone(this DateTime data, string timeZoneId = "E. South America Standard Time")
        {
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(data, clientTimeZone);
        }
    }
}