using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database
{
    public class AnalysisResultManagementDbContext : DbContext
    {
        public AnalysisResultManagementDbContext(DbContextOptions<AnalysisResultManagementDbContext> options) : base(options)
        {
        }
        public DbSet<AnalysisResult> AnalysisResults { get; set; } = null!;


    }
}
