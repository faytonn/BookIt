import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
  Box,
  Typography,
  Card,
  CircularProgress,
  Button,
} from '@mui/material';
import { useAuth } from '../context/AuthContext';
import paths from '../../../routes/paths';

const VerifyEmail = () => {
  const { verifyEmail, isLoading } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token');
  const [verificationStatus, setVerificationStatus] = useState<{
    success: boolean;
    message: string;
  } | null>(null);

  useEffect(() => {
    const verify = async () => {
      if (!token) {
        setVerificationStatus({
          success: false,
          message: 'Invalid verification link.',
        });
        return;
      }

      try {
        await verifyEmail(token);
        setVerificationStatus({
          success: true,
          message: 'Email verified successfully! You can now sign in.',
        });
      } catch (error) {
        setVerificationStatus({
          success: false,
          message:
            'Failed to verify email. The link may be expired or invalid.',
        });
      }
    };

    verify();
  }, [token, verifyEmail]);

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
        <Box sx={{ textAlign: 'center' }}>
          <Typography variant="h4" gutterBottom>
            Email Verification
          </Typography>

          {isLoading && (
            <Box sx={{ display: 'flex', justifyContent: 'center', my: 3 }}>
              <CircularProgress />
            </Box>
          )}

          {verificationStatus && (
            <>
              <Typography
                variant="body1"
                color={verificationStatus.success ? 'success.main' : 'error'}
                sx={{ mb: 3 }}
              >
                {verificationStatus.message}
              </Typography>

              <Button
                fullWidth
                size="large"
                variant="contained"
                onClick={() => navigate(paths.auth.login)}
              >
                Go to Sign In
              </Button>
            </>
          )}
        </Box>
      </Card>
    </Box>
  );
};

export default VerifyEmail;
