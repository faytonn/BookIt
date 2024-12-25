import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  TextField,
  Button,
  Grid,
  Paper,
  Alert,
  IconButton,
  MenuItem,
  InputAdornment,
} from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { adminService } from '../../admin/services/adminService';
import { DateTime } from 'luxon';

interface EventPricing {
  id: number;
  type: string;
  price: number;
  availableQuantity: number;
}

interface Event {
  id: number;
  title: string;
  description: string;
  type: string;
  startDate: string;
  endDate: string;
  locationName: string;
  locationAddress: string;
  locationCity: string;
  locationCountry: string;
  imageUrl?: string;
  pricing: EventPricing[];
}

const eventTypes = [
  { value: 'concert', label: 'Concert' },
  { value: 'sports', label: 'Sports' },
  { value: 'conference', label: 'Conference' },
  { value: 'theater', label: 'Theater' },
  { value: 'other', label: 'Other' },
];

const EventEdit = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [eventData, setEventData] = useState<Event>({
    id: 0,
    title: '',
    description: '',
    type: '',
    startDate: DateTime.now().toISO(),
    endDate: DateTime.now().plus({ hours: 2 }).toISO(),
    locationName: '',
    locationAddress: '',
    locationCity: '',
    locationCountry: '',
    imageUrl: '',
    pricing: [{ id: 0, type: '', price: 0, availableQuantity: 0 }]
  });

  useEffect(() => {
    const fetchEvent = async () => {
      if (!id) return;

      try {
        setLoading(true);
        const data = await adminService.getEvent(parseInt(id));
        setEventData(data);
      } catch (err) {
        setError('Failed to fetch event details');
        console.error('Error fetching event:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchEvent();
  }, [id]);

  const handleInputChange = (field: keyof Event, value: any) => {
    setEventData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handlePricingChange = (index: number, field: keyof EventPricing, value: any) => {
    setEventData(prev => ({
      ...prev,
      pricing: prev.pricing.map((p, i) =>
        i === index ? { ...p, [field]: value } : p
      )
    }));
  };

  const addPricing = () => {
    setEventData(prev => ({
      ...prev,
      pricing: [...prev.pricing, { id: 0, type: '', price: 0, availableQuantity: 0 }]
    }));
  };

  const removePricing = (index: number) => {
    setEventData(prev => ({
      ...prev,
      pricing: prev.pricing.filter((_, i) => i !== index)
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setLoading(true);
      setError(null);

      const eventUpdate = {
        title: eventData.title,
        description: eventData.description,
        type: eventData.type,
        startDate: eventData.startDate,
        endDate: eventData.endDate,
        locationName: eventData.locationName,
        locationAddress: eventData.locationAddress,
        locationCity: eventData.locationCity,
        locationCountry: eventData.locationCountry,
        imageUrl: eventData.imageUrl,
        pricing: eventData.pricing.map(p => ({
          id: p.id,
          category: p.type,
          price: p.price,
          totalSeats: p.availableQuantity,
          availableSeats: p.availableQuantity
        })),
        tags: []
      };

      await adminService.updateEvent(parseInt(id!), eventUpdate);
      navigate('/admin/events');
    } catch (err) {
      setError('Failed to update event. Please try again.');
      console.error('Error updating event:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading && !eventData.id) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h4" gutterBottom>
          Edit Event
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Title"
                value={eventData.title}
                onChange={(e) => handleInputChange('title', e.target.value)}
                required
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Description"
                value={eventData.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
                multiline
                rows={4}
                required
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                select
                label="Event Type"
                value={eventData.type}
                onChange={(e) => handleInputChange('type', e.target.value)}
                required
              >
                {eventTypes.map((option) => (
                  <MenuItem key={option.value} value={option.value}>
                    {option.label}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>

            <Grid item xs={12} sm={6}>
              <DateTimePicker
                label="Start Date"
                value={DateTime.fromISO(eventData.startDate)}
                onChange={(newValue) => handleInputChange('startDate', newValue?.toISO())}
                sx={{ width: '100%' }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <DateTimePicker
                label="End Date"
                value={DateTime.fromISO(eventData.endDate)}
                onChange={(newValue) => handleInputChange('endDate', newValue?.toISO())}
                sx={{ width: '100%' }}
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Location Name"
                value={eventData.locationName}
                onChange={(e) => handleInputChange('locationName', e.target.value)}
                required
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Address"
                value={eventData.locationAddress}
                onChange={(e) => handleInputChange('locationAddress', e.target.value)}
                required
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="City"
                value={eventData.locationCity}
                onChange={(e) => handleInputChange('locationCity', e.target.value)}
                required
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Country"
                value={eventData.locationCountry}
                onChange={(e) => handleInputChange('locationCountry', e.target.value)}
                required
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Image URL"
                value={eventData.imageUrl}
                onChange={(e) => handleInputChange('imageUrl', e.target.value)}
              />
            </Grid>

            <Grid item xs={12}>
              <Box sx={{ mb: 2, display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <Typography variant="h6">Pricing Options</Typography>
                <Button
                  startIcon={<AddIcon />}
                  onClick={addPricing}
                  variant="outlined"
                  size="small"
                >
                  Add Pricing
                </Button>
              </Box>

              {eventData.pricing.map((price, index) => (
                <Box key={index} sx={{ mb: 2, p: 2, border: '1px solid #e0e0e0', borderRadius: 1 }}>
                  <Grid container spacing={2} alignItems="flex-end">
                    <Grid item xs={12} sm={3}>
                      <TextField
                        fullWidth
                        label="Type"
                        value={price.type}
                        onChange={(e) => handlePricingChange(index, 'type', e.target.value)}
                        required
                      />
                    </Grid>
                    <Grid item xs={12} sm={3}>
                      <TextField
                        fullWidth
                        label="Price"
                        type="number"
                        value={price.price}
                        onChange={(e) => handlePricingChange(index, 'price', parseFloat(e.target.value))}
                        InputProps={{
                          startAdornment: <InputAdornment position="start" sx={{ ml: 4 }}>$</InputAdornment>,
                        }}
                        required
                      />
                    </Grid>
                    <Grid item xs={12} sm={3}>
                      <TextField
                        fullWidth
                        label="Available Quantity"
                        type="number"
                        value={price.availableQuantity}
                        onChange={(e) => handlePricingChange(index, 'availableQuantity', parseInt(e.target.value))}
                        required
                      />
                    </Grid>
                    <Grid item xs={12} sm={3}>
                      <IconButton
                        onClick={() => removePricing(index)}
                        color="error"
                        disabled={eventData.pricing.length === 1}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </Grid>
                  </Grid>
                </Box>
              ))}
            </Grid>

            <Grid item xs={12}>
              <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                <Button
                  variant="outlined"
                  onClick={() => navigate('/admin/events')}
                  disabled={loading}
                >
                  Cancel
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  disabled={loading}
                >
                  Save Changes
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Container>
  );
};

export default EventEdit;
