
import { Component }           from '@angular/core';
import { DocumentService }     from './document.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import e from 'express';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls:   ['./app.component.css'],
  providers:   [DocumentService],
  imports:     [CommonModule, FormsModule, HttpClientModule]
})


export class AppComponent {
  legalDocumentId = crypto.randomUUID();
  analysisResultId = ''; 
  responses = [
  { 
    model: 'Model 1 - Tinyllama', 
    initialAnalysis: '', 
    query: '', 
    followUps: [] as { question: string, answer: string }[] 
  },
  { 
    model: 'Model 2 - DeepSeek', 
    initialAnalysis: '', 
    query: '', 
    followUps: [] as { question: string, answer: string }[] 
  },
  { 
    model: 'Model 3 - Gemini flash 2.0', 
    initialAnalysis: '', 
    query: '', 
    followUps: [] as { question: string, answer: string }[] 
  },
];


  constructor(private docService: DocumentService) {}

  upload(type: 'pdf' | 'doc') {
    const input = document.createElement('input');
    input.type   = 'file';
    input.accept = type === 'pdf' ? '.pdf' : '.doc,.docx';
    input.click();
    input.onchange = (e: any) => {
      const file: File = e.target.files[0];
      if (file) this.send(file);
    };
  }

  private send(file: File) {
    this.docService.uploadDocument(file) 
      .subscribe({
        next: resp => {
          console.log(resp);

          const text = resp.analysisResult?.ollamaAnalysisResultText || 'Analiză primită';
          const textGemini = resp.analysisResult?.gemmaAnalysisResultText || 'Analiză primită';
          const textDeepSeek = resp.analysisResult?.phiAnalysisResultText || 'Analiză primită';

          this.responses[0].initialAnalysis = text;
          this.responses[1].initialAnalysis = textDeepSeek;
          this.responses[2].initialAnalysis = textGemini;

          this.analysisResultId = resp.analysisResult?.id || '';

        },
        error: err => {
          console.error(err);
          alert('Eroare la încărcare');
        }
      });
  }
  getDisplayText(i: number, r: any): string {
    if (i === 0) return r.analysis;
    if (i === 1) return r.analysis;
    if (i === 2) return r.analysis;
    return '— analiza va apărea aici —';
  }
  handleQuery(index: number, query: string) {
    if (index === 0) {
      this.askOllama(query);
    } else if (index === 1) {
      this.askDeepSeek(query);
    }
    else {
      this.askGemini( query);
    }
    this.responses[index].query = ''; 
  }
  
  askOllama(query: string) {
  this.docService.askFollowUp('tinyllama', this.analysisResultId, query)
  .subscribe({
    next: (resp) => {
      this.responses[0].followUps.push({
        question: query,
        answer: resp.answer
      });
    },
    error: err => {
      console.error(err);
      alert('Eroare la întrebarea suplimentară pentru Ollama');
    }
  });
}

askDeepSeek(query: string) {
  this.docService.askFollowUp('deepseek', this.analysisResultId, query)
  .subscribe({
    next: (resp) => {
      this.responses[1].followUps.push({
        question: query,
        answer: resp.answer
      });
    },
    error: err => {
      console.error(err);
      alert('Eroare la întrebarea suplimentară pentru DeepSeek');
    }
  });
}

askGemini(query: string) {
  this.docService.askFollowUp('gemini', this.analysisResultId, query)
  .subscribe({
    next: (resp) => {
      this.responses[2].followUps.push({
        question: query,
        answer: resp.answer
      });
    },
    error: err => {
      console.error(err);
      alert('Eroare la întrebarea suplimentară pentru Gemini');
    }
  });
}

  
}
