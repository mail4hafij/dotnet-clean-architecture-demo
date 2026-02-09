using Core.DB.Model;

namespace Core.DB.Repo
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        Order? GetOrder(long orderId);
        IEnumerable<Order> GetOrdersByUser(long userId);
        bool OrderNumberExists(string orderNumber);
    }
}
