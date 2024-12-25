import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider } from 'react-router-dom';
import { CssBaseline } from '@mui/material';
import BreakpointsProvider from 'providers/BreakpointsProvider.tsx';
import { SnackbarProvider } from 'notistack';
import router from 'routes/router';
import { CustomThemeProvider } from './providers/ThemeProvider';
import './index.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <CustomThemeProvider>
      <SnackbarProvider maxSnack={3}>
        <BreakpointsProvider>
          <CssBaseline />
          <RouterProvider router={router} />
        </BreakpointsProvider>
      </SnackbarProvider>
    </CustomThemeProvider>
  </React.StrictMode>,
);
