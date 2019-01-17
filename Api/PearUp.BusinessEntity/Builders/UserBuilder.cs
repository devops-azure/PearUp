using PearUp.CommonEntities;
using System.Collections.Generic;
using System.Linq;

namespace PearUp.BusinessEntity.Builders
{
    public class UserBuilder
    {
        public const string Fun_And_Interests_Should_Not_Exceed_140_Characters = "Fun and interests should not exceed 140 characters.";
        public const string BucketList_Should_Not_Exceed_140_Characters = "Bucket list should not exceed 140 characters.";
        public const string FullName_Is_Required = "FullName is required.";
        public const string Location_Shouldnt_Be_Null = "Location shouldn't be null.";

        private string _name;
        private Password _password;
        private string _profession;
        private string _school;
        private string _funAndInterstingThings;
        private string _bucketList;
        private UserPhoneNumber _phoneNumber;
        private UserMatchPreference _userPreference;
        private Age _age;
        private Gender _gender;
        private List<Interest> _userInterests = new List<Interest>();
        private Location _location;

        private UserBuilder()
        {

        }

        public static UserBuilder Builder()
        {
            return new UserBuilder();
        }

        public UserBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }


        public UserBuilder WithPhoneNumber(UserPhoneNumber phoneNumber)
        {
            this._phoneNumber = phoneNumber;
            return this;
        }

        public UserBuilder WithPassword(Password password)
        {
            this._password = password;
            return this;
        }

        public UserBuilder WithMatchPreference(UserMatchPreference userPreference)
        {
            this._userPreference = userPreference;
            return this;
        }

        public UserBuilder WithAge(Age age)
        {
            this._age = age;
            return this;
        }

        public UserBuilder WithGender(Gender gender)
        {
            this._gender = gender;
            return this;
        }

        public UserBuilder WithUserInterests(List<Interest> userInterests)
        {
            this._userInterests = userInterests;
            return this;
        }

        public UserBuilder WithProfession(string profession)
        {
            this._profession = profession;
            return this;
        }

        public UserBuilder WithSchool(string school)
        {
            this._school = school;
            return this;
        }

        public UserBuilder WithFunAndInterestingThings(string funAndInterestingThings)
        {
            this._funAndInterstingThings = funAndInterestingThings;
            return this;
        }

        public UserBuilder WithBucketList(string bucketList)
        {
            this._bucketList = bucketList;
            return this;
        }

        public UserBuilder WithLocation(Location location)
        {
            this._location = location;
            return this;
        }

        public Result<User> Build()
        {
            if (_funAndInterstingThings?.Trim().Length > 140)
                return Result.Fail<User>(Fun_And_Interests_Should_Not_Exceed_140_Characters);
            if (_bucketList?.Trim().Length > 140)
                return Result.Fail<User>(BucketList_Should_Not_Exceed_140_Characters);
            if (string.IsNullOrWhiteSpace(_name))
                return Result.Fail<User>(FullName_Is_Required);

            var user = new User(_name,
                _phoneNumber,
                _password,
                _userPreference,
                _age,
                _gender,
                _profession,
                _school,
                _funAndInterstingThings,
                _bucketList,
                _location
                );
            if (_userInterests.Any())
                user.SetInterests(_userInterests);
            return Result.Ok(user);
        }
    }
}
