using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Database;

namespace Infrastructure.Database
{
public class LLMResultManagementDbContext : DbContext
{
    public LLMResultManagementDbContext(DbContextOptions<LLMResultManagementDbContext> options) : base(options)
    {
    }
    public DbSet<LLMResult> LLMResults { get; set; } = null!;
}
}

