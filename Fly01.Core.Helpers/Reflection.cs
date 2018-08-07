using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fly01.Core.Helpers
{
    public static class Reflection
    {
        public static void CopyProperties<TFrom, TTo>(this TFrom source, TTo destination, params string[] exceptions)
        {
            if (source == null)
                return;

            if (destination == null)
                return;

            var destinationProperties = destination.GetType().GetProperties();
            destinationProperties = exceptions != null ? destinationProperties.Where(x => !exceptions.Contains(x.Name)).ToArray() : destinationProperties;

            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                if (sourcePi != null)
                    SetValue(destinationPi, destination, sourcePi.GetValue(source, null));
            }
        }

        private static void SetValue(PropertyInfo info, object instance, object value)
        {
            var targetType = info.PropertyType.IsNullableType()
                 ? Nullable.GetUnderlyingType(info.PropertyType)
                 : info.PropertyType;

            var convertedValue = value == null ? null : Convert.ChangeType(value, targetType);

            if (info.CanWrite)
                info.SetValue(instance, convertedValue, null);
        }

        public static void CopyProperties<T>(this T source, T destination, params string[] exceptions)
        {
            CopyProperties<T, T>(source, destination, exceptions);
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static List<string> PropertiesFromType(object source)
        {
            if (source == null) return new List<string>();

            return new List<string>(source.GetType().GetProperties().Select(x => x.Name).ToList());
        }

        public static T CopyPropertiesFromJson<T>(T entity, string json)
        {
            if (entity == null) return entity;
            if (string.IsNullOrEmpty(json)) return entity;

            var obj = JObject.Parse(json);

            PropertiesFromType(entity).ForEach(prop =>
            {
                var value = obj.GetValue(prop, StringComparison.InvariantCultureIgnoreCase);

                if (value != null)
                    SetValue(entity.GetType().GetProperty(prop), entity, value.Type == JTokenType.Null ? null : value);
            });

            return entity;
        }
    }
}