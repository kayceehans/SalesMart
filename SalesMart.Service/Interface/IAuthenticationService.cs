using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<Result<LoginResponseDto>> Login(LoginDto login);
        Task<Result<string>> SignUp(UserDto info);
    }
}
