import axios from 'axios';
import { DashboardData } from '../types';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

interface Event {
  id: number;
  title: string;
  description: string;
  type: string;
  startDate: string;
  endDate: string;
  locationName: string;
  locationAddress: string;
  locationCity: string;
  locationCountry: string;
  imageUrl: string;
  pricing: {
    id: number;
    type: string;
    price: number;
    availableQuantity: number;
  }[];
  organizer: {
    id: number;
    name: string;
    email: string;
  };
}

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: 'admin' | 'organizer' | 'attendee';
  status: 'active' | 'inactive' | 'pending';
  profileImage?: string;
  isEmailVerified: boolean;
  createdAt: string;
  lastLogin?: string;
}

interface ReportRequest {
  startDate: string;
  endDate: string;
  reportType: 'sales' | 'events' | 'reservations';
}

interface SalesReportItem {
  name: string;
  ticketsSold: number;
  revenue: number;
  averageTicketPrice: number;
}

interface SalesReport {
  totalRevenue: number;
  totalTicketsSold: number;
  salesByEvent: SalesReportItem[];
  salesByMonth: SalesReportItem[];
}

interface EventReportItem {
  id: number;
  title: string;
  type: string;
  locationCity: string;
  startDate: string;
  endDate: string;
  totalSeats: number;
  soldSeats: number;
  occupancyRate: number;
}

interface EventsReport {
  totalEvents: number;
  eventsByType: Record<string, number>;
  eventsByCity: Record<string, number>;
  events: EventReportItem[];
}

interface ReservationReportItem {
  reservationNumber: string;
  eventTitle: string;
  customerName: string;
  email: string;
  status: string;
  quantity: number;
  totalAmount: number;
  createdAt: string;
}

interface ReservationsReport {
  totalReservations: number;
  reservationsByStatus: Record<string, number>;
  reservations: ReservationReportItem[];
}

type ReportResponse = SalesReport | EventsReport | ReservationsReport;

const getToken = () => localStorage.getItem('token');

const handleApiError = (error: any) => {
  if (axios.isAxiosError(error)) {
    throw error.response?.data?.message || 'An error occurred';
  }
  throw error;
};

class AdminService {
  private getAuthHeaders() {
    const token = getToken();
    return {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    };
  }

  async getDashboardData(): Promise<DashboardData> {
    try {
      const response = await axios.get(`${API_URL}/admin/dashboard`, this.getAuthHeaders());
      return response.data;
    } catch (error) {
      throw handleApiError(error);
    }
  }

  // Event Management Methods
  async getEvents(search?: string, status?: string): Promise<Event[]> {
    try {
      const params = new URLSearchParams();
      if (search) params.append('search', search);
      if (status) params.append('status', status);

      const response = await axios.get(`${API_URL}/admin/events?${params.toString()}`, this.getAuthHeaders());
      return response.data;
    } catch (error) {
      throw handleApiError(error);
    }
  }

  async getEvent(id: number): Promise<Event> {
    try {
      const response = await axios.get(`${API_URL}/admin/events/${id}`, this.getAuthHeaders());
      return response.data;
    } catch (error) {
      throw handleApiError(error);
    }
  };

  async updateEvent(id: number, eventData: Partial<Event>): Promise<void> {
    try {
      await axios.put(`${API_URL}/admin/events/${id}`, eventData, this.getAuthHeaders());
    } catch (error) {
      throw handleApiError(error);
    }
  };

  async deleteEvent(id: number): Promise<void> {
    try {
      await axios.delete(`${API_URL}/admin/events/${id}`, this.getAuthHeaders());
    } catch (error) {
      throw handleApiError(error);
    }
  };

  // User Management Methods
  async getUsers(search?: string): Promise<User[]> {
    try {
      const params = new URLSearchParams();
      if (search) params.append('search', search);

      const response = await axios.get(`${API_URL}/admin/users?${params.toString()}`, this.getAuthHeaders());
      return response.data;
    } catch (error) {
      throw handleApiError(error);
    }
  }

  async updateUserStatus(userId: string, status: string): Promise<void> {
    try {
      await axios.put(
        `${API_URL}/admin/users/${userId}/status`,
        { status },
        this.getAuthHeaders()
      );
    } catch (error) {
      throw handleApiError(error);
    }
  }

  async updateUserRole(userId: string, role: string): Promise<void> {
    try {
      await axios.put(
        `${API_URL}/admin/users/${userId}/role`,
        { role },
        this.getAuthHeaders()
      );
    } catch (error) {
      throw handleApiError(error);
    }
  }

  async deleteUser(userId: string): Promise<void> {
    try {
      await axios.delete(`${API_URL}/admin/users/${userId}`, this.getAuthHeaders());
    } catch (error) {
      throw handleApiError(error);
    }
  }

  async generateReport(request: ReportRequest): Promise<ReportResponse> {
    try {
      const response = await axios.post(
        `${API_URL}/reports`,
        request,
        this.getAuthHeaders()
      );
      return response.data;
    } catch (error) {
      throw handleApiError(error);
    }
  }
}

export const adminService = new AdminService();
