import axios from 'axios';
import { CreateOrderDto, Order, OrderFilter } from './types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8080';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

interface ApiResponse<T> {
  success: boolean;
  message: string | null;
  data: T;
  errors: string[];
}

export const ordersApi = {
  createOrder: async (orderData: CreateOrderDto): Promise<Order> => {
    const response = await apiClient.post<ApiResponse<Order>>('/api/orders', orderData);
    return response.data.data;
  },

  getOrders: async (filters?: OrderFilter): Promise<Order[]> => {
    const response = await apiClient.get<ApiResponse<Order[]>>('/api/orders', {
      params: filters,
    });
    return response.data.data;
  },

  getOrder: async (orderId: string): Promise<Order> => {
    const response = await apiClient.get<ApiResponse<Order>>(`/api/orders/${orderId}`);
    return response.data.data;
  },

  cancelOrder: async (orderId: string): Promise<Order> => {
    const response = await apiClient.post<ApiResponse<Order>>(`/api/orders/${orderId}/cancel`);
    return response.data.data;
  },
};

export default apiClient; 