using System;

namespace Finale.Json {
    static class Helper {
        static Type[] ValidTypes = new Type[] {typeof(JsonArray),typeof(JsonObject),
                        typeof(string),typeof(bool),typeof(byte),typeof(sbyte),
                        typeof(short),typeof(ushort),typeof(int),typeof(uint),typeof(long),typeof(ulong),
                        typeof(float),typeof(double),typeof(decimal)};

        public static void AssertValidType(object value) {
            if (value == null) return;
            if (!IsValidType(value.GetType())) throw new FormatException("Invalid value type: " + value.GetType().ToString());
        }

        public static bool IsValidType(Type t) {
            for (int i = 0; i < ValidTypes.Length; ++i) if (t == ValidTypes[i]) return true;
            return false;
        }
    }
}