﻿using System;
using System.Linq;
using System.Collections.Generic;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Helpers
{
    public static class EnumHelper
    {
        private static APIEnumData SubtitleDataAnotation(Type enumType, string itemValue)
        {
            var items = GetDataEnumValues(enumType).Select(x => new APIEnumData() { Key = x.Key, Value = x.Value, CssClass = x.CssClass, Description = x.Description, TooltipHint = x.TooltipHint });

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

        public static string GetValue(Type enumType, string value)
        {
            try
            {
                return SubtitleDataAnotation(enumType, value)?.Value;
            }
            catch (Exception)
            {
                return string.Empty;
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

        public static string GetTooltipHint(Type enumType, string value)
        {
            try
            {
                var dataValue = SubtitleDataAnotation(enumType, value);

                return dataValue.TooltipHint != "" ? dataValue.TooltipHint : dataValue.Value;
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

        public static List<SubtitleAttribute> GetDataEnumValues(Type enumType)
        {
            var list = new List<SubtitleAttribute>();
            foreach (Enum item in Enum.GetValues(enumType))
                list.Add(SubtitleDataAnotation(item));

            return list;
        }
    }
}