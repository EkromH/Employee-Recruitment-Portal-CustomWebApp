using DeliveryService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data
{
    public class AppDbContext:IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Advertisement> advertisements { get; set; }
        public DbSet<StaffInfo> staffInfo { get; set; }
        public DbSet<Admin> admin { get; set; }

    }
}
