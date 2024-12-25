import { useState } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  Grid,
  TextField,
  Switch,
  FormControlLabel,
  Button,
  Stack,
  InputAdornment,
  IconButton,
} from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import CheckCircleIcon from '@mui/icons-material/CheckCircleOutlined';

const Settings = () => {
  const [showSuccess, setShowSuccess] = useState(false);
  const [settings, setSettings] = useState({
    siteName: 'Event Reservation System',
    supportEmail: 'support@eventreservation.com',
    maxTicketsPerUser: 10,
    enableWaitlist: true,
    enableAutoRefunds: true,
    enableEmailNotifications: true,
    stripePublicKey: 'pk_test_...',
    stripeSecretKey: 'sk_test_...',
    googleMapsApiKey: 'AIza...',
  });

  const handleSave = () => {
    // Implement settings save logic
    console.log('Saving settings...', settings);
    setShowSuccess(true);
    setTimeout(() => setShowSuccess(false), 3000);
  };

  const handleCopy = (text: string) => {
    navigator.clipboard.writeText(text);
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" gutterBottom>
          System Settings
        </Typography>
        <Typography color="text.secondary">
          Configure system-wide settings and integrations
        </Typography>
      </Box>

      {showSuccess && (
        <Box 
          sx={{ 
            p: 2, 
            mb: 3,
            bgcolor: 'success.lighter',
            borderRadius: 1,
            border: '1px solid',
            borderColor: 'success.light',
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <Box sx={{ color: 'success.main' }}>
            <CheckCircleIcon />
          </Box>
          <Box>
            <Typography variant="subtitle1" color="success.dark" sx={{ fontWeight: 600, mb: 0.5 }}>
              Success!
            </Typography>
            <Typography variant="body2" color="success.dark">
              System settings have been updated successfully
            </Typography>
          </Box>
        </Box>
      )}

      <Grid container spacing={3}>
        {/* General Settings */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              General Settings
            </Typography>
            <Stack spacing={3}>
              <TextField
                label="Site Name"
                fullWidth
                value={settings.siteName}
                onChange={(e) =>
                  setSettings({ ...settings, siteName: e.target.value })
                }
              />
              <TextField
                label="Support Email"
                fullWidth
                value={settings.supportEmail}
                onChange={(e) =>
                  setSettings({ ...settings, supportEmail: e.target.value })
                }
              />
              <TextField
                label="Max Tickets Per User"
                type="number"
                fullWidth
                value={settings.maxTicketsPerUser}
                onChange={(e) =>
                  setSettings({
                    ...settings,
                    maxTicketsPerUser: parseInt(e.target.value),
                  })
                }
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.enableWaitlist}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        enableWaitlist: e.target.checked,
                      })
                    }
                  />
                }
                label="Enable Waitlist"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.enableAutoRefunds}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        enableAutoRefunds: e.target.checked,
                      })
                    }
                  />
                }
                label="Enable Automatic Refunds"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.enableEmailNotifications}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        enableEmailNotifications: e.target.checked,
                      })
                    }
                  />
                }
                label="Enable Email Notifications"
              />
            </Stack>
          </Card>
        </Grid>

        {/* API Keys & Integration */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              API Keys & Integration
            </Typography>
            <Stack spacing={3}>
              <TextField
                label="Stripe Public Key"
                fullWidth
                value={settings.stripePublicKey}
                onChange={(e) =>
                  setSettings({ ...settings, stripePublicKey: e.target.value })
                }
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => handleCopy(settings.stripePublicKey)}
                        edge="end"
                      >
                        <ContentCopyIcon />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
              <TextField
                label="Stripe Secret Key"
                fullWidth
                type="password"
                value={settings.stripeSecretKey}
                onChange={(e) =>
                  setSettings({ ...settings, stripeSecretKey: e.target.value })
                }
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => handleCopy(settings.stripeSecretKey)}
                        edge="end"
                      >
                        <ContentCopyIcon />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
              <TextField
                label="Google Maps API Key"
                fullWidth
                value={settings.googleMapsApiKey}
                onChange={(e) =>
                  setSettings({ ...settings, googleMapsApiKey: e.target.value })
                }
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => handleCopy(settings.googleMapsApiKey)}
                        edge="end"
                      >
                        <ContentCopyIcon />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
            </Stack>
          </Card>
        </Grid>
      </Grid>

      <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end' }}>
        <Button
          variant="contained"
          startIcon={<SaveIcon />}
          onClick={handleSave}
          size="large"
        >
          Save Changes
        </Button>
      </Box>
    </Container>
  );
};

export default Settings;
