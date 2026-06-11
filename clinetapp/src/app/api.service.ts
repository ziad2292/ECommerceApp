import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, Order, Product, ShoppingCart, User } from './api.models';

const API_URL = 'http://localhost:5112/api';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  getProducts(): Observable<ApiResponse<Product[]>> {
    return this.http.get<ApiResponse<Product[]>>(`${API_URL}/Product/search`);
  }

  addToCart(productId: string, quantity: number): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${API_URL}/ShoppingCart/add-item`, {
      productId,
      quantity
    });
  }

  getCart(): Observable<ApiResponse<ShoppingCart>> {
    return this.http.get<ApiResponse<ShoppingCart>>(`${API_URL}/ShoppingCart/get-cart`);
  }

  updateCartItem(itemId: string, quantity: number): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(
      `${API_URL}/ShoppingCart/update-quantity?itemId=${itemId}&quantity=${quantity}`,
      null
    );
  }

  deleteCartItem(itemId: string): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${API_URL}/ShoppingCart/delete-item?itemId=${itemId}`);
  }

  emptyCart(): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${API_URL}/ShoppingCart/empty-cart`);
  }

  getCurrentUser(): Observable<ApiResponse<User>> {
    return this.http.get<ApiResponse<User>>(`${API_URL}/Account/get-current-user`);
  }

  getUserOrders(userId: string): Observable<ApiResponse<Order[]>> {
    return this.http.get<ApiResponse<Order[]>>(`${API_URL}/Order/get-user-orders?userId=${userId}`);
  }

  placeCashOrder(): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${API_URL}/Order/add-order?paymentMethod=1`, null);
  }
}
