/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Common.Contract;
using Core.DB;

namespace Core.LIB
{
    public abstract class RequestHandler<TReq, TResp> where TReq : Req, new() where TResp : Resp, new()
    {
        protected IUnitOfWorkFactory _unitOfWorkFactory { get; }
        private readonly IResponseFactory _responseFactory;

        public RequestHandler(IUnitOfWorkFactory unitOfWorkFactory, IResponseFactory responseFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _responseFactory = responseFactory;
        }

        public abstract TResp Process(TReq req);

        public TResp GetFailureResp(string text = "")
        {
            return _responseFactory.GetFailureResp<TResp>(text);
        }

        public TResp GetErrorResp(Exception ex, string text = "")
        {
            return _responseFactory.GetErrorResp<TResp>(ex, text);
        }
    }
}
