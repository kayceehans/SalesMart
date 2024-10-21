using Microsoft.Extensions.Logging;
using SalesMart.Data.Repositories;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepo<User> _userRepo;
        private readonly ILogger<UserService> _logger;
        public UserService(IGenericRepo<User> userRepo, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }
        public async Task<Result<List<User>>> GetUsers()
        {
            try
            {
                var response = new Result<List<User>>();
                var users = await _userRepo.GetAllAsync();
                if (users != null)
                {
                    response.Content = users;
                    response.IsSuccess = true;
                    response.Message = "List of all onboarded users";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get user records";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<List<User>>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<string>> CreateUser(UserDto user)
        {
            try
            {
                var response = new Result<string>();
                var request = new User
                {
                    DateOnBoarded = DateTime.Now,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId
                };

                  _userRepo.Add(request);
                var isAdded = await _userRepo.SaveAsync();
                if (isAdded > 0 )
                {
                    response.Content = "Sign-up successful";
                    response.IsSuccess = true;
                    response.Message = $"Welcome onboard! {user.FirstName}";
                }
                else
                {
                    response.Content = "Sign-up fail.";
                    response.IsSuccess = false;
                    response.Message = "Please try again";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<string>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<User>> GetUserByEmail(string email)
        {
            try
            {
                var response = new Result<User>();
                var getUser = await _userRepo.SelectAsync(c=>c.Email == email);
                if (getUser != null)
                {
                    response.Content = getUser;
                    response.IsSuccess = true;
                    response.Message = "User retrieved successfully";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get user record";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<User>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
    }
}
