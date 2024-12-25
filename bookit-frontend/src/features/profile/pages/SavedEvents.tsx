import { useState } from 'react';
import {
  Container,
  Typography,
  Box,
  Grid,
  Card,
  CardMedia,
  CardContent,
  CardActions,
  Button,
  IconButton,
  Chip,
  Stack,
  TextField,
  InputAdornment,
  MenuItem,
  FormControl,
  InputLabel,
  Select,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ShareIcon from '@mui/icons-material/Share';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { useNavigate } from 'react-router-dom';

interface SavedEvent {
  id: string;
  title: string;
  date: string;
  location: string;
  imageUrl: string;
  price: number;
  category: string;
  availableTickets: number;
}

const SavedEvents = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [sortBy, setSortBy] = useState('date');

  // Mock data - replace with API call
  const savedEvents: SavedEvent[] = [
    {
      id: '1',
      title: 'Summer Music Festival',
      date: '2024-07-15T18:00:00',
      location: 'Central Park, New York',
      imageUrl: 'https://source.unsplash.com/random/800x400/?concert',
      price: 99.99,
      category: 'Music',
      availableTickets: 245,
    },
    {
      id: '2',
      title: 'Tech Conference 2024',
      date: '2024-08-01T09:00:00',
      location: 'Convention Center, San Francisco',
      imageUrl: 'https://source.unsplash.com/random/800x400/?technology',
      price: 149.99,
      category: 'Conference',
      availableTickets: 120,
    },
    // Add more mock events
  ];

  const handleRemoveFromSaved = (eventId: string) => {
    // Implement remove from saved logic
    console.log('Removing event from saved...', eventId);
  };

  const handleShare = (eventId: string) => {
    // Implement share logic
    console.log('Sharing event...', eventId);
  };

  const handleAddToCalendar = (eventId: string) => {
    // Implement add to calendar logic
    console.log('Adding event to calendar...', eventId);
  };

  const filteredEvents = savedEvents.filter((event) =>
    event.title.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const sortedEvents = [...filteredEvents].sort((a, b) => {
    switch (sortBy) {
      case 'date':
        return new Date(a.date).getTime() - new Date(b.date).getTime();
      case 'price-low':
        return a.price - b.price;
      case 'price-high':
        return b.price - a.price;
      default:
        return 0;
    }
  });

  const navigate = useNavigate();

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" gutterBottom>
          Saved Events
        </Typography>
        <Typography color="text.secondary">
          Events you've saved for later
        </Typography>
      </Box>

      {/* Search and Sort Controls */}
      <Box sx={{ mb: 4, display: 'flex', gap: 2, alignItems: 'flex-end' }}>
        <TextField
          placeholder="Search saved events..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          sx={{ flexGrow: 1 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start" sx={{ ml: 4 }}>
                <SearchIcon />
              </InputAdornment>
            ),
          }}
        />
        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel>Sort By</InputLabel>
          <Select
            value={sortBy}
            label="Sort By"
            onChange={(e) => setSortBy(e.target.value)}
          >
            <MenuItem value="date">Date</MenuItem>
            <MenuItem value="price-low">Price: Low to High</MenuItem>
            <MenuItem value="price-high">Price: High to Low</MenuItem>
          </Select>
        </FormControl>
      </Box>

      {/* Events Grid */}
      <Grid container spacing={3}>
        {sortedEvents.map((event) => (
          <Grid item xs={12} sm={6} md={4} key={event.id}>
            <Card
              sx={{
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  boxShadow: (theme) => theme.shadows[8],
                }
              }}
            >
              <Box sx={{ position: 'relative' }}>
                <CardMedia
                  component="img"
                  height="220"
                  image={event.imageUrl}
                  alt={event.title}
                  sx={{
                    objectFit: 'cover',
                  }}
                />
                <IconButton
                  sx={{
                    position: 'absolute',
                    top: 8,
                    right: 8,
                    bgcolor: 'background.paper',
                    '&:hover': { bgcolor: 'background.paper' },
                    boxShadow: 1
                  }}
                  onClick={() => handleRemoveFromSaved(event.id)}
                >
                  <FavoriteIcon color="error" />
                </IconButton>
              </Box>

              <CardContent sx={{ flexGrow: 1, p: 2.5 }}>
                <Typography
                  variant="h6"
                  sx={{
                    fontWeight: 600,
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    display: '-webkit-box',
                    WebkitLineClamp: 2,
                    WebkitBoxOrient: 'vertical',
                    lineHeight: 1.3,
                    mb: 2.5,
                    height: 42
                  }}
                >
                  {event.title}
                </Typography>

                <Stack spacing={2.5}>
                  <Stack spacing={1.5}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                      <CalendarTodayIcon fontSize="small" sx={{ color: 'primary.main' }} />
                      <Typography
                        variant="body2"
                        color="text.primary"
                        sx={{
                          fontWeight: 500,
                          lineHeight: 1.4
                        }}
                      >
                        {new Date(event.date).toLocaleDateString('en-US', {
                          weekday: 'long',
                          year: 'numeric',
                          month: 'long',
                          day: 'numeric'
                        })}
                      </Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                      <LocationOnIcon fontSize="small" sx={{ color: 'primary.main' }} />
                      <Typography
                        variant="body2"
                        color="text.primary"
                        sx={{
                          fontWeight: 500,
                          overflow: 'hidden',
                          textOverflow: 'ellipsis',
                          whiteSpace: 'nowrap',
                          lineHeight: 1.4
                        }}
                      >
                        {event.location}
                      </Typography>
                    </Box>
                  </Stack>

                  <Box
                    sx={{
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'space-between',
                      mt: 'auto'
                    }}
                  >
                    <Typography
                      variant="h6"
                      color="primary.main"
                      sx={{
                        fontWeight: 700,
                        fontSize: '1.25rem'
                      }}
                    >
                      ${event.price.toFixed(2)}
                    </Typography>
                    <Chip
                      label={`${event.availableTickets} tickets left`}
                      size="small"
                      color={event.availableTickets < 50 ? 'error' : 'success'}
                      sx={{
                        fontWeight: 500,
                        '& .MuiChip-label': {
                          px: 1.5,
                          py: 0.5
                        }
                      }}
                    />
                  </Box>
                </Stack>
              </CardContent>

              <CardActions
                sx={{
                  justifyContent: 'space-between',
                  px: 2.5,
                  py: 2,
                  borderTop: 1,
                  borderColor: 'divider',
                  mt: 4
                }}
              >
                <Stack direction="row" spacing={1.5}>
                  <IconButton
                    size="small"
                    sx={{
                      bgcolor: 'background.paper',
                      boxShadow: 1,
                      '&:hover': {
                        bgcolor: 'background.paper',
                        boxShadow: 2
                      }
                    }}
                    onClick={() => handleShare(event.id)}
                  >
                    <ShareIcon fontSize="small" />
                  </IconButton>
                  <IconButton
                    size="small"
                    sx={{
                      bgcolor: 'background.paper',
                      boxShadow: 1,
                      '&:hover': {
                        bgcolor: 'background.paper',
                        boxShadow: 2
                      }
                    }}
                    onClick={() => handleAddToCalendar(event.id)}
                  >
                    <CalendarTodayIcon fontSize="small" />
                  </IconButton>
                </Stack>
                <Button
                  variant="contained"
                  onClick={() => navigate(`/events/${event.id}`)}
                  sx={{
                    px: 3,
                    py: 0.75,
                    fontWeight: 600,
                    textTransform: 'none',
                    boxShadow: 'none',
                    '&:hover': {
                      boxShadow: 'none',
                      bgcolor: 'primary.dark'
                    }
                  }}
                >
                  Book Now
                </Button>
              </CardActions>
            </Card>
          </Grid>
        ))}

        {sortedEvents.length === 0 && (
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
                No saved events found
              </Typography>
              <Button variant="contained" onClick={() => navigate('/')}>
                Browse Events
              </Button>
            </Box>
          </Grid>
        )}
      </Grid>
    </Container>
  );
};

export default SavedEvents;
