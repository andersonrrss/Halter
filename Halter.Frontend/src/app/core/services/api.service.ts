import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient)
  private baseUrl = environment.apiUrl

  get<T>(url:string, 
    params?: HttpParams | Record<string, string | number | boolean | ReadonlyArray<string | number | boolean>>
  ){
    return this.http.get<T>(`${this.baseUrl}${url}`, { params })
  }
  
  post<T>(url:string, body:unknown){
    return this.http.post<T>(`${this.baseUrl}${url}`, body)
  }

  put<T>(url:string, body:unknown){
    return this.http.put<T>(`${this.baseUrl}${url}`, body)
  }

  delete<T>(url:string){
    return this.http.delete<T>(`${this.baseUrl}${url}`)
  }
}
