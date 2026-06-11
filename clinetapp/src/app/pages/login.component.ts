import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <h1>Login</h1>

    <form class="form" (ngSubmit)="login()">
      <label>
        Email
        <input name="email" type="email" [(ngModel)]="email" required>
      </label>

      <label>
        Password
        <input name="password" type="password" [(ngModel)]="password" required>
      </label>

      <button type="submit">Login</button>
      <a routerLink="/register">Create a new account</a>
    </form>

    @if (message) {
      <p class="message" [class.error]="isError">{{ message }}</p>
    }
  `
})
export class LoginComponent {
  email = '';
  password = '';
  message = '';
  isError = false;

  constructor(private auth: AuthService, private router: Router) {}

  login(): void {
    this.message = 'Logging in...';
    this.isError = false;

    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: response => {
        this.message = response.message;
        if (response.isSuccess) {
          this.router.navigateByUrl('/home');
        } else {
          this.isError = true;
        }
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Login failed';
      }
    });
  }
}
