using AutoMapper;
using Common.Contract.Messaging;
using Core.DB;
using Core.DB.Logic;
using Core.LIB;

namespace Core.Handler.Order
{
    /// <summary>
    /// COMPLEX LOGIC EXAMPLE: Handler using OrderLogic
    /// OrderLogic depends on CarLogic - Handler creates both and passes CarLogic to OrderLogic
    /// Dependencies are VISIBLE at Handler level!
    /// </summary>
    public class CreateOrderHandler : RequestHandler<CreateOrderReq, CreateOrderResp>
    {
        private readonly ILogicFactory _logicFactory;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public CreateOrderHandler(
            ILogicFactory logicFactory,
            IRepositoryFactory repositoryFactory,
            IMapper mapper,
            IUnitOfWorkFactory unitOfWorkFactory,
            IResponseFactory responseFactory)
            : base(unitOfWorkFactory, responseFactory)
        {
            _logicFactory = logicFactory;
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        public override CreateOrderResp Process(CreateOrderReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    // Create CarLogic first
                    var carLogic = _logicFactory.CreateCarLogic(_repositoryFactory, unitOfWork);

                    // Create OrderLogic and PASS CarLogic as parameter
                    // Dependency is visible right here!
                    var orderLogic = _logicFactory.CreateOrderLogic(_repositoryFactory, carLogic, unitOfWork);

                    // Logic handles all business rules and validation
                    // Pass request items directly - no mapping needed!
                    var order = orderLogic.CreateOrderWithValidation(req.UserId, req.Items);

                    unitOfWork.Commit();

                    return new CreateOrderResp
                    {
                        OrderId = order.OrderId,
                        OrderNumber = order.OrderNumber,
                        TotalAmount = order.TotalAmount,
                        ItemCount = req.Items.Count
                    };
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Failed to create order");
            }
        }
    }
}
