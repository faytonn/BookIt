// Root paths for sidebar navigation
export const rootPaths = {
  homeRoot: '/',
  events: '/events',
  reservations: '/reservations',
  admin: '/admin',
  profile: '/profile',
  authRoot: '/auth',
};

const paths = {
  // Auth
  auth: {
    login: '/auth/login',
    register: '/auth/register',
    forgotPassword: '/auth/forgot-password',
    resetPassword: '/auth/reset-password',
    verifyEmail: '/auth/verify-email',
    verifySuccess: '/auth/verify-email/success',
    verifyFailed: '/auth/verify-email/failed',
  },
  // Events
  events: {
    root: '/',
    details: '/events/:id',
    edit: '/events/:id/edit',
    search: '/events/search',
  },
  // Reservations
  reservations: {
    root: '/reservations',
    book: '/events/:id/book',
    confirmation: '/reservations/:id/confirmation',
    history: '/reservations/history',
  },
  // Admin
  admin: {
    root: '/admin',
    dashboard: '/admin/dashboard',
    users: '/admin/users',
    createUser: '/admin/users/create',
    createEvent: '/admin/events/create',
    events: '/admin/events',
    reports: '/admin/reports',
    settings: '/admin/settings',
  },
  // Profile
  profile: {
    root: '/profile',
    settings: '/profile/settings',
    bookings: '/profile/bookings',
    savedEvents: '/profile/saved-events',
  },
  // Error
  error: {
    notFound: '/404',
    serverError: '/500',
  },
} as const;

export default paths;
