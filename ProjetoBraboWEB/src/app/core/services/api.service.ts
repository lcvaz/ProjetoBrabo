import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

/**
 * Serviço base para chamadas de API
 *
 * Centraliza a lógica de comunicação com o backend
 * Usa as configurações de environment para definir a URL base
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  /**
   * URL base da API vinda do environment
   * Development: http://localhost:5000/api
   * Production: https://api.marketplace.com/api
   */
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
    // Log apenas em development
    if (environment.logging.enableHttpLogging) {
      console.log('ApiService initialized with URL:', this.apiUrl);
    }
  }

  /**
   * GET request
   * @param endpoint - Endpoint da API (ex: 'products', 'users/123')
   * @param params - Query parameters opcionais
   */
  get<T>(endpoint: string, params?: HttpParams): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;

    if (environment.logging.enableHttpLogging) {
      console.log(`[GET] ${url}`, params);
    }

    return this.http.get<T>(url, { params });
  }

  /**
   * POST request
   * @param endpoint - Endpoint da API
   * @param body - Corpo da requisição
   */
  post<T>(endpoint: string, body: any): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;

    if (environment.logging.enableHttpLogging) {
      console.log(`[POST] ${url}`, body);
    }

    return this.http.post<T>(url, body);
  }

  /**
   * PUT request
   * @param endpoint - Endpoint da API
   * @param body - Corpo da requisição
   */
  put<T>(endpoint: string, body: any): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;

    if (environment.logging.enableHttpLogging) {
      console.log(`[PUT] ${url}`, body);
    }

    return this.http.put<T>(url, body);
  }

  /**
   * DELETE request
   * @param endpoint - Endpoint da API
   */
  delete<T>(endpoint: string): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;

    if (environment.logging.enableHttpLogging) {
      console.log(`[DELETE] ${url}`);
    }

    return this.http.delete<T>(url);
  }

  /**
   * Upload de arquivo (multipart/form-data)
   * @param endpoint - Endpoint da API
   * @param formData - FormData com o arquivo
   */
  upload<T>(endpoint: string, formData: FormData): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;

    if (environment.logging.enableHttpLogging) {
      console.log(`[UPLOAD] ${url}`);
    }

    return this.http.post<T>(url, formData);
  }

  /**
   * Retorna a URL completa para um arquivo no storage
   * @param path - Path relativo do arquivo (ex: 'products/image.jpg')
   */
  getStorageUrl(path: string): string {
    return `${environment.storageUrl}/${path}`;
  }

  /**
   * Retorna configurações úteis do environment
   */
  getConfig() {
    return {
      isProduction: environment.production,
      apiUrl: environment.apiUrl,
      storageUrl: environment.storageUrl,
      appName: environment.appName,
      version: environment.version,
    };
  }
}
