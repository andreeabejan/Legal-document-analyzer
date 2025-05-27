using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Simplex.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LegalDocumentsController : ControllerBase
    {
        private readonly ILegalDocumentRepository _legalDocumentRepository;
        public LegalDocumentsController(ILegalDocumentRepository legalDocumentRepository)
        {
            _legalDocumentRepository = legalDocumentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLegalDocuments()
        {
            var result = await _legalDocumentRepository.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLegalDocumentById(Guid id)
        {
            var result = await _legalDocumentRepository.GetLegalDocumentByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpPost]
        public async Task<IActionResult> AddLegalDocument([FromBody] LegalDocument legalDocument)
        {
            var result = await _legalDocumentRepository.AddLegalDocumentAsync(legalDocument);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetLegalDocumentById), new { id = result.Data }, legalDocument);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLegalDocument(Guid id, [FromBody] LegalDocument legalDocument)
        {
            if (id != legalDocument.Id)
            {
                return BadRequest("ID mismatch");
            }
            var result = await _legalDocumentRepository.UpdateLegalDocumentAsync(legalDocument);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLegalDocument(Guid id)
        {
            var result = await _legalDocumentRepository.DeleteLegalDocumentAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLegalDocumentsByUserId(Guid userId)
        {
            var result = await _legalDocumentRepository.GetLegalDocumentsByUserIdAsync(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteLegalDocumentsByUserId(Guid userId)
        {
            var result = await _legalDocumentRepository.DeleteLegalDocumentsByUserIdAsync(userId);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

    }
}
