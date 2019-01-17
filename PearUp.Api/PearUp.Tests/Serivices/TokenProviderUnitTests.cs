using Microsoft.Extensions.Options;
using NUnit.Framework;
using PearUp.Authentication;
using PearUp.CommonEntity;
using PearUp.Constants;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    class TokenProviderUnitTests
    {
        private AuthSettings _settings = null;
        public TokenProviderUnitTests()
        {
            _settings = new AuthSettings
            {
                CleintName = "PearUp",
                ExpiryInMinutes = 30,
                SecretKey = "PearUp-Secret-Key"
            };
        }

        [Test]
        public void CreateToken_Should_Return_SuccessResult_When_Inputs_Are_Valid()
        {
            var userToken = new TokenInfo
            {
                Id = 100,
                FullName = "admin"
            };
            var authSettings = Options.Create<AuthSettings>(_settings);
            var tokenProvider = new TokenProvider(authSettings);
            var acutalResults = tokenProvider.CreateToken(userToken,false);

            Assert.IsTrue(acutalResults.IsSuccessed);
            Assert.Greater(acutalResults.Value.Value.Length, 15);
        }

        [Test]
        public void CreateToken_Should_Return_ErrorResult_When_Inputs_Are_Null()
        {
            var authSettings = Options.Create<AuthSettings>(_settings);
            var tokenProvider = new TokenProvider(authSettings);
            var acutalResults = tokenProvider.CreateToken(null,false);

            Assert.AreEqual(UserErrorMessages.User_Token_Object_Should_Not_Be_null, acutalResults.GetErrorString());
        }

    }
}
