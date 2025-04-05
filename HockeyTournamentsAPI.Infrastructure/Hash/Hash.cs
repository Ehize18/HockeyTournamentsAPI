using System.Security.Cryptography;
using System.Text;

namespace HockeyTournamentsAPI.Infrastructure.Hash
{
    public static class Hash
    {
        public static string SHA256Hash(string value)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                return Convert.ToHexString(hash);
            }
        }
    }
}
