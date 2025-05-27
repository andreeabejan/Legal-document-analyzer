using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITextAnalyzer
    {
        Task<string> AnalyzeTextAsync(string content, string prompt);
        string SourceName { get; }
    }

}
