using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Service.Interface;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IActivityLogService _activityLogService;
        public AccountController(IUserService userService, IActivityLogService activityLogService)
        {
            _userService = userService;
            _activityLogService = activityLogService;
        }
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UserDto user)
        {
            try
            {
                await _activityLogService.AddActivityLog(new Domain.Entities.ActivityLogs
                {
                    Activity = $"Sign_Up request received from {user.Email}",
                    Date = DateTime.Now,
                    Email = user.Email
                }
              );

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
}

    }
}
