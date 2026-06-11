import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { ApiResponse, AuthResponse, LoginRequest, RegisterRequest } from './api.models';

const API_URL = 'http://localhost:5112/api';
const TOKEN_KEY = 'accessToken';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  register(request: RegisterRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${API_URL}/Auth/register-user`, request);
  }

  login(request: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${API_URL}/Auth/login`, request).pipe(
      tap(response => {
        if (response.isSuccess && response.data?.accessToken) {
          localStorage.setItem(TOKEN_KEY, response.data.accessToken);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
