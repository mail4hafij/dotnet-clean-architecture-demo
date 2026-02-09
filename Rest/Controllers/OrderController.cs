using Common;
using Common.Contract.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly ICoreService _coreService;

        public OrderController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        /// <summary>
        /// COMPLEX LOGIC EXAMPLE: Create order with validation
        /// Uses OrderLogic which validates business rules and handles order creation
        /// OrderLogic internally uses CarLogic (demonstrates logic-to-logic dependency)
        /// </summary>
        /// <param name="req">Order creation request with items</param>
        /// <returns>Created order details</returns>
        /// <response code="200">Order created successfully</response>
        /// <response code="400">Validation failed</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateOrderResp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CreateOrderResp> CreateOrder([FromBody] CreateOrderReq req)
        {
            var resp = _coreService.CreateOrder(req);

            if (!resp.Success)
            {
                return BadRequest(new { Error = resp.Error });
            }

            return Ok(resp);
        }

        /// <summary>
        /// Cancel an existing order
        /// Business rules:
        /// - Order must exist
        /// - Order must belong to the user
        /// - Order must be in Pending or Confirmed status
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>Cancellation confirmation</returns>
        /// <response code="200">Order cancelled successfully</response>
        /// <response code="400">Validation failed</response>
        [HttpPost("{orderId}/cancel")]
        [ProducesResponseType(typeof(CancelOrderResp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CancelOrderResp> CancelOrder(long orderId, [FromQuery] long userId)
        {
            var req = new CancelOrderReq
            {
                OrderId = orderId,
                UserId = userId
            };

            var resp = _coreService.CancelOrder(req);

            if (!resp.Success)
            {
                return BadRequest(new { Error = resp.Error });
            }

            return Ok(resp);
        }

        /// <summary>
        /// Get order summary with user information
        /// DEMONSTRATES LOGIC-TO-LOGIC DEPENDENCY:
        /// OrderLogic uses CarLogic to get user's car count
        /// This shows how logic classes can depend on other logic classes
        /// without the handler knowing about it!
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Order summary including user's total cars</returns>
        /// <response code="200">Returns order summary</response>
        /// <response code="404">Order not found</response>
        [HttpGet("{orderId}/summary")]
        [ProducesResponseType(typeof(GetOrderSummaryResp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GetOrderSummaryResp> GetOrderSummary(long orderId)
        {
            var req = new GetOrderSummaryReq { OrderId = orderId };
            var resp = _coreService.GetOrderSummary(req);

            if (!resp.Success)
            {
                return NotFound(new { Error = resp.Error });
            }

            return Ok(resp);
        }

        /// <summary>
        /// Get all orders for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of user's orders</returns>
        /// <response code="200">Returns list of orders</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(GetUserOrdersResp), StatusCodes.Status200OK)]
        public ActionResult<GetUserOrdersResp> GetUserOrders(long userId)
        {
            var req = new GetUserOrdersReq { UserId = userId };
            var resp = _coreService.GetUserOrders(req);

            if (!resp.Success)
            {
                return BadRequest(new { Error = resp.Error });
            }

            return Ok(resp);
        }
    }
}
