using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Infrastructure.Utilities;
using SalesMart.Service.Implementation;
using SalesMart.Service.Interface;
using static SalesMart.Domain.Enums.Enum;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly IUserService _userService;
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly ITokenMgtService _tokenMgt;
        private readonly ILogger<SalesController> _logger;
        public SalesController(ISalesOrderService salesOrderService, IUserService userService, IActivityLogService activityLogService, IConfiguration configuration, ILogger<SalesController> logger, ITokenMgtService tokenMgt)
        {
            _salesOrderService = salesOrderService;
            _userService = userService;
            _activityLogService = activityLogService;
            _configuration = configuration;
            _logger = logger;
            _tokenMgt = tokenMgt;
        }

        [HttpGet]
        [Route("GetAllSalesOrder")]
        public async Task<IActionResult> GetSales()
        {
            try
            {
                // check API client ID
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;


                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                // Validate Token

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

                //Send Request to Add Sales order
                var getSalesresponse = await _salesOrderService.GetAllSales();

                _logger.LogInformation($"Get all Sales Response => {JsonConvert.SerializeObject(getSalesresponse)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Get All Sale Response:{getSalesresponse.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });
                return Ok(getSalesresponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }

        [HttpPost]
        [Route("AddSales")]
        public async Task<IActionResult> AddSalesOrder(SalesOrderDto request)
        {
            try
            {
                // check API client ID
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;


                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                // Validate Token

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

                // Check user details
                var getUser = await _userService.GetUserByEmail(emailFromToken);

                if (!getUser.IsSuccess)
                {
                    var response = new Result<string>()
                    {
                        Content = "User Records not found.",
                        IsSuccess = false,
                        ErrorMessage = "Valid token is required to proceed",
                        Message = "Please Login, and try again"
                    };
                    return BadRequest(response);
                };

                // Validate User Role

                if (getUser.Content.RoleId != (int)RoleType.Admin)
                {
                    await _activityLogService.AddActivityLog(new ActivityLogs
                    {
                        Activity = $"{emailFromToken} is not permitted to add Sales",
                        Date = DateTime.Now,
                        Email = emailFromToken
                    });

                    var response = new Result<string>()
                    {
                        Content = $"{getUser.Content.Email} Not permitted.",
                        IsSuccess = false,
                        ErrorMessage = "Profile not permiited",
                        Message = "Only admin can add Sales Order"
                    };
                    return BadRequest(response);
                };

                _logger.LogInformation($"{emailFromToken} add Sales");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{emailFromToken} request to add Sales",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                //Send Request to Add Sales order
                var addSalesresponse = await _salesOrderService.AddSales(request);

                _logger.LogInformation($"Add Sales Response => {JsonConvert.SerializeObject(addSalesresponse)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"add Sales Response:{addSalesresponse.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });
                return Ok(addSalesresponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest(ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("DeleteSalesOrder")]
        public async Task<IActionResult> DeleteSales(int Id)
        {
            try
            {
                // check API client ID
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;


                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                // Validate Token

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

                // Check user details
                var getUser = await _userService.GetUserByEmail(emailFromToken);

                if (!getUser.IsSuccess)
                {
                    var response = new Result<string>()
                    {
                        Content = "User Records not found.",
                        IsSuccess = false,
                        ErrorMessage = "Valid token is required to proceed",
                        Message = "Please Login, and try again"
                    };
                    return BadRequest(response);
                };

                // Validate User Role

                if (getUser.Content.RoleId != (int)RoleType.Admin)
                {
                    await _activityLogService.AddActivityLog(new ActivityLogs
                    {
                        Activity = $"{emailFromToken} is not permitted to delete Sales",
                        Date = DateTime.Now,
                        Email = emailFromToken
                    });

                    var response = new Result<string>()
                    {
                        Content = $"{getUser.Content.Email} Not permitted.",
                        IsSuccess = false,
                        ErrorMessage = "Profile not permiited",
                        Message = "Only admin can delete Sales Order"
                    };
                    return BadRequest(response);
                };

                //Send Request to Add Sales order
                var deleteSalesresponse = await _salesOrderService.DeleteSalesById(Id);

                _logger.LogInformation($"Delete Sales Response => {JsonConvert.SerializeObject(deleteSalesresponse)}");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Delete Sale Response:{deleteSalesresponse.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });
                return Ok(deleteSalesresponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest(ex.InnerException);
            }
        }
        [HttpGet]
        [Route("GetSalesById")]
        public async Task<IActionResult> GetSalesById(int Id)
        {
            try
            {
                // check API client ID
                var clientId = Request.Headers["ClientID"];
                var getAPI_key = _configuration.GetSection("CLientID").Value;


                if (clientId != getAPI_key)
                {
                    return Unauthorized();
                }

                // Validate Token

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


                //Send Request to Add Sales order
                var getSaleresponse = await _salesOrderService.GetSalesById(Id);

                _logger.LogInformation($"Get all Sales Response => {JsonConvert.SerializeObject(getSaleresponse)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Get All Sale Response:{getSaleresponse.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });
                return Ok(getSaleresponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest(ex.InnerException);
            }
        }
    }
}
