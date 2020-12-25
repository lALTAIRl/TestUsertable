using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Task4.Helpers
{
    public static class PasswordHelper
    {
        private const int PBKDF2IterCount = 10000;
        private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return HashPasswordInternal(password);
        }

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private static string HashPasswordInternal(string password)
        {
            var bytes = HashPasswordInternal(password, KeyDerivationPrf.HMACSHA256, PBKDF2IterCount, SaltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(bytes);
        }

        private static byte[] HashPasswordInternal(
            string password,
            KeyDerivationPrf prf,
            int iterCount,
            int saltSize,
            int numBytesRequested)
        {
            // Produce a version 3 (see comment above) text hash.
            var salt = new byte[saltSize];
            _rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];

            // Write format marker.
            outputBytes[0] = 0x01;

            // Write hashing algorithm version.
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);

            // Write iteration count of the algorithm.
            WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);

            // Write size of the salt.
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);

            // Write the salt.
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);

            // Write the subkey.
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return outputBytes;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

    }
}
