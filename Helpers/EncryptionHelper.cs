using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace APIRest.Helpers
{
    public static class EncryptionHelper
    {
        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generar una sal aleatoria
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Generar el hash de la contraseña usando la sal
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Retornar el hash y la sal como base64
            return (Hash: hash, Salt: Convert.ToBase64String(salt));
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // Convertir la sal de base64 a bytes
            byte[] salt = Convert.FromBase64String(storedSalt);

            // Generar el hash de la contraseña ingresada usando la sal almacenada
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Comparar el hash generado con el almacenado
            return hash == storedHash;
        }
    }
}
