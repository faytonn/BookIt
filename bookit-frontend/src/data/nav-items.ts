export interface NavItem {
  id: number;
  path: string;
  title: string;
  icon: string;
  active: boolean;
  requiresAuth?: boolean;
  adminOnly?: boolean;
}

const navItems: NavItem[] = [
  {
    id: 1,
    path: '/',
    title: 'Events',
    icon: 'mingcute:home-1-fill',
    active: true,
  },
  // {
  //   id: 2,
  //   path: '/events/search',
  //   title: 'Search Events',
  //   icon: 'material-symbols:search',
  //   active: true,
  // },
  {
    id: 3,
    path: '/reservations',
    title: 'My Reservations',
    icon: 'material-symbols:calendar-month',
    active: true,
    requiresAuth: true,
  },
  {
    id: 4,
    path: '/profile',
    title: 'Profile',
    icon: 'material-symbols:person',
    active: true,
    requiresAuth: true,
  },
  // {
  //   id: 5,
  //   path: '/profile/saved-events',
  //   title: 'Saved Events',
  //   icon: 'material-symbols:favorite',
  //   active: true,
  //   requiresAuth: true,
  // },
  {
    id: 6,
    path: '/admin/dashboard',
    title: 'Admin Dashboard',
    icon: 'material-symbols:dashboard',
    active: true,
    requiresAuth: true,
    adminOnly: true,
  },
  {
    id: 7,
    path: '/admin/events',
    title: 'Event Management',
    icon: 'material-symbols:event-list',
    active: true,
    requiresAuth: true,
    adminOnly: true,
  },
  {
    id: 8,
    path: '/admin/users',
    title: 'User Management',
    icon: 'material-symbols:group',
    active: true,
    requiresAuth: true,
    adminOnly: true,
  },
  {
    id: 9,
    path: '/admin/reports',
    title: 'Reports & Analytics',
    icon: 'material-symbols:analytics',
    active: true,
    requiresAuth: true,
    adminOnly: true,
  },
  {
    id: 10,
    path: '/admin/settings',
    title: 'Settings',
    icon: 'material-symbols:settings',
    active: true,
    requiresAuth: true,
    adminOnly: true,
  },
];

export default navItems;
