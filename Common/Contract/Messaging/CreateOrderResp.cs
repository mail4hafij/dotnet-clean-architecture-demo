namespace Common.Contract.Messaging
{
    public class CreateOrderResp : Resp
    {
        public long OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }
}
