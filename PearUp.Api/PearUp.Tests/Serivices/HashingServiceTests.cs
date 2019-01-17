using NUnit.Framework;
using PearUp.Authentication;
using System.Security.Cryptography;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class HashingServiceTests
    {
        private RandomNumberGenerator _randomNumberGenerator;
        private HashAlgorithm _hashAlgorithm;
        private IHashingService _hashingService;

        [OneTimeSetUp]
        public void Setup()
        {
            _hashAlgorithm = new SHA256CryptoServiceProvider();
            _randomNumberGenerator = new RNGCryptoServiceProvider();
            _hashingService = new HashingService(_randomNumberGenerator, _hashAlgorithm);
        }

        [Test]
        public void SlowEquals_Returns_True_Given_Same_PlainText_And_Salt_To_GetHash()
        {
            string plaintext = "sometext";
            var salt = _hashingService.GenerateSalt();
            var hash1 = _hashingService.GetHash(plaintext, salt);
            var hash2 = _hashingService.GetHash(plaintext, salt);
            var result = _hashingService.SlowEquals(hash1, hash2);
            Assert.IsTrue(result);
        }

        [Test]
        public void SlowEquals_Returns_False_Given_Different_PlainText_To_GetHash()
        {
            string plaintext1 = "sometext1";
            string plaintext2 = "sometext2";
            var salt = _hashingService.GenerateSalt();
            var hash1 = _hashingService.GetHash(plaintext1, salt);
            var hash2 = _hashingService.GetHash(plaintext2, salt);
            var result = _hashingService.SlowEquals(hash1, hash2);
            Assert.IsFalse(result);
        }

        [Test]
        public void SlowEquals_Returns_False_Given_Different_Salt_To_GetHash()
        {
            string plaintext = "sometext";
            var salt1 = _hashingService.GenerateSalt();
            var salt2 = _hashingService.GenerateSalt();
            var hash1 = _hashingService.GetHash(plaintext, salt1);
            var hash2 = _hashingService.GetHash(plaintext, salt2);
            var result = _hashingService.SlowEquals(hash1, hash2);
            Assert.IsFalse(result);
        }
    }
}
