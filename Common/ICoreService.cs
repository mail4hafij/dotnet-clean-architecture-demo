using Common.Contract.Messaging;

namespace Common
{
    public interface ICoreService
    {
        // User operations
        GetUserResp GetUser(GetUserReq req);
        GetAllUserResp GetAllUser(GetAllUserReq req);

        // Car operations (with logic - business rules)
        AddCarResp AddCar(AddCarReq req);
        GetAllCarResp GetAllCar(GetAllCarReq req);
        GetUserCarsResp GetUserCars(GetUserCarsReq req);

        // Order operations (complex logic with dependencies)
        CreateOrderResp CreateOrder(CreateOrderReq req);
        CancelOrderResp CancelOrder(CancelOrderReq req);
        GetOrderSummaryResp GetOrderSummary(GetOrderSummaryReq req);
        GetUserOrdersResp GetUserOrders(GetUserOrdersReq req);
    }
}
