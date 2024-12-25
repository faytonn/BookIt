import { useState } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  Grid,
  Chip,
  Stack,
  Button,
  Tabs,
  Tab,
  Divider,
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import paths from '../../../routes/paths';

interface Booking {
  id: string;
  eventName: string;
  eventDate: string;
  location: string;
  ticketType: string;
  quantity: number;
  totalAmount: number;
  status: 'upcoming' | 'completed' | 'cancelled';
}

const BookingHistory = () => {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('upcoming');

  // Mock data - replace with API call
  const bookings: Booking[] = [
    {
      id: '1',
      eventName: 'Summer Music Festival',
      eventDate: '2024-07-15T18:00:00',
      location: 'Central Park, New York',
      ticketType: 'General Admission',
      quantity: 2,
      totalAmount: 105,
      status: 'upcoming',
    },
    {
      id: '2',
      eventName: 'Tech Conference 2024',
      eventDate: '2024-03-10T09:00:00',
      location: 'Convention Center, San Francisco',
      ticketType: 'VIP Pass',
      quantity: 1,
      totalAmount: 299,
      status: 'completed',
    },
    {
      id: '3',
      eventName: 'Basketball Championship',
      eventDate: '2024-05-20T19:30:00',
      location: 'Sports Arena, Chicago',
      ticketType: 'Premium Seats',
      quantity: 3,
      totalAmount: 450,
      status: 'cancelled',
    },
  ];

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'upcoming':
        return 'primary';
      case 'completed':
        return 'success';
      case 'cancelled':
        return 'error';
      default:
        return 'default';
    }
  };

  const filteredBookings = bookings.filter((booking) => booking.status === activeTab);

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Booking History
      </Typography>

      {/* Tabs */}
      <Box sx={{ mb: 3 }}>
        <Tabs
          value={activeTab}
          onChange={(_, value) => setActiveTab(value)}
          textColor="primary"
          indicatorColor="primary"
        >
          <Tab value="upcoming" label="Upcoming" />
          <Tab value="completed" label="Completed" />
          <Tab value="cancelled" label="Cancelled" />
        </Tabs>
      </Box>

      {/* Bookings List */}
      <Grid container spacing={3}>
        {filteredBookings.map((booking) => (
          <Grid item xs={12} key={booking.id}>
            <Card>
              <Box sx={{ p: 3 }}>
                <Grid container spacing={3}>
                  {/* Event Info */}
                  <Grid item xs={12} sm={8}>
                    <Stack spacing={2}>
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <Typography variant="h6">{booking.eventName}</Typography>
                        <Chip
                          label={booking.status}
                          color={getStatusColor(booking.status) as any}
                          size="small"
                        />
                      </Box>
                      <Box>
                        <Typography variant="body2" color="text.secondary">
                          Date & Time
                        </Typography>
                        <Typography>
                          {new Date(booking.eventDate).toLocaleString()}
                        </Typography>
                      </Box>
                      <Box>
                        <Typography variant="body2" color="text.secondary">
                          Location
                        </Typography>
                        <Typography>{booking.location}</Typography>
                      </Box>
                      <Divider />
                      <Box>
                        <Typography variant="body2" color="text.secondary">
                          Tickets
                        </Typography>
                        <Typography>
                          {booking.ticketType} x {booking.quantity}
                        </Typography>
                      </Box>
                      <Box>
                        <Typography variant="body2" color="text.secondary">
                          Total Amount
                        </Typography>
                        <Typography>${booking.totalAmount}</Typography>
                      </Box>
                    </Stack>
                  </Grid>

                  {/* Actions */}
                  <Grid item xs={12} sm={4}>
                    <Stack spacing={2} sx={{ height: '100%', justifyContent: 'center' }}>
                      {booking.status === 'upcoming' && (
                        <>
                          <Button
                            variant="contained"
                            onClick={() =>
                              navigate(paths.events.details.replace(':id', booking.id))
                            }
                          >
                            View Tickets
                          </Button>
                          <Button
                            variant="outlined"
                            color="error"
                            onClick={() => {
                              // Handle cancellation
                            }}
                          >
                            Cancel Booking
                          </Button>
                        </>
                      )}
                      {booking.status === 'completed' && (
                        <Button
                          variant="outlined"
                          onClick={() => {
                            // Handle review/rating
                          }}
                        >
                          Write a Review
                        </Button>
                      )}
                      {booking.status === 'cancelled' && (
                        <Button
                          variant="outlined"
                          onClick={() =>
                            navigate(paths.events.details.replace(':id', booking.id))
                          }
                        >
                          Book Again
                        </Button>
                      )}
                    </Stack>
                  </Grid>
                </Grid>
              </Box>
            </Card>
          </Grid>
        ))}

        {/* Empty State */}
        {filteredBookings.length === 0 && (
          <Grid item xs={12}>
            <Box sx={{ textAlign: 'center', py: 8 }}>
              <Typography variant="h6" color="text.secondary" gutterBottom>
                No {activeTab} bookings
              </Typography>
              <Button
                variant="contained"
                onClick={() => navigate(paths.events.root)}
              >
                Browse Events
              </Button>
            </Box>
          </Grid>
        )}
      </Grid>
    </Container>
  );
};

export default BookingHistory;
