import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../api.service';
import { Product } from '../api.models';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h1>Products</h1>

    <button type="button" class="secondary" (click)="loadProducts()">Reload products</button>

    @if (message) {
      <p class="message" [class.error]="isError">{{ message }}</p>
    }

    <div class="list">
      @for (product of products; track product.id) {
        <div class="card">
          <h3>{{ product.name }}</h3>
          <p>{{ product.description }}</p>
          <p>Price: {{ product.price }}</p>
          <p>Stock: {{ product.stock }}</p>
          <p>Product id: {{ product.id }}</p>

          <label>
            Quantity
            <input type="number" min="1" [max]="product.stock" [(ngModel)]="quantities[product.id]">
          </label>

          <button type="button" (click)="addToCart(product)">
            Add to cart
          </button>
        </div>
      }
    </div>
  `
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  quantities: Record<string, number> = {};
  message = '';
  isError = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.message = 'Loading products...';
    this.isError = false;

    this.api.getProducts().subscribe({
      next: response => {
        this.products = response.data ?? [];
        for (const product of this.products) {
          this.quantities[product.id] = this.quantities[product.id] ?? 1;
        }
        this.message = response.message;
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not load products';
      }
    });
  }

  addToCart(product: Product): void {
    const quantity = this.quantities[product.id] || 1;
    this.message = 'Adding item...';
    this.isError = false;

    this.api.addToCart(product.id, quantity).subscribe({
      next: response => {
        this.message = response.message;
        this.isError = !response.isSuccess;
      },
      error: error => {
        this.isError = true;
        this.message = error.error?.message ?? 'Could not add item';
      }
    });
  }
}
