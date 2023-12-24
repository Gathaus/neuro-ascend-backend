using System.Security.Cryptography;
using System.Text;

namespace Neuro.Application.Helpers;

public static class EncryptionHelper
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("9f8d2b6f8e3a4c8db1a4e247fc6a7eb78fbd9a2e34f5c7b9e2");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("7f1e2d343bd7e9b8a1c2d3e4f");

    public static string Encrypt(string? plainText)
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        var buffer = Convert.FromBase64String(cipherText);

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (var msDecrypt = new MemoryStream(buffer))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}