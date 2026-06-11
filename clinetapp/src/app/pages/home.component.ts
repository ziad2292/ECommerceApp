import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  template: `
    <h1>Home</h1>

    <div class="card">
      <p>This small Angular app is only for checking the demo endpoints.</p>
      <ol>
        <li>Open <a routerLink="/products">Products</a>.</li>
        <li>Add one product to the cart.</li>
        <li>Open <a routerLink="/cart">Shopping Cart</a> to check it.</li>
        <li>Open <a routerLink="/orders">Orders</a> and place a cash order.</li>
      </ol>
    </div>
  `
})
export class HomeComponent {}
