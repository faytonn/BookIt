import { createContext, useContext, useMemo, useState, ReactNode } from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

type ThemeContextType = {
  toggleColorMode: () => void;
  mode: 'light' | 'dark';
};

export const ThemeContext = createContext<ThemeContextType>({
  toggleColorMode: () => {},
  mode: 'light',
});

export const useThemeContext = () => useContext(ThemeContext);

type ThemeProviderWrapperProps = {
  children: ReactNode;
};

export const ThemeProviderWrapper = ({ children }: ThemeProviderWrapperProps) => {
  const [mode, setMode] = useState<'light' | 'dark'>('light');

  const colorMode = useMemo(
    () => ({
      toggleColorMode: () => {
        setMode((prevMode) => (prevMode === 'light' ? 'dark' : 'light'));
      },
      mode,
    }),
    [mode]
  );

  const theme = useMemo(
    () =>
      createTheme({
        palette: {
          mode,
          primary: {
            main: '#1976d2',
          },
          secondary: {
            main: '#dc004e',
          },
          background: {
            default: mode === 'light' ? '#f5f5f5' : '#121212',
            paper: mode === 'light' ? '#fff' : '#1e1e1e',
          },
        },
        components: {
          MuiCard: {
            styleOverrides: {
              root: {
                backgroundColor: mode === 'light' ? '#fff' : '#1e1e1e',
              },
            },
          },
        },
      }),
    [mode]
  );

  return (
    <ThemeContext.Provider value={colorMode}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </ThemeContext.Provider>
  );
};
