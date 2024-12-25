import { createTheme } from '@mui/material/styles';
import { createContext } from 'react';

import components from './component-overrides';
import breakpoints from './breakpoints';
import typography from './typography';
import palette from './palette';
import spacing from './spacing';
import shape from './shape';

export const ColorModeContext = createContext({
  toggleColorMode: () => {},
  mode: 'light',
});

const theme = createTheme({
  breakpoints: breakpoints,
  components: {
    ...components,
    MuiCard: {
      styleOverrides: {
        root: {
          backgroundColor: '#fff',
        },
      },
    },
  },
  typography: typography,
  palette: {
    ...palette,
    mode: 'light',
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
    background: {
      default: '#f5f5f5',
      paper: '#fff',
    },
  },
  spacing: spacing,
  shape: shape,
});

export default theme;
