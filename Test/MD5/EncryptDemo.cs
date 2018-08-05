using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Test.MD5
{
    /*
     * 非对称加密的突出优点是用于解密的密钥永远不需要传递给对方，但是他比较复杂，所以加密/解密速度慢，因此只适用于数据量小的场合
     * 对称加密效率高，系统开销小，适合进行大数据量的加/解密。
     */
    class EncryptDemo
    {
        public static void T()
        {
            string fileName = @"cn_windows_10_multiple_editions_version_1607_updated_jul_2016_x86_dvd_9060050.iso";
            string inFilePath = @"C:\Users\jhuang153889\Documents\Software\" + fileName;
            string outFilePath = @".\" + fileName;
            Stopwatch watch = Stopwatch.StartNew();
            SymmetricalEncrypt.EncryptFile(inFilePath, outFilePath, "KeithTest0912");
            Util.Print("Encrypt finished, Elapsed time is : " + watch.ElapsedMilliseconds/1000);    //  172s
            watch.Restart();
            SymmetricalEncrypt.DecryptFile(outFilePath, @".\temp.iso", "KeithTest0912");
            Util.Print("Decrypt finished, Elapsed time is : " + watch.ElapsedMilliseconds / 1000);  //   372s
        }
    }

    public class SymmetricalEncrypt
    {
        static int bufferSize = 128 * 1024;
        //密钥salt
        static byte[] salt = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };
        //初始化向量
        static byte[] iv = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

        //初始化并返回对称加密算法
        static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }

        public static void EncryptFile(string inFile, string outFile, string password)
        {
            using (FileStream infs = File.OpenRead(inFile), outfs = File.Open(outFile, FileMode.OpenOrCreate))
            {
                using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
                {
                    algorithm.IV = iv;
                    using (CryptoStream cryptoStream = new CryptoStream(outfs, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] bytes = new byte[bufferSize];
                        int readSize = -1;
                        while ((readSize = infs.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            cryptoStream.Write(bytes, 0, readSize);
                        }
                        cryptoStream.Flush();
                    }
                }
            }
        }

        public static void DecryptFile(string inFile, string outFile, string password)
        {
            using (FileStream infs = File.OpenRead(inFile), outfs = File.Open(outFile, FileMode.OpenOrCreate))
            {
                using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
                {
                    algorithm.IV = iv;
                    using (CryptoStream cryptoStream = new CryptoStream(infs, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] bytes = new byte[bufferSize];
                        int readSize = -1;
                        int numReads = (int)(infs.Length / bufferSize);
                        int slack = (int)(infs.Length % bufferSize);
                        for (int i = 0; i < numReads; i++)
                        {
                            readSize = cryptoStream.Read(bytes, 0, bytes.Length);
                            outfs.Write(bytes, 0, readSize);
                        }
                        if (slack > 0)
                        {
                            readSize = cryptoStream.Read(bytes, 0, (int)slack);
                            outfs.Write(bytes, 0, readSize);
                        }
                        outfs.Flush();
                    }
                }
            }
        }
    }
}
