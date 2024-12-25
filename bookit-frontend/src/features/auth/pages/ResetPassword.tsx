import { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useFormik } from 'formik';
import {
  Box,
  Button,
  TextField,
  Typography,
  Card,
  Stack,
  InputAdornment,
  IconButton,
} from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { resetPasswordSchema } from '../schemas/validation';
import { useAuth } from '../context/AuthContext';
import paths from '../../../routes/paths';

const ResetPassword = () => {
  const { resetPassword, isLoading } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token');

  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const formik = useFormik({
    initialValues: {
      password: '',
      confirmPassword: '',
    },
    validationSchema: resetPasswordSchema,
    onSubmit: async (values) => {
      if (token) {
        await resetPassword(token, values.password);
        navigate(paths.auth.login);
      }
    },
  });

  if (!token) {
    return (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          p: 3,
        }}
      >
        <Card sx={{ p: 4, width: '100%', maxWidth: 480 }}>
          <Typography variant="h6" color="error" textAlign="center">
            Invalid or expired reset password link.
          </Typography>
        </Card>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        p: 3,
      }}
    >
      <Card sx={{ p: 4, width: '100%', maxWidth: 480 }}>
        <Box sx={{ mb: 3, textAlign: 'center' }}>
          <Typography variant="h4" gutterBottom>
            Reset Password
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Please enter your new password below.
          </Typography>
        </Box>

        <form onSubmit={formik.handleSubmit}>
          <Stack spacing={3}>
            <TextField
              fullWidth
              id="password"
              name="password"
              label="New Password"
              type={showPassword ? 'text' : 'password'}
              value={formik.values.password}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.password && Boolean(formik.errors.password)}
              helperText={formik.touched.password && formik.errors.password}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={() => setShowPassword(!showPassword)}
                      edge="end"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />

            <TextField
              fullWidth
              id="confirmPassword"
              name="confirmPassword"
              label="Confirm New Password"
              type={showConfirmPassword ? 'text' : 'password'}
              value={formik.values.confirmPassword}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={
                formik.touched.confirmPassword &&
                Boolean(formik.errors.confirmPassword)
              }
              helperText={
                formik.touched.confirmPassword && formik.errors.confirmPassword
              }
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle confirm password visibility"
                      onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                      edge="end"
                    >
                      {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />

            <Button
              fullWidth
              size="large"
              type="submit"
              variant="contained"
              disabled={isLoading}
            >
              {isLoading ? 'Resetting...' : 'Reset Password'}
            </Button>
          </Stack>
        </form>
      </Card>
    </Box>
  );
};

export default ResetPassword;
