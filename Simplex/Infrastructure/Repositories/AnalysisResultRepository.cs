using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class AnalysisResultRepository : IAnalysisResultRepository
    {
        private readonly AnalysisResultManagementDbContext _context;

        public AnalysisResultRepository(AnalysisResultManagementDbContext context, IOllamaModel ollamaModel)
        {
            _context = context;
        }

        public async Task<Result<Guid>> AddAnalysisResultAsync(AnalysisResult analysisResult)
        {
            _context.AnalysisResults.Add(analysisResult);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(analysisResult.Id);
        }


        public Task<Result<Guid>> DeleteAnalysisResultAsync(Guid id)
        {
            var analysisResult = _context.AnalysisResults.Find(id);
            if (analysisResult == null)
            {
                return Task.FromResult(Result<Guid>.Failure("Analysis result not found"));
            }
            _context.AnalysisResults.Remove(analysisResult);
            _context.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(id));
        }

        public Task<Result<IEnumerable<AnalysisResult>>> GetAllAnalysisResultsAsync()
        {
            var analysisResults = _context.AnalysisResults.ToList();
            if (analysisResults == null || !analysisResults.Any())
            {
                return Task.FromResult(Result<IEnumerable<AnalysisResult>>.Failure("No analysis results found"));
            }
            return Task.FromResult(Result<IEnumerable<AnalysisResult>>.Success(analysisResults));
        }

        public Task<Result<AnalysisResult>> GetAnalysisResultByDocumentIdAsync(Guid documentId)
        {
            var analysisResult = _context.AnalysisResults.FirstOrDefault(ar => ar.LegalDocumentId == documentId);
            if (analysisResult == null)
            {
                return Task.FromResult(Result<AnalysisResult>.Failure("Analysis result not found"));
            }
            return Task.FromResult(Result<AnalysisResult>.Success(analysisResult));
        }

        public Task<Result<AnalysisResult>> GetAnalysisResultByIdAsync(Guid id)
        {
            var analysisResult = _context.AnalysisResults.Find(id);
            if (analysisResult == null)
            {
                return Task.FromResult(Result<AnalysisResult>.Failure("Analysis result not found"));
            }
            return Task.FromResult(Result<AnalysisResult>.Success(analysisResult));
        }

        public Task<Result<Guid>> UpdateAnalysisResultAsync(AnalysisResult analysisResult)
        {
            var existingResult = _context.AnalysisResults.Find(analysisResult.Id);
            if (existingResult == null)
            {
                return Task.FromResult(Result<Guid>.Failure("Analysis result not found"));
            }
            existingResult.Id = analysisResult.Id;
            existingResult.LegalDocumentId = analysisResult.LegalDocumentId;
            existingResult.Name = analysisResult.Name;
            existingResult.Description = analysisResult.Description;
            existingResult.CreatedAt = analysisResult.CreatedAt;
            existingResult.UpdatedAt = analysisResult.UpdatedAt;
            existingResult.FilePath = analysisResult.FilePath;
            existingResult.OllamaAnalysisResultText = analysisResult.OllamaAnalysisResultText;
            existingResult.PhiAnalysisResultText = analysisResult.PhiAnalysisResultText;
            existingResult.GemmaAnalysisResultText = analysisResult.GemmaAnalysisResultText;
            _context.AnalysisResults.Update(existingResult);
            _context.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(existingResult.Id));

        }

        public Task<Result<Guid>> DeleteAnalysisResultByDocumentIdAsync(Guid documentId)
        {
            var analysisResult = _context.AnalysisResults.FirstOrDefault(ar => ar.LegalDocumentId == documentId);
            if (analysisResult == null)
            {
                return Task.FromResult(Result<Guid>.Failure("Analysis result not found"));
            }
            _context.AnalysisResults.Remove(analysisResult);
            _context.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(documentId));
        }

    }
}