using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Moq;
using NUnit.Framework;
using PearUp.ApplicationServices;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.DTO;
using PearUp.IRepository;
using PearUp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class InterestServiceTests
    {
        private PearUpContext _pearUpContext;
        private List<Interest> _interestsList;
        private List<InterestDTO> _interestDTOs;
        private Interest _mockInterest;
        private InterestDTO _mockInterestDTO;

        public InterestServiceTests()
        {
            PreInit();
            InitContext();
        }

        private void PreInit()
        {
            _mockInterestDTO = new InterestDTO() { Id = 3, InterestDescription = "mock", InterestName = "mock" };
            _mockInterest = Interest.Create("mock", "mock").Value;
            _mockInterest.Id = 3;
            _interestsList = new List<Interest>() {
                Interest.Create("mock", "mock").Value,
                Interest.Create("mock", "mock").Value
            };

            _interestDTOs = new List<InterestDTO>() {
                new InterestDTO() {Id = 1, InterestDescription = "mock", InterestName = "mock" },
                new InterestDTO() {Id = 2, InterestDescription = "mock2", InterestName = "mock2" }
            };
        }

        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<PearUpContext>()
                 .UseInMemoryDatabase("PearUp");
            _pearUpContext = new PearUpContext(builder.Options);
            _pearUpContext.Interests.AddRange(_interestsList);
            _pearUpContext.SaveChanges();
        }

        [Test]
        public async Task GetInterests_Should_Return_Success_Result()
        {
            var mockInterestRepository = new Mock<IInterestRepository>();
            mockInterestRepository.Setup(x => x.GetAllInterests()).ReturnsAsync(Result.Ok(_interestsList as IReadOnlyList<Interest>));
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IReadOnlyList<InterestDTO>>(_interestsList)).Returns(_interestDTOs);
            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.GetInterests();
            result.IsSuccessed.Should().BeTrue();
            result.Value.Should().Equal(_interestDTOs);
        }

        [Test]
        public async Task InsertInterest_Should_Return_Success_Result()
        {
            var mockObject = new CreateInterestDTO()
            {
                InterestDescription = _mockInterestDTO.InterestDescription,
                InterestName = _mockInterestDTO.InterestName
            };
            var mockMapper = new Mock<IMapper>();
            var mockInterestRepository = new Mock<IInterestRepository>();

            mockMapper.Setup(x => x.Map<Interest>(It.IsAny<CreateInterestDTO>())).Returns(_mockInterest);
            mockInterestRepository.Setup(x => x.Create(It.IsAny<Interest>()));
            mockInterestRepository.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());

            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.InsertInterest(mockObject);

            result.IsSuccessed.Should().BeTrue();
        }

        [Test]
        public async Task InsertInterest_Should_Return_Failure_Result_When_Parameter_Is_Null()
        {
            var mockInterestRepository = new Mock<IInterestRepository>();
           
            var mockMapper = new Mock<IMapper>();

            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.InsertInterest(null);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.InterestErrorMessages.Interest_Should_Not_Be_Empty);
        }

        [Test]
        public async Task InsertInterest_Should_Return_Failure_Result_When_Error_Occurs_While_Saving()
        {
            var mockObject = new CreateInterestDTO()
            {
                InterestDescription = _mockInterestDTO.InterestDescription,
                InterestName = _mockInterestDTO.InterestName
            };
            var mockInterestRepository = new Mock<IInterestRepository>();
            mockInterestRepository.Setup(x => x.Create(It.IsAny<Interest>()));
            mockInterestRepository.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Fail<Interest>(Constants.InterestErrorMessages.Error_Occured_While_Updating_Interest));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Interest>(It.IsAny<CreateInterestDTO>())).Returns(_mockInterest);

            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.InsertInterest(mockObject);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.InterestErrorMessages.Error_Occured_While_Updating_Interest);
        }

        [Test]
        public async Task UpdateInterest_Should_Return_Success_Result()
        {
            
            var updatedInterest = new InterestDTO() { Id = 2, InterestDescription = "sample", InterestName = "sample" };
            var mockInterestRepository = new Mock<IInterestRepository>();
            mockInterestRepository.Setup(x => x.GetInterestById(It.IsAny<int>())).ReturnsAsync(Result.Ok(_mockInterest));
            mockInterestRepository.Setup(x => x.Update(It.IsAny<Interest>()));
            mockInterestRepository.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Interest>(It.IsAny<InterestDTO>())).Returns(_mockInterest);

            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.UpdateInterest(updatedInterest);

            result.IsSuccessed.Should().BeTrue();
        }

        [Test]
        public async Task UpdateInterest_Should_Return_Failure_Result_If_Interest_Does_Not_Exist()
        {
            var mockInterestRepository = new Mock<IInterestRepository>();
            mockInterestRepository.Setup(x => x.GetInterestById(It.IsAny<int>())).ReturnsAsync(Result.Fail<Interest>(InterestRepository.No_Interests_Found));
            mockInterestRepository.Setup(x => x.Update(It.IsAny<Interest>()));
            mockInterestRepository.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());
            var mockMapper = new Mock<IMapper>();

            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.UpdateInterest(new InterestDTO() { Id = 5, InterestDescription = "test", InterestName = "test"});

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(InterestRepository.No_Interests_Found);
        }

        [Test]
        public async Task UpdateInterest_Should_Return_Failure_Result_If_Error_Occurs_While_Save_Changes()
        {
            var mockInterestRepository = new Mock<IInterestRepository>();
            mockInterestRepository.Setup(x => x.GetInterestById(It.IsAny<int>())).ReturnsAsync(Result.Ok(_mockInterest));
            mockInterestRepository.Setup(x => x.Update(It.IsAny<Interest>()));
            mockInterestRepository.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Fail(Constants.InterestErrorMessages.Error_Occured_While_Updating_Interest));
            var mockMapper = new Mock<IMapper>();

            var interestService = new InterestService(mockInterestRepository.Object, mockMapper.Object);
            var result = await interestService.UpdateInterest(new InterestDTO() { Id = 5, InterestDescription = "test", InterestName = "test" });

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.InterestErrorMessages.Error_Occured_While_Updating_Interest);
        }

    }
}
