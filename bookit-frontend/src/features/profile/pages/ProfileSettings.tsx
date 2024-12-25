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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
} from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';
import SecurityIcon from '@mui/icons-material/Security';
import NotificationsIcon from '@mui/icons-material/Notifications';
import PaymentIcon from '@mui/icons-material/Payment';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { useAuth } from '../../auth/context/AuthContext';
import { authService } from '../../auth/services/authService';

const ProfileSettings = () => {
  const { user, updateUser } = useAuth();
  const [showSuccess, setShowSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [passwordDialogOpen, setPasswordDialogOpen] = useState(false);
  const [passwordData, setPasswordData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });
  const [settings, setSettings] = useState({
    emailNotifications: user?.emailNotifications || false,
    smsNotifications: user?.smsNotifications || false,
    marketingEmails: user?.marketingEmails || false,
    twoFactorAuth: user?.twoFactorAuth || false,
  });

  const [savedCards] = useState([
    { id: 1, last4: '4242', brand: 'Visa', expiry: '12/24' },
    { id: 2, last4: '5555', brand: 'Mastercard', expiry: '08/25' },
  ]);

  const handleSave = async () => {
    try {
      const updatedUser = await authService.updateSettings(settings);
      updateUser(updatedUser);
      setShowSuccess(true);
      setTimeout(() => setShowSuccess(false), 3000);
    } catch (err) {
      setError('Failed to update settings. Please try again.');
      setTimeout(() => setError(null), 3000);
    }
  };

  const handlePasswordChange = async () => {
    try {
      if (passwordData.newPassword !== passwordData.confirmPassword) {
        setError('New passwords do not match');
        return;
      }
      await authService.changePassword(passwordData);
      setPasswordDialogOpen(false);
      setShowSuccess(true);
      setTimeout(() => setShowSuccess(false), 3000);
      setPasswordData({
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
      });
    } catch (err) {
      setError('Failed to change password. Please try again.');
      setTimeout(() => setError(null), 3000);
    }
  };

  const handleDeleteCard = (cardId: number) => {
    // Implement card deletion logic
    console.log('Deleting card...', cardId);
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" gutterBottom>
          Account Settings
        </Typography>
        <Typography color="text.secondary">
          Manage your account preferences and security settings
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
              Your settings have been saved successfully
            </Typography>
          </Box>
        </Box>
      )}

      {error && (
        <Box 
          sx={{ 
            p: 2, 
            mb: 3,
            bgcolor: 'error.lighter',
            borderRadius: 1,
            border: '1px solid',
            borderColor: 'error.light',
            display: 'flex',
            alignItems: 'center',
            gap: 2,
            position: 'fixed',
            bottom: 24,
            right: 24,
            zIndex: 2000,
          }}
        >
          <Box sx={{ color: 'error.main' }}>
            <ErrorOutlineIcon />
          </Box>
          <Box>
            <Typography variant="subtitle1" color="error.dark" sx={{ fontWeight: 600, mb: 0.5 }}>
              Error
            </Typography>
            <Typography variant="body2" color="error.dark">
              {error}
            </Typography>
          </Box>
        </Box>
      )}

      <Grid container spacing={3}>
        {/* Notification Settings */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
              <NotificationsIcon sx={{ mr: 1 }} />
              <Typography variant="h6">Notification Preferences</Typography>
            </Box>
            <Stack spacing={2}>
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.emailNotifications}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        emailNotifications: e.target.checked,
                      })
                    }
                  />
                }
                label="Email Notifications"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.smsNotifications}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        smsNotifications: e.target.checked,
                      })
                    }
                  />
                }
                label="SMS Notifications"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.marketingEmails}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        marketingEmails: e.target.checked,
                      })
                    }
                  />
                }
                label="Marketing Emails"
              />
            </Stack>
          </Card>
        </Grid>

        {/* Security Settings */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
              <SecurityIcon sx={{ mr: 1 }} />
              <Typography variant="h6">Security Settings</Typography>
            </Box>
            <Stack spacing={3}>
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.twoFactorAuth}
                    onChange={(e) =>
                      setSettings({
                        ...settings,
                        twoFactorAuth: e.target.checked,
                      })
                    }
                  />
                }
                label="Two-Factor Authentication"
              />
              <Button
                variant="outlined"
                onClick={() => setPasswordDialogOpen(true)}
              >
                Change Password
              </Button>
            </Stack>
          </Card>
        </Grid>

        {/* Payment Methods */}
        <Grid item xs={12}>
          <Card sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
              <PaymentIcon sx={{ mr: 1 }} />
              <Typography variant="h6">Payment Methods</Typography>
            </Box>
            <List>
              {savedCards.map((card) => (
                <ListItem key={card.id}>
                  <ListItemText
                    primary={`${card.brand} •••• ${card.last4}`}
                    secondary={`Expires ${card.expiry}`}
                  />
                  <ListItemSecondaryAction>
                    <IconButton
                      edge="end"
                      onClick={() => handleDeleteCard(card.id)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>
              ))}
            </List>
            <Box sx={{ mt: 2 }}>
              <Button variant="outlined">Add New Card</Button>
            </Box>
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

      {/* Change Password Dialog */}
      <Dialog
        open={passwordDialogOpen}
        onClose={() => setPasswordDialogOpen(false)}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>Change Password</DialogTitle>
        <DialogContent>
          <Stack spacing={3} sx={{ mt: 2 }}>
            <TextField
              label="Current Password"
              type="password"
              fullWidth
              value={passwordData.currentPassword}
              onChange={(e) => setPasswordData({ ...passwordData, currentPassword: e.target.value })}
            />
            <TextField
              label="New Password"
              type="password"
              fullWidth
              value={passwordData.newPassword}
              onChange={(e) => setPasswordData({ ...passwordData, newPassword: e.target.value })}
            />
            <TextField
              label="Confirm New Password"
              type="password"
              fullWidth
              value={passwordData.confirmPassword}
              onChange={(e) => setPasswordData({ ...passwordData, confirmPassword: e.target.value })}
            />
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setPasswordDialogOpen(false)}>Cancel</Button>
          <Button 
            variant="contained" 
            onClick={handlePasswordChange}
            disabled={!passwordData.currentPassword || !passwordData.newPassword || !passwordData.confirmPassword}
          >
            Change Password
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default ProfileSettings;
