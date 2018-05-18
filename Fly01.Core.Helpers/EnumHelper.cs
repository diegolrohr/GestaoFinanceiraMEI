using Fly01.Core.Helpers.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Fly01.Core.Helpers
{
    public static class EnumHelper
    {
        public static APIEnumData SubtitleDataAnotation(Type enumType, string itemValue)
        {
            var items = GetDataEnumValues(enumType).Select(x => new APIEnumData() {Key = x.Key, Value = x.Value, CssClass = x.CssClass, Description = x.Description });

            return items.SingleOrDefault(x => x.Key.Equals(itemValue, StringComparison.InvariantCultureIgnoreCase));
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

        public static string GetDescription(Type enumType, string value)
        {
            try
            {
                return SubtitleDataAnotation(enumType, value)?.Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetCSS(Type enumType, string value)
        {
            try
            {
                return SubtitleDataAnotation(enumType, value)?.CssClass;
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
            return (T)Enum.Parse(typeof(T), value, true);
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
                list.Add(SubtitleDataAnotation(item));

            return list;
        }
    }
}