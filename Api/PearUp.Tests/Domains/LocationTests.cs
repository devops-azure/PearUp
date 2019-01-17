using FluentAssertions;
using NUnit.Framework;
using PearUp.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class LocationTests
    {
        [TestCase(23.23222, -44.33345)]
        public void Create_Should_Return_Success_Result_For_Valid_Inputs(double latitude, double longitude)
        {
            var locationResult = Location.Create(latitude,longitude);
            locationResult.IsSuccessed.Should().BeTrue();
            locationResult.Value.Longitude.Should().Be(longitude);
            locationResult.Value.Latitude.Should().Be(latitude);
        }

        [TestCase(0, 0)]
        public void Create_Should_Return_Failure_Result_For_Lat_And_Long_As_0(double latitude, double longitude)
        {
            var locationResult = Location.Create(latitude, longitude);
            locationResult.IsSuccessed.Should().BeTrue();
            locationResult.Value.Longitude.Should().Be(longitude);
            locationResult.Value.Latitude.Should().Be(latitude);
        }

        [TestCase(23.42343, 181)]
        [TestCase(23.42343, 180.01)]
        [TestCase(23.42343, 180.00001)]
        [TestCase(23.42343, -181)]
        [TestCase(23.42343, -180.01)]
        [TestCase(23.42343, -180.00001)]
        public void Create_Should_Return_Failure_Result_For_Longitude_Less_Than_minus_180_Greater_Than_plus_180(double latitude, double longitude)
        {
            var locationResult = Location.Create(latitude, longitude);
            locationResult.IsSuccessed.Should().BeFalse();
            locationResult.GetErrorString().Should().Be(Location.Longitude_Valid_Range);
        }

        [TestCase(91, 23.15441)]
        [TestCase(90.09, 23.15441)]
        [TestCase(90.0001, 23.15441)]
        [TestCase(-91, 23.15441)]
        [TestCase(-90.09, 23.15441)]
        [TestCase(-90.0001, 23.15441)]
        public void Create_Should_Return_Failure_Result_For_Latitude_Less_Than_minus_90_Greater_Than_plus_90(double latitude, double longitude)
        {
            var locationResult = Location.Create(latitude, longitude);
            locationResult.IsSuccessed.Should().BeFalse();
            locationResult.GetErrorString().Should().Be(Location.Latitude_Valid_Range);
        }


        [TestCase(23.42343, 179.99)]
        [TestCase(23.42343, 180)]
        [TestCase(23.42343, -179.99)]
        [TestCase(23.42343, -180)]
        public void Create_Should_Return_Success_Result_For_Longitude_Less_Than_minus_180_Greater_Than_plus_180(double latitude, double longitude)
        {
            var locationResult = Location.Create(latitude, longitude);
            locationResult.IsSuccessed.Should().BeTrue();
            locationResult.Value.Latitude.Should().Be(latitude);
            locationResult.Value.Longitude.Should().Be(longitude);
        }

        [TestCase(90, 23.15441)]
        [TestCase(89.99999, 23.15441)]
        [TestCase(-90, 23.15441)]
        [TestCase(-89.99999, 23.15441)]
        public void Create_Should_Return_Success_Result_For_Latitude_Less_Than_minus_90_Greater_Than_plus_90(double latitude, double longitude)
        {
            var locationResult = Location.Create(latitude, longitude);
            locationResult.IsSuccessed.Should().BeTrue();
            locationResult.Value.Latitude.Should().Be(latitude);
            locationResult.Value.Longitude.Should().Be(longitude);
        }
    }
}
