//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AdvantShop.Helpers;

namespace AdvantShop.Payment
{

    public class SagePayUtils
    {
        //** Wrapper function do encrypt an encode based on strEncryptionType setting **
        public static string EncryptAndEncode(string strIn, string password)
        {
            //** AES encryption, CBC blocking with PKCS5 padding then HEX encoding - DEFAULT **
            return "@" + ByteArrayToHexString(aesEncrypt(strIn,password));
        }

        //** Wrapper function do decode then decrypt based on header of the encrypted field **
        public static string DecodeAndDecrypt(string strIn, string password)
        {
            //** HEX decoding then AES decryption, CBC blocking with PKCS5 padding - DEFAULT **
            return AesDecrypt(HexStringToBytes(strIn.Substring(1)), password);
        }

        /// <summary>
        /// Encrypts input string using Rijndael (AES) algorithm with CBC blocking and PKCS7 padding.
        /// </summary>
        /// <param name="inputText">text string to encrypt </param>
        /// <param name="password"></param>
        /// <returns>Encrypted text in Byte array</returns>
        /// <remarks>The key and IV are the same, and use strEncryptionPassword.</remarks>
        private static byte[] aesEncrypt(string inputText, string password)
        {

            var AES = new RijndaelManaged
                          {
                              Padding = PaddingMode.PKCS7,
                              Mode = CipherMode.CBC,
                              KeySize = 128,
                              BlockSize = 128
                          };
            //set the mode, padding and block size for the key
            //convert key and plain text input into byte arrays
            byte[] keyAndIvBytes = Encoding.UTF8.GetBytes(password);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
            //create streams and encryptor object
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (
                        var cryptoStream = new CryptoStream(memoryStream,
                                                            AES.CreateEncryptor(keyAndIvBytes, keyAndIvBytes),
                                                            CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        //get encrypted stream into byte array
                        return memoryStream.ToArray();
                    }
                }
            }
            finally
            {
                AES.Clear();
            }
        }

        /// <summary>
        /// Decrypts input string from Rijndael (AES) algorithm with CBC blocking and PKCS7 padding.
        /// </summary>
        /// <param name="inputBytes">Encrypted binary array to decrypt</param>
        /// <param name="password"></param>
        /// <returns>string of Decrypted data</returns>
        /// <remarks>The key and IV are the same, and use strEncryptionPassword.</remarks>
        private static string AesDecrypt(byte[] inputBytes, string password)
        {

            var AES = new RijndaelManaged
                          {
                              Padding = PaddingMode.PKCS7,
                              Mode = CipherMode.CBC,
                              KeySize = 128,
                              BlockSize = 128
                          };

            byte[] keyAndIvBytes = Encoding.UTF8.GetBytes(password);
            //create streams and decryptor object
            try
            {
                using (var memoryStream = new MemoryStream(inputBytes))
                {
                    using (
                        var cryptoStream = new CryptoStream(memoryStream,
                                                            AES.CreateDecryptor(keyAndIvBytes, keyAndIvBytes),
                                                            CryptoStreamMode.Read))
                    {
                        var outputBytes = new byte[inputBytes.Length + 1];
                        cryptoStream.Read(outputBytes, 0, outputBytes.Length);
                        return Encoding.UTF8.GetString(outputBytes);
                    }
                }
            }
            finally
            {
                AES.Clear();
            }
        }

        /// <summary>
        /// Converts a string of characters representing hexadecimal values into an array of bytes
        /// </summary>
        /// <param name="strInput">A hexadecimal string of characters to convert to binary.</param>
        /// <returns>A byte array</returns>
        private static byte[] HexStringToBytes(string strInput)
        {
            int numBytes = (strInput.Length / 2);
            var bytes = new byte[numBytes];
            for (int x = 0; x <= numBytes - 1; x++)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }
            return bytes;

        }

        /// <summary>
        /// Converts an array of bytes into a hexadecimal string representation.
        /// </summary>
        /// <param name="ba">Array of bytes to convert</param>
        /// <returns>String of hexadecimal values.</returns>
        private static string ByteArrayToHexString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}