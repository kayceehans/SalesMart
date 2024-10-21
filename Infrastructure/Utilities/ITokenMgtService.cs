using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Infrastructure.Utilities
{
    public interface ITokenMgtService
    {
        Task<string> generateJWtToken(User user);
        Task<string> DecryptTokenToUserdetails(string token);
        Task<string> RefreshToken(string token);
    }
}
