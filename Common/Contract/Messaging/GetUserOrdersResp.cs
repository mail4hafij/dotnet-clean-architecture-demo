using Common.Contract.Model;

namespace Common.Contract.Messaging
{
    public class GetUserOrdersResp : Resp
    {
        public List<OrderContract> Orders { get; set; } = new List<OrderContract>();
        public int TotalCount { get; set; }
    }
}
