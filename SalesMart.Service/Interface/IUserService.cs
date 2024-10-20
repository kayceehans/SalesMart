using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Interface
{
    public interface IUserService
    {
        Task<Result<List<User>>> GetUsers();
        Task<Result<string>> CreateUser(UserDto user);
        Task<Result<User>> GetUserByEmail(string email);        
    }
}
