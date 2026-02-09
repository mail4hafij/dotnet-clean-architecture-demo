using Core.DB.Model;

namespace Core.DB.Repo
{
    public class OrderItemRepository : RepositoryBase, IOrderItemRepository
    {
        public OrderItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
            {
                throw new ArgumentNullException(nameof(orderItem));
            }

            _unitOfWork.Context.OrderItems.Add(orderItem);
            _unitOfWork.Context.SaveChanges();
        }

        public OrderItem? GetOrderItem(long orderItemId)
        {
            return _unitOfWork.Context.OrderItems
                .FirstOrDefault(oi => oi.OrderItemId == orderItemId && !oi.Deleted);
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderId(long orderId)
        {
            return _unitOfWork.Context.OrderItems
                .Where(oi => oi.OrderId == orderId && !oi.Deleted)
                .ToList();
        }
    }
}
