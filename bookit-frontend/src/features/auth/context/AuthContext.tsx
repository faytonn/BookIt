import { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import { useSnackbar } from 'notistack';
import axios from 'axios';
import { LoginCredentials, RegisterData, AuthState, User } from '../types';
import { authService } from '../services/authService';
import paths from '../../../routes/paths';

interface AuthContextType extends AuthState {
  login: (credentials: LoginCredentials) => Promise<void>;
  register: (data: RegisterData) => Promise<void>;
  logout: () => void;
  forgotPassword: (email: string) => Promise<void>;
  resetPassword: (token: string, password: string) => Promise<void>;
  verifyEmail: (token: string) => Promise<void>;
  updateUser: (user: User) => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const navigate = useNavigate();
  const { enqueueSnackbar } = useSnackbar();
  const [state, setState] = useState<AuthState>({
    user: authService.getCurrentUser(),
    token: authService.getToken(),
    isAuthenticated: authService.isAuthenticated(),
    isLoading: false,
    error: null,
  });

  useEffect(() => {
    // Setup axios interceptor for token
    const interceptor = axios.interceptors.request.use(
      (config) => {
        const token = authService.getToken();
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    return () => {
      axios.interceptors.request.eject(interceptor);
    };
  }, []);

  const login = async (credentials: LoginCredentials) => {
    try {
      setState((prev) => ({ ...prev, isLoading: true, error: null }));
      const { user, token } = await authService.login(credentials);
      setState((prev) => ({
        ...prev,
        user,
        token,
        isAuthenticated: true,
        isLoading: false,
      }));
      enqueueSnackbar('Successfully logged in!', { variant: 'success' });
      navigate(paths.events.root);
    } catch (error) {
      setState((prev) => ({
        ...prev,
        isLoading: false,
        error: error.response?.data?.message || 'Login failed',
      }));
      enqueueSnackbar(error.response?.data?.message || 'Login failed', {
        variant: 'error',
      });
    }
  };

  const register = async (data: RegisterData) => {
    try {
      setState((prev) => ({ ...prev, isLoading: true, error: null }));
      await authService.register(data);
      setState((prev) => ({ ...prev, isLoading: false }));
      enqueueSnackbar('Registration successful! Please check your email to verify your account.', {
        variant: 'success',
      });
      navigate(paths.auth.login);
    } catch (error) {
      setState((prev) => ({
        ...prev,
        isLoading: false,
        error: error.response?.data?.message || 'Registration failed',
      }));
      enqueueSnackbar(error.response?.data?.message || 'Registration failed', {
        variant: 'error',
      });
    }
  };

  const logout = () => {
    authService.logout();
    setState({
      user: null,
      token: null,
      isAuthenticated: false,
      isLoading: false,
      error: null,
    });
    navigate(paths.auth.login);
  };

  const forgotPassword = async (email: string) => {
    try {
      setState((prev) => ({ ...prev, isLoading: true, error: null }));
      await authService.forgotPassword(email);
      setState((prev) => ({ ...prev, isLoading: false }));
      enqueueSnackbar('Password reset instructions have been sent to your email.', {
        variant: 'success',
      });
    } catch (error) {
      setState((prev) => ({
        ...prev,
        isLoading: false,
        error: error.response?.data?.message || 'Failed to send reset instructions',
      }));
      enqueueSnackbar(error.response?.data?.message || 'Failed to send reset instructions', {
        variant: 'error',
      });
    }
  };

  const resetPassword = async (token: string, password: string) => {
    try {
      setState((prev) => ({ ...prev, isLoading: true, error: null }));
      await authService.resetPassword({ token, password, confirmPassword: password });
      setState((prev) => ({ ...prev, isLoading: false }));
      enqueueSnackbar('Password has been reset successfully!', { variant: 'success' });
      navigate(paths.auth.login);
    } catch (error) {
      setState((prev) => ({
        ...prev,
        isLoading: false,
        error: error.response?.data?.message || 'Failed to reset password',
      }));
      enqueueSnackbar(error.response?.data?.message || 'Failed to reset password', {
        variant: 'error',
      });
    }
  };

  const verifyEmail = async (token: string) => {
    try {
      await authService.verifyEmail(token);
      enqueueSnackbar('Email verified successfully!', { variant: 'success' });
      navigate(paths.auth.login);
    } catch (error) {
      if (axios.isAxiosError(error)) {
        enqueueSnackbar(error.response?.data?.message || 'Failed to verify email', { variant: 'error' });
      } else {
        enqueueSnackbar('An unexpected error occurred', { variant: 'error' });
      }
      throw error;
    }
  };

  const updateUser = (user: User) => {
    setState((prev) => ({
      ...prev,
      user,
    }));
  };

  return (
    <AuthContext.Provider
      value={{
        ...state,
        login,
        register,
        logout,
        forgotPassword,
        resetPassword,
        verifyEmail,
        updateUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
