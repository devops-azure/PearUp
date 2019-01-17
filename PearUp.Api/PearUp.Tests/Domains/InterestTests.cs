using FluentAssertions;
using NUnit.Framework;
using PearUp.BusinessEntity;
using System;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    public class InterestTests
    {
        private Interest _testInterest;

        public InterestTests()
        {
            _testInterest = Interest.Create("test", "test").Value;
        }

        [Order(1)]
        [TestCase("test", "test description")]
        public void Create_Interest_Should_Return_Success_Result(string interestName, string interestDescription)
        {
            var result = Interest.Create(interestName, interestDescription);
            result.IsSuccessed.Should().BeTrue();
            result.Value.InterestDescription.Should().Be(interestDescription);
            result.Value.InterestName.Should().Be(interestName);
        }

        [TestCase("test", "", Interest.Interest_Description_Should_Not_Be_Empty)]
        [TestCase("", "test description", Interest.Interest_Name_Should_Not_Be_Empty)]
        [TestCase("test", null, Interest.Interest_Description_Should_Not_Be_Empty)]
        [TestCase(null, "test description", Interest.Interest_Name_Should_Not_Be_Empty)]
        public void Create_Interest_Should_Return_Failure_Result_If_InterestName_Or_InterestDescription_Is_Null_Or_Empty(string interestName, string interestDescription, string errorMessage)
        {
            var result = Interest.Create(interestName, interestDescription);
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(errorMessage);
        }

        [Order(2)]
        [TestCase("test","test")]
        public void Update_Interest_Should_Return_Success_Result(string interestName, string interestDescription)
        {
            var mockInterest = Interest.Create(interestName, interestDescription);
            _testInterest.UpdateInterest(mockInterest.Value);
            _testInterest.InterestDescription.Should().Be(interestDescription);
            _testInterest.InterestName.Should().Be(interestName);
        }

        [Order(3)]
        [TestCase("test", "")]
        [TestCase("", "test description")]
        [TestCase("test", null)]
        [TestCase(null, "test description")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void Update_Interest_Should_Return_Failure_Result(string interestName, string interestDescription)
        {
            try
            {
                var mockInterest = Interest.Create(interestName, interestDescription);
                _testInterest.UpdateInterest(mockInterest.Value);
            }
            catch(Exception ex)
            {
                ex.Message.Should().Be(Interest.Interest_Should_Not_Be_Empty);
            }
            
        }

    }
}
