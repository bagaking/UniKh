using System.Text;

namespace UniKh.extensions {
    public static class StringExtension {
        public static bool Exists(this string v) { return !string.IsNullOrWhiteSpace(v); }

        public static string Repeat(this string v, int count) {
            var sb = new StringBuilder(); 
            for (var i = 0; i < count; i++) {
                sb.Append(v);
            }

            return sb.ToString();
        }
    }
}