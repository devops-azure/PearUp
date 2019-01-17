using PearUp.CommonEntities;
using System;
using System.Collections.Generic;

namespace PearUp.BusinessEntity
{
    public class Location : ValueObject
    {
        public const string Latitude_Is_Empty = "Latitude coordinates is empty.";
        public const string Longitude_Is_Empty = "Longitude coordinates is empty.";
        public const string Latitude_Coordinate_Is_Invalid = "Latitude coordinate is invalid.";
        public const string Longitude_Coordinate_Is_Invalid = "Longitude coordinate is invalid.";
        public const string Longitude_Valid_Range = "Longitude coordinate values should be between -180 and 180";
        public const string Latitude_Valid_Range = "Latitude coordinate values should be between -90 and 90";
        private Location()
        {

        }

        private Location(double latitude, double longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        
        public static Result<Location> Create(double latitude, double longitude)
        {
            if (!double.TryParse(latitude.ToString(), out double lat) )
                return Result.Fail<Location>(Latitude_Coordinate_Is_Invalid);
            if (!double.TryParse(longitude.ToString(), out double lng))
                return Result.Fail<Location>(Longitude_Coordinate_Is_Invalid);
            if(lat > 90 || lat < -90)
                return Result.Fail<Location>(Latitude_Valid_Range);
            if (lng > 180 || lng < -180)
                return Result.Fail<Location>(Longitude_Valid_Range);

            return Result.Ok(new Location(lat,lng));
        }

        public void ChangeLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException();
            this.Latitude = location.Latitude;
            this.Longitude = location.Longitude;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}
