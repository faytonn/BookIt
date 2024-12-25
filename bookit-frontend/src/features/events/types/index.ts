export interface Event {
  id: string;
  title: string;
  description: string;
  type: 'concert' | 'sports' | 'conference' | 'theater' | 'other';
  startDate: string;
  endDate: string;
  location: {
    name: string;
    address: string;
    city: string;
    country: string;
    coordinates?: {
      lat: number;
      lng: number;
    };
  };
  organizer: {
    id: string;
    name: string;
    email: string;
  };
  images: {
    url: string;
    type: 'main' | 'thumbnail' | 'gallery';
  }[];
  imageUrl: string;
  pricing: {
    id: number;
    category: string;
    price: number;
    availableSeats: number;
    totalSeats: number;
  }[];
  status: 'draft' | 'published' | 'upcoming' | 'cancelled' | 'completed';
  tags: string[];
  createdAt: string;
  updatedAt: string;
}

export interface EventFilters {
  search?: string;
  type?: Event['type'];
  startDate?: string;
  endDate?: string;
  city?: string;
  minPrice?: number;
  maxPrice?: number;
  status?: Event['status'];
}

export interface EventSort {
  field: 'date' | 'price' | 'title';
  direction: 'asc' | 'desc';
}

export interface EventsState {
  events: Event[];
  selectedEvent: Event | null;
  filters: EventFilters;
  sort: EventSort;
  isLoading: boolean;
  error: string | null;
  pagination: {
    page: number;
    limit: number;
    total: number;
  };
}

export interface CreateEventData {
  title: string;
  description: string;
  type: Event['type'];
  startDate: Date | null;
  endDate: Date | null;
  location: Event['location'];
  images: File[];
  pricing: Omit<Event['pricing'][0], 'availableSeats'>[];
  tags: string[];
}
