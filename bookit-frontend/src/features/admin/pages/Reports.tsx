import { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  Grid,
  Button,
  Stack,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  CircularProgress,
  Alert,
  IconButton,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import {
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  LineChart,
  Line,
  PieChart,
  Pie,
  Cell,
  BarChart,
  Bar,
  Legend,
} from 'recharts';
import DownloadIcon from '@mui/icons-material/Download';
import EmailIcon from '@mui/icons-material/Email';
import RefreshIcon from '@mui/icons-material/Refresh';
import { DateTime } from 'luxon';
import { adminService } from '../services/adminService';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];

interface SalesReportItem {
  name: string;
  ticketsSold: number;
  revenue: number;
  averageTicketPrice: number;
}

interface SalesReport {
  totalRevenue: number;
  totalTicketsSold: number;
  salesByEvent: SalesReportItem[];
  salesByMonth: SalesReportItem[];
}

interface EventReportItem {
  id: number;
  title: string;
  type: string;
  locationCity: string;
  locationCountry: string;
  startDate: string;
  endDate: string;
  totalSeats: number;
  soldSeats: number;
  occupancyRate: number;
}

interface EventsReport {
  totalEvents: number;
  eventsByType: Record<string, number>;
  eventsByCountry: Record<string, number>;
  eventsByMonth: Record<string, number>;
  attendanceByType: Record<string, number>;
  events: EventReportItem[];
}

interface ReservationReportItem {
  reservationNumber: string;
  eventTitle: string;
  customerName: string;
  email: string;
  status: string;
  quantity: number;
  totalAmount: number;
  createdAt: string;
}

interface ReservationsReport {
  totalReservations: number;
  reservationsByStatus: Record<string, number>;
  reservations: ReservationReportItem[];
}

type ReportData = SalesReport | EventsReport | ReservationsReport;

const Reports = () => {
  const [startDate, setStartDate] = useState<DateTime | null>(DateTime.now().startOf('month'));
  const [endDate, setEndDate] = useState<DateTime | null>(DateTime.now().endOf('month'));
  const [reportType, setReportType] = useState<'sales' | 'events' | 'reservations'>('events');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [reportData, setReportData] = useState<ReportData | null>(null);

  const fetchReportData = async () => {
    if (!startDate || !endDate) {
      setError('Please select both start and end dates');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const response = await adminService.generateReport({
        reportType,
        startDate: startDate.toJSDate(),
        endDate: endDate.toJSDate(),
      });
      setReportData(response);
    } catch (err: any) {
      setError(err.message || 'Failed to fetch report data');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchReportData();
  }, [reportType]);

  const renderEventsReport = () => {
    const data = reportData as EventsReport;
    if (!data?.eventsByType) return null;

    const eventsByTypeData = Object.entries(data.eventsByType).map(([name, value]) => ({
      name,
      value,
    }));

    const eventsByCountryData = Object.entries(data.eventsByCountry)
      .map(([name, value]) => ({ name, value }))
      .sort((a, b) => b.value - a.value)
      .slice(0, 10);

    const eventsByMonthData = Object.entries(data.eventsByMonth)
      .map(([name, value]) => ({ name, value }))
      .sort((a, b) => DateTime.fromFormat(a.name, 'MMMM yyyy').toMillis() - DateTime.fromFormat(b.name, 'MMMM yyyy').toMillis());

    const attendanceByTypeData = Object.entries(data.attendanceByType).map(([name, value]) => ({
      name,
      value: Math.round(value),
    }));

    return (
      <Grid container spacing={3}>
        {/* Events by Type */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Events by Type</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={eventsByTypeData}
                    dataKey="value"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    outerRadius={80}
                    label
                  >
                    {eventsByTypeData.map((_, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        {/* Events by Country */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Top 10 Countries by Events</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={eventsByCountryData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" angle={-45} textAnchor="end" height={80} />
                  <YAxis />
                  <Tooltip />
                  <Bar dataKey="value" fill="#8884d8" />
                </BarChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        {/* Events by Month */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Events Trend</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <LineChart data={eventsByMonthData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" angle={-45} textAnchor="end" height={80} />
                  <YAxis />
                  <Tooltip />
                  <Line type="monotone" dataKey="value" stroke="#82ca9d" />
                </LineChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        {/* Attendance Rate */}
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Attendance Rate by Event Type</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={attendanceByTypeData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" />
                  <YAxis unit="%" />
                  <Tooltip formatter={(value) => `${value}%`} />
                  <Bar dataKey="value" fill="#82ca9d" />
                </BarChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        {/* Event Details */}
        <Grid item xs={12}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Event Details</Typography>
            <Grid container spacing={2}>
              {data.events.map((event) => (
                <Grid item xs={12} sm={6} md={4} key={event.id}>
                  <Card variant="outlined">
                    <Box p={2}>
                      <Typography variant="subtitle1" gutterBottom>{event.title}</Typography>
                      <Typography variant="body2" color="text.secondary">Type: {event.type}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        Location: {event.locationCity}, {event.locationCountry}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Seats: {event.soldSeats}/{event.totalSeats} ({Math.round((event.soldSeats / event.totalSeats) * 100)}% occupied)
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Occupancy Rate: {event.occupancyRate}%
                      </Typography>
                    </Box>
                  </Card>
                </Grid>
              ))}
            </Grid>
          </Card>
        </Grid>
      </Grid>
    );
  };

  const renderSalesReport = () => {
    const data = reportData as SalesReport;
    if (!data?.salesByEvent) return null;

    return (
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Sales by Event</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={data.salesByEvent}
                    dataKey="revenue"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    outerRadius={80}
                    label
                  >
                    {data.salesByEvent.map((_, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip formatter={(value) => `$${value}`} />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Monthly Sales Trend</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <LineChart data={data.salesByMonth}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" angle={-45} textAnchor="end" height={80} />
                  <YAxis />
                  <Tooltip formatter={(value) => `$${value}`} />
                  <Line type="monotone" dataKey="revenue" stroke="#8884d8" />
                </LineChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        <Grid item xs={12}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Sales Summary</Typography>
            <Grid container spacing={3}>
              <Grid item xs={12} sm={6}>
                <Typography variant="subtitle1">Total Revenue:</Typography>
                <Typography variant="h4">${data.totalRevenue.toLocaleString()}</Typography>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Typography variant="subtitle1">Total Tickets Sold:</Typography>
                <Typography variant="h4">{data.totalTicketsSold.toLocaleString()}</Typography>
              </Grid>
            </Grid>
          </Card>
        </Grid>
      </Grid>
    );
  };

  const renderReservationsReport = () => {
    const data = reportData as ReservationsReport;
    if (!data?.reservationsByStatus) return null;

    const reservationsByStatusData = Object.entries(data.reservationsByStatus).map(([name, value]) => ({
      name,
      value,
    }));

    return (
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Reservations by Status</Typography>
            <Box sx={{ height: 300 }}>
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={reservationsByStatusData}
                    dataKey="value"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    outerRadius={80}
                    label
                  >
                    {reservationsByStatusData.map((_, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Reservations Summary</Typography>
            <Box p={2}>
              <Typography variant="h4" gutterBottom>
                {data.totalReservations.toLocaleString()}
              </Typography>
              <Typography color="text.secondary">Total Reservations</Typography>
            </Box>
          </Card>
        </Grid>

        <Grid item xs={12}>
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Recent Reservations</Typography>
            <Grid container spacing={2}>
              {data.reservations.slice(0, 6).map((reservation) => (
                <Grid item xs={12} sm={6} md={4} key={reservation.reservationNumber}>
                  <Card variant="outlined">
                    <Box p={2}>
                      <Typography variant="subtitle1" gutterBottom>{reservation.eventTitle}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        Customer: {reservation.customerName}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Status: {reservation.status}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Amount: ${reservation.totalAmount.toLocaleString()}
                      </Typography>
                    </Box>
                  </Card>
                </Grid>
              ))}
            </Grid>
          </Card>
        </Grid>
      </Grid>
    );
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" gutterBottom sx={{ mb: 0 }}>
          Reports & Analytics
        </Typography>
        <IconButton onClick={fetchReportData} disabled={loading}>
          <RefreshIcon />
        </IconButton>
      </Box>

      <Card sx={{ p: 3, mb: 4 }}>
        <Grid container spacing={3} alignItems="flex-end">
          <Grid item xs={12} md={3}>
            <FormControl fullWidth>
              <InputLabel>Report Type</InputLabel>
              <Select
                value={reportType}
                label="Report Type"
                onChange={(e) => setReportType(e.target.value as 'sales' | 'events' | 'reservations')}
              >
                <MenuItem value="sales">Sales Report</MenuItem>
                <MenuItem value="events">Events Report</MenuItem>
                <MenuItem value="reservations">Reservations Report</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={3}>
            <DatePicker
              label="Start Date"
              value={startDate}
              onChange={(newValue) => setStartDate(newValue)}
              slotProps={{ textField: { fullWidth: true } }}
            />
          </Grid>
          <Grid item xs={12} md={3}>
            <DatePicker
              label="End Date"
              value={endDate}
              onChange={(newValue) => setEndDate(newValue)}
              slotProps={{ textField: { fullWidth: true } }}
            />
          </Grid>
          <Grid item xs={12} md={3}>
            <Stack direction="row" spacing={2}>
              <Button
                variant="contained"
                onClick={fetchReportData}
                startIcon={<DownloadIcon />}
                fullWidth
              >
                Generate
              </Button>
              <Button
                variant="outlined"
                startIcon={<EmailIcon />}
                fullWidth
              >
                Email
              </Button>
            </Stack>
          </Grid>
        </Grid>
      </Card>

      {loading && <CircularProgress sx={{ display: 'block', mx: 'auto', my: 4 }} />}

      {error && (
        <Alert severity="error" sx={{ mb: 4 }}>
          {error}
        </Alert>
      )}

      {reportType === 'events' && renderEventsReport()}
      {reportType === 'sales' && renderSalesReport()}
      {reportType === 'reservations' && renderReservationsReport()}
    </Container>
  );
};

export default Reports;