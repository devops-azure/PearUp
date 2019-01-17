using NUnit.Framework;
using PearUp.BusinessEntity;
using System;
using FluentAssertions;
using System.Collections.Generic;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class PasswordTests
    {
        private static IEnumerable<Tuple<byte[], byte[]>> GetInputsWithInvalidPasswordHash
        {
            get
            {
                byte[] randomByteArray = new byte[32];
                Random random = new Random();
                random.NextBytes(randomByteArray);
                yield return new Tuple<byte[], byte[]>(null, null);
                yield return new Tuple<byte[], byte[]>(null, randomByteArray);
            }
        }

        private static IEnumerable<Tuple<byte[], byte[]>> GetInputsWithInvalidPasswordSalt
        {
            get
            {
                byte[] randomByteArray = new byte[32];
                Random random = new Random();
                random.NextBytes(randomByteArray);
                yield return new Tuple<byte[], byte[]>(randomByteArray, null);

            }
        }

        private static IEnumerable<Tuple<byte[], byte[]>> GetValidInputs
        {
            get
            {
                byte[] randomByteArray = new byte[32];
                Random random = new Random();
                random.NextBytes(randomByteArray);
                yield return new Tuple<byte[], byte[]>(randomByteArray, randomByteArray);
            }
        }

        [TestCaseSource(nameof(GetInputsWithInvalidPasswordHash))]
        public void Create_Should_Return_Fail_Result_For_Invalid_passwordHash(Tuple<byte[], byte[]> inputs)
        {

            byte[] passwordHash = inputs.Item1;
            byte[] passwordSalt = inputs.Item2;
            var passwordResult = Password.Create(passwordHash, passwordSalt);
            passwordResult.IsSuccessed.Should().BeFalse();
            passwordResult.GetErrorString().Should().Be(Password.Password_Hash_Is_Required);
        }

        [TestCaseSource(nameof(GetInputsWithInvalidPasswordSalt))]
        public void Create_Should_Return_Fail_Result_For_Invalid_passwordSalt(Tuple<byte[], byte[]> inputs)
        {

            byte[] passwordHash = inputs.Item1;
            byte[] passwordSalt = inputs.Item2;
            var passwordResult = Password.Create(passwordHash, passwordSalt);
            passwordResult.IsSuccessed.Should().BeFalse();
            passwordResult.GetErrorString().Should().Be(Password.Password_Salt_Is_Required);
        }

        [TestCaseSource(nameof(GetValidInputs))]
        public void Create_Should_Return_Success_Result(Tuple<byte[], byte[]> inputs)
        {
            byte[] passwordHash = inputs.Item1;
            byte[] passwordSalt = inputs.Item2;
            var passwordResult = Password.Create(passwordHash, passwordSalt);
            passwordResult.IsSuccessed.Should().BeTrue();
            passwordResult.Value.PasswordHash.Should().BeEquivalentTo(passwordHash);
            passwordResult.Value.PasswordHash.Should().BeEquivalentTo(passwordSalt);
        }
    }
}
