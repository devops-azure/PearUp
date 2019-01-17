using PearUp.CommonEntities;

namespace PearUp.BusinessEntity.Builders
{
    public class AdminBuilder
    {
        public const string FullName_Should_Not_Be_Empty = "FullName should not be empty.";
        public const string Email_Should_Not_Be_Empty = "Email should not be empty.";
        public const string Password_Should_Not_Be_Empty = "Password should not be empty.";

        private string _name;
        private Password _password;
        private Email _email;
        private AdminBuilder()
        {

        }
        public static AdminBuilder Builder()
        {
            return new AdminBuilder();
        }
        public AdminBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }
        public AdminBuilder WithEmail(Email email)
        {
            this._email = email;
            return this;
        }
        public AdminBuilder WithPassword(Password password)
        {
            this._password = password;
            return this;
        }
        public Result<Admin> Build()
        {

            if (string.IsNullOrWhiteSpace(_name))
                return Result.Fail<Admin>(FullName_Should_Not_Be_Empty);

            if (_email == null)
                return Result.Fail<Admin>(Email_Should_Not_Be_Empty);

            if (_password == null)
                return Result.Fail<Admin>(Password_Should_Not_Be_Empty);

            var user = new Admin(_name, _email, _password);
            return Result.Ok(user);
        }
    }
}
