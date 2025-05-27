using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILegalDocumentRepository
    {
        Task<Result<IEnumerable<LegalDocument>>> GetAll();
        Task<Result<Guid>> AddLegalDocumentAsync(LegalDocument legalDocument);
        Task<Result<LegalDocument>> GetLegalDocumentByIdAsync(Guid id);

        Task<Result<Guid>> UpdateLegalDocumentAsync(LegalDocument legalDocument);
        Task<Result<Guid>> DeleteLegalDocumentAsync(Guid id);

        Task<Result<Guid>> DeleteLegalDocumentsByUserIdAsync(Guid userId);
        Task<Result<LegalDocument>> GetLegalDocumentsByIdAsync(Guid Id);

        Task<Result<IEnumerable<LegalDocument>>> GetLegalDocumentsByUserIdAsync(Guid userId);

    }
}
