using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Example
{
    class Auth
    {
        public static string Password { get; set; } //change this to your enc key...
        public static bool Answer { get; set; }
        public static string Error { get; set; }
        public static void Login(string username, string userpass)
        {
            WebClient wb = new WebClient();
            string json = wb.DownloadString("http://worldclockapi.com/api/json/est/now");
            var jss = new JavaScriptSerializer();
            Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
            string ex = sData["currentDateTime"].ToString();
            string expire = Base64Encode(ex);
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(Password));
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            string test = BitConverter.ToString(iv);
            string user = EncryptString(username, key, iv);
            string pass = EncryptString(userpass, key, iv);
            WebClient web = new WebClient();
            string get = "https://samzydev.xyz/api/auth/v1?u=" + user + "&p=" + pass + "&e=" + expire; //change the url to your domain, otherwise this won't work!
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string result = DecryptString(web.DownloadString(get), key, iv);
            if (result.Contains("access granted"))
            {
                Answer = true;
            }
            else if (result.Contains("password does not match"))
            {
                Answer = false;
                Error = "Passowrd Incorrect";
            }
            else if (result.Contains("user does not exist"))
            {
                Answer = false;
                Error = "User Does Not Exist";
            }
            else if (result.Contains("expired"))
            {
                Answer = false;
                Error = "Session Expired";
            }
        }

        //Encryption
        private static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);
            string plainText = String.Empty;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] plainBytes = memoryStream.ToArray();
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return plainText;
        }
        private static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            return cipherText;
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
