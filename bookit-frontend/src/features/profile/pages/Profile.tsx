import { useState } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  Grid,
  Avatar,
  Button,
  TextField,
  Stack,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Divider,
} from '@mui/material';

import EditIcon from '@mui/icons-material/Edit';
import CameraAltIcon from '@mui/icons-material/CameraAlt';
import EventNoteIcon from '@mui/icons-material/EventNote';
import FavoriteIcon from '@mui/icons-material/Favorite';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import SettingsIcon from '@mui/icons-material/Settings';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../auth/context/AuthContext';
import paths, { rootPaths } from 'routes/paths';
import { authService } from '../../auth/services/authService';

const Profile = () => {
  const navigate = useNavigate();
  const { user, updateUser } = useAuth();
  const [showSuccess, setShowSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [avatarDialogOpen, setAvatarDialogOpen] = useState(false);
  const [profileData, setProfileData] = useState({
    firstName: user?.firstName || '',
    lastName: user?.lastName || '',
    email: user?.email || '',
    phone: user?.phone || '',
    bio: user?.bio || '',
    location: user?.location || '',
  });

  const handleSave = async () => {
    try {
      const updatedUser = await authService.updateProfile(profileData);
      updateUser(updatedUser);
      setShowSuccess(true);
      setTimeout(() => setShowSuccess(false), 3000);
    } catch (err) {
      setError('Failed to update profile. Please try again.');
      setTimeout(() => setError(null), 3000);
    }
  };

  const handleAvatarChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      try {
        const updatedUser = await authService.uploadProfileImage(event.target.files[0]);
        updateUser(updatedUser);
        setAvatarDialogOpen(false);
      } catch (err) {
        setError('Failed to upload image. Please try again.');
        setTimeout(() => setError(null), 3000);
      }
    }
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
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
              Your profile has been updated successfully
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
        {/* Profile Info Card */}
        <Grid item xs={12} md={4}>
          <Card sx={{ p: 3, textAlign: 'center' }}>
            <Box sx={{ position: 'relative', display: 'inline-block' }}>
              <Avatar
                sx={{ width: 120, height: 120, mx: 'auto', mb: 2 }}
                src={user?.avatar}
              >
                {user?.firstName?.charAt(0)}
              </Avatar>
              <IconButton
                sx={{
                  position: 'absolute',
                  bottom: 0,
                  right: 0,
                  backgroundColor: 'background.paper',
                  boxShadow: 1,
                  '&:hover': { backgroundColor: 'background.default' },
                }}
                onClick={() => setAvatarDialogOpen(true)}
              >
                <CameraAltIcon />
              </IconButton>
            </Box>
            <Typography variant="h5" gutterBottom>
              {profileData.firstName} {profileData.lastName}
            </Typography>
            <Typography color="text.secondary" gutterBottom>
              {profileData.email}
            </Typography>
            <Typography variant="body2" sx={{ mt: 1 }}>
              {profileData.bio}
            </Typography>

            <Divider sx={{ my: 3 }} />

            <Stack spacing={2}>
              <Button
                variant="outlined"
                startIcon={<SettingsIcon />}
                onClick={() => navigate(paths.profile.settings)}
                fullWidth
                color="secondary"
              >
                Profile Settings
              </Button>
              <Button
                variant="outlined"
                startIcon={<EventNoteIcon />}
                onClick={() => navigate(rootPaths.reservations)}
                fullWidth
              >
                My Bookings
              </Button>
              {/* <Button
                variant="outlined"
                startIcon={<FavoriteIcon />}
                onClick={() => navigate(paths.profile.savedEvents)}
                fullWidth
                color="info"
              >
                Saved Events
              </Button> */}
            </Stack>
          </Card>
        </Grid>

        {/* Profile Edit Form */}
        <Grid item xs={12} md={8}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Profile Information
            </Typography>
            <Stack spacing={3}>
              <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                  <TextField
                    label="First Name"
                    fullWidth
                    value={profileData.firstName}
                    onChange={(e) =>
                      setProfileData({ ...profileData, firstName: e.target.value })
                    }
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    label="Last Name"
                    fullWidth
                    value={profileData.lastName}
                    onChange={(e) =>
                      setProfileData({ ...profileData, lastName: e.target.value })
                    }
                  />
                </Grid>
              </Grid>
              <TextField
                label="Email"
                fullWidth
                value={profileData.email}
                onChange={(e) =>
                  setProfileData({ ...profileData, email: e.target.value })
                }
              />
              <TextField
                label="Phone"
                fullWidth
                value={profileData.phone}
                onChange={(e) =>
                  setProfileData({ ...profileData, phone: e.target.value })
                }
              />
              <TextField
                label="Location"
                fullWidth
                value={profileData.location}
                onChange={(e) =>
                  setProfileData({ ...profileData, location: e.target.value })
                }
              />
              <TextField
                label="Bio"
                fullWidth
                multiline
                rows={4}
                value={profileData.bio}
                onChange={(e) =>
                  setProfileData({ ...profileData, bio: e.target.value })
                }
              />
              <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                <Button
                  variant="contained"
                  startIcon={<EditIcon />}
                  onClick={handleSave}
                >
                  Save Changes
                </Button>
              </Box>
            </Stack>
          </Card>
        </Grid>
      </Grid>

      {/* Avatar Upload Dialog */}
      <Dialog
        open={avatarDialogOpen}
        onClose={() => setAvatarDialogOpen(false)}
        maxWidth="xs"
        fullWidth
      >
        <DialogTitle>Change Profile Picture</DialogTitle>
        <DialogContent>
          <Box sx={{ textAlign: 'center', py: 3 }}>
            <input
              accept="image/*"
              style={{ display: 'none' }}
              id="avatar-upload"
              type="file"
              onChange={handleAvatarChange}
            />
            <label htmlFor="avatar-upload">
              <Button
                variant="contained"
                component="span"
                startIcon={<CameraAltIcon />}
              >
                Upload Photo
              </Button>
            </label>
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setAvatarDialogOpen(false)}>Cancel</Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default Profile;
