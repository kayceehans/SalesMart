using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Domain.DataTransferObject
{
    public class LoginResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }        
        public string Phone { get; set; }       
        public string token { get; set; }
    }
}
