export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'admin' | 'organizer' | 'attendee';
  isEmailVerified: boolean;
  createdAt: string;
  updatedAt: string;
  avatar?: string;
  name?: string;
  phone?: string;
  bio?: string;
  location?: string;
  emailNotifications?: boolean;
  smsNotifications?: boolean;
  marketingEmails?: boolean;
  twoFactorAuth?: boolean;
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: 'organizer' | 'attendee';
  profileImage?: File | null;
}

export interface ResetPasswordData {
  token: string;
  password: string;
  confirmPassword: string;
}

export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  bio: string;
  location: string;
}

export interface UpdateSettingsRequest {
  emailNotifications: boolean;
  smsNotifications: boolean;
  marketingEmails: boolean;
  twoFactorAuth: boolean;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
}
