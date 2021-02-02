using System;
using System.Collections.Generic;

namespace Finale.Json {
    static class ModelConverter {
        static object ConvertType(object value, Type targetType) {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                if (value == null) return null;
                return ConvertType(value, Nullable.GetUnderlyingType(targetType));
            }

            if (targetType.IsEnum) return Enum.ToObject(targetType, value);

            return Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.InvariantCulture);
        }


        public static object ToModel(object source, Type targetType) {
            if (source == null) return null;
            if (targetType.IsInterface || targetType.IsAbstract) throw new ArgumentException("Cannot initiate interface or abstract type " + targetType);
            var sourceType = source.GetType();

            if (source is JsonArray) {
                var ja = source as JsonArray;

                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)) {
                    var elementType = targetType.GetGenericArguments()[0];
                    var listType = typeof(List<>).MakeGenericType(new[] { elementType });
                    var list = Activator.CreateInstance(listType);

                    var method = listType.GetMethod("Add");
                    for (var i = 0; i < ja.Count; ++i) method.Invoke(list, new[] { ToModel(ja[i], elementType) });

                    return list;
                }
                throw new NotSupportedException("Target type of JSON array must be List<>.");
            }


            if (source is JsonObject) {
                var target = Activator.CreateInstance(targetType);
                var jo = source as JsonObject;

                foreach (var f in targetType.GetFields()) {
                    if (jo.ContainsKey(f.Name)) f.SetValue(target, ToModel(jo[f.Name], f.FieldType));
                }
                foreach (var p in targetType.GetProperties()) {
                    if (p.CanWrite && jo.ContainsKey(p.Name)) p.SetValue(target, ToModel(jo[p.Name], p.PropertyType), null);
                }

                return target;
            }

            return ConvertType(source, targetType);
        }

        public static object ToJson(object source) {
            if (source == null) return null;

            var sourceType = source.GetType();

            //at this point, we know the source is not null. If it is a nullable type, get the underlying type
            if (sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(Nullable<>)) sourceType = Nullable.GetUnderlyingType(sourceType);

            if (Helper.IsValidType(sourceType)) return source;
            if (sourceType.IsEnum) return Convert.ChangeType(source, Enum.GetUnderlyingType(sourceType), System.Globalization.CultureInfo.InvariantCulture);

            var enumerable = source as System.Collections.IEnumerable;
            if (enumerable != null) {
                var ja = new JsonArray();
                foreach (var item in enumerable) ja.Add(ToJson(item));
                return ja;
            }

            if (sourceType.IsClass) {
                var jo = new JsonObject();
                foreach (var f in sourceType.GetFields()) jo[f.Name] = ToJson(f.GetValue(source));
                foreach (var p in sourceType.GetProperties()) {
                    if (p.CanRead) jo[p.Name] = ToJson(p.GetValue(source, null));
                }
                return jo;
            }

            throw new NotSupportedException(string.Format("Conversion from type {0} is not supported", sourceType.Name));
        }
    }
}