interface UserMenuItem {
  id: number;
  title: string;
  icon: string;
  color?: string;
  path?: string;
  action?: 'logout' | 'navigate';
}

const userMenuItems: UserMenuItem[] = [
  {
    id: 1,
    title: 'View Profile',
    icon: 'material-symbols:person',
    color: 'text.primary',
    path: '/profile',
    action: 'navigate',
  },
  {
    id: 2,
    title: 'My Bookings',
    icon: 'material-symbols:calendar-month',
    color: 'text.primary',
    path: '/profile/bookings',
    action: 'navigate',
  },
  // {
  //   id: 3,
  //   title: 'Saved Events',
  //   icon: 'material-symbols:favorite',
  //   color: 'text.primary',
  //   path: '/profile/saved-events',
  //   action: 'navigate',
  // },
  {
    id: 4,
    title: 'Account Settings',
    icon: 'material-symbols:settings',
    color: 'text.primary',
    path: '/profile/settings',
    action: 'navigate',
  },
  {
    id: 5,
    title: 'Notifications',
    icon: 'material-symbols:notifications',
    color: 'text.primary',
    path: '/notifications',
    action: 'navigate',
  },
  {
    id: 6,
    title: 'Logout',
    icon: 'material-symbols:logout',
    color: 'error.main',
    action: 'logout',
  },
];

export default userMenuItems;
