import { useState, useRef } from 'react';
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
  Grid,
  FormControl,
  FormHelperText,
  InputLabel,
  Select,
  MenuItem,
  Avatar,
} from '@mui/material';
import { Visibility, VisibilityOff, AddAPhoto } from '@mui/icons-material';
import { registerSchema } from '../schemas/validation';
import { useAuth } from '../context/AuthContext';
import paths from '../../../routes/paths';

interface RegisterFormValues {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  role: 'organizer' | 'attendee';
  profileImage: File | null;
}

interface RegisterData extends Omit<RegisterFormValues, 'confirmPassword' | 'profileImage'> {
  profileImage: string | null;
}

const Register = () => {
  const { register, isLoading } = useAuth();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [profileImage, setProfileImage] = useState<string | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const formik = useFormik({
    initialValues: {
      email: '',
      password: '',
      confirmPassword: '',
      firstName: '',
      lastName: '',
      role: '' as 'organizer' | 'attendee',
      profileImage: null as File | null,
    },
    validationSchema: registerSchema,
    onSubmit: async (values: RegisterFormValues) => {
      const { confirmPassword, ...registerData } = values;
      await register(registerData);
    },
  });

  const handleImageUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      formik.setFieldValue('profileImage', file);
      const reader = new FileReader();
      reader.onload = () => {
        setProfileImage(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleImageClick = () => {
    fileInputRef.current?.click();
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
      <Card sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Box sx={{ mb: 3, textAlign: 'center' }}>
          <Typography variant="h4" gutterBottom>
            Get started
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Already have an account?{' '}
            <Link component={RouterLink} to={paths.auth.login} variant="subtitle2">
              Sign in
            </Link>
          </Typography>
        </Box>

        <form onSubmit={formik.handleSubmit}>
          <Stack spacing={3}>
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 2 }}>
              <Box
                sx={{
                  position: 'relative',
                  '&:hover .upload-overlay': {
                    opacity: 1,
                  },
                }}
              >
                <Avatar
                  src={profileImage || undefined}
                  sx={{
                    width: 100,
                    height: 100,
                    cursor: 'pointer',
                  }}
                  onClick={handleImageClick}
                />
                <Box
                  className="upload-overlay"
                  sx={{
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    right: 0,
                    bottom: 0,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                    borderRadius: '50%',
                    opacity: 0,
                    transition: 'opacity 0.2s',
                    cursor: 'pointer',
                  }}
                  onClick={handleImageClick}
                >
                  <AddAPhoto sx={{ color: 'white' }} />
                </Box>
                <input
                  type="file"
                  ref={fileInputRef}
                  accept="image/*"
                  style={{ display: 'none' }}
                  onChange={handleImageUpload}
                />
              </Box>
            </Box>

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  id="firstName"
                  name="firstName"
                  label="First name"
                  value={formik.values.firstName}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  error={formik.touched.firstName && Boolean(formik.errors.firstName)}
                  helperText={formik.touched.firstName && formik.errors.firstName}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  id="lastName"
                  name="lastName"
                  label="Last name"
                  value={formik.values.lastName}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  error={formik.touched.lastName && Boolean(formik.errors.lastName)}
                  helperText={formik.touched.lastName && formik.errors.lastName}
                />
              </Grid>
            </Grid>

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

            <FormControl
              fullWidth
              error={formik.touched.role && Boolean(formik.errors.role)}
            >
              <InputLabel id="role-label">Role</InputLabel>
              <Select
                labelId="role-label"
                id="role"
                name="role"
                value={formik.values.role}
                label="Role"
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
              >
                <MenuItem value="attendee">Attendee</MenuItem>
                <MenuItem value="organizer">Event Organizer</MenuItem>
              </Select>
              {formik.touched.role && formik.errors.role && (
                <FormHelperText>{formik.errors.role}</FormHelperText>
              )}
            </FormControl>

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
              label="Confirm password"
              type={showConfirmPassword ? 'text' : 'password'}
              value={formik.values.confirmPassword}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={
                formik.touched.confirmPassword && Boolean(formik.errors.confirmPassword)
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
              {isLoading ? 'Creating account...' : 'Create account'}
            </Button>
          </Stack>
        </form>
      </Card>
    </Box>
  );
};

export default Register;
