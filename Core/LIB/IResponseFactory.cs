/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Common.Contract;

namespace Core.LIB
{
    public interface IResponseFactory
    {
        TResp GetSuccessResp<TResp>() where TResp : Resp, new();
        TResp GetFailureResp<TResp>(string text = "") where TResp : Resp, new();
        TResp GetErrorResp<TResp>(Exception exception, string text = "") where TResp : Resp, new();
    }
}