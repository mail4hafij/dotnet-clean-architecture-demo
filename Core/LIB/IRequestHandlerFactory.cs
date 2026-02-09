/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Common.Contract;

namespace Core.LIB
{
    public interface IRequestHandlerFactory
    {
        RequestHandler<TReq, TResp> Create<TReq, TResp>() where TReq : Req, new() where TResp : Resp, new();
    }
}