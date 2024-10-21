using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Infrastructure.Utilities;
using SalesMart.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesMart.Service.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IActivityLogService _activityLogService;
        private readonly IConfiguration _configuration;
        private readonly ISMTPEmailSender _sendEmail;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IUserService _userService;
        private readonly ITokenMgtService _tokenMgt;
        public AuthenticationService(IActivityLogService activityLogService, IConfiguration configuration, ILogger<AuthenticationService> logger, IUserService userService, ISMTPEmailSender sendEmail, ITokenMgtService tokenMgt)
        {
            _activityLogService = activityLogService;
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
            _sendEmail = sendEmail;
            _tokenMgt = tokenMgt;
        }

        public async Task<Result<LoginResponseDto>> Login(LoginDto login)
        {
            try
            {
                var user = await _userService.GetUserByEmail(login.email);

                if (user.Content == null)
                {
                    _logger.LogInformation($"Record not found for profile {login.email}");
                    await _activityLogService.AddActivityLog(new ActivityLogs
                    {
                        Activity = $"Log in failed|Profile record not found",
                        Date = DateTime.Now,
                        Email = login.email
                    });
                    return new Result<LoginResponseDto> { Message = $"Record not found for profile {login.email}. Please enrol or sign-up to use this service", IsSuccess = false };
                }

                if (user.Content.Password != Encryptor.EncodePassword(login.password, _configuration.GetSection("PwdEncKey").Value))
                {
                    _logger.LogInformation($"Incorrect Password for profile {login.email}");
                    await _activityLogService.AddActivityLog(new ActivityLogs
                    {
                        Activity = $"Log in failed|Wrong Password",
                        Date = DateTime.Now,
                        Email = login.email
                    });
                    return new Result<LoginResponseDto> { Message = $"Incorrect Password.", IsSuccess = false };
                }


                if (user.Content.IsDeleted)
                {
                    _logger.LogInformation($"You are no longer authorized to access this portal {login.email}");
                    await _activityLogService.AddActivityLog(new ActivityLogs
                    {
                        Activity = $"Log in failed|Profile deleted",
                        Date = DateTime.Now,
                        Email = login.email
                    });
                    return new Result<LoginResponseDto> { Message = "You are no longer authorized to access this portal. Kindly contact Admin", IsSuccess = false };
                }

                var token = await _tokenMgt.generateJWtToken(user.Content);
                if (token == null) return new Result<LoginResponseDto> { ErrorMessage = $"Unable to generate Toke for {login.email}, please try again" };

                var response = new LoginResponseDto
                {
                    Email = user.Content.Email,
                    FirstName = user.Content.FirstName,
                    LastName = user.Content.LastName,
                    Phone = user.Content.PhoneNumber,
                    token = token,
                    RoleId = user.Content.RoleId
                };

                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Logged in successfully",
                    Date = DateTime.Now,
                    Email = login.email
                });

                return new Result<LoginResponseDto>
                {
                    Content = response,
                    Message = "Login Successful.",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error occured {ex.Message}|{login.email}| Logged in failed |{DateTime.Now}");
                await _activityLogService.AddActivityLog(new ActivityLogs
                {
                    Activity = $"Error occured {ex.Message}|{login.email}|Logged in failed|{DateTime.Now}"
                });
                return new Result<LoginResponseDto> { Message = $"Login Failed. Exception:{ex.Message}", IsSuccess = false };
            }
        }
        public async Task<Result<string>> SignUp(UserDto info)
        {
            try
            {
                _logger.LogInformation($"request to sign-up for: {info.Email}");

                if (!ValidatorService.isValidEmail(info.Email))
                {
                    return new Result<string>
                    {
                        Content = "Invalid Email address",
                        ErrorMessage = "Email format error",
                        IsSuccess = false,
                        Message = "Check email field and try again"
                    };
                }

                if ((info.Password.All(char.IsLetter)) || (info.Password.All(char.IsNumber)))
                {
                    _logger.LogInformation("Non-Alphanumeric String: Non accepted");

                    return new Result<string>
                    {
                        Content = $"Password has to be alphanumeric",
                        ErrorMessage = "Password format error",
                        IsSuccess = false,
                        Message = "Check password and try again"
                    };
                }

                var getUser = await _userService.GetUserByEmail(info.Email);

                if (getUser.Content != null && getUser.IsSuccess)
                {
                    return new Result<string>
                    {
                        Content = $"User with email:{info.Email} already onboarded/exist",
                        ErrorMessage = "User exist",
                        IsSuccess = false,
                        Message = "Kindly contact admin for profile reset"
                    };
                }

                var role = _configuration.GetSection("Roles").Value;
                bool isValidRole = role.Contains(info.RoleId.ToString());

                if (role == null || !isValidRole)
                    return new Result<string>
                    {
                        Content = "Invalid Role selected",
                        ErrorMessage = "Invalid role",
                        IsSuccess = false,
                        Message = "Check role and try again"
                    };

                //Encrypt Password:
                info.Password = Encryptor.EncodePassword(info.Password, _configuration.GetSection("PwdEncKey").Value);
                var signUpResponse = await _userService.CreateUser(info);

                if (!signUpResponse.IsSuccess)
                {
                    _logger.LogInformation($"User could not be added to db: {JsonConvert.SerializeObject(signUpResponse)}");
                    return signUpResponse;
                }
                _logger.LogInformation($"User added to db successfully to sign-up: {JsonConvert.SerializeObject(signUpResponse)}");

                //Send Welcome Email 
                _logger.LogInformation($"about to send email for Signing-up");

                EmailSenderDto emailRequest = new EmailSenderDto
                {
                    EmailTo = info.Email,
                    Body = $"Welcome OnBoard, {info.FirstName} {info.LastName} ",
                    Subject = "Welcome"
                };

                _logger.LogInformation($"about to send welcom email with details: {emailRequest.EmailTo}");


                var sendMail = await _sendEmail.Email(emailRequest);

                _logger.LogInformation($"Email sent : {JsonConvert.SerializeObject(sendMail)}");

                return new Result<string>
                {
                    Content = "Sign-Up Completed successfully.",
                    Message = $"Welcome Onboard:{info.FirstName}",
                    IsSuccess = true
                }; ;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occured. Could not complete sign-up: {ex.Message}|{ex.InnerException}|{ex.ToString()}");
                return new Result<string> { Content = $"an error occured: {ex.Message}" };
            }
        }
    }
}
