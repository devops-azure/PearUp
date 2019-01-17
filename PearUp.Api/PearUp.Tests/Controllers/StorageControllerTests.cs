using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PearUp.Api.Controllers;
using PearUp.Constants;
using PearUp.Infrastructure.Azure;
using System;
using System.Threading.Tasks;

namespace PearUp.Tests.Controllers
{
    [TestFixture]
    public class StorageControllerTests
    {
        //success case
        [Test]
        public async Task GetSasKey_Should_Return_SasKey_When_Azure_Sas_Provider_Returns_SasObject()
        {
            var mockAzureSasProvider = new Mock<IAzureSasProvider>();
            mockAzureSasProvider.Setup(az => az.GetContainerSAS()).ReturnsAsync(
                new AzureSasKey
                {
                    ExpiryTimeInUtc = DateTimeOffset.Now,
                    SasKey = "TestSasKey"
                });
            var storageController = new StorageController(mockAzureSasProvider.Object);
            var actualResult = await storageController.GetSasKey();
            Assert.IsAssignableFrom<OkObjectResult>(actualResult);
            var contentResult = actualResult as OkObjectResult;
            Assert.AreEqual(200, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<AzureSasKey>(contentResult.Value);
        }

        //failure case
        [Test]
        public void GetSasKey_Should_Throw_Exception_When_Provider_Return_Error()
        {
            var mockAzureSasProvider = new Mock<IAzureSasProvider>();
            mockAzureSasProvider.Setup(az => az.GetContainerSAS()).Throws(new Exception(LoggerMessages.Oops_Message));
            var storageController = new StorageController(mockAzureSasProvider.Object);
            Exception actualException = Assert.ThrowsAsync<Exception>(() => storageController.GetSasKey());
            actualException.Message.Should().Be(LoggerMessages.Oops_Message);
        }
    }
}
