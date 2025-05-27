using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using Infrastructure.Repositories;
using AngleSharp.Dom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.Extensions.Options;
using Google;


namespace Infrastructure.Repositories
{
    public class LLMResultRepository : ILLMResultRepository
    {

        private readonly LLMResultManagementDbContext _context;
        private readonly GoogleSettings _googleSettings;
        private readonly OllamaModel _ollama;
        private readonly DeepSeekSettings _deepSeekSettings;

        public LLMResultRepository(LLMResultManagementDbContext context, IOptions<DeepSeekSettings> deepSeekSettings, IOptions<GoogleSettings> googleSettings)
        {
            _context = context;
            _ollama = new OllamaModel(new HttpClient());
            _deepSeekSettings = deepSeekSettings.Value;
            _googleSettings = googleSettings.Value;
        }
        public async Task<Result<Guid>> AddLLMResultAsync(LLMResult llmResult)
        {
            await _context.LLMResults.AddAsync(llmResult);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(llmResult.Id);

        }
        public async Task<Result<LLMResult>> GetLLMResultByIdAsync(Guid id)
        {
            var llmResult = await _context.LLMResults.FindAsync(id);
            return llmResult != null
                ? Result<LLMResult>.Success(llmResult)
                : Result<LLMResult>.Failure("LLMResult not found");
        }
        public async Task<Result<IEnumerable<LLMResult>>> GetAllLLMResultsAsync()
        {
            var results = await _context.LLMResults.ToListAsync();
            return Result<IEnumerable<LLMResult>>.Success(results);
        }
        public async Task<Result<Guid>> UpdateLLMResultAsync(LLMResult llmResult)
        {
            _context.LLMResults.Update(llmResult);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(llmResult.Id);
        }
        public async Task<Result<Guid>> DeleteLLMResultAsync(Guid id)
        {
            var llmResult = await _context.LLMResults.FindAsync(id);
            if (llmResult == null)
            {
                return Result<Guid>.Failure("LLMResult not found");
            }
            _context.LLMResults.Remove(llmResult);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(id);
        }

        public async Task<string> AskAsync(string question, string filepath, string model)
        {
            string response;

            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var absolutePath = Path.Combine(uploadsDirectory, filepath ?? "");

            if (!System.IO.File.Exists(absolutePath))
            {
                return $"File not found at path: {absolutePath}";
            }

            string fileContent;

            try
            {
                var fileExtension = Path.GetExtension(absolutePath).ToLower();
                if (fileExtension == ".pdf")
                {
                    fileContent = ExtractTextFromPdf(absolutePath);
                }
                else if (fileExtension == ".docx")
                {
                    fileContent = ExtractTextFromDocx(absolutePath);
                }
                else
                {
                    return "Unsupported file type. Only PDF and DOCX files are supported.";
                }
            }
            catch (Exception ex)
            {
                return $"Error extracting text from file: {ex.Message}";
            }

            var _gemini = new GeminiApiClient(_googleSettings.ApiKey);

            var _deepSeek = new DeepSeekAnalyzer(_deepSeekSettings.ApiKey);

            switch (model.ToLower())
            {
                case "gemini":
                    response = await _gemini.AnalyzeTextAsync(fileContent, question);
                    break;

                case "deepseek":
                    response = await _deepSeek.AnalyzeTextAsync(fileContent, question);
                    break;
                case "tinyllama":
                    response = await _ollama.AnalyzeText(fileContent, question, model);
                    break;

                default:
                    throw new ArgumentException($"Model necunoscut: {model}");
            }

            return response;
        }

        private string ExtractTextFromPdf(string filePath)
        {
           
            using var pdfReader = new iText.Kernel.Pdf.PdfReader(filePath);
            using var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader);
            
            var text = new StringBuilder();

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                text.Append(iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i)));
            }

            return text.ToString();
        }

        private string ExtractTextFromDocx(string filePath)
        {
            using var wordDoc = WordprocessingDocument.Open(filePath, false);

            if (wordDoc.MainDocumentPart == null)
            {
                throw new InvalidOperationException("The Word document does not contain a MainDocumentPart.");
            }

            if (wordDoc.MainDocumentPart.Document == null)
            {
                throw new InvalidOperationException("The MainDocumentPart does not contain a Document.");
            }

            if (wordDoc.MainDocumentPart.Document.Body == null)
            {
                throw new InvalidOperationException("The Document does not contain a Body.");
            }

            return wordDoc.MainDocumentPart.Document.Body.InnerText;
        }
    }
    
}
