using NUnit.Framework;
using PearUp.BusinessEntity;
using System;
using FluentAssertions;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class AgeTests
    {
        [TestCase(1999, 1, 1)]
        [TestCase(2000, 1, 1)]
        [TestCase(1998, 1, 1)]
        [TestCase(1900, 1, 1)]
        [TestCase(1993, 12, 31)]
        public void Create_Should_Return_Success_AgeResult_If_DateOfBirth_Is_Valid(int year, int month, int day)
        {
            var dateOfBirth = new DateTime(year, month, day);
            var result = Age.Create(dateOfBirth);
            result.IsSuccessed.Should().BeTrue();
            result.Value.CurrentAge.Should().BeGreaterOrEqualTo(18);
        }

        [Test]
        public void Create_Should_Return_Failed_AgeResult_If_DateOfBirth_Is_InValid()
        {
            var dateOfBirth = DateTime.Now.AddDays(-1);
            var result = Age.Create(dateOfBirth);
            result.IsSuccessed.Should().BeFalse();
            result.Value.Should().BeNull();
        }

        [Test]
        public void Create_Should_Return_Success_AgeResult_If_Age_Is_18_From_Todays_Date()
        {
            var todaysDateOfbirth = DateTime.Now.AddYears(-18);
            var result = Age.Create(todaysDateOfbirth);
            result.IsSuccessed.Should().BeTrue();
            result.Value.CurrentAge.Should().BeGreaterOrEqualTo(18);
        }

        [Test]
        public void Create_Should_Return_Failed_AgeResult_If_Age_Is_Not_18_From_Todays_Date()
        {
            var todaysDateOfbirth = DateTime.Now.AddYears(-18).AddDays(1);
            var result = Age.Create(todaysDateOfbirth);
            result.IsSuccessed.Should().BeFalse();
            result.Value.Should().BeNull();
        }
    }
}
