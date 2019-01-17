using FluentAssertions;
using NUnit.Framework;
using PearUp.BusinessEntity;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    public class UserPhotoTests
    {
        [TestCase(4, "path.test")]
        [TestCase(2, "path.test")]
        [TestCase(3, "path.test")]
        public void Create_Should_Return_Success_Result_When_Inputs_Are_Valid(int order, string path)
        {
            var userPhoto = UserPhoto.Create(order, path);
            userPhoto.IsSuccessed.Should().BeTrue();
        }

        [TestCase(0, "path.test")]
        public void Create_Should_Return_Failure_Result_When_Order_Is_Invalid(int order, string path)
        {
            var userPhoto = UserPhoto.Create(order, path);
            userPhoto.IsSuccessed.Should().BeFalse();
            userPhoto.GetErrorString().Should().Be(UserPhoto.Order_Should_Be_Greater_Than_Zero);
        }

        [TestCase(2, "")]
        public void Create_Should_Return_Failure_Result_When_Path_Is_Empity(int order, string path)
        {
            var userPhoto = UserPhoto.Create(order, path);
            userPhoto.IsSuccessed.Should().BeFalse();
            userPhoto.GetErrorString().Should().Be(UserPhoto.Path_Should_Not_Be_Empty);
        }

    }
}
