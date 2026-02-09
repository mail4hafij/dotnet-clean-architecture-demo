using Common.Contract.Model;

namespace Common.Contract.Messaging
{
    public class GetAllCarResp : Resp
    {
        public List<CarContract> carContracts = new List<CarContract>();
    }
}
