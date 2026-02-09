using Common;
using Common.Contract.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CarController : ControllerBase
    {
        private readonly ICoreService _coreService;

        public CarController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        /// <summary>
        /// SIMPLE LOGIC EXAMPLE: Add car with business rule validation
        /// Uses CarLogic which validates:
        /// - User exists
        /// - Nameplate is at least 3 characters
        /// - No duplicate nameplates for same user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="nameplate">Car nameplate</param>
        /// <returns>Success message</returns>
        /// <response code="200">Car added successfully</response>
        /// <response code="400">Validation failed</response>
        [HttpPost]
        [ProducesResponseType(typeof(AddCarResp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AddCarResp> AddCar([FromQuery] long userId, [FromQuery] string nameplate)
        {
            var req = new AddCarReq
            {
                UserId = userId,
                Nameplate = nameplate
            };

            var resp = _coreService.AddCar(req);

            if (!resp.Success)
            {
                return BadRequest(new { Error = resp.Error });
            }

            return Ok(resp);
        }

        /// <summary>
        /// Get all cars for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of user's cars</returns>
        /// <response code="200">Returns list of cars</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(GetUserCarsResp), StatusCodes.Status200OK)]
        public ActionResult<GetUserCarsResp> GetUserCars(long userId)
        {
            var req = new GetUserCarsReq { UserId = userId };
            var resp = _coreService.GetUserCars(req);

            if (!resp.Success)
            {
                return BadRequest(new { Error = resp.Error });
            }

            return Ok(resp);
        }
    }
}
