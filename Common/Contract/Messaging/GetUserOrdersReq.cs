namespace Common.Contract.Messaging
{
    public class GetUserOrdersReq : Req
    {
        public long UserId { get; set; }
    }
}
