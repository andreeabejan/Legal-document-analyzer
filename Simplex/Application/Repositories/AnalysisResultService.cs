using DocumentFormat.OpenXml.Packaging;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using System.Text;


namespace Application.Repositories
{
    public class AnalysisResultService
    {
        private readonly IAnalysisResultRepository _analysisResultRepository;
        private readonly IOllamaModel _ollamaModel;
        private readonly DeepSeekSettings _deepSeekSettings;
        private readonly GoogleSettings _googleSettings;

        public AnalysisResultService(IAnalysisResultRepository analysisResultRepository, IOllamaModel ollamaModel, IOptions<DeepSeekSettings> deepSeekSettings,IOptions<GoogleSettings> googleSettings)
        {
            _analysisResultRepository = analysisResultRepository;
            _ollamaModel = ollamaModel;
            _deepSeekSettings = deepSeekSettings.Value;
            _googleSettings = googleSettings.Value;
        }

        public async Task<Result<Guid>> AddAnalysisResultAsync(AnalysisResult analysisResult)
        {
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var absolutePath = Path.Combine(uploadsDirectory, analysisResult.FilePath ?? "");

            if (!System.IO.File.Exists(absolutePath))
            {
                return Result<Guid>.Failure($"File not found at path: {absolutePath}");
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
                    return Result<Guid>.Failure("Unsupported file type. Only PDF and DOCX files are supported.");
                }
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Error extracting text from file: {ex.Message}");
            }

            var prompt = @"
                Analyze the following legal document. Provide:
                1. A concise summary of the document.
                2. Key legal insights, including:
                   - Important clauses.
                   - Obligations of the parties.
                   - Deadlines or timelines.
                   - Any risks or ambiguities.
            ";

            var OllamaAnalyzedText = await _ollamaModel.AnalyzeText(fileContent, prompt, "tinyllama");

            analysisResult.OllamaAnalysisResultText = OllamaAnalyzedText;

            var gemini = new GeminiApiClient(_googleSettings.ApiKey);
            analysisResult.GemmaAnalysisResultText = await gemini.AnalyzeTextAsync(prompt, fileContent);


            var deepSeek = new DeepSeekAnalyzer(_deepSeekSettings.ApiKey);
            analysisResult.PhiAnalysisResultText = await deepSeek.AnalyzeTextAsync(fileContent, prompt);


            return await _analysisResultRepository.AddAnalysisResultAsync(analysisResult);
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

        public async Task<Result<AnalysisResult>> GetAnalysisResultByIdAsync(Guid id)
        {
            return await _analysisResultRepository.GetAnalysisResultByIdAsync(id);
        }

        public async Task<Result<Guid>> DeleteAnalysisResultAsync(Guid id)
        {
            return await _analysisResultRepository.DeleteAnalysisResultAsync(id);
        }

        public async Task<Result<IEnumerable<AnalysisResult>>> GetAllAnalysisResultsAsync()
        {
            return await _analysisResultRepository.GetAllAnalysisResultsAsync();
        }
    }
}