export interface Reservation {
  id: string;
  eventId: string;
  userId: string;
  status: 'pending' | 'confirmed' | 'cancelled' | 'waitlisted' | 'completed';
  tickets: {
    category: string;
    quantity: number;
    pricePerTicket: number;
    seats?: string[];
  }[];
  totalAmount: number;
  paymentStatus: 'pending' | 'completed' | 'failed' | 'refunded';
  paymentMethod: 'card' | 'other';
  waitlistPosition?: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateReservationData {
  eventId: string;
  tickets: {
    category: string;
    quantity: number;
    seats?: string[];
  }[];
  waitlist?: boolean;
}

export interface PaymentData {
  reservationId: string;
  cardNumber: string;
  expiryMonth: string;
  expiryYear: string;
  cvv: string;
  cardHolderName: string;
}

export interface ReservationsState {
  reservations: Reservation[];
  selectedReservation: Reservation | null;
  isLoading: boolean;
  error: string | null;
  pagination: {
    page: number;
    limit: number;
    total: number;
  };
}

export interface ReservationFilters {
  status?: Reservation['status'];
  startDate?: string;
  endDate?: string;
  paymentStatus?: Reservation['paymentStatus'];
}

export interface CreateReservation {
  eventId: number;
  pricingId: number;
  quantity: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}

export interface ReservationResponse {
  id: number;
  reservationNumber: string;
  eventId: number;
  eventTitle: string;
  ticketCategory: string;
  ticketPrice: number;
  quantity: number;
  totalAmount: number;
  status: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  createdAt: string;
  updatedAt: string;
}
