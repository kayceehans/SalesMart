using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Infrastructure.Utilities;
using SalesMart.Service.Interface;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly ITokenMgtService _tokenMgt;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IUserService userService, IActivityLogService activityLogService, IConfiguration configuration, ILogger<AccountController> logger, ITokenMgtService tokenMgt)
        {
            _userService = userService;
            _activityLogService = activityLogService;
            _configuration = configuration;
            _logger = logger;
            _tokenMgt = tokenMgt;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;

                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split().Last();

                if (token == null)
                {
                    var response = new Result<string>()
                    {
                        Content = "Session expired!/Invalid Token.",
                        IsSuccess = false,
                        ErrorMessage = "Valid token is required to proceed",
                        Message = "Please Login again"
                    };
                    return Unauthorized(response);
                };

                var emailFromToken = await _tokenMgt.DecryptTokenToUserdetails(token);

                if (emailFromToken == null)
                {
                    var response = new Result<string>()
                    {
                        Content = "Token expired!/Invalid Token.",
                        IsSuccess = false,
                        ErrorMessage = "Valid token is required to proceed",
                        Message = "Please Login with a valid token"
                    };
                    return BadRequest(response);
                };
                _logger.LogInformation($"{emailFromToken} tried get list of all users");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{emailFromToken} request to get all users profile",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                var GetAllUsers = await _userService.GetUsers();

                _logger.LogInformation($"Get all users Response => {JsonConvert.SerializeObject(GetAllUsers)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Get all users Response:{JsonConvert.SerializeObject(GetAllUsers)})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                return Ok(GetAllUsers);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured signing up:" + " " + ex.Message + " " + ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;

                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split().Last();

                if (token == null)
                {
                    var response = new Result<string>()
                    {
                        Content = "Session expired!/Invalid Token.",
                        IsSuccess = false,
                        ErrorMessage = "Valid token is required to proceed",
                        Message = "Please Login again"
                    };
                    return Unauthorized(response);
                };

                var emailFromToken = await _tokenMgt.DecryptTokenToUserdetails(token);

                if (emailFromToken == null)
                {
                    var response = new Result<string>()
                    {
                        Content = "Token expired!/Invalid Token.",
                        IsSuccess = false,
                        ErrorMessage = "Valid token is required to proceed",
                        Message = "Please Login with a valid token"
                    };
                    return BadRequest(response);
                };
                _logger.LogInformation($"{emailFromToken} tried get user by email");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{emailFromToken} request to get user profile",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                var GetUser = await _userService.GetUserByEmail(email);

                _logger.LogInformation($"Get user Response => {JsonConvert.SerializeObject(GetUser)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Get user Response:{JsonConvert.SerializeObject(GetUser)})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                return Ok(GetUser);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured getting user by email:" + " " + ex.Message + " " + ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }

}

