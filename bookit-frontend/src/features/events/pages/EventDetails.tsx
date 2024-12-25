import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Grid,
  Typography,
  Box,
  Card,
  CardContent,
  Button,
  Chip,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Skeleton,
  Alert,
  IconButton,
} from '@mui/material';
import {
  Edit as EditIcon,
  Delete as DeleteIcon,
  LocationOn as LocationIcon,
  CalendarToday as CalendarIcon,
  Person as PersonIcon,
} from '@mui/icons-material';
import { eventService } from '../services/eventService';
import { Event } from '../types';
import paths from 'routes/paths';
import { useAuth } from '../../auth/context/AuthContext';

const EventDetails = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [event, setEvent] = useState<Event | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        setLoading(true);
        if (!id) throw new Error('Event ID is required');
        const data = await eventService.getEventById(parseInt(id));
        setEvent(data);
      } catch (err) {
        setError('Failed to fetch event details');
        console.error('Error fetching event:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchEvent();
  }, [id]);

  const handleDelete = async () => {
    if (!event || !window.confirm('Are you sure you want to delete this event?')) return;

    try {
      await eventService.deleteEvent(parseInt(event.id));
      navigate(paths.events.root);
    } catch (err) {
      setError('Failed to delete event');
      console.error('Error deleting event:', err);
    }
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Grid container spacing={4}>
          <Grid item xs={12} md={8}>
            <Skeleton variant="rectangular" height={400} sx={{ mb: 2 }} />
            <Skeleton variant="text" height={60} sx={{ mb: 2 }} />
            <Skeleton variant="text" height={100} />
          </Grid>
          <Grid item xs={12} md={4}>
            <Skeleton variant="rectangular" height={300} />
          </Grid>
        </Grid>
      </Container>
    );
  }

  if (error || !event) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 2 }}>
          {error || 'Event not found'}
        </Alert>
        <Button variant="contained" onClick={() => navigate(paths.events.root)}>
          Back to Events
        </Button>
      </Container>
    );
  }

  const isOrganizer = user?.id && event?.organizer?.id && user.id === event.organizer.id;

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Grid container spacing={4}>
        <Grid item xs={12} md={8}>
          {/* Main Content */}
          <Box sx={{ mb: 4 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
              <Typography variant="h3" component="h1" gutterBottom>
                {event.title}
              </Typography>
              {isOrganizer && (
                <Box>
                  <IconButton
                    color="primary"
                    onClick={() => navigate(paths.events.edit.replace(':id', event.id))}
                  >
                    <EditIcon />
                  </IconButton>
                  <IconButton color="error" onClick={handleDelete}>
                    <DeleteIcon />
                  </IconButton>
                </Box>
              )}
            </Box>

            {/* Event Image */}
            <Box
              component="img"
              src={event.imageUrl ? `http://localhost:5000${event.imageUrl}` : '/event_ticket.png'}
              alt={event.title}
              sx={{
                width: '100%',
                height: 400,
                objectFit: 'cover',
                borderRadius: 1,
                mb: 4,
              }}
            />

            {/* Event Details */}
            <Grid container spacing={2} sx={{ mb: 4 }}>
              <Grid item xs={12} sm={4}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <CalendarIcon color="primary" />
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Start Date
                    </Typography>
                    <Typography>{formatDate(event.startDate)}</Typography>
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={4}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <CalendarIcon color="primary" />
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      End Date
                    </Typography>
                    <Typography>{formatDate(event.endDate)}</Typography>
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={4}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <LocationIcon color="primary" />
                  <Box>
                    <Typography variant="subtitle2" color="text.secondary">
                      Location
                    </Typography>
                    <Typography>
                      {event.location.name}, {event.location.city}
                    </Typography>
                  </Box>
                </Box>
              </Grid>
            </Grid>

            {/* Description */}
            <Typography variant="h5" gutterBottom>
              About This Event
            </Typography>
            <Typography paragraph>{event.description}</Typography>

            {/* Tags */}
            <Box sx={{ mt: 2, mb: 4 }}>
              {event.tags.map((tag) => (
                <Chip
                  key={tag}
                  label={tag}
                  sx={{ mr: 1, mb: 1 }}
                  size="small"
                  variant="outlined"
                />
              ))}
            </Box>

            {/* Organizer */}
            <Box sx={{ mt: 4 }}>
              <Typography variant="h5" gutterBottom>
                Organizer
              </Typography>
              <Card variant="outlined">
                <CardContent>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                    <PersonIcon fontSize="large" />
                    <Box>
                      <Typography variant="h6">{event.organizer.name}</Typography>
                      <Typography color="text.secondary">{event.organizer.email}</Typography>
                    </Box>
                  </Box>
                </CardContent>
              </Card>
            </Box>
          </Box>
        </Grid>

        {/* Sidebar */}
        <Grid item xs={12} md={4}>
          <Paper elevation={2} sx={{ p: 3, position: 'sticky', top: 20 }}>
            <Typography variant="h5" gutterBottom>
              Ticket Information
            </Typography>
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Category</TableCell>
                    <TableCell align="right">Price</TableCell>
                    <TableCell align="right">Available</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {event.pricing.map((price) => (
                    <TableRow key={price.category}>
                      <TableCell>{price.category}</TableCell>
                      <TableCell align="right">${price.price}</TableCell>
                      <TableCell align="right">
                        {price.availableSeats}/{price.totalSeats}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
            <Button
              variant="contained"
              color="primary"
              fullWidth
              size="large"
              sx={{ mt: 3 }}
              onClick={() => navigate(`/events/${event.id}/book`)}
            >
              Book Now
            </Button>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default EventDetails;
