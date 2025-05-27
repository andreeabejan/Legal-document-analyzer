using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILLMResultRepository
    {
        Task<Result<Guid>> AddLLMResultAsync(LLMResult llmResult);

        Task<string> AskAsync(string question, string context, string model);
        Task<Result<LLMResult>> GetLLMResultByIdAsync(Guid id);
        Task<Result<Guid>> UpdateLLMResultAsync(LLMResult llmResult);
        Task<Result<Guid>> DeleteLLMResultAsync(Guid id);
        Task<Result<IEnumerable<LLMResult>>> GetAllLLMResultsAsync();
    }
}
