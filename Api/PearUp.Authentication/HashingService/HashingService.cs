using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PearUp.Authentication
{
    public class HashingService : IHashingService
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly HashAlgorithm _hashAlgorithm;

        public HashingService(RandomNumberGenerator randomNumberGenerator, HashAlgorithm hashAlgorithm)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _hashAlgorithm = hashAlgorithm;
        }

   
        public byte[] GetHash(string plaintext, byte[] salt)
        {
            byte[] plainBytes = System.Text.Encoding.ASCII.GetBytes(plaintext);
            byte[] toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return _hashAlgorithm.ComputeHash(toHash);
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            _randomNumberGenerator.GetBytes(salt);
            return salt;
        }

        public bool SlowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}
