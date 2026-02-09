namespace Common.Contract.Messaging
{
    public class GetAllUserReq : Req
    {
        public QueryParameters QueryParameters { get; set; } = new QueryParameters();
    }
}
