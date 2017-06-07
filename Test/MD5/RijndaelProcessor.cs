using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Test.MD5
{
    public class RijndaelProcessor
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

        public static void DecryptDile(string inFile, string outFile, string password)
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

        public static string EncryptString(string input, string password)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
                {
                    algorithm.IV = iv;
                    using (
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(),
                            CryptoStreamMode.Write))
                    {
                        byte[] bytes = UTF32Encoding.Default.GetBytes(input);
                        cryptoStream.Write(bytes,0,bytes.Length);
                        cryptoStream.Flush();
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string DecryptString(string input, string password)
        {
            using (MemoryStream inputStream = new MemoryStream(Convert.FromBase64String(input)))
            {
                using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
                {
                    algorithm.IV = iv;
                    using (
                        CryptoStream cryptoStream = new CryptoStream(inputStream, algorithm.CreateDecryptor(),
                            CryptoStreamMode.Read))
                    {
                       StreamReader sr = new StreamReader(cryptoStream);
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
