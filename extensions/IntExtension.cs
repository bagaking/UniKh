namespace UniKh.extensions {
    public static class IntExtension {
        public static int IntegerPart(this int v, int digit) { return digit > 1 ? v / digit : v; }

        public static int DecimalPart(this int v, int digit) { return digit > 1 ? v % digit : 0; }

        public static float RealPart(this int v, int digit) { return digit > 1 ? (float) v / digit : v; }

        public static int Pow(this int v, uint pow) {
            var ret = 1;
            while (pow != 0) {
                if ((pow & 1) == 1) {
                    ret *= v;
                }

                v *= v;
                pow >>= 1;
            }

            return ret;
        }
        
        
    }
}