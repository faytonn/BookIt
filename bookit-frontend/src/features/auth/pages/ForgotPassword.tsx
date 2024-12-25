import { Link as RouterLink } from 'react-router-dom';
import { useFormik } from 'formik';
import {
  Box,
  Button,
  TextField,
  Typography,
  Link,
  Card,
  Stack,
} from '@mui/material';
import { forgotPasswordSchema } from '../schemas/validation';
import { useAuth } from '../context/AuthContext';
import paths from '../../../routes/paths';

const ForgotPassword = () => {
  const { forgotPassword, isLoading } = useAuth();

  const formik = useFormik({
    initialValues: {
      email: '',
    },
    validationSchema: forgotPasswordSchema,
    onSubmit: async (values) => {
      await forgotPassword(values.email);
    },
  });

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
            Forgot Password
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Enter your email address below and we'll send you a link to reset your
            password.
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

            <Button
              fullWidth
              size="large"
              type="submit"
              variant="contained"
              disabled={isLoading}
            >
              {isLoading ? 'Sending...' : 'Send Reset Link'}
            </Button>

            <Link
              component={RouterLink}
              to={paths.auth.login}
              variant="body2"
              color="text.secondary"
              sx={{ textAlign: 'center', display: 'block' }}
            >
              Back to Sign in
            </Link>
          </Stack>
        </form>
      </Card>
    </Box>
  );
};

export default ForgotPassword;
