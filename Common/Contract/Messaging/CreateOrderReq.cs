namespace Common.Contract.Messaging
{
    public class CreateOrderReq : Req
    {
        public long UserId { get; set; }
        public List<OrderItemInput> Items { get; set; }
    }

    public class OrderItemInput
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
