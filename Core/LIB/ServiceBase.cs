/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Common.Contract;

namespace Core.LIB
{
    public abstract class ServiceBase
    {
        private readonly IHandlerCaller _handlerCaller;

        protected ServiceBase(IHandlerCaller handlerCaller)
        {
            _handlerCaller = handlerCaller;
        }

        protected TResp Process<TReq, TResp>(TReq req) where TResp : Resp, new() where TReq : Req, new()
        {
            return _handlerCaller.Process<TReq, TResp>(req);
        }
    }
}
