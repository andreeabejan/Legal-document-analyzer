using Amazon.Runtime;
using Domain.Entities;
using Domain.Interfaces;
using System.Net.Http.Json;

namespace Application.Repositories
{
    public class LLMResultService
    {

        private readonly ILLMResultRepository _llmResultRepository;
        private readonly IOllamaModel _ollamaModel;
        public LLMResultService(ILLMResultRepository llmResultRepository, IOllamaModel ollamaModel)
        {
            _llmResultRepository = llmResultRepository;
            _ollamaModel = ollamaModel;
        }

        public async Task<LLMResult> GetLLMResultByIdAsync(Guid id)
        {
            var result = await _llmResultRepository.GetLLMResultByIdAsync(id);
            if (result.IsSuccess)
            {
                return result.Data; 
            }
            throw new Exception(result.ErrorMessage); 
        }

        public async Task<string> AskAsync(string question, string context, string model)
        {
            var result = await _ollamaModel.AnalyzeText(question, context, model);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            throw new Exception("The result is null or empty.");
        }

        public async Task<IEnumerable<LLMResult>> GetAllLLMResultsAsync()
        {
            var result = await _llmResultRepository.GetAllLLMResultsAsync();
            if (result.IsSuccess)
            {
                return result.Data; 
            }
            throw new Exception(result.ErrorMessage); 
        }

        public async Task AddLLMResultAsync(LLMResult llmResult)
        {
            var result = await _llmResultRepository.AddLLMResultAsync(llmResult);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage); 
            }
        }

        public async Task UpdateLLMResultAsync(LLMResult llmResult)
        {
            var result = await _llmResultRepository.UpdateLLMResultAsync(llmResult);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage); 
            }
        }

        public async Task DeleteLLMResultAsync(Guid id)
        {
            var result = await _llmResultRepository.DeleteLLMResultAsync(id);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage); 
            }
        }
    }
}
