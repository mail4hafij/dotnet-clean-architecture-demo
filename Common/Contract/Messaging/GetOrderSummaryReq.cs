namespace Common.Contract.Messaging
{
    public class GetOrderSummaryReq : Req
    {
        public long OrderId { get; set; }
    }
}
