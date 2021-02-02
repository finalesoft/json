using System.Text;

namespace Finale.Json {
    class Stringifier {
        const string _newline = "\r\n";
        const string _indent = "\t";

        public static void Stringify(JsonObject jo, StringBuilder sb, bool beautify, int depth = 0) {
            sb.Append('{');

            ++depth;

            bool firstValue = false;
            foreach (var kvp in jo) {
                if (firstValue) sb.Append(',');
                if (beautify) AppendIndent(sb, depth);

                WriteEscapedString(sb, kvp.Key);
                sb.Append(':');
                if (kvp.Value is JsonObject) Stringify(kvp.Value as JsonObject, sb, beautify, depth);
                else if (kvp.Value is JsonArray) Stringify(kvp.Value as JsonArray, sb, beautify, depth);
                else WriteValue(sb, kvp.Value);

                firstValue = true;
            }

            if (beautify) AppendIndent(sb, --depth);
            sb.Append('}');
        }

        public static void Stringify(JsonArray ja, StringBuilder sb, bool beautify, int depth = 0) {
            sb.Append('[');

            ++depth;

            for (int i = 0; i < ja.Count; i++) {
                if (i > 0) sb.Append(',');

                if (beautify) AppendIndent(sb, depth);

                if (ja[i] is JsonObject) Stringify(ja[i] as JsonObject, sb, beautify, depth);
                else if (ja[i] is JsonArray) Stringify(ja[i] as JsonArray, sb, beautify, depth);
                else WriteValue(sb, ja[i]);
            }

            if (beautify) AppendIndent(sb, --depth);
            sb.Append(']');
        }

        static void AppendIndent(StringBuilder sb, int depth) {
            sb.Append(_newline);
            while (depth-- > 0) sb.Append(_indent);
        }

        public static void WriteEscapedString(StringBuilder sb, string s) {
            sb.Append('"');
            for (int i = 0; i < s.Length; i++) {
                char c = s[i];
                switch (c) {
                    case '"':
                        sb.Append("\\\"");
                        continue;
                    case '\\':
                        sb.Append("\\\\");
                        continue;
                    case '\n':
                        sb.Append("\\n");
                        continue;
                    case '\r':
                        sb.Append("\\r");
                        continue;
                    case '\t':
                        sb.Append("\\t");
                        continue;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }

        static void WriteValue(StringBuilder sb, object o) {
            if (o == null) sb.Append("null");
            else if (o is string) WriteEscapedString(sb, o as string);
            else if (o is bool) sb.Append((bool)o ? "true" : "false");
            else sb.Append(o.ToString());
        }
    }
}