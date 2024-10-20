using Microsoft.EntityFrameworkCore;
using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Data.DBContext
{
    public class SalesMartContext: DbContext
    {
        public SalesMartContext(DbContextOptions<SalesMartContext> option): base(option)
        {
            
        }

        public DbSet<Product> products { get; set; }
        public DbSet<Role> roles { get; set; }  
        public DbSet<User> users { get; set; }
        public DbSet<SalesOrder> orders { get; set; }   
        public DbSet<ActivityLogs> activityLogs { get; set; }
    }
}
