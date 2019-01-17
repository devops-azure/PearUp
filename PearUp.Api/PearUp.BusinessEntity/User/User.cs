using System;
using System.Collections.Generic;
using System.Linq;

namespace PearUp.BusinessEntity
{
    public class User : Aggregate
    {
        public const string Interests_Cant_Be_Greater_Than_Ten = "Interests cannot be more than 10";
        public const string Atleast_One_Interest_Is_Required = "Atleast one interest is required";
        public const string User_Photo_Is_Required = "User Photo Is Required";
        public const string User_Location_Is_Required = "User Location Is Required";

        private User()
        {

        }

        internal User(string fullName,
                        UserPhoneNumber phoneNumber,
                        Password password,
                        UserMatchPreference preference,
                        Age age,
                        Gender gender,
                        string profession,
                        string school,
                        string funAndInterestingThings,
                        string bucketList,
                        Location location)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Password = password;
            MatchPreference = preference;
            Age = age;
            Gender = gender;
            Profession = profession;
            School = school;
            FunAndInterestingThings = funAndInterestingThings;
            BucketList = bucketList;
            Location = location;
        }
        public string FullName { get; private set; }
        public UserPhoneNumber PhoneNumber { get; private set; }
        public Password Password { get; private set; }
        public UserMatchPreference MatchPreference { get; private set; }
        public Age Age { get; private set; }
        public Gender Gender { get; private set; }
        public string Profession { get; private set; }
        public string School { get; private set; }
        public string FunAndInterestingThings { get; private set; }
        public string BucketList { get; private set; }
        private List<UserInterest> _interests = new List<UserInterest>();
        public IReadOnlyList<UserInterest> Interests => _interests;
        public Location Location { get; private set; }

        private List<UserPhoto> _photos = new List<UserPhoto>();
        public IReadOnlyList<UserPhoto> Photos => _photos;
        public void SetInterests(IEnumerable<Interest> interests)
        {
            if (interests == null)
            {
                throw new ArgumentException(Atleast_One_Interest_Is_Required);
            }
            var userInterests = interests.Distinct().Select(i => UserInterest.Create(i.Id).Value).ToList();
            if (userInterests.Count < 1)
            {
                throw new ArgumentException(Atleast_One_Interest_Is_Required);
            }
            if (userInterests.Count > 10)
            {
                throw new ArgumentException(Interests_Cant_Be_Greater_Than_Ten);
            }
            var interestsTobeRemoved = _interests.Except(userInterests).ToList();
            var interestsToBeAdded = userInterests.Except(_interests).ToList();
            interestsTobeRemoved.ForEach(interest => _interests.Remove(interest));
            interestsToBeAdded.ForEach(interest => _interests.Add(interest));
        }

        public void SetLocation(Location location)
        {
            if (location == null)
            {
                throw new ArgumentException(User_Location_Is_Required);
            }
            this.Location.ChangeLocation(location);
        }

        public void SetPhoto(UserPhoto userPhoto)
        {
            if (userPhoto == null)
                throw new ArgumentException(User_Photo_Is_Required);
            _photos.Add(userPhoto);
        }
    }
}
