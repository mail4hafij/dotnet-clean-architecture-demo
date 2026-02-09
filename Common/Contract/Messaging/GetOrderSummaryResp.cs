namespace Common.Contract.Messaging
{
    public class GetOrderSummaryResp : Resp
    {
        public long OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserEmail { get; set; }
        public int UserTotalCars { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int ItemCount { get; set; }
    }
}
