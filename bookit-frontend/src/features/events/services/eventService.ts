import axios from 'axios';
import { CreateEventData } from '../types';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

class EventService {
  async createEvent(eventData: CreateEventData) {
    const formData = new FormData();
    formData.append('title', eventData.title);
    formData.append('description', eventData.description);
    formData.append('type', eventData.type);
    formData.append('startDate', eventData.startDate);
    formData.append('endDate', eventData.endDate);

    // Location
    formData.append('location.name', eventData.location.name);
    formData.append('location.address', eventData.location.address);
    formData.append('location.city', eventData.location.city);
    formData.append('location.country', eventData.location.country);

    // Image
    if (eventData.images.length > 0) {
      formData.append('image', eventData.images[0]);
    }

    // Pricing
    eventData.pricing.forEach((price, index) => {
      formData.append(`pricing[${index}].category`, price.category);
      formData.append(`pricing[${index}].price`, price.price.toString());
      formData.append(`pricing[${index}].totalSeats`, price.totalSeats.toString());
    });

    // Tags
    eventData.tags.forEach((tag, index) => {
      formData.append(`tags[${index}]`, tag);
    });

    const response = await axios.post(`${API_URL}/event`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  }

  async getEvents(type?: string, search?: string) {
    const params = new URLSearchParams();
    if (type) params.append('type', type);
    if (search) params.append('search', search);

    const response = await axios.get(`${API_URL}/event?${params.toString()}`);
    return response.data;
  }

  async getEventById(id: number) {
    const response = await axios.get(`${API_URL}/event/${id}`);
    return response.data;
  }

  async updateEvent(id: number, eventData: CreateEventData) {
    const formData = new FormData();
    formData.append('title', eventData.title);
    formData.append('description', eventData.description);
    formData.append('type', eventData.type);
    formData.append('startDate', eventData.startDate);
    formData.append('endDate', eventData.endDate);

    // Location
    formData.append('location.name', eventData.location.name);
    formData.append('location.address', eventData.location.address);
    formData.append('location.city', eventData.location.city);
    formData.append('location.country', eventData.location.country);

    // Image
    if (eventData.images.length > 0) {
      formData.append('image', eventData.images[0]);
    }

    // Pricing
    eventData.pricing.forEach((price, index) => {
      formData.append(`pricing[${index}].category`, price.category);
      formData.append(`pricing[${index}].price`, price.price.toString());
      formData.append(`pricing[${index}].totalSeats`, price.totalSeats.toString());
    });

    // Tags
    eventData.tags.forEach((tag, index) => {
      formData.append(`tags[${index}]`, tag);
    });

    const response = await axios.put(`${API_URL}/event/${id}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  }

  async deleteEvent(id: number) {
    await axios.delete(`${API_URL}/event/${id}`);
  }
}

export const eventService = new EventService();
