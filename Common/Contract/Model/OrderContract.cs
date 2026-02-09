namespace Common.Contract.Model
{
    public class OrderContract
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemContract> OrderItems { get; set; } = new List<OrderItemContract>();
    }
}
