using Core.DB.Model;

namespace Core.DB.Repo
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _unitOfWork.Context.Orders.Add(order);
            _unitOfWork.Context.SaveChanges();
        }

        public Order? GetOrder(long orderId)
        {
            return _unitOfWork.Context.Orders
                .FirstOrDefault(o => o.OrderId == orderId && !o.Deleted);
        }

        public IEnumerable<Order> GetOrdersByUser(long userId)
        {
            return _unitOfWork.Context.Orders
                .Where(o => o.UserId == userId && !o.Deleted)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public bool OrderNumberExists(string orderNumber)
        {
            return _unitOfWork.Context.Orders
                .Any(o => o.OrderNumber == orderNumber && !o.Deleted);
        }
    }
}
