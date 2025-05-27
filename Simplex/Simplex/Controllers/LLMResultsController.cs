using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Simplex.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LLMResultsController : ControllerBase
    {
        private readonly ILLMResultRepository _llmResultRepository;
        private readonly IAnalysisResultRepository _analysisResultRepository;
        private readonly ILLMResultRepository _llmService;

        public LLMResultsController(
            ILLMResultRepository llmResultRepository,
            IAnalysisResultRepository analysisResultRepository,
            ILLMResultRepository llmService)
        {
            _llmResultRepository = llmResultRepository;
            _analysisResultRepository = analysisResultRepository;
            _llmService = llmService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLLMResults()
        {
            var result = await _llmResultRepository.GetAllLLMResultsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLLMResultById(Guid id)
        {
            var result = await _llmResultRepository.GetLLMResultByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpPost]
        public async Task<IActionResult> AddLLMResult([FromBody] LLMResult llmResult)
        {
            var result = await _llmResultRepository.AddLLMResultAsync(llmResult);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetLLMResultById), new { id = result.Data }, llmResult);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLLMResult(Guid id, [FromBody] LLMResult llmResult)
        {
            if (id != llmResult.Id)
            {
                return BadRequest("ID mismatch");
            }
            var result = await _llmResultRepository.UpdateLLMResultAsync(llmResult);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpPost("follow-up")]
        public async Task<IActionResult> AskFollowUp([FromBody] JsonElement body)
        {
            if (!body.TryGetProperty("model", out var modelProp) ||
                !body.TryGetProperty("analysisResultId", out var analysisIdProp) ||
                !body.TryGetProperty("question", out var questionProp))
            {
                return BadRequest("campuri lipsa");
            }

            string model = modelProp.GetString()!;
            string question = questionProp.GetString()!;
            Guid analysisId = Guid.Parse(analysisIdProp.GetString()!);

            var analysisResult = await _analysisResultRepository.GetAnalysisResultByIdAsync(analysisId);
            if (!analysisResult.IsSuccess || analysisResult.Data == null)
                return NotFound("Analiza nu a fost găsită.");

            var analysis = analysisResult.Data;

            string baseText = model.ToLower() switch
            {
                "tinyllama" => analysis.OllamaAnalysisResultText,
                "deepseek" => analysis.PhiAnalysisResultText,
                "gemini" => analysis.GemmaAnalysisResultText,
                _ => null
            };

            if (string.IsNullOrWhiteSpace(baseText))
                return BadRequest("Model invalid sau analiza lipsește.");

            string fullPrompt = $"{baseText}\n\nÎntrebare suplimentară: {question}";
            string response = await _llmService.AskAsync(fullPrompt, analysis.FilePath, model.ToLower());

            var newLLMResult = new LLMResult
            {
                Id = Guid.NewGuid(),
                Name = "Întrebare suplimentară",
                Description = $"Întrebare: {question}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AnalysisResultId = analysis.Id,
                LLMResultText = response,
                ModelUsed = model,
                PromptUsed = fullPrompt
            };

            return Ok(new
            {
                answer = response,
                llmResultId = newLLMResult.Id
            });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLLMResult(Guid id)
        {
            var result = await _llmResultRepository.DeleteLLMResultAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }
    }

}
