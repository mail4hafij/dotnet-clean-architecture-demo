using Common.Contract.Messaging;
using Core.DB;
using Core.LIB;

namespace Core.Handler.Order
{
    /// <summary>
    /// Shows OrderLogic using CarLogic to get additional user information
    /// Handler creates both CarLogic and OrderLogic, passing CarLogic to OrderLogic
    /// Demonstrates explicit logic-to-logic dependency
    /// </summary>
    public class GetOrderSummaryHandler : RequestHandler<GetOrderSummaryReq, GetOrderSummaryResp>
    {
        private readonly ILogicFactory _logicFactory;
        private readonly IRepositoryFactory _repositoryFactory;

        public GetOrderSummaryHandler(
            ILogicFactory logicFactory,
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IResponseFactory responseFactory)
            : base(unitOfWorkFactory, responseFactory)
        {
            _logicFactory = logicFactory;
            _repositoryFactory = repositoryFactory;
        }

        public override GetOrderSummaryResp Process(GetOrderSummaryReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    // Create CarLogic first
                    var carLogic = _logicFactory.CreateCarLogic(_repositoryFactory, unitOfWork);

                    // Create OrderLogic with CarLogic dependency
                    var orderLogic = _logicFactory.CreateOrderLogic(_repositoryFactory, carLogic, unitOfWork);

                    // OrderLogic uses CarLogic (passed via constructor) to get user's car count
                    // Logic returns the response directly - no mapping needed!
                    var response = orderLogic.GetOrderSummary(req.OrderId);

                    unitOfWork.Commit();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Failed to get order summary");
            }
        }
    }
}
