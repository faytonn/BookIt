import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Typography,
  Button,
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Box,
  Skeleton,
  CardActionArea,
  Stack,
  Chip,
  Divider,
} from '@mui/material';
import {
  Add as AddIcon,
  Search as SearchIcon,
  CalendarToday as CalendarTodayIcon,
  LocationOn as LocationOnIcon,
  Person as PersonIcon,
  EventSeat as EventSeatIcon,
} from '@mui/icons-material';
import { eventService } from '../services/eventService';
import { Event } from '../types';
import { useAuth } from '../../auth/context/AuthContext';
import paths from 'routes/paths';

const EventList = () => {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [events, setEvents] = useState<Event[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [eventType, setEventType] = useState<string>('');

  const fetchEvents = async (type?: string, search?: string) => {
    try {
      setLoading(true);
      const data = await eventService.getEvents(type, search);
      setEvents(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch events');
      console.error('Error fetching events:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchEvents(eventType, searchTerm);
  }, [eventType, searchTerm]);

  const handleSearch = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
  };

  const handleTypeChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    setEventType(event.target.value as string);
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4 }}>
        <Typography variant="h4" component="h1">
          Events
        </Typography>
        {user && (
          <Button
            variant="contained"
            color="primary"
            startIcon={<AddIcon />}
            onClick={() => navigate(paths.admin.createEvent)}
          >
            Create Event
          </Button>
        )}
      </Box>

      <Box sx={{ mb: 4, display: 'flex', gap: 2, alignItems: 'flex-end' }}>
        <TextField
          label="Search events"
          variant="outlined"
          value={searchTerm}
          onChange={handleSearch}
          sx={{ flexGrow: 1 }}
          InputProps={{
            startAdornment: <SearchIcon color="action" sx={{ mr: 1, ml: 4 }} />,
          }}
        />
        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel>Event Type</InputLabel>
          <Select
            value={eventType}
            label="Event Type"
            onChange={handleTypeChange}
          >
            <MenuItem value="">All Types</MenuItem>
            <MenuItem value="concert">Concert</MenuItem>
            <MenuItem value="conference">Conference</MenuItem>
            <MenuItem value="exhibition">Exhibition</MenuItem>
            <MenuItem value="sport">Sport</MenuItem>
            <MenuItem value="theater">Theater</MenuItem>
            <MenuItem value="other">Other</MenuItem>
          </Select>
        </FormControl>
      </Box>

      {error && (
        <Typography color="error" sx={{ mb: 2 }}>
          {error}
        </Typography>
      )}

      <Grid container spacing={3}>
        {loading
          ? Array.from(new Array(6)).map((_, index) => (
            <Grid item xs={12} sm={6} md={4} key={index}>
              <Card>
                <Skeleton variant="rectangular" height={140} />
                <CardContent>
                  <Skeleton variant="text" height={32} />
                  <Skeleton variant="text" />
                  <Skeleton variant="text" width="60%" />
                </CardContent>
              </Card>
            </Grid>
          ))
          : events.map((event) => (
            <Grid item xs={12} sm={6} md={4} key={event.id}>
              <Card>
                <CardActionArea onClick={() => navigate(paths.events.details.replace(':id', event.id))}>
                  <CardMedia
                    component="img"
                    height="200"
                    image={event.imageUrl ? `${event.imageUrl}` : '/event_ticket.png'}
                    alt={event.title}
                  />
                  <CardContent sx={{height: 'fit-content'}}>
                    <Typography gutterBottom variant="h6" component="div" noWrap>
                      {event.title}
                    </Typography>
                    
                    <Stack spacing={1}>
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <CalendarTodayIcon sx={{ fontSize: 18, color: 'text.secondary' }} />
                        <Typography variant="body2" color="text.secondary">
                          {formatDate(event.startDate)}
                        </Typography>
                      </Box>

                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <LocationOnIcon sx={{ fontSize: 18, color: 'text.secondary' }} />
                        <Typography variant="body2" color="text.secondary" noWrap>
                          {event.location.name}, {event.location.city}
                        </Typography>
                      </Box>

                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <PersonIcon sx={{ fontSize: 18, color: 'text.secondary' }} />
                        <Typography variant="body2" color="text.secondary" noWrap>
                          By {event.organizer.name}
                        </Typography>
                      </Box>

                      <Divider />

                      {/* {event.pricing.map((price) => (
                        <Box key={price.id}>
                          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                            <Typography variant="body2" color="text.secondary">
                              {price.category}
                            </Typography>
                            <Typography variant="subtitle1" color="primary" fontWeight="bold">
                              ${price.price}
                            </Typography>
                          </Box>
                          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                            <EventSeatIcon 
                              sx={{ 
                                fontSize: 16,
                                color: price.availableSeats > 0 ? 'success.main' : 'error.main'
                              }} 
                            />
                            <Typography 
                              variant="caption"
                              color={price.availableSeats > 0 ? 'success.main' : 'error.main'}
                            >
                              {price.availableSeats} / {price.totalSeats} seats available
                            </Typography>
                          </Box>
                        </Box>
                      ))} */}

                      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 1 }}>
                        <Chip
                          label={event.type}
                          color="primary"
                          size="small"
                          sx={{ textTransform: 'capitalize' }}
                        />
                        <Chip
                          label={new Date(event.startDate) > new Date() ? 'Upcoming' : 'Past'}
                          color={new Date(event.startDate) > new Date() ? 'success' : 'error'}
                          size="small"
                          variant="outlined"
                        />
                      </Box>
                    </Stack>
                  </CardContent>
                </CardActionArea>
              </Card>
            </Grid>
          ))}
      </Grid>

      {!loading && events.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="h6" color="text.secondary">
            No events found
          </Typography>
        </Box>
      )}
    </Container>
  );
};

export default EventList;
