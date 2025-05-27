using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAnalysisResultRepository
    {
        Task<Result<Guid>> AddAnalysisResultAsync(AnalysisResult analysisResult);
        Task<Result<AnalysisResult>> GetAnalysisResultByIdAsync(Guid id);
        Task<Result<AnalysisResult>> GetAnalysisResultByDocumentIdAsync(Guid documentId);
        Task<Result<Guid>> DeleteAnalysisResultAsync(Guid id);

        Task<Result<Guid>> DeleteAnalysisResultByDocumentIdAsync(Guid documentId);
        Task<Result<IEnumerable<AnalysisResult>>> GetAllAnalysisResultsAsync();
    }
}
