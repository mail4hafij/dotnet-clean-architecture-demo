namespace Common.Contract.Messaging
{
    public class GetAllCarReq : Req
    {
        public string Email { get; set; } = string.Empty;
        public QueryParameters QueryParameters { get; set; } = new QueryParameters();
    }
}
