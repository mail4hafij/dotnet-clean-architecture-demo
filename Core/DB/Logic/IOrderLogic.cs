using Common.Contract.Messaging;
using Common.Contract.Model;
using Core.DB.Model;

namespace Core.DB.Logic
{
    /// <summary>
    /// COMPLEX EXAMPLE: Logic class that depends on OTHER logic classes
    /// Uses existing contracts from Common project - no duplicate classes!
    /// </summary>
    public interface IOrderLogic
    {
        /// <summary>
        /// Creates a new order with validation
        /// Business rules:
        /// - User must exist
        /// - Order must have at least one item
        /// - All items must have valid quantity and price
        /// - Order number must be unique
        /// </summary>
        Order CreateOrderWithValidation(long userId, List<OrderItemInput> items);

        /// <summary>
        /// Cancels an order
        /// Business rules:
        /// - Order must exist
        /// - Order must belong to the user
        /// - Order must be in Pending or Confirmed status
        /// </summary>
        void CancelOrder(long orderId, long userId);

        /// <summary>
        /// Gets order summary with user information
        /// Demonstrates using CarLogic for additional operations (logic-to-logic dependency)
        /// Returns GetOrderSummaryResp - reusing existing response contract
        /// </summary>
        GetOrderSummaryResp GetOrderSummary(long orderId);
    }
}
