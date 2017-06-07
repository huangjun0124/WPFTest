using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.MD5
{
    class Md5Test
    {
        public static void T()
        {
            string pwd = "FuckTheWorldTo00988";
            string hash = MD5Original.GetMd5Hash(pwd);

            string input = Util.ReadLine();
            bool ret = MD5Original.VerifyMd5Hash(input, hash);
            if (ret)
            {
                Util.Print("PassWord correct");
            }
            else
            {
                Util.Print("Password incorrect");
            }
        }

        public static void T1()
        {
            string fileHash = MD5Original.GetFileHash(
                        @"C:\Users\jhuang153889\Documents\Software\cn_windows_10_multiple_editions_version_1511_x86_dvd_7223635.iso");
            Util.Print("File's MD5-HASH is : {0}", fileHash);
        }
    }
}
