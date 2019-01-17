using PearUp.CommonEntities;
using System;
using System.Collections.Generic;

namespace PearUp.BusinessEntity
{
    public class Gender : ValueObject
    {
        public const string Invalid_Gender_Type = "Invalid gender type";

        public GenderType GenderType { get; private set; }

        private Gender()
        {

        }

        private Gender(GenderType genderType)
        {
            this.GenderType = genderType;
        }

        public static Result<Gender> Create(int genderType)
        {
            GenderType type;
            var isDefined = Enum.IsDefined(typeof(GenderType), genderType);
            if (isDefined)
                type = (GenderType)genderType;
            else
                return Result.Fail<Gender>(Invalid_Gender_Type);

            return Result.Ok(new Gender(type));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return GenderType;
        }
    }
}