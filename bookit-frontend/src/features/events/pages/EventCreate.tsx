import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import {
  Box,
  Container,
  Typography,
  TextField,
  Button,
  Grid,
  MenuItem,
  Card,
  Stack,
  InputAdornment,
  IconButton,
  Avatar,
  Alert,
} from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import DeleteIcon from '@mui/icons-material/Delete';
import { CreateEventData } from '../types';
import { eventService } from '../services/eventService';
import paths from '../../../routes/paths';

const eventTypes = [
  { value: 'concert', label: 'Concert' },
  { value: 'conference', label: 'Conference' },
  { value: 'exhibition', label: 'Exhibition' },
  { value: 'sport', label: 'Sport' },
  { value: 'theater', label: 'Theater' },
  { value: 'other', label: 'Other' },
];

const validationSchema = Yup.object({
  title: Yup.string().required('Title is required'),
  description: Yup.string().required('Description is required'),
  type: Yup.string().required('Event type is required'),
  startDate: Yup.date().nullable().required('Start date is required'),
  endDate: Yup.date()
    .nullable()
    .required('End date is required')
    .test('is-after-start', 'End date must be after start date', function (value) {
      const { startDate } = this.parent;
      if (!startDate || !value) return true;
      return new Date(value) > new Date(startDate);
    }),
  location: Yup.object({
    name: Yup.string().required('Venue name is required'),
    address: Yup.string().required('Address is required'),
    city: Yup.string().required('City is required'),
    country: Yup.string().required('Country is required'),
  }),
  pricing: Yup.array().of(
    Yup.object({
      category: Yup.string().required('Category is required'),
      price: Yup.number().min(0, 'Price must be positive').required('Price is required'),
      totalSeats: Yup.number().min(1, 'Must have at least 1 seat').required('Total seats is required'),
    })
  ),
});

const EventCreate = () => {
  const navigate = useNavigate();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const formik = useFormik<CreateEventData>({
    initialValues: {
      title: '',
      description: '',
      type: 'concert',
      startDate: null,
      endDate: null,
      location: {
        name: '',
        address: '',
        city: '',
        country: '',
      },
      images: [],
      pricing: [{
        id: 1,
        category: 'General Admission',
        price: 0,
        totalSeats: 0,
      }],
      tags: [],
    },
    validationSchema,
    onSubmit: async (values) => {
      console.log(values);
      setIsSubmitting(true);
      setError(null);
      try {
        await eventService.createEvent(values);
        navigate(paths.events.root);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to create event');
      } finally {
        setIsSubmitting(false);
      }
    },
  });

  const handleImageUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (files) {
      const filesArray = Array.from(files);
      formik.setFieldValue('images', filesArray);
    }
  };

  const handleDeleteTicket = (index: number) => {
    const newPricing = [...formik.values.pricing];
    newPricing.splice(index, 1);
    formik.setFieldValue('pricing', newPricing);
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Create New Event
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      <form onSubmit={formik.handleSubmit}>
        <Stack spacing={4}>
          {/* Basic Information */}
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Basic Information
            </Typography>
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  name="title"
                  label="Event Title"
                  value={formik.values.title}
                  onChange={formik.handleChange}
                  error={formik.touched.title && Boolean(formik.errors.title)}
                  helperText={formik.touched.title && formik.errors.title}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  multiline
                  rows={4}
                  name="description"
                  label="Event Description"
                  value={formik.values.description}
                  onChange={formik.handleChange}
                  error={formik.touched.description && Boolean(formik.errors.description)}
                  helperText={formik.touched.description && formik.errors.description}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  select
                  name="type"
                  label="Event Type"
                  value={formik.values.type}
                  onChange={formik.handleChange}
                  error={formik.touched.type && Boolean(formik.errors.type)}
                  helperText={formik.touched.type && formik.errors.type}
                >
                  {eventTypes.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                      {option.label}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
            </Grid>
          </Card>

          {/* Date and Time */}
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Date and Time
            </Typography>
            <Grid container spacing={3}>
              <Grid item xs={12} sm={6}>
                <DateTimePicker
                  label="Start Date"
                  value={formik.values.startDate ? formik.values.startDate : null}
                  onChange={(value) => {
                    formik.setFieldValue('startDate', value);
                  }}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      error: formik.touched.startDate && Boolean(formik.errors.startDate),
                      helperText: formik.touched.startDate && formik.errors.startDate
                    }
                  }}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <DateTimePicker
                  label="End Date"
                  value={formik.values.endDate ? formik.values.endDate : null}
                  onChange={(value) => { formik.setFieldValue('endDate', value); }}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      error: formik.touched.endDate && Boolean(formik.errors.endDate),
                      helperText: formik.touched.endDate && formik.errors.endDate
                    }
                  }}
                />
              </Grid>
            </Grid>
          </Card>

          {/* Location */}
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Location
            </Typography>
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  name="location.name"
                  label="Venue Name"
                  value={formik.values.location.name}
                  onChange={formik.handleChange}
                  error={formik.touched.location && formik.touched.location.name && Boolean(formik.errors.location?.name)}
                  helperText={formik.touched.location && formik.touched.location.name && formik.errors.location?.name}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  name="location.address"
                  label="Address"
                  value={formik.values.location.address}
                  onChange={formik.handleChange}
                  error={formik.touched.location && formik.touched.location.address && Boolean(formik.errors.location?.address)}
                  helperText={formik.touched.location && formik.touched.location.address && formik.errors.location?.address}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  name="location.city"
                  label="City"
                  value={formik.values.location.city}
                  onChange={formik.handleChange}
                  error={formik.touched.location && formik.touched.location.city && Boolean(formik.errors.location?.city)}
                  helperText={formik.touched.location && formik.touched.location.city && formik.errors.location?.city}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  name="location.country"
                  label="Country"
                  value={formik.values.location.country}
                  onChange={formik.handleChange}
                  error={formik.touched.location && formik.touched.location.country && Boolean(formik.errors.location?.country)}
                  helperText={formik.touched.location && formik.touched.location.country && formik.errors.location?.country}
                />
              </Grid>
            </Grid>
          </Card>

          {/* Pricing */}
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Pricing
            </Typography>
            {formik.values.pricing.map((price, index) => (
              <Grid container spacing={2} key={index} alignItems="center">
                <Grid item xs={12} sm={4}>
                  <TextField
                    fullWidth
                    name={`pricing.${index}.category`}
                    label="Ticket Category"
                    value={price.category}
                    onChange={formik.handleChange}
                    error={
                      Boolean(
                        formik.touched.pricing?.[index] &&
                        typeof formik.errors.pricing?.[index] === 'object' &&
                        (formik.errors.pricing[index] as any)?.category
                      )
                    }
                    helperText={
                      formik.touched.pricing?.[index] &&
                      typeof formik.errors.pricing?.[index] === 'object' &&
                      (formik.errors.pricing[index] as any)?.category
                    }
                  />
                </Grid>
                <Grid item xs={12} sm={3}>
                  <TextField
                    fullWidth
                    type="number"
                    name={`pricing.${index}.price`}
                    label="Price"
                    value={price.price}
                    onChange={formik.handleChange}
                    error={
                      Boolean(
                        formik.touched.pricing?.[index] &&
                        typeof formik.errors.pricing?.[index] === 'object' &&
                        (formik.errors.pricing[index] as any)?.price
                      )
                    }
                    helperText={
                      formik.touched.pricing?.[index] &&
                      typeof formik.errors.pricing?.[index] === 'object' &&
                      (formik.errors.pricing[index] as any)?.price
                    }
                    InputProps={{
                      startAdornment: <InputAdornment position="start" sx={{ ml: 4 }}>$</InputAdornment>,
                    }}
                  />
                </Grid>
                <Grid item xs={12} sm={3}>
                  <TextField
                    fullWidth
                    type="number"
                    name={`pricing.${index}.totalSeats`}
                    label="Total Seats"
                    value={price.totalSeats}
                    onChange={formik.handleChange}
                    error={
                      Boolean(
                        formik.touched.pricing?.[index] &&
                        typeof formik.errors.pricing?.[index] === 'object' &&
                        (formik.errors.pricing[index] as any)?.totalSeats
                      )
                    }
                    helperText={
                      formik.touched.pricing?.[index] &&
                      typeof formik.errors.pricing?.[index] === 'object' &&
                      (formik.errors.pricing[index] as any)?.totalSeats
                    }
                  />
                </Grid>
                <Grid item xs={12} sm={2}>
                  <IconButton
                    color="error"
                    onClick={() => handleDeleteTicket(index)}
                    disabled={formik.values.pricing.length === 1}
                  >
                    <DeleteIcon />
                  </IconButton>
                </Grid>
              </Grid>
            ))}
            <Button
              sx={{ mt: 6 }}
              size="small"
              onClick={() =>
                formik.setFieldValue('pricing', [
                  ...formik.values.pricing,
                  { category: '', price: 0, totalSeats: 0 },
                ])
              }
            >
              Add Ticket Category
            </Button>
          </Card>

          {/* Images */}
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Event Images
            </Typography>
            <Stack direction="row" spacing={2} alignItems="center">
              <Button variant="outlined" component="label">
                Upload Images
                <input
                  type="file"
                  hidden
                  accept="image/*"
                  onChange={handleImageUpload}
                />
              </Button>
              {formik.values.images.length > 0 && (
                <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
                  {Array.from(formik.values.images).map((image, index) => (
                    <Avatar
                      key={index}
                      src={URL.createObjectURL(image)}
                      alt={`Event image ${index + 1}`}
                      sx={{
                        width: 100,
                        height: 100,
                        border: '2px solid',
                        borderColor: 'primary.main'
                      }}
                    />
                  ))}
                </Box>
              )}
            </Stack>
          </Card>

          {/* Submit Button */}
          <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button
              variant="outlined"
              onClick={() => navigate(paths.events.root)}
            >
              Cancel
            </Button>
            <Button
              type="submit"
              variant="contained"
              disabled={isSubmitting}
              sx={{ minWidth: 120 }}
            >
              {isSubmitting ? 'Creating...' : 'Create Event'}
            </Button>
          </Box>
        </Stack>
      </form>
    </Container>
  );
};

export default EventCreate;
