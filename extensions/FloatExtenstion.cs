namespace UniKh.extensions {
    public static class FloatExtension {
        public static bool AlmostZero(this float v) { return v < 0.0000001f && v > -0.0000001f; }
    }
}