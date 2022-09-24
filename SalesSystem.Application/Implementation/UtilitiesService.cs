using SalesSystem.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SalesSystem.Application.Implementation
{
    public class UtilitiesService : IUtilitiesService
    {
        public string GeneratePassword()
        {
            string pass = Guid.NewGuid().ToString("N").Substring(1,10);
            return pass;
        }

        public string ConvertToSha256(string text)
        {
            StringBuilder sb = new();
            using SHA256 hash = SHA256.Create();
            byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
