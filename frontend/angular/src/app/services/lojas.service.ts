import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ListarLojasResponse, Loja } from './listar-lojas-response';
import { ListarTransacoesRequest } from './listar-transacoes-request';
import { ListarTransacoesResponse } from './listar-transacoes-response';

@Injectable({
  providedIn: 'root'
})
export class LojasService {

  private apiURL = "http://localhost:5000/api/loja";

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private httpClient: HttpClient) { }

  listarLojas(): Observable<ListarLojasResponse> {
    return this.httpClient.get<ListarLojasResponse>(this.apiURL)
      .pipe(
        catchError(this.errorHandler)
      )
  }

  listarTransacoes(request: ListarTransacoesRequest): Observable<ListarTransacoesResponse> {
    return this.httpClient.post<ListarTransacoesResponse>(this.apiURL + '/listar_transacoes/', JSON.stringify(request), this.httpOptions)
      .pipe(
        catchError(this.errorHandler)
      )
  }

  // find(id): Observable<Post> {
  //   return this.httpClient.get<Post>(this.apiURL + '/posts/' + id)
  //     .pipe(
  //       catchError(this.errorHandler)
  //     )
  // }

  // update(id, post): Observable<Post> {
  //   return this.httpClient.put<Post>(this.apiURL + '/posts/' + id, JSON.stringify(post), this.httpOptions)
  //     .pipe(
  //       catchError(this.errorHandler)
  //     )
  // }

  // delete(id) {
  //   return this.httpClient.delete<Post>(this.apiURL + '/posts/' + id, this.httpOptions)
  //     .pipe(
  //       catchError(this.errorHandler)
  //     )
  // }


  errorHandler(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(errorMessage);
  }
}
