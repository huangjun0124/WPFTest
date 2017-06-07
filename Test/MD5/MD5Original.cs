using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Test.MD5
{
    class MD5Original
    {
        public static string GetMd5Hash(string input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input))).Replace("-", "");
            }
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer compare = StringComparer.OrdinalIgnoreCase;
            return compare.Compare(hashOfInput, hash) == 0 ? true : false;
        }

        /**
         *首先设计一个足够复杂的密码hashKey，然后将它的MD5值和用户输入密码的MD5值相加，再求一次MD5值作为返回值。
         *经过这个过程以后，密码的长度够了，复杂度也够了，想要通过穷举法来得到真正的密码值其成本也就大大增加了
         */
        public static string GetMd5HashRev1(string input)
        {
            string hashKey = "KdHJ6S@89Hte,.&+{>.45oP";
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                string hashCode = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input))).Replace("-", "")
                                  +
                                  BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(hashKey))).Replace("-", "");
                return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(hashCode))).Replace("-", "");
            }
        }

        public static string GetFileHash(string filePath)
        {
            Stopwatch watch = Stopwatch.StartNew();
            string retHash = "";
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open,FileAccess.Read,FileShare.Read))
                {
                    retHash = BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", "");
                }
            }
            Util.Print("Caculate Elapsed Time :\n" + watch.ElapsedMilliseconds);
            return retHash;
        }
    }
}
