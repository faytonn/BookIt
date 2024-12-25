import React, { useEffect } from 'react';
import { useLocation, Navigate } from 'react-router-dom';
import { Box, Container, Typography, Paper, Button, Stack } from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import ErrorIcon from '@mui/icons-material/Error';
import { useNavigate } from 'react-router-dom';
import { useSnackbar } from 'notistack';
import paths from '../../../routes/paths';

const EmailVerification: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { enqueueSnackbar } = useSnackbar();
  const isSuccess = location.pathname === paths.auth.verifySuccess;

  useEffect(() => {
    if (isSuccess) {
      enqueueSnackbar('Email verification successful! You can now log in.', {
        variant: 'success',
        autoHideDuration: 5000,
        anchorOrigin: { vertical: 'top', horizontal: 'center' }
      });
    } else {
      enqueueSnackbar('Email verification failed. Please try again or contact support.', {
        variant: 'error',
        autoHideDuration: 5000,
        anchorOrigin: { vertical: 'top', horizontal: 'center' }
      });
    }
  }, [isSuccess, enqueueSnackbar]);

  const handleLoginClick = () => {
    navigate(paths.auth.login);
  };

  const handleTryAgainClick = () => {
    navigate(paths.auth.register);
  };

  // If the path is neither success nor failed, redirect to home
  if (![paths.auth.verifySuccess, paths.auth.verifyFailed].includes(location.pathname)) {
    return <Navigate to="/" />;
  }

  return (
    <Container component="main" maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Paper
          elevation={3}
          sx={{
            padding: 4,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            width: '100%',
            backgroundColor: 'background.paper',
            borderRadius: 2,
          }}
        >
          {isSuccess ? (
            <CheckCircleIcon
              sx={{ fontSize: 60, color: 'success.main', mb: 2 }}
            />
          ) : (
            <ErrorIcon sx={{ fontSize: 60, color: 'error.main', mb: 2 }} />
          )}

          <Typography component="h1" variant="h5" align="center" gutterBottom>
            {isSuccess
              ? 'Email Verification Successful!'
              : 'Email Verification Failed'}
          </Typography>

          <Typography color="text.secondary" align="center" sx={{ mt: 1, mb: 3 }}>
            {isSuccess
              ? 'Your email has been successfully verified. You can now log in to your account and start using all features.'
              : 'We were unable to verify your email. The verification link may be expired or invalid. Please try registering again or contact our support team for assistance.'}
          </Typography>

          {isSuccess ? (
            <Button
              fullWidth
              variant="contained"
              color="primary"
              onClick={handleLoginClick}
              sx={{
                mt: 2,
                py: 1.5,
                backgroundColor: 'primary.main',
                '&:hover': {
                  backgroundColor: 'primary.dark',
                },
              }}
            >
              Proceed to Login
            </Button>
          ) : (
            <Stack spacing={2} sx={{ width: '100%', mt: 2 }}>
              <Button
                fullWidth
                variant="contained"
                color="primary"
                onClick={handleTryAgainClick}
                sx={{
                  py: 1.5,
                  backgroundColor: 'primary.main',
                  '&:hover': {
                    backgroundColor: 'primary.dark',
                  },
                }}
              >
                Register Again
              </Button>
              <Button
                fullWidth
                variant="outlined"
                color="primary"
                onClick={handleLoginClick}
                sx={{ py: 1.5 }}
              >
                Back to Login
              </Button>
            </Stack>
          )}
        </Paper>
      </Box>
    </Container>
  );
};

export default EmailVerification;
