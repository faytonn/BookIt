import { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  IconButton,
  InputAdornment,
  Menu,
  MenuItem,
  TablePagination,
  FormControl,
  InputLabel,
  Select,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  Button,
  Chip,
} from '@mui/material';
import { DateTime } from 'luxon';
import SearchIcon from '@mui/icons-material/Search';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AddIcon from '@mui/icons-material/Add';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/adminService';

interface Event {
  id: number;
  title: string;
  description: string;
  type: string;
  startDate: string;
  endDate: string;
  locationName: string;
  locationCity: string;
  organizer: {
    name: string;
    email: string;
  };
  pricing: {
    id: number;
    type: string;
    price: number;
    availableQuantity: number;
  }[];
  status?: 'upcoming' | 'ongoing' | 'past';
}

const EventManagement = () => {
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState<string>('');
  const [events, setEvents] = useState<Event[]>([]);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);
  const [menuAnchor, setMenuAnchor] = useState<null | HTMLElement>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const navigate = useNavigate();

  const fetchEvents = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await adminService.getEvents(searchQuery, statusFilter);
      setEvents(data);
    } catch (err) {
      setError('Failed to fetch events. Please try again.');
      console.error('Error fetching events:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchEvents();
  }, [searchQuery, statusFilter]);

  const handleChangePage = (_: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>, eventItem: Event) => {
    setSelectedEvent(eventItem);
    setMenuAnchor(event.currentTarget);
  };

  const handleMenuClose = () => {
    setMenuAnchor(null);
  };

  const handleDeleteClick = () => {
    setDeleteDialogOpen(true);
    handleMenuClose();
  };

  const handleDeleteCancel = () => {
    setDeleteDialogOpen(false);
    setSelectedEvent(null);
  };

  const handleDeleteConfirm = async () => {
    if (!selectedEvent) return;

    try {
      setLoading(true);
      await adminService.deleteEvent(selectedEvent.id);
      await fetchEvents();
      setDeleteDialogOpen(false);
      setSelectedEvent(null);
    } catch (err) {
      setError('Failed to delete event. Please try again.');
      console.error('Error deleting event:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleEditClick = () => {
    if (selectedEvent) {
      navigate(`/events/${selectedEvent.id}/edit`);
    }
    handleMenuClose();
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'upcoming':
        return 'primary';
      case 'ongoing':
        return 'success';
      case 'past':
        return 'default';
      default:
        return 'default';
    }
  };

  const getEventStatus = (event: Event): 'upcoming' | 'ongoing' | 'past' => {
    const now = DateTime.now();
    const startDate = DateTime.fromISO(event.startDate);
    const endDate = DateTime.fromISO(event.endDate);

    if (startDate > now) return 'upcoming';
    if (endDate < now) return 'past';
    return 'ongoing';
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h4">Event Management</Typography>
        <Button
          startIcon={<AddIcon />}
          variant="contained"
          onClick={() => navigate('/admin/events/create')}
        >
          Create Event
        </Button>
      </Box>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Card>
        <Box sx={{ p: 2, display: 'flex', gap: 2, alignItems: 'flex-end' }}>
          <TextField
            size="small"
            placeholder="Search events..."
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
          <FormControl size="small" sx={{ minWidth: 120 }}>
            <InputLabel>Status</InputLabel>
            <Select
              value={statusFilter}
              label="Status"
              onChange={(e) => setStatusFilter(e.target.value)}
            >
              <MenuItem value="">All</MenuItem>
              <MenuItem value="upcoming">Upcoming</MenuItem>
              <MenuItem value="ongoing">Ongoing</MenuItem>
              <MenuItem value="past">Past</MenuItem>
            </Select>
          </FormControl>
        </Box>

        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Title</TableCell>
                <TableCell>Organizer</TableCell>
                <TableCell>Type</TableCell>
                <TableCell>Start Date</TableCell>
                <TableCell>Location</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {events
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                .map((event) => {
                  const status = getEventStatus(event);
                  return (
                    <TableRow key={event.id}>
                      <TableCell>{event.title}</TableCell>
                      <TableCell>{event.organizer.name}</TableCell>
                      <TableCell>{event.type}</TableCell>
                      <TableCell>
                        {DateTime.fromISO(event.startDate).toLocaleString(DateTime.DATETIME_SHORT)}
                      </TableCell>
                      <TableCell>{event.locationCity}</TableCell>
                      <TableCell>
                        <Chip
                          label={status}
                          color={getStatusColor(status)}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        <IconButton onClick={(e) => handleMenuOpen(e, event)}>
                          <MoreVertIcon />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  );
                })}
            </TableBody>
          </Table>
        </TableContainer>

        <TablePagination
          rowsPerPageOptions={[5, 10, 25]}
          component="div"
          count={events.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Card>

      <Menu
        anchorEl={menuAnchor}
        open={Boolean(menuAnchor)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleEditClick}>Edit Event</MenuItem>
        <MenuItem onClick={handleDeleteClick} sx={{ color: 'error.main' }}>
          Delete
        </MenuItem>
      </Menu>

      {/* Delete Confirmation Dialog */}
      <Dialog
        open={deleteDialogOpen}
        onClose={handleDeleteCancel}
        aria-labelledby="delete-dialog-title"
        aria-describedby="delete-dialog-description"
      >
        <DialogTitle id="delete-dialog-title">
          Confirm Delete Event
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="delete-dialog-description">
            Are you sure you want to delete the event "{selectedEvent?.title}"? This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel} color="primary">
            Cancel
          </Button>
          <Button onClick={handleDeleteConfirm} color="error" variant="contained" autoFocus>
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default EventManagement;
