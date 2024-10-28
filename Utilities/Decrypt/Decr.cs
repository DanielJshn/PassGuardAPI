using System.Security.Cryptography;

namespace apief
{
    public class Crypted
    {
        public string DecryptStringAES(string cipherText, string key, string iv)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);
            Console.WriteLine($"Before key{keyBytes}" + $"After{key}");
            Console.WriteLine($"Before iv {ivBytes}" + $"After{iv}");

            if (keyBytes.Length != 32)
                throw new ArgumentException("Invalid key length. Must be 32 bytes for AES-256.");
            if (ivBytes.Length != 16)
                throw new ArgumentException("Invalid IV length. Must be 16 bytes for AES-CBC.");

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
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

        public string EncryptStringAES(string plainText, string base64Key, string base64IV)
        {
            byte[] keyBytes = Convert.FromBase64String(base64Key);
            byte[] ivBytes = Convert.FromBase64String(base64IV);

            if (keyBytes.Length != 32)
                throw new ArgumentException("Invalid key length. Must be 32 bytes for AES-256.");
            if (ivBytes.Length != 16)
                throw new ArgumentException("Invalid IV length. Must be 16 bytes for AES.");

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
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
        }
    }

}
