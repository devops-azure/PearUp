using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.Authentication
{
    public interface IHashingService
    {
        byte[] GenerateSalt();
        byte[] GetHash(string plaintext, byte[] salt);
        bool SlowEquals(byte[] a, byte[] b);
    }
}
