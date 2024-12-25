import { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { useFormik } from 'formik';
import {
  Box,
  Button,
  TextField,
  Typography,
  Link,
  InputAdornment,
  IconButton,
  Card,
  Stack,
} from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { loginSchema } from '../schemas/validation';
import { useAuth } from '../context/AuthContext';
import paths from '../../../routes/paths';

const Login = () => {
  const { login, isLoading } = useAuth();
  const [showPassword, setShowPassword] = useState(false);

  const formik = useFormik({
    initialValues: {
      email: '',
      password: '',
    },
    validationSchema: loginSchema,
    onSubmit: async (values) => {
      await login(values);
    },
  });

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

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
            Sign in
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Don't have an account?{' '}
            <Link component={RouterLink} to={paths.auth.register} variant="subtitle2">
              Get started
            </Link>
          </Typography>
        </Box>

        <form onSubmit={formik.handleSubmit}>
          <Stack spacing={3}>
            <TextField
              fullWidth
              id="email"
              name="email"
              label="Email address"
              value={formik.values.email}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.email && Boolean(formik.errors.email)}
              helperText={formik.touched.email && formik.errors.email}
            />

            <TextField
              fullWidth
              id="password"
              name="password"
              label="Password"
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
                      onClick={handleClickShowPassword}
                      edge="end"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />

            <Link
              component={RouterLink}
              to={paths.auth.forgotPassword}
              variant="body2"
              color="text.secondary"
              alignSelf="flex-end"
            >
              Forgot password?
            </Link>

            <Button
              fullWidth
              size="large"
              type="submit"
              variant="contained"
              disabled={isLoading}
            >
              {isLoading ? 'Signing in...' : 'Sign in'}
            </Button>
          </Stack>
        </form>
      </Card>
    </Box>
  );
};

export default Login;
