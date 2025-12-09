import { AxiosError } from 'axios';
import type { AxiosInstance, InternalAxiosRequestConfig } from 'axios';
import { toast } from 'react-hot-toast';

export interface ApiErrorResponse {
  message?: string;
  error?: string;
  errors?: Record<string, string[]>;
}

export const setupInterceptors = (client: AxiosInstance) => {
  // Request interceptor
  client.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
      return config;
    },
    (error: AxiosError) => {
      return Promise.reject(error);
    }
  );

  // Response interceptor
  client.interceptors.response.use(
    (response) => response,
    (error: AxiosError<ApiErrorResponse>) => {
      if (error.response) {
        const { status, data } = error.response;

        switch (status) {
          case 401:
            toast.error('You are not logged in');
            break;
          case 403:
            toast.error('You do not have permission');
            break;
          case 404:
            toast.error('Resource not found');
            break;
          case 422: {
            const validationErrors = data?.errors;
            if (validationErrors) {
              Object.values(validationErrors).flat().forEach((msg) => {
                toast.error(msg);
              });
            } else {
              toast.error(data?.message || data?.error || 'Validation error');
            }
            break;
          }
          case 500:
            toast.error('Server error, please try again later');
            break;
          default:
            toast.error(data?.message || data?.error || 'Something went wrong');
        }
      } else if (error.request) {
        toast.error('No connection to the server');
      } else {
        toast.error('An unexpected error occurred');
      }

      return Promise.reject(error);
    }
  );
};