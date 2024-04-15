using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecrypt
{
    public class Program
    {
        static void Main(string[] args)
        {
            string originalText = Console.ReadLine(); // Əsas mətn
            string key = "bu_gizli_anahtar"; // Şifrələmə üçün istifadə olunan gizli açar

            // AES alqoritmi ilə şifrələmə
            string encryptedText = Encrypt(originalText, key);
            Console.WriteLine("Şifrelenmiş metn: " + encryptedText);

            // Şifrələnmiş mətndən orijinal mətni çıxarmaq
            string decryptedText = Decrypt(encryptedText, key);
            Console.WriteLine("Orijinal metn: " + decryptedText);
        }
        static string Encrypt(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // IV (İnitialization Vector) təyin edilməlidir, burada sadəlik məqsədiylə sıfırlar istifadə edilir.

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        static string Decrypt(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // IV (İnitialization Vector) təyin edilməlidir, burada sadəlik məqsədiylə sıfırlar istifadə edilir.

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
