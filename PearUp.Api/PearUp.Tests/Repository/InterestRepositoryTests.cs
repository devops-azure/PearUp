using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PearUp.BusinessEntity;
using PearUp.DTO;
using PearUp.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.Tests.Repository
{
    [TestFixture]
    public class InterestRepositoryTests
    {
        private PearUpContext _pearUpContext;
        private List<Interest> _interestsList = new List<Interest>();

        public InterestRepositoryTests()
        {
            PreInit();
            InitContext();
        }

        private void PreInit()
        {
            //_interestsList = new List<Interest>() {
            //    new Interest() {Id = 1, InterestDescription = "mock", InterestName = "mock" },
            //    new Interest() {Id = 2, InterestDescription = "mock2", InterestName = "mock2" }
            //};
            _interestsList.Add(Interest.Create("mock", "mock").Value);
            _interestsList.Add(Interest.Create("mock2", "mock2").Value);
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
        public async Task GetInterestById_Should_Return_Success_Result_With_Valid_Interest_Id()
        {
            var interestRepository = new InterestRepository(_pearUpContext);
            var result = await interestRepository.GetInterestById(1);
            result.IsSuccessed.Should().BeTrue();
            result.Value.Id.Should().Be(1);
            result.Value.InterestDescription.Should().Be("mock");
            result.Value.InterestName.Should().Be("mock");
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(null)]
        public async Task GetInterestById_Should_Return_Failure_Result_With_InValid_Interest_Id(int id)
        {
            var interestRepository = new InterestRepository(_pearUpContext);
            var result = await interestRepository.GetInterestById(id);
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(InterestRepository.No_Interests_Found);
        }

        [Test]
        public async Task GetAllIntrests_Should_Return_Success_Result()
        {
            var interestRepository = new InterestRepository(_pearUpContext);
            var result = await interestRepository.GetAllInterests();
            result.IsSuccessed.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(_interestsList);
        }

        [Test]
        public async Task GetInterestsByIds_Should_Return_Success_Result()
        {
            int[] arr = new int[] { 1, 2};
            var interestRepository = new InterestRepository(_pearUpContext);
            var result = await interestRepository.GetInterestsByIds(arr);
            result.IsSuccessed.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(_interestsList);
        }


        [Test]
        public async Task GetInterestsByIds_Should_Return_Failure_Result_If_Interest_Id_Not_Available()
        {
            int[] arr = new int[] { 4, 5 };
            var interestRepository = new InterestRepository(_pearUpContext);
            var result = await interestRepository.GetInterestsByIds(arr);
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(InterestRepository.No_Interests_Found);
        }

        [Test]
        public async Task GetInterestsByIds_Should_Return_Failure_Result_If_Interest_Id_Params_Empty()
        {
            int[] arr = new int[] { };
            var interestRepository = new InterestRepository(_pearUpContext);
            var result = await interestRepository.GetInterestsByIds(arr);
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(InterestRepository.No_Interests_Found);
        }
    }
}
