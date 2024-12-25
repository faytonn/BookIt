import { lazy, Suspense, ReactElement, PropsWithChildren } from 'react';
import { Outlet, RouteObject, RouterProps, createBrowserRouter } from 'react-router-dom';

import PageLoader from 'components/loading/PageLoader';
import Splash from 'components/loading/Splash';
import { rootPaths } from './paths';
import paths from './paths';

const App = lazy<() => ReactElement>(() => import('App'));

const MainLayout = lazy<({ children }: PropsWithChildren) => ReactElement>(
  () => import('layouts/main-layout'),
);
const AuthLayout = lazy<({ children }: PropsWithChildren) => ReactElement>(
  () => import('layouts/auth-layout'),
);

const Login = lazy(() => import('../features/auth/pages/Login'));
const Register = lazy(() => import('../features/auth/pages/Register'));
const ForgotPassword = lazy(() => import('../features/auth/pages/ForgotPassword'));
const ResetPassword = lazy(() => import('../features/auth/pages/ResetPassword'));
const VerifyEmail = lazy(() => import('../features/auth/pages/VerifyEmail'));
const EmailVerification = lazy(() => import('../features/auth/components/EmailVerification'));

const EventList = lazy(() => import('../features/events/pages/EventList'));
const EventDetails = lazy(() => import('../features/events/pages/EventDetails'));
const EventCreate = lazy(() => import('../features/events/pages/EventCreate'));
const EventEdit = lazy(() => import('../features/events/pages/EventEdit'));
const EventSearch = lazy(() => import('../features/events/pages/EventSearch'));

const ReservationsList = lazy(() => import('../features/reservations/pages/ReservationsList'));
const BookingPage = lazy(() => import('../features/reservations/pages/BookingPage'));
const ConfirmationPage = lazy(() => import('../features/reservations/pages/ConfirmationPage'));
const BookingHistory = lazy(() => import('../features/reservations/pages/BookingHistory'));

const AdminDashboard = lazy(() => import('../features/admin/pages/Dashboard'));
const UserManagement = lazy(() => import('../features/admin/pages/UserManagement'));
const CreateUser = lazy(() => import('../features/admin/pages/CreateUser'));
const EventManagement = lazy(() => import('../features/admin/pages/EventManagement'));
const Reports = lazy(() => import('../features/admin/pages/Reports'));
const Settings = lazy(() => import('../features/admin/pages/Settings'));

const Profile = lazy(() => import('../features/profile/pages/Profile'));
const ProfileSettings = lazy(() => import('../features/profile/pages/ProfileSettings'));
const ProfileBookings = lazy(() => import('../features/profile/pages/ProfileBookings'));
const SavedEvents = lazy(() => import('../features/profile/pages/SavedEvents'));

const ErrorPage = lazy(() => import('../pages/error/ErrorPage'));

const routes: RouteObject[] = [
  {
    path: '/',
    element: (
      <Suspense fallback={<Splash />}>
        <App />
      </Suspense>
    ),
    children: [
      {
        path: '/',
        element: (
          <MainLayout>
            <Suspense fallback={<PageLoader />}>
              <Outlet />
            </Suspense>
          </MainLayout>
        ),
        children: [
          { path: '/', element: <EventList /> },
          { path: paths.events.details, element: <EventDetails /> },
          // { path: paths.events.create, element: <EventCreate /> },
          { path: paths.events.edit, element: <EventEdit /> },
          { path: paths.events.search, element: <EventSearch /> },
          
          { path: paths.reservations.root, element: <ReservationsList /> },
          { path: paths.reservations.book, element: <BookingPage /> },
          { path: paths.reservations.confirmation, element: <ConfirmationPage /> },
          { path: paths.reservations.history, element: <BookingHistory /> },
          
          { path: paths.admin.dashboard, element: <AdminDashboard /> },
          { path: paths.admin.users, element: <UserManagement /> },
          { path: paths.admin.createUser, element: <CreateUser /> },
          { path: paths.admin.createEvent, element: <EventCreate /> },
          { path: paths.admin.events, element: <EventManagement /> },
          { path: paths.admin.reports, element: <Reports /> },
          { path: paths.admin.settings, element: <Settings /> },
          
          { path: paths.profile.root, element: <Profile /> },
          { path: paths.profile.settings, element: <ProfileSettings /> },
          { path: paths.profile.bookings, element: <ProfileBookings /> },
          { path: paths.profile.savedEvents, element: <SavedEvents /> },
        ],
      },
      {
        path: rootPaths.authRoot,
        element: (
          <AuthLayout>
            <Suspense fallback={<PageLoader />}>
              <Outlet />
            </Suspense>
          </AuthLayout>
        ),
        children: [
          { path: paths.auth.login, element: <Login /> },
          { path: paths.auth.register, element: <Register /> },
          { path: paths.auth.forgotPassword, element: <ForgotPassword /> },
          { path: paths.auth.resetPassword, element: <ResetPassword /> },
          { path: paths.auth.verifyEmail, element: <VerifyEmail /> },
          { path: paths.auth.verifySuccess, element: <EmailVerification /> },
          { path: paths.auth.verifyFailed, element: <EmailVerification /> },
        ],
      },
    ],
  },
  {
    path: '*',
    element: <ErrorPage />,
  },
];

const options: { basename: string } = {
  basename: '/',
};

const router: Partial<RouterProps> = createBrowserRouter(routes, options);

export default router;
