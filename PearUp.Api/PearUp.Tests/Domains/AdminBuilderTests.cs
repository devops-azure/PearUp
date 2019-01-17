using FluentAssertions;
using NUnit.Framework;
using PearUp.BusinessEntity;
using PearUp.BusinessEntity.Builders;
using System;

namespace PearUp.Tests.Domains
{

    [TestFixture]
    public class AdminBuilderTests
    {
        private static byte[] _Bytes
        {
            get
            {
                byte[] randomByteArray = new byte[32];
                Random random = new Random();
                random.NextBytes(randomByteArray);
                return randomByteArray;
            }
        }
        [Test]
        public void AdminBuilder_Should_Build_Valid_Admin_When_Inputs_Are_Valid()
        {
            var adminBuilder = AdminBuilder.Builder()
                .WithName("Admin1")
                .WithEmail(Email.Create("admin1@pearup.com").Value)
                .WithPassword(Password.Create(_Bytes, _Bytes).Value)
                .Build();
            adminBuilder.IsSuccessed.Should().BeTrue();
        }

        [Test]
        public void AdminBuilder_Should_Return_Full_Name_Empty_Error_When_Name_Is_Empty()
        {
            var adminBuilder = AdminBuilder.Builder()
                .WithName("")
                .WithEmail(Email.Create("admin1@pearup.com").Value)
                .WithPassword(Password.Create(_Bytes, _Bytes).Value)
                .Build();
            adminBuilder.IsSuccessed.Should().BeFalse();
            adminBuilder.GetErrorString().Should().Be(AdminBuilder.FullName_Should_Not_Be_Empty);
        }

        [Test]
        public void AdminBuilder_Should_Return_Email_Empty_Error_When_Email_Is_Empty()
        {
            var adminBuilder = AdminBuilder.Builder()
                .WithName("Admin")
                .WithEmail(null)
                .WithPassword(Password.Create(_Bytes, _Bytes).Value)
                .Build();
            adminBuilder.IsSuccessed.Should().BeFalse();
            adminBuilder.GetErrorString().Should().Be(AdminBuilder.Email_Should_Not_Be_Empty);
        }

        [Test]
        public void AdminBuilder_Should_Return_Password_Empty_Error_When_Password_Is_Empty()
        {
            var adminBuilder = AdminBuilder.Builder()
                .WithName("Admin")
                .WithEmail(Email.Create("admin1@pearup.com").Value)
                .WithPassword(null)
                .Build();
            adminBuilder.IsSuccessed.Should().BeFalse();
            adminBuilder.GetErrorString().Should().Be(AdminBuilder.Password_Should_Not_Be_Empty);

        }
    }
}
