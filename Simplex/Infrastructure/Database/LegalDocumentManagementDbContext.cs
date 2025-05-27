using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class LegalDocumentManagementDbContext : DbContext
    {
        public LegalDocumentManagementDbContext(DbContextOptions<LegalDocumentManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<LegalDocument> LegalDocuments { get; set; } = null!;
    }

}
