using System.Security.Cryptography;
using System.Text;

namespace FileManager.Shared.Extensions
{
    public static class EncryptionExtensions
    {
        public static string Sha256(this string rawData)
        {
            using SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData ?? string.Empty));

            // Convert byte array to a string
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
