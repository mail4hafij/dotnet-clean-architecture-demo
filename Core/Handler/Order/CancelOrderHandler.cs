using Common.Contract.Messaging;
using Core.DB;
using Core.LIB;

namespace Core.Handler.Order
{
    public class CancelOrderHandler : RequestHandler<CancelOrderReq, CancelOrderResp>
    {
        private readonly ILogicFactory _logicFactory;
        private readonly IRepositoryFactory _repositoryFactory;

        public CancelOrderHandler(
            ILogicFactory logicFactory,
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IResponseFactory responseFactory)
            : base(unitOfWorkFactory, responseFactory)
        {
            _logicFactory = logicFactory;
            _repositoryFactory = repositoryFactory;
        }

        public override CancelOrderResp Process(CancelOrderReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    // OrderLogic requires CarLogic in constructor, so create it
                    var carLogic = _logicFactory.CreateCarLogic(_repositoryFactory, unitOfWork);
                    var orderLogic = _logicFactory.CreateOrderLogic(_repositoryFactory, carLogic, unitOfWork);

                    // Logic handles all business rules
                    orderLogic.CancelOrder(req.OrderId, req.UserId);

                    unitOfWork.Commit();

                    return new CancelOrderResp
                    {
                        Message = $"Order {req.OrderId} cancelled successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Failed to cancel order");
            }
        }
    }
}
