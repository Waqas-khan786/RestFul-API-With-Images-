using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace DataAccessLayer.Helpers
{
    public class Helper
    {
        private static IWebHostEnvironment _IWebHostEnvironment;
        private static ConfigurationManager _configurationManager;
        public Helper(IWebHostEnvironment IWebHostEnvironment, ConfigurationManager configurationManager)
        {
            _IWebHostEnvironment = IWebHostEnvironment;
            _configurationManager = configurationManager;
        }

        #region UploadImage
        public static async Task<string> UploadImage(byte[] Image, string employeeImagename, string extension)
        {
            string Content_root_path = _IWebHostEnvironment.ContentRootPath;
            string Upload_Directory = Path.Combine(Content_root_path, "EmployeeImage");
            string ImageName = Guid.NewGuid().ToString() + "-" + employeeImagename + extension;
            string FilePath = Path.Combine(Upload_Directory, ImageName);
            try
            {
                if (!Directory.Exists(Upload_Directory))
                {
                    Directory.CreateDirectory(Upload_Directory);
                }
                using (FileStream stream = new FileStream(FilePath, FileMode.Create))
                {
                    stream.Write(Image, 0, Image.Length);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return await Task.FromResult(FilePath);
        }
        #endregion

        #region Encryption
        public static string EncryptString(string plainText)
        {
            string secretkey = _configurationManager.GetSection("Security").GetSection("SecretKey").Value ?? "No Secret Key";
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(secretkey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
        #endregion

        #region Decryption
        public static string DecryptString(string cipherText)
        {
            string secretkey = _configurationManager.GetSection("Security").GetSection("SecretKey").Value ?? "No Secret Key";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(secretkey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

    }
}
