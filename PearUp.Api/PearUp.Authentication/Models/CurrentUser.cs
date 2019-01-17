namespace PearUp.CommonEntity
{
    public class CurrentUser
    {
        public readonly int UserId;

        private CurrentUser(int userId)
        {
            this.UserId = userId;
        }

        public static CurrentUser Create(int userId)
        {
            return new CurrentUser(userId);
        }
    }
}
