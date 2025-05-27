using Domain.Entities;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Simplex.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AnalysisResultsController : ControllerBase
    {
        private readonly AnalysisResultService _analysisResultService;

        public AnalysisResultsController(AnalysisResultService analysisResultService)
        {
            _analysisResultService = analysisResultService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnalysisResults()
        {
            var result = await _analysisResultService.GetAllAnalysisResultsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnalysisResultById(Guid id)
        {
            var result = await _analysisResultService.GetAnalysisResultByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }
        

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] Guid legalDocumentId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsDirectory))
                Directory.CreateDirectory(uploadsDirectory);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsDirectory, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var analysisResult = new AnalysisResult
            {
                Id = Guid.NewGuid(),
                LegalDocumentId = legalDocumentId,      
                Name = file.FileName,
                Description = $"Uploaded on {DateTime.UtcNow:O}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FilePath = fileName,           
                OllamaAnalysisResultText = string.Empty,      
                PhiAnalysisResultText = string.Empty,
                GemmaAnalysisResultText = string.Empty
            };

            var addResult = await _analysisResultService.AddAnalysisResultAsync(analysisResult);
            if (!addResult.IsSuccess)
                return BadRequest(addResult.ErrorMessage);

            var getResult = await _analysisResultService.GetAnalysisResultByIdAsync(addResult.Data);
            if (!getResult.IsSuccess)
                return NotFound(getResult.ErrorMessage);

            return Ok(new
            {
                message = "File uploaded and analyzed successfully",
                analysisResult = getResult.Data
            });

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalysisResult(Guid id)
        {
            var result = await _analysisResultService.DeleteAnalysisResultAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("byDocumentId/{id}")]
        public async Task<IActionResult> GetAnalysisResultByDocumentIdAsync(Guid id)
        {
            var result = await _analysisResultService.GetAnalysisResultByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("byDocumentId/{id}")]
        public async Task<IActionResult> DeleteAnalysisResultByDocumentIdAsync(Guid id)
        {
            var result = await _analysisResultService.DeleteAnalysisResultAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
