using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PearUp.Api.Controllers;
using PearUp.CommonEntities;
using PearUp.DTO.PhoneVerification;
using PearUp.IBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.Tests.Controllers
{
    [TestFixture]
    public class PhoneVerificationControllerTests
    {
        #region Initializer
        private const string TEST_NUMBER = "8897322553";
        private const string TEST_COUNTRY_CODE = "91";
        private const int TEST_VERIFICATION_CODE = 9097;
        private const int Status200 = (int)StatusCodes.Status200OK;
        private const int Status400 = (int)StatusCodes.Status400BadRequest;

        private static IEnumerable GetGeneratePhoneVerificationParams
        {
            get
            {
                yield return new GeneratePhoneVerificationDTO();
                yield return new GeneratePhoneVerificationDTO() { CountryCode = TEST_COUNTRY_CODE };
                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = TEST_NUMBER };
                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = "asdfasdf", CountryCode = "sdf" };
                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = TEST_NUMBER, CountryCode = "sdf" };
                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = "asdfasdf", CountryCode = TEST_COUNTRY_CODE };

                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = "", CountryCode = "", };
                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = "", CountryCode = TEST_COUNTRY_CODE };
                yield return new GeneratePhoneVerificationDTO() { PhoneNumber = TEST_NUMBER, CountryCode = "" };

            }
        }

        private static IEnumerable GetValidatePhoneVerificationParams
        {
            get
            {
                yield return new ValidatePhoneVerificationDTO() { PhoneNumber = TEST_NUMBER, CountryCode = TEST_COUNTRY_CODE };
            }
        }

        #endregion

        #region GenerateVerificationCode_Method_ControllerTest
        [Test, TestCaseSource(nameof(GetGeneratePhoneVerificationParams))]
        public async Task GenerateVerificationCode_ShouldReturn_ErrorMessage_If_Phone_Or_Country_Is_Invalid(GeneratePhoneVerificationDTO param)
        {
            // Arrange
            var verifyServiceMock = new Mock<IPhoneVerificationService>();
            verifyServiceMock.Setup(vs => vs.GenerateVerificationCodeAsync(param.CountryCode, param.PhoneNumber)).ReturnsAsync(Result.Fail<bool>(Constants.PhoneVerifyMessages.Country_Phone_Invalid));

            // Act
            var controller = new PhoneVerificationController(verifyServiceMock.Object);
            var actualResult = await controller.GenerateVerificationCode(param);

            // Assert
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(Status400, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            var actualValue = contentResult.Value as string;
        }

        [Test]
        public async Task GenerateVerificationCode_ShouldReturn_SuccessMessage_If_Phone_And_CountryCode_Are_Valid()
        {
            // Arrange
            var verifyServiceMock = new Mock<IPhoneVerificationService>();
            var param = new GeneratePhoneVerificationDTO()
            {
                PhoneNumber = TEST_NUMBER,
                CountryCode = TEST_COUNTRY_CODE,
            };
            verifyServiceMock.Setup(vs => vs.GenerateVerificationCodeAsync(param.CountryCode, param.PhoneNumber)).ReturnsAsync(Result.Ok(true));

            // Act
            var controller = new PhoneVerificationController(verifyServiceMock.Object);
            var actualResult = await controller.GenerateVerificationCode(param);

            // Assert
            verifyServiceMock.Verify(vs => vs.GenerateVerificationCodeAsync(param.CountryCode, param.PhoneNumber), Times.Once);
            Assert.IsAssignableFrom<OkObjectResult>(actualResult);
            var contentResult = actualResult as OkObjectResult;
            Assert.AreEqual(Status200, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            var actualValue = contentResult.Value as string;
            Assert.AreEqual(Constants.PhoneVerifyMessages.Verification_Code_Sent, actualValue);
        }

        #endregion

        #region ValidateVerificationCode_Method_Controller_Test
        [Test, TestCaseSource(nameof(GetValidatePhoneVerificationParams))]
        public async Task ValidateVerificationCode_ShouldReturn_ErrorMessage_If_Phone_Or_CountryCode_Or_VerificationCode_Are_Invalid(ValidatePhoneVerificationDTO param)
        {
            // Arrange
            var verifyServiceMock = new Mock<IPhoneVerificationService>();
            verifyServiceMock.Setup(vs => vs.ValidateVerificationCodeAsync(param.CountryCode, param.PhoneNumber, param.VerificationCode)).ReturnsAsync(Result.Fail<bool>(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Invalid));

            // Act
            var controller = new PhoneVerificationController(verifyServiceMock.Object);
            var actualResult = await controller.ValidateVerificationCode(param);

            // Assert
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(Status400, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            var actualValue = contentResult.Value as string;
            Assert.AreEqual(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Invalid, actualValue);
        }


        [Test]
        public async Task ValidateVerificationCode_ShouldReturn_SuccessMessage_If_Phone_CountryCode_VerificationCode_Are_Valid()
        {
            // Arrange
            var verifyServiceMock = new Mock<IPhoneVerificationService>();
            var param = new ValidatePhoneVerificationDTO()
            {
                PhoneNumber = TEST_NUMBER,
                CountryCode = TEST_COUNTRY_CODE,
                VerificationCode = TEST_VERIFICATION_CODE
            };
            verifyServiceMock.Setup(vs => vs.ValidateVerificationCodeAsync(param.CountryCode, param.PhoneNumber, param.VerificationCode)).ReturnsAsync(Result.Ok(true));

            // Act
            var controller = new PhoneVerificationController(verifyServiceMock.Object);
            var actualResult = await controller.ValidateVerificationCode(param);

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(actualResult);
            var contentResult = actualResult as OkObjectResult;
            Assert.AreEqual(Status200, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            var actualValue = contentResult.Value as string;
            Assert.AreEqual(Constants.PhoneVerifyMessages.Verification_Completed, actualValue);
        }

        #endregion
    }
}
