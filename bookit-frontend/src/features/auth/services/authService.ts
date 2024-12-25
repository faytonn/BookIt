import axios from 'axios';
import { 
  LoginCredentials, 
  RegisterData, 
  ResetPasswordData, 
  User,
  UpdateProfileRequest,
  UpdateSettingsRequest,
  ChangePasswordRequest
} from '../types';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

class AuthService {
  async login(credentials: LoginCredentials) {
    const response = await axios.post(`${API_URL}/auth/login`, credentials);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('user', JSON.stringify(response.data.user));
    }
    return response.data;
  }

  async register(data: RegisterData) {
    const formData = new FormData();
    formData.append('email', data.email);
    formData.append('password', data.password);
    formData.append('firstName', data.firstName);
    formData.append('lastName', data.lastName);
    formData.append('role', data.role);
    if (data.profileImage) {
      formData.append('profileImage', data.profileImage);
    }

    const response = await axios.post(`${API_URL}/auth/register`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  }

  async verifyEmail(token: string) {
    const response = await axios.post(`${API_URL}/auth/verify-email`, { token });
    return response.data;
  }

  async forgotPassword(email: string) {
    const response = await axios.post(`${API_URL}/auth/forgot-password`, { email });
    return response.data;
  }

  async resetPassword(data: ResetPasswordData) {
    const response = await axios.post(`${API_URL}/auth/reset-password`, data);
    return response.data;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  getCurrentUser(): User | null {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      return JSON.parse(userStr);
    }
    return null;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  async updateProfile(data: UpdateProfileRequest) {
    const token = localStorage.getItem('token');
    const response = await axios.put(`${API_URL}/auth/profile`, data, {
      headers: { Authorization: `Bearer ${token}` }
    });
    if (response.data) {
      localStorage.setItem('user', JSON.stringify(response.data));
    }
    return response.data;
  }

  async updateSettings(data: UpdateSettingsRequest) {
    const token = localStorage.getItem('token');
    const response = await axios.put(`${API_URL}/auth/settings`, data, {
      headers: { Authorization: `Bearer ${token}` }
    });
    if (response.data) {
      localStorage.setItem('user', JSON.stringify(response.data));
    }
    return response.data;
  }

  async changePassword(data: ChangePasswordRequest) {
    const token = localStorage.getItem('token');
    const response = await axios.put(`${API_URL}/auth/change-password`, data, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data;
  }

  async uploadProfileImage(file: File) {
    const token = localStorage.getItem('token');
    const formData = new FormData();
    formData.append('profileImage', file);
    const response = await axios.post(`${API_URL}/auth/profile-image`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${token}`
      }
    });
    if (response.data) {
      localStorage.setItem('user', JSON.stringify(response.data));
    }
    return response.data;
  }
}

export const authService = new AuthService();
