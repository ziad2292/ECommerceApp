export interface ApiResponse<T = unknown> {
  isSuccess: boolean;
  message: string;
  data?: T;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  refreshTokenExpirationDateTime: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber: string;
  gender: 'Male' | 'Female';
  birthDate: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  imageUrl?: string;
  categoryId: number;
  stock: number;
}

export interface CartItem {
  id: string;
  productId: string;
  shoppingCartId: string;
  quantity: number;
}

export interface ShoppingCart {
  id: string;
  userId: string;
  items?: CartItem[];
}

export interface User {
  id: string;
  name: string;
  email: string;
  phoneNumber: string;
  birthDate: string;
  gender: string;
}

export interface Order {
  id: string;
  userId: string;
  paymentId: string;
  orderStatus: string;
  totoalAmount: number;
  date: string;
}
