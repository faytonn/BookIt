import { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Box,
  Chip,
  TablePagination,
  Alert,
  CircularProgress,
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../../features/auth/context/AuthContext';
import { reservationService } from '../services/reservationService';
import { ReservationResponse } from '../types';
import paths from '../../../routes/paths';

const ReservationsList = () => {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [reservations, setReservations] = useState<ReservationResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchReservations = async () => {
      try {
        setLoading(true);
        const data = await reservationService.getUserReservations();
        setReservations(data);
        setError(null);
      } catch (err: any) {
        setError(err.message || 'Failed to fetch reservations');
      } finally {
        setLoading(false);
      }
    };

    if (user) {
      fetchReservations();
    }
  }, [user]);

  const handleChangePage = (_: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return 'success';
      case 'pending':
        return 'warning';
      case 'cancelled':
        return 'error';
      default:
        return 'default';
    }
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4, textAlign: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  if (!user) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="info">Please log in to view your reservations.</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        My Reservations
      </Typography>

      {reservations.length === 0 ? (
        <Alert severity="info">You don't have any reservations yet.</Alert>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Event</TableCell>
                <TableCell>Ticket Type</TableCell>
                <TableCell>Quantity</TableCell>
                <TableCell>Total Amount</TableCell>
                <TableCell>Status</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {reservations
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                .map((reservation) => (
                  <TableRow key={reservation.id}>
                    <TableCell>{reservation.eventTitle}</TableCell>
                    <TableCell>{reservation.ticketCategory}</TableCell>
                    <TableCell>{reservation.quantity}</TableCell>
                    <TableCell>${reservation.totalAmount.toFixed(2)}</TableCell>
                    <TableCell>
                      <Chip
                        label={reservation.status}
                        color={getStatusColor(reservation.status) as any}
                        size="small"
                      />
                    </TableCell>
                    <TableCell align="right">
                      <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end' }}>
                        <Button
                          size="small"
                          onClick={() =>
                            navigate(paths.events.details.replace(':id', reservation.eventId.toString()))
                          }
                        >
                          View Event
                        </Button>
                        {reservation.status.toLowerCase() === 'pending' && (
                          <Button
                            size="small"
                            variant="contained"
                            color="error"
                            onClick={async () => {
                              try {
                                await reservationService.cancelReservation(reservation.id);
                                // Refresh the reservations list
                                const data = await reservationService.getUserReservations();
                                setReservations(data);
                              } catch (err: any) {
                                setError(err.message || 'Failed to cancel reservation');
                              }
                            }}
                          >
                            Cancel
                          </Button>
                        )}
                      </Box>
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
          <TablePagination
            component="div"
            count={reservations.length}
            page={page}
            onPageChange={handleChangePage}
            rowsPerPage={rowsPerPage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </TableContainer>
      )}
    </Container>
  );
};

export default ReservationsList;
