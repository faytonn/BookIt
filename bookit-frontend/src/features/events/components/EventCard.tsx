import { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Card,
  CardContent,
  CardMedia,
  Typography,
  Box,
  Chip,
  Button,
  Stack,
} from '@mui/material';
import { Event } from '../types';
import paths from '../../../routes/paths';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import ConfirmationNumberIcon from '@mui/icons-material/ConfirmationNumber';

interface EventCardProps {
  event: Event;
}

const EventCard: FC<EventCardProps> = ({ event }) => {
  const navigate = useNavigate();

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });
  };

  const getLowestPrice = () => {
    if (!event.pricing.length) return 'Free';
    const lowestPrice = Math.min(...event.pricing.map(p => p.price));
    return lowestPrice === 0 ? 'Free' : `From $${lowestPrice}`;
  };

  const getAvailableSeats = () => {
    return event.pricing.reduce((total, p) => total + p.availableSeats, 0);
  };

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        '&:hover': {
          transform: 'translateY(-4px)',
          transition: 'transform 0.2s ease-in-out',
          boxShadow: 4,
        },
      }}
    >
      <CardMedia
        component="img"
        height="200"
        image={event.images[0]?.url || '/event_ticket.png'}
        alt={event.title}
        sx={{ objectFit: 'cover' }}
      />
      <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
        <Box sx={{ mb: 2 }}>
          <Chip
            label={event.type}
            size="small"
            color="primary"
            sx={{ mb: 1 }}
          />
          <Typography
            gutterBottom
            variant="h6"
            component="h2"
            sx={{
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical',
              minHeight: '3.6em',
            }}
          >
            {event.title}
          </Typography>
        </Box>

        <Stack spacing={1} sx={{ mb: 2 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <CalendarTodayIcon fontSize="small" color="action" />
            <Typography variant="body2" color="text.secondary">
              {formatDate(event.startDate)}
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <LocationOnIcon fontSize="small" color="action" />
            <Typography
              variant="body2"
              color="text.secondary"
              sx={{
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
              }}
            >
              {event.location.city}, {event.location.country}
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <ConfirmationNumberIcon fontSize="small" color="action" />
            <Typography variant="body2" color="text.secondary">
              {getAvailableSeats()} seats available
            </Typography>
          </Box>
        </Stack>

        <Box
          sx={{
            mt: 'auto',
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
          }}
        >
          <Typography variant="subtitle1" color="primary" fontWeight="bold">
            {getLowestPrice()}
          </Typography>
          <Button
            size="small"
            variant="contained"
            onClick={() => navigate(paths.events.details.replace(':id', event.id))}
          >
            View Details
          </Button>
        </Box>
      </CardContent>
    </Card>
  );
};

export default EventCard;
