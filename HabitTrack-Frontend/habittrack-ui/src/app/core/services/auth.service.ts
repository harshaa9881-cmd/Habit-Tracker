import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import {
  LoginRequest,
  RegisterRequest,
  AuthResponse
} from '../../models/auth';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      data
    ).pipe(
      tap(response => {
        localStorage.setItem('habitTrackToken', response.token);
        localStorage.setItem('habitTrackUser', response.fullName);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('habitTrackToken');
    localStorage.removeItem('habitTrackUser');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('habitTrackToken');
  }
}