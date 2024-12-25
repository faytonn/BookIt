export interface DashboardStats {
  totalUsers: number;
  activeEvents: number;
  ticketsSold: number;
  totalRevenue: number;
  userGrowth: number;
  eventGrowth: number;
  ticketGrowth: number;
  revenueGrowth: number;
}

export interface RevenueData {
  month: string;
  revenue: number;
}

export interface EventTypeData {
  type: string;
  count: number;
}

export interface TopEvent {
  id: string;
  title: string;
  ticketsSold: number;
  revenue: number;
}

export interface DashboardData {
  stats: DashboardStats;
  revenueData: RevenueData[];
  eventTypeData: EventTypeData[];
  topEvents: TopEvent[];
}
