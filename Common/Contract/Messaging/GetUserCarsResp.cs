using Common.Contract.Model;

namespace Common.Contract.Messaging
{
    public class GetUserCarsResp : Resp
    {
        public List<CarContract> Cars { get; set; } = new List<CarContract>();
        public int TotalCount { get; set; }
    }
}
