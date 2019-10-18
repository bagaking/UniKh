/** ColorExtension.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/18 12:55:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.utils;
using UnityEngine;

namespace UniKh.extensions {
    public static class ColorExtension {

        public static string ToHex(this Color32 color) {
            var builder = SGen.New["#"][color.r.ToString("X2")][color.g.ToString("X2")][color.b.ToString("X2")];
            if (color.a != 255) builder.Append(color.a.ToString("X2"));
            return builder.End;
        }

        public static Color FromHex(this Color color, string hex) {
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF 
            if (!hex.StartsWith("#")) return Color.white;
            byte a = 255;
            var r = byte.Parse(hex.Length > 6 ? hex.Substring(1, 2) : hex.Substring(1, 1) + "0",
                System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hex.Length > 6 ? hex.Substring(3, 2) : hex.Substring(2, 1) + "0",
                System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hex.Length > 6 ? hex.Substring(5, 2) : hex.Substring(3, 1) + "0",
                System.Globalization.NumberStyles.HexNumber);

            if (hex.Length == 9)
                a = byte.Parse(hex.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
            else {
                if (hex.Length == 4)
                    a = byte.Parse(hex.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r,g,b,a);
        }

    }
}
