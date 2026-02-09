
namespace Common.Contract.Model
{
    public class CarContract
    {
        public long CarId { get; set; }
        public long UserId { get; set; }
        public string Nameplate { get; set; } = string.Empty;
    }
}
