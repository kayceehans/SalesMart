using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SalesMart.Domain.Enums.Enum;

namespace SalesMart.Domain.DataTransferObject
{
    public class UserDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Password { get; set; }       
        public int RoleId { get; set; }        
    }
}
