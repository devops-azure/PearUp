using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PearUp.Api.Controllers;
using PearUp.CommonEntities;
using PearUp.DTO;
using PearUp.IApplicationServices;
using PearUp.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PearUp.Tests.Controllers
{
    [TestFixture]
    public class InterestsControllerTests
    {

        [Test]
        public async Task Get_Should_Return_Ok_Result_When_Valid()
        {
            var mockInterest = new List<InterestDTO>()
            {
                new InterestDTO(){ Id = 1,InterestDescription = "test",InterestName = "test"},
                new InterestDTO(){ Id = 2,InterestDescription = "test",InterestName = "test"}
            } as IReadOnlyList<InterestDTO>;
            var mockInterestsService = new Mock<IInterestService>();
            mockInterestsService.Setup(x => x.GetInterests()).ReturnsAsync(Result.Ok(mockInterest));

            var controller = new InterestController(mockInterestsService.Object);
            var result = await controller.GetAll();

            var contentResult = result as OkObjectResult;
            contentResult.StatusCode.Should().Be(200);
            contentResult.Value.Should().BeAssignableTo<IReadOnlyList<InterestDTO>>();
            var actualValue = contentResult.Value as IReadOnlyList<InterestDTO>;
            actualValue.Should().BeEquivalentTo(mockInterest);

        }

        [Test]
        public async Task Get_Should_Return_Failure_Result_When_No_Content_Is_Available()
        {
            var mockInterestsService = new Mock<IInterestService>();
            mockInterestsService.Setup(x => x.GetInterests()).ReturnsAsync(Result.Fail<IReadOnlyList<InterestDTO>>(InterestRepository.No_Interests_Found));

            var controller = new InterestController(mockInterestsService.Object);
            var result = await controller.GetAll();

            var contentResult = result as BadRequestObjectResult;
            contentResult.StatusCode.Should().Be(400);
            contentResult.Value.Should().Be(InterestRepository.No_Interests_Found);
        }
        [Test]
        public async Task Insert_Should_Return_Ok_Result_When_Valid_InterestsDTO_Is_Passed()
        {
            var testDto = new CreateInterestDTO();

            var mockInterestsService = new Mock<IInterestService>();
            mockInterestsService.Setup(x => x.InsertInterest(testDto)).ReturnsAsync(Result.Ok());

            var controller = new InterestController(mockInterestsService.Object);
            var result = await controller.Insert(testDto);
            var contentResult = result as OkObjectResult;

            contentResult.StatusCode.Should().Be(200);
            contentResult.Value.Should().BeAssignableTo<bool>();
            var actualValue = contentResult.Value as bool?;
            actualValue.Should().BeTrue();
        }

        [Test]
        public async Task Insert_Should_Return_Failure_Result_When_InterestsDTO_Is_Null()
        {
            var mockInterestsService = new Mock<IInterestService>();

            var controller = new InterestController(mockInterestsService.Object);
            var result = await controller.Insert(null);

            var contentResult = result as BadRequestObjectResult;
            contentResult.StatusCode.Should().Be(400);
            contentResult.Value.Should().BeAssignableTo<string>();
            var actualValue = contentResult.Value as string;
            actualValue.Should().Be(Constants.InterestErrorMessages.Interest_Should_Not_Be_Empty);
        }

        [Test]
        public async Task Update_Should_Return_Ok_Result_When_Valid_InterestsDto_Is_Passed()
        {
            var testDto = new InterestDTO() { Id = 1 };
            var mockInterestsService = new Mock<IInterestService>();
            mockInterestsService.Setup(x => x.UpdateInterest(It.IsAny<InterestDTO>())).ReturnsAsync(Result.Ok());
            var controller = new InterestController(mockInterestsService.Object);
            var result = await controller.Update(testDto);
            var contentResult = result as OkObjectResult;
            contentResult.StatusCode.Should().Be(200);
            contentResult.Value.Should().BeAssignableTo<bool>();
            var actualValue = contentResult.Value as bool?;
            actualValue.Should().BeTrue();
        }

        [Test]
        public async Task Update_Should_Return_Failure_Result_When_There_Is_Issue_While_Saving()
        {
            var testDto = new InterestDTO() { Id = 1 };
            var mockInterestsService = new Mock<IInterestService>();
            mockInterestsService.Setup(x => x.UpdateInterest(It.IsAny<InterestDTO>())).ReturnsAsync(Result.Fail(Constants.InterestErrorMessages.Error_Occured_While_Updating_Interest));
            var controller = new InterestController(mockInterestsService.Object);
            var result = await controller.Update(testDto);
            var contentResult = result as BadRequestObjectResult;
            contentResult.StatusCode.Should().Be(400);
            contentResult.Value.Should().BeAssignableTo<string>();
            var actualValue = contentResult.Value as string;
            actualValue.Should().Be(Constants.InterestErrorMessages.Error_Occured_While_Updating_Interest);
        }
    }
}
