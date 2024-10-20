using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesMart.Domain.Common.Generic
{
    public class Result<T>
    {
        public T Content { get; set; }       
        public bool HasError => ErrorMessage != "";
        public string ErrorMessage { get; set; } = "";
        public string Message { get; set; } = "";       
        public bool IsSuccess { get; set; } = true;       
    }
}
