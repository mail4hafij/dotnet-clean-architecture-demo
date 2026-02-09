using AutoMapper;
using Common.Contract.Messaging;
using Common.Contract.Model;
using Core.DB;
using Core.LIB;

namespace Core.Handler.Order
{
    /// <summary>
    /// Get all orders for a specific user
    /// Uses AutoMapper for entity-to-contract mapping
    /// </summary>
    public class GetUserOrdersHandler : RequestHandler<GetUserOrdersReq, GetUserOrdersResp>
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public GetUserOrdersHandler(
            IRepositoryFactory repositoryFactory,
            IMapper mapper,
            IUnitOfWorkFactory unitOfWorkFactory,
            IResponseFactory responseFactory)
            : base(unitOfWorkFactory, responseFactory)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        public override GetUserOrdersResp Process(GetUserOrdersReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    var orderRepo = _repositoryFactory.CreateOrderRepository(unitOfWork);
                    var orderItemRepo = _repositoryFactory.CreateOrderItemRepository(unitOfWork);

                    var orders = orderRepo.GetOrdersByUser(req.UserId);

                    // Use AutoMapper to map entities to contracts
                    var orderContracts = new List<OrderContract>();
                    foreach (var order in orders)
                    {
                        var orderContract = _mapper.Map<OrderContract>(order);
                        // Load order items
                        var items = orderItemRepo.GetOrderItemsByOrderId(order.OrderId);
                        orderContract.OrderItems = _mapper.Map<List<OrderItemContract>>(items);
                        orderContracts.Add(orderContract);
                    }

                    unitOfWork.Commit();

                    return new GetUserOrdersResp
                    {
                        Orders = orderContracts,
                        TotalCount = orderContracts.Count
                    };
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Failed to get user orders");
            }
        }
    }
}
