/**
 * Tipos de usuário do sistema
 */
export enum UserType {
  Customer = 'Customer',
  Vendor = 'Vendor',
  Admin = 'Admin'
}

/**
 * Interface do usuário
 */
export interface User {
  id: string;
  email: string;
  userType: UserType;
  name?: string;
  isActive: boolean;
  createdAt: Date;
}

/**
 * DTO de Login Request
 */
export interface LoginRequest {
  email: string;
  password: string;
}

/**
 * DTO de Login Response
 */
export interface LoginResponse {
  token: string;
  user: User;
  expiresAt: Date;
}

/**
 * DTO de Register Request
 */
export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  userType: UserType;
  name?: string;
}
