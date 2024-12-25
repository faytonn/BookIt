import { useState, useEffect } from 'react';
import {
  Container,
  Grid,
  Card,
  Typography,
  Box,
  Stack,
  LinearProgress,
  IconButton,
  Alert,
} from '@mui/material';
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  Legend,
} from 'recharts';
import RefreshIcon from '@mui/icons-material/Refresh';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import TrendingDownIcon from '@mui/icons-material/TrendingDown';
import PeopleIcon from '@mui/icons-material/People';
import EventIcon from '@mui/icons-material/Event';
import ConfirmationNumberIcon from '@mui/icons-material/ConfirmationNumber';
import AttachMoneyIcon from '@mui/icons-material/AttachMoney';
import { adminService } from '../services/adminService';
import { DashboardData } from '../types';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];

interface StatCard {
  title: string;
  value: string | number;
  change: number;
  icon: React.ReactNode;
  color: string;
}

const Dashboard = () => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<DashboardData | null>(null);

  const fetchDashboardData = async () => {
    setLoading(true);
    try {
      const dashboardData = await adminService.getDashboardData();
      setData(dashboardData);
      setError(null);
    } catch (err: any) {
      setError(err.message || 'Failed to fetch dashboard data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const stats: StatCard[] = data
    ? [
        {
          title: 'Total Users',
          value: data.stats.totalUsers.toLocaleString(),
          change: data.stats.userGrowth,
          icon: <PeopleIcon />,
          color: '#0088FE',
        },
        {
          title: 'Active Events',
          value: data.stats.activeEvents.toLocaleString(),
          change: data.stats.eventGrowth,
          icon: <EventIcon />,
          color: '#00C49F',
        },
        {
          title: 'Tickets Sold',
          value: data.stats.ticketsSold.toLocaleString(),
          change: data.stats.ticketGrowth,
          icon: <ConfirmationNumberIcon />,
          color: '#FFBB28',
        },
        {
          title: 'Revenue',
          value: `$${data.stats.totalRevenue.toLocaleString()}`,
          change: data.stats.revenueGrowth,
          icon: <AttachMoneyIcon />,
          color: '#FF8042',
        },
      ]
    : [];

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" gutterBottom sx={{ mb: 0 }}>
          Dashboard
        </Typography>
        <IconButton onClick={fetchDashboardData} disabled={loading}>
          <RefreshIcon />
        </IconButton>
      </Box>

      {loading && <LinearProgress sx={{ mb: 3 }} />}

      <Grid container spacing={3}>
        {/* Stats Cards */}
        {stats.map((stat, index) => (
          <Grid item xs={12} sm={6} md={3} key={index}>
            <Card sx={{ p: 2 }}>
              <Stack spacing={1}>
                <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                  <Box sx={{ color: stat.color }}>{stat.icon}</Box>
                  <Box
                    sx={{
                      display: 'flex',
                      alignItems: 'center',
                      color: stat.change >= 0 ? 'success.main' : 'error.main',
                    }}
                  >
                    {stat.change >= 0 ? <TrendingUpIcon /> : <TrendingDownIcon />}
                    <Typography variant="body2" sx={{ ml: 0.5 }}>
                      {Math.abs(stat.change)}%
                    </Typography>
                  </Box>
                </Box>
                <Typography variant="h4">{stat.value}</Typography>
                <Typography color="text.secondary" variant="body2">
                  {stat.title}
                </Typography>
              </Stack>
            </Card>
          </Grid>
        ))}

        {/* Revenue Chart */}
        {data && (
          <Grid item xs={12} md={8}>
            <Card sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Revenue Over Time
              </Typography>
              <Box sx={{ height: 300 }}>
                <ResponsiveContainer width="100%" height="100%">
                  <BarChart data={data.revenueData}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="month" />
                    <YAxis />
                    <Tooltip 
                      formatter={(value: any) => [`$${value}`, 'Revenue']}
                    />
                    <Bar dataKey="revenue" fill="#0088FE" />
                  </BarChart>
                </ResponsiveContainer>
              </Box>
            </Card>
          </Grid>
        )}

        {/* Event Types Pie Chart */}
        {data && (
          <Grid item xs={12} md={4}>
            <Card sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Event Distribution
              </Typography>
              <Box sx={{ height: 300 }}>
                <ResponsiveContainer width="100%" height="100%">
                  <PieChart>
                    <Pie
                      data={data.eventTypeData}
                      dataKey="count"
                      nameKey="type"
                      cx="50%"
                      cy="50%"
                      outerRadius={80}
                      label
                    >
                      {data.eventTypeData.map((entry, index) => (
                        <Cell key={entry.type} fill={COLORS[index % COLORS.length]} />
                      ))}
                    </Pie>
                    <Tooltip />
                    <Legend />
                  </PieChart>
                </ResponsiveContainer>
              </Box>
            </Card>
          </Grid>
        )}

        {/* Top Events Table */}
        {data && (
          <Grid item xs={12}>
            <Card sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Top Performing Events
              </Typography>
              <Box sx={{ overflowX: 'auto' }}>
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                  <thead>
                    <tr>
                      <th style={{ textAlign: 'left', padding: '12px' }}>Event</th>
                      <th style={{ textAlign: 'right', padding: '12px' }}>Tickets Sold</th>
                      <th style={{ textAlign: 'right', padding: '12px' }}>Revenue</th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.topEvents.map((event) => (
                      <tr key={event.id}>
                        <td style={{ padding: '12px' }}>{event.title}</td>
                        <td style={{ textAlign: 'right', padding: '12px' }}>
                          {event.ticketsSold.toLocaleString()}
                        </td>
                        <td style={{ textAlign: 'right', padding: '12px' }}>
                          ${event.revenue.toLocaleString()}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </Box>
            </Card>
          </Grid>
        )}
      </Grid>
    </Container>
  );
};

export default Dashboard;
