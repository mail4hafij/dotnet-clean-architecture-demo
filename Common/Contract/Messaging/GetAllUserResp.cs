using Common.Contract.Model;

namespace Common.Contract.Messaging
{
    public class GetAllUserResp : Resp
    {
        public List<UserContract> userContracts = new List<UserContract>();
    }
}
