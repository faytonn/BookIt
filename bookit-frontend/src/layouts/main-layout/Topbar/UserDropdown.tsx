import {
  Menu,
  Avatar,
  Button,
  Tooltip,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Divider,
} from '@mui/material';
import IconifyIcon from 'components/base/IconifyIcon';
import profile from 'assets/images/account/Profile.png';
import { useState, MouseEvent, useCallback, ReactElement } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from 'features/auth/context/AuthContext';
import userMenuItems from 'data/usermenu-items';

const UserDropdown = (): ReactElement => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const menuOpen = Boolean(anchorEl);
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const handleUserClick = useCallback((event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  }, []);

  const handleUserClose = useCallback(() => {
    setAnchorEl(null);
  }, []);

  const handleMenuItemClick = useCallback(
    async (action?: 'logout' | 'navigate', path?: string) => {
      handleUserClose();

      if (action === 'logout') {
        await logout();
        navigate('/');
      } else if (action === 'navigate' && path) {
        navigate(path);
      }
    },
    [navigate, logout]
  );

  return (
    <>
      <Button
        color="inherit"
        variant="text"
        id="account-dropdown-menu"
        aria-controls={menuOpen ? 'account-dropdown-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={menuOpen ? 'true' : undefined}
        onClick={handleUserClick}
        disableRipple
        sx={{
          borderRadius: 2,
          gap: 2,
          px: { xs: 0, sm: 0.625 },
          py: 0.625,
          '&:hover': {
            bgcolor: 'action.hover',
          },
        }}
      >
        <Tooltip title={user?.name || 'User'} arrow placement="bottom">
          <Avatar
            src={user?.avatar || profile}
            alt={user?.name || 'User'}
            sx={{
              width: 40,
              height: 40,
              bgcolor: 'primary.main',
            }}
          >
            {user?.name?.charAt(0).toUpperCase()}
          </Avatar>
        </Tooltip>
        <IconifyIcon
          icon="material-symbols:keyboard-arrow-down"
          sx={(theme) => ({
            color: 'text.primary',
            transform: menuOpen ? `rotate(180deg)` : `rotate(0deg)`,
            transition: theme.transitions.create('transform'),
          })}
        />
      </Button>

      <Menu
        id="account-dropdown-menu"
        anchorEl={anchorEl}
        open={menuOpen}
        onClose={handleUserClose}
        MenuListProps={{
          'aria-labelledby': 'account-dropdown-button',
        }}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
        PaperProps={{
          elevation: 3,
          sx: {
            mt: 1.5,
            minWidth: 200,
            overflow: 'visible',
            '&:before': {
              content: '""',
              display: 'block',
              position: 'absolute',
              top: 0,
              right: 14,
              width: 10,
              height: 10,
              bgcolor: 'background.paper',
              transform: 'translateY(-50%) rotate(45deg)',
              zIndex: 0,
            },
          },
        }}
      >
        <MenuItem
          onClick={() => handleMenuItemClick('navigate', '/profile')}
          sx={{ py: 1 }}
        >
          <ListItemIcon sx={{ minWidth: 36 }}>
            <Avatar
              src={user?.avatar || profile}
              alt={user?.name || 'User'}
              sx={{ width: 32, height: 32 }}
            >
              {user?.name?.charAt(0).toUpperCase()}
            </Avatar>
          </ListItemIcon>
          <ListItemText
            primary={user?.name || 'User'}
            secondary={user?.email}
            primaryTypographyProps={{
              variant: 'subtitle1',
              color: 'text.primary',
            }}
            secondaryTypographyProps={{
              variant: 'caption',
              color: 'text.secondary',
            }}
          />
        </MenuItem>

        <Divider />

        {userMenuItems.map((userMenuItem) => (
          <MenuItem
            key={userMenuItem.id}
            onClick={() => handleMenuItemClick(userMenuItem.action, userMenuItem.path)}
            sx={{
              py: 1,
              px: 2,
              '&:hover': {
                bgcolor: 'action.hover',
              },
            }}
          >
            <ListItemIcon sx={{ minWidth: 36, color: userMenuItem.color }}>
              <IconifyIcon icon={userMenuItem.icon} />
            </ListItemIcon>
            <ListItemText
              primary={userMenuItem.title}
              sx={{
                '& .MuiTypography-root': {
                  color: userMenuItem.color,
                },
              }}
            />
          </MenuItem>
        ))}
      </Menu>
    </>
  );
};

export default UserDropdown;
