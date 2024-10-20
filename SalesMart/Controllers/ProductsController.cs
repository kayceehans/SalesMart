using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Domain.Enums;
using SalesMart.Infrastructure.Utilities;
using SalesMart.Service.Interface;
using static SalesMart.Domain.Enums.Enum;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly ITokenMgtService _tokenMgt;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, IUserService userService, IActivityLogService activityLogService, IConfiguration configuration, ILogger<ProductsController> logger, ITokenMgtService tokenMgt)
        {
            _productService = productService;
            _userService = userService;
            _activityLogService = activityLogService;
            _configuration = configuration;
            _logger = logger;
            _tokenMgt = tokenMgt;
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddPorduct(ProductDto request)
        {
            try
            {
                // Validate API client
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

                // Get user's profile and valide role to perform action
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


                if (getUser.Content.RoleId != (int) RoleType.Admin)
                {
                    await _activityLogService.AddActivityLog(new ActivityLogs
                    {
                        Activity = $"{emailFromToken} is not permitted to add product",
                        Date = DateTime.Now,
                        Email = emailFromToken
                    });

                    var response = new Result<string>()
                    {
                        Content = $"{getUser.Content.Email} Not permitted.",
                        IsSuccess = false,
                        ErrorMessage = "Profile not permiited",
                        Message = "Only admin can add products"
                    };
                    return BadRequest(response);
                };

                _logger.LogInformation($"{emailFromToken} about to add product");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{emailFromToken} request to add product",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });


                var addProductResponse = await _productService.CreateProduct(request);

                _logger.LogInformation($"Add Product Response => {JsonConvert.SerializeObject(addProductResponse)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"add Product Response:{addProductResponse.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                return Ok(addProductResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured getting product:" + " " + ex.Message + " " + ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetPorduct()
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
                _logger.LogInformation($"{emailFromToken} get all products");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{emailFromToken} request to get all products",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                var getProducts = await _productService.GetProducts();

                _logger.LogInformation($"Get all products Response => {JsonConvert.SerializeObject(getProducts)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Get all Products Response:{getProducts.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                return Ok(getProducts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured getting products:" + " " + ex.Message + " " + ex.StackTrace);
                return BadRequest(ex);
            }

        }
        [HttpGet]
        [Route("GetProductsById")]
        public async Task<IActionResult> GetProductById(int id)
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
                _logger.LogInformation($"{emailFromToken} get product");

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"{emailFromToken} request to get product",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                var getProduct = await _productService.GetProductById(id);

                _logger.LogInformation($"Get product Response => {JsonConvert.SerializeObject(getProduct)}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Get all Products Response:{getProduct.Message})",
                    Date = DateTime.Now,
                    Email = emailFromToken
                });

                return Ok(getProduct);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured getting product:" + " " + ex.Message + " " + ex.StackTrace);
                return BadRequest(ex);
            }

        }
    }
}
