using Common;
using Common.Contract;
using Common.Contract.Messaging;
using Common.Contract.Model;
using Microsoft.AspNetCore.Mvc;

namespace Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ICoreService _coreService;
        public UserController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns the list of users</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserContract>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserContract>> GetUsers([FromQuery] QueryParameters queryParameters)
        {
            var userResp = _coreService.GetAllUser(new GetAllUserReq() { QueryParameters = queryParameters });
            return Ok(userResp.userContracts);
        }
    }
}
