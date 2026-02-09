/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Autofac;
using Common.Contract;

namespace Core.LIB
{
    public class RequestHandlerFactory : IRequestHandlerFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public RequestHandlerFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public RequestHandler<TReq, TResp> Create<TReq, TResp>() where TReq : Req, new() where TResp : Resp, new()
        {
            // Hafij: Ninject had this factory feature but not in autofac
            return _lifetimeScope.Resolve<RequestHandler<TReq, TResp>>();
        }

    }
}
