import { ReactElement } from 'react';
import { Link, List, Toolbar, Box } from '@mui/material';
import navItems from 'data/nav-items';
import SimpleBar from 'simplebar-react';
import NavItem from './NavItem';
import { drawerCloseWidth, drawerOpenWidth } from '..';
import { rootPaths } from 'routes/paths';
import { useTheme } from '@mui/material/styles';

const Sidebar = ({ open }: { open: boolean }): ReactElement => {
  const theme = useTheme();

  return (
    <>
      <Toolbar
        sx={{
          position: 'fixed',
          height: 80,
          zIndex: 1,
          bgcolor: 'background.paper',
          p: 0,
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          width: open ? drawerOpenWidth - 1 : drawerCloseWidth - 1,
          borderBottom: '1px solid',
          borderColor: 'divider',
          transition: theme.transitions.create(['width', 'margin'], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
          }),
        }}
      >
        <Link
          href={rootPaths.homeRoot}
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            textDecoration: 'none',
            transition: theme.transitions.create('transform', {
              duration: theme.transitions.duration.shorter,
            }),
            '&:hover': {
              transform: 'scale(1.05)',
            },
          }}
        >
          <Box
            component="img"
            src={open ? '/src/assets/logo-with-text.svg' : '/src/assets/logo.svg'}
            alt={open ? 'Event Reserve' : 'ER'}
            sx={{
              height: 40,
              width: open ? 'auto' : 40,
              color: theme.palette.mode === 'light' ? 'text.primary' : 'common.white',
            }}
          />
        </Link>
      </Toolbar>
      <SimpleBar style={{ maxHeight: '100vh' }}>
        <List
          component="nav"
          sx={{
            mt: 24.5,
            py: 2.5,
            height: 724,
          }}
        >
          {navItems.map((navItem) => (
            <NavItem key={navItem.id} navItem={navItem} open={open} />
          ))}
        </List>
      </SimpleBar>
    </>
  );
};

export default Sidebar;
