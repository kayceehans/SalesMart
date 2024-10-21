using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.Entities;
using SalesMart.Infrastructure.Utilities;
using SalesMart.Service.Interface;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DashBoardController> _logger;
        private readonly IDashBoardService _dashboardService;
        public DashBoardController(IActivityLogService activityLogService, IConfiguration configuration, ILogger<DashBoardController> logger, ITokenMgtService tokenMgtService, IDashBoardService dashboardService)
        {
            _activityLogService = activityLogService;
            _configuration = configuration;
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("GetDashBoardItem")]
        public async Task<IActionResult> DashBoard()
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

                // Check Token
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

                return Ok(await _dashboardService.GetDashboardItem());

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured signing up:" + " " + ex.Message + " " + ex.StackTrace);
                return BadRequest(ex); ;
            }
        }
    }
}
