using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Service.Interface;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AuthenticationController(IActivityLogService activityLogService, IConfiguration configuration, ILogger<AccountController> logger, IAuthenticationService authenticationService)
        {
            _activityLogService = activityLogService;
            _configuration = configuration;
            _logger = logger;
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("Login/")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            try
            {
                _logger.LogInformation($"Attempt login with profile: {model.email} ");
                // Check client ID
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("ClientID").Value;

                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                //Send request to log users and generate login jwt token

                var resp = await _authenticationService.Login(model);
                _logger.LogInformation($"login response: {JsonConvert.SerializeObject(resp)}");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"login error occurs while logging-in: {ex.Message} ");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UserDto model)
        {
            try
            {
                // Check client ID
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;

                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                _logger.LogInformation($"create account: ==> {JsonConvert.SerializeObject(model)}");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{model.FirstName}{model.LastName} sent request to sign up",
                    Date = DateTime.Now,
                    Email = $"{model.Email} about to SignUp"
                });

                //Send request to sign up users

                var signUpResponse = await _authenticationService.SignUp(model);

                _logger.LogInformation($"create account: ==> {JsonConvert.SerializeObject(signUpResponse)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Signup response:{JsonConvert.SerializeObject(signUpResponse)})",
                    Date = DateTime.Now,
                    Email = model.Email
                });

                return Ok(signUpResponse);
            }
            catch (Exception ex)
            {
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{model.Email} Exception occurs while signing up:{ex.Message}",
                    Date = DateTime.Now,
                    Email = model.Email
                });

                _logger.LogError("An error occured signing up:" + " " + ex.Message + " " + ex.StackTrace);

                return BadRequest(ex);
            }
        }
    }
}
