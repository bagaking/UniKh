namespace UniKh.extensions {
    public static class StringExtension {
        public static bool Exists(this string v) { return !string.IsNullOrWhiteSpace(v); }
    }
}