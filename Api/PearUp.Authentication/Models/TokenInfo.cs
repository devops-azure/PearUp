namespace PearUp.CommonEntity
{
    public class TokenInfo
    {
        public string FullName { get; set; } = string.Empty;

        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return this.Id == ((TokenInfo)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
