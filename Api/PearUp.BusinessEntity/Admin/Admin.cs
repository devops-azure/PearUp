namespace PearUp.BusinessEntity
{
    public class Admin : Aggregate
    {
        private Admin()
        {

        }
        internal Admin(string fullName,Email email, Password password)
        {
            this.FullName = fullName;
            this.Email = email;
            this.Password = password;
        }        
        public string FullName { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
    }
}
