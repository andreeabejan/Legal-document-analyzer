using Domain.Entities;
using Domain.Interfaces;


namespace Application.Repositories
{
    public class LegalDocumentService
    {
        private readonly ILegalDocumentRepository _legalDocumentRepository;

        public LegalDocumentService(ILegalDocumentRepository legalDocumentRepository)
        {
            _legalDocumentRepository = legalDocumentRepository;
        }

        public async Task<LegalDocument> GetLegalDocumentByIdAsync(Guid id)
        {
            var result = await _legalDocumentRepository.GetLegalDocumentByIdAsync(id);
            if (result.IsSuccess)
            {
                return result.Data;
            }
            throw new Exception(result.ErrorMessage);
        }

        public async Task<IEnumerable<LegalDocument>> GetAllLegalDocumentsAsync()
        {
            var result = await _legalDocumentRepository.GetAll();
            if (result.IsSuccess)
            {
                return result.Data;
            }
            throw new Exception(result.ErrorMessage);
        }

        public async Task AddLegalDocumentAsync(LegalDocument legalDocument)
        {
            var result = await _legalDocumentRepository.AddLegalDocumentAsync(legalDocument);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public async Task UpdateLegalDocumentAsync(LegalDocument legalDocument)
        {
            var result = await _legalDocumentRepository.UpdateLegalDocumentAsync(legalDocument);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public async Task DeleteLegalDocumentAsync(Guid id)
        {
            var result = await _legalDocumentRepository.DeleteLegalDocumentAsync(id);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
        }
    }
}
