
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UploadResponse } from './upload-response';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  apiURL = "http://localhost:5000/api/loja/upload";

  constructor(private http: HttpClient) { }

  upload(file: File): Observable<UploadResponse> {
    const formData = new FormData();
    formData.append("file", file, file.name);
    return this.http.post<UploadResponse>(this.apiURL, formData)
  }
}
