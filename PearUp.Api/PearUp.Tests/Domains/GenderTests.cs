using NUnit.Framework;
using PearUp.BusinessEntity;
using FluentAssertions;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class GenderTests
    {
        [TestCase(0)]
        [TestCase(4)]
        public void Create_Should_Return_Fail_Result_For_Invalid_genderType(int genderType)
        {
            var genderResult = Gender.Create(genderType);
            genderResult.IsSuccessed.Should().BeFalse();
            genderResult.GetErrorString().Should().Be(Gender.Invalid_Gender_Type);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Create_Should_Return_Success_Result_For_Valid_genderType(int genderType)
        {
            var expectedValue = (GenderType)genderType;

            var genderResult = Gender.Create(genderType);
            genderResult.IsSuccessed.Should().BeTrue();
            genderResult.Value.GenderType.Should().Be(expectedValue);
        }
    }
}
