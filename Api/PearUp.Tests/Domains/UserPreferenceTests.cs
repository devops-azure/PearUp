using NUnit.Framework;
using PearUp.BusinessEntity;
using System;
using FluentAssertions;
using System.Collections.Generic;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class UserPreferenceTests
    {
        private static IEnumerable<Tuple<Gender, int, int, int>> GetValidInputs
        {
            get
            {

                yield return new Tuple<Gender, int, int, int>(Gender.Create(1).Value, 25, 25, 50);
                yield return new Tuple<Gender, int, int, int>(Gender.Create(2).Value, 25, 25, 50);
                yield return new Tuple<Gender, int, int, int>(Gender.Create(3).Value, 25, 25, 50);
                yield return new Tuple<Gender, int, int, int>(Gender.Create(1).Value, 18, 100, 100);
                yield return new Tuple<Gender, int, int, int>(Gender.Create(1).Value, 100, 100, 1);

            }
        }

        [Test]
        public void Create_Should_Return_Fail_Result_For_Invalid_Gender()
        {
            Gender lookingFor = null;
            var userMatchPreferenceResult = UserMatchPreference.Create(lookingFor, 20, 25, 50);
            userMatchPreferenceResult.IsSuccessed.Should().BeFalse();
            userMatchPreferenceResult.GetErrorString().Should().Be(UserMatchPreference.Gender_Preference_Is_Required);
        }

        [TestCase(17)]
        public void Create_Should_Return_Fail_Result_For_MinAge_Less_Than_Eighteen(int minAge)
        {
            var userMatchPreferenceResult = UserMatchPreference.Create(Gender.Create(1).Value, minAge , 18, 50);
            userMatchPreferenceResult.IsSuccessed.Should().BeFalse();
            userMatchPreferenceResult.GetErrorString().Should().Be(UserMatchPreference.Minimum_Age_Preference_Should_Be_Greater_Than__Or_Equal_To_18);
        }


        [TestCase(101)]
        public void Create_Should_Return_Fail_Result_For_MaxAge_Greater_Than_Hundred(int maxAge)
        {
            var userMatchPreferenceResult = UserMatchPreference.Create(Gender.Create(1).Value, 18, maxAge, 50);
            userMatchPreferenceResult.IsSuccessed.Should().BeFalse();
            userMatchPreferenceResult.GetErrorString().Should().Be(UserMatchPreference.Maximum_age_Preference_Should_Not_Cross_100);
        }
        [TestCase(25,20)]
        [TestCase(100,18)]
        public void Create_Should_Return_Fail_Result_For_MinAge_Greater_MaxAge(int minAge, int maxAge)
        {
            var userMatchPreferenceResult = UserMatchPreference.Create(Gender.Create(1).Value, minAge, maxAge, 50);
            userMatchPreferenceResult.IsSuccessed.Should().BeFalse();
            userMatchPreferenceResult.GetErrorString().Should().Be(UserMatchPreference.Maximum_age_Preference_Should_Be_Greater_Than_Or_Equal_To_Minimum_Age_Preference);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Create_Should_Return_Fail_Result_For_Distance_Less_Than_One(int distance)
        {
            var userMatchPreferenceResult = UserMatchPreference.Create(Gender.Create(1).Value, 20, 25, distance);
            userMatchPreferenceResult.IsSuccessed.Should().BeFalse();
            userMatchPreferenceResult.GetErrorString().Should().Be(UserMatchPreference.Distance_Should_Be_Greater_Than_0);
        }

        [TestCase(101)]
        public void Create_Should_Return_Fail_Result_For_Distance_Greater_Than_Hundred(int distance)
        {
            var userMatchPreferenceResult = UserMatchPreference.Create(Gender.Create(1).Value, 20, 25, distance);
            userMatchPreferenceResult.IsSuccessed.Should().BeFalse();
            userMatchPreferenceResult.GetErrorString().Should().Be(UserMatchPreference.Distance_Should_Be_Less_Than_Or_Equal_To_100);
        }

        [TestCaseSource(nameof(GetValidInputs))]
        public void Create_Should_Return_Success_Result_For_Valid_Arguments(Tuple<Gender, int, int, int> input)
        {
            Gender lookingFor = input.Item1;
            int minAge = input.Item2;
            int maxAge = input.Item3;
            int distance = input.Item4;

            var userMatchPreferenceResult = UserMatchPreference.Create(lookingFor, minAge, maxAge, distance);
            userMatchPreferenceResult.IsSuccessed.Should().BeTrue();
        }
    }
}
