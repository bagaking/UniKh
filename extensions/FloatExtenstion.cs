namespace UniKh.extensions {
    public static class FloatExtension {
        public static bool AlmostZero(this float v, float threshold = 0.0000001f) {
            return v < threshold && v > -threshold;
        }

        public static bool EqualsTo(this float v, float vAnother, float threshold = 0.0000001f) {
            var offset = v - vAnother;
            return offset < threshold && offset > -threshold;
        }
    }
}