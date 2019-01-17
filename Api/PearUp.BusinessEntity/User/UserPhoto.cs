using PearUp.CommonEntities;

namespace PearUp.BusinessEntity
{
    public class UserPhoto
    {
        public const int Primary_Photo_Order = 1;
        public const string Order_Should_Be_Greater_Than_Zero = "Order Should be Grater Than Zero.";
        public const string Path_Should_Not_Be_Empty = "Path Should Not Be Empty.";
        public const string UserId_Should_be_Less_Than_Zero = "UserId Should be Less Than Zero.";
        public const string Order_Should_Not_Equals_To_Primary_Photo = "Order should not equals primary photo";

        private UserPhoto()
        {

        }
        private UserPhoto(int order, string path) : this()
        {
            this.Order = order;
            this.Path = path;
        }
        public int Order { get; private set; }
        public string Path { get; private set; }
        public int UserId { get; private set; }
        public static Result<UserPhoto> Create(int order, string path)
        {
            if (order <= default(int))
                return Result.Fail<UserPhoto>(Order_Should_Be_Greater_Than_Zero);

            if (string.IsNullOrWhiteSpace(path))
                return Result.Fail<UserPhoto>(Path_Should_Not_Be_Empty);

            if (order == Primary_Photo_Order)
                return CreatePrimaryPhoto(path);

            return Result.Ok(new UserPhoto(order, path));
        }

        private static Result<UserPhoto> CreatePrimaryPhoto(string path)
        {
            return Result.Ok(new UserPhoto(Primary_Photo_Order, path));
        }
    }
}
