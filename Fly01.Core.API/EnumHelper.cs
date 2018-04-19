using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;

namespace Fly01.Core.API
{
    public static class EnumHelper
    {
        public static APIEnumData SubtitleDataAnotation(string enumName, string itemValue)
        {
            var filterObjects = RestHelper.ExecuteGetRequest<IEnumerable<APIEnumData>>(enumName).SingleOrDefault(x => x.Key.Equals(itemValue));
            return filterObjects;
        }

        private static SubtitleAttribute SubtitleDataAnotation(this Enum value)
        {
            try
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);

                var subtitleAttribute = type.GetField(name)
                    .GetCustomAttributes(false)
                    .OfType<SubtitleAttribute>()
                    .SingleOrDefault();

                return subtitleAttribute ?? new SubtitleAttribute("", "", "");
            }
            catch (Exception)
            {
                return new SubtitleAttribute("", "", "");
            }
        }

        public static string GetTitle(this Enum value)
        {
            try
            {
                var result = SubtitleDataAnotation(value);

                return result.Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetCSS(this Enum value)
        {
            try
            {
                var result = SubtitleDataAnotation(value);

                return result.CssClass;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetDescription(this Enum value)
        {
            try
            {
                var result = SubtitleDataAnotation(value);

                return result.Value == "" ? result.Description : result.Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static T ToEnum<T>(this string value)
        {
            //string
            return (T)Enum.Parse(typeof(T), value, true);

            //char || int
            //int intValue;
            //Type enumType = typeof(T);
            //T enumValue = (T)Enum.ToObject(enumType, (int.TryParse(value, out intValue) ? intValue : value[0]));
            //return enumValue;
        }

        public static Dictionary<object, object> GetDescriptionEnumValues(Type enumeratorType)
        {
            var enumValues = new Dictionary<object, object>();

            for (int i = 0; i < Enum.GetValues(enumeratorType).Length; i++)
            {
                var name = Enum.GetName(enumeratorType, Enum.GetValues(enumeratorType).GetValue(i));

                var val = enumeratorType.GetField(name)
                        .GetCustomAttributes(false)
                        .OfType<DescriptionAttribute>()
                        .SingleOrDefault();

                enumValues.Add(name, val.Description);
            }

            return enumValues;
        }

        public static List<SubtitleAttribute> GetDataEnumValues(Type enumType)
        {
            var list = new List<SubtitleAttribute>();

            foreach (Enum item in Enum.GetValues(enumType))
            {
                list.Add(SubtitleDataAnotation(item));
            }

            return list;
        }
    }
}