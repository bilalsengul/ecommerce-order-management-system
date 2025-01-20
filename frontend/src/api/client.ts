import axios from 'axios';
import { CreateOrderDto, Order, OrderFilter } from './types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const ordersApi = {
  createOrder: async (orderData: CreateOrderDto): Promise<Order> => {
    const response = await apiClient.post<Order>('/api/orders', orderData);
    return response.data;
  },

  getOrders: async (filters?: OrderFilter): Promise<Order[]> => {
    const response = await apiClient.get<Order[]>('/api/orders', {
      params: filters,
    });
    return response.data;
  },

  getOrder: async (orderNumber: string): Promise<Order> => {
    const response = await apiClient.get<Order>(`/api/orders/${orderNumber}`);
    return response.data;
  },

  cancelOrder: async (orderNumber: string): Promise<Order> => {
    const response = await apiClient.post<Order>(`/api/orders/${orderNumber}/cancel`);
    return response.data;
  },
};

export default apiClient; 