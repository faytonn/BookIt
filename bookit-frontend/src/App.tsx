import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterLuxon } from '@mui/x-date-pickers/AdapterLuxon';
import { Outlet } from 'react-router-dom';
import { AuthProvider } from 'features/auth/context/AuthContext';

function App() {
  return (
    <LocalizationProvider dateAdapter={AdapterLuxon}>
      <AuthProvider>
        <Outlet />
      </AuthProvider>
    </LocalizationProvider>
  );
}

export default App;
