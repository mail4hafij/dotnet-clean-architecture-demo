/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Common.Contract;

namespace Core.LIB
{
    public class ResponseFactory : IResponseFactory
    {
        public TResp GetSuccessResp<TResp>() where TResp : Resp, new()
        {
            return new TResp()
            {
                Success = true
            };
        }

        public TResp GetFailureResp<TResp>(string text = "") where TResp : Resp, new()
        {
            return new TResp()
            {
                Success = false,
                Error = new Error()
                {
                    Text = text
                }
            };
        }

        public TResp GetErrorResp<TResp>(Exception exception, string text = "") where TResp : Resp, new()
        {
            return new TResp()
            {
                Success = false,
                Error = new Error()
                {
                    StackTrace = exception.Message,
                    Text = text == "" ? exception.Message : text
                }
            };
        }

    }
}