import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { Order, User } from '../api.models';

@Component({
  selector: 'app-orders',
  standalone: true,
  template: `
    <h1>Orders</h1>

    <button type="button" class="secondary" (click)="loadOrders()">Reload orders</button>
    <button type="button" (click)="placeCashOrder()">Place cash order</button>

    @if (message) {
      <p class="message" [class.error]="isError">{{ message }}</p>
    }

    @if (currentUser) {
      <p>User: {{ currentUser.email }}</p>
    }

    @if (orders.length === 0) {
      <p>No orders yet.</p>
    } @else {
      <table>
        <thead>
          <tr>
            <th>Order id</th>
            <th>Date</th>
            <th>Status</th>
            <th>Total</th>
            <th>Payment id</th>
          </tr>
        </thead>
        <tbody>
          @for (order of orders; track order.id) {
            <tr>
              <td>{{ order.id }}</td>
              <td>{{ order.date }}</td>
              <td>{{ order.orderStatus }}</td>
              <td>{{ order.totoalAmount }}</td>
              <td>{{ order.paymentId }}</td>
            </tr>
          }
        </tbody>
      </table>
    }
  `
})
export class OrdersComponent implements OnInit {
  currentUser: User | null = null;
  orders: Order[] = [];
  message = '';
  isError = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.message = 'Loading orders...';
    this.isError = false;

    this.api.getCurrentUser().subscribe({
      next: userResponse => {
        this.currentUser = userResponse.data ?? null;
        if (!this.currentUser) {
          this.isError = true;
          this.message = 'Could not find current user';
          return;
        }

        this.api.getUserOrders(this.currentUser.id).subscribe({
          next: ordersResponse => {
            this.orders = ordersResponse.data ?? [];
            this.message = ordersResponse.message;
          },
          error: error => {
            this.isError = true;
            this.message = error.error?.message ?? 'Could not load orders';
          }
        });
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not load current user';
      }
    });
  }

  placeCashOrder(): void {
    this.message = 'Placing cash order...';
    this.isError = false;

    this.api.placeCashOrder().subscribe({
      next: response => {
        this.message = response.message;
        this.isError = !response.isSuccess;
        this.loadOrders();
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not place order';
      }
    });
  }
}
