using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class LegalDocumentRepository : ILegalDocumentRepository
    {
        private readonly LegalDocumentManagementDbContext _dbContext;
        public LegalDocumentRepository(LegalDocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Result<Guid>> AddLegalDocumentAsync(LegalDocument legalDocument)
        {
            var entityEntry = _dbContext.LegalDocuments.Add(legalDocument);
            _dbContext.SaveChanges();

            return Task.FromResult(Result<Guid>.Success(legalDocument.Id));
        }

        public Task<Result<Guid>> DeleteLegalDocumentAsync(Guid id)
        {
            var legalDocument = _dbContext.LegalDocuments.Find(id);
            if (legalDocument == null)
            {
                return Task.FromResult(Result<Guid>.Failure("Legal document not found"));
            }
            _dbContext.LegalDocuments.Remove(legalDocument);
            _dbContext.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(id));
        }

        public Task<Result<Guid>> DeleteLegalDocumentsByUserIdAsync(Guid userId)
        {
            var legalDocuments = _dbContext.LegalDocuments.Where(ld => ld.UserId == userId).ToList();
            if (legalDocuments == null || !legalDocuments.Any())
            {
                return Task.FromResult(Result<Guid>.Failure("No legal documents found for this user"));
            }
            _dbContext.LegalDocuments.RemoveRange(legalDocuments);
            _dbContext.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(userId));
        }

        public Task<Result<IEnumerable<LegalDocument>>> GetAll()
        {
            var legalDocuments = _dbContext.LegalDocuments.ToList();
            if (legalDocuments == null || !legalDocuments.Any())
            {
                return Task.FromResult(Result<IEnumerable<LegalDocument>>.Failure("No legal documents found"));
            }
            return Task.FromResult(Result<IEnumerable<LegalDocument>>.Success(legalDocuments));
        }

        public Task<Result<LegalDocument>> GetLegalDocumentByIdAsync(Guid id)
        {
            var legalDocument = _dbContext.LegalDocuments.Find(id);
            if (legalDocument == null)
            {
                return Task.FromResult(Result<LegalDocument>.Failure("Legal document not found"));
            }
            return Task.FromResult(Result<LegalDocument>.Success(legalDocument));

        }

        public Task<Result<LegalDocument>> GetLegalDocumentsByIdAsync(Guid Id)
        {
            var legalDocument = _dbContext.LegalDocuments.Find(Id);
            if (legalDocument == null)
            {
                return Task.FromResult(Result<LegalDocument>.Failure("Legal document not found"));
            }
            return Task.FromResult(Result<LegalDocument>.Success(legalDocument));
                
        }

        public Task<Result<IEnumerable<LegalDocument>>> GetLegalDocumentsByUserIdAsync(Guid userId)
        {
            var legalDocuments = _dbContext.LegalDocuments.Where(ld => ld.UserId == userId).ToList();
            if (legalDocuments == null || !legalDocuments.Any())
            {
                return Task.FromResult(Result<IEnumerable<LegalDocument>>.Failure("No legal documents found for this user"));
            }
            return Task.FromResult(Result<IEnumerable<LegalDocument>>.Success(legalDocuments));
        }

        public Task<Result<Guid>> UpdateLegalDocumentAsync(LegalDocument legalDocument)
        {
           var legalDocumentToUpdate = _dbContext.LegalDocuments.Find(legalDocument.Id);
            if (legalDocumentToUpdate == null)
            {
                return Task.FromResult(Result<Guid>.Failure("Legal document not found"));
            }
            //_dbContext.Entry(legalDocumentToUpdate).State = EntityState.Modified;
            legalDocumentToUpdate.Id = legalDocument.Id;
            legalDocumentToUpdate.UserId = legalDocument.UserId;
            legalDocumentToUpdate.Description = legalDocument.Description;
            legalDocumentToUpdate.Name = legalDocument.Name;
            legalDocumentToUpdate.CreatedAt = legalDocument.CreatedAt;
            legalDocumentToUpdate.UpdatedAt = legalDocument.UpdatedAt;
            legalDocumentToUpdate.FilePath = legalDocument.FilePath;
            _dbContext.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(legalDocument.Id));
        }
    }
}
