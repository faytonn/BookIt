import { useState } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  Tabs,
  Tab,
  Chip,
  Button,
  Stack,
  Grid,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton,
} from '@mui/material';
import QRCode from 'react-qr-code';
import DownloadIcon from '@mui/icons-material/Download';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import ShareIcon from '@mui/icons-material/Share';
import CloseIcon from '@mui/icons-material/Close';
import { useNavigate } from 'react-router-dom';

interface Booking {
  id: string;
  eventName: string;
  eventDate: string;
  ticketType: string;
  quantity: number;
  totalAmount: number;
  status: 'upcoming' | 'completed' | 'cancelled';
  qrCode: string;
}

const ProfileBookings = () => {
  const [currentTab, setCurrentTab] = useState(0);
  const [selectedBooking, setSelectedBooking] = useState<Booking | null>(null);
  const [ticketDialogOpen, setTicketDialogOpen] = useState(false);

  // Mock data - replace with API call
  const bookings: Booking[] = [
    {
      id: 'BK001',
      eventName: 'Summer Music Festival',
      eventDate: '2024-07-15T18:00:00',
      ticketType: 'VIP Pass',
      quantity: 2,
      totalAmount: 200,
      status: 'upcoming',
      qrCode: 'https://example.com/qr/BK001',
    },
    {
      id: 'BK002',
      eventName: 'Tech Conference 2024',
      eventDate: '2024-08-01T09:00:00',
      ticketType: 'Regular Entry',
      quantity: 1,
      totalAmount: 50,
      status: 'upcoming',
      qrCode: 'https://example.com/qr/BK002',
    },
    // Add more mock bookings
  ];

  const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
    setCurrentTab(newValue);
  };

  const handleViewTicket = (booking: Booking) => {
    setSelectedBooking(booking);
    setTicketDialogOpen(true);
  };

  const handleDownloadTicket = () => {
    // Implement ticket download logic
    console.log('Downloading ticket...');
  };

  const handleAddToCalendar = () => {
    // Implement calendar integration logic
    console.log('Adding to calendar...');
  };

  const handleShareTicket = () => {
    // Implement ticket sharing logic
    console.log('Sharing ticket...');
  };

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

  const filteredBookings = bookings.filter((booking) => {
    switch (currentTab) {
      case 0:
        return booking.status === 'upcoming';
      case 1:
        return booking.status === 'completed';
      case 2:
        return booking.status === 'cancelled';
      default:
        return true;
    }
  });

  const navigate = useNavigate();

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        My Bookings
      </Typography>

      <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
        <Tabs value={currentTab} onChange={handleTabChange}>
          <Tab label="Upcoming" />
          <Tab label="Completed" />
          <Tab label="Cancelled" />
        </Tabs>
      </Box>

      <Grid container spacing={3}>
        {filteredBookings.map((booking) => (
          <Grid item xs={12} key={booking.id}>
            <Card sx={{ p: 3 }}>
              <Grid container spacing={2} alignItems="center">
                <Grid item xs={12} sm={6}>
                  <Typography variant="h6" gutterBottom>
                    {booking.eventName}
                  </Typography>
                  <Typography color="text.secondary" gutterBottom>
                    {new Date(booking.eventDate).toLocaleString()}
                  </Typography>
                  <Stack direction="row" spacing={1} alignItems="center">
                    <Chip
                      label={booking.status}
                      color={getStatusColor(booking.status) as any}
                      size="small"
                    />
                    <Typography variant="body2">
                      {booking.quantity} × {booking.ticketType}
                    </Typography>
                  </Stack>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Stack
                    direction="row"
                    spacing={2}
                    justifyContent={{ xs: 'flex-start', sm: 'flex-end' }}
                  >
                    <Typography variant="h6" color="primary">
                      ${booking.totalAmount}
                    </Typography>
                    <Button
                      variant="contained"
                      onClick={() => handleViewTicket(booking)}
                    >
                      View Ticket
                    </Button>
                  </Stack>
                </Grid>
              </Grid>
            </Card>
          </Grid>
        ))}

        {filteredBookings.length === 0 && (
          <Grid item xs={12}>
            <Box
              sx={{
                textAlign: 'center',
                py: 8,
                bgcolor: 'background.paper',
                borderRadius: 1,
              }}
            >
              <Typography variant="h6" color="text.secondary" gutterBottom>
                No bookings found
              </Typography>
              <Button variant="contained" onClick={() => navigate("")}>
                Browse Events
              </Button>
            </Box>
          </Grid>
        )}
      </Grid>

      {/* Ticket Dialog */}
      <Dialog
        open={ticketDialogOpen}
        onClose={() => setTicketDialogOpen(false)}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Typography variant="h6">Ticket Details</Typography>
            <IconButton onClick={() => setTicketDialogOpen(false)}>
              <CloseIcon />
            </IconButton>
          </Box>
        </DialogTitle>
        <DialogContent>
          {selectedBooking && (
            <Stack spacing={3} alignItems="center" sx={{ py: 2 }}>
              <QRCode value={selectedBooking.qrCode} />
              <Typography variant="h6">{selectedBooking.eventName}</Typography>
              <Typography color="text.secondary">
                {new Date(selectedBooking.eventDate).toLocaleString()}
              </Typography>
              <Box sx={{ width: '100%' }}>
                <Grid container spacing={2}>
                  <Grid item xs={6}>
                    <Typography variant="body2" color="text.secondary">
                      Ticket Type
                    </Typography>
                    <Typography>{selectedBooking.ticketType}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="body2" color="text.secondary">
                      Quantity
                    </Typography>
                    <Typography>{selectedBooking.quantity}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="body2" color="text.secondary">
                      Booking ID
                    </Typography>
                    <Typography>{selectedBooking.id}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="body2" color="text.secondary">
                      Total Amount
                    </Typography>
                    <Typography>${selectedBooking.totalAmount}</Typography>
                  </Grid>
                </Grid>
              </Box>
            </Stack>
          )}
        </DialogContent>
        <DialogActions sx={{ p: 3 }}>
          <Button
            startIcon={<DownloadIcon />}
            onClick={handleDownloadTicket}
          >
            Download
          </Button>
          <Button
            startIcon={<CalendarTodayIcon />}
            onClick={handleAddToCalendar}
          >
            Add to Calendar
          </Button>
          <Button
            startIcon={<ShareIcon />}
            onClick={handleShareTicket}
          >
            Share
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default ProfileBookings;
