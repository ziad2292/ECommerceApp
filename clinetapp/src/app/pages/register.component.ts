import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <h1>Register</h1>

    <form class="form" (ngSubmit)="register()">
      <label>
        Username
        <input name="userName" [(ngModel)]="userName" required>
      </label>

      <label>
        Email
        <input name="email" type="email" [(ngModel)]="email" required>
      </label>

      <label>
        Phone
        <input name="phoneNumber" [(ngModel)]="phoneNumber" required>
      </label>

      <label>
        Gender
        <select name="gender" [(ngModel)]="gender">
          <option value="Male">Male</option>
          <option value="Female">Female</option>
        </select>
      </label>

      <label>
        Birth date
        <input name="birthDate" type="date" [(ngModel)]="birthDate" required>
      </label>

      <label>
        Password
        <input name="password" type="password" [(ngModel)]="password" required>
      </label>

      <label>
        Confirm password
        <input name="confirmPassword" type="password" [(ngModel)]="confirmPassword" required>
      </label>

      <button type="submit">Register</button>
      <a routerLink="/login">Already have an account?</a>
    </form>

    @if (message) {
      <p class="message" [class.error]="isError">{{ message }}</p>
    }
  `
})
export class RegisterComponent {
  userName = '';
  email = '';
  phoneNumber = '';
  gender: 'Male' | 'Female' = 'Male';
  birthDate = '2000-01-01';
  password = '';
  confirmPassword = '';
  message = '';
  isError = false;

  constructor(private auth: AuthService) {}

  register(): void {
    this.message = 'Registering...';
    this.isError = false;

    this.auth.register({
      userName: this.userName,
      email: this.email,
      phoneNumber: this.phoneNumber,
      gender: this.gender,
      birthDate: this.birthDate,
      password: this.password,
      confirmPassword: this.confirmPassword
    }).subscribe({
      next: response => {
        this.message = `${response.message}. You can login now.`;
        this.isError = !response.isSuccess;
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Registration failed';
      }
    });
  }
}
