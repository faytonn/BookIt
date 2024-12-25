import {
  Badge,
  Stack,
  AppBar,
  Toolbar,
  TextField,
  IconButton,
  InputAdornment,
  Button,
  useTheme,
} from '@mui/material';
import IconifyIcon from 'components/base/IconifyIcon';
import { ReactElement, useState } from 'react';
import { drawerCloseWidth, drawerOpenWidth } from '..';
import UserDropdown from './UserDropdown';
import { useBreakpoints } from 'providers/BreakpointsProvider';
import { useNavigate } from 'react-router-dom';
import { useAuth } from 'features/auth/context/AuthContext';
import { rootPaths } from 'routes/paths';

const Topbar = ({
  open,
  handleDrawerToggle,
}: {
  open: boolean;
  handleDrawerToggle: () => void;
}): ReactElement => {
  const { down } = useBreakpoints();
  const theme = useTheme();
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();
  const [searchQuery, setSearchQuery] = useState('');

  const isMobileScreen = down('sm');

  const handleSearch = () => {
    if (searchQuery.trim()) {
      navigate(`/events/search?q=${encodeURIComponent(searchQuery.trim())}`);
    }
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    if (event.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <AppBar
      position="fixed"
      sx={{
        left: 0,
        ml: isMobileScreen ? 0 : open ? 60 : 27.5,
        width: isMobileScreen
          ? 1
          : open
            ? `calc(100% - ${drawerOpenWidth}px)`
            : `calc(100% - ${drawerCloseWidth}px)`,
        paddingRight: '0 !important',
        bgcolor: 'background.paper',
        boxShadow: theme.shadows[3],
      }}
    >
      <Toolbar
        component={Stack}
        direction="row"
        alignItems="center"
        justifyContent="space-between"
        sx={{
          bgcolor: 'background.paper',
          height: 80,
          px: 2,
        }}
      >
        <Stack direction="row" gap={2} alignItems="center" flex="1 1 52.5%">
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={handleDrawerToggle}
            edge="start"
            sx={{ color: 'text.primary' }}
          >
            <IconifyIcon
              icon={open ? 'material-symbols:menu-open' : 'material-symbols:menu'}
            />
          </IconButton>

          <TextField
            variant="outlined"
            size="small"
            fullWidth
            placeholder="Search events..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onKeyPress={handleKeyPress}
            sx={{
              display: { xs: 'none', sm: 'flex' },
              maxWidth: 400,
              '& .MuiOutlinedInput-root': {
                bgcolor: 'background.default',
                '&:hover': {
                  bgcolor: 'action.hover',
                },
              },
            }}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start" sx={{ ml: 4 }}>
                  <IconifyIcon icon="material-symbols:search" />
                </InputAdornment>
              ),
            }}
          />
        </Stack>

        <Stack
          direction="row"
          gap={2}
          alignItems="center"
          justifyContent="flex-end"
          flex="1 1 20%"
        >
          {!isAuthenticated ? (
            <Stack direction="row" spacing={1}>
              <Button
                variant="outlined"
                onClick={() => navigate(rootPaths.authRoot + '/login')}
                size="small"
              >
                Login
              </Button>
              <Button
                variant="contained"
                onClick={() => navigate(rootPaths.authRoot + '/register')}
                size="small"
              >
                Sign Up
              </Button>
            </Stack>
          ) : (
            <>
              {/* <Badge
                color="error"
                badgeContent={3}
                sx={{
                  '& .MuiBadge-badge': {
                    right: -3,
                    top: 3,
                  },
                }}
              >
                <IconButton
                  sx={{
                    color: 'text.primary',
                  }}
                >
                  <IconifyIcon icon="material-symbols:notifications" />
                </IconButton>
              </Badge> */}
              <UserDropdown />
            </>
          )}
        </Stack>
      </Toolbar>
    </AppBar>
  );
};

export default Topbar;
