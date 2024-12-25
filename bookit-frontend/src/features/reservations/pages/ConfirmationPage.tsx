import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  Card,
  Button,
  Divider,
  Grid,
  Stack,
  Chip,
} from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import paths from '../../../routes/paths';

interface BookingConfirmation {
  id: string;
  eventName: string;
  eventDate: string;
  location: string;
  ticketType: string;
  quantity: number;
  totalAmount: number;
  bookingDate: string;
  qrCode: string;
}

const ConfirmationPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [booking, setBooking] = useState<BookingConfirmation | null>(null);

  useEffect(() => {
    const fetchBooking = async () => {
      try {
        // Replace with actual API call
        await new Promise(resolve => setTimeout(resolve, 1000));
        // Mock data
        setBooking({
          id: id || '123',
          eventName: 'Summer Music Festival',
          eventDate: '2024-07-15T18:00:00',
          location: 'Central Park, New York',
          ticketType: 'General Admission',
          quantity: 2,
          totalAmount: 105,
          bookingDate: new Date().toISOString(),
          qrCode: 'https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=123456789',
        });
      } catch (error) {
        console.error('Failed to fetch booking:', error);
      }
    };

    if (id) {
      fetchBooking();
    }
  }, [id]);

  if (!booking) {
    return null;
  }

  const handleDownloadTickets = () => {
    // Implement ticket download functionality
    console.log('Downloading tickets...');
  };

  const handleAddToCalendar = () => {
    // Implement calendar integration
    console.log('Adding to calendar...');
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Box sx={{ textAlign: 'center', mb: 4 }}>
        <CheckCircleIcon color="success" sx={{ fontSize: 64, mb: 2 }} />
        <Typography variant="h4" gutterBottom>
          Booking Confirmed!
        </Typography>
        <Typography color="text.secondary">
          Your booking reference number is #{booking.id}
        </Typography>
      </Box>

      <Card sx={{ p: 4 }}>
        <Grid container spacing={4}>
          {/* Event Details */}
          <Grid item xs={12} md={8}>
            <Stack spacing={3}>
              <Box>
                <Typography variant="h6" gutterBottom>
                  Event Details
                </Typography>
                <Stack spacing={2}>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Event
                    </Typography>
                    <Typography variant="body1">{booking.eventName}</Typography>
                  </Box>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Date & Time
                    </Typography>
                    <Typography variant="body1">
                      {new Date(booking.eventDate).toLocaleString()}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Location
                    </Typography>
                    <Typography variant="body1">{booking.location}</Typography>
                  </Box>
                </Stack>
              </Box>

              <Divider />

              <Box>
                <Typography variant="h6" gutterBottom>
                  Booking Details
                </Typography>
                <Stack spacing={2}>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Ticket Type
                    </Typography>
                    <Typography variant="body1">
                      {booking.ticketType} x {booking.quantity}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Total Amount
                    </Typography>
                    <Typography variant="body1">${booking.totalAmount}</Typography>
                  </Box>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Booking Date
                    </Typography>
                    <Typography variant="body1">
                      {new Date(booking.bookingDate).toLocaleString()}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Status
                    </Typography>
                    <Chip label="Confirmed" color="success" size="small" />
                  </Box>
                </Stack>
              </Box>
            </Stack>
          </Grid>

          {/* QR Code */}
          <Grid item xs={12} md={4}>
            <Box sx={{ textAlign: 'center' }}>
              <img
                src={booking.qrCode}
                alt="Booking QR Code"
                style={{ width: '100%', maxWidth: 200 }}
              />
              <Typography variant="caption" display="block" sx={{ mt: 1 }}>
                Show this QR code at the venue
              </Typography>
            </Box>
          </Grid>
        </Grid>

        {/* Action Buttons */}
        <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button
            variant="contained"
            onClick={handleDownloadTickets}
          >
            Download Tickets
          </Button>
          <Button
            variant="outlined"
            onClick={handleAddToCalendar}
          >
            Add to Calendar
          </Button>
        </Box>

        {/* Navigation Links */}
        <Box sx={{ mt: 4, textAlign: 'center' }}>
          <Button
            onClick={() => navigate(paths.events.root)}
          >
            Browse More Events
          </Button>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 2 }}>
            A confirmation email has been sent to your registered email address
          </Typography>
        </Box>
      </Card>
    </Container>
  );
};

export default ConfirmationPage;
