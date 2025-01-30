namespace WlConsultings.BankChallenge.Application.Utils
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System;
    using System.Security.Cryptography;

    public static class PasswordHasher
    {
        // Tamanho do salt (em bytes)
        private const int SaltSize = 16; // 128 bits

        // Tamanho do hash (em bytes)
        private const int HashSize = 32; // 256 bits

        // Número de iterações para o PBKDF2
        private const int Iterations = 10000;

        // Gera um hash para a senha
        public static string HashPassword(string password)
        {
            // Gera um salt aleatório
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Gera o hash da senha usando PBKDF2
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            // Combina o salt e o hash em um único array
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Converte para base64 para armazenamento
            return Convert.ToBase64String(hashBytes);
        }

        // Verifica se a senha corresponde ao hash
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Converte o hash de base64 para bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extrai o salt do hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Gera o hash da senha fornecida usando o mesmo salt
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            // Compara os hashes
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
