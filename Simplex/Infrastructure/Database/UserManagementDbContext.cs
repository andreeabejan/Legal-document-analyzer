using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Database
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
    }
}
