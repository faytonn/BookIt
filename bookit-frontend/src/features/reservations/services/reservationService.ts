import axios from 'axios';
import { CreateReservation, ReservationResponse } from '../types';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

class ReservationService {
  async createReservation(reservationData: CreateReservation): Promise<ReservationResponse> {
    const response = await axios.post(`${API_URL}/reservation`, reservationData, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return response.data;
  }

  async getReservation(id: number): Promise<ReservationResponse> {
    const response = await axios.get(`${API_URL}/reservation/${id}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return response.data;
  }

  async getUserReservations(): Promise<ReservationResponse[]> {
    const response = await axios.get(`${API_URL}/reservation/user`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return response.data;
  }

  async cancelReservation(id: number): Promise<void> {
    await axios.post(`${API_URL}/reservation/${id}/cancel`, null, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  }
}

export const reservationService = new ReservationService();
