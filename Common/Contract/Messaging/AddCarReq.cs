

namespace Common.Contract.Messaging
{
    public class AddCarReq : Req
    {
        public long UserId { get; set; }
        public string Nameplate { get; set; } = string.Empty;
    }
}
