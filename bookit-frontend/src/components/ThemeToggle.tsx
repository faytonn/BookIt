import { IconButton, Tooltip, useTheme } from '@mui/material';
import Brightness4Icon from '@mui/icons-material/Brightness4';
import Brightness7Icon from '@mui/icons-material/Brightness7';
import { useContext } from 'react';
import { ColorModeContext } from '../theme/theme';

export const ThemeToggle = () => {
  const theme = useTheme();
  const colorMode = useContext(ColorModeContext);

  return (
    <Tooltip title={`Switch to ${theme.palette.mode === 'light' ? 'dark' : 'light'} mode`}>
      <IconButton onClick={colorMode.toggleColorMode} color="inherit" sx={{ ml: 1 }}>
        {theme.palette.mode === 'light' ? <Brightness4Icon /> : <Brightness7Icon />}
      </IconButton>
    </Tooltip>
  );
};
