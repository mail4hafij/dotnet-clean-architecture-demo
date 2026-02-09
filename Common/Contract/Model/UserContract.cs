namespace Common.Contract.Model
{
    public class UserContract
    {
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<CarContract> CarContracts { get; set; } = new List<CarContract>();
    }
}
