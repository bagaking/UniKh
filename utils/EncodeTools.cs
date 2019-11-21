/** == EncodeTools.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/20 22:26:25
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace UniKh.utils {
    public static class EncodeTools {
        private const string Base58Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        public static string IntToBase58(int num) {
            var builder = SGen.New;
            if (num < 0) return null;
            do {
                var remainder = (int) (num % 58);
                num /= 58;
                builder.Insert(0, Base58Digits[remainder]);
            } while (num > 0);

            return builder.End;
        }

        public static string IntToBase58(string strNum) {
            var builder = SGen.New;
            var num = BigInteger.Parse(strNum);
            if (num < 0) return null;
            do {
                var remainder = (int) (num % 58);
                num /= 58;
                builder.Insert(0, Base58Digits[remainder]);
            } while (num > 0);

            return builder.End;
        }

        public static string HashMd5(string plainText) {
            var bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(plainText));
            var builder = SGen.New;
            foreach (var b in bytes) {
                builder.Append(b.ToString("x2"));
            }
            return builder.End;
        }
    }
}