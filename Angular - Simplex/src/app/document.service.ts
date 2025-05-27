import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  private apiUrl = 'https://localhost:7263/api/v1/AnalysisResults/upload'; 
  private baseUrl = 'https://localhost:7263/api/v1/AnalysisResults';
  private baseUrlLLM = 'https://localhost:7263/api/v1/LLMResults';

  constructor(private http: HttpClient) {}

  uploadDocument(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const headers = new HttpHeaders();

    return this.http.post<any>(this.apiUrl, formData, { headers });
  }

  askFollowUp(modelName: string, analysisResultId: string, question: string): Observable<any> {
    const payload = {
      model: modelName,
      analysisResultId: analysisResultId,
      question: question
    };
  
    return this.http.post<any>(`${this.baseUrlLLM}/follow-up`, payload);
  }
  

}
