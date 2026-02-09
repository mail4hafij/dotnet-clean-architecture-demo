namespace Common.Contract.Messaging
{
    public class CancelOrderReq : Req
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
    }
}
