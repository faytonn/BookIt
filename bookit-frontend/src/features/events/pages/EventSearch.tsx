import { useState } from 'react';
import {
  Box,
  Container,
  TextField,
  Button,
  Grid,
  Card,
  Typography,
  MenuItem,
  Chip,
  Stack,
  InputAdornment,
  Pagination,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import SearchIcon from '@mui/icons-material/Search';
import FilterListIcon from '@mui/icons-material/FilterList';
import ClearIcon from '@mui/icons-material/Clear';
import EventCard from '../components/EventCard';
import { Event } from '../types';

const eventTypes = [
  { value: 'all', label: 'All Types' },
  { value: 'concert', label: 'Concert' },
  { value: 'sports', label: 'Sports' },
  { value: 'conference', label: 'Conference' },
  { value: 'theater', label: 'Theater' },
  { value: 'other', label: 'Other' },
];

const priceRanges = [
  { value: 'all', label: 'All Prices' },
  { value: '0-50', label: 'Under $50' },
  { value: '50-100', label: '$50 - $100' },
  { value: '100-200', label: '$100 - $200' },
  { value: '200+', label: '$200+' },
];

interface SearchFilters {
  keyword: string;
  type: string;
  priceRange: string;
  startDate: Date | null;
  endDate: Date | null;
  location: string;
}

const EventSearch = () => {
  const [showFilters, setShowFilters] = useState(false);
  const [filters, setFilters] = useState<SearchFilters>({
    keyword: '',
    type: 'all',
    priceRange: 'all',
    startDate: null,
    endDate: null,
    location: '',
  });
  const [page, setPage] = useState(1);
  const [events, setEvents] = useState<Event[]>([]);
  const [loading, setLoading] = useState(false);

  const handleFilterChange = (field: keyof SearchFilters, value: any) => {
    setFilters(prev => ({
      ...prev,
      [field]: value,
    }));
  };

  const clearFilters = () => {
    setFilters({
      keyword: '',
      type: 'all',
      priceRange: 'all',
      startDate: null,
      endDate: null,
      location: '',
    });
  };

  const handleSearch = async () => {
    setLoading(true);
    try {
      // Replace with actual API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      // Mock events data
      const mockEvents: Event[] = [
        {
          id: '1',
          title: 'Summer Music Festival',
          description: 'A fantastic summer music festival',
          type: 'concert',
          startDate: '2024-07-15T18:00:00',
          endDate: '2024-07-15T23:00:00',
          location: {
            name: 'Central Park',
            address: '123 Park Avenue',
            city: 'New York',
            country: 'USA',
          },
          organizer: {
            id: '1',
            name: 'Event Masters',
            email: 'contact@eventmasters.com',
          },
          images: [],
          pricing: [
            {
              category: 'General Admission',
              price: 50,
              availableSeats: 1000,
              totalSeats: 1000,
            },
          ],
          status: 'upcoming',
          tags: ['music', 'festival', 'summer'],
          createdAt: '2024-03-15T10:00:00',
          updatedAt: '2024-03-15T10:00:00'
        },
        // Add more mock events as needed
      ];
      setEvents(mockEvents);
    } catch (error) {
      console.error('Failed to search events:', error);
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
    handleSearch();
  };

  const getActiveFiltersCount = () => {
    let count = 0;
    if (filters.type !== 'all') count++;
    if (filters.priceRange !== 'all') count++;
    if (filters.startDate) count++;
    if (filters.endDate) count++;
    if (filters.location) count++;
    return count;
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Search Events
      </Typography>

      {/* Search Bar */}
      <Card sx={{ p: 3, mb: 4 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs>
            <TextField
              fullWidth
              placeholder="Search events..."
              value={filters.keyword}
              onChange={(e) => handleFilterChange('keyword', e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start" sx={{ ml: 4 }}>
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          <Grid item>
            <Button
              variant="outlined"
              size="small"
              startIcon={<FilterListIcon />}
              onClick={() => setShowFilters(!showFilters)}
              sx={{ mr: 1 }}
            >
              Filters {getActiveFiltersCount() > 0 && `(${getActiveFiltersCount()})`}
            </Button>
            <Button
              variant="contained"
              size="small"
              onClick={handleSearch}
            >
              Search
            </Button>
          </Grid>
        </Grid>

        {/* Advanced Filters */}
        {showFilters && (
          <Box sx={{ mt: 3 }}>
            <Grid container spacing={3}>
              <Grid item xs={12} sm={6} md={3}>
                <TextField
                  fullWidth
                  select
                  label="Event Type"
                  value={filters.type}
                  onChange={(e) => handleFilterChange('type', e.target.value)}
                >
                  {eventTypes.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                      {option.label}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
              <Grid item xs={12} sm={6} md={3}>
                <TextField
                  fullWidth
                  select
                  label="Price Range"
                  value={filters.priceRange}
                  onChange={(e) => handleFilterChange('priceRange', e.target.value)}
                >
                  {priceRanges.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                      {option.label}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
              <Grid item xs={12} sm={6} md={3}>
                <DatePicker
                  label="Start Date"
                  value={filters.startDate}
                  onChange={(date) => handleFilterChange('startDate', date)}
                  slotProps={{ textField: { fullWidth: true } }}
                />
              </Grid>
              <Grid item xs={12} sm={6} md={3}>
                <DatePicker
                  label="End Date"
                  value={filters.endDate}
                  onChange={(date) => handleFilterChange('endDate', date)}
                  slotProps={{ textField: { fullWidth: true } }}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Location"
                  value={filters.location}
                  onChange={(e) => handleFilterChange('location', e.target.value)}
                  placeholder="City, Country"
                />
              </Grid>
            </Grid>
            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'flex-end' }}>
              <Button
                variant="text"
                onClick={clearFilters}
                startIcon={<ClearIcon />}
              >
                Clear Filters
              </Button>
            </Box>
          </Box>
        )}
      </Card>

      {/* Active Filters */}
      {getActiveFiltersCount() > 0 && (
        <Stack direction="row" spacing={1} sx={{ mb: 3 }}>
          {filters.type !== 'all' && (
            <Chip
              label={`Type: ${filters.type}`}
              onDelete={() => handleFilterChange('type', 'all')}
            />
          )}
          {filters.priceRange !== 'all' && (
            <Chip
              label={`Price: ${filters.priceRange}`}
              onDelete={() => handleFilterChange('priceRange', 'all')}
            />
          )}
          {filters.startDate && (
            <Chip
              label={`From: ${filters.startDate.toLocaleDateString()}`}
              onDelete={() => handleFilterChange('startDate', null)}
            />
          )}
          {filters.endDate && (
            <Chip
              label={`To: ${filters.endDate.toLocaleDateString()}`}
              onDelete={() => handleFilterChange('endDate', null)}
            />
          )}
          {filters.location && (
            <Chip
              label={`Location: ${filters.location}`}
              onDelete={() => handleFilterChange('location', '')}
            />
          )}
        </Stack>
      )}

      {/* Search Results */}
      <Grid container spacing={3}>
        {events.map((event) => (
          <Grid item key={event.id} xs={12} sm={6} md={4}>
            <EventCard event={event} />
          </Grid>
        ))}
      </Grid>

      {/* Pagination */}
      {events.length > 0 && (
        <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
          <Pagination
            count={10}
            page={page}
            onChange={handlePageChange}
            color="primary"
          />
        </Box>
      )}

      {/* No Results */}
      {!loading && events.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">
            No events found
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Try adjusting your search filters
          </Typography>
        </Box>
      )}
    </Container>
  );
};

export default EventSearch;
