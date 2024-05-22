using System;
using System.Security.Cryptography;
using System.Text;

public class TripleDESAlgorithm
{
    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        using (var tripleDes = new TripleDESCryptoServiceProvider())
        {
            tripleDes.Key = key;
            tripleDes.Mode = CipherMode.ECB;
            tripleDes.Padding = PaddingMode.Zeros;
            using (var encryptor = tripleDes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }

    public static byte[] Decrypt(byte[] data, byte[] key)
    {
        using (var tripleDes = new TripleDESCryptoServiceProvider())
        {
            tripleDes.Key = key;
            tripleDes.Mode = CipherMode.ECB;
            tripleDes.Padding = PaddingMode.Zeros;
            using (var decryptor = tripleDes.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }

    public static byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
    
    public static byte[] GenerateRandomKey()
    {
        using (var tripleDes = new TripleDESCryptoServiceProvider())
        {
            tripleDes.GenerateKey();
            return tripleDes.Key;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        EncryptPersonalName();
    }

    private static void EncryptPersonalName()
    {
        string textToEncrypt = "Kolomis"; // Your name in transliteration
        byte[] key = TripleDESAlgorithm.GenerateRandomKey(); // Generate a random 16-byte key

        byte[] textBytes = Encoding.ASCII.GetBytes(textToEncrypt.PadRight(8, ' '));
        byte[] encrypted = TripleDESAlgorithm.Encrypt(textBytes, key);
        byte[] decrypted = TripleDESAlgorithm.Decrypt(encrypted, key);

        Console.WriteLine($"Original Text: {textToEncrypt}");
        Console.WriteLine($"Key (HEX): {BitConverter.ToString(key).Replace("-", " ")}");
        Console.WriteLine($"Encrypted (HEX): {BitConverter.ToString(encrypted).Replace("-", " ")}");
        Console.WriteLine($"Decrypted Text: {Encoding.ASCII.GetString(decrypted)}");
    }
}
