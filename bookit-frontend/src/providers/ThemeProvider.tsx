import { useState, useMemo, ReactNode } from 'react';
import { ThemeProvider as MuiThemeProvider, createTheme } from '@mui/material/styles';
import { ColorModeContext } from '../theme/theme';
import components from '../theme/component-overrides';
import breakpoints from '../theme/breakpoints';
import typography from '../theme/typography';
import spacing from '../theme/spacing';
import shape from '../theme/shape';

type CustomThemeProviderProps = {
  children: ReactNode;
};

export function CustomThemeProvider({ children }: CustomThemeProviderProps) {
  const [mode, setMode] = useState<'light' | 'dark'>('dark');

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
        breakpoints,
        components: {
          ...components,
          MuiCard: {
            styleOverrides: {
              root: {
                backgroundColor: mode === 'light' ? '#fff' : '#1e1e1e',
              },
            },
          },
        },
        typography,
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
        spacing,
        shape,
      }),
    [mode]
  );

  return (
    <ColorModeContext.Provider value={colorMode}>
      <MuiThemeProvider theme={theme}>{children}</MuiThemeProvider>
    </ColorModeContext.Provider>
  );
}
