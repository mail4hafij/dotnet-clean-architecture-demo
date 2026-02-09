using Common.Contract.Messaging;
using Common.Contract.Model;
using Core.DB.Model;

namespace Core.DB.Logic
{
    /// <summary>
    /// COMPLEX EXAMPLE: Logic class that depends on OTHER logic classes
    ///
    /// KEY POINT: Dependencies are passed as constructor parameters, NOT injected!
    /// Handler creates all dependencies and passes them explicitly.
    ///
    /// Traditional approach (BAD):
    ///   - Inject IUserLogic, ICarLogic, IPaymentLogic, etc. in constructor via DI
    ///   - Constructor becomes huge with 10+ injected dependencies
    ///   - Hard to maintain and understand
    ///
    /// Factory pattern approach (GOOD):
    ///   - Handler has factories injected
    ///   - Handler creates dependencies and passes them as parameters
    ///   - Dependencies are visible at Handler level
    ///   - Clean, no injection in Logic classes
    /// </summary>
    public class OrderLogic : LogicBase, IOrderLogic
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ICarLogic _carLogic;

        // NOTICE: Dependencies passed as PARAMETERS, not injected!
        // Handler creates these and passes them in
        public OrderLogic(
            IRepositoryFactory repositoryFactory,
            ICarLogic carLogic,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repositoryFactory = repositoryFactory;
            _carLogic = carLogic;
        }

        public Order CreateOrderWithValidation(long userId, List<OrderItemInput> items)
        {
            // Get repositories
            var userRepo = _repositoryFactory.CreateUserRepository(_unitOfWork);
            var orderRepo = _repositoryFactory.CreateOrderRepository(_unitOfWork);
            var orderItemRepo = _repositoryFactory.CreateOrderItemRepository(_unitOfWork);

            // Business Rule 1: User must exist
            var user = userRepo.GetUser(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} does not exist");
            }

            // Business Rule 2: Order must have at least one item
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("Order must have at least one item");
            }

            // Business Rule 3: Validate all items have valid quantities and prices
            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                {
                    throw new ArgumentException($"Invalid quantity for product '{item.ProductName}': {item.Quantity}");
                }
                if (item.UnitPrice <= 0)
                {
                    throw new ArgumentException($"Invalid price for product '{item.ProductName}': {item.UnitPrice}");
                }
            }

            // Business Rule 4: Generate unique order number
            var orderNumber = GenerateOrderNumber();
            if (orderRepo.OrderNumberExists(orderNumber))
            {
                throw new InvalidOperationException($"Order number {orderNumber} already exists");
            }

            // Calculate total amount
            var totalAmount = items.Sum(i => i.Quantity * i.UnitPrice);

            // Create the order
            var order = new Order(userId, orderNumber, DateTime.UtcNow, "Pending")
            {
                TotalAmount = totalAmount
            };
            orderRepo.AddOrder(order);

            // Flush to get the OrderId
            _unitOfWork.Flush();

            // Create order items
            foreach (var item in items)
            {
                var orderItem = new OrderItem(order.OrderId, item.ProductName, item.Quantity, item.UnitPrice);
                orderItemRepo.AddOrderItem(orderItem);
            }

            Console.WriteLine("═══════════════════════════════════════════");
            Console.WriteLine($"✅ Order Created: {orderNumber}");
            Console.WriteLine($"   User: {user.Email}");
            Console.WriteLine($"   Total: ${totalAmount:F2}");
            Console.WriteLine($"   Items: {items.Count}");
            Console.WriteLine("═══════════════════════════════════════════");

            return order;
        }

        public void CancelOrder(long orderId, long userId)
        {
            // Get repositories
            var userRepo = _repositoryFactory.CreateUserRepository(_unitOfWork);
            var orderRepo = _repositoryFactory.CreateOrderRepository(_unitOfWork);

            // Business Rule 1: User must exist
            var user = userRepo.GetUser(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} does not exist");
            }

            // Business Rule 2: Order must exist
            var order = orderRepo.GetOrder(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderId} does not exist");
            }

            // Business Rule 3: Order must belong to the user
            if (order.UserId != userId)
            {
                throw new InvalidOperationException($"Order {orderId} does not belong to user {userId}");
            }

            // Business Rule 4: Order must be cancellable (Pending or Confirmed only)
            if (order.Status != "Pending" && order.Status != "Confirmed")
            {
                throw new InvalidOperationException($"Cannot cancel order with status '{order.Status}'");
            }

            // Cancel the order
            order.Status = "Cancelled";

            Console.WriteLine("═══════════════════════════════════════════");
            Console.WriteLine($"❌ Order Cancelled: {order.OrderNumber}");
            Console.WriteLine($"   User: {user.Email}");
            Console.WriteLine("═══════════════════════════════════════════");
        }

        public GetOrderSummaryResp GetOrderSummary(long orderId)
        {
            // Get repositories
            var userRepo = _repositoryFactory.CreateUserRepository(_unitOfWork);
            var orderRepo = _repositoryFactory.CreateOrderRepository(_unitOfWork);
            var orderItemRepo = _repositoryFactory.CreateOrderItemRepository(_unitOfWork);

            var order = orderRepo.GetOrder(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderId} does not exist");
            }

            var user = userRepo.GetUser(order.UserId);
            var orderItems = orderItemRepo.GetOrderItemsByOrderId(orderId);

            // Using CarLogic (passed via constructor) to get user's car count
            // This demonstrates logic-to-logic dependency
            var userCarCount = _carLogic.GetUserCarCount(user.UserId);

            return new GetOrderSummaryResp
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                UserEmail = user.Email,
                UserTotalCars = userCarCount, // Data from CarLogic!
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ItemCount = orderItems.Count()
            };
        }

        private string GenerateOrderNumber()
        {
            // Simple order number generation
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
