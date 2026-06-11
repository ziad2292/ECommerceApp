import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../api.service';
import { CartItem, ShoppingCart } from '../api.models';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h1>Shopping Cart</h1>

    <button type="button" class="secondary" (click)="loadCart()">Reload cart</button>
    <button type="button" class="secondary" (click)="emptyCart()">Empty cart</button>

    @if (message) {
      <p class="message" [class.error]="isError">{{ message }}</p>
    }

    @if (cart) {
      <p>Cart id: {{ cart.id }}</p>
    }

    @if (items.length === 0) {
      <p>No cart items.</p>
    } @else {
      <table>
        <thead>
          <tr>
            <th>Item id</th>
            <th>Product id</th>
            <th>Quantity</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          @for (item of items; track item.id) {
            <tr>
              <td>{{ item.id }}</td>
              <td>{{ item.productId }}</td>
              <td>
                <input type="number" min="1" [(ngModel)]="quantities[item.id]">
              </td>
              <td>
                <button type="button" (click)="updateItem(item)">Update</button>
                <button type="button" class="secondary" (click)="deleteItem(item)">Delete</button>
              </td>
            </tr>
          }
        </tbody>
      </table>
    }
  `
})
export class CartComponent implements OnInit {
  cart: ShoppingCart | null = null;
  items: CartItem[] = [];
  quantities: Record<string, number> = {};
  message = '';
  isError = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.message = 'Loading cart...';
    this.isError = false;

    this.api.getCart().subscribe({
      next: response => {
        this.cart = response.data ?? null;
        this.items = this.cart?.items ?? [];
        for (const item of this.items) {
          this.quantities[item.id] = item.quantity;
        }
        this.message = response.message;
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not load cart';
      }
    });
  }

  updateItem(item: CartItem): void {
    const quantity = this.quantities[item.id] || item.quantity;
    this.message = 'Updating item...';
    this.isError = false;

    this.api.updateCartItem(item.id, quantity).subscribe({
      next: response => {
        this.message = response.message;
        this.isError = !response.isSuccess;
        this.loadCart();
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not update item';
      }
    });
  }

  deleteItem(item: CartItem): void {
    this.message = 'Deleting item...';
    this.isError = false;

    this.api.deleteCartItem(item.id).subscribe({
      next: response => {
        this.message = response.message;
        this.isError = !response.isSuccess;
        this.loadCart();
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not delete item';
      }
    });
  }

  emptyCart(): void {
    this.message = 'Emptying cart...';
    this.isError = false;

    this.api.emptyCart().subscribe({
      next: response => {
        this.message = response.message;
        this.isError = !response.isSuccess;
        this.loadCart();
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not empty cart';
      }
    });
  }
}
