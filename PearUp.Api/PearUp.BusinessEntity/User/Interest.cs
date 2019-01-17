using PearUp.CommonEntities;
using System;
using System.Collections.Generic;

namespace PearUp.BusinessEntity
{
    public class Interest : Entity
    {
        public const string Interest_Should_Not_Be_Empty = "Interest should not be empty.";
        public const string Interest_Name_Should_Not_Be_Empty = "Interest name should not be empty.";
        public const string Interest_Description_Should_Not_Be_Empty = "Interest description should not be empty.";

        public string InterestName { get; private set; }
        public string InterestDescription { get; private set; }
        public ICollection<UserInterest> UserInterests { get; set; }
        private Interest()
        {

        }

        internal Interest(string interestName, string interestDescription) : this()
        {
            this.InterestDescription = interestDescription;
            this.InterestName = interestName;
        }

        /// <summary>
        /// Used to Create Interest domain object.
        /// </summary>
        /// <param name="interestName">interest name as a string.</param>
        /// <param name="interestDescription">interest description as a string.</param>
        /// <returns>Interest result object.</returns>
        public static Result<Interest> Create(string interestName, string interestDescription)
        {
            if (string.IsNullOrWhiteSpace(interestName))
                return Result.Fail<Interest>(Interest_Name_Should_Not_Be_Empty);
            if (string.IsNullOrWhiteSpace(interestDescription))
                return Result.Fail<Interest>(Interest_Description_Should_Not_Be_Empty);

            return Result.Ok(new Interest(interestName, interestDescription));
        }

        /// <summary>
        /// Used to update the Interest domain model.
        /// </summary>
        /// <param name="interest">Interest domain object need to passed as param.</param>
        public void UpdateInterest(Interest interest)
        {
            if (interest == null)
                throw new ArgumentException(Interest_Should_Not_Be_Empty);
            if (string.IsNullOrWhiteSpace(interest.InterestName))
                throw new ArgumentNullException(Interest_Name_Should_Not_Be_Empty);
            if (string.IsNullOrWhiteSpace(interest.InterestDescription))
                throw new ArgumentNullException(Interest_Description_Should_Not_Be_Empty);
            this.InterestName = interest.InterestName;
            this.InterestDescription = interest.InterestDescription;
        }



        public override bool Equals(object obj)
        {
            return ((Interest)obj).Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
