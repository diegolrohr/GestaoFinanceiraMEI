﻿using System;
using System.Reflection;

namespace Fly01.Core.Helpers
{
    public static class Reflection
    {
        public static void CopyProperties<TFrom, TTo>(this TFrom source, TTo destination)
        {
            if (source == null)
                return;

            if (destination == null)
                return;

            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
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

        public static void CopyProperties<T>(this T source, T destination)
        {
            CopyProperties<T, T>(source, destination);
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}