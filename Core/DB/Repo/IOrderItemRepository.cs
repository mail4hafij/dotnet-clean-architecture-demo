using Core.DB.Model;

namespace Core.DB.Repo
{
    public interface IOrderItemRepository
    {
        void AddOrderItem(OrderItem orderItem);
        OrderItem? GetOrderItem(long orderItemId);
        IEnumerable<OrderItem> GetOrderItemsByOrderId(long orderId);
    }
}
