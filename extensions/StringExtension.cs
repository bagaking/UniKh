using System.Text;
using System.Text.RegularExpressions;
using UniKh.utils;

namespace UniKh.extensions {
    public static class StringExtension {
        public static bool Exists(this string v) {
            return !string.IsNullOrWhiteSpace(v);
        }

        public static string Repeat(this string v, int count) {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++) {
                sb.Append(v);
            }

            return sb.ToString();
        }

        public static string IncNumberTail(this string str, int digitsOfDefaultZeros = 0) {
            Match match;
            if (!str.Exists() || !(match = Regex.Match(str, @"(^.+?)(\d+$)")).Success)
                return digitsOfDefaultZeros > 0
                    ? SGen.New[str].AppendFormat("{0:d" + digitsOfDefaultZeros + "}", 0).End
                    : str;
            
            Capture group = match.Groups[2];
            var result = int.Parse(group.Value) + 1;
            var newLength = result.ToString().Length;
            return SGen.New[str]
                .Remove(group.Index, group.Length)
                .AppendFormat("{0:d" + (newLength > @group.Length ? newLength : @group.Length) + "}", result)
                .End;
        }
    }
}