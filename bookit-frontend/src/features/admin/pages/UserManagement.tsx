import { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Button,
  Box,
  Card,
  TextField,
  InputAdornment,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Chip,
  Menu,
  MenuItem,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Stack,
  TablePagination,
  Avatar,
  FormControl,
  InputLabel,
  Select,
  ListItemIcon,
  CircularProgress,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AddIcon from '@mui/icons-material/Add';
import FilterListIcon from '@mui/icons-material/FilterList';
import EditIcon from '@mui/icons-material/Edit';
import BlockIcon from '@mui/icons-material/Block';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import DeleteIcon from '@mui/icons-material/Delete';
import { useNavigate } from 'react-router-dom';
import paths from 'routes/paths';
import { toast } from 'react-toastify';
import { adminService, User } from '../services/adminService';

const UserManagement = () => {
  const navigate = useNavigate();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [menuAnchor, setMenuAnchor] = useState<null | HTMLElement>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [deleteError, setDeleteError] = useState<string | null>(null);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const data = await adminService.getUsers(searchQuery);
      setUsers(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch users');
      toast.error('Failed to fetch users');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateUserStatus = async (userId: string, newStatus: string) => {
    try {
      await adminService.updateUserStatus(userId, newStatus);
      toast.success('User status updated successfully');
      fetchUsers();
    } catch (err) {
      toast.error('Failed to update user status');
    }
  };

  const handleUpdateUserRole = async (userId: string, newRole: string) => {
    try {
      await adminService.updateUserRole(userId, newRole);
      toast.success('User role updated successfully');
      fetchUsers();
    } catch (err) {
      toast.error('Failed to update user role');
    }
  };

  const handleDeleteUser = async (userId: string) => {
    try {
      setDeleteError(null);
      await adminService.deleteUser(userId);
      toast.success('User deleted successfully');
      fetchUsers();
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || 'Failed to delete user';
      setDeleteError(errorMessage);
      if (!err.response?.data?.message) {
        toast.error(errorMessage);
      }
    }
  };

  const handleChangePage = (_: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>, user: User) => {
    setSelectedUser(user);
    setMenuAnchor(event.currentTarget);
  };

  const handleMenuClose = () => {
    setMenuAnchor(null);
  };

  const handleEditClick = () => {
    handleMenuClose();
    setDialogOpen(true);
  };

  const handleDialogClose = () => {
    setDialogOpen(false);
    setSelectedUser(null);
  };

  const handleDeleteDialogClose = () => {
    setDeleteDialogOpen(false);
    setSelectedUser(null);
  };

  const handleConfirmDelete = async () => {
    if (selectedUser) {
      await handleDeleteUser(selectedUser.id);
    }
    handleDeleteDialogClose();
    handleMenuClose();
  };

  const getRoleColor = (role: string) => {
    switch (role) {
      case 'admin':
        return 'error';
      case 'organizer':
        return 'primary';
      default:
        return 'default';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active':
        return 'success';
      case 'inactive':
        return 'warning';
      case 'pending':
        return 'info';
      default:
        return 'default';
    }
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      {/* Header */}
      <Box sx={{ mb: 4, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h4">User Management</Typography>
        <Button
          startIcon={<AddIcon />}
          variant="contained"
          onClick={() => navigate(paths.admin.createUser)}
        >
          Add User
        </Button>
      </Box>

      <Card>
        {/* Search and Filters */}
        <Box sx={{ p: 2, display: 'flex', gap: 2, alignItems: "center" }}>
          <TextField
            size="small"
            placeholder="Search users..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onKeyUp={(e) => {
              if (e.key === 'Enter') {
                fetchUsers();
              }
            }}
            sx={{ flexGrow: 1 }}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start" sx={{ ml: 4 }}>
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
          />
          <Button startIcon={<FilterListIcon />} size="small">
            Filters
          </Button>
        </Box>

        {/* Users Table */}
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Role</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Join Date</TableCell>
                <TableCell>Email Verified</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {loading ? (
                <TableRow>
                  <TableCell colSpan={7} align="center">
                    <Box sx={{ py: 3 }}>
                      <CircularProgress />
                    </Box>
                  </TableCell>
                </TableRow>
              ) : error ? (
                <TableRow>
                  <TableCell colSpan={7} align="center">
                    <Typography color="error">{error}</Typography>
                  </TableCell>
                </TableRow>
              ) : (
                users
                  .filter(user =>
                    searchQuery
                      ? user.firstName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                      user.lastName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                      user.email.toLowerCase().includes(searchQuery.toLowerCase())
                      : true
                  )
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map((user) => (
                    <TableRow key={user.id}>
                      <TableCell>
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                          {user.profileImage ? (
                            <Avatar src={user.profileImage} />
                          ) : (
                            <Avatar>{`${user.firstName[0]}${user.lastName[0]}`}</Avatar>
                          )}
                          <Typography>{`${user.firstName} ${user.lastName}`}</Typography>
                        </Box>
                      </TableCell>
                      <TableCell>{user.email}</TableCell>
                      <TableCell>
                        <Chip
                          label={user.role}
                          color={getRoleColor(user.role) as any}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={user.status}
                          color={getStatusColor(user.status) as any}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        {new Date(user.createdAt).toLocaleDateString()}
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={user.isEmailVerified ? 'Verified' : 'Not Verified'}
                          color={user.isEmailVerified ? 'success' : 'warning'}
                          size="small"
                        />
                      </TableCell>
                      <TableCell align="right">
                        <IconButton
                          size="small"
                          onClick={(event) => handleMenuOpen(event, user)}
                        >
                          <MoreVertIcon />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  ))
              )}
            </TableBody>
          </Table>
        </TableContainer>

        {/* Pagination */}
        <TablePagination
          component="div"
          count={users.length}
          page={page}
          onPageChange={handleChangePage}
          rowsPerPage={rowsPerPage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Card>

      {/* Actions Menu */}
      <Menu
        anchorEl={menuAnchor}
        open={Boolean(menuAnchor)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleEditClick}>
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>
          Edit
        </MenuItem>
        {/* <MenuItem onClick={() => {
          if (selectedUser) {
            handleUpdateUserStatus(
              selectedUser.id,
              selectedUser.status === 'active' ? 'inactive' : 'active'
            );
          }
          handleMenuClose();
        }}>
          <ListItemIcon>
            {selectedUser?.status === 'active' ? (
              <BlockIcon fontSize="small" color="error" />
            ) : (
              <CheckCircleIcon fontSize="small" color="success" />
            )}
          </ListItemIcon>
          {selectedUser?.status === 'active' ? 'Deactivate' : 'Activate'}
        </MenuItem> */}
        <MenuItem
          onClick={() => {
            setDeleteDialogOpen(true);
            handleMenuClose();
          }}
          sx={{ color: 'error.main' }}
        >
          <ListItemIcon>
            <DeleteIcon fontSize="small" color="error" />
          </ListItemIcon>
          Delete
        </MenuItem>
      </Menu>

      {/* Delete Confirmation Dialog */}
      <Dialog 
        open={deleteDialogOpen} 
        onClose={handleDeleteDialogClose}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>Delete User</DialogTitle>
        <DialogContent>
          {deleteError ? (
            <Typography color="error" sx={{ mb: 2 }}>
              {deleteError}
            </Typography>
          ) : (
            <Typography>
              Are you sure you want to delete user{' '}
              <strong>{selectedUser ? `${selectedUser.firstName} ${selectedUser.lastName}` : ''}</strong>?
              This action cannot be undone.
            </Typography>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteDialogClose}>
            {deleteError ? 'Close' : 'Cancel'}
          </Button>
          {!deleteError && (
            <Button
              onClick={handleConfirmDelete}
              color="error"
              variant="contained"
            >
              Delete
            </Button>
          )}
        </DialogActions>
      </Dialog>

      {/* Edit Dialog */}
      <Dialog open={dialogOpen} onClose={handleDialogClose} maxWidth="sm" fullWidth>
        <DialogTitle>Edit User</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 2 }}>
            <TextField
              label="First Name"
              value={selectedUser?.firstName || ''}
              fullWidth
              size="small"
            />
            <TextField
              label="Last Name"
              value={selectedUser?.lastName || ''}
              fullWidth
              size="small"
            />
            <TextField
              label="Email"
              value={selectedUser?.email || ''}
              fullWidth
              size="small"
              disabled
            />
            <FormControl fullWidth size="small">
              <InputLabel>Role</InputLabel>
              <Select
                value={selectedUser?.role || ''}
                label="Role"
                onChange={(e) => {
                  if (selectedUser) {
                    handleUpdateUserRole(selectedUser.id, e.target.value);
                  }
                }}
              >
                <MenuItem value="admin">Admin</MenuItem>
                <MenuItem value="organizer">Organizer</MenuItem>
                <MenuItem value="attendee">Attendee</MenuItem>
              </Select>
            </FormControl>
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDialogClose}>Cancel</Button>
          <Button variant="contained" onClick={handleDialogClose}>
            Save Changes
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default UserManagement;
