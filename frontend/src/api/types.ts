export interface Order {
  id: string;
  orderNumber: string;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;
  userId: string;
  userName: string;
  orderItems: OrderItem[];
}

export interface OrderItem {
  id: string;
  productId: string;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export enum OrderStatus {
  Created = 'Created',
  Cancelled = 'Cancelled'
}

export interface CreateOrderDto {
  userId: string;
  orderItems: CreateOrderItemDto[];
}

export interface CreateOrderItemDto {
  productId: string;
  quantity: number;
}

export interface OrderFilter {
  startDate?: string;
  endDate?: string;
  minAmount?: number;
  maxAmount?: number;
  status?: OrderStatus;
} 