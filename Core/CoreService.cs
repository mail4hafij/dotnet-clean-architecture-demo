using Common;
using Common.Contract.Messaging;
using Core.LIB;

namespace Core
{
    public class CoreService : ServiceBase, ICoreService
    {
        public CoreService(IHandlerCaller handlerCaller) : base(handlerCaller) { }

        // User operations
        public GetUserResp GetUser(GetUserReq req) => Process<GetUserReq, GetUserResp>(req);
        public GetAllUserResp GetAllUser(GetAllUserReq req) => Process<GetAllUserReq, GetAllUserResp>(req);

        // Car operations (with logic - business rules)
        public AddCarResp AddCar(AddCarReq req) => Process<AddCarReq, AddCarResp>(req);
        public GetAllCarResp GetAllCar(GetAllCarReq req) => Process<GetAllCarReq, GetAllCarResp>(req);
        public GetUserCarsResp GetUserCars(GetUserCarsReq req) => Process<GetUserCarsReq, GetUserCarsResp>(req);

        // Order operations (complex logic with dependencies)
        public CreateOrderResp CreateOrder(CreateOrderReq req)
            => Process<CreateOrderReq, CreateOrderResp>(req);

        public CancelOrderResp CancelOrder(CancelOrderReq req)
            => Process<CancelOrderReq, CancelOrderResp>(req);

        public GetOrderSummaryResp GetOrderSummary(GetOrderSummaryReq req)
            => Process<GetOrderSummaryReq, GetOrderSummaryResp>(req);

        public GetUserOrdersResp GetUserOrders(GetUserOrdersReq req)
            => Process<GetUserOrdersReq, GetUserOrdersResp>(req);
    }
}
