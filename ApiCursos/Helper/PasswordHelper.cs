using System.Security.Cryptography;
using System.Text;

namespace ApiCursos.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
