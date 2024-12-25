import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  Card,
  Grid,
  TextField,
  Button,
  Divider,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Stack,
  Alert,
  CircularProgress,
  InputAdornment,
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import PhoneIcon from '@mui/icons-material/Phone';
import paths from '../../../routes/paths';
import { eventService } from '../../events/services/eventService';
import { reservationService } from '../services/reservationService';
import { Event } from '../../events/types';
import { useAuth } from '../../auth/context/AuthContext';

interface BookingFormData {
  ticketTypeId: number;
  quantity: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}

const BookingPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [event, setEvent] = useState<Event | null>(null);
  console.log(event);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<BookingFormData>({
    ticketTypeId: 0,
    quantity: 1,
    firstName: user?.firstName || '',
    lastName: user?.lastName || '',
    email: user?.email || '',
    phone: '',
  });
  const [phoneError, setPhoneError] = useState('');

  useEffect(() => {
    const loadEvent = async () => {
      try {
        if (!id) throw new Error('Event ID is required');
        const data = await eventService.getEventById(parseInt(id));
        setEvent(data);
      } catch (err) {
        setError('Failed to load event details');
      } finally {
        setLoading(false);
      }
    };

    loadEvent();
  }, [id]);

  const handleInputChange = (field: keyof BookingFormData, value: string | number) => {
    setFormData(prev => ({
      ...prev,
      [field]: value,
    }));
  };

  const formatPhoneNumber = (value: string) => {
    // Remove all non-numeric characters
    const cleaned = value.replace(/\D/g, '');

    // Format as (XXX) XXX-XXXX
    if (cleaned.length >= 10) {
      return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6, 10)}`;
    } else if (cleaned.length > 6) {
      return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6)}`;
    } else if (cleaned.length > 3) {
      return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3)}`;
    } else if (cleaned.length > 0) {
      return `(${cleaned}`;
    }
    return cleaned;
  };

  const validatePhoneNumber = (phone: string) => {
    const cleaned = phone.replace(/\D/g, '');
    if (cleaned.length === 0) {
      setPhoneError('Phone number is required');
      return false;
    }
    if (cleaned.length !== 10) {
      setPhoneError('Phone number must be 10 digits');
      return false;
    }
    setPhoneError('');
    return true;
  };

  const handlePhoneChange = (value: string) => {
    const formattedPhone = formatPhoneNumber(value);
    handleInputChange('phone', formattedPhone);
    validatePhoneNumber(formattedPhone);
  };

  const calculateTotal = () => {
    if (!event) return 0;
    const selectedTicket = event.pricing.find(p => p.id === formData.ticketTypeId);
    return selectedTicket ? selectedTicket.price * formData.quantity : 0;
  };

  const validateForm = () => {
    if (!formData.ticketTypeId) return 'Please select a ticket type';
    if (formData.quantity < 1) return 'Quantity must be at least 1';
    if (!formData.firstName) return 'First name is required';
    if (!formData.lastName) return 'Last name is required';
    if (!formData.email) return 'Email is required';
    if (!formData.phone) return 'Phone is required';
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const validationError = validateForm();
    if (validationError) {
      setError(validationError);
      return;
    }

    try {
      setSubmitting(true);
      setError(null);

      const reservation = await reservationService.createReservation({
        eventId: parseInt(id!),
        pricingId: formData.ticketTypeId,
        quantity: formData.quantity,
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        phone: formData.phone,
      });

      navigate(paths.reservations.confirmation.replace(':id', reservation.id.toString()));
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to create reservation');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4, textAlign: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (!event) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">Event not found</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      <Typography variant="h4" gutterBottom>
        Book Tickets
      </Typography>

      <Grid container spacing={4}>
        {/* Event Summary */}
        <Grid item xs={12} md={4}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Event Details
            </Typography>
            <Stack spacing={2}>
              <Box>
                <Typography variant="subtitle2" color="text.secondary">
                  Event
                </Typography>
                <Typography variant="body1">{event.title}</Typography>
              </Box>
              <Box>
                <Typography variant="subtitle2" color="text.secondary">
                  Date
                </Typography>
                <Typography variant="body1">
                  {new Date(event.startDate).toLocaleDateString()}
                </Typography>
              </Box>
              <Box>
                <Typography variant="subtitle2" color="text.secondary">
                  Location
                </Typography>
                <Typography variant="body1">
                  {event.location.name}, {event.location.city}
                </Typography>
              </Box>
              <Divider />
              <Box>
                <Typography variant="subtitle1" gutterBottom>
                  Order Summary
                </Typography>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                  <Typography>Subtotal</Typography>
                  <Typography>${calculateTotal()}</Typography>
                </Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                  <Typography>Booking Fee</Typography>
                  <Typography>$5</Typography>
                </Box>
                <Divider sx={{ my: 1 }} />
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Typography variant="subtitle1">Total</Typography>
                  <Typography variant="subtitle1" color="primary">
                    ${calculateTotal() + 5}
                  </Typography>
                </Box>
              </Box>
            </Stack>
          </Card>
        </Grid>

        {/* Booking Form */}
        <Grid item xs={12} md={8}>
          <Card sx={{ p: 3 }}>
            <form onSubmit={handleSubmit}>
              <Stack spacing={3}>
                <Box
                  sx={{
                    p: 2,
                    bgcolor: 'primary.lighter',
                    borderRadius: 1,
                    border: '1px solid',
                    borderColor: 'primary.light',
                    display: 'flex',
                    alignItems: 'center',
                    gap: 2,
                    mb: 3
                  }}
                >
                  <Box sx={{ color: 'primary.main' }}>
                    <InfoIcon fontSize="medium" />
                  </Box>
                  <Box>
                    <Typography variant="subtitle1" color="primary.dark" sx={{ fontWeight: 600, mb: 0.5 }}>
                      Complete Your Booking
                    </Typography>
                    <Typography variant="body2" color="primary.dark">
                      Please provide your details below to secure your tickets
                    </Typography>
                  </Box>
                </Box>

                {/* Ticket Selection */}
                <Box>
                  <Typography variant="h6" gutterBottom>
                    Ticket Selection
                  </Typography>
                  <Grid container spacing={2} alignItems="flex-end">
                    <Grid item xs={12} sm={8}>
                      <FormControl fullWidth>
                        <InputLabel>Ticket Type</InputLabel>
                        <Select
                          value={formData.ticketTypeId}
                          label="Ticket Type"
                          onChange={(e) => handleInputChange('ticketTypeId', Number(e.target.value))}
                        >
                          {event.pricing.map((type) => (
                            <MenuItem key={type.id} value={type.id}>
                              {type.category} - ${type.price} ({type.availableSeats} available)
                            </MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </Grid>
                    <Grid item xs={12} sm={4}>
                      <TextField
                        fullWidth
                        label="Quantity"
                        type="number"
                        value={formData.quantity}
                        onChange={(e) => handleInputChange('quantity', parseInt(e.target.value))}
                        InputProps={{
                          inputProps: {
                            min: 1,
                            max: event.pricing.find(p => p.id === formData.ticketTypeId)?.availableSeats || 10
                          }
                        }}
                      />
                    </Grid>
                  </Grid>
                </Box>

                {/* Contact Information */}
                <Box>
                  <Typography variant="h6" gutterBottom>
                    Contact Information
                  </Typography>
                  <Grid container spacing={2}>
                    <Grid item xs={12} sm={6}>
                      <TextField
                        fullWidth
                        label="First Name"
                        value={formData.firstName}
                        onChange={(e) => handleInputChange('firstName', e.target.value)}
                        required
                      />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                      <TextField
                        fullWidth
                        label="Last Name"
                        value={formData.lastName}
                        onChange={(e) => handleInputChange('lastName', e.target.value)}
                        required
                      />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                      <TextField
                        fullWidth
                        label="Email"
                        type="email"
                        value={formData.email}
                        onChange={(e) => handleInputChange('email', e.target.value)}
                        required
                      />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                      <TextField
                        fullWidth
                        label="Phone"
                        value={formData.phone}
                        onChange={(e) => handlePhoneChange(e.target.value)}
                        required
                        error={!!phoneError}
                        helperText={phoneError || 'Format: (XXX) XXX-XXXX'}
                        inputProps={{
                          maxLength: 14,
                          inputMode: 'numeric',
                          pattern: '[0-9]*'
                        }}
                        InputProps={{
                          startAdornment: (
                            <InputAdornment position="start">
                              <PhoneIcon color="action" sx={{ ml: 4 }} />
                            </InputAdornment>
                          ),
                        }}
                      />
                    </Grid>
                  </Grid>
                </Box>

                {/* Submit Buttons */}
                <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                  <Button
                    variant="outlined"
                    onClick={() => navigate(-1)}
                    disabled={submitting}
                  >
                    Cancel
                  </Button>
                  <Button
                    type="submit"
                    variant="contained"
                    size="large"
                    disabled={submitting}
                  >
                    {submitting ? (
                      <>
                        <CircularProgress size={20} sx={{ mr: 1 }} />
                        Processing...
                      </>
                    ) : (
                      'Proceed to Payment'
                    )}
                  </Button>
                </Box>
              </Stack>
            </form>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};

export default BookingPage;